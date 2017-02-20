using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Algorithm.Crossover;
using INFOEA.Assignment1.Genome;
using System.Threading;
using INFOEA.Assignment1.Results;

namespace INFOEA.Assignment1.Algorithm
{
    class GeneticAlgorithm<T> where T:IGenome
    {
        private int genome_size;
        private int current_generation;

        private int seed;

        private ICrossover<T> crossover_provider;
        private List<T> population;
        private Random random;
        private Goal goal;
        
        public GeneticAlgorithm(int _genome_size, ICrossover<T> _crossover_provider, Goal _goal, Random _random)
        {
            crossover_provider = _crossover_provider;
            goal = _goal;
            genome_size = _genome_size;

            random = _random;
        }
        

        // TODO: Write results to InnerResult.
        public InnerResult start(int population_size, bool silent = false)
        {
            current_generation = 0;
            population = new List<T>();

            Console.WriteLine("Going to run algorithm. Max generations: {0}, Min fitness: {1}", goal.MaxGenerations, goal.MinFitness);
            generatePopulation(population_size);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            do
            {
                shufflePopulation();
                procreatePopulation(population_size);
                sortPopulation();
                selectPopulation(population_size);

                if(!silent)
                    printPopulation();

                current_generation++;
            } while (!goal.AchievedGoal(current_generation, population[0].Fitness));

            stopwatch.Stop();

            Console.WriteLine(results());
            //Console.ReadLine();

            InnerResult res = new InnerResult();
            res.FirstHitGeneration = current_generation;
            res.Success = goal.AchievedFitnessGoal(population[0].Fitness);

            // TODO:
            res.ConvergenceGeneration = 0;
            res.CPUTime = stopwatch.ElapsedTicks;
            res.FunctionEvaluations = 0;

            return res;
        }

        private void generatePopulation(int population_size)
        {
            population = new List<T>();

            for(uint i = 0; i < population_size; ++i)
            {
                T g = (T)Activator.CreateInstance(typeof(T), genome_size);
                g.Generate(ref random);
                population.Add(g);
            }
        }

        private void shufflePopulation()
        {
            population = population.OrderBy(genome => random.Next()).ToList();
        }

        private void procreatePopulation(int population_size)
        {

            for(int i = 0; i < population_size; i+=2)
            {
                Tuple<T, T> children = crossover_provider.DoCrossover(population[i], population[i + 1]);
                population.Add(children.Item1);
                population.Add(children.Item2);
            }
        }

        private void sortPopulation()
        {
            population = population.OrderByDescending(x=>x.Fitness).ToList();
        }

        private void selectPopulation(int population_size)
        {
            population = population.Take(population_size).ToList();
        }

        private void printPopulation()
        {
            Console.WriteLine("Current generation: " + current_generation);
            for(int i = 0; i < 5; ++i)
            {
                Console.WriteLine(population[i]);
            }            
        }

        private string results()
        {
            return String.Format("====================Results:====================\nSeed: {0}\nGenerations: {1}\nBest: {2}\n================================================",
                seed, current_generation, population[0]);
        }
    }
}
