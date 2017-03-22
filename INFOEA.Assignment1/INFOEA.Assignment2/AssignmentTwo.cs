using INFOEA.Algorithm.Algorithm;
using INFOEA.Algorithm.Comparer;
using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using INFOEA.Algorithm.Neighborhood;
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

        private const int nOfProc = 4;
        private Thread[] threads = new Thread[nOfProc];
        private GraphGenome[] solutionsThreads = new GraphGenome[nOfProc];
        private List<long>[] elapsedMilisecondsThreads = new List<long>[nOfProc];

        private void searchThread(int pID, int max, int seed, bool silent = false)
        {
            Console.WriteLine("Thread {0} started...", pID);

            Random random = new Random(seed);

            // TODO: Resultaten (dus CPU computational time) opslaan in bestand.
            LocalSearch<GraphGenome> local_search =
                new LocalSearch<GraphGenome>(500,
                        new SwapNeighborhood<GraphGenome>(random),
                        new GraphComparer<GraphGenome>(), random);

            // Dit wil ik eigenlijk static doen, maar dan komen we in de knoei met data_size.
            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");

            GraphGenome optimum = null;

            List<long> elapsedMilisecondsList = new List<long>();

            for (int j = 0; j < max; j++)
            {
                graph = new GraphGenome(500);
                graph.Generate(ref random);
                Stopwatch sw = Stopwatch.StartNew();
                GraphGenome inner_optimum = local_search.Search(graph);
                sw.Stop();
                elapsedMilisecondsList.Add(sw.ElapsedMilliseconds);

                if (!silent)
                    Console.WriteLine("Found {0} in {1} ticks or {2}ms. ({3}/{4}), pID: {5}", inner_optimum.Fitness, sw.ElapsedTicks, 
                        sw.ElapsedMilliseconds, j, max, pID);

                if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                    optimum = inner_optimum;
            }
            lock(solutionsThreads)
            {
                solutionsThreads[pID] = optimum;
            }
            lock(elapsedMilisecondsThreads)
            {
                elapsedMilisecondsThreads[pID] = elapsedMilisecondsList;
            }
        }

        private bool threadsAreRunning()
        {
            foreach(Thread T in threads)
            {
                if (T.ThreadState == System.Threading.ThreadState.Running) return true;
            }
            return false;
        }

        private void MultiStartLocalSearch()
        {
            //we moeten nog iets met ExperimentAmount doen...
            //for(int i = 0; i < ExperimentAmount; ++i)

            for (int j = 0; j < threads.Length; j++)
            {
                int k = j;
                int seed = k;
                int amount = OptimaAmount / threads.Length;
                Thread T = new Thread(() => searchThread(k, amount, seed));
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

            List<long> results = new List<long>();
            foreach (List<long> L in elapsedMilisecondsThreads)
            {
                results.AddRange(L);
            }

            //Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, cpu_ticks.Average());
            Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, results.Sum() / OptimaAmount);
            Console.WriteLine("Total time: {0} sec", sw.ElapsedMilliseconds / 1000f);

            optimum.ToImage(3000, 3000);
        }
        
        public void start()
        {
            MultiStartLocalSearch();
        }

    }
}
