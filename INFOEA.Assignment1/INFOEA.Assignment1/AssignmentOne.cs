using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Algorithm.Results;
using INFOEA.Algorithm.Algorithm;
using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Crossover;
using INFOEA.Algorithm.Comparer;
using INFOEA.Algorithm.Selector;
using INFOEA.Algorithm.Procreation;

namespace INFOEA.Assignment1
{
    class AssignmentOne
    {
        private Random random;
        private int seed;
        private int string_length = 100;

        private const string filename = "results.csv";
        private const string tex_filename = "results-latex.txt";

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
        
        public AssignmentOne(int _seed = -1)
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
                                                new DefaultProcreator<UCOGenome>(new TwoPointCrossover<UCOGenome>(random)),
                                                new DefaultSelector<UCOGenome>(new DefaultComparer<UCOGenome>()),
                                                new Goal(100, 100),
                                                random, "2X");

            GeneticAlgorithm<UCOGenome> experiment_one_two =
                new GeneticAlgorithm<UCOGenome>(string_length,
                                                new DefaultProcreator<UCOGenome>(new UniformCrossover<UCOGenome>(random)),
                                                new DefaultSelector<UCOGenome>(new DefaultComparer<UCOGenome>()),
                                                new Goal(100, 100),
                                                random, "UX");

            GeneticAlgorithm<LCOGenome> experiment_two_one =
                new GeneticAlgorithm<LCOGenome>(string_length,
                                                new DefaultProcreator<LCOGenome>(new TwoPointCrossover<LCOGenome>(random)),
                                                new DefaultSelector<LCOGenome>(new DefaultComparer<LCOGenome>()),
                                                new Goal(100, linear_score(string_length)),
                                                random, "2X");

            GeneticAlgorithm<LCOGenome> experiment_two_two =
                new GeneticAlgorithm<LCOGenome>(string_length,
                                                new DefaultProcreator<LCOGenome>(new UniformCrossover<LCOGenome>(random)),
                                                new DefaultSelector<LCOGenome>(new DefaultComparer<LCOGenome>()),
                                                new Goal(100, linear_score(string_length)),
                                                random, "UX");

            GeneticAlgorithm<DTTGenome> experiment_three_one =
                new GeneticAlgorithm<DTTGenome>(string_length,
                                                new DefaultProcreator<DTTGenome>(new TwoPointCrossover<DTTGenome>(random)),
                                                new DefaultSelector<DTTGenome>(new DefaultComparer<DTTGenome>()),
                                                new Goal(100, 100),
                                                random, "2X");

            GeneticAlgorithm<DTTGenome> experiment_three_two =
                new GeneticAlgorithm<DTTGenome>(string_length,
                                                new DefaultProcreator<DTTGenome>(new UniformCrossover<DTTGenome>(random)),
                                                new DefaultSelector<DTTGenome>(new DefaultComparer<DTTGenome>()),
                                                new Goal(100, 100),
                                                random, "UX");

            GeneticAlgorithm<NDTTGenome> experiment_four_one =
                new GeneticAlgorithm<NDTTGenome>(string_length,
                                                new DefaultProcreator<NDTTGenome>(new TwoPointCrossover<NDTTGenome>(random)),
                                                new DefaultSelector<NDTTGenome>(new DefaultComparer<NDTTGenome>()),
                                                new Goal(100, 100),
                                                random, "2X");

            GeneticAlgorithm<NDTTGenome> experiment_four_two =
                new GeneticAlgorithm<NDTTGenome>(string_length,
                                                new DefaultProcreator<NDTTGenome>(new UniformCrossover<NDTTGenome>(random)),
                                                new DefaultSelector<NDTTGenome>(new DefaultComparer<NDTTGenome>()),
                                                new Goal(100, 100),
                                                random, "UX");

            GeneticAlgorithm<DTRGenome> experiment_five_one =
                new GeneticAlgorithm<DTRGenome>(string_length,
                                                new DefaultProcreator<DTRGenome>(new TwoPointCrossover<DTRGenome>(random)),
                                                new DefaultSelector<DTRGenome>(new DefaultComparer<DTRGenome>()),
                                                new Goal(100, 100),
                                                random, "2X");

            GeneticAlgorithm<DTRGenome> experiment_five_two =
                 new GeneticAlgorithm<DTRGenome>(string_length,
                                                new DefaultProcreator<DTRGenome>(new UniformCrossover<DTRGenome>(random)),
                                                new DefaultSelector<DTRGenome>(new DefaultComparer<DTRGenome>()),
                                                new Goal(100, 100),
                                                random, "UX");

            GeneticAlgorithm<NDTRGenome> experiment_six_one =
    new GeneticAlgorithm<NDTRGenome>(string_length,
                                                new DefaultProcreator<NDTRGenome>(new TwoPointCrossover<NDTRGenome>(random)),
                                                new DefaultSelector<NDTRGenome>(new DefaultComparer<NDTRGenome>()),
                                                new Goal(100, 100),
                                                random, "2X");

            GeneticAlgorithm<NDTRGenome> experiment_six_two =
                 new GeneticAlgorithm<NDTRGenome>(string_length,
                                                new DefaultProcreator<NDTRGenome>(new UniformCrossover<NDTRGenome>(random)),
                                                new DefaultSelector<NDTRGenome>(new DefaultComparer<NDTRGenome>()),
                                                new Goal(100, 100),
                                                random, "UX");

