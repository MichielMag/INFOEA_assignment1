﻿using INFOEA.Algorithm.Algorithm;
using INFOEA.Algorithm.Comparer;
using INFOEA.Algorithm.Crossover;
using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Genome.Graph;
using INFOEA.Algorithm.Mutation;
using INFOEA.Algorithm.Neighborhood;
using INFOEA.Algorithm.PopulationGeneration;
using INFOEA.Algorithm.Procreation;
using INFOEA.Algorithm.Results;
using INFOEA.Algorithm.Selector;
using INFOEA.Algoritmh.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace INFOEA.Assignment2
{
    class AssignmentTwo
    {
        public int OptimaAmount { get; set; } = 250;
        public int ExperimentAmount { get; set; } = 3;

        //private Random[] random;

        private const int nOfProc = 1;
        private Thread[] threads = new Thread[nOfProc];
        private GraphGenome[] solutionsThreads = new GraphGenome[nOfProc];
        private List<AssignmentTwoResults<GraphGenome>>[] elapsedMilisecondsThreads = new List<AssignmentTwoResults<GraphGenome>>[nOfProc];

        private Random main_random_source;

        private void searchThread(int pID, int max, int seed, INeighborhood<GraphGenome> neighborhood, bool silent = false)
        {
            Console.WriteLine("Thread {0} started...", pID);

            Random random = new Random(seed);

            // TODO: Resultaten (dus CPU computational time) opslaan in bestand.
            LocalSearch<GraphGenome> local_search =
                new LocalSearch<GraphGenome>(500,
                        neighborhood,
                        new GraphComparer<GraphGenome>(), random);

            // Dit wil ik eigenlijk static doen, maar dan komen we in de knoei met data_size.
            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");

            GraphGenome optimum = null;

            for (int i = 0; i < ExperimentAmount; ++i)
            {
                AssignmentTwoResults<GraphGenome> inner_results = new AssignmentTwoResults<GraphGenome>();
                for (int j = 0; j < max; j++)
                {
                    graph = new GraphGenome(500);
                    graph.Generate(ref random);
                    Stopwatch sw = Stopwatch.StartNew();
                    GraphGenome inner_optimum = local_search.Search(graph);
                    sw.Stop();
                    inner_results.Add(inner_optimum, sw.ElapsedTicks);

                    if (!silent)
                        Console.WriteLine("Found {0} in {1} ticks or {2}ms. ({3}/{4}), pID: {5}", inner_optimum.Fitness, sw.ElapsedTicks,
                            sw.ElapsedMilliseconds, j, max, pID);

                    if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                        optimum = inner_optimum;
                }
                lock (solutionsThreads)
                {
                    solutionsThreads[pID] = optimum;
                }
                lock (elapsedMilisecondsThreads)
                {
                    elapsedMilisecondsThreads[pID].Add(inner_results);
                }
            }
        }

        private bool threadsAreRunning()
        {
            foreach (Thread T in threads)
            {
                if (T.ThreadState == System.Threading.ThreadState.Running) return true;
            }
            return false;
        }

        private AssignmentTwoResultList<GraphGenome> MultiStartLocalSearch(INeighborhood<GraphGenome> neighborhood)
        {
            //we moeten nog iets met ExperimentAmount doen...
            //for(int i = 0; i < ExperimentAmount; ++i)

            AssignmentTwoResultList<GraphGenome> results = new AssignmentTwoResultList<GraphGenome>("$MLS_{" + neighborhood.Name + "}$");

            for (int j = 0; j < threads.Length; j++)
            {
                elapsedMilisecondsThreads[j] = new List<AssignmentTwoResults<GraphGenome>>();
                int k = j;
                int seed = main_random_source.Next();
                int amount = OptimaAmount / threads.Length;
                Thread T = new Thread(() => searchThread(k, amount, seed, neighborhood));
                threads[j] = T;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (Thread T in threads)
                T.Start();
            while (this.threadsAreRunning()) ;
            sw.Stop();

            GraphGenome optimum = null;
            for (int j = 0; j < threads.Length; j++)
            {
                if (optimum == null || optimum.Fitness > solutionsThreads[j].Fitness)
                    optimum = solutionsThreads[j];
            }

            //List<AssignmentTwoResults> results = new List<AssignmentTwoResults>();
            foreach (List<AssignmentTwoResults<GraphGenome>> L in elapsedMilisecondsThreads)
            {
                results.AddRange(L);
            }

            //Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, cpu_ticks.Average());
            Console.WriteLine("Best solution found: {0} in an average of ... ms", optimum.Fitness); //, results.Sum() / OptimaAmount);
            Console.WriteLine("Total time: {0} sec", sw.ElapsedMilliseconds / 1000f);

            optimum.ToImage(String.Format("results/MLS[{0}]-{1}.bmp", neighborhood.Name, optimum.Fitness), 3000, 3000);

            return results;
        }

        private AssignmentTwoResultList<GraphGenome> IteratedLocalSearch(INeighborhood<GraphGenome> neighborhood, bool silent = false)
        {
            LocalSearch<GraphGenome> local_search =
                new LocalSearch<GraphGenome>(500,
                        neighborhood,
                        new GraphComparer<GraphGenome>(), main_random_source);
            IMutation<GraphGenome> pertubation = new ILSPertubation<GraphGenome>(main_random_source);

            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");

            GraphGenome optimum = null;

            List<long> elapsedMilisecondsList = new List<long>();

            AssignmentTwoResultList< GraphGenome> results = new AssignmentTwoResultList<GraphGenome>("$ILS_{" + neighborhood.Name + "}$");

            for (int i = 0; i < ExperimentAmount; ++i)
            {
                AssignmentTwoResults<GraphGenome> res = new AssignmentTwoResults<GraphGenome>();
                for (int j = 0; j < OptimaAmount; j++)
                {
                    if (optimum == null)
                    {
                        graph = new GraphGenome(500);
                        graph.Generate(ref main_random_source);
                    }
                    else
                    {
                        graph = pertubation.Mutate(optimum);
                    }

                    Stopwatch sw = Stopwatch.StartNew();
                    GraphGenome inner_optimum = local_search.Search(graph);
                    sw.Stop();
                    elapsedMilisecondsList.Add(sw.ElapsedMilliseconds);

                    res.Add(inner_optimum, sw.ElapsedTicks);

                    if (!silent)
                        Console.WriteLine("Found {0} in {1} ticks or {2}ms. ({3}/{4})", inner_optimum.Fitness, sw.ElapsedTicks,
                            sw.ElapsedMilliseconds, j, OptimaAmount);

                    if (optimum == null || optimum.Fitness >= inner_optimum.Fitness)
                        optimum = inner_optimum;

                    optimum.FunctionEvaluations = 0;
                }
                results.Add(res);
            }
            //Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, cpu_ticks.Average());
            Console.WriteLine("Best solution found: {0} in an average of {1} ms", optimum.Fitness, elapsedMilisecondsList.Sum() / OptimaAmount);
            //Console.WriteLine("Total time: {0} sec", sw.ElapsedMilliseconds / 1000f);

            optimum.ToImage(String.Format("results/ILS[{0}]-{1}.bmp", neighborhood.Name, optimum.Fitness), 3000, 3000);

            //Console.ReadLine();
            return results;
        }

        private AssignmentTwoResultList<GraphGenome> GeneticLocalSearch(INeighborhood<GraphGenome> neighborhood)
        {
            AssignmentTwoResultList<GraphGenome> results = new AssignmentTwoResultList<GraphGenome>("$GLS_{" + neighborhood.Name + "}$");

            GraphGenome graph = new GraphGenome(500);
            graph.CreateGraph("Graph500.txt");

            GraphGenome optimum = null;

            for (int i = 0; i < ExperimentAmount; ++i)
            {
                AssignmentTwoResults<GraphGenome> res = new AssignmentTwoResults<GraphGenome>();
                int population_size = 50;
                int data_size = 500;

                LocalSearch<GraphGenome> local_search =
                    new LocalSearch<GraphGenome>(data_size,
                            neighborhood,
                            new GraphComparer<GraphGenome>(), main_random_source);

                LocalSearchProcreator<GraphGenome> lsp = new LocalSearchProcreator<GraphGenome>(
                    new UniformSymmetricCrossover<GraphGenome>(main_random_source),
                    local_search,
                    main_random_source,
                    res);

                GeneticAlgorithm<GraphGenome> ga = new GeneticAlgorithm<GraphGenome>(
                    data_size,
                    lsp,
                    new DefaultSelector<GraphGenome>(
                        new GraphComparer<GraphGenome>()),
                    new LocalSearchPopulationGenerator<GraphGenome>(main_random_source, local_search),
                    new Goal(100, 0),
                    main_random_source,
                    "GLS");

                InnerResult ir = ga.start(population_size, OptimaAmount / (population_size / 2)); 
                // Eventueel nog een keer uitvoeren om tot 2500 optima te komen.
                while (res.Count < OptimaAmount)
                    ir = ga.start(population_size, OptimaAmount / (population_size / 2));

                // Als we er teveel hebben gekregen door de GLS run meerdere keren uit te voeren,
                // pak enkel hoeveel we nodig hebben.
                res = res.TakeFirstN(OptimaAmount);
                results.Add(res);

                if (optimum == null || res.BestResult.Optimum.Fitness < optimum.Fitness)
                    optimum = new GraphGenome(res.BestResult.Optimum.Data, res.BestResult.Optimum.Fitness);
            }




            // We maken iedere keer population_size / 2 optima.
            // We willen OptimaAmount optima. Dus we gaan OptimaAmount / (population_size / 2) generaties uitvoeren.

            //IteratedLocalSearch();

            optimum.ToImage(String.Format("results/GLS[{0}]-{1}.bmp", neighborhood.Name, optimum.Fitness), 3000, 3000);

            return results;
        }

        public void start(int seed = -1)
        {
            System.IO.Directory.CreateDirectory("results");
            if (seed > 0)
                main_random_source = new Random(seed);
            else
                main_random_source = new Random();

            INeighborhood<GraphGenome> swap_neighborhood = new SwapNeighborhood<GraphGenome>(main_random_source);
            INeighborhood<GraphGenome> FM_neigborhood = new FiducciaMatheysesNeighborhood<GraphGenome>(main_random_source, 7);

            // Dan de Fiduccia experimenten:
            AssignmentTwoResultList<GraphGenome> ils_FM_results = IteratedLocalSearch(FM_neigborhood);
            AssignmentTwoResultList<GraphGenome> mls_FM_results = MultiStartLocalSearch(FM_neigborhood);
            AssignmentTwoResultList<GraphGenome> gls_FM_results = GeneticLocalSearch(FM_neigborhood);

            // Eerst de "gewone" experimenten:
            AssignmentTwoResultList<GraphGenome> mls_results = MultiStartLocalSearch(swap_neighborhood);
            AssignmentTwoResultList<GraphGenome> ils_results = IteratedLocalSearch(swap_neighborhood);
            AssignmentTwoResultList<GraphGenome> gls_results = GeneticLocalSearch(swap_neighborhood);

            string tex_results = "";

            tex_results += "\\begin{table}[H]\n\\centering\n\\caption{Comparing 2500 local optima}\n\\label{Comparing 2500 local optima}\n\\begin{tabular}{|l|l|l|l|l|l|l|l|l|}\n";
            tex_results += "\\hline\n";
            tex_results += "Experiment & Avg. time total & Avg. time 1 optimum & Avg. score & Best score \\\\\n";
            tex_results += "\\hline\n";
            tex_results += mls_results.OptimaString();
            tex_results += ils_results.OptimaString();
            tex_results += gls_results.OptimaString();
            tex_results += "\\hline\n";
            tex_results += mls_FM_results.OptimaString();
            tex_results += ils_FM_results.OptimaString();
            tex_results += gls_FM_results.OptimaString();
            tex_results += "\\hline\n";
            tex_results += "\\end{tabular}\n\\end{table}\n";

            long max_ticks = gls_results.MinTotalTicks;
            tex_results += "\n\n\\begin{table}[H]\n\\centering\n\\caption{Comparing max time of " + max_ticks + " ticks}\n\\label{Comparing max time of " + max_ticks + " ticks}\n\\begin{tabular}{|l|l|l|l|l|l|l|l|l|}\n";
            tex_results += "\\hline\n";
            tex_results += "Experiment & Avg. \\# optima & Avg. time 1 optimum & Avg. score & Best score \\\\\n";
            tex_results += "\\hline\n";
            tex_results += mls_results.MaxTicksSubList(max_ticks).MaxTimeString();
            tex_results += ils_results.MaxTicksSubList(max_ticks).MaxTimeString();
            tex_results += gls_results.MaxTicksSubList(max_ticks).MaxTimeString();
            tex_results += "\\hline\n";
            tex_results += mls_FM_results.MaxTicksSubList(max_ticks).MaxTimeString();
            tex_results += ils_FM_results.MaxTicksSubList(max_ticks).MaxTimeString();
            tex_results += gls_FM_results.MaxTicksSubList(max_ticks).MaxTimeString();
            tex_results += "\\hline\n";
            tex_results += "\\end{tabular}\n\\end{table}\n";

            System.IO.StreamWriter file = new System.IO.StreamWriter("results/mls_swap.csv");
            file.WriteLine(mls_results.CSV());
            file.Close();
            file = new System.IO.StreamWriter("results/mls_fm_swap.csv");
            file.WriteLine(mls_FM_results.CSV());
            file.Close();

            file = new System.IO.StreamWriter("results/ils_swap.csv");
            file.WriteLine(ils_results.CSV());
            file.Close();
            file = new System.IO.StreamWriter("results/ils_fm_swap.csv");
            file.WriteLine(ils_FM_results.CSV());
            file.Close();

            file = new System.IO.StreamWriter("results/gls_swap.csv");
            file.WriteLine(gls_results.CSV());
            file.Close();
            file = new System.IO.StreamWriter("results/gls_fm_swap.csv");
            file.WriteLine(gls_FM_results.CSV());
            file.Close();

            double mls_ils_t = t_score(mls_results, ils_results);
            double mls_gls_t = t_score(mls_results, gls_results);
            double ils_gls_t = t_score(ils_results, gls_results);

            double mls_fm_ils_t = t_score(mls_FM_results, ils_FM_results);
            double mls_fm_gls_t = t_score(mls_FM_results, gls_FM_results);
            double ils_fm_gls_t = t_score(ils_FM_results, gls_FM_results);

            double mls_mls_t = t_score(mls_results, mls_FM_results);
            double ils_ils_t = t_score(ils_results, ils_FM_results);
            double gls_gls_t = t_score(gls_results, gls_FM_results);

            tex_results += "\n\n\\begin{table}[H]\n\\centering\n\\caption{Significance of difference between experiments of swap-neighborhood}\n\\label{Significance of difference between experiments of swap-neighborhood}\n\\begin{tabular}{|l|l|l|l|l|l|l|l|l|}\n";
            tex_results += "\\hline\n";
            tex_results += "Experiment 1 & Experiment 2 & DF & t-score & p-score & significant? \\\\\n";
            tex_results += "\\hline\n";
            tex_results += "$MLS_{SWAP}$ & $ILS_{SWAP}$ & " + (ExperimentAmount - 1) + " & " + mls_ils_t.ToString() + " & p & x \\\\\n";
            tex_results += "$MLS_{SWAP}$ & $GLS_{SWAP}$ & " + (ExperimentAmount - 1) + " & " + mls_gls_t.ToString() + " & p & x \\\\\n";
            tex_results += "$ILS_{SWAP}$ & $GLS_{SWAP}$ & " + (ExperimentAmount - 1) + " & " + ils_gls_t.ToString() + " & p & x \\\\\n";
            tex_results += "\\hline\n";
            tex_results += "\\end{tabular}\n\\end{table}\n";

            tex_results += "\n\n\\begin{table}[H]\n\\centering\n\\caption{Significance of difference between experiments of FM-neighborhood}\n\\label{Significance of difference between experiments of FM-neighborhood}\n\\begin{tabular}{|l|l|l|l|l|l|l|l|l|}\n";
            tex_results += "\\hline\n";
            tex_results += "Experiment 1 & Experiment 2 & DF & t-score & p-score & significant? \\\\\n";
            tex_results += "\\hline\n";
            tex_results += "$MLS_{FM}$ & $ILS_{FM}$ & " + (ExperimentAmount - 1) + " & " + mls_fm_ils_t.ToString() + " & p & x \\\\\n";
            tex_results += "$MLS_{FM}$ & $GLS_{FM}$ & " + (ExperimentAmount - 1) + " & " + mls_fm_gls_t.ToString() + " & p & x \\\\\n";
            tex_results += "$ILS_{FM}$ & $GLS_{FM}$ & " + (ExperimentAmount - 1) + " & " + ils_fm_gls_t.ToString() + " & p & x \\\\\n";
            tex_results += "\\hline\n";
            tex_results += "\\end{tabular}\n\\end{table}\n";

            tex_results += "\n\n\\begin{table}[H]\n\\centering\n\\caption{Significance of difference between neighborhoods}\n\\label{Significance of difference between neighborhoods}\n\\begin{tabular}{|l|l|l|l|l|l|l|l|l|}\n";
            tex_results += "\\hline\n";
            tex_results += "Experiment 1 & Experiment 2 & DF & t-score & p-score & significant? \\\\\n";
            tex_results += "\\hline\n";
            tex_results += "$MLS_{SWAP}$ & $MLS_{FM}$ & " + (ExperimentAmount - 1) + " & " + mls_mls_t.ToString() + " & p & x \\\\\n";
            tex_results += "$ILS_{SWAP}$ & $ILS_{FM}$ & " + (ExperimentAmount - 1) + " & " + ils_ils_t.ToString() + " & p & x \\\\\n";
            tex_results += "$GLS_{SWAP}$ & $GLS_{FM}$ & " + (ExperimentAmount - 1) + " & " + gls_gls_t.ToString() + " & p & x \\\\\n";
            tex_results += "\\hline\n";
            tex_results += "\\end{tabular}\n\\end{table}\n";

            string filename = "results/assignment2results.txt";
            file = new System.IO.StreamWriter(filename);
            file.WriteLine(tex_results);
            file.Close();
        }



        private void WriteStatistics(List<AssignmentTwoResults<GraphGenome>> mls_results,
                                     List<AssignmentTwoResults<GraphGenome>> ils_results,
                                     List<AssignmentTwoResults<GraphGenome>> gls_results)
        {
            double mls_ils_t = t_score(mls_results, ils_results);
            double mls_gls_t = t_score(mls_results, gls_results);
            double ils_gls_t = t_score(ils_results, gls_results);
        }

        private double t_score(List<AssignmentTwoResults<GraphGenome>> first, List<AssignmentTwoResults<GraphGenome>> second)
        {
            int n = ExperimentAmount;
            // Eerst van MLS <-> ILS
            List<long> differences = new List<long>();
            for (int i = 0; i < n; ++i)
                differences.Add(first[i].TotalTicks - second[i].TotalTicks);

            double sample_mean = differences.Average();
            double std_dev = Math.Sqrt(differences.Sum(d => Math.Pow((d - sample_mean), 2)) / (n - 1));
            double t_score = (sample_mean - 0) / (std_dev / Math.Sqrt(n));

            return t_score;
        }


    }
}
