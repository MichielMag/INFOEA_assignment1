using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Genome;

namespace INFOEA.Assignment1.Algorithm.Crossover
{
    class UniformCrossover<T> : AbstractCrossover<T>
    {
        public UniformCrossover(Random random) : base(random)
        {
        }

        public override Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo)
        {
            throw new NotImplementedException();
        }
    }
}
