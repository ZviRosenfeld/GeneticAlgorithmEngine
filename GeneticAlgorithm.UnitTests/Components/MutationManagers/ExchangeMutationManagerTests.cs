using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class ExchangeMutationManagerTests
    {
        private readonly IMutationManager<string> mutationManager = new ExchangeMutationManager<string>();

        [TestMethod]
        [DataRow(20)]
        public void ExchangeMutationManager_AllElementsInEachVector(int vectors)
        {
            mutationManager.TestAllElementsInEachVector(vectors);
        }

        // This test is probabilistic, so there's a certain chance that this test will fail even if the code is okay.
        [TestMethod]
        public void ExchangeMutationManager_ChromosomeChanged()
        {
            mutationManager.TestChromosomeChanged();
        }

        [TestMethod]
        [DataRow(20)]
        public void ExchangeMutationManager_ExactlyTwoGenomesAreDiffrent(int vectors)
        {
            var elements = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            for (int i = 0; i < vectors; i++)
            {
                var before = ((VectorChromosome<string>)generator.GeneratePopulation(1).First()).GetVector();
                var after = mutationManager.Mutate(before.ToArray());
                var diffrentGenomes = before.Where((t, j) => t != after[j]).Count();
                Assert.IsTrue(diffrentGenomes == 0 || diffrentGenomes == 2);
            }
        }
    }
}
