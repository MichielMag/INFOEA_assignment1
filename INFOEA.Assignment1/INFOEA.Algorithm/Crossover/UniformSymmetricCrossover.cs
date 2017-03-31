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

        private int computeHammingDistance(T ParentOne, T ParentTwo)
        {
            int val = 0;
            for (int i = 0; i < ParentOne.DataSize; i++)
            {
                if (ParentOne.Data[i] != ParentTwo.Data[i]) val++;
            }
            return val;
        }

        private T invertBits(T Parent)
        {
            string new_data = "";
            foreach(char c in Parent.Data)
                new_data += (c == '1') ? "0" : "1";
            return (T)Activator.CreateInstance(typeof(T), new_data);
        }

        private T generateChild(T ParentOne, T ParentTwo)
        {
            int data_size = ParentOne.DataSize;
            char[] child_data = new char[data_size];
            Dictionary<char, int> counted = new Dictionary<char, int>();
            counted.Add('0', 0);
            counted.Add('1', 0);

            for (int i = 0; i < data_size; i++)
            {
                char c = ParentOne.Data[i];
                if (c == ParentTwo.Data[i])
                {
                    child_data[i] = c;
                    counted[c]++;
                }
            }

            for (int i = 0; i < data_size; i++)
            {
                if (ParentOne.Data[i] != ParentTwo.Data[i]) 
                {
                    char c = random_source.Next(2).ToString()[0];
                    if (counted[c] > data_size / 2 - 1)
                        c = (c == '1') ? '0' : '1';
                    child_data[i] = c;
                    counted[c]++;
                }
            }

            return (T)Activator.CreateInstance(typeof(T), new string(child_data));
        }

        public override Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo)
        {
            T child;
            int data_size = ParentOne.DataSize;
            int hammingDistance = computeHammingDistance(ParentOne, ParentTwo);
            if (hammingDistance > data_size / 2)
            {
                T ParentTwoInverted = invertBits(ParentTwo);
                child = this.generateChild(ParentOne, ParentTwoInverted);
            }
            else
                child = this.generateChild(ParentOne, ParentTwo);

            return new Tuple<T, T>(child, child);

            /*
            int hammingDistance = this.computeHammingDistance(ParentOne, ParentTwo);


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
            */
        } 
    }
}
