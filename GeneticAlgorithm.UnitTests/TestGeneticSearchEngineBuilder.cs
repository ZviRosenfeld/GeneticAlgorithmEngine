namespace GeneticAlgorithm.UnitTests
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
        
        public override GeneticSearchEngine Build()
        {
            var options = new GeneticSearchOptions(populationSize,
                mutationProbability, stopManagers, includeAllHistory, populationRenwalManagers, elitPercentage);
            return new GeneticSearchEngine(options, populationGenerator, populationManager.GetChildrenGenerator());
        }
    }
}
