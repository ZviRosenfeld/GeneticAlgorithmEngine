namespace GeneticAlgorithm.Interfaces
{
    public interface ICrossoverManager
    {
        IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2);
    }
}
