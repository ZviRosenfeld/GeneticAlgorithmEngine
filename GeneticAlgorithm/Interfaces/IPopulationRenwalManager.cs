namespace GeneticAlgorithm.Interfaces
{
    public interface IPopulationRenwalManager
    {
        /// <summary>
        /// Returns the percentage of chromosomes to renew
        /// </summary>
        double ShouldRenew(IChromosome[] population, double[] evaluations, int generation);
    }
}
