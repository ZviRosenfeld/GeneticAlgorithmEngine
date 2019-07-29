namespace GeneticAlgorithm.UnitTests
{
    static class TestUtils
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
    }
}
