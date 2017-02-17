using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Results
{

    class Result
    {
        public Result()
        {
            TwoPointCrossoverResults = new ResultMap();
            UniformCrossoverResults = new ResultMap();
        }
        public ResultMap TwoPointCrossoverResults { get; set; }
        public ResultMap UniformCrossoverResults { get; set; }
    }

    // <PopulationSize, TestResults>
    class ResultMap : Dictionary<int, InnerResultList>
    {

    }

    class InnerResultList : List<InnerResult>
    {
        public int Successes
        {
            get
            {
                return this.Sum(x => x.Success ? 1 : 0);
            }
        }
        public double FirstHitGenerationMean
        {
            get
            {
                return this.OrderBy(x => x.FirstHitGeneration)
                    .ElementAt((int)Math.Ceiling((double)(this.Count) / 2.0))
                    .FirstHitGeneration;
            }
        }
        public double FirstHitGenerationStandardDeviation
        {
            get
            {
                double avg = this.Average(x => x.FirstHitGeneration);
                return Math.Sqrt(this.Sum(x => Math.Pow(x.FirstHitGeneration - avg, 2)));
            }
        }

        public double ConvergenceGenerationMean
        {
            get
            {
                return this.OrderBy(x => x.ConvergenceGeneration)
                    .ElementAt((int)Math.Ceiling((double)(this.Count) / 2.0))
                    .ConvergenceGeneration;
            }
        }
        public double ConvergenceGenerationStandardDeviation
        {
            get
            {
                double avg = this.Average(x => x.ConvergenceGeneration);
                return Math.Sqrt(this.Sum(x => Math.Pow(x.ConvergenceGeneration - avg, 2)));
            }
        }

        public double FunctionEvaluationsMean
        {
            get
            {
                return this.OrderBy(x => x.FunctionEvaluations)
                    .ElementAt((int)Math.Ceiling((double)(this.Count) / 2.0))
                    .FunctionEvaluations;
            }
        }
        public double FunctionEvaluationsStandardDeviation
        {
            get
            {
                double avg = this.Average(x => x.FunctionEvaluations);
                return Math.Sqrt(this.Sum(x => Math.Pow(x.FunctionEvaluations - avg, 2)));
            }
        }

        public double CPUTimeMean
        {
            get
            {
                return this.OrderBy(x => x.CPUTime)
                    .ElementAt((int)Math.Ceiling((double)(this.Count) / 2.0))
                    .CPUTime;
            }
        }
        public double CPUTimeStandardDeviation
        {
            get
            {
                double avg = this.Average(x => x.CPUTime);
                return Math.Sqrt(this.Sum(x => Math.Pow(x.CPUTime - avg, 2)));
            }
        }
    }

    class InnerResult
    {
        public bool Success { get; set; }
        public int FirstHitGeneration { get; set; }
        public int ConvergenceGeneration { get; set; }
        public int FunctionEvaluations { get; set; }
        public long CPUTime { get; set; }
    }
}
