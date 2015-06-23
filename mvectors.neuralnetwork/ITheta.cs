namespace mvectors.neuralnetwork
{
    public interface ITheta
    {
        void LoadFrom(double[] weights);
        void SaveTo(ref double[] weights);
    }
}