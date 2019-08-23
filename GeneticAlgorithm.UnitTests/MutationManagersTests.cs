using GeneticAlgorithm.MutationManagers;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class MutationManagersTests
    {
        [TestMethod]
        public void ConvergenceMutationManagerTest()
        {
            var convergenceMutationManager = new ConvergenceMutationManager();

            var homogeneousPopulation = new Population(new double[] {2, 2, 2, 2, 2 }.ToChromosomes());
            homogeneousPopulation.Evaluate();

            var diversifiedPopulation = new Population(new double[] { 1, 2, 3, 4, 5 }.ToChromosomes());
            diversifiedPopulation.Evaluate();

            convergenceMutationManager.AddGeneration(homogeneousPopulation);

            var homogeneousMutationRate =
                convergenceMutationManager.MutationProbability(homogeneousPopulation, null, 2);
            var diversifiedMutationRate =
                convergenceMutationManager.MutationProbability(diversifiedPopulation, null, 2);

            Assert.IsTrue(diversifiedMutationRate > homogeneousMutationRate,
                $"{nameof(diversifiedMutationRate)} == {diversifiedMutationRate}; {nameof(homogeneousMutationRate)} == {homogeneousMutationRate}");
        }
    }
}
