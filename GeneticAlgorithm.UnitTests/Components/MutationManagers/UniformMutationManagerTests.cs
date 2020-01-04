using GeneticAlgorithm.Components.MutationManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class UniformMutationManagerTests
    {
        [TestMethod]
        public void IntUniformMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntUniformMutationManager(-100, 100);
            mutationManager.CheckMutationsHappenWithRightProbability(g => g != 0);
        }

        [TestMethod]
        public void IntUniformMutationManager_AllValuesGenerated()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntUniformMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreGenerated(maxValue, minValue);
        }

        [TestMethod]
        public void IntUniformMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntUniformMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreWithinRange(maxValue, minValue);
        }

        [TestMethod]
        public void DoubleUniformMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new DoubleUniformMutationManager(-100, 100);
            mutationManager.CheckMutationsHappenWithRightProbability(g => g != 0);
        }

        [TestMethod]
        public void DoubleUniformMutationManager_AllValuesGenerated()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleUniformMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreGenerated(maxValue, minValue);
        }

        [TestMethod]
        public void DoubleUniformMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleUniformMutationManager(minValue, maxValue);
            mutationManager.AssertAllValuesAreWithinRange(maxValue, minValue);
        }

    }
}
