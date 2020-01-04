using GeneticAlgorithm.Components.MutationManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class GaussianMutationManagerTests
    {
        [TestMethod]
        public void IntGaussianMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntGaussianMutationManager(-100, 100);
            mutationManager.CheckMutationsHappenWithRightProbability(g => g != 0);
        }

        [TestMethod]
        public void IntGaussianMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntGaussianMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreWithinRange(maxValue, minValue);
        }

        [TestMethod]
        public void IntGaussianMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new IntGaussianMutationManager(minValue, maxValue);
            mutationManager.AssertCommonValuesAreMoreLikely(maxValue / 2.0);
        }

        [TestMethod]
        public void IntGaussianMutationManager_AssertValuesAreScattered()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new IntGaussianMutationManager(minValue, maxValue);
            mutationManager.AssertValuesAreScattered();
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new DoubleGaussianMutationManager(-100, 100);
            mutationManager.CheckMutationsHappenWithRightProbability(g => g != 0);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleGaussianMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreWithinRange(maxValue, minValue);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -31;
            var maxValue = 31;
            var mutationManager = new DoubleGaussianMutationManager(minValue, maxValue);
            mutationManager.AssertCommonValuesAreMoreLikely(maxValue / 2.0);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_AssertValuesAreScattered()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new DoubleGaussianMutationManager(minValue, maxValue);
            mutationManager.AssertValuesAreScattered();
        }
    }
}
