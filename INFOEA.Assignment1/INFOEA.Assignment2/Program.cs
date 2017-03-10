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
            alg.start(100, false);
            alg.BestResult.ToImage(3000, 3000);

            Console.WriteLine(graph);
            Console.ReadLine();
        }
    }
}
