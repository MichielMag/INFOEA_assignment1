using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Genome
{
    public abstract class RandomlyLinkedAbstractGenome : AbstractGenome
    {
        protected static int[] elementOrder;

        public RandomlyLinkedAbstractGenome(string _data) : base(_data)
        {
        }

        public RandomlyLinkedAbstractGenome(int _data_size) : base(_data_size)
        {
        }

        public override void Generate(ref Random random)
        {
            if(elementOrder == null || elementOrder.Length == 0)
            {
                elementOrder = new int[data_size];
                for (int i = 0; i < data_size; i++)
                    elementOrder[i] = i;

                for (int i = 0; i < data_size; i++)
                {
                    int j = random.Next(data_size - i);
                    int x = elementOrder[i];
                    elementOrder[i] = elementOrder[j + i];
                    elementOrder[j + i] = x;
                }
            }
            for (int i = 0; i < data_size; ++i)
            {
                data += random.Next(2);
            }
        }

        protected abstract override void calculateFitness();
    }
}
