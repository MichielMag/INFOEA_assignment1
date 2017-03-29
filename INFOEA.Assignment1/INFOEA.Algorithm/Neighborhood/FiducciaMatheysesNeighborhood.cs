using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
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
        private class VertexSwap
        {
            public VertexSwap(int _id, char _swap_to)
            {
                id = _id;
                swap_to = _swap_to;
            }
            public char swap_to;
            public int id;
            public bool free = true;
        }
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
        /// 6. If |A| > |B|(resp. |A| < |B|) then move the vertex v from A (resp. B) that has the highest gain Wv to the subset B (resp. A). 
        ///     Mark the vertex v ﬁxed. Fixed vertices are removed from the arrays A and B. 
        ///     Update the positions in the arrays A and B of the free nodes that are connected to the moved vertex. 
        /// 7. Continue moving vertices until there are no free nodes left. The resulting partitioning is the same as the one we started with. 
        /// 8. FM keeps track of all valid partitionings during the search process and returns the one with the lowest cut size. 
        /// 9. Repeat the FM procedure until no further improvement is found.
        /// </summary>
        /// <param name="solution"></param>
        /// <returns></returns>
        public override IEnumerable<T> Neighbors(T solution)
        {
            // Lijst A en B. A bevat alle scores van verplaatsen van 0 -> 1
            // B bevat alle scores van verplaatsen 1 -> 0 
            Dictionary<int, List<VertexSwap>> A = new Dictionary<int, List<VertexSwap>>(), 
                                              B = new Dictionary<int, List<VertexSwap>>();

            // We moeten ook de positie bijhouden van de swap zodat we deze later makkelijk kunnen verplaatsen
            Dictionary<int, int> positions = new Dictionary<int, int>();

            // Lijsten vullen van [-degree, +degree]
            for(int i = -1 * degree; i < degree; ++i)
            {
                A.Add(i, new List<VertexSwap>());
                B.Add(i, new List<VertexSwap>());
            }

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
                int[] neighbors = GraphGenome.vertices[i + 1].Connections;

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
                    VertexSwap swap;
                    if (value == '0')
                    {
                        swap = new VertexSwap(id, '1');
                        A[gain].Add(swap);
                    }
                    else
                    {
                        swap = new VertexSwap(id, '0');
                        B[gain].Add(swap);
                    }

                    // Eerst in goede array, daarna in de positions.
                    positions.Add(id, gain);
                }
            }

            // Dus, welke array gaan we vanuit verplaatsen? (Welke lijst heeft de meeste items?)
            Dictionary<int, List<VertexSwap>> replacement_list = A.Sum(x => x.Value.Count) > B.Sum(x => x.Value.Count) ? A : B;
            Dictionary<int, List<VertexSwap>> other = replacement_list == A ? B : A;

            foreach (KeyValuePair<int, List<VertexSwap>> kvp in replacement_list)
            {
                // Van beste gain (oftewel, grootste min getal) naar slechtste (grootste plus getal)
                // (want zo is deze dictionary hierboven geinitialiseerd)
                for(int i = 0; i < kvp.Value.Count; ++i)
                {
                    VertexSwap vs = kvp.Value[i];
                    if(vs.free)
                    {
                        data[vs.id - 1] = vs.swap_to; // data swap doen
                        fitness += kvp.Key;           // fitness aanpassen
                        vs.free = false;              // swap is niet meer free maar fixed

                        // Nu moeten we de positie (gain) van al zijn buren updaten...
                        // gelukkig weten we wat voor gain deze buren zullen hebben :)
                        int[] neighbors = GraphGenome.vertices[vs.id].Connections;

                        foreach (int neighbor in neighbors)
                        {
                            if(positions.ContainsKey(neighbor))
                            {
                                int gain = positions[neighbor];
                                int new_gain = gain;

                                // Het zoeken naar zo'n vertexswap is veel te costly. Hoewel het vrij kleine lijsten
                                // zullen zijn...
                                if (data[neighbor - 1] == vs.swap_to)
                                {
                                    VertexSwap swap = other[gain].FirstOrDefault(x => x != null && x.id == neighbor);

                                    if (swap == null)
                                        continue;

                                    other[gain].Remove(swap);
                                    if (gain - 1 >= -1 * degree)
                                        other[gain - 1].Add(swap);
                                    new_gain--;
                                }
                                else
                                {
                                    VertexSwap swap = replacement_list[gain].FirstOrDefault(x => x != null && x.id == neighbor);

                                    if (swap == null)
                                        continue; 

                                    replacement_list[gain].Remove(swap);
                                    if (gain + 1 <=  degree)
                                        other[gain + 1].Add(swap);

                                    new_gain++;
                                }
                                positions[neighbor] = new_gain;
                            }
                        }
                    }
                }
            }

            // Maar wat moeten we nou returnen? Beschrijvingen lijken erop te wijzen dat we het meerdere keren moeten uitvoeren...
            // Maar wanneer? Hoevaak? Dan kunnen we dat gebruiken om een yield return te doen. Voor nu even deze.
            yield return (T)Activator.CreateInstance(typeof(T), new string(data), fitness);
        }
    }
}
