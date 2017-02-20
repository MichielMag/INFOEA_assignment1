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
        public DTTGenome(int data_size) : base(data_size) { }

        public DTTGenome(string data) : base(data) { }

        protected override void calculateFitness()
        {
            int score = 0;

            for (int i = 0; i < data_size; i += 4)
            {
                int ones = 0;
                for (int j = 0; j < 4; ++j)
                    ones += data[i + j] == '1' ? 1 : 0;
                switch (ones)
                {
                    case 0:
                        score += 3;
                        break;
                    case 1:
                        score += 2;
                        break;
                    case 2:
                        score += 1;
                        break;
                    case 4:
                        score += 4;
                        break;
                }
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
            return String.Format("{0}: {1}", Fitness, chopped);
        }
    }
}
