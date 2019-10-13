using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.Interfaces
{
    /// <summary>
    /// An interface to represent a chromosme that's formed by a vercotr of integers
    /// </summary>
    public interface IVectorChromosome : IChromosome
    {
        int[] GetVector();
    }
}
