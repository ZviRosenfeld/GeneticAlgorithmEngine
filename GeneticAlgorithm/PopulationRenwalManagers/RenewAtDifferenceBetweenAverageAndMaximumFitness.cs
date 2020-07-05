using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
using System.Linq;

namespace GeneticAlgorithm.PopulationRenwalManagers
{
    /// <summary>
    /// Will renew "precentageToRenew" of the population when the difference between the average and max evaluation is equal to or less than "diff".
    /// </summary>
    public class RenewAtDifferenceBetweenAverageAndMaximumFitness : IPopulationRenwalManager
    {
        private readonly double precentageToRenew;
        private readonly double diff;

        /// <summary>
        /// Will renew "precentageToRenew" of the population when the difference between the min evaluation and max evaluation is equal to or less than "diff".
        /// </summary>
        public RenewAtDifferenceBetweenAverageAndMaximumFitness(double diff, double precentageToRenew)
        {
            precentageToRenew.VerifyPrecentageToRenew();
            this.precentageToRenew = precentageToRenew;
            this.diff = diff >= 0 ? diff : throw GeneticAlgorithmArgumentException.SmallerThanZeroException(nameof(diff), diff); ;
        }

        public void AddGeneration(Population population)
        {
            // nothing to do here
        }

        public double ShouldRenew(Population population, IEnvironment environment, int generation)
        {
            var max = population.Select(c => c.Evaluation).Max();
            var average = population.Select(c => c.Evaluation).Average();

            return max - average < diff ? precentageToRenew : 0;
        }
    }
}
