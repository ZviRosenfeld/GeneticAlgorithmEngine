using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class EnvironmentTests
    {
        [TestMethod]
        public void IfNoEnvironmentIsSetUseDefaultEnvironment()
        {
            var chromosomeEvaluator = A.Fake<IChromosomeEvaluator>();
            A.CallTo(() => chromosomeEvaluator.SetEnvierment(A<IEnvironment>._)).Invokes((IEnvironment e) =>
                Assert.AreEqual(typeof(DefaultEnvironment), e.GetType()));
            var engine = new TestGeneticSearchEngineBuilder(2, 10, new double[] { 1, 1 }).SetCustomChromosomeEvaluator(chromosomeEvaluator).Build();
            engine.Next();
        }

        [TestMethod]
        public void UpdateEnviermentGetRightParameters()
        {
            var counter = 0;
            var environment = A.Fake<IEnvironment>();
            A.CallTo(() => environment.UpdateEnvierment(A<IChromosome[]>._, A<int>._)).Invokes((IChromosome[] c, int g) =>
            {
                Assert.AreEqual(counter, g, "Wrong generation provided");
                foreach (var chromosome in c)
                    Assert.AreEqual(counter, chromosome.Evaluate(), "Wrong chromosome provided");
                counter++;
            });
            var populationManager = new TestPopulationManager(new double[] {0, 0}, c => c.Evaluate() + 1);
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager).SetEnvironment(environment).Build();

            engine.Next();
            engine.Next();
            engine.Next();

            Assert.AreNotEqual(0, counter, "UpdateEnvierment was never called");
        }

        [TestMethod]
        public void ChromosomeEvaluatorGetRightEnvironment()
        {
            var counter = 0;
            var chromosomeEvaluator = A.Fake<IChromosomeEvaluator>();
            A.CallTo(() => chromosomeEvaluator.SetEnvierment(A<IEnvironment>._)).Invokes((IEnvironment e) =>
            {
                AssertIsRightEnvironment(e, counter);
                counter++;
            });
            var populationManager = new TestPopulationManager(new double[] { 0, 0 }, c => c.Evaluate() + 1);
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager).SetCustomChromosomeEvaluator(chromosomeEvaluator).Build();

            engine.Next();
            engine.Next();
            engine.Next();

            Assert.AreNotEqual(0, counter, "SetEnvierment was never called");
        }

        [TestMethod]
        public void OnNewGenerationEventGetsRightEnvironment()
        {
            var counter = 0;
            var populationManager = new TestPopulationManager(new double[] { 0, 0 }, c => c.Evaluate() + 1);
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager).SetCustomChromosomeEvaluator(A.Fake<IChromosomeEvaluator>()).Build();
            engine.OnNewGeneration += (p, en) =>
            {
                AssertIsRightEnvironment(en, counter);
                counter++;
            };
            
            engine.Next();
            engine.Next();
            engine.Next();

            Assert.AreNotEqual(0, counter, "SetEnvierment was never called");
        }

        [TestMethod]
        public void StopManagerGetsRightEnvironment()
        {
            var counter = 0;
            var populationManager = new TestPopulationManager(new double[] { 0, 0 }, c => c.Evaluate() + 1);
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.ShouldStop(A<Population>._, A<IEnvironment>._, A<int>._)).Invokes((Population p, IEnvironment e, int g) =>
            {
                AssertIsRightEnvironment(e, counter);
                counter++;
            });
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager)
                .SetEnvironment(new DefaultEnvironment()).AddStopManager(stopManager).Build();
            
            engine.Next();
            engine.Next();
            engine.Next();

            Assert.AreNotEqual(0, counter, "SetEnvierment was never called");
        }

        [TestMethod]
        public void PopulationConverterGetsRightEnvironment()
        {
            var counter = 0;
            var populationManager = new TestPopulationManager(new double[] { 0, 0 }, c => c.Evaluate() + 1);
            var populationConverter = A.Fake<IPopulationConverter>();
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._)).ReturnsLazily((IChromosome[] c, int g, IEnvironment e) =>
            {
                if (counter > 0)
                    AssertIsRightEnvironment(e, counter - 1);
                counter++;
                return c;
            });
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager)
                .SetEnvironment(new DefaultEnvironment()).AddPopulationConverter(populationConverter).Build();

            engine.Next();
            engine.Next();
            engine.Next();

            Assert.AreNotEqual(0, counter, "SetEnvierment was never called");
        }
        
        [TestMethod]
        public void PopulationRenwalManagerGetsRightEnvironment()
        {
            var counter = 0;
            var populationManager = new TestPopulationManager(new double[] { 0, 0 }, c => c.Evaluate() + 1);
            var renwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => renwalManager.ShouldRenew(A<Population>._, A<IEnvironment>._, A<int>._)).Invokes((Population p, IEnvironment e, int g) =>
            {
                AssertIsRightEnvironment(e, counter);
                counter++;
            });
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager)
                .SetEnvironment(new DefaultEnvironment()).AddPopulationRenwalManager(renwalManager).Build();

            engine.Next();
            engine.Next();
            engine.Next();

            Assert.AreNotEqual(0, counter, "SetEnvierment was never called");
        }

        [TestMethod]
        public void ResultGetsRightEnvironment()
        {
            var populationManager = new TestPopulationManager(new double[] { 0, 0 }, c => c.Evaluate() + 1);
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager).SetCustomChromosomeEvaluator(A.Fake<IChromosomeEvaluator>()).Build();

            int i;
            for (i = 0; i < 4; i++)
            {
                var result = engine.Next();
                AssertIsRightEnvironment(result.Environment, i);
            }

            Assert.AreNotEqual(0, i, "SetEnvierment was never called");
        }

        private static void AssertIsRightEnvironment(IEnvironment en, int counter)
        {
            var defaultEnvironment = (DefaultEnvironment) en;
            Assert.AreEqual(counter, defaultEnvironment.Generation, "Wrong generation provided");
            foreach (var chromosome in defaultEnvironment.Chromosomes)
                Assert.AreEqual(counter, chromosome.Evaluate(), "Wrong chromosome provided");
        }

        [TestMethod]
        public void ReturnChromosomeEvaluationIfSet()
        {
            var evaluation = 8;
            var chromosomeEvaluator = A.Fake<IChromosomeEvaluator>();
            A.CallTo(() => chromosomeEvaluator.Evaluate(A<IChromosome>._)).Returns(evaluation);
            var engine = new TestGeneticSearchEngineBuilder(2, 10, new double[] { 1, 1 }).SetCustomChromosomeEvaluator(chromosomeEvaluator).Build();
            var result = engine.Next();

            foreach (var actualEvaluation in result.Population.GetEvaluations())
                Assert.AreEqual(evaluation, actualEvaluation);  
        }
    }
}
