using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Algorithm.Crossover;
using INFOEA.Algorithm.Algorithm;

namespace INFOEA.Algorithm.Procreation
{
    public class LocalSearchProcreator<T> : AbstractProcreator<T> where T : IGenome
    {
        private Random random;
        private LocalSearch<T> local_search;

        public LocalSearchProcreator(ICrossover<T> crossover, LocalSearch<T> _local_search, Random _random) : base(crossover, "LS")
        {
            random = _random;
            local_search = _local_search;
        }

        public override List<T> Procreate(List<T> population)
        {
            int parent_one = random.Next(population.Count);
            int parent_two = parent_one;

            // Even zeker weten dat we niet 2 dezelfde hebben
            do {
                parent_two = random.Next(population.Count);
            } while (parent_two == parent_one);

            // We verwachten maar 1 kind.
            T child = crossover_provider.DoCrossover(population[parent_one], population[parent_two]).Item1;
            child = local_search.Search(child);
            if(!child.Data.Equals(population[parent_one].Data) && !child.Data.Equals(population[parent_two].Data))
                population.Add(child);
            return population;
        }
    }
}
