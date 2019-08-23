namespace GeneticAlgorithm.Interfaces
{
    /// <summary>
    /// IMutationManagers lets you dynamically determine the probability of a mutation based on the current population.
    /// </summary>
    public interface IMutationManager
    {
        /// <summary>
        /// This method is will be called once per generation (after the MutationProbability method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(Population population);

        /// <summary>
        /// Returns the probability for a mutation
        /// </summary>
        double MutationProbability(Population population, IEnvironment environment, int generation);
    }
}
