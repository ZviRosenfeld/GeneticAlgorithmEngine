namespace GeneticAlgorithm.Interfaces
{
    public interface IPopulationConverter
    {
        /// <summary>
        /// This method is will be called once per generation (after the ConvertPopulation method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(IChromosome[] population, double[] evaluations);

        IChromosome[] ConvertPopulation(IChromosome[] population, int generation);
    }
}
