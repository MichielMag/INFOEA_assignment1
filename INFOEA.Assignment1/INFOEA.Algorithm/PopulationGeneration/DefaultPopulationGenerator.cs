using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.PopulationGeneration
{
    public class DefaultPopulationGenerator<T> : AbstractPopulationGenerator<T> where T : IGenome
    {
        public DefaultPopulationGenerator(Random random) : base(random, "Default")
        {
        }

        public override List<T> Generate(int population_size, int genome_size)
        {
            List<T> population = new List<T>();

            for (uint i = 0; i < population_size; ++i)
            {
                T g = (T)Activator.CreateInstance(typeof(T), genome_size);
                g.Generate(ref random_source);
                population.Add(g);
            }

            return population;
        }
    }
}
