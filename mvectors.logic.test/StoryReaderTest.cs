using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mvectors.logic;

namespace mvectors.logic.test
{
    [TestClass]
    public class StoryReaderTest
    {
        private StoryReader _target;

        [TestMethod]
        public void TestReadOneSentence()
        {
            var sr = new StringReader(@"This is a simple sentence. ");
            _target = new StoryReader(sr);
            var sentences = _target.ReadStory();

            Assert.AreEqual(1,sentences.SentencesOfLength(5).Count());
            Assert.AreEqual(0,sentences.SentencesOfLength(4).Count());

            var sentence = sentences.SentencesOfLength(5).Single();
            Assert.AreEqual("This",sentence.Words[0].Word);
            Assert.AreEqual("sentence",sentence.Words[4].Word);
            Assert.AreEqual(".",sentence.Words[5].Word);
            Assert.IsTrue(sentence.Words[5].IsEndOfSentence(), "Expected '.' to end sentence");

            Assert.AreEqual(1, sentences.FirstWords().ToList().Count, "Expected 1 first word");
            Assert.AreEqual("This",sentences.FirstWords().Single().Word, "Wrong first word");
        }

        [TestMethod]
        public void TestReadTwoSentences()
        {
            var sr = new StringReader(@"This is a simple sentence. And this is another. ");
            _target = new StoryReader(sr);
            var sentences = _target.ReadStory();

            Assert.AreEqual(1, sentences.SentencesOfLength(5).Count());
            Assert.AreEqual(1, sentences.SentencesOfLength(4).Count());

            var sentence = sentences.SentencesOfLength(5).Single();
            Assert.AreEqual("This", sentence.Words[0].Word);
            sentence = sentences.SentencesOfLength(4).Single();
            Assert.AreEqual("And", sentence.Words[0].Word);
            Assert.AreEqual("another", sentence.Words[3].Word);
        }

        [TestMethod]
        public void TestSmallSentence()
        {
            var sr = new StringReader(@"Hey!");
            _target = new StoryReader(sr);
            var sentences = _target.ReadStory();

            Assert.AreEqual(1, sentences.SentencesOfLength(1).Count());

            var sentence = sentences.SentencesOfLength(1).Single();
            sentence.Words[0].Word.Should().Be("Hey");
            
            sentence.Words[1].Word.Should().Be("!");
            sentence.Words[1].IsEndOfSentence().Should().BeTrue();
        }

        [TestMethod]
        public void TestFirstWords()
        {
            var sr = new StringReader(@"This is a simple sentence. This is another. ");
            _target = new StoryReader(sr);
            var sentences = _target.ReadStory();

            var firstWord = sentences.FirstWords().First();
            firstWord.Word.Should().Be("This");
            

        }
    }
}
