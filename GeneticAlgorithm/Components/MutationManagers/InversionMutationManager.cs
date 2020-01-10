using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// Removes a stretch of genomes from the chromosome, inverses them, and inserts them in a new location.
    /// For instance, if the original chromosome was 1 2 3 4 5 6 7. We might chose to remove the stretch 3 4 5 and insert it at index 1.
    /// This would result in the chromosome 1 5 4 3 2 6 7.
    /// 
    /// DisplacementMutationManager guarantees that if the original chromosome contained each genome exactly once, so will the mutated chromosome.
    /// </summary>
    public class InversionMutationManager<T> : IMutationManager<T>
    {
        private readonly DisplacementMutationBase<T> displacementMutationBase = new DisplacementMutationBase<T>(true);

        public T[] Mutate(T[] vector) => displacementMutationBase.Mutate(vector);
    }
}
