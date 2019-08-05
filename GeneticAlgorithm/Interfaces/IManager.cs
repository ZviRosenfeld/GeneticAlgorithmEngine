namespace GeneticAlgorithm.Interfaces
{
    public interface IManager
    {
        /// <summary>
        /// This method is will be called once per generation (after the ShouldStop method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(IChromosome[] population, double[] evaluations);
    }
}
