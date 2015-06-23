using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mvectors.logic
{
    public class StoryReader
    {
        private readonly TextReader _fileReader;
        public List<string> words;

        public StoryReader(string filename)
        {
            if (!File.Exists(filename)) throw new FileNotFoundException(String.Format("{0} not found", filename));
            _fileReader = File.OpenText(filename);

        }
        public StoryReader(TextReader stream)
        {
            _fileReader = stream;

        }

        public WrittenSentence ReadSentence()
        {
            var sentence = new List<WrittenWord>();

            foreach (var writtenWord in ReadWords())
            {

                sentence.Add(writtenWord);
                if (writtenWord.IsEndOfSentence())
                {
                    break;
                }
            }
            return new WrittenSentence { Words = sentence.ToArray() };
        }
        public Sentences ReadStory()
        {
            var result = new Sentences();
            var sentence = new List<WrittenWord>();
            try
            {

                foreach (var writtenWord in ReadWords())
                {

                    sentence.Add(writtenWord);
                    if (writtenWord.IsEndOfSentence())
                    {
                        var writtenSentence = new WrittenSentence {Words = sentence.ToArray()};

                        result.AddSentence(writtenSentence);
                        sentence.Clear();
                    }
                }
            }
            catch (EndOfStreamException)
            {

                return result;
            }


            return null;
        }

        private IEnumerable<WrittenWord> ReadWords()
        {
            var newWord = new StringBuilder(20);
            while (true)
            {
                
                char c = NextChar();
                if (Char.IsWhiteSpace(c))
                {
                    if (newWord.Length>0) yield return new WrittenWord(newWord.ToString());
                    newWord.Clear();                    
                }
                while (Char.IsWhiteSpace(c)) c = NextChar();
            
                if (Char.IsPunctuation(c))
                {
                    if (newWord.Length>0) yield return new WrittenWord(newWord.ToString());
                    yield return new WrittenWord(c.ToString());
                    newWord.Clear();
                    continue;
                }
                if (Char.IsLetterOrDigit(c))
                {
                    newWord.Append(c);
                }
            }

        }

        private char NextChar()
        {
            var intChar = _fileReader.Read();

            if (intChar == -1)
            {
                throw new EndOfStreamException();
            }
            return (char) intChar;
        }

    }
}
