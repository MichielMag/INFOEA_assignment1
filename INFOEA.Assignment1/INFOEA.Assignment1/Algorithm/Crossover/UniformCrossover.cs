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
            // TODO: Implementation :)

            T child_one = (T)Activator.CreateInstance(typeof(T), "");
            T child_two = (T)Activator.CreateInstance(typeof(T), "");

            return new Tuple<T, T>(child_one, child_two);
        }
    }
}
