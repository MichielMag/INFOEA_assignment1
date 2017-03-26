using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.PopulationGeneration
{
    public abstract class AbstractPopulationGenerator<T> : IPopulationGenerator<T> where T : IGenome
    {
        private string name;

        protected Random random_source;

        public string Name { get { return name; } }

        public AbstractPopulationGenerator(Random random, string _name)
        {
            random_source = random;
            name = _name;
        }

        public abstract List<T> Generate(int population_size, int genome_size);
    }
}
