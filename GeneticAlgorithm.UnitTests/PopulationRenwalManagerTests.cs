﻿using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.PopulationRenwalManagers;
using GeneticAlgorithm.UnitTests.TestUtils;
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
                {new double[] {2, 3, 2}, new double[] {4, 4, 4}, new double[] {5, 5, 5}});
            using (var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewAtConvergence(0.9, 1))
                .IncludeAllHistory().Build())
            {

                var result = engine.Run(runType);

                Assert.AreEqual(3, result.BestChromosome.Evaluate());
            }
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewOnlySomeChromosomes(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 4, 4, 4 });
            populationManager.SetPopulationGenerated(new[]
                {new double[] {3, 3, 3}, new double[] {2, 2, 2}, new double[] {1, 1, 1}});
            using (var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewAtConvergence(0.9, 0.5))
                .IncludeAllHistory().Build())
            {

                var result = engine.Run(runType);

                Assert.AreEqual(4, result.BestChromosome.Evaluate());
            }
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewIfNoImprovmentTest1(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 });
            populationManager.SetPopulationGenerated(new[]
                {new double[] {2, 2, 2}, new double[] {3, 3, 3}});
            using (var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewIfNoImprovment(1, 3, 1)).IncludeAllHistory().Build())
            {

                var result = engine.Run(runType);

                Assert.AreEqual(3, result.BestChromosome.Evaluate());
            }
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewIfNoImprovmentTest2(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 });
            populationManager.SetPopulationGenerated(new[] {new double[] {2, 2, 2}});
            using (var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewIfNoImprovment(2, 10, 1)).IncludeAllHistory().Build())
            {

                var result = engine.Run(runType);

                Assert.AreEqual(2, result.BestChromosome.Evaluate());
            }
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void RenewAtDifferenceBetweenAverageAndMaximumFitnessTest(RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 2, 1 });
            populationManager.SetPopulationGenerated(new[]
                {new double[] {2, 5, 2}, new double[] {6, 6, 6}, new double[] {7, 7, 7}});
            using (var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 4, populationManager)
                .AddPopulationRenwalManager(new RenewAtDifferenceBetweenAverageAndMaximumFitness(1, 1))
                .IncludeAllHistory().Build())
            {

                var result = engine.Run(runType);

                Assert.AreEqual(5, result.BestChromosome.Evaluate());
            }
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void PopulationRenwalManagerGetsRightInfoTest(RunType runType)
        {
            var generation = 0;
            var populationRenwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenwalManager.ShouldRenew(A<Population>._, A<IEnvironment>._, A<int>._)).Invokes(
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
            using (var engine = new TestGeneticSearchEngineBuilder(POPULATION_SIZE, 3, populationManager)
                .AddPopulationRenwalManager(populationRenwalManager).IncludeAllHistory().Build())
            {
                engine.Run(runType);
            }
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

            using (var engine = new TestGeneticSearchEngineBuilder(5, 10, testPopulationManager)
                .AddPopulationRenwalManagers(managers)
                .Build())
            {

                engine.Next();

                Assert.IsTrue(manager1Called);
                Assert.IsTrue(manager2Called);
            }
        }
    }
}
