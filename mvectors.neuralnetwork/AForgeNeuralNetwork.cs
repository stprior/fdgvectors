using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AForge.Neuro;
using AForge.Neuro.Learning;

namespace mvectors.neuralnetwork
{
    public class AForgeNeuralNetwork : INeuralNetwork
    {
        private int _inputNodes;
        private int _hiddenNodes;
        private ActivationNetwork _network;
        private int _staticWeights;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputNodes"></param>
        /// <param name="hiddenNodes"></param>
        /// <param name="staticWeights">The number of inputs to each node which are not affected by load and save methods in Theta. 
        /// <remarks>Note this includes a bias/threshold term, along with the inputs to the layer</remarks></param>
        public AForgeNeuralNetwork(int inputNodes, int hiddenNodes, int staticWeights = 0)
        {
            var fn = new BipolarSigmoidFunction();
            _network = new ActivationNetwork(fn, inputNodes, hiddenNodes,1);
            _inputNodes = inputNodes;
            _hiddenNodes = hiddenNodes;
            _staticWeights = staticWeights;
        }
        public void SetTheta(ITheta theta)
        {
        }

        public ITheta Theta
        {
            get
            {
                return new AForgeTheta(_staticWeights)
                {
                    Layer = (ActivationLayer) _network.Layers[0]
                };                
            }
            set
            {
                var aforgeTheta = value as AForgeTheta;
                if (aforgeTheta == null) throw new ArgumentException("Wrong implementation of ITheta");

                _network = new ActivationNetwork(new SigmoidFunction(), _inputNodes, _hiddenNodes);
                _network.Layers[0] = aforgeTheta.Layer;
            }
        }

        public double[] HiddenLayer
        {
            get { return _network.Layers[0].Output; }
        }
        public double[] Run(double[] inputs)
        {
            return _network.Compute(inputs);
        }

        public double Train(IEnumerable<ITrainingExample> samples, double maxError)
        {
            long counter = 0;
            var stopwatch = Stopwatch.StartNew();
            var teacher = new BackPropagationLearning(_network)
            {
                LearningRate = 0.1
            };
            double error = 1.0, lastError = 1.1;
            var trainingExamples = samples as ITrainingExample[] ?? samples.ToArray();
            while (error > maxError)// && error < lastError)
            {
                lastError = error;
                var inputs = trainingExamples.Select(x => x.Input).ToArray();
                var outputs = trainingExamples.Select(x => x.ExpectedResult).ToArray();

                error=teacher.RunEpoch(inputs, outputs);
                counter++;
            }
            stopwatch.Stop();
            Debug.WriteLine("{0} loops took {1} ms",counter,stopwatch.ElapsedMilliseconds);
            return error;
        }

        public double Train(ITrainingExample sample, double maxError)
        {
            long counter = 0;
            var stopwatch = Stopwatch.StartNew();
            var teacher = new BackPropagationLearning(_network);
            double error = 1.0, lastError = 1.1;
            
            while (error > maxError && error < lastError)
            {
                lastError = error;
            
                error = teacher.Run(sample.Input, sample.ExpectedResult);
                counter++;
            }
            stopwatch.Stop();
            Debug.WriteLine("{0} loops took {1} ms", counter, stopwatch.ElapsedMilliseconds);
            return error;
        }

        public void Randomize()
        {
            _network.Randomize();
        }

        public ActivationNetwork UnderlyingNetwork
        {
            get { return _network; }
        }
    }
}