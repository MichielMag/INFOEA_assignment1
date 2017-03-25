using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Procreation
{
    public interface IProcreator<T> where T:IGenome
    {
        List<T> Procreate(List<T> population);
    }
}
