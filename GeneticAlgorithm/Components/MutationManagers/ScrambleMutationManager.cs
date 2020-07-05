using System;
using System.Linq;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// Selects a random two cut points in the string, and scrambles the substring between these two cut points.
    /// ScrambleMutationManager guarantees that if the original chromosome contained each genome exactly once, so will the mutated chromosome.
    /// </summary>
    public class ScrambleMutationManager<T> : IMutationManager<T>
    {
        public T[] Mutate(T[] vector)
        {
            (var start, var end) = ComponetsUtils.GetTwoRandomNumbers(vector.Length + 1);
            var scrambledGenomes = vector.Skip(start).Take(end - start).ToArray().Shuffle();

            var newVector = new T[vector.Length];
            for (int i = 0; i < start; i++)
                newVector[i] = vector[i];
            for (int addTo = start, addFrom = 0; addTo < end; addTo++, addFrom++)
                newVector[addTo] = scrambledGenomes[addFrom];
            for (int i = end; i < vector.Length; i++)
                newVector[i] = vector[i];

            return newVector;
        }
    }
}
