using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Selector
{
    public interface ISelector<T> where T:IGenome
    {
        IEnumerable<T> DoSelection(IEnumerable<T> population, int population_size);
    }
}
