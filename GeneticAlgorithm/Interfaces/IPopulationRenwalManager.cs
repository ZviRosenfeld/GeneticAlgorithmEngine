namespace GeneticAlgorithm.Interfaces
{
    /// <summary>
    /// A populationRenwalManager will tell the einge to renew a certain percentage of the population if some condition is met.
    /// </summary>
    public interface IPopulationRenwalManager
    {
        /// <summary>
        /// This method is will be called once per generation (after the ShouldRenew method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(Population population);

        /// <summary>
        /// Returns the percentage of chromosomes to renew
        /// </summary>
        double ShouldRenew(Population population, IEnvironment environment, int generation);
    }
}
