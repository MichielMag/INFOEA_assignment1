using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INFOEA.Algorithm.Crossover;
using INFOEA.Algorithm;
using INFOEA.Algorithm.Comparer;
using INFOEA.Algorithm.Genome.Graph;
using INFOEA.Algorithm.Algorithm;
using INFOEA.Algorithm.Neighborhood;
using System.Diagnostics;
using System.Threading;

namespace INFOEA.Assignment2
{
    class Program
    {
        static void test()
        {
            
        }
        static void Main(string[] args)
        {
            AssignmentTwo assignment = new AssignmentTwo();
            assignment.start(new Random());
            Console.ReadLine();

            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");
            Random r = new Random();
            graph.Generate(ref r);

            
            GeneticAlgorithm<GraphGenome> alg = new GeneticAlgorithm<GraphGenome>(500, 
                new UniformCrossover<GraphGenome>(r), 
                new GraphComparer<GraphGenome>(),
                new Goal(999999, 999999), 
                r);
            //alg.start(100, false);
            //alg.BestResult.ToImage(3000, 3000);

            LocalSearch<GraphGenome> local_search = new LocalSearch<GraphGenome>(500, new SwapNeighborhood<GraphGenome>(r), new GraphComparer<GraphGenome>(), r);
            GraphGenome optimum = null;
            List<long> times = new List<long>();
            for(int i = 0; i < 100; ++i)
            {
                graph = new GraphGenome(500);
                graph.Generate(ref r);
                Stopwatch sw = new Stopwatch();
                sw.Start();
                GraphGenome inner_optimum = local_search.Search(graph);
                sw.Stop();
                times.Add(sw.ElapsedMilliseconds);
                Console.WriteLine("Found {0} in {1}ms.", inner_optimum.Fitness, sw.ElapsedMilliseconds);

                if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                    optimum = inner_optimum;
            }

            Console.WriteLine("Best solution found: {0} in an average of {1}ms", optimum.Fitness, times.Average());
            optimum.ToImage(3000, 3000);

            double compared_opt = optimum.Fitness;
            optimum.recalculate();
            double recalculated = optimum.Fitness;

            Console.WriteLine(optimum);
            Console.ReadLine();
        }
    }
}
