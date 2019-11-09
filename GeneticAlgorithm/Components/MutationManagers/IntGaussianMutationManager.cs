using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This operator adds a unit Gaussian distributed random value to the chosen gene. 
    /// If it falls outside of the user-specified lower or upper bounds for that gene, the new gene value is clipped.
    /// </summary>
    public class IntGaussianMutationManager : IMutationManager<int>
    {
        private readonly int minValue;
        private readonly int maxValue;
        private readonly double standardDeviation;

        public IntGaussianMutationManager(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            standardDeviation = (maxValue - minValue) / 4.0;
        }

        public IntGaussianMutationManager(int minValue, int maxValue, double standardDeviation)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            if (standardDeviation < 0)
                throw new GeneticAlgorithmException($"{nameof(standardDeviation)} can't be nagitave");
            this.standardDeviation = standardDeviation;
        }

        public int[] Mutate(int[] vector)
        {
            var mean = (maxValue + minValue) / 2.0;
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = (int) ProbabilityUtils.GaussianDistribution(standardDeviation, mean).Clip(minValue, maxValue);

            return vector;
        }
    }
}
