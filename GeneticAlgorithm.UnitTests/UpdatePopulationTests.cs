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

        public void Save(Population population) =>
            Save(population, null);

        public void Save(Population population, IEnvironment environment)
        {
            chromosomes.Add(population.GetChromosomes());
            evaluations.Add(population.GetEvaluations());
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
            A.CallTo(() => renewManager.ShouldRenew(A<Population>._, A<IEnvironment>._, A<int>._)).Returns(1);
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

        [TestMethod]
        public void ConvertPopulation_NewPopulationUpdated()
        {
            var initialPopulation = new double[] { 2, 2 };
            var newPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(initialPopulation);

            var engine = CreateEngineBuilder(populationManager).Build();
            engine.OnNewGeneration += populationUpdatedOnEvent.Save;

            engine.Next();

            engine.SetCurrentPopulation(newPopulation.ToChromosomes("Converted"));

            AssertManagersUpdated(new[] { initialPopulation, newPopulation });
        }

        [TestMethod]
        public void ConvertPopulationViaManager_NewPopulationUpdated()
        {
            var initialPopulation = new double[] { 2, 2 };
            var newPopulation = new double[] { 3, 3 };
            var populationManager = new TestPopulationManager(initialPopulation);
            var populationConverter = A.Fake<IPopulationConverter>();
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._))
                .Returns(newPopulation.ToChromosomes("Converted"));
            A.CallTo(() => populationConverter.AddGeneration(A<Population>._))
                .Invokes((Population p) => populationUpdatedForPopulationConverter.Save(p, null));

            var engine = CreateEngineBuilder(populationManager).SetPopulationConverter(populationConverter).Build();
            engine.OnNewGeneration += populationUpdatedOnEvent.Save;

            engine.Next();
            AssertManagersUpdated(newPopulation);
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

        private GeneticSearchEngineBuilder CreateEngineBuilder(TestPopulationManager populationManager, int generations = int.MaxValue)
        {
            var populationConverter = A.Fake<IPopulationConverter>();
            A.CallTo(() => populationConverter.AddGeneration(A<Population>._))
                .Invokes((Population population) => populationUpdatedForPopulationConverter.Save(population));
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._))
                .ReturnsLazily((IChromosome[] c, int g, IEnvironment e) => c);
            var stopManager = A.Fake<IStopManager>();
            A.CallTo(() => stopManager.AddGeneration(A<Population>._))
                .Invokes((Population p) => populationUpdatedForStopManager.Save(p));
            var populationRenwalManager = A.Fake<IPopulationRenwalManager>();
            A.CallTo(() => populationRenwalManager.AddGeneration(A<Population>._))
                .Invokes((Population p) => populationUpdatedForRenewalManager.Save(p));
            var mutationManager = A.Fake<IMutationManager>();
            A.CallTo(() => mutationManager.AddGeneration(A<Population>._))
                .Invokes((Population p) => populationUpdatedForMutationManager.Save(p));
            var builder = new TestGeneticSearchEngineBuilder(2, generations, populationManager)
                .AddStopManager(stopManager)
                .AddPopulationRenwalManager(populationRenwalManager).SetCustomMutationManager(mutationManager)
                .SetPopulationConverter(populationConverter);
            return builder;
        }
    }
}
