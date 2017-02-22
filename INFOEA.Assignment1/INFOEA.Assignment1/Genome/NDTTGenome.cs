using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    /**
    * Non-Deceptive Trap Tightly Function Genome
    **/

    class NDTTGenome : DTTGenome
    {
        public NDTTGenome(int data_size) : base(data_size)
        {
            name = "NDTT";
            fitnessValues = new float[5] { 1.5f, 1.0f, 0.5f, 0f, 4f };
        }

        public NDTTGenome(string data) : base(data)
        {
            name = "NDTT";
            fitnessValues = new float[5] { 1.5f, 1.0f, 0.5f, 0f, 4f};
        }

    }
}
