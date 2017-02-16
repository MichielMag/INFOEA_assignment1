using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Algorithm;
using INFOEA.Assignment1.Genome;
using INFOEA.Assignment1.Algorithm.Crossover;

namespace INFOEA.Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneticAlgorithm<UCOGenome> experiment_one = new GeneticAlgorithm<UCOGenome>(250, 100, new TwoPointCrossover<UCOGenome>(), new Goal(100, 100));
            experiment_one.start();
        }
    }
}
