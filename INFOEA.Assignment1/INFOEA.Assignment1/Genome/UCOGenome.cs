using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    /**
     * Uniformly Counting Ones Genome
     **/
    class UCOGenome : AbstractGenome
    {
        public UCOGenome(int data_size) : base(data_size) { }

        public UCOGenome(string data) : base(data) { }

        public override int Fitness
        {
            get
            {
                return data.Count(c => c.Equals('1'));
            }
        }
    }
}
