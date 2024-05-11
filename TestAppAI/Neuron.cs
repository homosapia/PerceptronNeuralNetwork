using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestAppAI
{
    public class Neuron
    {
        private const double _convergenceStep = 0.1;
        public double LocalGradient {  get; private set; }
        public List<NeuronСonnection> Connection { get; private set; }
        public double Output { get; private set; }
        private double Vector {  get; set; }
        public void SetNeuronСonnection(List<NeuronСonnection> connection)
        {
            Connection = connection;
        }

        public void WeightAdjustment(double localGradient)
        {
            LocalGradient = localGradient * (LogisticFunction(Vector) * (1 - LogisticFunction(Vector)));//g = e * f'(v)
            foreach (NeuronСonnection сonnection in Connection)
            {
                сonnection.weight = сonnection.weight - (_convergenceStep * LocalGradient * сonnection.incomingValue);
            }
        }

        public double Сalculation(List<double> incomingValues)
        {
            if (Connection.Count != incomingValues.Count)
            {
                throw new Exception("количество входящих значений в нейрон не равняется количеству связей");
            }
            double vector = 0;
            for (int i = 0; i < Connection.Count; i++)
            {
                vector += incomingValues[i] * Connection[i].weight;
                Connection[i].incomingValue = incomingValues[i];
            }
            Vector = vector;
            Output = LogisticFunction(vector);

            return Output;
        }

        private double LogisticFunction(double x)
        {
            return 1 / (1 + Math.Exp(-x));//1 / (1 + e^-x)
        }
    }
}
