using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.PopulationGeneration
{
    public interface IPopulationGenerator<T> where T:IGenome
    {
        List<T> Generate(int population_size, int genome_size);
    }
}
