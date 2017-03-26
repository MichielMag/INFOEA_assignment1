using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Selector
{
    public class DefaultSelector<T> : AbstractSelector<T> where T : IGenome
    {
        public DefaultSelector(IComparer<T> _comparer) : base(_comparer)
        {
        }

        public override List<T> DoSelection(List<T> population, int population_size)
        {
            return population.OrderByDescending(x => x, comparer).Take(population_size).ToList();
        }
    }
}