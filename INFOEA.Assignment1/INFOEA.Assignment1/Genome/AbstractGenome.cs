using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    abstract class AbstractGenome : IGenome
    {
        protected string data;
        protected int data_size;

        public AbstractGenome(int _data_size)
        {
            data_size = _data_size;
        }

        public AbstractGenome(string _data)
        {
            data = _data;
            data_size = _data.Count();
        }

        public string Data
        {
            get
            {
                return data;
            }
        }

        public int DataSize
        {
            get
            {
                return data_size;
            }
        }

        public abstract int Fitness
        {
            get;
        }

        public int CompareTo(IGenome other)
        {
            return this.Fitness.CompareTo(other.Fitness);
        }

        public void Generate(ref Random random)
        {
            for (int i = 0; i < data_size; ++i)
            {
                data += random.Next(2);
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Fitness, Data);
        }
    }
}
