namespace GeneticAlgorithm.Interfaces
{
    public interface IStopManager
    {
        bool ShouldStop(IChromosome[] population, double[] evaluations, int generation);
    }
}
