using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.MutationManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class ScrambleMutationManagerTests
    {
        [TestMethod]
        [DataRow(20)]
        public void ScrambleMutationManager_AllElementsInEachVector(int vectors)
        {
            new ScrambleMutationManager<string>().TestAllElementsInEachVector(vectors);
        }

        // This test is probabilistic, so there's a certain chance that this test will fail even if the code is okay.
        [TestMethod]
        public void ScrambleMutationManager_ChromosomeChanged()
        {
            new ScrambleMutationManager<string>().TestChromosomeChanged();
        }
    }
}
