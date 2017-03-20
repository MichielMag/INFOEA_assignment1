using INFOEA.Algorithm.Genome;
using INFOEA.Algorithm.Neighborhood;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Algorithm
{
    public class LocalSearch<T> where T:IGenome
    {
        private INeighborhood<T> neighborhood;
        private IComparer<T> comparer;

        private Random random;

        public LocalSearch(int _data_size, INeighborhood<T> _neighborhood, IComparer<T> _comparer, Random _random)
        {
            neighborhood = _neighborhood;
            comparer = _comparer;

            random = _random;
        }

        public T Search(T solution, bool silent=true)
        {
            if(!silent)
                Console.WriteLine("Starting Search, current optimum is {0}", solution.Fitness);
            foreach(T t in neighborhood.Neighbors(solution))
            {
                if (comparer.Compare(t, solution) > 0)
                    return Search(t);
            }
            if(!silent)
                Console.WriteLine("Reached local optimum with score of {0}", solution.Fitness);
            return solution;
        }
    }
}
