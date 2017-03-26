using INFOEA.Algorithm.Algorithm;
using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.PopulationGeneration
{
    public class LocalSearchPopulationGenerator<T> : AbstractPopulationGenerator<T> where T : IGenome
    {
        LocalSearch<T> local_search;

        public LocalSearchPopulationGenerator(Random random, LocalSearch<T> _local_search) : base(random, "LS")
        {
            local_search = _local_search;
        }

        public override List<T> Generate(int population_size, int genome_size)
        {
            List<T> population = new List<T>();

            for (uint i = 0; i < population_size; ++i)
            {
                T g = (T)Activator.CreateInstance(typeof(T), genome_size);
                g.Generate(ref random_source);
                g = local_search.Search(g);
                population.Add(g);
            }

            return population;
        }
    }
}
