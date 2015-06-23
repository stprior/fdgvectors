using System;
using System.Collections.Generic;
using MathNet.Numerics.Distributions;
using mvectors.neuralnetwork;

namespace mvectors.logic
{
    public class TrainingAction : RootTrainingAction 
    {
        private static WordVectors _vectors;
        private WordVector _wordVector;
        private double[] _inputArray = null;
        private double[] _outputArray = null;
        private double[] _weights = null;
        private ContextMap contextMap = null;
        public static ITrainingAction ImplementPlan(IPlan plan, WordVectors vectors, ContextMaps maps)
        {        
            _vectors = vectors;
            var action = new RootTrainingAction(plan, maps);
            return action;
        }

        public TrainingAction(IPlan plan, ContextMaps maps) : base(plan,maps)
        {
            contextMap = maps.For(plan.Word);
            _weights = contextMap.NeuronWeights;
            if (!plan.Output.HasValue) throw new ArgumentException("plan.Output must be set", "plan");
            _wordVector = _vectors[plan.Word];
            if (_outputArray == null)
            {
                _outputArray = OutputToArray(plan.Output.Value);
            }
        }

        private double[] OutputToArray(TrainingOutput output)
        {            
            switch (output)
            {
                case TrainingOutput.Complete:
                    return new double[] {1.0};
                case TrainingOutput.Incomplete:
                    return new double[] {0.0};
                case TrainingOutput.Incorrect:
                    return new double[] {-1.0};
            }
            return null;
        }

        private class Example : ITrainingExample
        {
            public double[] Input { get; set; }
            public double[] ExpectedResult { get; set; }
        }
        public override double Train(INeuralNetwork network, MorphoSyntacticContext context)
        {
            if (_inputArray == null)
            {
                _inputArray = new double[_wordVector.Elements.Length + context.Elements.Length];
                _wordVector.Elements.CopyTo(_inputArray, 0);
            }

            context.Elements.CopyTo(_inputArray, WordVector.VectorLength);

            var example = new Example {Input = _inputArray, ExpectedResult = _outputArray};
            network.Theta.LoadFrom(_weights);
            var error = network.Train(example, 0.01);
            network.Theta.SaveTo(ref _weights);

            var childContext = new MorphoSyntacticContext(network.HiddenLayer);
            error = (error + base.Train(network, childContext)/2.0);
            return error;
        }
        public override void SaveContext(ContextMaps contextMaps)
        {
            contextMap.NeuronWeights = _weights;
        }

    }
}