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
            int string_length = 100;
            GeneticAlgorithm<UCOGenome> experiment_one_one = new GeneticAlgorithm<UCOGenome>(250, string_length, new TwoPointCrossover<UCOGenome>(), new Goal(100, 100));
            GeneticAlgorithm<LCOGenome> experiment_two_one = new GeneticAlgorithm<LCOGenome>(500, string_length, new TwoPointCrossover<LCOGenome>(), new Goal(100, Program.linear_score(string_length)));

            GeneticAlgorithm<DTTGenome> experiment_three_one = new GeneticAlgorithm<DTTGenome>(500, string_length, new TwoPointCrossover<DTTGenome>(), new Goal(100, 100));
            experiment_three_one.start();
        }

        static int linear_score(int length)
        {
            int score = 0;
            for(int i = 1; i < length + 1; ++i)
            {
                score += i;
            }
            return score;
        }
    }
}
