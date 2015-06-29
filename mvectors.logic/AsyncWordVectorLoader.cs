using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace mvectors.logic
{
    public class AsyncWordVectorLoader : WordVectorLoader
    {        
        private ISourceBlock<string> _linesSource;

        public AsyncWordVectorLoader(string filename) : base(filename)
        {
            _linesSource = ReadWordVecLines(filename);
        }

        public override WordVectors LoadVectors()
        {
            var fileReader = new ActionBlock<string>(line => ParseAsync(line),
                new ExecutionDataflowBlockOptions{MaxDegreeOfParallelism = 6,SingleProducerConstrained = true});
            _linesSource.LinkTo(fileReader);
            _linesSource.Completion.Wait();
            return WordVectors;
        }

// ReSharper disable once CSharpWarnings::CS1998
        public async Task ParseAsync(string line)
        {
            var wv = ParseLine(line);
            WordVectors.TryAdd(wv.Name, wv);
        }



        private ISourceBlock<string> ReadWordVecLines(string fileLocation)
        {
            var block = new BufferBlock<string>(new DataflowBlockOptions {BoundedCapacity = 1000});

            Task.Run(async () =>
            {
                string line;

                using (TextReader file = File.OpenText(fileLocation))
                {
                    while ((line = await file.ReadLineAsync()) != null)
                    {
                        block.Post(line);
                    }
                }

                block.Complete();
            });

            return block;
        }


    }
}
