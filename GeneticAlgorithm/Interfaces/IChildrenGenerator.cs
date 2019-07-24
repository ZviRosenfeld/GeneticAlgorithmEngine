namespace GeneticAlgorithm.Interfaces
{
    public interface IChildrenGenerator
    {
        IChromosome[] GenerateChildren(IChromosome[] population, double[] evaluations, int number);
    }
}