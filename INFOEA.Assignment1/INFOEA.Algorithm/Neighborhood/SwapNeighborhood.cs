using INFOEA.Algorithm.Genome;
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
                int pos_one = random_source.Next(n);
                int pos_two = random_source.Next(n);

                char[] data = solution.Data.ToCharArray();

                // Should not take that long. 50/50 chance.
                while (data[pos_one] == data[pos_two])
                    pos_two = random_source.Next(n);
                
                char old_one = data[pos_one];
                data[pos_one] = data[pos_two];
                data[pos_two] = old_one;

                yield return (T)Activator.CreateInstance(typeof(T), new string(data), solution, new int[2] { pos_one + 1, pos_two + 1}); // PLus 1 omdat het om ID's gaat ipv posities in string...
            }
        }
    }
}
