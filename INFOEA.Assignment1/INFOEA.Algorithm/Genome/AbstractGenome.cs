using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Genome
{
    public abstract class AbstractGenome : IGenome
    {
        protected string data;
        protected int data_size;
        protected int hash = -1;

        protected string name;

        public string Name
        {
            get { return name; }
        }

        protected float fitness = -1;

        static long function_evaluations;

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

        public long FunctionEvaluations
        {
            get
            {
                return function_evaluations;
            }
            set
            {
                function_evaluations = value;
            }
        }

        public float Fitness
        {
            get
            {
                if (fitness < 0)
                {
                    calculateFitness();
                    function_evaluations++;
                }
                return fitness;
            }
        }

        protected abstract void calculateFitness();

        public int CompareTo(IGenome other)
        {
            return this.Fitness.CompareTo(other.Fitness);
        }

        public abstract void Generate(ref Random random);

        public override string ToString()
        {
            return String.Format("{0}: {1}", Fitness, Name); //, {2}", Fitness, Name, Data);
        }

        

        public override int GetHashCode()
        {
            if (this.hash > -1) return hash;
            else
            {
                calculateHashCode();
                return hash;
            }
        }

        protected void calculateHashCode()
        {
            int M = 104729;
            int q = 34061;
            int a = 13;
            int b = 17;

            int h = 0;

            for (int i = 0; i < data_size; i++)
            {
                if (data[i] == '1')
                {
                    int p = i * q + b;
                    p = ((p * a) % M);
                    h = ((h + p) % M);
                }
            }

            this.hash = h;
        }
    }
}
