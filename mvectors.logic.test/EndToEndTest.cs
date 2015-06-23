using System;
using System.Diagnostics;
using System.IO;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mvectors.logic.test
{
    [TestClass]
    public class EndToEndTest
    {
        [TestMethod]
        public void LoadAndLearn()
        {
            var wvl =
                new WordVectorLoader(
                    "C:\\Code\\word2vec\\wikipedia_vectors.txt");
            var wordVectors = wvl.LoadVectors();
            var s = new StoryReader("C:\\Users\\Stephen\\Dropbox\\Private\\AI\\WikiJunior_Biology.txt");
            var sentences = s.ReadStory();
            var contextMaps = new ContextMaps();
            var sentenceLearner = new SentenceLearner(wordVectors, sentences,contextMaps);
            var plan = sentenceLearner.PreparePlan(4);
            sentenceLearner.ExecutePlan(plan,MorphoSyntacticContext.InitialState());
            var stream = new StringReader(@"A tiger has stripes.");
            var storyReader = new StoryReader(stream);
            var sentence = storyReader.ReadSentence();

            var result = sentenceLearner.Run(sentence);
            Debug.Print(result);

            stream = new StringReader(@"A crocodile has sharp teeth.");
            storyReader = new StoryReader(stream);
            sentence = storyReader.ReadSentence();

            result = sentenceLearner.Run(sentence);
            Debug.Print(result);

            
        }
    }
}
