using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    /**
     * Lineary Counting Ones
     **/
    class LCOGenome : AbstractGenome
    {
        public LCOGenome(int data_size) : base(data_size) { }

        public LCOGenome(string data) : base(data) { }

        public override int Fitness
        {
            get
            {
                int score = 0; 
                for(int i = 0; i < data_size; ++i)
                {
                    score += data[i] == '1' ? i : 0;
                }

                return score;
            }
        }

        public override void Generate(ref Random random)
        {
            for (int i = 0; i < data_size; ++i)
            {
                data += random.Next(2);
            }
        }
    }
}
