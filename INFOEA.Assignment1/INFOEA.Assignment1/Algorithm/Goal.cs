using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Assignment1.Algorithm
{
    class Goal
    {
        public int MaxGenerations { get; private set; }
        public float MinFitness { get; private set; }

        public Goal(int _max_generations, float _min_fitness)
        {
            MaxGenerations = _max_generations;
            MinFitness = _min_fitness;
        }

        public bool AchievedGoal(int generation, float fitness)
        {
            return generation >= MaxGenerations || fitness >= MinFitness;
        }

        public bool AchievedFitnessGoal(float fitness)
        {
            return fitness >= MinFitness;
        }
    }
}
