using System.Linq;
using AForge.Neuro;

namespace mvectors.neuralnetwork
{
    public class AForgeTheta: ITheta
    {
        private ActivationLayer _layer;

        public ActivationLayer Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        public void LoadFrom(double[] weights,int startFrom=0)
        {
            int i = 0;

            for (int n = startFrom; i<_layer.Neurons.Length; i++)
            {                
                var neuron = _layer.Neurons[n];
                for (int j = 0; j < neuron.Weights.Length; j++)
                {
                    neuron.Weights[j] = weights[i];
                    i++;
                }
            }
        }

        public double[] SaveTo(int startFrom=0)
        {
            var weights = new double[(_layer.Neurons.Length - startFrom)* _layer.Neurons[0].Weights.Length];
            int i = 0;
            foreach (var neuron in _layer.Neurons.Skip(startFrom))
            {
                foreach (var weight in neuron.Weights)
                {
                    weights[i] = weight;
                    i++;
                }
            }
            return weights;
        }
    }
}