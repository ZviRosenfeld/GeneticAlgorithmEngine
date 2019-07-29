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
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void CancellationTokenTest(RunType runType)
        {
            var cancellationSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));
            var populationManager =
                new TestPopulationManager(
                    new double[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1});
            var engine = new TestGeneticSearchEngineBuilder(25, int.MaxValue, populationManager)
                .SetCancellationToken(cancellationSource.Token).Build();
            
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                Assert.Fail("We should have finished running by now");
            });

            engine.Run(runType);
        }

        // In this test, without elitism, the population will decrease without the elitism.
        [TestMethod]
        [DataRow(0.1, RunType.Run)]
        [DataRow(0.5, RunType.Run)]
        [DataRow(0.1, RunType.Next)]
        [DataRow(0.5, RunType.Next)]
        public void ElitismTest(double eilentPrecentage, RunType runType)
        {
            var populationSize = 10;
            var populationManager =
                new TestPopulationManager(
                    new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, c => c.Evaluate() - 1);
            var engine = new TestGeneticSearchEngineBuilder(populationSize, 10, populationManager)
                .SetElitPercentage(eilentPrecentage).IncludeAllHistory().Build();
            
            var result = engine.Run(runType);
            var maxEvaluation = result.BestChromosome.Evaluate();
            
            Assert.AreEqual(10, maxEvaluation);
            Assert.AreEqual(eilentPrecentage * populationSize, result.Population.Count(c => c.Evaluate() == maxEvaluation));
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void SearchTimeTest(RunType runType)
        {
            var populationManager = new TestPopulationManager( new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
            var engine1 = new TestGeneticSearchEngineBuilder(10, 10, populationManager).Build();
            var engine2 = new TestGeneticSearchEngineBuilder(10, 1000, populationManager).Build();

            var result1 = engine1.Run(runType);
            var result2 = engine2.Run(runType);

            Assert.IsTrue(result2.SearchTime > result1.SearchTime, $"engine1 ran for less time than engine2 (engine1 = {result1.SearchTime}; engine2 = {result2.SearchTime})");
        }
        
        [DataRow(false, RunType.Run)]
        [DataRow(true, RunType.Run)]
        [DataRow(false, RunType.Next)]
        [DataRow(true, RunType.Next)]
        [TestMethod]
        public void HistoryExistTest(bool includeHistory, RunType runType)
        {
            var populationManager = new TestPopulationManager(new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
            var enigneBuilder = new TestGeneticSearchEngineBuilder(10, 2, populationManager);
            var searchEngine = includeHistory ? enigneBuilder.IncludeAllHistory().Build() : enigneBuilder.Build();

            var result = searchEngine.Run(runType);

            if (includeHistory)
                Assert.AreEqual(2, result.History.Count, "There should have been history");
            else
                Assert.AreEqual(0, result.History.Count, "There shouldn't be any history");
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void HistoryIsRightTest(RunType runType)
        {
            var population = new[] {new double[] {1, 1, 1}, new double[] {2, 2, 2}, new double[] {2, 3, 2}};
            var populationManager = new TestPopulationManager(population);
            var searchEngine =
                new TestGeneticSearchEngineBuilder(population[0].Length, population.Length - 1, populationManager)
                    .IncludeAllHistory().Build();

            var result = searchEngine.Run(runType);
            
            AssertHasEvaluation(result.History, population);
        }
        
        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void OnNewGenerationEventTest(RunType runType)
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

            searchEngine.Run(runType);
            
            AssertAreTheSame(actualEvaluations, population);
            AssertHasEvaluation(actualPopulation, population);
        }

        [TestMethod]
        public void NextGetsRightGenerationsTest()
        {
            var population = new[] { new double[] { 1, 1, 1 }, new double[] { 2, 2, 2 }, new double[] { 2, 3, 2 } };
            var populationManager = new TestPopulationManager(population);
            var searchEngine =
                new TestGeneticSearchEngineBuilder(population[0].Length, population.Length, populationManager)
                    .IncludeAllHistory().Build();

            var actualPopulation = new List<IChromosome[]>();
            GeneticSearchResult result = null;
            while (result == null || !result.IsCompleted)
            {
                result = searchEngine.Next();
                actualPopulation.Add(result.Population);
            }
            
            Assert.AreEqual(3, result.Generations, "We should have ran for 3 generations");
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
