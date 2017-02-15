using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Domain.Genome
{
    interface IGenome : IComparable<IGenome>
    {
        int Fitness { get; }
    }
}
