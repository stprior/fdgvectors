using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace mvectors.logic
{
    public class WordVectors
    {
        readonly ConcurrentDictionary<string, WordVector> _wordVectorDictionary;
        private IEnumerator<WordVector> _randomValueSequence = null;
        public WordVectors()
        {
            _wordVectorDictionary = new ConcurrentDictionary<string, WordVector>();
        }
        public WordVectors(int numVectors)
        {
            _wordVectorDictionary = new ConcurrentDictionary<string, WordVector>(2, numVectors);
        }

        public bool TryAdd(string key, WordVector value)
        {
            return _wordVectorDictionary.TryAdd(key, value);
        }

        public WordVector this[WrittenWord writtenWord]
        {
            get { return _wordVectorDictionary.ContainsKey(writtenWord.Word) ?  _wordVectorDictionary[writtenWord.Word] : null; }
        }

        public WordVector RandomExcluding(HashSet<WordVector> exclusions)
        {
            if (_randomValueSequence == null) _randomValueSequence = _wordVectorDictionary.RandomValues().GetEnumerator();
            _randomValueSequence.MoveNext(); 
            var value = _randomValueSequence.Current;
            while (exclusions.Contains(value))
            {
                _randomValueSequence.MoveNext();
                value = _randomValueSequence.Current;
            }
            
            return value;
        }

    }
}
