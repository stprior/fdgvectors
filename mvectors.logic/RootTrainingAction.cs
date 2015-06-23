using System;
using System.Collections.Generic;
using mvectors.neuralnetwork;

namespace mvectors.logic
{
    public class RootTrainingAction : ITrainingAction
    {
        protected List<TrainingAction> ChildActions = new List<TrainingAction>();

        public RootTrainingAction(IPlan plan, ContextMaps maps)
        {
            foreach (var contained in plan.ContainedPlans)
            {
                ChildActions.Add(new TrainingAction(contained, maps));
            }

        }

        public virtual double Train(INeuralNetwork network, MorphoSyntacticContext context)
        {
            var error = 0.0;
            var denominator = 0.0;
            foreach (var contained in ChildActions)
            {
                denominator += 1.0;
                error += contained.Train(network, context);
            }
            return (denominator > 0.0 ? error/denominator : 0.0);
        }

        public virtual void SaveContexts(ContextMaps contextMaps)
        {
            SaveContext(contextMaps);
            foreach (var trainingAction in ChildActions)
            {
                trainingAction.SaveContexts(contextMaps);
            }
        }

        public virtual void SaveContext(ContextMaps contextMaps)
        {
            //overridden in derived class
        }
    }
}
