namespace GeneticAlgorithm.Interfaces
{
    public interface IChildrenGenerator
    {
        IChromosome[] GenerateChildren(Population population, int number);
    }
}