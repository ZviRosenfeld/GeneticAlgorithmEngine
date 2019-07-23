using System;
using System.Threading;
using Benchmarking;
using FakeItEasy;
using GeneticAlgorithm;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Banchmarking
{
    [TestCategory("Benchmarking")]
    [TestClass]
    public class Benchmarking
    {
        private const int POPULATION_SIZE = 10;
        private const int MAX_GENERATIONS = 100;
        private const int SLEEP_TIME = 10;

        [TestMethod]
        public void Banchmark()
        {
            var crossoverManager = A.Fake<ICrossoverManager>();
            A.CallTo(() => crossoverManager.Crossover(A<IChromosome>._, A<IChromosome>._))
                .ReturnsLazily(delegate(IChromosome c1, IChromosome c2) { Thread.Sleep(SLEEP_TIME); return c1; });

            var engine = new GeneticSearchEngineBuilder(POPULATION_SIZE, MAX_GENERATIONS, crossoverManager,
                new SlowPopulationGenerator(SLEEP_TIME)).Build();
            var result = engine.Search();

            Console.WriteLine("Total time: " + result.SearchTime);
        }
    }
}
