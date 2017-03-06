using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment2
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph.Graph graph = new Graph.Graph("Graph500.txt");
            Random r = new Random();
            graph.Generate(ref r);
            graph.ToImage(3000, 3000);
        }
    }
}
