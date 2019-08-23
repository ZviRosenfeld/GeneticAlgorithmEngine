using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.PopulationRenwalManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class PopulationRenwalManagerTests
    {
        private const int POPULATION_SIZE = 3;

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewAtConvergenceTest(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] {1, 1, 1});
            populationManager.SetPopulationGenerated(new[]
                {new double[] {2, 2, 2}, new double[] {3, 3, 3}, new double[] {4, 4, 4}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewAtConvergence(0.9, 1))
                .IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(4, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewOnlySomeChromosomes(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 4, 4, 4 });
            populationManager.SetPopulationGenerated(new[]
                {new double[] {3, 3, 3}, new double[] {2, 2, 2}, new double[] {1, 1, 1}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewAtConvergence(0.9, 0.5))
                .IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(4, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewIfNoImprovmentTest1(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 });
            populationManager.SetPopulationGenerated(new[]
                {new double[] {2, 2, 2}, new double[] {3, 3, 3}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewIfNoImprovment(1, 3, 1)).IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(3, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewIfNoImprovmentTest2(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 });
            populationManager.SetPopulationGenerated(new[] {new double[] {2, 2, 2}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewIfNoImprovment(2, 10, 1)).IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(2, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void PopulationRenwalManagerGetsRightInfoTest(RunType runType)
        {
            var generation = 1;
            var populationRenwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenwalManager.ShouldRenew(A<Population>._, A<IEnvironment>._, A<int>._)).Invokes(
                (Population p, IEnvironment e, int g) =>
                {
                    Assert.AreEqual(generation, g, "Wrong generation");
                    foreach (var chromosome in p.GetChromosomes())
                        Assert.AreEqual(generation, chromosome.Evaluate(), "Wrong chromosome");
                    foreach (var evaluation in p.GetEvaluations())
                        Assert.AreEqual(generation, evaluation, "Wrong evaluation");
                    generation++;

                });
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {3, 3, 3}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 3, populationManager)
                .AddPopulationRenwalManager(populationRenwalManager).IncludeAllHistory().Build();

            engine.Run(runType);
        }

        [TestMethod]
        public void AddMultiplePopulationRenewalManagers_AllManagersAreCalled()
        {
            bool manager1Called = false, manager2Called = false;
            var testPopulationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var managers = new[] { A.Fake<IPopulationRenwalManager>(), A.Fake<IPopulationRenwalManager>() };
            A.CallTo(() => managers[0].ShouldRenew(A<Population>._, A<IEnvironment>._, A<int>._))
                .Invokes((Population p, IEnvironment e, int g) => manager1Called = true);
            A.CallTo(() => managers[1].ShouldRenew(A<Population>._, A<IEnvironment>._, A<int>._))
                .Invokes((Population p, IEnvironment e, int g) => manager2Called = true);

            var engine = new TestGeneticSearchEngineBuilder(5, 10, testPopulationManager)
                .AddPopulationRenwalManagers(managers)
                .Build();

            engine.Next();

            Assert.IsTrue(manager1Called);
            Assert.IsTrue(manager2Called);
        }
    }
}
