using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This operator adds a unit Gaussian distributed random value to the chosen gene. 
    /// If it falls outside of the user-specified lower or upper bounds for that gene, the new gene value is clipped.
    /// </summary>
    public class DoubleGaussianMutationManager : IMutationManager<double>
    {
        private readonly double minValue;
        private readonly double maxValue;
        private readonly double variance;

        public DoubleGaussianMutationManager(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            variance = (maxValue - minValue) / 2.0;
        }

        public DoubleGaussianMutationManager(double minValue, double maxValue, double variance)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.variance = variance;
        }

        public double[] Mutate(double[] vector)
        {
            var mean = (maxValue + minValue) / 2.0;
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = ProbabilityUtils.GaussianDistribution(variance, mean).Clip(minValue, maxValue);

            return vector;
        }
    }
}
