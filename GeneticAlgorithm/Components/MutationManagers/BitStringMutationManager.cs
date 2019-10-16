using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm.Components.MutationManagers
{
    /// <summary>
    /// This mutation only works on binary chromosomes. It flips bits at random (that is replaces 1 with 0 and 0 with 1).
    /// The probability of a bit being flipped is 1 / <vector-length>.
    /// </summary>
    public class BitStringMutationManager : IMutationManager<int>
    {
        public int[] Mutate(int[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i] != 1 && vector[i] != 0)
                    throw new BadChromosomeTypeException($"{nameof(BitStringMutationManager)} can only work on bit-chromosomes (chromosomes formed of zeros and ones).");

                if (ProbabilityUtils.P(1.0 / vector.Length))
                    vector[i] = vector[i] == 1 ? 0 : 1;
            }

            return vector;
        }
    }
}
