using INFOEA.Assignment1.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Algorithm.Comparer
{
    public class DefaultComparer<T> : IComparer<T> where T : IGenome
    {
        public int Compare(T x, T y)
        {
            return x.Fitness.CompareTo(y.Fitness);
        }
    }
}
