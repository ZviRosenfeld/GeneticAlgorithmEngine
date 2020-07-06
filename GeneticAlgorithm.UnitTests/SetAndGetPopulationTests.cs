using System;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class SetAndGetPopulationTests
    {
        [TestMethod]
        [ExpectedException(typeof(EngineAlreadyRunningException))]
        public void GetCurrentPopulation_EngineRunning_ThrowException()
        {
            Utils.RunTimedTest(() =>
            {
                using (var engine =
                    new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build())
                {

                    Task.Run(() => engine.Run());
                    while (!engine.IsRunning) ;

                    engine.GetCurrentPopulation();
                    Assert.Fail("Should have thrown an exception by now");
                }
            });
        }

        [TestMethod]
        [ExpectedException(typeof(EngineAlreadyRunningException))]
        public void SetCurrentPopulation_EngineRunning_ThrowException()
        {
            Utils.RunTimedTest(() =>
            {
                using (var engine =
                    new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build())
                {

                    Task.Run(() => engine.Run());
                    while (!engine.IsRunning) ;

                    engine.SetCurrentPopulation(new double[] { 3, 3 }.ToChromosomes("Converted"));

                    if (engine.IsRunning)
                        Assert.Fail("Should have thrown an exception.");
                    else
                        Assert.Fail("For some reason, the engine is no longer running.");
                }
            });
        }

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void SetCurrentPopulation_WrongNumberOfChromosomes_ThrowException()
        {
            Utils.RunTimedTest(() =>
            {
                using (var engine = new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build())
                {

                    Task.Run(() => engine.Run());
                    while (!engine.IsRunning) ;

                    engine.SetCurrentPopulation(new double[] { 3, 3, 3 }.ToChromosomes("Converted"));

                    Assert.Fail("Should have thrown an exception by now");
                }
            });
        }

        [TestMethod]
        public void SetPopulation_PopulationSet()
        {
            Utils.RunTimedTest(() =>
            {
                var newPopulationEvaluation = new double[] { 2, 2, 2 };
                var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 });
                var engine = new TestGeneticSearchEngineBuilder(3, 10, populationManager).Build();
                engine.Next();

                var newPopulation = engine.SetCurrentPopulation(newPopulationEvaluation.ToChromosomes("Converted"));
                newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);

                newPopulation = engine.GetCurrentPopulation();
                newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);
            });
        }

        [TestMethod]
        public void SetPopulation_NextGetsRightPopulation()
        {
            var newPopulationEvaluation = new double[] { 2, 2, 2 };
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 }, c => c.Evaluate());
            var engine = new TestGeneticSearchEngineBuilder(3, 10, populationManager).Build();
            engine.Next();

            var newPopulation = engine.SetCurrentPopulation(newPopulationEvaluation.ToChromosomes("Converted"));
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);

            newPopulation = engine.Next();
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);
        }
    }
}
