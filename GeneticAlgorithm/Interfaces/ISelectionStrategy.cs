namespace GeneticAlgorithm.Interfaces
{
    /// <summary>
    /// SelectionStrategies tell the engine how to choose the chromosmes that will create the next generation.
    /// </summary>
    public interface ISelectionStrategy
    {
        /// <summary>
        /// Is called to set the population before the selection starts
        /// </summary>
        /// <param name="population"> The population to select from</param>
        /// <param name="requestedChromosomes"> The total number of chromosomes we are going to need</param>
        void SetPopulation(Population population, int requestedChromosomes);

        /// <summary>
        /// Selects a chromosme from the population.
        /// </summary>
        IChromosome SelectChromosome();
    }
}
