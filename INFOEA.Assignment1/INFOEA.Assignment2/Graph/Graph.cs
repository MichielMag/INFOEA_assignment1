using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment2.Graph
{
    class Graph
    {
        private Vertex[] vertices;

        public Graph(string path_to_graph_file)
        {
            vertices = ReadFromFile(path_to_graph_file);
        }

        public Graph(Vertex[] _vertices)
        {
            vertices = _vertices;
        }

        private Vertex[] ReadFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            int n = lines.Length;

            Vertex[] vs = new Vertex[n];
            for(int i = 0; i < n; ++i)
            {
                vs[i] = Vertex.Parse(lines[i]);
            }

            return vs;
        }
    }
}
