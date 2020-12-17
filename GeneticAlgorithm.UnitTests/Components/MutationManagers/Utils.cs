using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.MutationManagers
{
    static class Utils
    {
        private const int attempts = 1000;

        public static void CheckMutationsHappenWithRightProbability<T>(this IMutationManager<T> mutationManager, Func<T, bool> isMutated)
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

        public static void AssertAllValuesAreWithinRange<T>(this IMutationManager<T> mutationManager, int maxValue, int minValue)
        {
            for (int i = 0; i < attempts; i++)
            {
                var value = mutationManager.Mutate(new[] { default(T) }).First().ToInt();
                Assert.IsTrue(value <= maxValue, $"{nameof(value)} ({value}) > {nameof(maxValue)} ({maxValue})");
                Assert.IsTrue(value >= minValue, $"{nameof(value)} ({value}) < {nameof(minValue)} ({minValue})");
            }
        }

        public static void AssertAllValuesAreGenerated<T>(this IMutationManager<T> mutationManager, int maxValue, int minValue)
        {
            var gottenValues = new HashSet<int>();
            var runs = maxValue - minValue;
            for (int i = 0; i < runs * 5; i++)
                gottenValues.Add(mutationManager.Mutate(new[] { default(T) }).First().ToInt() + 5);

            for (int i = 1; i < runs; i++)
                Assert.IsTrue(gottenValues.Contains(i), $"We didn't get {i}");
        }

        public static void AssertCommonValuesAreMoreLikely<T>(this IMutationManager<T> mutationManager, double bourderValue)
        {
            var smallCount = 0;
            var bigCount = 0;
            for (int i = 0; i < 10; i++)
            {
                var value = Math.Abs(mutationManager.Mutate(new[] { default(T) }).First().ToInt());
                Console.WriteLine(value);
                if (value >= bourderValue) bigCount++;
                else smallCount++;
            }

            Assert.IsTrue(smallCount > bigCount, "Got to many big genomes");
        }

        public static void AssertValuesAreScattered<T>(this IMutationManager<T> mutationManager)
        {
            var values = new HashSet<int>();
            for (int i = 0; i < 50; i++)
            {
                var value = mutationManager.Mutate(new[] { default(T) }).First().ToInt();
                values.Add(value.ToInt());
            }

            Assert.IsTrue(values.Count > 8, $"We only got {values.Count} different values");
        }

        public static void TestChromosomeChanged(this IMutationManager<string> mutationManager)
        {
            var elements = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);

            // Since there's a certain chance that this test will fail, I want to run it twice
            var passed = false;
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    var before = ((VectorChromosome<string>) generator.GeneratePopulation(1).First()).GetVector();
                    var after = mutationManager.Mutate(before.ToArray());

                    before.AssertAreNotTheSame(after);
                    passed = true;
                }
                catch
                {
                    // Do nothing
                }
            }
            Assert.IsTrue(passed);
        }

        public static void TestAllElementsInEachVector(this IMutationManager<string> mutationManager, int testRuns)
        {
            var elements = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            for (int i = 0; i < testRuns; i++)
            {
                var before = ((VectorChromosome<string>)generator.GeneratePopulation(1).First()).GetVector();
                var after = mutationManager.Mutate(before.ToArray());
                before.AssertContainSameElements(after);
            }
        }
    }
}
