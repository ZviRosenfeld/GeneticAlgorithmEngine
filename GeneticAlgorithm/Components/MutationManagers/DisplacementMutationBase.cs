using System;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// A base class that will be used for DisplacementMutationManager and InversionMutationManager.
    /// Both managers removes a stretch of genomes from the chromosome, and inserts them in a new location.
    /// The difference is that while DisplacementMutationManager just inserts the stretch at a new location,
    /// InversionMutationManager reverses the stretch before inserting it.
    /// 
    /// For instance, if the original chromosome was 1 2 3 4 5 6 7. We might chose to remove the stretch 3 4 5 and insert it at index 1.
    /// In this case DisplacementMutationManager 1 3 4 5 2 6 7, while InversionMutationManager would give 1 5 4 3 2 6 7.
    /// </summary>
    class DisplacementMutationBase<T> : IMutationManager<T>
    {
        private readonly Random random = new Random();
        private readonly bool inversionSttretch;

        public DisplacementMutationBase(bool inversionSttretch)
        {
            this.inversionSttretch = inversionSttretch;
        }

        public T[] Mutate(T[] vector)
        {
            (var start, var end) = random.GetTwoRandomNumbers(vector.Length + 1);
            var insertionIndex = random.Next(vector.Length - (end - start));
            var vectorWithoutStretch = new T[vector.Length - (end - start)];
            for (int i = 0; i < start; i++)
                vectorWithoutStretch[i] = vector[i];
            for (int addTo = start, addFrom = end; addFrom < vector.Length; addFrom++, addTo++)
                vectorWithoutStretch[addTo] = vector[addFrom];

            var newVector = new T[vector.Length];
            for (int i = 0; i < insertionIndex; i++)
                newVector[i] = vectorWithoutStretch[i];
            if (inversionSttretch)
                for (int addTo = insertionIndex, addFrom = end - 1; addFrom >= start; addFrom--, addTo++)
                    newVector[addTo] = vector[addFrom];
            else
                for (int addTo = insertionIndex, addFrom = start; addFrom < end; addFrom++, addTo++)
                    newVector[addTo] = vector[addFrom];
            for (int addTo = insertionIndex + (end - start), addFrom = insertionIndex; addTo < vector.Length; addFrom++, addTo++)
                newVector[addTo] = vectorWithoutStretch[addFrom];

            return newVector;
        }
    }
}
