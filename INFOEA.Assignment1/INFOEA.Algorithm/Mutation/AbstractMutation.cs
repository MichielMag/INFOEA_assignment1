using INFOEA.Algorithm.Genome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Mutation
{
    public abstract class AbstractMutation<T> : IMutation<T> where T : IGenome
    {
        protected Random random_source;
        private string name;

        public string Name { get { return name; } }

        public AbstractMutation(Random random, string _name)
        {
            random_source = random;
            name = _name;
        }

        public abstract T Mutate(T solution);
    }
}
