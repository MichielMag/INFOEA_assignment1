using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Genome
{
    public abstract class TightlyLinkedAbstractGenome : AbstractGenome
    {
        public TightlyLinkedAbstractGenome(string _data) : base(_data)
        {
        }

        public TightlyLinkedAbstractGenome(int _data_size) : base(_data_size)
        {
        }

        protected abstract override void calculateFitness();

        public override void Generate(ref Random random)
        {
            for (int i = 0; i < data_size; ++i)
            {
                data += random.Next(2);
            }
        }
    }
}
