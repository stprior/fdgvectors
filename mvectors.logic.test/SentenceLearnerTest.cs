using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mvectors.neuralnetwork;
using NSubstitute;

namespace mvectors.logic.test
{
    [TestClass]
    public class SentenceLearnerTest
    {
        private SentenceLearner _target;
        private WordVectors _vectors;
        private Sentences _sentences;
        private NeuralNetworkFactory _neuralNetworkFactory;
        private ContextMaps _contextMaps;

        [TestInitialize]
        public void SetupNetwork()
        {
            _vectors = Substitute.For<WordVectors>();
            _sentences = Substitute.For<Sentences>();
            SentenceLearner.NeuralNetworkFactory = _neuralNetworkFactory = new NeuralNetworkFactory();
            _contextMaps = new ContextMaps();
            _target = new SentenceLearner(_vectors, _sentences,_contextMaps);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var factory = Substitute.For<NeuralNetworkFactory>();
            SentenceLearner.NeuralNetworkFactory = factory;
            WordVector.VectorLength = 10;
            MorphoSyntacticContext.VectorLength = 15;
            var target = new SentenceLearner(_vectors, _sentences, _contextMaps);
            factory.Received(1).CreateNeuralNetwork(25, 15,10);
        }

        [TestMethod]
        public void TestTrain()
        {
            var stream = new StringReader(@"Hey! Look here!");
            var storyReader = new StoryReader(stream);
            var sentences = storyReader.ReadStory();
            var wordVectors = new WordVectors(10);
            wordVectors.TryAdd("Hey", new WordVector { Name = "Hey" });
            wordVectors.TryAdd("Look", new WordVector { Name = "Look" });
            wordVectors.TryAdd("here", new WordVector { Name = "here" });
            wordVectors.TryAdd("Test", new WordVector { Name = "Test" });
            wordVectors.TryAdd("!", new WordVector { Name = "!" });

            var contextMaps = new ContextMaps();
            var target = new SentenceLearner(wordVectors, sentences, contextMaps);
            
            var trainingPlan = target.PreparePlan(4);
            //should train Hey,ctx0,1; Look,ctx0,0; here,ctx0,-1, here,ctx1,1
            trainingPlan.ContainsPlanStep(new WrittenWord("Hey"), TrainingOutput.Complete).Should().BeTrue();
            trainingPlan.ContainsPlanStep(new WrittenWord("Look"), TrainingOutput.Incomplete).Should().BeTrue();
            trainingPlan.ContainsPlanStep(new WrittenWord("Test"), TrainingOutput.Incomplete).Should().BeFalse();

        }

        ///training plan generates a tree structure of words and expected outputs
        ///action plan looks up word vectors and outputs
        [TestMethod]
        public void TestTrain2()
        {

            var stream = new StringReader(@"Hey! Look here!");
            var storyReader = new StoryReader(stream);
            var sentences = storyReader.ReadStory();
            var wordVectors = new WordVectors(10);
            var contextMaps = new ContextMaps();
            wordVectors.TryAdd("Hey", new WordVector { Name = "Hey" });
            wordVectors.TryAdd("Look", new WordVector { Name = "Look" });
            wordVectors.TryAdd("here", new WordVector { Name = "here" });
            wordVectors.TryAdd("Test", new WordVector { Name = "Test" });
            wordVectors.TryAdd("!", new WordVector { Name = "!" });

            var target = new SentenceLearner(wordVectors, sentences, contextMaps);

            var plan = target.PreparePlan(4);
            
            target.ExecutePlan(plan, MorphoSyntacticContext.InitialState());


            var weights = target.GetWeightsForWord(new WrittenWord("Hey"));

        }
    }

    internal static class TrainingPlanExtensions
    {
        public static void ShouldHaveTrainingAction(this TrainingPlan strategy, string word, double target)
        {
            
        }
    }
}
