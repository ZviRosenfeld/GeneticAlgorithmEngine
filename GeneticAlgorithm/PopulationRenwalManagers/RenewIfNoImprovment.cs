using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.StopManagers;

namespace GeneticAlgorithm.PopulationRenwalManagers
{
    public class RenewIfNoImprovment : IPopulationRenwalManager
    {
        private readonly double precentageToRenew;
        private readonly IStopManager stopManager;

        /// <summary>
        /// Will renew "precentageToRenew" of the population when there isn't an improvement of at least "minImprovment" after "generations" generations
        /// </summary>
        public RenewIfNoImprovment(int generations, double minImprvment,  double precentageToRenew)
        {
            this.precentageToRenew = precentageToRenew;
            stopManager = new StopIfNoImprovment(generations, minImprvment);
        }

        public double ShouldRenew(IChromosome[] population, double[] evaluations, int generation) =>
            stopManager.ShouldStop(population, evaluations, generation) ? precentageToRenew : 0;

        public void AddGeneration(IChromosome[] population, double[] evaluations)
        {
            stopManager.AddGeneration(population, evaluations);
        }
    }
}
