using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components
{
    [TestClass]
    public class CrossoverManagerTests
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
            var crossoverManager = new K_PointCrossover<int>(k, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            for (int i = 0; i < TEST_RUNS; i++)
            {
                var chromosome1 = (VectorChromosome<int>) smallPopulationGenerator1.GeneratePopulation(1).First();
                var chromosome2 = (VectorChromosome<int>) smallPopulationGenerator2.GeneratePopulation(1).First();
                var newChromosome = (VectorChromosome<int>) crossoverManager.Crossover(chromosome1, chromosome2);

                var crossoverPoints = K_CrossoverGetCrossoverPoints(newChromosome, chromosome2, chromosome1);
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
            var crossoverManager = new K_PointCrossover<int>(k, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            for (int i = 0; i < TEST_RUNS; i++)
            {
                var chromosome1 = (VectorChromosome<int>)smallPopulationGenerator1.GeneratePopulation(1).First();
                var chromosome2 = (VectorChromosome<int>)largePopulationGenerator.GeneratePopulation(1).First();
                var newChromosome = (VectorChromosome<int>)crossoverManager.Crossover(chromosome1, chromosome2);

                var crossoverPoints = K_CrossoverGetCrossoverPoints(newChromosome, chromosome2, chromosome1);
                Assert.AreEqual(k, crossoverPoints.Count,
                    $"Found wrong number of crossoverPoints. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome}");
            }
        }

        [TestMethod]
        public void K_PointCrossover_CrossoverPointsAreDiffrent()
        {
            var crossoverPoints = new List<int>();
            var crossoverManager = new K_PointCrossover<int>(2, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            for (int i = 0; i < 100; i++)
            {
                var chromosome1 = (VectorChromosome<int>)smallPopulationGenerator1.GeneratePopulation(1).First();
                var chromosome2 = (VectorChromosome<int>)smallPopulationGenerator2.GeneratePopulation(1).First();
                var newChromosome = (VectorChromosome<int>)crossoverManager.Crossover(chromosome1, chromosome2);

                crossoverPoints.AddRange(K_CrossoverGetCrossoverPoints(newChromosome, chromosome2, chromosome1));        
            }

            for (int i = 1; i < SMALL_CHROMOSOME_SIZE; i++)
                Assert.IsTrue(crossoverPoints.Contains(i), $"{nameof(crossoverPoints)} dosn't contain {i}");
        }

        private static List<int> K_CrossoverGetCrossoverPoints(VectorChromosome<int> newChromosome, VectorChromosome<int> chromosome2,
            VectorChromosome<int> chromosome1)
        {
            var crossoverPoints = new List<int>();
            var takingFromFirstChromosome = true;
            for (int i = 0; i < SMALL_CHROMOSOME_SIZE; i++)
            {
                if (takingFromFirstChromosome)
                {
                    if (newChromosome[i] == chromosome2[i])
                    {
                        crossoverPoints.Add(i);
                        takingFromFirstChromosome = !takingFromFirstChromosome;
                    }
                    else if (newChromosome[i] != chromosome1[i])
                        Assert.Fail(
                            $"Got Genome that dosn't seem to have came from anywhere. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome} ");
                }
                else
                {
                    if (newChromosome[i] == chromosome1[i])
                    {
                        crossoverPoints.Add(i);
                        takingFromFirstChromosome = !takingFromFirstChromosome;
                    }
                    else if (newChromosome[i] != chromosome2[i])
                        Assert.Fail(
                            $"Got Genome that dosn't seem to have came from anywhere. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome} ");
                }
            }
            for (int j = SMALL_CHROMOSOME_SIZE; j < LARGE_CHROMOSOME_SIZE && j < newChromosome.GetVector().Length; j++)
            {
                Assert.AreEqual(chromosome2[j], newChromosome[j]);
            }

            return crossoverPoints;
        }
    }
}
