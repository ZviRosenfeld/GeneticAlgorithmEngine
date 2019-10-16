using System;
using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This mutation operator replaces the genome with a random value between the lower and upper bound.
    /// The probability of a bit being replaced is 1 / <vector-length>.
    /// </summary>
    public class DoubleUniformMutationManager : IMutationManager<double>
    {
        private readonly double minValue;
        private readonly double range;
        private readonly Random random = new Random();

        public DoubleUniformMutationManager(double minValue, double maxValue)
        {
            this.minValue = minValue;
            range = maxValue - minValue;
        }

        public double[] Mutate(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = minValue + random.NextDouble() * range;

            return vector;
        }
    }
}
