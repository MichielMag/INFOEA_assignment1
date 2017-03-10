using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Algorithm.Genome;

namespace INFOEA.Algorithm.Neighborhood
{
    public interface INeighborhood<T> where T:IGenome
    {
        IEnumerable<T> Neighbors(T solution);
    }
}
