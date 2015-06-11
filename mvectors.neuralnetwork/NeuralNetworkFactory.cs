namespace mvectors.neuralnetwork
{
    public class NeuralNetworkFactory : INeuralNetworkFactory
    {
        public virtual INeuralNetwork CreateNeuralNetwork(int inputNodes, int hiddenNodes)
        {
            var wrapper = new AForgeNeuralNetwork(inputNodes, hiddenNodes);

            wrapper.Randomize();
            return wrapper;
        }
    }
}

