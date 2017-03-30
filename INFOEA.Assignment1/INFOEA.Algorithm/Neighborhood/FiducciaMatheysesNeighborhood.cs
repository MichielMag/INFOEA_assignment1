using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using INFOEA.Algorithm.Neighborhood.Fiduccia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Neighborhood
{
    /// <summary>
    /// Fiduccia-Mattheyses is een algoritme om een lokale zoekactie uit te voeren.
    /// 
    /// Deze methode wordt gebruikt bij het graaf-bipartioneringsprobleem(zie Grafentheorie). 
    /// Hierbij is het doel het de twee kleuren van de knopen van de graaf zo aan te passen zodat er 
    /// zo min mogelijk knopen van verschillende kleur met elkaar verbonden zijn.Het aantal knopen dat 
    /// met elkaar verbonden is en een andere kleur bezit noemt met de knipgrootte.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FiducciaMatheysesNeighborhood<T> : AbstractNeighborhood<T> where T:IGenome
    {
        
        private int degree;

        public FiducciaMatheysesNeighborhood(Random random, int _degree) : base(random, "FM")
        {
            degree = _degree;
        }


        /// <summary>
        /// ==============================
        /// === Beschrijing wikipedia: ===
        /// ==============================
        /// Het algoritme begint met een gebalanceerde graaf (evenveel 'witte' en 'zwarte' knooppunten)
        /// 1. Wissel de zwarte knoop van kleur die de knipgrootte het meest verkleint
        /// 2. Wissel de witte knoop van kleur die de knipgrootte het meest verkleint
        /// 3. Fixeer beide knopen
        /// 4. Herhaal stap 1 t/m 3 totdat er geen vrije knopen meer zijn.
        /// 5 Ga nu terug naar de situatie waarbij de knipgrootte minimaal is.
        /// 6. Herhaal stap 1 t/m 6 totdat deze geen verbetering meer opleveren.
        /// 7. Nu is er een lokaal optimum gevonden.
        /// 
        /// ============================ 
        /// === Beschrijving sheets: ===
        /// ============================
        /// 1. Start from a partitioning (A,B) of the graph (V,E) 
        /// 2. Compute for each vertex v the gain Wv obtained by moving the vertex to the other subset 
        /// 3. Create 2 arrays A and B with boundaries [- MaxDegree, + MaxDegree]. 
        ///     Array A (resp. B) stores at position i a list of all vertices in subset A (resp. B) with gain Wv = i. 
        /// 4. Both arrays have an associated pointer that keeps track of the index with maximal value k 
        /// 5. Initially all vertices of the graph are marked free.
        //  6. If |A| > |B|(resp. |A| < |B|) then move the vertex v from A (resp. B) that has the highest gain Wv to the subset B (resp. A). 
        ///     Mark the vertex v ﬁxed. Fixed vertices are removed from the arrays A and B. 
        ///     Update the positions in the arrays A and B of the free nodes that are connected to the moved vertex. 
        /// 7. Continue moving vertices until there are no free nodes left. The resulting partitioning is the same as the one we started with. 
        /// 8. FM keeps track of all valid partitionings during the search process and returns the one with the lowest cut size. 
        /// 9. Repeat the FM procedure until no further improvement is found.
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        /// 

        /*
         * ----------source:
         * https://pdfs.semanticscholar.org/a487/b518d172c43471c8d3236a1855d9b36a2a6a.pdf
         * ------------------
         * 
        The Fiduccia-Mattheyses (FM) heuristic for bipartitioning circuit hyper- graphs [20] 
        is an iterative improvement algorithm. Its neighborhood structure is induced by single-vertex, partition-to-partition moves. 
        FM starts with a possibly random solution and changes the solution by a sequence of moves which are organized as passes.
        At the beginning of a pass, all ver- tices are free to move (unlocked), and each possible move is labeled with the immediate 
        change in total cost  it would cause;  this is called  the  gain of the move (positive gains reduce solution cost, while 
        negative gains in- crease it). Iteratively, a move with highest gain is selected and executed, and the moving vertex is locked,
        i.e., is not allowed to move again during that pass. Since moving a vertex can change gains of adjacent vertices, after a move
        is executed all a ected gains are updated. Selection and execution of a best-gain move, followed by gain update, are repeated
        until every vertex is locked. Then, the best solution seen during the pass is adopted as the starting solution of the next pass. 
        The algorithm terminates when a pass fails to improve solution quality.
        The FM algorithm can be easily seen to have three main operations: (1) the
        computation of initial gain values at the beginning of a pass; (2) the retrieval of the best-gain (feasible) move; 
        and (3) the update of all a ected gain values after a move is made. The contribution of Fiduccia and Mattheyses lies 
        in observing that circuit hypergraphs are sparse, so that any move gain is bounded between two and negative two times the
        maximal vertex degree in the hypergraph (times the maximal edge weight, if edge weights are used). This allows hashing of 
        moves by their gains: all a ected gains can be updated in linear time, yielding overall linear complexity per pass. In [20], 
        all moves with the same gain are stored in a linked list representing a \gain bucket".

         */

        public override IEnumerable<T> Neighbors(T solution)
        {
            // Lijst A en B. A bevat alle scores van verplaatsen van 0 -> 1
            // B bevat alle scores van verplaatsen 1 -> 0 
            FiducciaBucket bucketA = new FiducciaBucket(degree);
            FiducciaBucket bucketB = new FiducciaBucket(degree);

            float fitness = solution.Fitness;
            int n = solution.DataSize;
            char[] data = solution.Data.ToArray();

            // Nu alle vertices af gaan, en (plus of min) gain berekenen
            // en in betreffende lijst plaatsen.
            for(int i = 0; i < n; ++i)
            {
                int id = i + 1;
                char value = data[i];
                int gain = 0;
                int[] neighbors = GraphGenome.vertices[id].Connections;

                foreach(int neighbor in neighbors)
                {
                    if (data[neighbor - 1] == value)
                        gain--;
                    else
                        gain++;
                }

                // Als die tussen onze degree zit
                if (gain >= -1 * degree && gain <= degree)
                {
                    if (value == '0')
                        bucketA.AddSwap(gain, id, '1');
                    else
                        bucketB.AddSwap(gain, id, '0');
                }
            }

            while(true)
            {
                VertexSwap swapA = bucketA.GetNext();
                VertexSwap swapB = bucketB.GetNext();

                // Als we de graph niet meer gebalanceerd kunnen houden...
                if (swapA == null || swapB == null)
                    break;

                // Eerst voor A
                data[swapA.id - 1] = swapA.swap_to; // data swap doen
                fitness += swapA.gain;              // fitness aanpassen

                // Nu moeten we de positie (gain) van al zijn buren updaten...
                // gelukkig weten we wat voor gain deze buren zullen hebben :)
                int[] neighbors = GraphGenome.vertices[swapA.id].Connections;

                // Alle gains van alle neighbours updaten
                foreach (int neighbor in neighbors)
                {
                    char n_value = data[neighbor - 1];
                    try
                    {
                        if (n_value == '0')
                            bucketA.EditSwap(neighbor, swapA.swap_to);
                        else
                            bucketB.EditSwap(neighbor, swapA.swap_to);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        // Kon niet toevoegen, maar laten we dat maar gewoon negeren :)
                    }
                }

                // Zelfde riedeltje voor B
                data[swapB.id - 1] = swapB.swap_to; // data swap doen
                fitness += swapB.gain;              // fitness aanpassen

                // Nu moeten we de positie (gain) van al zijn buren updaten...
                // gelukkig weten we wat voor gain deze buren zullen hebben :)
                neighbors = GraphGenome.vertices[swapB.id].Connections;

                foreach (int neighbor in neighbors)
                {
                    char n_value = data[neighbor - 1];
                    try
                    {
                        if (n_value == '0')
                            bucketA.EditSwap(neighbor, swapB.swap_to);
                        else
                            bucketB.EditSwap(neighbor, swapB.swap_to);
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        // Kon niet toevoegen, maar laten we dat maar gewoon negeren :)
                    }
                }
            }

            // Maar wat moeten we nou returnen? Beschrijvingen lijken erop te wijzen dat we het meerdere keren moeten uitvoeren...
            // Maar wanneer? Hoevaak? Dan kunnen we dat gebruiken om een yield return te doen. Voor nu even deze.
            yield return (T)Activator.CreateInstance(typeof(T), new string(data));
        }
    }
}
