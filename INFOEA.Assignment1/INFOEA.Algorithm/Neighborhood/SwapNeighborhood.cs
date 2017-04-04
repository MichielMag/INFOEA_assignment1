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
        public SwapNeighborhood(Random random) : base(random, "SWAP")
        {
        }
        
        // Work with IEnumerable and yield for lazy evaluation :)
        public override IEnumerable<T> Neighbors(T solution)
        {
            int n = solution.DataSize;
            int amount_of_neighbors = (n * n) / 4;

            for(int i = 0; i < amount_of_neighbors; ++i)
            {
                int vertex_one = random_source.Next(1, n + 1);
                int vertex_two = random_source.Next(1, n + 1);

                char one = solution.Data[vertex_one - 1];

                // Should not take that long. 50/50 chance.
                while (one == solution.Data[vertex_two - 1])
                    vertex_two = random_source.Next(1, n + 1);

                char two = solution.Data[vertex_two - 1];

                float fitness = solution.Fitness;

                foreach (int other in GraphGenome.vertices[vertex_one].Connections)
                {
                    if (other == vertex_two) continue;
                    // == two, so the new one...
                    if (solution.Data[other - 1] == two)
                        fitness--;
                    else
                        fitness++;
                }
                foreach(int other in GraphGenome.vertices[vertex_two].Connections)
                {
                    // Already calculated
                    if (other == vertex_one)
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

                    data[vertex_one - 1] = two;
                    data[vertex_two - 1] = one;

                    yield return (T) Activator.CreateInstance(typeof(T), new string(data), fitness);
                }
                continue;
            }
        }
    }
}
