using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Interfaces;
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
            maxGenomes.AssertIsWithinRange(attempts / 2.0, attempts * 0.1);
        }

        [TestMethod]
        public void UniformMutationManagerTest()
        {
            var mutationManager = new UniformMutationManager(-100, 100);
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }

        [TestMethod]
        public void UniformMutationManager_AllValuesGenerated()
        {
            var gottenValues = new HashSet<int>();
            var mutationManager = new UniformMutationManager(-5, 5);
            for (int i = 0; i < 11 * 5; i++)
                gottenValues.Add(mutationManager.Mutate(new[] {0}).First() + 5);

            for (int i = 1; i < 11; i++)
                Assert.IsTrue(gottenValues.Contains(i), $"We didn't get {i}");
        }

        [TestMethod]
        public void UniformMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new UniformMutationManager(minValue, maxValue);
            for (int i = 0; i < attempts; i++)
            {
                var value = mutationManager.Mutate(new[] {0}).First();
                Assert.IsTrue(value <= maxValue, $"{nameof(value)} ({value}) > {nameof(maxValue)} ({maxValue})");
                Assert.IsTrue(value >= minValue, $"{nameof(value)} ({value}) < {nameof(minValue)} ({minValue})");
            }
        }

        [TestMethod]
        public void BitStringMutationManagerTest()
        {
            var mutationManager = new BitStringMutationManager();
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }

        [TestMethod]
        [ExpectedException(typeof(BadChromosomeTypeException))]
        public void BitStringMutationManager_SendNotBinayChromosome_ThrowException() =>
            new BitStringMutationManager().Mutate(new[] { 0, 0, 3, 0 });

        private static void CheckMutationsHappenWithRightProbability(IMutationManager<int> mutationManager, Func<int, bool> isMutated)
        {
            var mutatedGenomes = 0;
            for (int i = 0; i < attempts; i++)
            {
                var newChromosome = mutationManager.Mutate(new[] {0, 0, 0, 0, 0});
                mutatedGenomes += newChromosome.Count(isMutated);
            }

            mutatedGenomes.AssertIsWithinRange(attempts, attempts * 0.1);
        }
    }
}
