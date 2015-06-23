using System;
using System.Linq;
using System.Text;
using mvectors.neuralnetwork;

namespace mvectors.logic
{
    public class SentenceLearner
    {
        private WordVectors _vectors;
        private Sentences _sentences;
        private INeuralNetwork _network;
        private ContextMaps _contextMaps;
        public double MinError { get; set; }
        public static NeuralNetworkFactory NeuralNetworkFactory { get; set; }

        static SentenceLearner()
        {
            NeuralNetworkFactory = new NeuralNetworkFactory();
        }

        public SentenceLearner(WordVectors vectors, Sentences sentences, ContextMaps contextMaps)
        {
            _vectors = vectors;
            _sentences = sentences;
            _network = NeuralNetworkFactory.CreateNeuralNetwork(
                    WordVector.VectorLength + MorphoSyntacticContext.VectorLength, MorphoSyntacticContext.VectorLength, WordVector.VectorLength);
            _contextMaps = contextMaps;
            MinError = 0.01;
        }

        public double[] GetWeightsForWord(WrittenWord word)
        {
            var map = _contextMaps.For(word);
            return map==null ? null : map.NeuronWeights;
        }

        public IPlan PreparePlan(int maxSentenceLength)
        {
            var plan = TrainingPlan.InitialPlan();
            for (int sentenceLength = 0; sentenceLength < maxSentenceLength; sentenceLength++)
            {
                PreparePlan(sentenceLength, plan);
            }
            return plan;
        }
        public bool PreparePlan(int sentenceLength, IPlan basePlan)
        {
            bool foundAny = false;
            foreach (var writtenSentence in _sentences.SentencesOfLength(sentenceLength))
            {
                foundAny = true;
                IPlan nextPlanNode= basePlan;
                WrittenWord prevWord = null;
                foreach (WrittenWord writtenWord in writtenSentence.Words)
                {
                    var output = writtenWord.IsEndOfSentence() ? TrainingOutput.Complete : TrainingOutput.Incomplete;
                    if (prevWord != null)
                    {
                        nextPlanNode = nextPlanNode.AddPlanStep(prevWord, output);
                    }
                    prevWord = writtenWord;
                }
            }
            return foundAny;
        }

        public void ExecutePlan(IPlan plan, MorphoSyntacticContext initialContext)
        {

            var baseAction = TrainingAction.ImplementPlan(plan, _vectors, _contextMaps);
            //todo - could loop to reduce error
            var error = baseAction.Train(_network,initialContext);
            baseAction.SaveContexts(_contextMaps);
        }

        public string Run(WrittenSentence sentence)
        {
            StringBuilder summary = new StringBuilder();
            WordVector wordVector;
            MorphoSyntacticContext context = MorphoSyntacticContext.InitialState();
            var inputs = new double[WordVector.VectorLength + MorphoSyntacticContext.VectorLength ];
            summary.Append("<table><tr><th>Word</th><th>Context</th><th>Output</th></tr>");
            foreach (var word in sentence.Words)
            {
                summary.Append("<tr><td>").Append(word.Word).Append("</td><td>");               
                foreach (var element in context.Elements)
                {
                    summary.Append(element.ToString("F")).Append(' ');
                }                

                wordVector = _vectors[word];
                Array.Copy(wordVector.Elements,inputs,WordVector.VectorLength);
                Array.Copy(context.Elements, 0, inputs, WordVector.VectorLength, MorphoSyntacticContext.VectorLength);
                
                var result = _network.Run(inputs);
                summary.Append("</td><td>").Append(result[0]).Append("</td></tr>");
                _network.HiddenLayer.CopyTo(context.Elements,0);
            }
            summary.Append("</table>");
            return summary.ToString();
        }
    }

    internal class Example : ITrainingExample
    {
        public double[] Input { get; private set; }
        public double[] ExpectedResult { get; private set; }
   
        public Example(WordVector word, MorphoSyntacticContext context, double[] target )
        {
            Input = new double[WordVector.VectorLength + MorphoSyntacticContext.VectorLength];
            word.Elements.CopyTo(Input,0);
            context.Elements.CopyTo(Input, WordVector.VectorLength);
            ExpectedResult = target;
        }
    }

}
