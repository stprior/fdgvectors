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
        static void Main(string[] args)
        {
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

            //sentenceLearner.Learn(1);
            Console.ReadLine();
        }
    }
}
