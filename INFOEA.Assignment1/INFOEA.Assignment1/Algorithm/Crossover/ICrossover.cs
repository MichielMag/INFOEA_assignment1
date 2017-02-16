using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Algorithm.Crossover
{
    interface ICrossover<T>
    {
        Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo, ref Random random);
    }
}
