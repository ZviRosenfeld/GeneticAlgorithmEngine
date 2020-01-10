using System;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// Removes a random genome and inserts it at a new location.
    /// InsertionMutationManager guarantees that if the original chromosome contained each genome exactly once, so will the mutated chromosome.
    /// </summary>
    public class InsertionMutationManager<T> : IMutationManager<T>
    {
        private readonly Random random = new Random();

        public T[] Mutate(T[] vector)
        {
            var toRemove = random.Next(vector.Length);
            var insertAt = random.Next(vector.Length);

            var newVector = new T[vector.Length];
            if (toRemove < insertAt)
            {
                for (int i = 0; i < toRemove; i++)
                    newVector[i] = vector[i];
                for (int i = toRemove; i < insertAt; i++)
                    newVector[i] = vector[i + 1];
                newVector[insertAt] = vector[toRemove];
                for (int i = insertAt + 1; i < vector.Length; i++)
                    newVector[i] = vector[i];
            }
            else
            {
                for (int i = 0; i < insertAt; i++)
                    newVector[i] = vector[i];
                newVector[insertAt] = vector[toRemove];
                for (int i = insertAt + 1; i <= toRemove; i++)
                    newVector[i] = vector[i - 1];
                for (int i = toRemove + 1; i < vector.Length; i++)
                    newVector[i] = vector[i];
            }

            return newVector;
        }
    }
}
