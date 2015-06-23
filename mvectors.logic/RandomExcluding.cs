using System;
using System.Collections.Generic;
using System.Linq;

namespace mvectors.logic
{
    public static class RandomizeDictionary
    {
        public static IEnumerable<TValue> RandomValues<TKey, TValue>(this IDictionary<TKey, TValue> dict)
        {
            var rand = new Random();
            var values = Enumerable.ToList(dict.Values);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }
    }
}
