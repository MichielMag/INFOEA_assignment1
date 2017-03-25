using INFOEA.Algorithm.Crossover;
using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Procreation
{
    public abstract class AbstractProcreator<T> : IProcreator<T> where T : IGenome
    {
        protected ICrossover<T> crossover_provider;
        private string name;
        public string Name { get { return name; } }

        public AbstractProcreator(ICrossover<T> crossover, string _name)
        {
            name = _name;
            crossover_provider = crossover;
        }

        public abstract List<T> Procreate(List<T> population);
    }
}
