using GeneticAlgorithm.UnitTests.TestUtils;
using GreatestVectorTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class BassicTests
    {
        private const int POPULATION_SIZE = 100;

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void BassicTest(RunType runType)
        {
            var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, 50, new NumberVectorCrossoverManager(),
                new NumberVectorBassicPopulationGenerator()).Build();

            var result = searchEngine.Run(runType);

            Assert.AreEqual(NumberVectorBassicPopulationGenerator.VECTOR_SIZE, result.BestChromosome.Evaluate());
            Assert.AreEqual(50, result.Generations, "Wrong number of generations");
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void MutationTest(RunType runType)
        {
            var searchEngine = new GeneticSearchEngineBuilder(POPULATION_SIZE, 50, new NumberVectorCrossoverManager(),
                new NumberVectorBassicPopulationGenerator()).SetMutationProbability(0.1).Build();

            var result = searchEngine.Run(runType);

            Assert.IsTrue(NumberVectorBassicPopulationGenerator.VECTOR_SIZE < result.BestChromosome.Evaluate(),
                $"best result ({result.BestChromosome.Evaluate()}) should have been greater than {NumberVectorBassicPopulationGenerator.VECTOR_SIZE}");
        }
    }
}

