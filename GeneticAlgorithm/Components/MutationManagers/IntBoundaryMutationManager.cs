using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This mutation operator replaces the genome with either lower or upper bound randomly.
    /// The probability of a bit being replaced is 1 / <vector-length>.
    /// </summary>
    public class IntBoundaryMutationManager : IMutationManager<int>
    {
        private readonly int minValue;
        private readonly int maxValue;

        public IntBoundaryMutationManager(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public int[] Mutate(int[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = ProbabilityUtils.P(0.5) ? maxValue : minValue;

            return vector;
        }
    }
}
