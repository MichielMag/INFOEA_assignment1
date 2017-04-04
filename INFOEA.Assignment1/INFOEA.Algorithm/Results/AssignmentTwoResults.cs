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

    public class AssignmentTwoResultList<T> : List<AssignmentTwoResults<T>> where T:IGenome
    {
        private string name;

        public AssignmentTwoResultList(string _name)
        {
            name = _name;
        }
        public string MaxTimeString(bool last_line = false)
        {
            return String.Format("{0} & {1} ({2}) & {3} ({4}) & {5} ({6}) & {7} {8}\n",
                name,
                OptimaAmountAverage, OptimaAmountStdDev,
                SingleOptimumTimeAverage, SingleOptimumTimeStdDev,
                ScoreAverage, ScoreStdDev,
                BestScore, 
                !last_line ? "\\\\" : "");
        }
        public string OptimaString(bool last_line = false)
        {
            return String.Format("{0} & {1} ({2}) & {3} ({4}) & {5} ({6}) & {7} {8}\n",
                name,
                TotalTimeAverage, TotalTimeStdDev,
                SingleOptimumTimeAverage, SingleOptimumTimeStdDev,
                ScoreAverage, ScoreStdDev,
                BestScore,
                !last_line ? "\\\\" : "");
        }
        public AssignmentTwoResultList<T> MaxTicksSubList(long max_ticks)
        {
            AssignmentTwoResultList<T> inner_list = new AssignmentTwoResultList<T>(name);
            foreach (AssignmentTwoResults<T> results in this)
            {
                AssignmentTwoResults<T> inner_results = new AssignmentTwoResults<T>();
                long current_ticks = 0;
                foreach (AssignmentTwoResult<T> result in results)
                {
                    current_ticks += result.TicksToReach;
                    if (current_ticks > max_ticks)
                        break;

                    inner_results.Add(result);
                }
                inner_list.Add(inner_results);
            }

            return inner_list;
        }
        
        public double OptimaAmountAverage
        {
            get
            {
                return this.Average(x => x.Count);
            }
        }
        public double OptimaAmountStdDev
        {
            get
            {
                double avg = this.OptimaAmountAverage;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.Count - avg, 2)) / this.Count);
            }
        }

        public long MaxTotalTime
        {
            get
            {
                long max_ticks = -1;
                foreach (AssignmentTwoResults<T> res in this)
                {
                    if (max_ticks < res.TotalTicks)
                        max_ticks = res.TotalTicks;
                }
                return max_ticks;
            }
        }
        public double TotalTimeAverage
        {
            get
            {
                return this.Average(x => x.TotalTicks);
            }
        }
        public double TotalTimeStdDev
        {
            get
            {
                double avg = this.TotalTimeAverage;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.TotalTicks - avg, 2)) / this.Count);
            }
        }
        public double SingleOptimumTimeAverage
        {
            get
            {
                return this.Average(x => x.AverageTicks);
            }
        }
        public double SingleOptimumTimeStdDev
        {
            get
            {
                double avg = this.SingleOptimumTimeAverage;
                double stddev = Math.Sqrt(this.Sum(x => Math.Pow(x.AverageTicks - avg, 2)) / this.Count);
                return stddev;
            }
        }
        public double ScoreAverage
        {
            get
            {
                return this.Average(x => x.AverageOptimum);
            }
        }
        public double ScoreStdDev
        {
            get
            {
                double avg = this.ScoreAverage;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.AverageOptimum - avg, 2)) / this.Count);
            }
        }
        public double BestScore
        {
            get
            {
                double best_score = double.MaxValue;
                foreach(AssignmentTwoResults<T> res in this)
                {
                    foreach(AssignmentTwoResult<T> inner_rest in res)
                    {
                        if (best_score > inner_rest.Optimum.Fitness)
                            best_score = inner_rest.Optimum.Fitness;
                    }
                }
                return best_score;
            }
        }
    }
}
