using System;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// Swaps two genomes in the chromosome.
    /// ExchangeMutationManager guarantees that if the original chromosome contained each genome exactly once, so will the mutated chromosome.
    /// </summary>
    public class ExchangeMutationManager<T> : IMutationManager<T>
    {
        public T[] Mutate(T[] vector)
        {
            int from = ProbabilityUtils.GetRandomInt(vector.Length);
            int to = ProbabilityUtils.GetRandomInt(vector.Length);

            var temp = vector[to];
            vector[to] = vector[from];
            vector[from] = temp;

            return vector;
        }
    }
}
