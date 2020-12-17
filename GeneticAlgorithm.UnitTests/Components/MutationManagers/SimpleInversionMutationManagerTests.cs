using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class SimpleInversionMutationManagerTests
    {
        [TestMethod]
        [DataRow(20)]
        public void SimpleInversionMutationManager_AllElementsInEachVector(int vectors)
        {
            new SimpleInversionMutationManager<string>().TestAllElementsInEachVector(vectors);
        }

        // This test is probabilistic, so there's a certain chance that this test will fail even if the code is okay.
        [TestMethod]
        public void SimpleInversionMutationManager_ChromosomeChanged()
        {
            new SimpleInversionMutationManager<string>().TestChromosomeChanged();
        }

        [TestMethod]
        [DataRow(20)]
        public void SimpleInversionMutationManager_OneScratchInversed(int vectors)
        {
            var mutationManager = new SimpleInversionMutationManager<string>();

            var elements = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            for (int i = 0; i < vectors; i++)
            {
                var before = ((VectorChromosome<string>)generator.GeneratePopulation(1).First()).GetVector();
                var after = mutationManager.Mutate(before.ToArray());

                var (start, end) = GetStartAndEndInversionPoints(before, after);
                AssertInversionIsCorrect(start, end, before, after);
            }
        }

        private void AssertInversionIsCorrect(int start, int end, string[] before, string[] after)
        {
            for (int i = 0; i < start; i++)
                Assert.AreEqual(before[i], after[i]);
            for (int addTo = start, addFrom = end - 1; addTo < end; addTo++, addFrom--)
                Assert.AreEqual(before[addFrom], after[addTo]);
            for (int i = end; i < after.Length; i++)
                Assert.AreEqual(before[i], after[i]);
        }

        private (int, int) GetStartAndEndInversionPoints(string[] before, string[] after)
        {
            int start = -1, end = -1, j = 0;
            for (; j < after.Length; j++)
                if (before[j] != after[j])
                {
                    start = j;
                    break;
                }
            for (; j < after.Length; j++)
                if (before[j] == after[j] && (j + 1 >= after.Length || before[j + 1] == after[j + 1]))
                {
                    end = j;
                    break;
                }
            if (end == -1)
                end = after.Length;
            if (start == -1)
                start = after.Length;

            return (start, end);
        }
    }
}
