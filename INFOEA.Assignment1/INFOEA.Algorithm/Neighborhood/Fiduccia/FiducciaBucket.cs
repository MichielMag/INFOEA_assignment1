using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Neighborhood.Fiduccia
{
    class VertexSwap
    {
        public VertexSwap(int _id, char _swap_to, int gain)
        {
            id = _id;
            swap_to = _swap_to;
        }
        public char swap_to;
        public int id;
        public bool free = true;
        public int gain;
    }

    class FiducciaBucket
    {
        private Dictionary<int, int> positions = new Dictionary<int, int>();
        private Dictionary<int, List<VertexSwap>> swaps = new Dictionary<int, List<VertexSwap>>();

        public FiducciaBucket(int degree)
        {
            for(int i = -degree; i < degree + 1; ++i)
            {
                swaps.Add(i, new List<VertexSwap>());
            }
        }
        
        public void AddSwap(int gain, int id, char swap_to)
        {
            if (!swaps.ContainsKey(gain))
                throw new ArgumentOutOfRangeException("Gain either too high or too low for the degree");

            positions.Add(id, gain);
            swaps[gain].Add(new VertexSwap(id, swap_to, gain));
        }

        public VertexSwap GetSwap(int id)
        {
            VertexSwap to_return = null;
            if(positions.ContainsKey(id))
            {
                to_return = swaps[positions[id]].FirstOrDefault(x => x.id == id);
            }
            return to_return;
        }

        public void EditSwap(int id, char new_neighbor)
        {
            if (!positions.ContainsKey(id))
                throw new ArgumentOutOfRangeException("This id is not in the list of positions");

            int old_gain = positions[id];
            int new_gain = old_gain;
            VertexSwap swap = swaps[old_gain].FirstOrDefault(x => x.id == id);

            // Swap to is het tegenovergestelde van wat hij eigenlijk is.
            // dus wanneer deze equal zijn "Ik ga naar 0, dan is mijn gain x, 
            // maar ik heb een nieuwe neighbor die is al 0 ipv 1 wat hij eerst was, 
            // dat betekent dat mijn gain 1 minder wordt"
            if (swap.swap_to == new_neighbor)
                new_gain--;
            else
                new_gain++;

            if (!swaps.ContainsKey(new_gain))
                throw new ArgumentOutOfRangeException("You can't add this new gain");

            swap.gain = new_gain;

            swaps[old_gain].Remove(swap);
            swaps[new_gain].Add(swap);

            positions[id] = new_gain;
        }

        public void EditSwap(int id, int new_gain)
        {
            if (!swaps.ContainsKey(new_gain))
                throw new ArgumentOutOfRangeException("You can't add this new gain");

            if (!positions.ContainsKey(id))
                throw new ArgumentOutOfRangeException("This id is not in the list of positions");

            int old_gain = positions[id];
            VertexSwap swap = swaps[old_gain].FirstOrDefault(x => x.id == id);
            
            if(swap == null)
                throw new ArgumentOutOfRangeException("This id was not in the known swaps");

            swap.gain = new_gain;

            swaps[old_gain].Remove(swap);
            swaps[new_gain].Add(swap);

            positions[id] = new_gain;
        }

        public VertexSwap GetNext()
        {
            foreach(KeyValuePair<int, List<VertexSwap>> kvp in swaps)
            {
                if (kvp.Key > 0)
                    return null;
                foreach (VertexSwap swap in kvp.Value)
                {
                    if (swap.free)
                    {
                        swap.free = false;
                        return swap;
                    }
                }
            }
            return null;
        }
    }
}
