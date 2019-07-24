using GeneticAlgorithm.PopulationRenwalManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class PopulationRenwalManagerTests
    {
        private const int POPULATION_SIZE = 3;
        private const int MAX_GENERATIONS = 3;

        [TestMethod]
        public void RenewAtConvergenceTest()
        {
            var populationManager = new TestPopulationManager(new double[] {1, 1, 1});
            populationManager.SetPopulationGenerated(new[]
                {new double[] {2, 2, 2}, new double[] {3, 3, 3}, new double[] {4, 4, 4}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddPopulationRenwalManager(new RenewAtConvergence(0.9, 1))
                .IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(4, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        public void RenewOnlySomeChromosomes()
        {
            var populationManager = new TestPopulationManager(new double[] { 4, 4, 4 });
            populationManager.SetPopulationGenerated(new[]
                {new double[] {3, 3, 3}, new double[] {2, 2, 2}, new double[] {1, 1, 1}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddPopulationRenwalManager(new RenewAtConvergence(0.9, 0.5))
                .IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(4, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        public void RenewIfNoImprovmentTest1()
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 });
            populationManager.SetPopulationGenerated(new[]
                {new double[] {2, 2, 2}, new double[] {3, 3, 3}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddPopulationRenwalManager(new RenewIfNoImprovment(1, 3, 1)).IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(3, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        public void RenewIfNoImprovmentTest2()
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 });
            populationManager.SetPopulationGenerated(new[] {new double[] {2, 2, 2}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddPopulationRenwalManager(new RenewIfNoImprovment(2, 10, 1)).IncludeAllHistory().Build();

            var result = engine.Search();

            Assert.AreEqual(2, result.BestChromosome.Evaluate());
        }
    }
}
