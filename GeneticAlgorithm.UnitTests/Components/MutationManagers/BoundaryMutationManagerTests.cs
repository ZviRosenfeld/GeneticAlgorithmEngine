using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    [TestClass]
    public class BoundaryMutationManagerTests
    {
        private const int attempts = 1000;

        [TestMethod]
        public void IntBoundaryMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntBoundaryMutationManager(-1, 1);
            var minGenomes = 0;
            var maxGenomes = 0;
            for (int i = 0; i < attempts; i++)
            {
                var newChromosome = mutationManager.Mutate(new[] { 0, 0, 0, 0, 0 });
                foreach (var genome in newChromosome)
                {
                    if (genome == -1) minGenomes++;
                    if (genome == 1) maxGenomes++;
                }
            }

            minGenomes.AssertIsWithinRange(attempts / 2.0, attempts * 0.1);
            maxGenomes.AssertIsWithinRange(attempts / 2.0, attempts * 0.1);
        }

        [TestMethod]
        public void DoubleBoundaryMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new DoubleBoundaryMutationManager(-1.5, 1.5);
            var minGenomes = 0;
            var maxGenomes = 0;
            for (int i = 0; i < attempts; i++)
            {
                var newChromosome = mutationManager.Mutate(new double[] { 0, 0, 0, 0, 0 });
                foreach (var genome in newChromosome)
                {
                    if (genome == -1.5) minGenomes++;
                    if (genome == 1.5) maxGenomes++;
                }
            }

            minGenomes.AssertIsWithinRange(attempts / 2.0, attempts * 0.1);
            maxGenomes.AssertIsWithinRange(attempts / 2.0, attempts * 0.1);
        }

    }
}
