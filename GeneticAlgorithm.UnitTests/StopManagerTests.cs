using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.StopManagers;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class StopManagerTests
    {
        private const int POPULATION_SIZE = 3;
        private const int MAX_GENERATIONS = 100;

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void StopAtEvaluationTest(RunType runType)
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {2, 3, 2}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopAtEvaluation(3)).IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(3, result.Generations);
        }
        
        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void StopAtConvergenceTest(RunType runType)
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 2, 1}, new double[] {2, 6, 2}, new double[] {2, 2.5, 2}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopAtConvergence(0.5)).IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(3, result.Generations);
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void StopIfNoImprovmentTest1(RunType runType)
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {3, 3, 3}, new double[] {4, 4, 4}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopIfNoImprovment(3, 3)).IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(4, result.Generations);
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void StopIfNoImprovmentTest2(RunType runType)
        {
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2.5, 2}, new double[] {3, 3, 3}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopIfNoImprovment(1, 0.9)).IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(3, result.Generations);
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void StopIfNoImprovmentTest3(RunType runType)
        {
            var populationManager = new TestPopulationManager(new[]
            {
                new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {3, 3, 3}, new double[] {4, 4, 4},
                new double[] {4, 4, 4}, new double[] {4, 4, 4}
            });
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, populationManager)
                .AddStopManager(new StopIfNoImprovment(2, 0.9)).IncludeAllHistory().Build();

            var result = engine.Run(runType);

            Assert.AreEqual(6, result.Generations);
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void StopManagerGetsRightInfoTest(RunType runType)
        {
            var generation = 0;
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.ShouldStop(A<Population>._, A<IEnvironment>._, A<int>._)).Invokes(
                (Population p, IEnvironment e, int g) =>
                {
                    Assert.AreEqual(generation, g, "Wrong generation");
                    foreach (var chromosome in p.GetChromosomes())
                        Assert.AreEqual(generation + 1, chromosome.Evaluate(), "Wrong chromosome");
                    foreach (var evaluation in p.GetEvaluations())
                        Assert.AreEqual(generation + 1, evaluation, "Wrong evaluation");
                    generation++;

                });
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {3, 3, 3}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 3, populationManager)
                .AddStopManager(stopManager).IncludeAllHistory().Build();

            engine.Run(runType);
        }

        [TestMethod]
        public void AddMultipleStopManagers_AllManagersAreCalled()
        {
            bool manager1Called = false, manager2Called = false;
            var testPopulationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var managers = new[] { A.Fake<IStopManager>(), A.Fake<IStopManager>() };
            A.CallTo(() => managers[0].ShouldStop(A<Population>._, A<IEnvironment>._, A<int>._))
                .Invokes((Population p, IEnvironment e, int g) => manager1Called = true);
            A.CallTo(() => managers[1].ShouldStop(A<Population>._, A<IEnvironment>._, A<int>._))
                .Invokes((Population p, IEnvironment e, int g) => manager2Called = true);

            var engine = new TestGeneticSearchEngineBuilder(5, 10, testPopulationManager)
                .AddStopManagers(managers)
                .Build();

            engine.Next();

            Assert.IsTrue(manager1Called);
            Assert.IsTrue(manager2Called);
        }
    }
}
