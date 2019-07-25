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
    }
}
