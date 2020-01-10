using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class InsertionMutationManagerTests
    {
        private readonly IMutationManager<string> mutationManager = new InsertionMutationManager<string>();

        [TestMethod]
        [DataRow(20)]
        public void InsertionMutationManager_AllElementsInEachVector(int vectors)
        {
            mutationManager.TestAllElementsInEachVector(vectors);
        }

        // This test is probabilistic, so there's a certain chance that this test will fail even if the code is okay.
        [TestMethod]
        public void InsertionMutationManager_ChromosomeChanged()
        {
            mutationManager.TestChromosomeChanged();
        }

        [TestMethod]
        [DataRow(20)]
        public void InsertionMutationManager_OneGenomeInserted(int vectors)
        {
            var elements = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            for (int i = 0; i < vectors; i++)
            {
                var before = ((VectorChromosome<string>)generator.GeneratePopulation(1).First()).GetVector();
                var after = mutationManager.Mutate(before.ToArray());
                var length = after.Length;

                var removedGenomes = 0;

                // The only genome that should meet these conditions is the removed one
                if (after[0] != before[0] && after[0] != before[1]) removedGenomes++;
                if (after[length - 1] != before[length - 1] && after[length - 1] != before[length - 2])
                    removedGenomes++;
                for (int k = 1; k < after.Length - 1; k++)
                    if (after[k] != before[k] && after[k] != before[k - 1] && after[k] != before[k + 1])
                        removedGenomes++;

                Assert.IsTrue(removedGenomes <= 1, "Only one genome should have been removed");
            }
        }
    }
}
