using System;
using System.Linq;
using AForge.Neuro;

namespace mvectors.neuralnetwork
{
    public class AForgeTheta: ITheta
    {
        private ActivationLayer _layer;
        private readonly int _startWeightsFrom;
        public AForgeTheta() : this(0) { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="staticWeights">The number of inputs to each node which are not affected by load and save methods in Theta. 
        /// <remarks>Note this includes a bias term, along with the inputs to the layer</remarks></param>
        public AForgeTheta(int staticWeights)
        {
            _startWeightsFrom = staticWeights;
        }
        public ActivationLayer Layer
        {
            get { return _layer; }
            set { _layer = value; }
        }

        public void LoadFrom(double[] weights)
        {
            var i = 0;
            foreach (var neuron in _layer.Neurons.Cast<ActivationNeuron>())
            {
                neuron.Threshold = weights[i];
                i++;
                for (var j = _startWeightsFrom; j < neuron.Weights.Length; j++)
                {
                    neuron.Weights[j] = weights[i];
                    i++;
                }
            }
        }

        public void SaveTo(ref double[] weights)
        {
            if (weights == null) weights = new double[ArrayLength()];
            var index = 0;
            var increment = _layer.Neurons.First().Weights.Length - _startWeightsFrom;
            foreach (var neuron in _layer.Neurons.Cast<ActivationNeuron>())
            {
                weights[index] = neuron.Threshold;
                index++;
                Array.ConstrainedCopy(neuron.Weights,_startWeightsFrom,weights,index,increment);
                index += increment;
            }            
        }

        public int ArrayLength()
        {
            return _layer.Neurons.Length * (1 + (_layer.Neurons[0].Weights.Length - _startWeightsFrom));
        }
    }
}