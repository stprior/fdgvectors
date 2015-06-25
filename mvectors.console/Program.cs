using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mvectors.logic;

namespace mvectors.console
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Use the unit tests");
            var loader = new WordVectorLoader(args[0]);
            var vectors = loader.LoadVectors();

            string fileName;
            if (args.Length == 1)
            {
                Console.WriteLine("Story file:");
                fileName = Console.ReadLine();
            }
            else
            {
                fileName = args[1];
            }

            var storyReader = new StoryReader(fileName);
            var sentences = storyReader.ReadStory();
            var firstWord = sentences.FirstWords().FirstOrDefault();
            Console.Write(firstWord != null ? firstWord.Word : "empty file");
            var contextMaps = new ContextMaps();
            var sentenceLearner = new SentenceLearner(vectors, sentences, contextMaps);
            var plan = sentenceLearner.PreparePlan(4);
            sentenceLearner.ExecutePlan(plan, MorphoSyntacticContext.InitialState());

            //sentenceLearner.Learn(1);
            while (true)
            {
                Console.Write("\n> ");
                var stdinReader = new StoryReader(Console.In);
                var sentence = stdinReader.ReadSentence();
                if (sentence.Words[0].IsEndOfSentence()) break;
                var result = sentenceLearner.Run(sentence);
                Console.WriteLine(result);

            }
            Console.WriteLine("Quitting");
        }
    }
}
