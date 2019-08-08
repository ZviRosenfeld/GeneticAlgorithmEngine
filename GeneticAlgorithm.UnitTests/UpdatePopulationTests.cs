using System;
using System.Collections.Generic;
using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    class SavedTestopulation
    {
        private readonly List<IChromosome[]> chromosomes = new List<IChromosome[]>();
        private readonly List<double[]> evaluations = new List<double[]>();

        public void Save(IChromosome[] chromosomes, double[] evaluations)
        {
            this.chromosomes.Add(chromosomes);
            this.evaluations.Add(evaluations);
        }
        
        public void AssertAreRightChromosomes(double[][] expactedEvaluation)
        {
            for (int i = 0; i < expactedEvaluation.Length; i++)
            for (int j = 0; j < expactedEvaluation[0].Length; j++)
            {
                Assert.AreEqual(expactedEvaluation[i][j], chromosomes[i][j].Evaluate(), "Bad chromosome");
                Assert.AreEqual(expactedEvaluation[i][j], evaluations[i][j], "Bad evaluation");
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
        private SavedTestopulation populationUpdatedForPopulationConverter;

        [TestInitialize]
        public void CleanChromosomesAndEvaluations()
        {
            populationUpdatedOnEvent = new SavedTestopulation();
            populationUpdatedForStopManager = new SavedTestopulation();
            populationUpdatedForRenewalManager = new SavedTestopulation();
            populationUpdatedForMutationManager = new SavedTestopulation();
            populationUpdatedForPopulationConverter = new SavedTestopulation();
        }

        [TestMethod]
        [DataRow(RunType.Run)]
        [DataRow(RunType.Next)]
        public void PopulationIsUpdatedOnNewGeneration(RunType runType)
        {
            var population = new[] { new double[] { 1, 1, 1 }, new double[] { 2, 2, 2 }, new double[] { 2, 3, 2 } };
            var populationManager = new TestPopulationManager(population);
            var engine = CreateEngineBuilder(populationManager, population.Length).Build();
            engine.OnNewGeneration += populationUpdatedOnEvent.Save;

            engine.Run(runType);

            AssertManagersUpdated(population);
        }

        [TestMethod]
        public void RenewPopulation_RenewedPopulationUpdated()
        {
            var initialPopulation = new double[] { 2, 2 };
            var renewedPopulation = new double[] {3, 3};
            var populationManager = new TestPopulationManager(initialPopulation);
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });

            var engine = CreateEngineBuilder(populationManager).Build();
            engine.OnNewGeneration += populationUpdatedOnEvent.Save;

            engine.Next();

            engine.RenewPopulation(1);

            AssertManagersUpdated(new []{initialPopulation, renewedPopulation});
        }
        
        [TestMethod]
        public void RenewPopulationViaManager_RenewedPopulationUpdated()
        {
            CleanChromosomesAndEvaluations();

            var renewManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => renewManager.ShouldRenew(A<IChromosome[]>._, A<double[]>._, A<int>._)).Returns(1);
            var renewedPopulation = new double[] { 3, 3 };
            var initialPopulation = new double[] {2, 2};
            var populationManager = new TestPopulationManager(initialPopulation);
            populationManager.SetPopulationGenerated(new[] { renewedPopulation });
            var builder = CreateEngineBuilder(populationManager);
            builder.AddPopulationRenwalManager(renewManager);
            var engine = builder.Build();
            engine.OnNewGeneration += populationUpdatedOnEvent.Save;

            engine.Next();

            AssertManagersUpdated( renewedPopulation);
        }

        private void AssertManagersUpdated(double[] renewedPopulation) =>
            AssertManagersUpdated(new[] { renewedPopulation });

        private void AssertManagersUpdated(double[][] renewedPopulation)
        {
            populationUpdatedOnEvent.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForStopManager.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForRenewalManager.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForMutationManager.AssertAreRightChromosomes(renewedPopulation);
            populationUpdatedForPopulationConverter.AssertAreRightChromosomes(renewedPopulation);
        }

        private GeneticSearchEngineBuilder CreateEngineBuilder(TestPopulationManager populationManager, int generations = Int32.MaxValue)
        {
            var populationConverter = A.Fake<IPopulationConverter>();
            A.CallTo(() => populationConverter.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForPopulationConverter.Save(c, e));
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._))
                .ReturnsLazily((IChromosome[] c, int g) => c);
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForStopManager.Save(c, e));
            var populationRenwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenwalManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForRenewalManager.Save(c, e));
            var mutationManager = A.Fake<IMutationManager>();
            A.CallTo(() => mutationManager.AddGeneration(A<IChromosome[]>._, A<double[]>._))
                .Invokes((IChromosome[] c, double[] e) => populationUpdatedForMutationManager.Save(c, e));
            var builder = new TestGeneticSearchEngineBuilder(2, generations, populationManager)
                .AddStopManager(stopManager)
                .AddPopulationRenwalManager(populationRenwalManager).SetMutationManager(mutationManager)
                .SetPopulationConverter(populationConverter);
            return builder;
        }
    }
}
