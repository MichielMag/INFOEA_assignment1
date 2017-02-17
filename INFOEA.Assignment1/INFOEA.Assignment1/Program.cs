using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INFOEA.Assignment1.Algorithm;
using INFOEA.Assignment1.Genome;
using INFOEA.Assignment1.Algorithm.Crossover;

namespace INFOEA.Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            int seed = -1;
            if(args.Count() > 0)
            {
                foreach(string arg in args)
                {
                    try
                    {
                        if (arg.Contains("-seed=")) // Seed?
                            seed = int.Parse(arg.Split('=')[1]);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Could not parse commandline arguments.");
                    }
                }
            }

            Assignment assignment = new Assignment(seed);
            assignment.RunExperiments();

            int string_length = 100;





            
            //experiment_three_one.start();
        }
    }
}
