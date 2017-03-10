using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Algorithm.Genome;

namespace INFOEA.Algorithm.Crossover
{
    public class TwoPointCrossover<T> : AbstractCrossover<T> where T:IGenome
    {
        public TwoPointCrossover(Random random) : base(random, "2X")
        {
        }

        public override Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo)
        {
            int data_size = ParentOne.DataSize;
            int index_one = random_source.Next(data_size);
            int index_two = random_source.Next(index_one, data_size);

            string data_child_one = ParentOne.Data.Substring(0, index_one) +
                                    ParentTwo.Data.Substring(index_one, index_two - index_one) +
                                    ParentOne.Data.Substring(index_two, data_size - index_two);

            string data_child_two = ParentTwo.Data.Substring(0, index_one) +
                                    ParentOne.Data.Substring(index_one, index_two - index_one) +
                                    ParentTwo.Data.Substring(index_two, data_size - index_two);

            T child_one = (T)Activator.CreateInstance(typeof(T), data_child_one);
            T child_two = (T)Activator.CreateInstance(typeof(T), data_child_two);

            return new Tuple<T, T>(child_one, child_two);
        }
    }
}
