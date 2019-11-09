using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;

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
        private readonly double standardDeviation;

        public DoubleGaussianMutationManager(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            standardDeviation = (maxValue - minValue) / 4.0;
        }

        public DoubleGaussianMutationManager(double minValue, double maxValue, double standardDeviation)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            if (standardDeviation < 0)
                throw new GeneticAlgorithmException($"{nameof(standardDeviation)} can't be nagitave");
            this.standardDeviation = standardDeviation;
        }

        public double[] Mutate(double[] vector)
        {
            var mean = (maxValue + minValue) / 2.0;
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = ProbabilityUtils.GaussianDistribution(standardDeviation, mean).Clip(minValue, maxValue);

            return vector;
        }
    }
}
