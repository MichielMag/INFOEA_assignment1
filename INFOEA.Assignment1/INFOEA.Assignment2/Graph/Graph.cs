using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Genome;

namespace INFOEA.Assignment2.Graph
{
    class Graph : TightlyLinkedAbstractGenome
    {
        private static Vertex[] vertices;
        private static bool[,] connections;
        
        public Graph(string path_to_graph_file) : base(0)
        {
            vertices = ReadFromFile(path_to_graph_file);
            connections = MakeConnections(vertices);
            data_size = vertices.Length;
        }

        public Graph(Vertex[] _vertices) : base (_vertices.Length)
        {
            vertices = _vertices;
            connections = MakeConnections(vertices);
        }

        private bool[,] MakeConnections(Vertex[] _vertices)
        {
            int n = _vertices.Length;
            bool[,] cons = new bool[n, n];
            foreach(Vertex v in _vertices)
            {
                int id = v.Id;
                foreach(int con in v.Connections)
                {
                    cons[id, con] = true;
                }
            }

            return cons;
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

        protected override void calculateFitness()
        {
            foreach(char c in data)
            {

            }
        }
    }
}
