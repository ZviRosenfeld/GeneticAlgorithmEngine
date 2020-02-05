namespace GeneticAlgorithm.Components.CrossoverManagers.Utilities
{
    interface IIndexManager<T>
    {
        int GetIndex(T element);
    }
}