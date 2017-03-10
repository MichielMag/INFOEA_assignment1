using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using INFOEA.Assignment1.Algorithm.Crossover;
using INFOEA.Assignment1.Algorithm;
using INFOEA.Assignment1.Algorithm.Comparer;


namespace INFOEA.Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph.Graph graph = new Graph.Graph(500);
            graph.CreateGraph("Graph500.txt");
            Random r = new Random();
            graph.Generate(ref r);

            GeneticAlgorithm<Graph.Graph> alg = new GeneticAlgorithm<Graph.Graph>(500, 
                new UniformCrossover<Graph.Graph>(r), 
                new GraphComparer<Graph.Graph>(),
                new Goal(999999, 999999), 
                r);
            alg.start(100, false);
            alg.BestResult.ToImage(3000, 3000);

            Console.WriteLine(graph);
            Console.ReadLine();
        }
    }
}
