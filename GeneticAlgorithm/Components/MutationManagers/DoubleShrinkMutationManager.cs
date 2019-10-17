using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This operator adds a random number taken from a Gaussian distribution with mean equal to the original genome.
    /// </summary>
    public class DoubleShrinkMutationManager : IMutationManager<double>
    {
        private readonly double minValue;
        private readonly double maxValue;
        private readonly double standardDeviation;

        public DoubleShrinkMutationManager(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            standardDeviation = (maxValue - minValue) / 4.0;
        }

        public DoubleShrinkMutationManager(double minValue, double maxValue, double standardDeviation)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            if (standardDeviation < 0)
                throw new GeneticAlgorithmException($"{nameof(standardDeviation)} can't be nagitave");
            this.standardDeviation = standardDeviation;
        }

        public double[] Mutate(double[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = ProbabilityUtils.GaussianDistribution(standardDeviation, vector[i]).Clip(minValue, maxValue);

            return vector;
        }
    }
}
