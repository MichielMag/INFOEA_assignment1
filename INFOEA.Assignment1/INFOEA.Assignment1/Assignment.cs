using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Results;
using INFOEA.Assignment1.Algorithm;
using INFOEA.Assignment1.Genome;
using INFOEA.Assignment1.Algorithm.Crossover;

namespace INFOEA.Assignment1
{
    class Assignment
    {
        private Random random;
        private int seed;
        private int string_length = 100;

        private const string filename = "results.csv";

        /*
         * Experiments: 1: Uniformly Scaled Counting Ones Function
         *              2: Linearly Scaled Counting Ones Function
         *              3: Deceptive Trap Function (tightly linked)
         *              4: Non-deceptive Trap Function (tightly linked)
         *              5: Deceptive Trap Function (randomly linked)
         *              6: Non-deceptive Trap Function (Randomly linked)
        /* List of: Experiment number, 
                    Tuple of: - Two-Point Crossover results
                              - Uniform crossover results */
        private Dictionary<int, Result> Results;
        private Dictionary<int, string> ExperimentNames;
        private List<int> PopulationSizes;
        
        public Assignment(int _seed = -1)
        {
            if(_seed < 0)
            {
                random = new Random();
                seed = random.Next();
            }
            else
            {
                seed = _seed;
            }

            random = new Random(seed);

            ExperimentNames = new Dictionary<int, string>();
            ExperimentNames.Add(1, "Uniformly Scaled Counting Ones Function");
            ExperimentNames.Add(2, "Linearly Scaled Counting Ones Function");
            ExperimentNames.Add(3, "Deceptive Trap Function (tightly linked)");
            ExperimentNames.Add(4, "Non-deceptive Trap Function (tightly linked)");
            ExperimentNames.Add(5, "Deceptive Trap Function (randomly linked)");
            ExperimentNames.Add(6, "Non-deceptive Trap Function (Randomly linked)");

            Results = new Dictionary<int, Result>();
            foreach (KeyValuePair<int, string> kvp in ExperimentNames)
                Results.Add(kvp.Key, new Result());

            PopulationSizes = new List<int>();
            PopulationSizes.Add(50);
            PopulationSizes.Add(100);
            PopulationSizes.Add(250);
            PopulationSizes.Add(500);
        }

        public void RunExperiments()
        {
            GeneticAlgorithm<UCOGenome> experiment_one_one =
                new GeneticAlgorithm<UCOGenome>(string_length,
                                                new TwoPointCrossover<UCOGenome>(random),
                                                new Goal(100, 100),
                                                random);

            GeneticAlgorithm<LCOGenome> experiment_two_one =
                new GeneticAlgorithm<LCOGenome>(string_length,
                                                new TwoPointCrossover<LCOGenome>(random),
                                                new Goal(100, linear_score(string_length)),
                                                random);

            GeneticAlgorithm<DTTGenome> experiment_three_one =
                new GeneticAlgorithm<DTTGenome>(string_length,
                                                new TwoPointCrossover<DTTGenome>(random),
                                                new Goal(100, 100),
                                                random);

            Results[1].TwoPointCrossoverResults = RunExperiment(experiment_one_one);
            Results[2].TwoPointCrossoverResults = RunExperiment(experiment_two_one);
            Results[3].TwoPointCrossoverResults = RunExperiment(experiment_three_one);

            writeResultsToFile();
        }

        private ResultMap RunExperiment<T>(GeneticAlgorithm<T> alg, int runs = 25) where T:IGenome
        {
            ResultMap res_list = new ResultMap();
            
            foreach(int pop_size in PopulationSizes)
            {
                res_list.Add(pop_size, new InnerResultList());
                for (int i = 0; i < runs; ++i)
                {
                    res_list[pop_size].Add(alg.start(pop_size, true));
                }
            }

            return res_list;
        }

        private void writeResultsToFile()
        {
            string results = "Seed: " + seed + ";;;;;;\n";
            foreach(KeyValuePair<int, string> experiment in ExperimentNames)
            {
                results += String.Format("{0}: {1};;;;;;\n", experiment.Key, experiment.Value);
                results += "Crossover;PopSize;Successes;Of;Gen.(First Hit);Gen.(Convergence);Fct Evals;CPU Time\n";
                Result result = Results[experiment.Key];
                foreach (KeyValuePair<int, InnerResultList> kvp in result.TwoPointCrossoverResults)
                {
                    results += String.Format("{0};{1};{2};25;{3} ({4});{5} ({6});{7} ({8});{9} ({10})\n",
                        "2X",
                        kvp.Key,
                        kvp.Value.Successes,
                        kvp.Value.FirstHitGenerationMean,
                        kvp.Value.FirstHitGenerationStandardDeviation,
                        kvp.Value.ConvergenceGenerationMean,
                        kvp.Value.ConvergenceGenerationStandardDeviation,
                        kvp.Value.FunctionEvaluationsMean,
                        kvp.Value.FunctionEvaluationsStandardDeviation,
                        kvp.Value.CPUTimeMean,
                        kvp.Value.CPUTimeStandardDeviation
                    );
                }
                foreach (KeyValuePair<int, InnerResultList> kvp in result.UniformCrossoverResults)
                {
                    results += String.Format("{0};{1};{2};25;{3} ({4});{5} ({6});{7} ({8});{9} ({10})\n",
                         "UX",
                        kvp.Key,
                        kvp.Value.Successes,
                        kvp.Value.FirstHitGenerationMean,
                        kvp.Value.FirstHitGenerationStandardDeviation,
                        kvp.Value.ConvergenceGenerationMean,
                        kvp.Value.ConvergenceGenerationStandardDeviation,
                        kvp.Value.FunctionEvaluationsMean,
                        kvp.Value.FunctionEvaluationsStandardDeviation,
                        kvp.Value.CPUTimeMean,
                        kvp.Value.CPUTimeStandardDeviation
                    );
                }
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.WriteLine(results);
            file.Close();
        }

        private int linear_score(int length)
        {
            int score = 0;
            for (int i = 1; i < length + 1; ++i)
            {
                score += i;
            }
            return score;
        }
    }
}
