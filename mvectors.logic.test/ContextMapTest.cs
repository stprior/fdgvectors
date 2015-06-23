using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mvectors.logic.test
{
    [TestClass]
    public class ContextMapTest
    {
        [TestMethod]
        public void TestGenerateWeights()
        {
            var target = new ContextMap();
            target.NeuronWeights.Should().NotBeNullOrEmpty();
            target.NeuronWeights.Length.Should().BeGreaterThan(1);
        }
    }
}
