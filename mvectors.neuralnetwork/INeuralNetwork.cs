using System.Collections.Generic;

namespace mvectors.neuralnetwork
{
    public interface INeuralNetwork
    {
        /// <summary>
        /// Representation of the weights mapping input to hidden layer
        /// </summary>
        ITheta Theta { get; set; }
    
        double Train(IEnumerable<ITrainingExample> samples, double minError);
        double Train(ITrainingExample sample, double minError);

        double[] Run(double[] inputs);
    }
}