using FakeItEasy;
using GeneticAlgorithm.Interfaces;
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
        public void StopManagerGetsRightInfoTest(RunType runType)
        {
            var generation = 1;
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.ShouldStop(A<IChromosome[]>._, A<double[]>._, A<int>._)).Invokes(
                (IChromosome[] c, double[] e, int g) =>
                {
                    Assert.AreEqual(generation, g, "Wrong generation");
                    foreach (var chromosome in c)
                        Assert.AreEqual(generation, chromosome.Evaluate(), "Wrong chromosome");
                    foreach (var evaluation in e)
                        Assert.AreEqual(generation, evaluation, "Wrong evaluation");
                    generation++;

                });
            var populationManager = new TestPopulationManager(new[]
                {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {3, 3, 3}});
            var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 3, populationManager)
                .AddStopManager(stopManager).IncludeAllHistory().Build();

            engine.Run(runType);
        }
    }
}
