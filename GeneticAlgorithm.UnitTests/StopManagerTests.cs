using GeneticAlgorithm.StopManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class StopManagerTests
    {
        private const int POPULATION_SIZE = 3;
        private const int MAX_GENERATIONS = 100;

        [TestMethod]
        public void StopAtEvaluationTest()
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {2, 3, 2}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopAtEvaluation(3)).IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(2, result.Generations);
        }

        [TestMethod]
        public void StopAtConvergenceTest()
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 2, 1}, new double[] {2, 6, 2}, new double[] {2, 2.5, 2}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopAtConvergence(0.5)).IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(2, result.Generations);
        }

        [TestMethod]
        public void StopIfNoImprovmentTest1()
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {3, 3, 3}, new double[] {4, 4, 4}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopIfNoImprovment(3, 3)).IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(3, result.Generations);
        }

        [TestMethod]
        public void StopIfNoImprovmentTest2()
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2.5, 2}, new double[] {3, 3, 3}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopIfNoImprovment(1, 0.9)).IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(2, result.Generations);
        }
    }
}
