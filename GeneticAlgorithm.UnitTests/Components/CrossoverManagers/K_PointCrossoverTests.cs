using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class K_PointCrossoverTests
    {
        private const int TEST_RUNS = 20;
        private const int SMALL_CHROMOSOME_SIZE = 10;
        private const int LARGE_CHROMOSOME_SIZE = 20;

        private readonly IPopulationGenerator smallPopulationGenerator1 =
            new IntVectorChromosomePopulationGenerator(SMALL_CHROMOSOME_SIZE, 0, 10, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());
        private readonly IPopulationGenerator smallPopulationGenerator2 =
            new IntVectorChromosomePopulationGenerator(SMALL_CHROMOSOME_SIZE, 11, 20, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());
        private readonly IPopulationGenerator largePopulationGenerator =
            new IntVectorChromosomePopulationGenerator(LARGE_CHROMOSOME_SIZE, 21, 30, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(5)]
        [DataRow(SMALL_CHROMOSOME_SIZE - 1)]
        public void K_PointCrossoverTest(int k)
        {
            var crossoverManager = new K_PointCrossoverManager<int>(k, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            for (int i = 0; i < TEST_RUNS; i++)
            {
                var chromosome1 = (VectorChromosome<int>)smallPopulationGenerator1.GeneratePopulation(1).First();
                var chromosome2 = (VectorChromosome<int>)smallPopulationGenerator2.GeneratePopulation(1).First();
                var newChromosome = (VectorChromosome<int>)crossoverManager.Crossover(chromosome1, chromosome2);

                var crossoverPoints = Utils.K_CrossoverGetCrossoverPointsAndAssertThatGenomesAreRight(newChromosome, chromosome2, chromosome1);
                Assert.AreEqual(k, crossoverPoints.Count,
                    $"Found wrong number of crossoverPoints. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome}");
            }
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(5)]
        [DataRow(SMALL_CHROMOSOME_SIZE - 1)]
        public void K_PointCrossover_ShortAndLongChromosomes(int k)
        {
            var crossoverManager = new K_PointCrossoverManager<int>(k, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            for (int i = 0; i < TEST_RUNS; i++)
            {
                var chromosome1 = (VectorChromosome<int>)smallPopulationGenerator1.GeneratePopulation(1).First();
                var chromosome2 = (VectorChromosome<int>)largePopulationGenerator.GeneratePopulation(1).First();
                var newChromosome = (VectorChromosome<int>)crossoverManager.Crossover(chromosome1, chromosome2);

                var crossoverPoints = Utils.K_CrossoverGetCrossoverPointsAndAssertThatGenomesAreRight(newChromosome, chromosome2, chromosome1);
                Assert.AreEqual(k, crossoverPoints.Count,
                    $"Found wrong number of crossoverPoints. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome}");
            }
        }

        [TestMethod]
        public void K_PointCrossover_CrossoverPointsAreDiffrent()
        {
            var crossoverPoints = new List<int>();
            var crossoverManager = new K_PointCrossoverManager<int>(2, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            for (int i = 0; i < 100; i++)
            {
                var chromosome1 = (VectorChromosome<int>)smallPopulationGenerator1.GeneratePopulation(1).First();
                var chromosome2 = (VectorChromosome<int>)smallPopulationGenerator2.GeneratePopulation(1).First();
                var newChromosome = (VectorChromosome<int>)crossoverManager.Crossover(chromosome1, chromosome2);

                crossoverPoints.AddRange(Utils.K_CrossoverGetCrossoverPointsAndAssertThatGenomesAreRight(newChromosome, chromosome2, chromosome1));
            }

            for (int i = 1; i < SMALL_CHROMOSOME_SIZE; i++)
                Assert.IsTrue(crossoverPoints.Contains(i), $"{nameof(crossoverPoints)} dosn't contain {i}");
        }
    }
}
