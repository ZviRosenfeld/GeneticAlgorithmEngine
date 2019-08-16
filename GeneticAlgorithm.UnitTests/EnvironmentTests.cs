using FakeItEasy;
using GeneticAlgorithm.Interfaces;
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
                counter++;
                Assert.AreEqual(counter, g, "Wrong generation provided");
                foreach (var chromosome in c)
                    Assert.AreEqual(counter, chromosome.Evaluate(), "Wrong chromosome provided");
            });
            var populationManager = new TestPopulationManager(new double[] {1, 1}, c => c.Evaluate() + 1);
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
                counter++;

                var defaultEnvironment = (DefaultEnvironment) e;
                Assert.AreEqual(counter, defaultEnvironment.Generation, "Wrong generation provided");
                foreach (var chromosome in defaultEnvironment.Chromosomes)
                    Assert.AreEqual(counter, chromosome.Evaluate(), "Wrong chromosome provided");
            });
            var populationManager = new TestPopulationManager(new double[] { 1, 1 }, c => c.Evaluate() + 1);
            var engine = new TestGeneticSearchEngineBuilder(2, 10, populationManager).SetCustomChromosomeEvaluator(chromosomeEvaluator).Build();

            engine.Next();
            engine.Next();
            engine.Next();

            Assert.AreNotEqual(0, counter, "SetEnvierment was never called");
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
