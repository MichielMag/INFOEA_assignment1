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
    class GeneticAlgorithm<T> where T : IGenome
    {
        private int genome_size;
        private int current_generation;

        //private int seed;

        private T best_result;

        private ICrossover<T> crossover_provider;
        private List<T> population;
        
        private T previous_last;

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
            int last_index = population_size - 1;
            int convergence_hit = -1;
            current_generation = 0;
            int first_goal_hit = -1;
            population = new List<T>();

            //Console.WriteLine("Going to run algorithm. Max generations: {0}, Min fitness: {1}", goal.MaxGenerations, goal.MinFitness);
            generatePopulation(population_size);

            population[0].FunctionEvaluations = 0;

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            do
            {
                shufflePopulation();
                procreatePopulation(population_size);
                sortPopulation();
                selectPopulation(population_size);

                if (!silent)
                    printPopulation();

                current_generation++;

                if (first_goal_hit < 0 && population[0].Fitness >= goal.MinFitness)
                    first_goal_hit = current_generation;

                // No new genes entered the population (whatever the genome as, the last one should have shifted.
                // Comparing pointers here
                if ((IGenome)previous_last == (IGenome)population[last_index])
                {
                    convergence_hit = current_generation;
                    break;
                }

                // Or if all genomes are the same, possibly costly operation.
                if (converged(last_index))
                {
                    convergence_hit = current_generation;
                    break;
                }
            } while (true);

            stopwatch.Stop();

            Console.WriteLine(results(population_size));

            InnerResult res = new InnerResult();
            res.FirstHitGeneration = first_goal_hit;
            res.Success = goal.AchievedFitnessGoal(population[0].Fitness);
            
            res.ConvergenceGeneration = convergence_hit;
            res.CPUTime = stopwatch.ElapsedTicks;
            res.FunctionEvaluations = population[0].FunctionEvaluations;
            res.BestScore = population[0].Fitness;
            res.BestSolution = population[0].ToString();

            if (best_result == null || population[0].Fitness > best_result.Fitness)
                best_result = population[0];

            return res;
        }

        public T BestResult { get { return best_result; } }

        private bool converged(int last_index)
        {
            // So, let's start by comparing the last with the first, and then go from there one
            T last = population[last_index];

            for (int i = 0; i < population.Count; ++i)
            {
                // First check fitness, a cheaper operation than comparing strings.
                if (last.Fitness != population[i].Fitness)
                    return false;

                if (last.GetHashCode() != population[i].GetHashCode())
                    return false;
            }


            for (int i = 0; i < population.Count; i++)
            {
                // Only if the fitnesses were equal, let's check the string.
                if (!last.Data.Equals(population[i].Data))
                    return false;

                // If both were equal, we can't do much else but to continue searching.
            }

            // Can we do this any smarter?
            return true;
        }

        private void generatePopulation(int population_size)
        {
            population = new List<T>();

            for (uint i = 0; i < population_size; ++i)
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
            previous_last = population.Last();

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

        private string results(int population_size)
        {
            return String.Format("[{0}|{1}] - {2}", crossover_provider.Name, population_size,
                population[0]);
        }
    }
}
