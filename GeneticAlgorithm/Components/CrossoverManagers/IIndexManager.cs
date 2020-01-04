namespace GeneticAlgorithm.Components.CrossoverManagers
{
    interface IIndexManager<T>
    {
        int GetIndex(T element);
    }
}