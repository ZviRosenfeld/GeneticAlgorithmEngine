using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This mutation only works on binary chromosomes. It flips bits at random (that is replaces 1 with 0 and 0 with 1).
    /// The probability of a bit being flipped is 1 / <vector-length>.
    /// </summary>
    public class BitStringMutationManager : IMutationManager<bool>
    {
        public bool[] Mutate(bool[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = !vector[i];
            }

            return vector;
        }
    }
}
