using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace mvectors.logic
{
    /// <summary>
    /// Represents intent to train that a word makes a LinguisticExpression
    /// complete, incomplete or incorrect given a particular context. Plans 
    /// are nested to represent a series of words that will be trained in 
    /// sequence carrying context. Plans are instantiated into a set of 
    /// actions (an ActionPlan)
    /// </summary>
    public class TrainingPlan : IPlan
    {
        private List<TrainingPlan> _containedPlans = new List<TrainingPlan>();
        public WrittenWord Word { get; private set; }
        public TrainingOutput? Output { get; private set; }

        public IEnumerable<IPlan> ContainedPlans
        {
            get { return _containedPlans; }
        }

        private TrainingPlan(WrittenWord word, TrainingOutput? output)
        {
            Word = word;
            Output = output;
        }

        public static IPlan InitialPlan()
        {
            return new TrainingPlan(null, null);
        }

        /// <summary>
        /// Intended primarily to support tests. Does simple breadth first search for a given training example
        /// </summary>
        /// <param name="word"></param>
        /// <param name="output"></param>
        /// <returns></returns>
        public bool ContainsPlanStep(WrittenWord word, TrainingOutput output)
        {
            var cmp = new PlanComparer();
            return _containedPlans.Contains(new TrainingPlan(word, output), cmp)
                || _containedPlans.Any(containedPlan => containedPlan.ContainsPlanStep(word, output));            
        }

        private class PlanComparer : IEqualityComparer<TrainingPlan>
        {
            public bool Equals(TrainingPlan x, TrainingPlan y)
            {
                //TODO: consider eliminating this check with seperate class for root
                if (x.Word == null || x.Output == null) return false;
                var result = x.Word.Equals(y.Word) && x.Output.Equals(y.Output);
                return result;
            }

            public int GetHashCode(TrainingPlan obj)
            {
                return obj.Word.GetHashCode();
            }
        }

        public IPlan AddPlanStep(WrittenWord writtenWord, TrainingOutput output)
        {
            var planStep = new TrainingPlan(writtenWord, output);
            _containedPlans.Add(planStep);
            return planStep;
        }

    }

    public enum TrainingOutput
    {
        Incomplete,
        Complete,
        Incorrect
    };


    public interface IPlan
    {
        IPlan AddPlanStep(WrittenWord writtenWord, TrainingOutput output);
        bool ContainsPlanStep(WrittenWord word, TrainingOutput output);
        WrittenWord Word { get; }
        TrainingOutput? Output { get; }
        IEnumerable<IPlan> ContainedPlans { get; }
    }

}