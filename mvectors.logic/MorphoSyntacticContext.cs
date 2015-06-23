using System;

namespace mvectors.logic
{
    public class MorphoSyntacticContext
    {
        public static int VectorLength { get; set; }
        public static double Epsilon { get; set; }
        public double[] Elements { get; set; }

        static MorphoSyntacticContext()
        {
            VectorLength = 5;
            Epsilon = 0.1;
        }
        private MorphoSyntacticContext()
        {
            
        }

        public MorphoSyntacticContext(double[] elements)
        {
            Elements = elements;
        }

        public static MorphoSyntacticContext InitialState()
        {

            var result = new MorphoSyntacticContext {Elements = new double[VectorLength]};
            var r = new Random();
            for (var i = 0; i < result.Elements.Length; i++)
            {
                result.Elements[i] = r.NextDouble()*Epsilon;
            }
            return result;
        }
    }
}
