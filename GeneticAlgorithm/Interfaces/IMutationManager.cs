namespace GeneticAlgorithm.Interfaces
{
    public interface IMutationManager : IManager
    {
        double MutationProbability(IChromosome[] population, double[] evaluations, int generation);
    }
}
