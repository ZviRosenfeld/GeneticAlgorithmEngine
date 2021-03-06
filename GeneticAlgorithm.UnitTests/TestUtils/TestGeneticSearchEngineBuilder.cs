﻿namespace GeneticAlgorithm.UnitTests.TestUtils
{
    class TestGeneticSearchEngineBuilder : GeneticSearchEngineBuilder
    {
        private readonly TestPopulationManager populationManager;

        public TestGeneticSearchEngineBuilder(int populationSize, int maxGenerations,
            TestPopulationManager populationManager) : base(populationSize,
            maxGenerations, populationManager.GetCrossoverManager(), populationManager.GetPopulationGenerator())
        {
            this.populationManager = populationManager;
        }

        public TestGeneticSearchEngineBuilder(int populationSize, int maxGenerations,
            double[] population) : this(populationSize,
            maxGenerations, new TestPopulationManager(population))
        {
        }

        public override GeneticSearchEngine Build()
        {
            PreBuildActions();

            var options = new GeneticSearchOptions(populationSize, stopManagers, includeAllHistory,
                populationRenwalManagers, elitePercentage, mutationManager, populationConverters, chromosomeEvaluator);
            return new GeneticSearchEngine(options, populationGenerator, populationManager.GetChildrenGenerator(), environment);
        }
    }
}
