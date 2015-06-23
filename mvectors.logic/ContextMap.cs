using mvectors.neuralnetwork;

namespace mvectors.logic
{
    public class ContextMap
    {

        private double[] _neuronWeights;
        public double[] NeuronWeights
        {
            get { return _neuronWeights ?? (_neuronWeights = DefaultNeuronWeights()); }
            set { _neuronWeights = value; }
        }
        /// <summary>
        /// Initialise to random values, implemented by generating a new network and extracting the relevant weights
        /// </summary>
        /// <returns></returns>
        private double[] DefaultNeuronWeights()
        {
            double[] weights = null;
            
            var net =
                new AForgeNeuralNetwork(MorphoSyntacticContext.VectorLength + MorphoSyntacticContext.VectorLength, MorphoSyntacticContext.VectorLength);

            net.Randomize();
            net.Theta.SaveTo(ref weights);
            return weights;
        }
    }
}