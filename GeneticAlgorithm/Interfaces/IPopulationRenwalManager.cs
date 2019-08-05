namespace GeneticAlgorithm.Interfaces
{
    public interface IPopulationRenwalManager : IManager
    {
        /// <summary>
        /// Returns the percentage of chromosomes to renew
        /// </summary>
        double ShouldRenew(IChromosome[] population, double[] evaluations, int generation);
    }
}
