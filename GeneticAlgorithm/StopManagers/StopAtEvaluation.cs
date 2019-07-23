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

        public bool ShouldStop(IChromosome[] population, double[] evaluations, int generation) =>
            evaluations.Any(evaluation => evaluation >= evaluationToStopAt);
    }
}
