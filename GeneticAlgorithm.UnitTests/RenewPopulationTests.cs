using System;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FakeItEasy;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
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
                new TestGeneticSearchEngineBuilder(2, 4, new TestPopulationManager(new double[] {2, 2})).Build();
            engine.Next();

            engine.RenewPopulation(percentage);
        }

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void RenewPopulation_EngineNotStated_ThrowException()
        {
            var engine =
                new TestGeneticSearchEngineBuilder(2, 4, new TestPopulationManager(new double[] { 2, 2 })).Build();
            engine.RenewPopulation(0.5);
        }

        [TestMethod]
        [ExpectedException(typeof(EngineAlreadyRunningException))]
        public void RenewPopulation_EngineRunning_ThrowException()
        {
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, new TestPopulationManager(new double[] { 2, 2 })).Build();
            
            Task.Run(() => engine.Run());
            Thread.Sleep(50); // give the engine some time to start
            engine.RenewPopulation(0.5);
        }

        [TestMethod]
        public void RenewPopulation_CheckPopulationRenewedRight()
        {
            var populationManager = new TestPopulationManager(new double[] {2, 2});
            populationManager.SetPopulationGenerated(new[] { new double[] { 3, 3 } });
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager).Build();
            engine.Next();

            var result = engine.RenewPopulation(1);
            foreach (var chromosme in result.Population)
                Assert.AreEqual(3, chromosme.Evaluation);
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

        private IChromosome[] chromosomes;
        private double[] evaluations;

        private void Save(IChromosome[] chromosomes, double[] evaluations)
        {
            this.chromosomes = chromosomes;
            this.evaluations = evaluations;
        }

        private void CleanChromosomesAndEvaluations()
        {
            chromosomes = null;
            evaluations = null;
        }

        private void AssertAreRightChromosomes(double[] expactedEvaluation)
        {
            for (int i = 0; i < expactedEvaluation.Length; i++)
            {
                Assert.AreEqual(expactedEvaluation[i], chromosomes[i].Evaluate(), "Bad chromosome");
                Assert.AreEqual(expactedEvaluation[i], evaluations[i], "Bad evaluation");
            }
        }

        [TestMethod]
        public void RenewPopulation_RenewedPopulationSentToOnNewGeneration()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] {3, 3};
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager).Build();
            engine.Next();
            engine.OnNewGeneration += Save;

            engine.RenewPopulation(1);
            AssertAreRightChromosomes(renewedPopulation);
        }

        [TestMethod]
        public void RenewPopulation_RenewedPopulationSentToStopManager()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => Save(c, e));
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager).AddStopManager(stopManager).Build();
            engine.Next();

            engine.RenewPopulation(1);
            AssertAreRightChromosomes(renewedPopulation);
        }

        [TestMethod]
        public void RenewPopulation_RenewedPopulationSentToPopulationRenewalManager()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var populationRenwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenwalManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => Save(c, e));
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager)
                    .AddPopulationRenwalManager(populationRenwalManager).Build();
            engine.Next();

            engine.RenewPopulation(1);
            AssertAreRightChromosomes(renewedPopulation);
        }
    }
}
