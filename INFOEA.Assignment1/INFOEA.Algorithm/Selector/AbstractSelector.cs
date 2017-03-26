using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Selector
{
    public abstract class AbstractSelector<T> : ISelector<T> where T:IGenome
    {
        protected IComparer<T> comparer;

        public AbstractSelector(IComparer<T> _comparer)
        {
            comparer = _comparer;
        }

        public abstract List<T> DoSelection(List<T> population, int population_size);
    }
}
