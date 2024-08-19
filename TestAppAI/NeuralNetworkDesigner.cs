using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppAI
{
    public class NeuralNetworkDesigner
    {
        public NeuralNetwork CreateNeuralNetwork(List<int> listLayers, int firstLayerSize)
        {
            List<Layer> layers = new List<Layer>();

            for (int i = 0; i < listLayers.Count; i++)
            {
                Layer layer = new Layer();
                layer.LayerNumber = i;
                layer.NumberNeurons = listLayers[i];
                layer.Neurons = new List<Neuron>();
                for (int j = 0; j < layer.NumberNeurons; j++)
                {
                    Neuron neuron = new Neuron();
                    layer.Neurons.Add(neuron);
                }
                layers.Add(layer);
            }


            for (int i = layers.Count-1; i >= 0; i--)
            {
                Layer layer = layers[i];
                foreach (Neuron neuron in layer.Neurons)
                {
                    try
                    {
                        int countNeurons = layers[i - 1].NumberNeurons;
                        List<NeuronСonnection> neuronСonnections = GetNeuronСonnection(countNeurons);
                        neuron.SetNeuronСonnection(neuronСonnections);
                    }
                    catch (Exception ex)//System.ArgumentOutOfRangeException
                    {
                        int countNeurons = firstLayerSize;
                        List<NeuronСonnection> neuronСonnections = GetNeuronСonnection(countNeurons);
                        neuron.SetNeuronСonnection(neuronСonnections);
                    }
                }
            }

            return new NeuralNetwork(layers);
        }

        private List<NeuronСonnection> GetNeuronСonnection(int countNeurons)
        {
            List<NeuronСonnection> neuronСonnections = new List<NeuronСonnection>();
            for (int j = 0; j < countNeurons; j++)
            {
                NeuronСonnection neuronСonnection = new NeuronСonnection();
                neuronСonnection.weight = (j % 2 == 0) ? (double)(new Random()).NextDouble() : -(double)(new Random()).NextDouble();
                neuronСonnections.Add(neuronСonnection);
            }
            return neuronСonnections;
        }
    }
}
