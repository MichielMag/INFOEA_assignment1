using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Neighborhood
{
    public class SwapNeighborhood<T> : AbstractNeighborhood<T> where T:IGenome
    {
        public SwapNeighborhood(Random random) : base(random, "Swap")
        {
        }
        
        // Work with IEnumerable and yield for lazy evaluation :)
        public override IEnumerable<T> Neighbors(T solution)
        {
            int n = solution.DataSize;
            int amount_of_neighbors = (n * n) / 4;

            for(int i = 0; i < amount_of_neighbors; ++i)
            {
                int vertice_one = random_source.Next(1, n + 1);
                int vertice_two = random_source.Next(1, n + 1);

                char one = solution.Data[vertice_one - 1];

                // Should not take that long. 50/50 chance.
                while (one == solution.Data[vertice_two - 1])
                    vertice_two = random_source.Next(1, n + 1);

                char two = solution.Data[vertice_two - 1];

                float fitness = solution.Fitness;

                foreach (int other in GraphGenome.vertices[vertice_one].Connections)
                {
                    // == two, so the new one...
                    if (solution.Data[other - 1] == two)
                        fitness--;
                    else
                        fitness++;
                }

                foreach(int other in GraphGenome.vertices[vertice_two].Connections)
                {
                    // Already calculated
                    if (other == vertice_one)
                        continue;

                    // == one, so the new two...
                    if (solution.Data[other - 1] == one)
                        fitness--;
                    else
                        fitness++;
                }

                if (fitness <= solution.Fitness)
                {
                    char[] data = solution.Data.ToCharArray();

                    data[vertice_one - 1] = two;
                    data[vertice_two - 1] = one;

                    // Eigenlijk wil ik de statement hieronder returnen, maar deze functie hierboven rekent blijkbaar de fitness niet goed uit.
                    yield return (T)Activator.CreateInstance(typeof(T), new string(data), solution, new int[2] { vertice_one, vertice_two }); 
                    //yield return (T)Activator.CreateInstance(typeof(T), new string(data), fitness);
                }
                continue;
            }

            /*for(int i = 0; i < amount_of_neighbors; ++i)
            {
                int pos_one = random_source.Next(n);
                int pos_two = random_source.Next(n);

                // Should not take that long. 50/50 chance.
                while (solution.Data[pos_one] == solution.Data[pos_two])
                    pos_two = random_source.Next(n);

                float fitness = solution.Fitness;

                char c = solution.Data[pos_two];
                foreach (int other in GraphGenome.vertices[pos_one+1].Connections)
                {
                    // Already plussed the score for this connection.
                    if (solution.Data[other - 1] == c)
                        fitness--;
                    else
                        fitness++;
                }

                c = solution.Data[pos_one];
                foreach (int other in GraphGenome.vertices[pos_two+1].Connections)
                {
                    if (other == pos_one + 1)
                        continue;

                    // Already plussed the score for this connection.
                    if (solution.Data[other - 1] == c)
                        fitness--;
                    else
                        fitness++;
                }

                if(fitness <= solution.Fitness)
                {
                    char[] data = solution.Data.ToCharArray();
                    char old_one = data[pos_one];
                    data[pos_one] = data[pos_two];
                    data[pos_two] = old_one;

                    yield return (T)Activator.CreateInstance(typeof(T), new string(data), fitness); //solution, new int[2] { pos_one + 1, pos_two + 1 }); // PLus 1 omdat het om ID's gaat ipv posities in string...
                }
                continue;
            }*/
        }
    }
}
