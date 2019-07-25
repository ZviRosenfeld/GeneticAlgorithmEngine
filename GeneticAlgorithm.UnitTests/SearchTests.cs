using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Interfaces;
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

        [TestMethod]
        public void SearchTimeTest()
        {
            var populationManager = new TestPopulationManager( new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
            var engine1 = new TestGeneticSearchEngineBuilder(10, 10, populationManager).Build();
            var engine2 = new TestGeneticSearchEngineBuilder(10, 1000, populationManager).Build();

            var result1 = engine1.Search();
            var result2 = engine2.Search();

            Assert.IsTrue(result2.SearchTime > result1.SearchTime, $"engine1 ran for less time than engine2 (engine1 = {result1.SearchTime}; engine2 = {result2.SearchTime})");
        }

        [DataRow(false)]
        [DataRow(true)]
        [TestMethod]
        public void HistoryExistTest(bool includeHistory)
        {
            var populationManager = new TestPopulationManager(new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
            var enigneBuilder = new TestGeneticSearchEngineBuilder(10, 2, populationManager);
            var searchEngine = includeHistory ? enigneBuilder.IncludeAllHistory().Build() : enigneBuilder.Build();

            var result = searchEngine.Search();

            if (includeHistory)
                Assert.AreEqual(3, result.History.Count, "There should have been history");
            else
                Assert.AreEqual(0, result.History.Count, "There shouldn't be any history");
        }

        [TestMethod]
        public void HistoryIsRightTest()
        {
            var population = new[] {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {2, 3, 2}};
            var populationManager = new TestPopulationManager(population);
            var searchEngine =
                new TestGeneticSearchEngineBuilder(population[0].Length, population.Length - 1, populationManager)
                    .IncludeAllHistory().Build();

            var result = searchEngine.Search();
            
            AssertHasEvaluation(result.History, population);
        }

        [TestMethod]
        public void OnNewGenerationEventTest()
        {
            var population = new[] { new double[] { 1, 1, 1 }, new double[] { 2, 2, 2 }, new double[] { 2, 3, 2 } };
            var populationManager = new TestPopulationManager(population);
            var searchEngine =
                new TestGeneticSearchEngineBuilder(population[0].Length, population.Length - 1, populationManager)
                    .Build();

            var actualPopulation = new List<IChromosome[]>();
            var actualEvaluations = new List<double[]>();
            searchEngine.OnNewGeneration += (c, d) =>
            {
                actualEvaluations.Add(d);
                actualPopulation.Add(c);
            };

            searchEngine.Search();
            
            AssertAreTheSame(actualEvaluations, population);
            AssertHasEvaluation(actualPopulation, population);
        }

        private void AssertHasEvaluation(List<IChromosome[]> chromosomes, double[][] evaluations)
        {
            for (int i = 0; i < chromosomes.Count; i++)
            for (int j = 0; j < chromosomes[0].Length; j++)
                Assert.AreEqual(evaluations[i][j], chromosomes[i][j].Evaluate());
        }

        private void AssertAreTheSame(List<double[]> collection1, double[][] collection2)
        {
            for (int i = 0; i < collection1.Count; i++)
            for (int j = 0; j < collection1[0].Length; j++)
                Assert.AreEqual(collection1[i][j], collection2[i][j]);
        }
    }
}
