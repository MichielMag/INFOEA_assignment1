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

        public override void Generate(ref Random random)
        {
            for(int i = 0; i < data_size; ++i)
            {
                data += random.Next(2);
            }
        }
    }
}
