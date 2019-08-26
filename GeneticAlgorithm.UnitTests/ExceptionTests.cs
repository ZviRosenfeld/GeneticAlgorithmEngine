﻿using System;
using System.Linq;
using System.Threading.Tasks;
using FakeItEasy;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class ExceptionTests
    {
        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        [DataRow(0)]
        [DataRow(-2)]
        public void SetNegativePopulationSize_ThrowsException(int size) =>
            new GeneticSearchEngineBuilder(size, 2, A.Fake<ICrossoverManager>(), A.Fake<IPopulationGenerator>())
                .Build();

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        [DataRow(1)]
        [DataRow(-2)]
        public void BadNumberOfGenerations_ThrowsException(int size) =>
            new GeneticSearchEngineBuilder(2, size, A.Fake<ICrossoverManager>(), A.Fake<IPopulationGenerator>())
                .Build();

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        [DataRow(-0.1)]
        [DataRow(1.1)]
        public void SetIllegalMutationProbability_ThrowsException(double probability) =>
            new GeneticSearchEngineBuilder(2, 2, A.Fake<ICrossoverManager>(), A.Fake<IPopulationGenerator>())
                .SetMutationProbability(probability).Build();

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        [DataRow(-0.1)]
        [DataRow(1.1)]
        public void SetIllegalElitPercentage_ThrowsException(double percentage) =>
            new GeneticSearchEngineBuilder(2, 2, A.Fake<ICrossoverManager>(), A.Fake<IPopulationGenerator>())
                .SetElitePercentage(percentage).Build();

        [TestMethod]
        public void NagitiveEvaluation_ThrowException()
        {
            try
            {
                var chromosme = A.Fake<IChromosome>();
                A.CallTo(() => chromosme.Evaluate()).Returns(-1);
                var populationGenerator = A.Fake<IPopulationGenerator>();
                A.CallTo(() => populationGenerator.GeneratePopulation(A<int>._))
                    .Returns(new[] {chromosme});

                var engine = new GeneticSearchEngineBuilder(1, 2, A.Fake<ICrossoverManager>(), populationGenerator)
                    .Build();
                engine.Run();
                Assert.Fail("Should have thrown an exception by now");
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(typeof(NegativeEvaluationException), e.InnerExceptions.First().GetType());
            }
        }

        [DataRow(-1)]
        [DataRow(1.1)]
        [ExpectedException(typeof(PopulationRenewalException))]
        [TestMethod]
        public void BedPopulationRenewalPercantage_ThrowException(double percantage)
        {
            var chromosme = A.Fake<IChromosome>();
            A.CallTo(() => chromosme.Evaluate()).Returns(1);
            var populationGenerator = A.Fake<IPopulationGenerator>();
            A.CallTo(() => populationGenerator.GeneratePopulation(A<int>._))
                .Returns(new[] {chromosme});

            var populationRenewalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenewalManager.ShouldRenew(A<Population>._, A<IEnvironment>._, A<int>._))
                .Returns(percantage);
            var engine =
                new GeneticSearchEngineBuilder(1, 2, A.Fake<ICrossoverManager>(), populationGenerator)
                    .AddPopulationRenwalManager(populationRenewalManager)
                    .Build();
            engine.Run();
            Assert.Fail("Should have thrown an exception by now");
        }
        
        [TestMethod]
        [ExpectedException(typeof(EngineAlreadyRunningException))]
        public void GetCurrentPopulation_EngineRunning_ThrowException()
        {
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build();

            Task.Run(() => engine.Run());
            while (!engine.IsRunning) ;

            engine.GetCurrentPopulation();
            Assert.Fail("Should have thrown an exception by now");
        }

        [TestMethod]
        [ExpectedException(typeof(EngineAlreadyRunningException))]
        public void SetCurrentPopulation_EngineRunning_ThrowException()
        {
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build();

            Task.Run(() => engine.Run());
            while (!engine.IsRunning) ;

            engine.SetCurrentPopulation(new double[] { 3 ,3 }.ToChromosomes("Converted"));

            Assert.Fail("Should have thrown an exception by now");
        }

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void SetCurrentPopulation_WrongNumberOfChromosomes_ThrowException()
        {
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build();

            Task.Run(() => engine.Run());
            while (!engine.IsRunning) ;

            engine.SetCurrentPopulation(new double[] { 3, 3 ,3 }.ToChromosomes("Converted"));

            Assert.Fail("Should have thrown an exception by now");
        }
    }
}
