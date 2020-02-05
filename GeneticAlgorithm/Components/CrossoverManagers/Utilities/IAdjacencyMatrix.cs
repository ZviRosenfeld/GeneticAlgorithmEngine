namespace GeneticAlgorithm.Components.CrossoverManagers.Utilities
{
    public interface IAdjacencyMatrix<T>
    {
        /// <summary>
        /// Returns the neighbor with the smallest set of neighbors.
        /// Depending on the implemantation, this may also remove the original element.
        /// </summary>
        T GetNeighbor(T element);
    }
}