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
            this.diff = diff;
        }

        public bool ShouldStop(IChromosome[] population, double[] evaluations, int generation)
        {
            var minEvaluation = double.MaxValue;
            var maxEvaluation = double.MinValue;

            foreach (var evaluation in evaluations)
            {
                if (evaluation < minEvaluation)
                    minEvaluation = evaluation;
                if (evaluation > maxEvaluation)
                    maxEvaluation = evaluation;
            }

            return (maxEvaluation - minEvaluation) <= diff;
        }
    }
}
