using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class PartiallyMatchedCrossoverTests
    {
        private static readonly List<string> elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        private readonly IPopulationGenerator generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
        private readonly ICrossoverManager crossoverManager = new PartiallyMatchedCrossover<string>(null, null);

        [TestMethod]
        [DataRow(20)]
        public void PartiallyMatchedCrossover_AllElementsInEachVector(int vectors)
        {
            for (int i = 0; i < vectors; i++)
            {
                var parentChromosomes = generator.GeneratePopulation(2);
                var child = (VectorChromosome<string>)crossoverManager.Crossover(parentChromosomes.ElementAt(0), parentChromosomes.ElementAt(1));
                child.AssertContainSameElements(elements);
            }
        }

        [TestMethod]
        public void PartiallyMatchedCrossover_ChildChanged()
        {
            // Since there's a certain chance that this test will fail, I want to run it twice
            var passed = false;
            for (int i = 0; i < 2; i++)
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
