using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment2.Graph
{
    enum Partition
    {
        ONE, TWO
    }
    class Vertex
    {
        public int Id { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public int[] Connections { get; private set; }
        public Partition Partition { get; set; }

        public Vertex(int _id, double _x, double _y, int[] _connections)
        {
            Id = _id;
            X = _x;
            Y = _y;
            Connections = _connections;
        }

        public static Vertex Parse(string vertex_string)
        {
            string[] pieces = vertex_string.Split(' ');
            int id = int.Parse(pieces[0]);
            string[] coordinates = pieces[1].Substring(1, pieces[1].Length - 2).Split(',');
            double x = double.Parse(coordinates[0], CultureInfo.InvariantCulture);
            double y = double.Parse(coordinates[1], CultureInfo.InvariantCulture);
            int n = int.Parse(pieces[2]);
            int[] connections = new int[n];
            for(int i = 3; i < n + 3; ++i)
            {
                connections[i - 3] = int.Parse(pieces[i]);
            }

            return new Vertex(id, x, y, connections);
        }
    }
}
