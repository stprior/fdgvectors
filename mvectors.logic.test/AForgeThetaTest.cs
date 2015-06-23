using System;
using AForge.Neuro;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mvectors.neuralnetwork;

namespace mvectors.logic.test
{
    [TestClass]
    public class AForgeThetaTest
    {
        private AForgeTheta _target;
        private AForgeNeuralNetwork _net1;

        [TestInitialize]
        public void Setup()
        {
            _net1 = new AForgeNeuralNetwork(4, 5, 2);
            _target = _net1.Theta as AForgeTheta;
        }

        [TestMethod]
        public void TestArrayContents()
        {
            double[] weights = null;
            _target.SaveTo(ref weights);
            weights.Should().NotBeNull();
            weights.Length.Should().Be(15,"5 neurons * (1 Threshold + 4-2 saved inputs)");
            var neuron = (ActivationNeuron)_target.Layer.Neurons[0];
            weights[0].Should().Be(neuron.Threshold);
            weights[1].Should().Be(neuron.Weights[2]);
        }

        [TestMethod]
        public void TestSaveArray()
        {
            const double setWeight = 0.13;
            var weights = new double[15];
            weights[1] = setWeight;
            _target.LoadFrom(weights);
            _target.Layer.Neurons[0].Weights[2].Should().Be(setWeight);
        }


        [TestMethod]
        public void TestUpdatesNetwork()
        {
            const double setWeight = 0.15;
            var weights = new double[15];
            weights[1] = setWeight;
            _target.LoadFrom(weights);
            _net1.UnderlyingNetwork.Layers[0].Neurons[0].Weights[2].Should().Be(setWeight);
            
        }

        [TestMethod]
        public void TestStaticWeightsPreserved()
        {
            double[] weights = null;
            _target.SaveTo(ref weights);

            var net2 = new AForgeNeuralNetwork(4, 5, 2);
            net2.Theta.LoadFrom(weights);
            var result1 = _net1.Run(new[] { 0.3, 0.4, 0.0, 0.0});
            var result2 = net2.Run(new[] { 0.3, 0.4, 0.0, 0.0 });

            _net1.HiddenLayer[0].Should().NotBe(net2.HiddenLayer[0],"Static weights from neurons were not preserved after LoadFrom");

        }

        [TestMethod]//cant see bias term, so cant compare results of two networks
        public void TestNonStaticWeightsRestored()
        {
            double[] weights = null;
            _target.SaveTo(ref weights);

            var net2 = new AForgeNeuralNetwork(4, 5, 2);
            net2.Theta.LoadFrom(weights);
            

            _net1.UnderlyingNetwork.Layers[0].Neurons[0].Weights[3].Should().Be(net2.UnderlyingNetwork.Layers[0].Neurons[0].Weights[3]);
            var result1 = _net1.Run(new[]   { 0.0, 0.0, 0.4, 0.5});
            var result2 = net2.Run(new[]    { 0.0, 0.0, 0.4, 0.5});
            _net1.HiddenLayer[0].Should().Be(net2.HiddenLayer[0], "same inputs with restored weights");
        }

    }
}

