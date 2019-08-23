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
        void SetPopulation(Population population);

        /// <summary>
        /// Selects a chromosme from the population.
        /// </summary>
        IChromosome SelectChromosome();
    }
}
