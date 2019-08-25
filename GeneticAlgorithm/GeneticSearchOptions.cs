using System.Collections.Generic;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchOptions
    {
        public GeneticSearchOptions(int populationSize,List<IStopManager> stopManagers, bool includeAllHistory,
            List<IPopulationRenwalManager> populationRenwalManagers, double elitePercentage, IMutationManager mutationManager, List<IPopulationConverter> populationConverters, IChromosomeEvaluator chromosomeEvaluator)
        {
            StopManagers = stopManagers;
            IncludeAllHistory = includeAllHistory;
            PopulationRenwalManagers = populationRenwalManagers;
            AssertIsBetweenZeroAndOne(elitePercentage, nameof(elitePercentage));
            ElitePercentage = elitePercentage;
            MutationManager = mutationManager;
            PopulationConverters = populationConverters;
            ChromosomeEvaluator = chromosomeEvaluator;

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

        public double ElitePercentage { get; }

        public List<IStopManager> StopManagers { get; }

        public List<IPopulationRenwalManager> PopulationRenwalManagers { get; }

        public IMutationManager MutationManager { get; }

        public IChromosomeEvaluator ChromosomeEvaluator { get; }

        public List<IPopulationConverter> PopulationConverters { get; }
    }
}
