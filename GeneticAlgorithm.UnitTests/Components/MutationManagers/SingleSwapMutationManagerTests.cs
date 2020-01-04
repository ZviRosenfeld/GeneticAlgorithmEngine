using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class SingleSwapMutationManagerTests
    {
        [TestMethod]
        [DataRow(20)]
        public void SingleSwapMutationManager_AllElementsInEachVector(int vectors)
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            var mutationManager = new SingleSwapMutationManager<string>();
            for (int i = 0; i < vectors; i++)
            {
                var before = ((VectorChromosome<string>)generator.GeneratePopulation(1).First()).GetVector();
                var after = mutationManager.Mutate(before.ToArray());
                before.AssertContainSameElements(after);
            }
        }

        // This test is probabilistic, so there's a certain chance that this test will fail even if the code is okay.
        [TestMethod]
        public void SingleSwapMutationManager_ChromosomeChanged()
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            var mutationManager = new SingleSwapMutationManager<string>();
            var before = ((VectorChromosome<string>)generator.GeneratePopulation(1).First()).GetVector();
            var after = mutationManager.Mutate(before.ToArray());

            before.AssertAreNotTheSame(after);
        }
    }
}
