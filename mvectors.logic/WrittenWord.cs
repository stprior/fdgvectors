using System;
using System.Collections;

namespace mvectors.logic
{
    public class WrittenWord : IEqualityComparer
    {
        public override string ToString()
        {
            return Word;
        }

        public WrittenWord(string text)
        {
            Word = text;
        }
        public string Word { get; set; }

        public bool IsEndOfSentence()
        {
            return Word.Length == 1 && (Word.Equals(".") || Word.Equals("!") || Word.Equals("?"));
        }

        public new bool Equals(object x, object y)
        {
            var xw = x as WrittenWord;
            var xy = y as WrittenWord;
            if (xw == null || xy == null) return false;
            
            return xw.Word.Equals(xy.Word);
        }

        public override bool Equals(object obj)
        {
            var ww = obj as WrittenWord;
            if (ww != null) return Equals(ww);
            return base.Equals(obj);
        }

        public int GetHashCode(object obj)
        {
            var ww = (WrittenWord) obj;
            return ww.Word.GetHashCode();
        }


        public override int GetHashCode()
        {
            return (Word != null ? Word.GetHashCode() : 0);
        }


        protected bool Equals(WrittenWord other)
        {
            return string.Equals(Word, other.Word);
        }

        
        
    }
}