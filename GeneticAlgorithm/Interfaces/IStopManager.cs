namespace GeneticAlgorithm.Interfaces
{
    public interface IStopManager
    {
        bool ShouldStop(IChromosome[] population, double[] evaluations, int generation);

        /// <summary>
        /// This method is will be called once per generation (after the ShouldStop method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(IChromosome[] population, double[] evaluations);
    }
}
