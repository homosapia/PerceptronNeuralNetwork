using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TestAppAI
{
    public class NeuralNetwork
    {
        private List<Layer> _layers;
        public NeuralNetwork(List<Layer> layers)
        {
            _layers = layers;
        }

        public double Reply
        {
            get
            {
                return _layers.Last().Neurons.Last().Output;
            }
        }

        public void StartTraining(List<TrainingData> expectedAnswer, int period)
        {
            for (int i = 0; i < period; i++)
            {
                foreach (TrainingData data in expectedAnswer)
                {
                    Start(data.Inbox);

                    List<double> preliminaryAnswers = _layers.Last().Neurons.Select(i => i.Output).ToList();
                    for (int j = 0; j < preliminaryAnswers.Count; j++)
                    {
                        double errorValue = preliminaryAnswers[j] - data.ExpectedAnswer[j];//e = y-d
                        Console.WriteLine(errorValue);
                        Neuron neurons = _layers.Last().Neurons[j];
                        neurons.WeightAdjustment(errorValue);
                    }

                    for (int layer = _layers.Count - 1; layer > 0; layer--)
                    {
                        for (int neuronNumberPreviousLayer = 0; neuronNumberPreviousLayer < _layers[layer - 1].Neurons.Count; neuronNumberPreviousLayer++)
                        {
                            List<double> gradientWeights = new List<double>();
                            foreach (Neuron currentLayerNeuron in _layers[layer].Neurons)
                            {
                                double neuronGradient = currentLayerNeuron.LocalGradient * currentLayerNeuron.Connection[neuronNumberPreviousLayer].weight;
                                gradientWeights.Add(neuronGradient);
                            }
                            _layers[layer - 1].Neurons[neuronNumberPreviousLayer].WeightAdjustment(gradientWeights.Sum());
                        }
                    }
                }
            }
        }

        public void Start(List<double> incomingValues)
        {
            List<double> incomingValuesStart = incomingValues.ToList();
            foreach (Layer layer in _layers)
            {
                List<double> outputValues = new List<double>();
                foreach(Neuron neuron in layer.Neurons)
                {
                    outputValues.Add(neuron.Сalculation(incomingValuesStart));
                }
                incomingValuesStart.Clear();
                incomingValuesStart = outputValues;
            }
        }
    }
}
