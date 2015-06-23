using mvectors.neuralnetwork;

namespace mvectors.logic
{
    public interface ITrainingAction
    {
        double Train(INeuralNetwork network, MorphoSyntacticContext context);
        void SaveContexts(ContextMaps contextMaps);
    }
}