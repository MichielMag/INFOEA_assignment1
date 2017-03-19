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

namespace INFOEA.Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
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

            for(int i = 0; i < 100; ++i)
            {
                graph = new GraphGenome(500);
                graph.Generate(ref r);
                GraphGenome inner_optimum = local_search.Search(graph);
                if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                    optimum = inner_optimum;
            }

            optimum.ToImage(3000, 3000);

            double compared_opt = optimum.Fitness;
            optimum.recalculate();
            double recalculated = optimum.Fitness;

            Console.WriteLine(optimum);
            Console.ReadLine();
        }
    }
}
