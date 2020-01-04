using GeneticAlgorithm.Components.MutationManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class ShrinkMutationManagerTests
    {
        [TestMethod]
        public void DoubleShrinkMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new DoubleShrinkMutationManager(-100, 100);
            mutationManager.CheckMutationsHappenWithRightProbability(g => g != 0);
        }

        [TestMethod]
        public void DoubleShrinkMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleShrinkMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreWithinRange(maxValue, minValue);
        }

        [TestMethod]
        public void DoubleShrinkMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new DoubleShrinkMutationManager(minValue, maxValue);
            mutationManager.AssertCommonValuesAreMoreLikely(maxValue / 2.0);
        }

        [TestMethod]
        public void IntShrinkMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntShrinkMutationManager(-100, 100);
            mutationManager.CheckMutationsHappenWithRightProbability(g => g != 0);
        }

        [TestMethod]
        public void IntShrinkMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntShrinkMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreWithinRange(maxValue, minValue);
        }

        [TestMethod]
        public void IntShrinkMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -21;
            var maxValue = 21;
            var mutationManager = new IntShrinkMutationManager(minValue, maxValue);
            mutationManager.AssertCommonValuesAreMoreLikely(maxValue / 2.0);
        }

        [TestMethod]
        public void DoubleShrinkMutationManager_AssertValuesAreScattered()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new DoubleShrinkMutationManager(minValue, maxValue);
            mutationManager.AssertValuesAreScattered();
        }

        [TestMethod]
        public void IntShrinkMutationManager_AssertValuesAreScattered()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new IntShrinkMutationManager(minValue, maxValue);
            mutationManager.AssertValuesAreScattered();
        }
    }
}
