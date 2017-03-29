using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Algorithm.Genome;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace INFOEA.Algorithm.Genome.Graph
{
    public class GraphGenome : AbstractGenome
    {
        public static Dictionary<int, Vertex> vertices;
        
        public void CreateGraph(string path_to_graph_file)
        {
            vertices = ReadFromFile(path_to_graph_file);
            //connections = MakeConnections(vertices);
            data_size = vertices.Count;
        }
        public GraphGenome(string data ) : base(data)
        {
        }

        
        public GraphGenome(string data, float new_fitness) :base(data)
        {
            fitness = new_fitness;
        }

        public GraphGenome(int genome_size) : base(genome_size)
        {
        }

        public GraphGenome(Dictionary<int, Vertex> _vertices) : base (_vertices.Count)
        {
            vertices = _vertices;
            //connections = MakeConnections(vertices);
        }


        private Dictionary<int, Vertex> ReadFromFile(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            int n = lines.Length+1;

            Dictionary<int, Vertex> vs = new Dictionary<int, Vertex>();
            foreach(string line in lines)
            {
                Vertex v = Vertex.Parse(line);
                vs.Add(v.Id, v);
            }

            return vs;
        }

        protected override void calculateFitness()
        {
            fitness = 0;
            Dictionary<int, List<int>> counted = new Dictionary<int, List<int>>();
            for (int i = 1; i < data_size + 1; ++i)
            {
                char c = data[i - 1];
                Vertex v = vertices[i];
                counted.Add(i, new List<int>());
                foreach (int other in v.Connections)
                {
                    // Already plussed the score for this connection.
                    if (counted.ContainsKey(other) && counted[other].Contains(i))
                        continue;
                    else if (data[other - 1] != c) // If they are not in the same partition
                    {
                        fitness++;
                        counted[i].Add(other);
                    }
                }
            }
        }

        
        public void ToImage(int width = 1000, int height = 1000)
        {
            int point_size_width = (int)(width * 0.004);
            int point_size_height = (int)(height * 0.004);

            Bitmap bm = new Bitmap(width, height);
            Graphics gfx = Graphics.FromImage(bm);
            gfx.Clear(Color.White);
            Size point_size = new Size(point_size_width, point_size_height);
            foreach(KeyValuePair<int, Vertex> vertex in vertices) // lines
            {
                Vertex v = vertex.Value;

                int s_conx = (int)((width - point_size_width) * v.X);
                int s_cony = (int)((height - point_size_height) * v.Y);

                Point s_point = new Point(s_conx, s_cony);

                foreach (int id in v.Connections)
                {
                    Vertex con = vertices[id];
                    int t_conx = (int)((width - point_size_width) * con.X);
                    int t_cony = (int)((height - point_size_height) * con.Y);


                    Point t_point = new Point(t_conx, t_cony);

                    Pen pen = Pens.Black;

                    if (data[id - 1] != data[v.Id - 1]) // If they are not in the same partition
                    {
                        pen = Pens.Purple;
                    }

                    gfx.DrawLine(pen, s_point, t_point);
                }
            }
            foreach (KeyValuePair<int, Vertex> vertex in vertices) // nodes
            {
                Vertex v = vertex.Value;


                int x = (int)((width - point_size_width) * v.X - point_size_width / 2);
                int y = (int)((height - point_size_height) * v.Y - point_size_height / 2);
                Point v_point = new Point(x, y);
                Brush brush = null;

                if (data[v.Id - 1] == '0')
                    brush = Brushes.Blue;
                else
                    brush = Brushes.Red;                

                gfx.FillRectangle(brush, new Rectangle(v_point, point_size));
            }

             bm.Save("graph.bmp");
        }

        public override void Generate(ref Random random)
        {
            for (int i = 0; i < data_size; ++i)
            {
                data += random.Next(2);
            }

            int zero_count = data.Count(x => x == '0');

            // Make the bipartition solution valid
            while(zero_count != data_size / 2)
            {
                int random_pos = random.Next(data_size);
                if(zero_count > data_size / 2 && data[random_pos] == '0')
                {
                    data = data.Substring(0, random_pos) + '1' + data.Substring(random_pos + 1, data_size - random_pos - 1);
                    zero_count--;
                }
                else if (zero_count < data_size && data[random_pos] == '1')
                {
                    data = data.Substring(0, random_pos) + '0' + data.Substring(random_pos + 1, data_size - random_pos - 1);
                    zero_count++;
                }
            }
        }
    }
}