            Results[1].TwoPointCrossoverResults = RunExperiment(experiment_one_one);
            Results[2].TwoPointCrossoverResults = RunExperiment(experiment_two_one);
            Results[3].TwoPointCrossoverResults = RunExperiment(experiment_three_one);
            Results[4].TwoPointCrossoverResults = RunExperiment(experiment_four_one);
            Results[5].TwoPointCrossoverResults = RunExperiment(experiment_five_one);
            Results[6].TwoPointCrossoverResults = RunExperiment(experiment_six_one);

            Results[1].UniformCrossoverResults = RunExperiment(experiment_one_two);
            Results[2].UniformCrossoverResults = RunExperiment(experiment_two_two);
            Results[3].UniformCrossoverResults = RunExperiment(experiment_three_two);
            Results[4].UniformCrossoverResults = RunExperiment(experiment_four_two);
            Results[5].UniformCrossoverResults = RunExperiment(experiment_five_two);
            Results[6].UniformCrossoverResults = RunExperiment(experiment_six_two);

            writeResultsToFiles();
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

        private void writeResultsToFiles()
        {
            string results = "Seed: " + seed + ";;;;;;\n";
            string tex_results = "";
            Dictionary<string, string> experiment_results = new Dictionary<string, string>();
            foreach(KeyValuePair<int, string> experiment in ExperimentNames)
            {
                string experiment_key = experiment.Key + " - " + experiment.Value;

                experiment_results.Add(experiment_key, "");
                experiment_results[experiment_key] += String.Format("{0}: {1};;;;;;\n", experiment.Key, experiment.Value);
                experiment_results[experiment_key] += "Crossover;PopSize;Success;Gen.(First Hit);Gen.(Convergence);Fct Evals;CPU Time;Best Score;Best Solution;\n";

                results += String.Format("{0}: {1};;;;;;\n", experiment.Key, experiment.Value);
                results += "Crossover;PopSize;Successes;Of;Gen.(First Hit);Gen.(Convergence);Fct Evals;CPU Time;Best Score\n";

                tex_results += "\\begin{table}[]\n\\centering\n\\caption{" + experiment.Key + ": " + experiment.Value + "}\n\\label{" + experiment.Key + ": " + experiment.Value + "}\n\\begin{tabular}{lllllllll}\n";
                tex_results += "Crossover & PopSize & Successes & Of & Gen.(First Hit) & Gen.(Convergence) & Fct Evals & CPU Time & Best Score\\\\\n";

                Result result = Results[experiment.Key];
                foreach (KeyValuePair<int, InnerResultList> kvp in result.TwoPointCrossoverResults)
                {
                    results += String.Format("{0};{1};{2};25;{3} ({4});{5} ({6});{7} ({8});{9} ({10});{11} ({12})\n",
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
                        kvp.Value.CPUTimeStandardDeviation,
                        kvp.Value.BestScoreMean,
                        kvp.Value.BestScoreStandardDeviation
                    );

                    tex_results += String.Format("{0} & {1} & {2} & 25 & {3} ({4}) & {5} ({6}) & {7} ({8}) & {9} ({10}) & {11} ({12})\\\\\n",
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
                        kvp.Value.CPUTimeStandardDeviation,
                        kvp.Value.BestScoreMean,
                        kvp.Value.BestScoreStandardDeviation
                    );

                    foreach (InnerResult inner_result in kvp.Value)
                    {
                        experiment_results[experiment_key] += String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}\n",
                            "2X",
                            kvp.Key,
                            inner_result.Success ? "1" : "0",
                            inner_result.FirstHitGeneration,
                            inner_result.ConvergenceGeneration,
                            inner_result.FunctionEvaluations,
                            inner_result.CPUTime,
                            inner_result.BestScore,
                            inner_result.BestSolution
                        );
                    }
                }
                foreach (KeyValuePair<int, InnerResultList> kvp in result.UniformCrossoverResults)
                {
                    results += String.Format("{0};{1};{2};25;{3} ({4});{5} ({6});{7} ({8});{9} ({10});{11} ({12})\n",
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
                        kvp.Value.CPUTimeStandardDeviation,
                        kvp.Value.BestScoreMean,
                        kvp.Value.BestScoreStandardDeviation
                    );

                    tex_results += String.Format("{0} & {1} & {2} & 25 & {3} ({4}) & {5} ({6}) & {7} ({8}) & {9} ({10}) & {11} ({12})\\\\\n",
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
                        kvp.Value.CPUTimeStandardDeviation,
                        kvp.Value.BestScoreMean,
                        kvp.Value.BestScoreStandardDeviation
                    );
                    foreach (InnerResult inner_result in kvp.Value)
                    {
                        experiment_results[experiment_key] += String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}\n",
                            "UX",
                            kvp.Key,
                            inner_result.Success ? "1" : "0",
                            inner_result.FirstHitGeneration,
                            inner_result.ConvergenceGeneration,
                            inner_result.FunctionEvaluations,
                            inner_result.CPUTime,
                            inner_result.BestScore,
                            inner_result.BestSolution
                        );
                    }
                }
                tex_results = tex_results.Substring(0, tex_results.Length - 3) + "\n";
                tex_results += "\\end{tabular}\n\\end{table}\n";
            }

            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.WriteLine(results);
            file.Close();

            file = new System.IO.StreamWriter(tex_filename);
            file.WriteLine(tex_results);
            file.Close();

            foreach (KeyValuePair<string, string> kvp in experiment_results)
            {
                System.IO.StreamWriter f = new System.IO.StreamWriter(kvp.Key+".csv");
                f.WriteLine(kvp.Value);
                f.Close();
            }
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
