using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class BassicMutationManager : IMutationManager
    {
        private readonly double mutation;

        public BassicMutationManager(double mutation)
        {
            if (mutation > 1 || mutation < 0)
                throw new GeneticAlgorithmException(nameof(mutation) + " must be between 0.0 to 1.0 (including)");

            this.mutation = mutation;
        }

        public void AddGeneration(Population population)
        {
            // Do nothing
        }

        public double MutationProbability(Population population, IEnvironment environment, int generation) => mutation;
    }
}
