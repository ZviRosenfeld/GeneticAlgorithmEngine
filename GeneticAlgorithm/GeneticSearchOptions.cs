using System.Collections.Generic;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchOptions
    {
        public GeneticSearchOptions(int populationSize,List<IStopManager> stopManagers, bool includeAllHistory,
            List<IPopulationRenwalManager> populationRenwalManagers, double elitPercentage, IMutationManager mutationManager, IPopulationConverter populationConverter)
        {
            StopManagers = stopManagers;
            IncludeAllHistory = includeAllHistory;
            PopulationRenwalManagers = populationRenwalManagers;
            AssertIsBetweenZeroAndOne(elitPercentage, nameof(elitPercentage));
            ElitPercentage = elitPercentage;
            MutationManager = mutationManager;
            PopulationConverter = populationConverter;

            PopulationSize = populationSize > 0
                ? populationSize
                : throw new GeneticAlgorithmException(nameof(populationSize) + " must be greater then zero");
        }

        private void AssertIsBetweenZeroAndOne(double value, string name)
        {
            if (value > 1 || value < 0)
                throw new GeneticAlgorithmException(name + " must be between 0.0 to 1.0 (including)");
        }

        public int PopulationSize { get; }
        
        public bool IncludeAllHistory { get; }

        public double ElitPercentage { get; }

        public List<IStopManager> StopManagers { get; }

        public List<IPopulationRenwalManager> PopulationRenwalManagers { get; }

        public IMutationManager MutationManager { get; }

        public IPopulationConverter PopulationConverter { get; }
    }
}
