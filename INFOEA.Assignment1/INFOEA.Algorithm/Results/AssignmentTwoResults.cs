using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algoritmh.Results
{
    public class AssignmentTwoResult<T> where T:IGenome
    {
        public T Optimum { get; set; }
        public long TicksToReach { get; set; }

    }
    public class AssignmentTwoResults<T> : List<AssignmentTwoResult<T>> where T:IGenome
    {
        public double AverageOptimum
        {
            get
            {
                return this.Average(g => g.Optimum.Fitness);
            }
        }
        public double AverageTicks
        {
            get
            {
                return this.Average(g => g.TicksToReach);
            }
        }

        public long TotalTicks
        {
            get
            {
                return this.Sum(g => g.TicksToReach);
            }
        }

        public void Add(T optimum, long ticks)
        {
            this.Add(new AssignmentTwoResult<T>() { Optimum = optimum, TicksToReach = ticks });
        }
    }
}
