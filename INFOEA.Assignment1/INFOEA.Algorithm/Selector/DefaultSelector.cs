using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Selector
{
    public class DefaultSelector<T> : ISelector<T> where T : IGenome
    {
        private IComparer<T> comparer;

        public DefaultSelector(IComparer<T> _comparer)
        {
            comparer = _comparer;
        }

        public IEnumerable<T> DoSelection(IEnumerable<T> population, int population_size)
        {
            return population.OrderByDescending(x => x, comparer).Take(population_size);
        }
    }
}