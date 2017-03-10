using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Algorithm.Crossover
{
    public abstract class AbstractCrossover<T> : ICrossover<T>
    {
        private string name;

        protected Random random_source;

        public string Name { get { return name; } }

        public AbstractCrossover(Random random, string _name)
        {
            random_source = random;
            name = _name;
        }

        public abstract Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo);
    }
}
