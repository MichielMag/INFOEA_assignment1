using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    /**
     * Deceptive Trap Tightly Function Genome
     **/
    class DTTGenome : AbstractGenome
    {
        public DTTGenome(int data_size) : base(data_size)
        {
            name = "DTT";
            fitnessValues = new float[5] { 3, 2, 1, 0, 4 };
        }

        public DTTGenome(string data) : base(data)
        {
            name = "DTT";
            fitnessValues = new float[5] { 3, 2, 1, 0, 4 };
        }

        protected float[] fitnessValues;

        protected override void calculateFitness()
        {
            float score = 0;

            for (int i = 0; i < data_size; i += 4)
            {
                int ones = 0;
                for (int j = 0; j < 4; ++j)
                    ones += data[i + j] == '1' ? 1 : 0;

                score += fitnessValues[ones];
            }

            fitness = score;
        }

        public override string ToString()
        {
            string chopped = "|";
            for (int i = 0; i < data_size; i += 4)
            {
                chopped += data.Substring(i, 4) + "|";
            }
            return String.Format("{0}: {1}, {2}", Fitness, Name, chopped);
        }
    }
}
