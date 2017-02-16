using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Algorithm
{
    class Goal
    {
        private int max_generations;
        private int min_fitness;

        public Goal(int _max_generations, int _min_fitness)
        {
            max_generations = _max_generations;
            min_fitness = _min_fitness;
        }

        public bool AchievedGoal(int generation, int fitness)
        {
            return generation >= max_generations || fitness >= min_fitness;
        }
    }
}
