using System;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// Selects a random two cut points in the string, and  reverses the substring between these two cut points.
    /// SimpleInversionMutationManager guarantees that if the original chromosome contained each genome exactly once, so will the mutated chromosome.
    /// </summary>
    public class SimpleInversionMutationManager<T> : IMutationManager<T>
    {
        private readonly Random random = new Random();

        public T[] Mutate(T[] vector)
        {
            (var start, var end) = random.GetTwoRandomNumbers(vector.Length + 1);

            var newVector = new T[vector.Length];
            for (int i = 0; i < start; i++)
                newVector[i] = vector[i];
            for (int addTo = start, addFrom = end - 1; addTo < end; addTo++, addFrom--)
                newVector[addTo] = vector[addFrom];
            for (int i = end; i < vector.Length; i++)
                newVector[i] = vector[i];

            return newVector;
        }
    }
}
