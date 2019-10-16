using System.Linq;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Exceptions;
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
                var newChromosome = mutationManager.Mutate(new[] {0, 0, 0, 0, 0});
                foreach (var genome in newChromosome)
                {
                    if (genome == -1) minGenomes++;
                    if (genome == 1) maxGenomes++;
                }
            }

            minGenomes.AssertIsWithinRange(attempts / 2.0, attempts * 0.1);
        }

        [TestMethod]
        public void BitStringMutationManagerTest()
        {
            var mutatedGenomes = 0;
            var mutationManager = new BitStringMutationManager();
            for (int i = 0; i < attempts; i++)
            {
                var newChromosome = mutationManager.Mutate(new[] { 0, 0, 0, 0, 0 });
                mutatedGenomes += newChromosome.Count(genome => genome == 1);
            }

            mutatedGenomes.AssertIsWithinRange(attempts, attempts * 0.1);
        }

        [TestMethod]
        [ExpectedException(typeof(BadChromosomeTypeException))]
        public void BitStringMutationManager_SendNotBinayChromosome_ThrowException() =>
            new BitStringMutationManager().Mutate(new[] {0, 0, 3, 0});
    }
}
