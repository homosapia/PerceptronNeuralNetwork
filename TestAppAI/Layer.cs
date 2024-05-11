using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppAI
{
    public class Layer
    {
        public int LayerNumber { get; set; }

        public int NumberNeurons {  get; set; }
        
        public List<Neuron> Neurons { get; set; }
    }
}
