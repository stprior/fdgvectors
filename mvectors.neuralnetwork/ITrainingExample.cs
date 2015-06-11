namespace mvectors.neuralnetwork
{
    public interface ITrainingExample
    {
        double[] Input { get; }
        double[] ExpectedResult { get; }
    }
}