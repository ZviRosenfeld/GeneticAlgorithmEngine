using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.StopManagers
{
    public class StopAtConvergence : IStopManager
    {
        private readonly double diff;

        /// <summary>
        /// Will stop when the difference between the min evaluation and max evaluation is equal to or less than "diff"
        /// </summary>
        public StopAtConvergence(double diff)
        {
            if (diff < 0)
                throw GeneticAlgorithmArgumentException.SmallerThanZeroException(nameof(diff), diff);

            this.diff = diff;
        }

        public bool ShouldStop(Population population, IEnvironment environment, int generation)
        {
            var minEvaluation = double.MaxValue;
            var maxEvaluation = double.MinValue;

            foreach (var evaluation in population.GetEvaluations())
            {
                if (evaluation < minEvaluation)
                    minEvaluation = evaluation;
                if (evaluation > maxEvaluation)
                    maxEvaluation = evaluation;
            }

            return (maxEvaluation - minEvaluation) <= diff;
        }

        public void AddGeneration(Population population)
        {
            // Do nothing
        }
    }
}
