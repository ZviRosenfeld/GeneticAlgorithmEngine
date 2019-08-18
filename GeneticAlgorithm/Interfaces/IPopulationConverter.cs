namespace GeneticAlgorithm.Interfaces
{
    public interface IPopulationConverter
    {
        /// <summary>
        /// This method is will be called once per generation (after the ConvertPopulation method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(Population population);

        /// <summary>
        /// This method will be called every generation after the population is created. 
        /// In this method you can change the population in any way you want.
        /// This allows you to add Lamarckian evolution to your algorithm - that is, let the chromosomes improve themselves before generating the children.
        /// </summary>
        IChromosome[] ConvertPopulation(IChromosome[] population, int generation, IEnvironment environment);
    }
}
