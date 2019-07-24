using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class SearchTests
    {
        [TestMethod]
        public void CancellationTokenTest()
        {
            var cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
            var populationManager =
                new TestPopulationManager(
                    new double[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1});
            var engine = new TestGeneticSearchEngineBuilder(25, int.MaxValue, populationManager)
                .SetCancellationToken(cancellationSource.Token).Build();
            engine.Search();
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Assert.Fail("We should have finished running by now");
            });
        }

        // In this test, without elitism, the population will decrease without the elitism.
        [TestMethod]
        [DataRow(0.1)]
        [DataRow(0.5)]
        public void ElitismTest(double eilentPrecentage)
        {
            var populationSize = 10;
            var populationManager =
                new TestPopulationManager(
                    new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, c => c.Evaluate() - 1);
            var engine = new TestGeneticSearchEngineBuilder(populationSize, 10, populationManager)
                .SetElitPercentage(eilentPrecentage).IncludeAllHistory().Build();
            
            var result = engine.Search();
            var maxEvaluation = result.BestChromosome.Evaluate();
            
            Assert.AreEqual(10, maxEvaluation);
            Assert.AreEqual(eilentPrecentage * populationSize, result.Population.Count(c => c.Evaluate() == maxEvaluation));
        }
    }
}
