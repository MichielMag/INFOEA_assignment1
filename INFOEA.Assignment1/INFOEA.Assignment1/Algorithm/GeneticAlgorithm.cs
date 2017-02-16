using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Algorithm.Crossover;
using INFOEA.Assignment1.Genome;
using System.Threading;

namespace INFOEA.Assignment1.Algorithm
{
    class GeneticAlgorithm<T> where T:IGenome
    {
        private int population_size;
        private int genome_size;
        private int current_generation;

        private int seed;

        private ICrossover<T> crossover_provider;
        private List<T> population;
        private Random random;
        private Goal goal;

        private bool stop_criteria = false;

        public GeneticAlgorithm(int _population_size, int _genome_size, ICrossover<T> _crossover_provider, Goal _goal, int _seed = -1)
        {
            population_size = _population_size;
            crossover_provider = _crossover_provider;
            goal = _goal;
            genome_size = _genome_size;
            if (_seed > 0)
            {
                seed = _seed;
                random = new Random(seed);
            }
            else
            {
                random = new Random();
                seed = random.Next();
                random = new Random(seed);
            }
        }

        public void start()
        {
            generatePopulation();

            while (!goal.AchievedGoal(current_generation, population[0].Fitness))
            {
                shufflePopulation();
                procreatePopulation();
                sortPopulation();
                selectPopulation();
                printPopulation();

                current_generation++;
            }

            Console.WriteLine(results());
            Console.ReadLine();
        }

        private void generatePopulation()
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

        private void procreatePopulation()
        {

            for(int i = 0; i < population_size; i+=2)
            {
                Tuple<T, T> children = crossover_provider.DoCrossover(population[i], population[i + 1], ref random);
                population.Add(children.Item1);
                population.Add(children.Item2);
            }
        }

        private void sortPopulation()
        {
            population = population.OrderByDescending(x=>x.Fitness).ToList();
        }

        private void selectPopulation()
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
