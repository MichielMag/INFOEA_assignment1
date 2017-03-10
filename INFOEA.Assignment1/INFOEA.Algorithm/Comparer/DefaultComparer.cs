using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Comparer
{
    public class DefaultComparer<T> : IComparer<T> where T : IGenome
    {
        public int Compare(T x, T y)
        {
            return x.Fitness.CompareTo(y.Fitness);
        }
    }
}
