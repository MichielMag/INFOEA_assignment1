﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Crossover
{
    public interface ICrossover<T>
    {
        Tuple<T, T> DoCrossover(T ParentOne, T ParentTwo);

        string Name { get;  }
    }
}
