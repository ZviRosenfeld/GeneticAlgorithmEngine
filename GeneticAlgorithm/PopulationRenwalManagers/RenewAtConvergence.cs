using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.PopulationRenwalManagers
{
    /// <summary>
    /// Will renew "precentageToRenew" of the population when the difference between the min evaluation and max evaluation is equal to or less than "diff".
    /// </summary>
    public class RenewAtConvergence : IPopulationRenwalManager
    {
        private readonly double precentageToRenew;
        private readonly IStopManager stopManager;

        /// <summary>
        /// Will renew "precentageToRenew" of the population when the difference between the min evaluation and max evaluation is equal to or less than "diff".
        /// </summary>
        public RenewAtConvergence(double diff, double precentageToRenew)
        {
            this.precentageToRenew = precentageToRenew;
            stopManager = new StopManagers.StopAtConvergence(diff);
        }

        public double ShouldRenew(Population population, IEnvironment environment, int generation) =>
            stopManager.ShouldStop(population, environment, generation) ? precentageToRenew : 0;

        public void AddGeneration(Population population) =>
            stopManager.AddGeneration(population);
    }
}
