using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class RenewPopulationTests
    {
        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        [DataRow(0)]
        [DataRow(1.1)]
        [DataRow(-1)]
        public void RenewPopulation_BedPercentage_ThrowException(double percentage)
        {
            var engine =
                new TestGeneticSearchEngineBuilder(2, 4, new TestPopulationManager(new double[] { 2, 2 })).Build();
            engine.Next();

            engine.RenewPopulation(percentage);
            Assert.Fail("Should have thrown an exception by now");
        }

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void RenewPopulation_EngineNotStated_ThrowException()
        {
            var engine =
                new TestGeneticSearchEngineBuilder(2, 4, new TestPopulationManager(new double[] { 2, 2 })).Build();
            engine.RenewPopulation(0.5);
            Assert.Fail("Should have thrown an exception by now");
        }

        [TestMethod]
        [ExpectedException(typeof(EngineAlreadyRunningException))]
        public void RenewPopulation_EngineRunning_ThrowException()
        {
            using (var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build())
            {

                Task.Run(() => engine.Run());
                Thread.Sleep(50); // Give the eingine some time to start

                engine.RenewPopulation(0.5);
                Assert.Fail("Should have thrown an exception by now");
            }
        }

        [TestMethod]
        public void RenewPopulation_CheckPopulationRenewedRight()
        {
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { new double[] { 3, 3 } });
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager).Build();
            engine.Next();

            var result = engine.RenewPopulation(1);
            foreach (var chromosme in result.Population)
                Assert.AreEqual(3, chromosme.Evaluation);
        }

        [TestMethod]
        [DataRow(0.1)]
        [DataRow(0.5)]
        public void RenewPercantagePopulation_PercentageRenewed(double percent)
        {
            var populationManager = new TestPopulationManager(new double[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 });
            populationManager.SetPopulationGenerated(new[] { new double[] { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3 } });
            var engine =
                new TestGeneticSearchEngineBuilder(10, int.MaxValue, populationManager).Build();
            engine.Next();

            var result = engine.RenewPopulation(percent);
            var threeCounters = result.Population.Count(chromosme => chromosme.Evaluation == 3);

            Assert.AreEqual(percent * 10, threeCounters);
        }

        [TestMethod]
        public void RenewPopulation_CheckPopulationRenewedSentToNextGeneration()
        {
            var populationManager = new TestPopulationManager(new double[] { 2, 2 }, c => c.Evaluate() + 1);
            populationManager.SetPopulationGenerated(new[] { new double[] { 3, 3 } });
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager).Build();

            engine.Next();
            engine.RenewPopulation(1);
            var result = engine.Next();
            foreach (var chromosme in result.Population)
                Assert.AreEqual(4, chromosme.Evaluation);
        }
    }
}
