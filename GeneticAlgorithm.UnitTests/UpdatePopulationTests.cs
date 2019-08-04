using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    /// <summary>
    /// Tests that check that the new population is update everywhere it needs to be.
    /// </summary>
    [TestClass]
    public class UpdatePopulationTests
    {
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

        [TestMethod]
        public void RenewPopulationViaManager_RenewedPopulationSentToOnNewGeneration()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var renewManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => renewManager.ShouldRenew(A<IChromosome[]>._, A<double[]>._, A<int>._)).Returns(1);
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager)
                    .AddPopulationRenwalManager(renewManager).Build();
            engine.OnNewGeneration += Save;
            engine.Next();

            AssertAreRightChromosomes(renewedPopulation);
        }

        [TestMethod]
        public void RenewPopulationViaManager_RenewedPopulationSentToStopManager()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => Save(c, e));
            var renewManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => renewManager.ShouldRenew(A<IChromosome[]>._, A<double[]>._, A<int>._)).Returns(1);
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager).AddStopManager(stopManager)
                    .AddPopulationRenwalManager(renewManager).Build();
            engine.Next();
            
            AssertAreRightChromosomes(renewedPopulation);
        }

        [TestMethod]
        public void RenewPopulationViaManager_RenewedPopulationSentToPopulationRenewalManager()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var populationRenwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenwalManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => Save(c, e));
            A.CallTo(() => populationRenwalManager.ShouldRenew(A<IChromosome[]>._, A<double[]>._, A<int>._)).Returns(1);
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager)
                    .AddPopulationRenwalManager(populationRenwalManager).Build();
            engine.Next();
            
            AssertAreRightChromosomes(renewedPopulation);
        }
    }
}
