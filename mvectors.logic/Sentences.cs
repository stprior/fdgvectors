using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace mvectors.logic
{
    public class Sentences
    {
        private IDictionary<int, List<WrittenSentence>> _sentences = new Dictionary<int, List<WrittenSentence>>();
        private IDictionary<WrittenWord, List<WrittenSentence>> _firstWordsDictionary = new Dictionary<WrittenWord, List<WrittenSentence>>();
        public void AddSentence(WrittenSentence sentence)
        {
            if (_sentences.ContainsKey(sentence.Length - 1))
            {
                _sentences[sentence.Length - 1].Add(sentence);
            }
            else
            {
                _sentences.Add(sentence.Length-1, new List<WrittenSentence>{sentence});
            }
            if (_firstWordsDictionary.ContainsKey(sentence.Words[0]))
            {
                _firstWordsDictionary[sentence.Words[0]].Add(sentence);
            }
            else
            {
                _firstWordsDictionary.Add(sentence.Words[0], new List<WrittenSentence>{ sentence });
            }
        }

        public IEnumerable<WrittenSentence> SentencesOfLength(int length)
        {
            return _sentences.ContainsKey(length) ? 
                _sentences[length] : 
                Enumerable.Empty<WrittenSentence>();
        }

        public IEnumerable<WrittenWord> FirstWords()
        {

            return _firstWordsDictionary.OrderByDescending(t => t.Value.Count).Select(kvp => kvp.Key);
        }

        public IEnumerable AllSentences()
        {
            throw new NotImplementedException();
        }
    }
}