using System;
using System.Linq;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.MutationManagers
{
    /// <summary>
    /// A MutationManager that will return a mutation based on the population's convergence.
    /// The more homogeneous the population is, the higher the mutation probability will be.
    /// </summary>
    public class ConvergenceMutationManager : IMutationManager
    {
        private double previousConvergence;

        public void AddGeneration(Population population)
        {
            previousConvergence = GetConvergence(population.GetEvaluations());
        }

        public double MutationProbability(Population population, IEnvironment environment, int generation)
        {
            var maxEvaluation = population.GetEvaluations().Max();
            var currentConvergence = GetConvergence(population.GetEvaluations());
            var overallConvergence = Math.Max(currentConvergence, previousConvergence);

            return overallConvergence / maxEvaluation;
        }

        /// <summary>
        /// This method will return the diversity in the population's evaluations.
        /// For most search domains, it would probably be better to implement this method so that it returns the diversity between the chromosomes (and not the evaluations).
        /// </summary>
        private double GetConvergence(double[] evaluations)
        {
            var minEvaluation = evaluations.Min();
            var maxEvaluation = evaluations.Max();

            return maxEvaluation - minEvaluation;
        }
    }
}
