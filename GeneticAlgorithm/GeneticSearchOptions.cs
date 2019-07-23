using System.Collections.Generic;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchOptions
    {
        public GeneticSearchOptions(int populationSize, int maxGenerations, double mutationProbability,
            List<IStopManager> stopManagers, bool includeAllHistory,
            List<IPopulationRenwalManager> populationRenwalManagers)
        {
            if (mutationProbability > 1 || mutationProbability < 0)
                throw new GeneticAlgorithmException(nameof(mutationProbability) +
                                                    " must be between 0.0 to 1.0 (including)");
            MutationProbability = mutationProbability;
            StopManagers = stopManagers;
            IncludeAllHistory = includeAllHistory;
            PopulationRenwalManagers = populationRenwalManagers;

            MaxGenerations = maxGenerations > 0
                ? maxGenerations
                : throw new GeneticAlgorithmException(nameof(maxGenerations) + " must be greater then zero");
            PopulationSize = populationSize > 0
                ? populationSize
                : throw new GeneticAlgorithmException(nameof(populationSize) + " must be greater then zero");
        }

        public int PopulationSize { get; }

        public int MaxGenerations { get; }

        public double MutationProbability { get; }
        
        public bool IncludeAllHistory { get; }

        public List<IStopManager> StopManagers { get; }

        public List<IPopulationRenwalManager> PopulationRenwalManagers { get; }
    }
}
