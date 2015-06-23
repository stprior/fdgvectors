using System.Runtime.Versioning;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mvectors.neuralnetwork;

namespace mvectors.logic.test
{
    [TestClass]
    public class NeuralNetworkTest
    {
        [TestMethod]
        public void TestNeuralNetworkFactory()
        {
            var target = new NeuralNetworkFactory();
            var result = target.CreateNeuralNetwork(3, 2);
            result.Should().BeAssignableTo<AForgeNeuralNetwork>();
        }

        private class Example : ITrainingExample
        {
            public double[] Input { get; set; }
            public double[] ExpectedResult { get; set; }
        }
        [TestMethod]
        public void TestNetwork()
        {
            var wordVector = new WordVector() {Name = "test"};
            var elements = new[] {0.1, 0.2, 0.3};
            elements.CopyTo(wordVector.Elements,0);
            var context = new[] {0.4, 0.5};
            var factory = new NeuralNetworkFactory();
            var network = factory.CreateNeuralNetwork(2, 2);
            var examples = new[]
            {
                new Example {Input = new [] {1.0, 0.0}, ExpectedResult = new []{1.0}},
                new Example {Input = new [] {0.0, 1.0}, ExpectedResult = new []{1.0}},
                new Example {Input = new [] {1.0, 1.0}, ExpectedResult = new []{0.0}},
                new Example {Input = new [] {0.0, 0.0}, ExpectedResult = new []{0.0}}
            };
            var error = network.Train(examples,0.1);
            var result = network.Run(new[] {0.0, 0.0});
            result[0].Should().BeLessOrEqualTo(error + 0.1);
        }


    }
}
