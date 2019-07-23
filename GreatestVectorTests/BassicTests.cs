using GeneticAlgorithm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GreatestVectorTests
{
    [TestClass]
    public class BassicTests
    {
        public const int POPULATION_SIZE = 100;

        [TestMethod]
        public void BassicTest()
        {
            var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, 50, new NumberVectorCrossoverManager(),
                new NumberVectorBassicPopulationGenerator()).Build();

            var result = searchEngine.Search();

            Assert.AreEqual(NumberVectorBassicPopulationGenerator.VECTOR_SIZE, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        public void MutationTest()
        {
            var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, 50, new NumberVectorCrossoverManager(),
                new NumberVectorBassicPopulationGenerator()).SetMutationProbability(0.1).Build();

            var result = searchEngine.Search();

            Assert.IsTrue(NumberVectorBassicPopulationGenerator.VECTOR_SIZE < result.BestChromosome.Evaluate(),
                $"best result ({result.BestChromosome.Evaluate()}) should have been greater than {NumberVectorBassicPopulationGenerator.VECTOR_SIZE}");
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void HistoryTest(bool includeHistory)
        {
            var enigneBuilder = new GeneticSearchEngineBuilder(POPULATION_SIZE, 2, new NumberVectorCrossoverManager(),
                new NumberVectorBassicPopulationGenerator());
            if (includeHistory)
                enigneBuilder = enigneBuilder.IncludeAllHistory();
            var searchEngine = enigneBuilder.Build();

            var result = searchEngine.Search();

            if (includeHistory)
                Assert.AreEqual(3, result.History.Count, "There should have been history");
            else
                Assert.AreEqual(0, result.History.Count, "There shouldn't be any history");
        }
    }
}
