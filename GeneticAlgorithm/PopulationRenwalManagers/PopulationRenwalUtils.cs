using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm.PopulationRenwalManagers
{
    static class PopulationRenwalUtils
    {
        public static void VerifyPrecentageToRenew(this double precentageToRenew)
        {
            if (precentageToRenew <= 0)
                throw new GeneticAlgorithmArgumentException($"{nameof(precentageToRenew)} can't be smaller or equale to zero (was {precentageToRenew})");
            if (precentageToRenew > 1)
                throw new GeneticAlgorithmArgumentException($"{nameof(precentageToRenew)} can't be greater than one (was {precentageToRenew})");

        }
    }
}
