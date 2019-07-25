namespace GeneticAlgorithm.Interfaces
{
    public interface IPopulationRenwalManager
    {
        /// <summary>
        /// Returns the percentage of chromosomes to renew
        /// </summary>
        double ShouldRenew(IChromosome[] population, double[] evaluations, int generation);

        /// <summary>
        /// This method is will be called once per generation (after the ShouldRenew method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(IChromosome[] population, double[] evaluations);
    }
}
