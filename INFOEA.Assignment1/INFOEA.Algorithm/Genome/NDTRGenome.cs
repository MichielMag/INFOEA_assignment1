using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Genome
{
    /**
     * Non-Deceptive Trap Randomly linked Function Genome
     **/
    public class NDTRGenome : RandomlyLinkedAbstractGenome
    {
        public NDTRGenome(int data_size) : base(data_size)
        {
            name = "NDTR";
        }

        public NDTRGenome(string data) : base(data)
        {
            name = "NDTR";
        }

        protected override void calculateFitness()
        {
            float score = 0;

            for (int i = 0; i < data_size; i += 4)
            {
                int ones = 0;
                for (int j = 0; j < 4; ++j)
                    ones += data[elementOrder[i + j]] == '1' ? 1 : 0;

                switch (ones)
                {
                    case 0:
                        {
                            score += 1.5f;
                            break;
                        }
                    case 1:
                        {
                            score += 1.0f;
                            break;
                        }
                    case 2:
                        {
                            score += 0.5f;
                            break;
                        }
                    case 4:
                        {
                            score += 4f;
                            break;
                        }
                }
            }

            fitness = score;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}, {2}", Fitness, Name, Data);
        }
    }
}
