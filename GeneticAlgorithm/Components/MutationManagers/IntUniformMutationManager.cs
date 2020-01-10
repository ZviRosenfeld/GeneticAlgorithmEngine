using System;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This mutation operator replaces the genome with a random value between the lower and upper bound.
    /// The probability of a bit being replaced is 1 / vector-length.
    /// </summary>
    public class IntUniformMutationManager : IMutationManager<int>
    {
        private readonly int minValue;
        private readonly int maxValue;
        private readonly Random random = new Random();

        public IntUniformMutationManager(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public int[] Mutate(int[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = random.Next(minValue, maxValue + 1);

            return vector;
        }
    }
}
