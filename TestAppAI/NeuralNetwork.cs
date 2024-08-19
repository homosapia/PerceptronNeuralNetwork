using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestAppAI
{
    public class NeuralNetwork
    {
        private List<Layer> _layers;
        public NeuralNetwork(List<Layer> layers)
        {
            _layers = layers;
        }

        public List<double> GetResponse()
        {
            var neuron = _layers.Last().Neurons.Select(x => x.Output);
            return neuron.ToList();
        }


        public void StartTraining(List<TrainingData> expectedAnswer, int period)
        {
            for (int i = 0; i < period; i++)
            {
                Console.WriteLine($"Эпоха {i}");

                //int amountElements = expectedAnswer.Count / 20;

                //var partsTraining = Enumerable.Range(0, amountElements)
                //            .Select(i => expectedAnswer.Skip(i * 20).Take(20).ToList())
                //            .ToList();

                foreach (TrainingData data in expectedAnswer)
                {
                    Start(data.Inbox);

                    List<double> preliminaryAnswers = _layers.Last().Neurons.Select(i => i.Output).ToList();
                    if (data.ExpectedAnswer.Count != preliminaryAnswers.Count)
                        throw new Exception("Количество выходных слоев не равно количеству ожидаемых ответов");

                    double errorValue = 0.0;
                    for (int j = 0; j < preliminaryAnswers.Count; j++)
                    {
                        errorValue = preliminaryAnswers[j] - data.ExpectedAnswer[j];//e = y-d

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
