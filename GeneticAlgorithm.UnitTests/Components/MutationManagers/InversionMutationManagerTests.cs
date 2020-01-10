using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.MutationManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class InversionMutationManagerTests
    {
        private readonly IMutationManager<string> mutationManager = new InversionMutationManager<string>();

        [TestMethod]
        [DataRow(20)]
        public void DisplacementMutationManager_AllElementsInEachVector(int vectors)
        {
            mutationManager.TestAllElementsInEachVector(vectors);
        }

        // This test is probabilistic, so there's a certain chance that this test will fail even if the code is okay.
        [TestMethod]
        public void DisplacementMutationManager_ChromosomeChanged()
        {
            mutationManager.TestChromosomeChanged();
        }
    }
}
