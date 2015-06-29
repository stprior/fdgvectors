using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace mvectors.logic
{
    public class WordVectorLoader
    {
        private readonly System.Diagnostics.TraceSource _trace = new System.Diagnostics.TraceSource(typeof(WordVectorLoader).ToString());
        protected StreamReader _vectorFileReader;
        readonly int _numDimensions;
        protected readonly int _numEntries;
        
        protected readonly WordVectors WordVectors;

        public WordVectorLoader(string filename)
        {

            if (!File.Exists(filename)) throw new FileNotFoundException(String.Format("{0} not found", filename));
            _vectorFileReader = File.OpenText(filename);
            var readLine = _vectorFileReader.ReadLine();
            if (readLine == null)
            {
                throw new InvalidDataException("Missing header line");
            }
            if (readLine.StartsWith("Copy this file from"))
            {
                throw new InvalidDataException("The file wikipedia_vectors.txt needs to be replaced. " + readLine);
            }
            var headerFields = readLine.Split(' ');
            _numEntries = int.Parse(headerFields[0]);
            _numDimensions = int.Parse(headerFields[1]);

            WordVectors = new WordVectors(_numEntries);
        }

        public virtual WordVectors LoadVectors()
        {
            for (var i = 0; i < _numEntries; i++)
            {
                var nextLine = _vectorFileReader.ReadLine();
                Parse(nextLine);
            }
            return WordVectors;

        }

        public void Parse(string line)
        {
            try
            {
                //            var line = await lineTask;
                var wv = ParseLine(line);
                WordVectors.TryAdd(wv.Name, wv);
            }
            catch (LoaderException)
            { }
        }

        protected WordVector ParseLine(string line)
        {
            var fields = line.Split(' ');
            var name = fields[0];
            var elements = new List<double>();
            //skip first (name) and last (blank)
            for (int i = 1; i < fields.Length-1; i++)
            {
                var element = fields[i];
                try
                {

                    elements.Add(double.Parse(element));
                }
                catch (FormatException fe)
                {

                    _trace.TraceData(System.Diagnostics.TraceEventType.Error, 1, "could not parse element: " + element);
                    throw new LoaderException("could not parse element", fe);
                }
            }
            var wv = new WordVector
           {
               Name = name,               
           };
            elements.CopyTo(wv.Elements);
            return wv;
        }

        [Serializable]
        public class LoaderException : Exception
        {
            public LoaderException() { }
            public LoaderException(string message) : base(message) { }
            public LoaderException(string message, Exception inner) : base(message, inner) { }
            protected LoaderException(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    }
}
