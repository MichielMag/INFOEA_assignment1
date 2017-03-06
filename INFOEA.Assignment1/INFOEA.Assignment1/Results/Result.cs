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
                return this.Average(x => x.FirstHitGeneration);
            }
        }
        public double FirstHitGenerationStandardDeviation
        {
            get
            {
                double avg = this.FirstHitGenerationMean;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.FirstHitGeneration - avg, 2)) / this.Count);
            }
        }

        public double ConvergenceGenerationMean
        {
            get
            {
                return this.Average(x => x.ConvergenceGeneration);
            }
        }
        public double ConvergenceGenerationStandardDeviation
        {
            get
            {
                double avg = this.ConvergenceGenerationMean;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.ConvergenceGeneration - avg, 2)) / this.Count);
            }
        }

        public double FunctionEvaluationsMean
        {
            get
            {
                return this.Average(x => x.FunctionEvaluations);
            }
        }
        public double FunctionEvaluationsStandardDeviation
        {
            get
            {
                double avg = this.FunctionEvaluationsMean;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.FunctionEvaluations - avg, 2)) / this.Count);
            }
        }

        public double CPUTimeMean
        {
            get
            {
                return this.Average(x => x.CPUTime);
            }
        }
        public double CPUTimeStandardDeviation
        {
            get
            {
                double avg = this.CPUTimeMean;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.CPUTime - avg, 2)) / this.Count);
            }
        }
        public double BestScoreMean
        {
            get
            {
                return this.Average(x => x.BestScore);
            }
        }
        public double BestScoreStandardDeviation
        {
            get
            {
                double avg = this.BestScoreMean;
                return Math.Sqrt(this.Sum(x => Math.Pow(x.BestScore - avg, 2)) / this.Count);
            }
        }

    }

    class InnerResult
    {
        public bool Success { get; set; }
        public float BestScore { get; set; }
        public int FirstHitGeneration { get; set; }
        public int ConvergenceGeneration { get; set; }
        public long FunctionEvaluations { get; set; }
        public long CPUTime { get; set; }
        public string BestSolution { get; set; }
    }
}
