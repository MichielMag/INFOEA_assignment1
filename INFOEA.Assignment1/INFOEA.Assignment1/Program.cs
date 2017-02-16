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
            int seed = -1;
            if(args.Count() > 0)
            {
                foreach(string arg in args)
                {
                    try
                    {
                        if (arg.Contains("-seed=")) // Seed?
                            seed = int.Parse(arg.Split('=')[1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Could not parse commandline arguments.");
                    }
                }
            }

            Random random;
            if(seed < 0)
            {
                random = new Random();
                seed = random.Next();
            }
            random = new Random(seed);

            Console.WriteLine("Initialized program with seed: {0}", seed);

            int string_length = 100;

            GeneticAlgorithm<UCOGenome> experiment_one_one = 
                new GeneticAlgorithm<UCOGenome>(250, 
                                                string_length, 
                                                new TwoPointCrossover<UCOGenome>(random), 
                                                new Goal(100, 100),
                                                random);

            GeneticAlgorithm<LCOGenome> experiment_two_one = 
                new GeneticAlgorithm<LCOGenome>(500, 
                                                string_length, 
                                                new TwoPointCrossover<LCOGenome>(random), 
                                                new Goal(100, Program.linear_score(string_length)),
                                                random);

            GeneticAlgorithm<DTTGenome> experiment_three_one = 
                new GeneticAlgorithm<DTTGenome>(500, 
                                                string_length, 
                                                new TwoPointCrossover<DTTGenome>(random), 
                                                new Goal(100, 100),
                                                random);
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
