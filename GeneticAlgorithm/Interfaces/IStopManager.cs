namespace GeneticAlgorithm.Interfaces
{
    public interface IStopManager : IManager
    {
        bool ShouldStop(IChromosome[] population, double[] evaluations, int generation);
    }
}
