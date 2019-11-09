using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Interfaces;
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
        public void IntBoundaryMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntBoundaryMutationManager(-1, 1);
            var minGenomes = 0;
            var maxGenomes = 0;
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

        [TestMethod]
        public void IntUniformMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntUniformMutationManager(-100, 100);
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }

        [TestMethod]
        public void IntUniformMutationManager_AllValuesGenerated()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntUniformMutationManager(minValue, maxValue);
            AssertAllValuesAreGenerated(maxValue, minValue, mutationManager);
        }
        
        [TestMethod]
        public void IntUniformMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntUniformMutationManager(minValue, maxValue);
            AssertAllValuesAreWithinRange(mutationManager, maxValue, minValue);
        }

        [TestMethod]
        public void DoubleUniformMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new DoubleUniformMutationManager(-100, 100);
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }

        [TestMethod]
        public void DoubleUniformMutationManager_AllValuesGenerated()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleUniformMutationManager(minValue, maxValue);
            AssertAllValuesAreGenerated(maxValue, minValue, mutationManager);
        }

        [TestMethod]
        public void DoubleUniformMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleUniformMutationManager(minValue, maxValue);
            AssertAllValuesAreWithinRange(mutationManager, maxValue, minValue);
        }

        [TestMethod]
        public void IntGaussianMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntGaussianMutationManager(-100, 100);
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }
        
        [TestMethod]
        public void IntGaussianMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntGaussianMutationManager(minValue, maxValue);
            AssertAllValuesAreWithinRange(mutationManager, maxValue, minValue);
        }

        [TestMethod]
        public void IntGaussianMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new IntGaussianMutationManager(minValue, maxValue);
            AssertCommonValuesAreMoreLikely(maxValue / 2.0, mutationManager);
        }

        [TestMethod]
        public void IntGaussianMutationManager_AssertValuesAreScattered()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new IntGaussianMutationManager(minValue, maxValue);
            AssertValuesAreScattered(mutationManager);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new DoubleGaussianMutationManager(-100, 100);
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleGaussianMutationManager(minValue, maxValue);
            AssertAllValuesAreWithinRange(mutationManager, maxValue, minValue);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -31;
            var maxValue = 31;
            var mutationManager = new DoubleGaussianMutationManager(minValue, maxValue);
            AssertCommonValuesAreMoreLikely(maxValue / 2.0, mutationManager);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_AssertValuesAreScattered()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new DoubleGaussianMutationManager(minValue, maxValue);
            AssertValuesAreScattered(mutationManager);
        }

        [TestMethod]
        public void DoubleShrinkMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new DoubleShrinkMutationManager(-100, 100);
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }

        [TestMethod]
        public void DoubleShrinkMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new DoubleShrinkMutationManager(minValue, maxValue);
            AssertAllValuesAreWithinRange(mutationManager, maxValue, minValue);
        }

        [TestMethod]
        public void DoubleShrinkMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new DoubleShrinkMutationManager(minValue, maxValue);
            AssertCommonValuesAreMoreLikely(maxValue / 2.0, mutationManager);
        }

        [TestMethod]
        public void DoubleGaussianMutationManager_DoubleShrinkMutationManager()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new DoubleShrinkMutationManager(minValue, maxValue);
            AssertValuesAreScattered(mutationManager);
        }

        [TestMethod]
        public void IntShrinkMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new IntShrinkMutationManager(-100, 100);
            CheckMutationsHappenWithRightProbability(mutationManager, g => g != 0);
        }

        [TestMethod]
        public void IntShrinkMutationManager_AllValuesWithinRange()
        {
            var minValue = -5;
            var maxValue = 5;
            var mutationManager = new IntShrinkMutationManager(minValue, maxValue);
            AssertAllValuesAreWithinRange(mutationManager, maxValue, minValue);
        }

        [TestMethod]
        public void IntShrinkMutationManager_CommandValuesAreMoeLikely()
        {
            var minValue = -21;
            var maxValue = 21;
            var mutationManager = new IntShrinkMutationManager(minValue, maxValue);
            AssertCommonValuesAreMoreLikely(maxValue / 2.0, mutationManager);
        }

        [TestMethod]
        public void IntGaussianMutationManager_DoubleShrinkMutationManager()
        {
            var minValue = -11;
            var maxValue = 11;
            var mutationManager = new IntShrinkMutationManager(minValue, maxValue);
            AssertValuesAreScattered(mutationManager);
        }

        [TestMethod]
        public void BitStringMutationManager_MutationHappensWithRightProbability()
        {
            var mutationManager = new BitStringMutationManager();
            CheckMutationsHappenWithRightProbability(mutationManager, g => g);
        }
        
        private static void CheckMutationsHappenWithRightProbability<T>(IMutationManager<T> mutationManager, Func<T, bool> isMutated)
        {
            var mutatedGenomes = 0;
            for (int i = 0; i < attempts; i++)
            {
                var newChromosome = mutationManager.Mutate(new[]
                    {default(T), default(T), default(T), default(T), default(T)});
                mutatedGenomes += newChromosome.Count(isMutated);
            }

            mutatedGenomes.AssertIsWithinRange(attempts, attempts * 0.1);
        }
        
        private static void AssertAllValuesAreWithinRange<T>(IMutationManager<T> mutationManager, int maxValue, int minValue)
        {
            for (int i = 0; i < attempts; i++)
            {
                var value = mutationManager.Mutate(new[] { default(T) }).First().ToInt();
                Assert.IsTrue(value <= maxValue, $"{nameof(value)} ({value}) > {nameof(maxValue)} ({maxValue})");
                Assert.IsTrue(value >= minValue, $"{nameof(value)} ({value}) < {nameof(minValue)} ({minValue})");
            }
        }

        private static void AssertAllValuesAreGenerated<T>(int maxValue, int minValue, IMutationManager<T> mutationManager)
        {
            var gottenValues = new HashSet<int>();
            var runs = maxValue - minValue;
            for (int i = 0; i < runs * 5; i++)
                gottenValues.Add(mutationManager.Mutate(new[] { default(T) }).First().ToInt() + 5);

            for (int i = 1; i < runs; i++)
                Assert.IsTrue(gottenValues.Contains(i), $"We didn't get {i}");
        }

        private static void AssertCommonValuesAreMoreLikely<T>(double bourderValue, IMutationManager<T> mutationManager)
        {
            var smallCount = 0;
            var bigCount = 0;
            for (int i = 0; i < 10; i++)
            {
                var value = Math.Abs(mutationManager.Mutate(new[] {default(T)}).First().ToInt());
                Console.WriteLine(value);
                if (value >= bourderValue) bigCount++;
                else smallCount++;
            }

            Assert.IsTrue(smallCount > bigCount, "Got to many big genomes");
        }

        private static void AssertValuesAreScattered<T>(IMutationManager<T> mutationManager)
        {
            var values = new HashSet<int>();
            for (int i = 0; i < 50; i++)
            {
                var value = mutationManager.Mutate(new[] { default(T) }).First().ToInt();
                values.Add(value.ToInt());
            }

            Assert.IsTrue(values.Count > 8, $"We only got {values.Count} diffrent values");
        }
    }
}
