using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This operator adds a unit Gaussian distributed random value to the chosen gene. 
    /// If it falls outside of the user-specified lower or upper bounds for that gene, the new gene value is clipped.
    /// </summary>
    public class GaussianMutationManager : IMutationManager<int>
    {
        private readonly int minValue;
        private readonly int maxValue;
        private readonly double variance;

        public GaussianMutationManager(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            variance = (maxValue - minValue) / 2.0;
        }

        public GaussianMutationManager(int minValue, int maxValue, double variance)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.variance = variance;
        }

        public int[] Mutate(int[] vector)
        {
            var mean = (maxValue + minValue) / 2.0;
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = (int) ProbabilityUtils.GaussianDistribution(variance, mean).Clip(minValue, maxValue);

            return vector;
        }
    }
}
