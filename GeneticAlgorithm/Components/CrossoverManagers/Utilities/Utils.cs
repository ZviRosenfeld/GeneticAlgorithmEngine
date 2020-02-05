using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// Returns the shorter chromosome as the first, and the longer chromosome second
        /// </summary>
        public static (VectorChromosome<T>, VectorChromosome<T>) OrderChromosomes<T>(IChromosome chromosome1,
            IChromosome chromosome2)
        {
            var vectorChromosome1 = (VectorChromosome<T>) chromosome1;
            var vectorChromosome2 = (VectorChromosome<T>) chromosome2;
            return vectorChromosome1.GetVector().Length <= vectorChromosome2.GetVector().Length
                ? (vectorChromosome1, vectorChromosome2)
                : (vectorChromosome2, vectorChromosome1);
        }

        /// <summary>
        /// Create a new vector bassed on the neighbors in the adjacencyMatrix.
        /// </summary>
        public static T[] Crossover<T>(this IAdjacencyMatrix<T> adjacencyMatrix, T firstElement, int childLength)
        {
            var childArray = new T[childLength];
            childArray[0] = firstElement;
            for (int i = 1; i < childLength; i++)
                childArray[i] = adjacencyMatrix.GetNeighbor(childArray[i - 1]);

            return childArray;
        }
    }
}
