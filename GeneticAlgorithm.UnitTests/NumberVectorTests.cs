using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class NumberVectorTests
    {
        private const int VECTOR_SIZE = 10;
        private const int POPULATION_SIZE = 100;
        private readonly IMutationManager<int> mutationManager;
        private readonly IEvaluator evaluator;
        private readonly IPopulationGenerator populationGenerator;
        private readonly ICrossoverManager crossoverManager;
        
        public NumberVectorTests()
        {
            mutationManager = new IntUniformMutationManager(0, 100);
            evaluator = new BasicEvaluator();
            populationGenerator = new IntVectorChromosomePopulationGenerator(VECTOR_SIZE, 0, 1, mutationManager, evaluator);
            crossoverManager = new SinglePointCrossoverManager<int>(mutationManager, evaluator);
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void BassicTest(RunType runType)
        {
            var searchEngine =
                new GeneticSearchEngineBuilder(POPULATION_SIZE, 50, crossoverManager, populationGenerator)
                    .SetSelectionStrategy(new AssertRequestedChromosomesIsRightSelectionWrapper()).Build();

            var result = searchEngine.Run(runType);

            Assert.AreEqual(VECTOR_SIZE, result.BestChromosome.Evaluate());
            Assert.AreEqual(50, result.Generations, "Wrong number of generations");
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        public void MutationTest(RunType runType)
        {
            var searchEngine =
                new GeneticSearchEngineBuilder(POPULATION_SIZE, 50, crossoverManager, populationGenerator)
                    .SetSelectionStrategy(new AssertRequestedChromosomesIsRightSelectionWrapper())
                    .SetMutationProbability(0.1).IncludeAllHistory().Build();

            var result = searchEngine.Run(runType);

            Assert.IsTrue(VECTOR_SIZE < result.BestChromosome.Evaluate(),
                $"best result ({result.BestChromosome.Evaluate()}) should have been greater than {VECTOR_SIZE}. Chromosome is {result.BestChromosome}");
        }

        [TestMethod]
        [DataRow(0.1)]
        [DataRow(0.5)]
        [DataRow(0.219)]
        public void RequestedChromosomesIsRightWithElite(double elite)
        {
            var searchEngine =
                new GeneticSearchEngineBuilder(POPULATION_SIZE, 50, crossoverManager, populationGenerator)
                    .SetElitePercentage(elite)
                    .SetSelectionStrategy(new AssertRequestedChromosomesIsRightSelectionWrapper()).Build();

            searchEngine.Next();
        }
    }
}

