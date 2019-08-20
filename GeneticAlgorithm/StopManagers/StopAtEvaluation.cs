using System.Linq;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.StopManagers
{
    public class StopAtEvaluation : IStopManager
    {
        private double evaluationToStopAt;

        /// <summary>
        /// Will stop when we reach a max evaluation equal to or greater then "evaluationToStopAt" 
        /// </summary>
        public StopAtEvaluation(double evaluationToStopAt)
        {
            this.evaluationToStopAt = evaluationToStopAt;
        }

        public bool ShouldStop(Population population, IEnvironment environment, int generation) =>
            population.GetEvaluations().Any(evaluation => evaluation >= evaluationToStopAt);

        public void AddGeneration(Population population)
        {
            // Do nothing
        }
    }
}
