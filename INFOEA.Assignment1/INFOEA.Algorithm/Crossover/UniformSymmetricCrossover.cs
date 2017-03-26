using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Crossover
{
    public class UniformSymmetricCrossover<T> : AbstractCrossover<T> where T:IGenome
    {
        private double flipping_chance = 0.5;

        public UniformSymmetricCrossover(Random random) : base(random, "UBG")
        {
        }

        public override Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo)
        {
            int data_size = ParentOne.DataSize;
            
            string child_data = "";

            for (int i = 0; i < data_size; i++)
            {
                if (random_source.NextDouble() <= flipping_chance)
                {
                    child_data += ParentOne.Data[i];
                }
                else
                {
                    child_data += ParentTwo.Data[i];
                }
            }

            // En nu repareren. Ben niet heel blij met deze functie...
            int zero_count = child_data.Count(x=>x=='0');
            while (zero_count != data_size / 2)
            {
                int random_pos = random_source.Next(data_size);
                if (zero_count > data_size / 2 && child_data[random_pos] == '0')
                {
                    child_data = child_data.Substring(0, random_pos) + '1' + child_data.Substring(random_pos + 1, data_size - random_pos - 1);
                    zero_count--;
                }
                else if (zero_count < data_size && child_data[random_pos] == '1')
                {
                    child_data = child_data.Substring(0, random_pos) + '0' + child_data.Substring(random_pos + 1, data_size - random_pos - 1);
                    zero_count++;
                }
            }

            T child_one = (T)Activator.CreateInstance(typeof(T), child_data);

            // We verwachten er toch maar 1, dus dan maar zo... 
            // In C++ hadden we met templates ook wel aantallen kunnen meegeven om te returnen
            // In C# kan ik me alleen maar bedenken om een lijst te returnen, maar dat is ook weer zo..
            // Dus dan maar zo.
            return new Tuple<T, T>(child_one, child_one);
        }
    }
}
