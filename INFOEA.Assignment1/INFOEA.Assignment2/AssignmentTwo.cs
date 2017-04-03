using INFOEA.Algorithm.Algorithm;
using INFOEA.Algorithm.Comparer;
using INFOEA.Algorithm.Crossover;
using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using INFOEA.Algorithm.Mutation;
using INFOEA.Algorithm.Neighborhood;
using INFOEA.Algorithm.PopulationGeneration;
using INFOEA.Algorithm.Procreation;
using INFOEA.Algorithm.Results;
using INFOEA.Algorithm.Selector;
using INFOEA.Algoritmh.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INFOEA.Assignment2
{
    class AssignmentTwo
    {
        public int OptimaAmount { get; set; } = 2500;
        public int ExperimentAmount { get; set; } = 25;

        //private Random[] random;

        private const int nOfProc = 1;
        private Thread[] threads = new Thread[nOfProc];
        private GraphGenome[] solutionsThreads = new GraphGenome[nOfProc];
        private List<AssignmentTwoResults<GraphGenome>>[] elapsedMilisecondsThreads = new List<AssignmentTwoResults<GraphGenome>>[nOfProc];

        private Random main_random_source;

        private void searchThread(int pID, int max, int seed, INeighborhood<GraphGenome> neighborhood, bool silent = false)
        {
            Console.WriteLine("Thread {0} started...", pID);

            Random random = new Random(seed);

            // TODO: Resultaten (dus CPU computational time) opslaan in bestand.
            LocalSearch<GraphGenome> local_search =
                new LocalSearch<GraphGenome>(500,
                        neighborhood,
                        new GraphComparer<GraphGenome>(), random);

            // Dit wil ik eigenlijk static doen, maar dan komen we in de knoei met data_size.
            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");

            GraphGenome optimum = null;

            for (int i = 0; i < ExperimentAmount; ++i)
            {
                AssignmentTwoResults<GraphGenome> inner_results = new AssignmentTwoResults<GraphGenome>();
                for (int j = 0; j < max; j++)
                {
                    graph = new GraphGenome(500);
                    graph.Generate(ref random);
                    Stopwatch sw = Stopwatch.StartNew();
                    GraphGenome inner_optimum = local_search.Search(graph);
                    sw.Stop();
                    inner_results.Add(inner_optimum, sw.ElapsedTicks);

                    if (!silent)
                        Console.WriteLine("Found {0} in {1} ticks or {2}ms. ({3}/{4}), pID: {5}", inner_optimum.Fitness, sw.ElapsedTicks,
                            sw.ElapsedMilliseconds, j, max, pID);

                    if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                        optimum = inner_optimum;
                }
                lock (solutionsThreads)
                {
                    solutionsThreads[pID] = optimum;
                }
                lock (elapsedMilisecondsThreads)
                {
                    elapsedMilisecondsThreads[pID].Add(inner_results);
                }
            }
        }

        private bool threadsAreRunning()
        {
            foreach (Thread T in threads)
            {
                if (T.ThreadState == System.Threading.ThreadState.Running) return true;
            }
            return false;
        }

        private List<AssignmentTwoResults<GraphGenome>> MultiStartLocalSearch(INeighborhood<GraphGenome> neighborhood)
        {
            //we moeten nog iets met ExperimentAmount doen...
            //for(int i = 0; i < ExperimentAmount; ++i)

            List<AssignmentTwoResults<GraphGenome>> results = new List<AssignmentTwoResults<GraphGenome>>();

            for (int j = 0; j < threads.Length; j++)
            {
                int k = j;
                int seed = main_random_source.Next();
                int amount = OptimaAmount / threads.Length;
                Thread T = new Thread(() => searchThread(k, amount, seed, neighborhood));
                threads[j] = T;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (Thread T in threads)
                T.Start();
            while (this.threadsAreRunning()) ;
            sw.Stop();

            GraphGenome optimum = null;
            for (int j = 0; j < threads.Length; j++)
            {
                if (optimum == null || optimum.Fitness > solutionsThreads[j].Fitness)
                    optimum = solutionsThreads[j];
            }

            //List<AssignmentTwoResults> results = new List<AssignmentTwoResults>();
            foreach (List<AssignmentTwoResults<GraphGenome>> L in elapsedMilisecondsThreads)
            {
                results.AddRange(L);
            }

            //Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, cpu_ticks.Average());
            Console.WriteLine("Best solution found: {0} in an average of {} ms", optimum.Fitness); //, results.Sum() / OptimaAmount);
            Console.WriteLine("Total time: {0} sec", sw.ElapsedMilliseconds / 1000f);

            optimum.ToImage("Graph.bmp", 3000, 3000);

            return results;
        }

        private List<AssignmentTwoResults<GraphGenome>> IteratedLocalSearch(INeighborhood<GraphGenome> neighborhood, bool silent = false)
        {
            LocalSearch<GraphGenome> local_search =
                new LocalSearch<GraphGenome>(500,
                        neighborhood,
                        new GraphComparer<GraphGenome>(), main_random_source);
            IMutation<GraphGenome> pertubation = new ILSPertubation<GraphGenome>(main_random_source);

            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");

            GraphGenome optimum = null;

            List<long> elapsedMilisecondsList = new List<long>();

            List<AssignmentTwoResults<GraphGenome>> results = new List<AssignmentTwoResults<GraphGenome>>();

            for (int i = 0; i < ExperimentAmount; ++i)
            {
                AssignmentTwoResults<GraphGenome> res = new AssignmentTwoResults<GraphGenome>();
                for (int j = 0; j < OptimaAmount; j++)
                {
                    if (optimum == null)
                    {
                        graph = new GraphGenome(500);
                        graph.Generate(ref main_random_source);
                    }
                    else
                    {
                        graph = pertubation.Mutate(optimum);
                    }

                    Stopwatch sw = Stopwatch.StartNew();
                    GraphGenome inner_optimum = local_search.Search(graph);
                    sw.Stop();
                    elapsedMilisecondsList.Add(sw.ElapsedMilliseconds);

                    res.Add(inner_optimum, sw.ElapsedTicks);

                    if (!silent)
                        Console.WriteLine("Found {0} in {1} ticks or {2}ms. ({3}/{4})", inner_optimum.Fitness, sw.ElapsedTicks,
                            sw.ElapsedMilliseconds, j, OptimaAmount);

                    if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                        optimum = inner_optimum;

                    optimum.FunctionEvaluations = 0;
                }
                results.Add(res);
            }
            //Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, cpu_ticks.Average());
            Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, elapsedMilisecondsList.Sum() / OptimaAmount);
            //Console.WriteLine("Total time: {0} sec", sw.ElapsedMilliseconds / 1000f);

            optimum.ToImage(String.Format("ILS-{0}.bmp", optimum.Fitness), 3000, 3000);

            return results;
        }

        private List<AssignmentTwoResults<GraphGenome>> GeneticLocalSearch(INeighborhood<GraphGenome> neighborhood)
        {
            List<AssignmentTwoResults<GraphGenome>> results = new List<AssignmentTwoResults<GraphGenome>>();

            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");

            GraphGenome optimum = null;

            for (int i = 0; i < ExperimentAmount; ++i)
            {
                AssignmentTwoResults<GraphGenome> res = new AssignmentTwoResults<GraphGenome>();

                LocalSearch<GraphGenome> local_search =
                    new LocalSearch<GraphGenome>(500,
                            neighborhood,
                            new GraphComparer<GraphGenome>(), main_random_source);

                LocalSearchProcreator<GraphGenome> lsp = new LocalSearchProcreator<GraphGenome>(
                    new UniformSymmetricCrossover<GraphGenome>(main_random_source),
                    local_search,
                    main_random_source,
                    res);

                int population_size = 50;
                GeneticAlgorithm<GraphGenome> ga = new GeneticAlgorithm<GraphGenome>(
                    population_size,
                    lsp,
                    new DefaultSelector<GraphGenome>(
                        new GraphComparer<GraphGenome>()),
                    new LocalSearchPopulationGenerator<GraphGenome>(main_random_source, local_search),
                    new Goal(100, 0),
                    main_random_source,
                    "GLS");

                InnerResult ir = ga.start(100, OptimaAmount / (population_size / 2));

                results.Add(res);

                if (optimum == null || ir.BestScore < optimum.Fitness)
                    optimum = new GraphGenome(ir.BestSolution, ir.BestScore);
            }




            // We maken iedere keer population_size / 2 optima.
            // We willen OptimaAmount optima. Dus we gaan OptimaAmount / (population_size / 2) generaties uitvoeren.

            //IteratedLocalSearch();

            optimum.ToImage(String.Format("GLS-{0}.bmp", optimum.Fitness), 3000, 3000);

            return results;
        }

        public void start(int seed = -1)
        {
            if (seed > 0)
                main_random_source = new Random(seed);
            else
                main_random_source = new Random();

            INeighborhood<GraphGenome> swap_neighborhood = new SwapNeighborhood<GraphGenome>(main_random_source);
            INeighborhood<GraphGenome> FM_neigborhood = new FiducciaMatheysesNeighborhood<GraphGenome>(main_random_source, 7);

            // Eerst de "gewone" experimenten:
            List<AssignmentTwoResults<GraphGenome>> mls_results = MultiStartLocalSearch(swap_neighborhood);
            List<AssignmentTwoResults<GraphGenome>> ils_results = IteratedLocalSearch(swap_neighborhood);
            List<AssignmentTwoResults<GraphGenome>> gls_results = GeneticLocalSearch(swap_neighborhood);

            // Dan de Fiduccia experimenten:
            List<AssignmentTwoResults<GraphGenome>> fm_mls_results = MultiStartLocalSearch(swap_neighborhood);
            List<AssignmentTwoResults<GraphGenome>> fm_ils_results = IteratedLocalSearch(swap_neighborhood);
            List<AssignmentTwoResults<GraphGenome>> fm_gls_results = GeneticLocalSearch(swap_neighborhood);
        }

        private void WriteStatistics(List<AssignmentTwoResults<GraphGenome>> mls_results,
                                     List<AssignmentTwoResults<GraphGenome>> ils_results,
                                     List<AssignmentTwoResults<GraphGenome>> gls_results)
        {
            double mls_ils_t = t_score(mls_results, ils_results);
            double mls_gls_t = t_score(mls_results, gls_results);
            double ils_gls_t = t_score(ils_results, gls_results);
        }

        private double t_score(List<AssignmentTwoResults<GraphGenome>> first, List<AssignmentTwoResults<GraphGenome>> second)
        {
            int n = first.Count;
            // Eerst van MLS <-> ILS
            List<long> differences = new List<long>();
            for (int i = 0; i < n; ++i)
                differences.Add(first[i].TotalTicks - second[i].TotalTicks);

            double sample_mean = differences.Average();
            double std_dev = Math.Sqrt(differences.Sum(d => Math.Pow((d - sample_mean), 2)) / (n - 1));
            double t_score = (sample_mean - 0) / (std_dev / Math.Sqrt(n));

            return t_score;
        }
    }
}
