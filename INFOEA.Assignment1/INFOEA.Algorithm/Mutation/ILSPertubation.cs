using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Mutation
{
    public class ILSPertubation<T> : AbstractMutation<T> where T : IGenome
    {
        // Percentages van de datasize. Dus van string van 100, 
        // verwachten we een max van 50 mutaties, minimaal 20
        private const double min_amount_of_pertubations = 0.2;
        private const double max_amount_of_pertubations = 0.5;

        public ILSPertubation(Random random) : base(random, "ILSP")
        {
        }

        public override T Mutate(T solution)
        {
            int data_size = solution.DataSize;
            int amount = random_source.Next(
                (int)(min_amount_of_pertubations * data_size), 
                (int)(max_amount_of_pertubations * data_size));

            char[] data = solution.Data.ToCharArray();
            List<int> positions = new List<int>();

            for (int i = 0; i < amount; ++i)
            {
                int vertex_one = random_source.Next(1, data_size + 1);
                int vertex_two = random_source.Next(1, data_size + 1);

                char one = solution.Data[vertex_one - 1];

                // Should not take that long. 50/50 chance.
                while (one == solution.Data[vertex_one - 1])
                    vertex_one = random_source.Next(1, data_size + 1);

                char two = solution.Data[vertex_two - 1];

                if (positions.Contains(vertex_one) || positions.Contains(vertex_two))
                {
                    i--;
                    continue;
                }

                data[vertex_one - 1] = two;
                data[vertex_two - 1] = one;

                positions.Add(vertex_one);
                positions.Add(vertex_two);

            }

            return (T)Activator.CreateInstance(typeof(T), new string(data));//, solution, positions.ToArray());
        }
    }
}
