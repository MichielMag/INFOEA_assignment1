using INFOEA.Algorithm.Crossover;
using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Procreation
{
    public class DefaultProcreator<T> : AbstractProcreator<T> where T : IGenome
    {
        public DefaultProcreator(ICrossover<T> crossover) : base(crossover, "Default")
        {
        }

        public override List<T> Procreate(List<T> population)
        {
            int n = population.Count;
            for (int i = 0; i < n; i += 2)
            {
                Tuple<T, T> children = crossover_provider.DoCrossover(population[i], population[i+1]);
                population.Add(children.Item1);
                population.Add(children.Item2);
            }
            return population;
        }
    }
}
