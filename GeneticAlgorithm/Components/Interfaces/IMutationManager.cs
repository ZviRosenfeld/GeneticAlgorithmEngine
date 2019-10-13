namespace GeneticAlgorithm.Components.Interfaces
{
    public interface IMutationManager<T>
    {
        T[] Mutate(T[] vector);
    }
}
