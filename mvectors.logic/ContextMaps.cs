using System.Collections.Generic;

namespace mvectors.logic
{
    public class ContextMaps
    {
        private Dictionary<WrittenWord,ContextMap> _contextMapLookup = new Dictionary<WrittenWord, ContextMap>();

        public ContextMap For(WrittenWord word)
        {
            if (_contextMapLookup.ContainsKey(word))
            {
                return _contextMapLookup[word];
            }
            var contextMap = new ContextMap();
            _contextMapLookup.Add(word,contextMap);
            return contextMap;
        }


        public bool HasKey(WrittenWord writtenWord)
        {
            return _contextMapLookup.ContainsKey(writtenWord);
        }

    }
}