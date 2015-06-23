using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace mvectors.logic.test
{
    /// <summary>
    /// Summary description for WordVectorsTest
    /// </summary>
    [TestClass]
    public class WordVectorsTest
    {

        private WordVectors _target;

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestRandomExcluding()
        {
            _target = new WordVectors(10);
            var wv1 = new WordVector {Name = "wv1" };
            var wv2 = new WordVector { Name = "wv2" }; ;
            var wv3 = new WordVector { Name = "wv3" }; ;
            var wv4 = new WordVector { Name = "wv4" }; ;

            _target.TryAdd("wv1", wv1);
            _target.TryAdd("wv2", wv2);
            _target.TryAdd("wv3", wv3);
            _target.TryAdd("wv4", wv4);

            var result = _target.RandomExcluding(new HashSet<WordVector> {wv1, wv2, wv3});
            result.Should().Be(wv4);

        }
    }
}
