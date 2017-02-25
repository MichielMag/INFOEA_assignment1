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
    class UCOGenome : TightlyLinkedAbstractGenome
    {
        public UCOGenome(int data_size) : base(data_size) { name = "UCO"; }

        public UCOGenome(string data) : base(data) { name = "UCO"; }

        

        protected override void calculateFitness()
        {
            fitness = data.Count(c => c.Equals('1'));
        }
    }
}
