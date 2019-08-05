using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    class SavedTestopulation
    {
        private IChromosome[] chromosomes;
        private double[] evaluations;

        public void Save(IChromosome[] chromosomes, double[] evaluations)
        {
            this.chromosomes = chromosomes;
            this.evaluations = evaluations;
        }

        public void AssertAreRightChromosomes(double[] expactedEvaluation)
        {
            for (int i = 0; i < expactedEvaluation.Length; i++)
            {
                Assert.AreEqual(expactedEvaluation[i], chromosomes[i].Evaluate(), "Bad chromosome");
                Assert.AreEqual(expactedEvaluation[i], evaluations[i], "Bad evaluation");
            }
        }
    }

    /// <summary>
    /// Tests that check that the new population is update everywhere it needs to be.
    /// </summary>
    [TestClass]
    public class UpdatePopulationTests
    {
        private SavedTestopulation populationUpdatedOnEvent;
        private SavedTestopulation populationUpdatedForStopManager;
        private SavedTestopulation populationUpdatedForRenewalManager;
        private SavedTestopulation populationUpdatedForMutationManager;

        private void CleanChromosomesAndEvaluations()
        {
            populationUpdatedOnEvent = new SavedTestopulation();
            populationUpdatedForStopManager = new SavedTestopulation();
            populationUpdatedForRenewalManager = new SavedTestopulation();
            populationUpdatedForMutationManager = new SavedTestopulation();
        }
        
        [TestMethod]
        public void RenewPopulation_RenewedPopulationUpdated()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] {3, 3};
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForStopManager.Save(c, e));
            var populationRenwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenwalManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForRenewalManager.Save(c, e));
            var mutationManager = A.Fake<IMutationManager>();
            A.CallTo(() => mutationManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForMutationManager.Save(c, e));
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager).AddStopManager(stopManager)
                    .AddPopulationRenwalManager(populationRenwalManager).SetMutationManager(mutationManager).Build();
            engine.Next();
            engine.OnNewGeneration += populationUpdatedOnEvent.Save;

            engine.RenewPopulation(1);
            populationUpdatedOnEvent.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForStopManager.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForRenewalManager.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForMutationManager.AssertAreRightChromosomes(renewedPopulation);
        }
        
        [TestMethod]
        public void RenewPopulationViaManager_RenewedPopulationUpdated()
        {
            CleanChromosomesAndEvaluations();
            var renewedPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(new double[] { 2, 2 });
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForStopManager.Save(c, e));
            var testPopulationRenewalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => testPopulationRenewalManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForRenewalManager.Save(c, e));
            var mutationManager = A.Fake<IMutationManager>();
            A.CallTo(() => mutationManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForMutationManager.Save(c, e));
            var renewManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => renewManager.ShouldRenew(A<IChromosome[]>._, A<double[]>._, A<int>._)).Returns(1);
            var engine =
                new TestGeneticSearchEngineBuilder(2, int.MaxValue, populationManager)
                    .AddPopulationRenwalManager(renewManager).AddStopManager(stopManager)
                    .AddPopulationRenwalManager(testPopulationRenewalManager).SetMutationManager(mutationManager).Build();
            engine.OnNewGeneration += populationUpdatedOnEvent.Save;
            engine.Next();

            populationUpdatedOnEvent.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForStopManager.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForRenewalManager.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForMutationManager.AssertAreRightChromosomes(renewedPopulation);
        }
    }
}
