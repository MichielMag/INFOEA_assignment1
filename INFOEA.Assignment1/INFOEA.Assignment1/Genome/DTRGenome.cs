using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    /**
     * Deceptive Trap Randomly linked Function Genome
     **/
    class DTRGenome : DTTGenome
    {
        public DTRGenome(int data_size) : base(data_size)  { name = "DTR"; }

        public DTRGenome(string data) : base(data)  { name = "DTR"; }

        protected override void calculateFitness()
        {
            float score = 0;

            for (int i = 0; i < data_size; i += 4)
            {
                int ones = 0;
                for (int j = 0; j < 4; ++j)
                    ones += data[ elementOrder[ i + j ] ] == '1' ? 1 : 0;

                score += fitnessValues[ones];
            }

            fitness = score;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}, {2}", Fitness, Name, Data);
        }
    }
}
