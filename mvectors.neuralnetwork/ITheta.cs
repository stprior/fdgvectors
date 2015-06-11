namespace mvectors.neuralnetwork
{
    public interface ITheta
    {
        void LoadFrom(double[] weights, int startFrom = 0);
        double[] SaveTo(int startFrom = 0);
    }
}