using System.Text;

namespace mvectors.logic
{
    public class WrittenSentence
    {
        public WrittenSentence()
        {
            Words = new WrittenWord[0];
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            bool first = true;
            foreach (var writtenWord in Words)
            {
                if (first)
                {
                    first = false;
                }else
                {
                    sb.Append(' ');                    
                }
                sb.Append(writtenWord);                
            }
            return sb.ToString();
        }

        public WrittenWord[] Words { get; set; }
        public int Length { get { return Words.Length; } }
    }
}