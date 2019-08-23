using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
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
            Assert.AreEqual(eilentPrecentage * populationSize,
                result.Population.GetChromosomes().Count(c => c.Evaluate() == maxEvaluation));
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

        [TestMethod]
        public void BestChromosome()
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 10, 14, 7, 8, 13, 11, 1, 6, 6 });
            var engine = new TestGeneticSearchEngineBuilder(10, 10, populationManager).Build();

            var result = engine.Next();

            Assert.AreEqual(14, result.BestChromosome.Evaluate());
        }

        [TestMethod]
        public void SearchTimeWithNextTest()
        {
            var sleepTime = 10;
            var generations = 50;
            var populationManager = new TestPopulationManager(new double[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 });
            var engine = new TestGeneticSearchEngineBuilder(10, generations, populationManager).Build();

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            GeneticSearchResult result = null;
            while (result == null || !result.IsCompleted)
            {
                result = engine.Next();
                Thread.Sleep(sleepTime);
            }
            stopwatch.Stop();

            var time = stopwatch.Elapsed.TotalMilliseconds - sleepTime * generations -
                       result.SearchTime.TotalMilliseconds;
            Assert.IsTrue(time < 0.2 * stopwatch.Elapsed.TotalMilliseconds);
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
            
            result.History.AssertHasEvaluation(population);
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
            searchEngine.OnNewGeneration += (p, e) =>
            {
                actualEvaluations.Add(p.GetEvaluations());
                actualPopulation.Add(p.GetChromosomes());
            };

            searchEngine.Run(runType);
            
            actualEvaluations.AssertAreTheSame(population);
            actualPopulation.AssertHasEvaluation(population);
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
                actualPopulation.Add(result.Population.GetChromosomes());
            }
            
            Assert.AreEqual(3, result.Generations, "We should have ran for 3 generations");
            actualPopulation.AssertHasEvaluation(population);
        }

        [TestMethod]
        public void IsCompletedTests()
        {
            var generations = 30;
            var populationManager = new TestPopulationManager(new double[]{1,2,3});
            var searchEngine =
                new TestGeneticSearchEngineBuilder(3, generations, populationManager)
                    .IncludeAllHistory().Build();
            
            for (int i = 0; i < generations - 1; i++)
            {
                var result = searchEngine.Next();
                Assert.IsFalse(result.IsCompleted, "Shouldn't have finished yet");
            }

            var finalResult = searchEngine.Next();
            Assert.IsTrue(finalResult.IsCompleted);
        }
        
        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void GetCurrentPopulation_CheckPopulationIsRight(bool includeHistory)
        {
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { new double[] { 3, 3 } });
            var engineBuilder =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager);
            if (includeHistory)
                engineBuilder.IncludeAllHistory();

            var engine = engineBuilder.Build();

            var result1 = engine.Next();
            var result2 = engine.GetCurrentPopulation();

            TestUtils.TestUtils.AssertAreTheSame(result1, result2);

            result1 = engine.Next();
            result2 = engine.GetCurrentPopulation();

            TestUtils.TestUtils.AssertAreTheSame(result1, result2);
        }

        [TestMethod]
        public void ChangeResultPopulation_AlgorithmPopulationNotChanged()
        {
            var expectedPopulation = new double[] {1, 1};
            var engine = new TestGeneticSearchEngineBuilder(2, 10, expectedPopulation).Build();
            var fakeChromosome = TestUtils.TestUtils.CreateChromosome(10, "ChangedChromosome");

            var result = engine.Next();
            result.Population.GetChromosomes()[0] = fakeChromosome;
            result = engine.GetCurrentPopulation();

            foreach (var chromosome in result.Population.GetChromosomes())
                Assert.AreNotEqual(fakeChromosome, chromosome);
        }

        [TestMethod]
        public void ChangeHistoryLastGeneration_AlgorithmPopulationNotChanged()
        {
            var expectedPopulation = new double[] { 1, 1 };
            var engine = new TestGeneticSearchEngineBuilder(2, 10, expectedPopulation).IncludeAllHistory().Build();
            var fakeChromosome = TestUtils.TestUtils.CreateChromosome(10, "ChangedChromosome");

            var result = engine.Next();
            result.History[result.History.Count - 1][0] = fakeChromosome;
            result = engine.GetCurrentPopulation();

            foreach (var chromosome in result.Population.GetChromosomes())
                Assert.AreNotEqual(fakeChromosome, chromosome);
        }
    }
}
