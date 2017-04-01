using INFOEA.Algorithm.Genome.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFOEA.Algorithm.Neighborhood.Fiduccia
{
    class VertexSwap
    {
        public VertexSwap(int _id, char _swap_to, int _gain)
        {
            id = _id;
            swap_to = _swap_to;
            gain = _gain;
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

        public void EditSwap(int id, int neighbor_id, char neighbor_value, ref char[] data)
        {
            if (!positions.ContainsKey(id))
                throw new ArgumentOutOfRangeException("This id is not in the list of positions");

            int old_gain = positions[id];
            int new_gain = 0;
            VertexSwap swap = swaps[old_gain].FirstOrDefault(x => x.id == id);

             /* Alle neighbors weer langs. Het kan namelijk een groter verschil geven dan gewoon 1
             *  gain-- of gain++:
             *  
             *  ( ) -- (x) -- ( ) <- wanneer we (x) naar ( ) veranderen, hebben we een gain van -2
             *  ( ) -- (x) -- (x) <- maar wanneer 1 van zijn buren is veranderd naar (x), is de gain wanneer 
             *                       we de middelste (x) veranderen naar ( ) ineens 0. 
             *                       (plus 1 voor de rechter, min 1 voor de linker)
             */ 
            int[] neighbors = GraphGenome.vertices[swap.id].Connections;
            foreach(int neighbor in neighbors)
            {
                char n_value = neighbor == neighbor_id ? neighbor_value : data[neighbor - 1];

                if (n_value == swap.swap_to)
                    new_gain--;
                else
                    new_gain++;
            }

            if (!swaps.ContainsKey(new_gain))
                throw new ArgumentOutOfRangeException("You can't add this new gain");

            // Niks hoeft te veranderen.
            if (new_gain == old_gain)
                return;

            swap.gain = new_gain;

            swaps[old_gain].Remove(swap);
            swaps[new_gain].Add(swap);

            positions[id] = new_gain;
        }

        /*public void EditSwap(int id, int new_gain)
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
        }*/

        public VertexSwap GetNext()
        {
            int count = swaps.Keys.Count;
            List<int> keys = swaps.Keys.ToList();
            for(int i = 0; i < count; ++i)
            {
                int key = keys[i];

                if (swaps[key].Count == 0)
                    continue;

                VertexSwap swap = swaps[key][0];
                swaps[key].Remove(swap);
                positions.Remove(swap.id);

                return swap;
            }
            return null;
        }
    }
}
