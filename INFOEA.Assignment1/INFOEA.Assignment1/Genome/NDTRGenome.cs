using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    /**
     * Non-Deceptive Trap Randomly linked Function Genome
     **/
    class NDTRGenome : DTRGenome
    {
        public NDTRGenome(int data_size) : base(data_size)
        {
            name = "NDTR";
            fitnessValues = new float[5] { 1.5f, 1.0f, 0.5f, 0f, 4f };
        }

        public NDTRGenome(string data) : base(data)
        {
            name = "NDTR";
            fitnessValues = new float[5] { 1.5f, 1.0f, 0.5f, 0f, 4f };
        }
    }
}
