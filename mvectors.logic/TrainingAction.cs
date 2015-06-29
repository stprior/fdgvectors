using System;
using mvectors.neuralnetwork;

namespace mvectors.logic
{
    public class TrainingAction : RootTrainingAction 
    {
        private static WordVectors _vectors;
        private readonly WordVector _wordVector;
        private double[] _inputArray;
        private readonly double[] _outputArray;
        private double[] _weights;
        private readonly ContextMap _contextMap;
        public static ITrainingAction ImplementPlan(IPlan plan, WordVectors vectors, ContextMaps maps)
        {        
            _vectors = vectors;
            var action = new RootTrainingAction(plan, maps);
            return action;
        }

        public TrainingAction(IPlan plan, ContextMaps maps) : base(plan,maps)
        {
            _contextMap = maps.For(plan.Word);
            _weights = _contextMap.NeuronWeights;
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
                    return new[] {1.0};
                case TrainingOutput.Incomplete:
                    return new[] {0.0};
                case TrainingOutput.Incorrect:
                    return new[] {-1.0};
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
            if (_inputArray == null && _wordVector != null)
            {
                _inputArray = new double[_wordVector.Elements.Length + context.Elements.Length];
                _wordVector.Elements.CopyTo(_inputArray, 0);
            }
            if (_inputArray == null) return base.Train(network, context);

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
            _contextMap.NeuronWeights = _weights;
        }

    }
}