using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Genome;

namespace INFOEA.Assignment1.Algorithm.Crossover
{
    class UniformCrossover<T> : AbstractCrossover<T> where T:IGenome
    {
        public UniformCrossover(Random random) : base(random)
        {
        }

        public override Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo)
        {
            int data_size = ParentOne.DataSize;

            // Boolean array to keep track of indexes to swap.
            // A lot faster than keeping track in a list that has to be walked through each time :)
            bool[] swap_indexes = new bool[data_size];
            
            // What should the size maximally be? ALso, this should be generated with a distribution
            // That makes sure lower numbers have a higher probabiity, i.e. lognormal?
            int swap_amount = random_source.Next(1, data_size / 4);

            for(int i = 0; i < swap_amount; ++i)
            {
                int index = random_source.Next(0, data_size);

                // Already in swap indexes, so reverting i to go again.
                if (swap_indexes[index])
                    i--;
                else
                    swap_indexes[index] = true;
            }


            string child_one_data = "";
            string child_two_data = "";

            for(int i = 0; i < data_size; ++i)
            {
                if (swap_indexes[i])
                {
                    child_one_data += ParentTwo.Data[i];
                    child_two_data += ParentOne.Data[i];
                }
                else
                {
                    child_one_data += ParentOne.Data[i];
                    child_two_data += ParentTwo.Data[i];
                }
            }
            // TODO: Implementation :

            T child_one = (T)Activator.CreateInstance(typeof(T), child_one_data);
            T child_two = (T)Activator.CreateInstance(typeof(T), child_two_data);

            return new Tuple<T, T>(child_one, child_two);
        }
    }
}
