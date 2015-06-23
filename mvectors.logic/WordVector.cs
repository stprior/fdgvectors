using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mvectors.logic
{
    public class WordVector
    {
        private static int _vectorLength = 100;
        public static int VectorLength
        {
            get { return _vectorLength; }
            set { _vectorLength = value; }
        }

        public WordVector()
        {
            _elements = new double[VectorLength];
        }
        private double[] _elements;
        
        public string Name { get; set; }

        public double[] Elements
        {
            get { return _elements; }            
        }
        
    }
}
