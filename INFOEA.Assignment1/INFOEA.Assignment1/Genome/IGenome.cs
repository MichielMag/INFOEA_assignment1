using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Genome
{
    interface IGenome : IComparable<IGenome>
    {
        float Fitness { get; }
        void Generate(ref Random random);
        string Data { get; }
        int DataSize { get; }
        long FunctionEvaluations { get; set; }
    }
}
