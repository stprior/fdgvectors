namespace mvectors.neuralnetwork
{
    public class NeuralNetworkFactory : INeuralNetworkFactory
    {
        public virtual INeuralNetwork CreateNeuralNetwork(int inputNodes, int hiddenNodes, int staticWeights=0)
        {
            var wrapper = new AForgeNeuralNetwork(inputNodes, hiddenNodes,staticWeights);
            
            wrapper.Randomize();
            return wrapper;
        }
    }
}

