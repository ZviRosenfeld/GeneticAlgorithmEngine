using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This operator adds a random number taken from a Gaussian distribution with mean equal to the original genome.
    /// </summary>
    public class IntShrinkMutationManager : IMutationManager<int>
    {
        private readonly double minValue;
        private readonly double maxValue;
        private readonly double standardDeviation;

        public IntShrinkMutationManager(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            standardDeviation = (maxValue - minValue) / 4.0;
        }

        public IntShrinkMutationManager(double minValue, double maxValue, double standardDeviation)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            if (standardDeviation < 0)
                throw new GeneticAlgorithmException($"{nameof(standardDeviation)} can't be nagitave");
            this.standardDeviation = standardDeviation;
        }

        public int[] Mutate(int[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = (int) ProbabilityUtils.GaussianDistribution(standardDeviation, vector[i]).Clip(minValue, maxValue);

            return vector;
        }
    }
}
