using GeneticAlgorithm.Components.MutationManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class BitStringMutationManagerTests
    {
        [TestMethod]
        public void BitStringMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new BitStringMutationManager();
            mutationManager.CheckMutationsHappenWithRightProbability(g => g);
        }

    }
}
