using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components
{
    [TestClass]
    public class MutationManagerTests
    {
        private const int attempts = 1000;

        [TestMethod]
        public void BoundaryMutationManagerTest()
        {
            var minGenomes = 0;
            var maxGenomes = 0;
            var mutationManager = new BoundaryMutationManager(-1, 1);
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
        }
    }
}
