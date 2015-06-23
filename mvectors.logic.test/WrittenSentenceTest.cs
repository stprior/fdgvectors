using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mvectors.logic.test
{
    [TestClass]
    public class WrittenSentenceTest
    {
        [TestMethod]
        public void TestLength()
        {
            var target = new WrittenSentence {Words = new[] { new WrittenWord("word1"), new WrittenWord("word2")}};
            var result = target.Length;
            result.Should().Be(2);
        }

        [TestMethod]
        public void TestLengthEmpty()
        {
            var target = new WrittenSentence();
            var result = target.Length;
            result.Should().Be(0);
        }
    }
}
