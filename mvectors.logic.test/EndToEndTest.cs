using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mvectors.logic.test
{
    [TestClass]
    public class EndToEndTest
    {
        private WordVectors _wordVectors;
        private SentenceLearner _sentenceLearner;
        private ContextMaps _contextMaps;

        
        public void LoadAndLearn()
        {
            var wvl =new WordVectorLoader("wikipedia_vectors.txt");
            _wordVectors = wvl.LoadVectors();
            var s = new StoryReader("WikiJunior_Biology.txt");
            var sentences = s.ReadStory();
            _contextMaps = new ContextMaps();
            _sentenceLearner = new SentenceLearner(_wordVectors, sentences, _contextMaps);
            var plan = _sentenceLearner.PreparePlan(4);
            _sentenceLearner.ExecutePlan(plan, MorphoSyntacticContext.InitialState());
        }

        [TestMethod]
        public void RunSentences()
        {
            LoadAndLearn();
            var stream = new StringReader(@"A tiger has stripes.");
            var storyReader = new StoryReader(stream);
            var sentence = storyReader.ReadSentence();

            var result = _sentenceLearner.Run(sentence);
            Debug.Print(result);

            stream = new StringReader(@"A crocodile has sharp teeth.");
            storyReader = new StoryReader(stream);
            sentence = storyReader.ReadSentence();

            result = _sentenceLearner.Run(sentence);
            Debug.Print(result);
        }

        [TestMethod]
        public void ListContexts()
        {
            LoadAndLearn();
            var sb = new StringBuilder("<table><tr><th>Word</th>");
            foreach (var map in _contextMaps.AllItems())
            {
                sb.Append("<tr><td>")
                    .Append(map.Key.Word).AppendLine("</td><td><table>");
                int colNum = 0;
                int lastCol = WordVector.VectorLength;
                foreach (var neuronWeight in map.Value.NeuronWeights)
                {
                    if (colNum == 0) sb.Append("<tr>");
                    sb.Append("<td>").Append(neuronWeight).Append("</td>");
                    colNum = (colNum + 1)%lastCol;
                    if (colNum == 0) sb.AppendLine("</tr>");
                }
                sb.Append("</table></td></tr>");
                Debug.Write(sb.ToString());
                sb.Clear();
            }

            Debug.WriteLine("</table>");
        }
    }
}
