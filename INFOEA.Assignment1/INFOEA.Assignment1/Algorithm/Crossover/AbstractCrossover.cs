using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Algorithm.Crossover
{
    abstract class AbstractCrossover<T> : ICrossover<T>
    {
        protected Random random_source;

        public AbstractCrossover(Random random)
        {
            random_source = random;
        }

        public abstract Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo);

        protected int GenerateCrossoverSize()
        {

            return 0;
        }
    }
}
