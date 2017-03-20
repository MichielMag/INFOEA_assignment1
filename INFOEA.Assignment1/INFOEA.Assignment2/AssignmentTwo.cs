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
        
        private Random random;

        private void MultiStartLocalSearch()
        {
            // TODO: Resultaten (dus CPU computational time) opslaan in bestand.
            LocalSearch<GraphGenome> local_search = 
                new LocalSearch<GraphGenome>(500, 
                        new SwapNeighborhood<GraphGenome>(random), 
                        new GraphComparer<GraphGenome>(), random);

            // Dit wil ik eigenlijk static doen, maar dan komen we in de knoei met data_size.
            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");
            
            GraphGenome optimum = null;

            List<long> cpu_ticks = new List<long>();
            for(int i = 0; i < ExperimentAmount; ++i)
            {
                List<long> ticks = new List<long>();
                for (int j = 0; j < OptimaAmount; j++)
                {
                    graph = new GraphGenome(500);
                    graph.Generate(ref random);
                    Stopwatch sw = Stopwatch.StartNew();
                    GraphGenome inner_optimum = local_search.Search(graph);
                    sw.Stop();
                    ticks.Add(sw.ElapsedMilliseconds);
                    Console.WriteLine("Found {0} in {1} ticks or {2}ms. ({3}/{4})", inner_optimum.Fitness, sw.ElapsedTicks, sw.ElapsedMilliseconds, j, OptimaAmount);

                    if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                        optimum = inner_optimum;
                }
                cpu_ticks.Add(ticks.Sum());
                Console.WriteLine("Did {0} in {1} cpu ticks.", cpu_ticks.Last());
            }

            Console.WriteLine("Best solution found: {0} in an average of {1}ms", optimum.Fitness, cpu_ticks.Average());
            optimum.ToImage(3000, 3000);
        }
        
        public void start(Random _random)
        {
            random = _random;
            MultiStartLocalSearch();
        }

    }
}
