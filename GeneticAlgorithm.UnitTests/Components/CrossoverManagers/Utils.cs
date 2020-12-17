using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    static class Utils
    {
        public static List<int> K_CrossoverGetCrossoverPointsAndAssertThatGenomesAreRight(VectorChromosome<int> newChromosome, VectorChromosome<int> chromosome2, VectorChromosome<int> chromosome1)
        {
            var crossoverPoints = new List<int>();
            var takingFromFirstChromosome = true;
            for (int i = 0; i < chromosome1.Length; i++)
            {
                if (takingFromFirstChromosome)
                {
                    if (newChromosome[i] == chromosome2[i])
                    {
                        crossoverPoints.Add(i);
                        takingFromFirstChromosome = !takingFromFirstChromosome;
                    }
                    else if (newChromosome[i] != chromosome1[i])
                        Assert.Fail($"Got Genome that dosn't seem to have came from anywhere. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome} ");
                }
                else
                {
                    if (newChromosome[i] == chromosome1[i])
                    {
                        crossoverPoints.Add(i);
                        takingFromFirstChromosome = !takingFromFirstChromosome;
                    }
                    else if (newChromosome[i] != chromosome2[i])
                        Assert.Fail($"Got Genome that doesn't seem to have came from anywhere. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome} ");
                }
            }
            for (int j = chromosome1.Length; j < chromosome2.Length && j < newChromosome.GetVector().Length; j++)
                Assert.AreEqual(chromosome2[j], newChromosome[j]);

            return crossoverPoints;
        }

        public static void TestThatAllElementsInEachVector(this ICrossoverManager crossoverManager, int testRuns)
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            for (int i = 0; i < testRuns; i++)
            {
                var parentChromosomes = generator.GeneratePopulation(2);
                var child = (VectorChromosome<string>)crossoverManager.Crossover(parentChromosomes.ElementAt(0), parentChromosomes.ElementAt(1));
                child.AssertContainSameElements(elements);
            }
        }

        public static void TestThatChildChanged(this ICrossoverManager crossoverManager)
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            
            // Since there's a certain chance that this test will fail, I want to run it twice
            var passed = false;
            for (int i = 0; i < 3; i++)
            {
                var parentChromosomes = generator.GeneratePopulation(2);
                var child = (VectorChromosome<string>)crossoverManager.Crossover(parentChromosomes.ElementAt(0),
                    parentChromosomes.ElementAt(1));

                try
                {
                    ((VectorChromosome<string>)parentChromosomes.ElementAt(0)).AssertAreNotTheSame(child);
                    ((VectorChromosome<string>)parentChromosomes.ElementAt(1)).AssertAreNotTheSame(child);
                    passed = true;
                }
                catch
                {
                    // Do nothing
                }
            }
            Assert.IsTrue(passed);
        }
    }
}
