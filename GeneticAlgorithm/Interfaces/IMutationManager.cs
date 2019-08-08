namespace GeneticAlgorithm.Interfaces
{
    public interface IMutationManager
    {
        /// <summary>
        /// This method is will be called once per generation (after the MutationProbability method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(IChromosome[] population, double[] evaluations);

        double MutationProbability(IChromosome[] population, double[] evaluations, int generation);
    }
}
