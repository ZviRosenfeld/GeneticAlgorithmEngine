using System;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// Swaps two genomes in the chromosome
    /// </summary>
    public class SingleSwapMutationManager<T> : IMutationManager<T>
    {
        private readonly Random random = new Random();

        public T[] Mutate(T[] vector)
        {
            int from = random.Next(vector.Length);
            int to = random.Next(vector.Length);

            var temp = vector[to];
            vector[to] = vector[from];
            vector[from] = temp;

            return vector;
        }
    }
}
