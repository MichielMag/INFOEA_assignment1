using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Mutation
{
    public interface IMutation<T> where T:IGenome
    {
        T Mutate(T solution);
    }
}
