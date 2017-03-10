using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Neighborhood
{
    public abstract class AbstractNeighborhood<T> : INeighborhood<T> where T:IGenome
    {
        private string name;

        protected Random random_source;

        public string Name { get { return name; } }

        public AbstractNeighborhood(Random random, string _name)
        {
            random_source = random;
            name = _name;
        }

        public abstract IEnumerable<T> Neighbors(T solution);
    }
}
