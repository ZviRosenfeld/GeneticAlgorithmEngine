using GeneticAlgorithm.SelectionStrategies;

namespace GeneticAlgorithm.UnitTests.TestUtils
{
    static class Utils
    {
        public static GeneticSearchResult Run(this GeneticSearchEngine engine, RunType runType)
        {
            if (runType == RunType.Run)
                return engine.Run();

            GeneticSearchResult result = null;
            while (result == null || !result.IsCompleted)
                result = engine.Next();

            return result;
        }

        public static  Population Evaluate(this Population population)
        {
            foreach (var chromosome in population)
                chromosome.Evaluation = chromosome.Chromosome.Evaluate();

            return population;
        }

        public static GeneticSearchEngine GetBassicEngine()
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var engineBuilder = new TestGeneticSearchEngineBuilder(5, int.MaxValue, populationManager);
            return engineBuilder.Build();
        }
    }
}
