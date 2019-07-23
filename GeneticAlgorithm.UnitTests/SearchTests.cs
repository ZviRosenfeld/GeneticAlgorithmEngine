using System;
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
                    new double[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}, false);
            var engine = new TestGeneticSearchEngineBuilder(25, int.MaxValue, populationManager)
                .SetCancellationToken(cancellationSource.Token).Build();
            engine.Search();
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Assert.Fail("We should have finished running by now");
            });
        }
    }
}
