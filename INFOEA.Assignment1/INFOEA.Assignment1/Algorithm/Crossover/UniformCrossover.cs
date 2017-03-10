using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Genome;

namespace INFOEA.Assignment1.Algorithm.Crossover
{
    public class UniformCrossover<T> : AbstractCrossover<T> where T:IGenome
    {
        private const double flipping_chance = 0.5;
        public UniformCrossover(Random random) : base(random, "UX")
        {
        }

        public override Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo)
        {
            int data_size = ParentOne.DataSize;

            string child_one_data = "";
            string child_two_data = "";

            for (int i = 0; i < data_size; i++)
            {
                if(random_source.NextDouble() <= flipping_chance)
                {
                    child_one_data += ParentTwo.Data[i];
                    child_two_data += ParentTwo.Data[i];
                }
                else
                {
                    child_one_data += ParentOne.Data[i];
                    child_two_data += ParentTwo.Data[i];
                }
            }

            T child_one = (T)Activator.CreateInstance(typeof(T), child_one_data);
            T child_two = (T)Activator.CreateInstance(typeof(T), child_two_data);

            return new Tuple<T, T>(child_one, child_two);
        }
    }
}
