using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.PopulationRenwalManagers
{
    public class RenewAtConvergence : IPopulationRenwalManager
    {
        private readonly double precentageToRenew;
        private readonly IStopManager stopManager;

        /// <summary>
        /// Will renew "precentageToRenew" of the population when the difference between the min evaluation and max evaluation is equal to or less than "diff"
        /// </summary>
        public RenewAtConvergence(double diff, double precentageToRenew)
        {
            this.precentageToRenew = precentageToRenew;
            stopManager = new StopManagers.StopAtConvergence(diff);
        }

        public double ShouldRenew(IChromosome[] population, double[] evaluations, int generation) =>
            stopManager.ShouldStop(population, evaluations, generation) ? precentageToRenew : 0;

        public void AddGeneration(IChromosome[] population, double[] evaluations) =>
            stopManager.AddGeneration(population, evaluations);
    }
}
