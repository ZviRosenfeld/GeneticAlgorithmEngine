using System;
using System.Collections.Generic;
using System.Threading;
using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class ChildrenGeneratorTests
    {
        [TestMethod]
        [DataRow(0)]
        [DataRow(0.1)]
        [DataRow(0.5)]
        [DataRow(1)]
        public void TestMutationsHappen(double mutationProbability)
        {
            const int tries = 1500;
            int mutationCounter = 0;
            var population = GetPopulation(tries, 0.1, 0.1, 0.8, () => Interlocked.Increment(ref mutationCounter));
            var option = new GeneticSearchOptions(tries, mutationProbability, new List<IStopManager>(), false, new List<IPopulationRenwalManager>(), 0);
            var crossoverManager = A.Fake<ICrossoverManager>();
            A.CallTo(() => crossoverManager.Crossover(A<IChromosome>._, A<IChromosome>._))
                .ReturnsLazily((IChromosome c1, IChromosome c2) => c1);
            var childrenGenerator = new ChildrenGenerator(option, crossoverManager);
            childrenGenerator.GenerateChildren(population, tries);

            const double errorMargin = 0.05;
            Assert.IsTrue(mutationCounter < tries * mutationProbability + errorMargin * tries, $"Too few mutations ({mutationCounter})");
            Assert.IsTrue(mutationCounter > tries * mutationProbability - errorMargin * tries, $"Too many mutations ({mutationCounter})");
        }


        [TestMethod]
        public void MostLieklyToChooseBestChromosomeTest()
        {
            const int tries = 1500;
            const double chromosome1Probability = 0.1, chromosome2Probability = 0.3, chromosome3Probability = 0.6;
            var counters = new int[3];
            var population = GetPopulation(tries, chromosome1Probability, chromosome2Probability, chromosome3Probability);
            var option = new GeneticSearchOptions(tries, 0, new List<IStopManager>(), false, new List<IPopulationRenwalManager>(), 0);
            var crossoverManager = A.Fake<ICrossoverManager>();
            A.CallTo(() => crossoverManager.Crossover(A<IChromosome>._, A<IChromosome>._)).Invokes(
                (IChromosome c1, IChromosome c2) =>
                {
                    UpdateCounters(c1.ToString(), counters);
                    UpdateCounters(c2.ToString(), counters);
                });
            var childrenGenerator = new ChildrenGenerator(option, crossoverManager);
            childrenGenerator.GenerateChildren(population, tries);

            AssertIsWithinRange(counters[0], chromosome1Probability, tries, "c1");
            AssertIsWithinRange(counters[1], chromosome2Probability, tries, "c2");
            AssertIsWithinRange(counters[2], chromosome3Probability, tries, "c3");
        }

        private void UpdateCounters(string chromosomeName, int[] counters)
        {
            if (chromosomeName == "c1")
                counters[0]++;
            if (chromosomeName == "c2")
                counters[1]++;
            if (chromosomeName == "c3")
                counters[2]++;
        }

        /// <summary>
        /// Returns a population with 500 of each chromosome
        /// </summary>
        /// <returns></returns>
        private Population GetPopulation(int tries, double chromosome1Probability, double chromosome2Probability, double chromosome3Probability, Action onMutation = null)
        {
            var chromosomes = new List<IChromosome>();
            for (int i = 0; i < tries / 3; i++)
            {
                chromosomes.Add(CreateNewChromosome("c1", chromosome1Probability, onMutation));
                chromosomes.Add(CreateNewChromosome("c2", chromosome2Probability, onMutation));
                chromosomes.Add(CreateNewChromosome("c3", chromosome3Probability, onMutation));
            }

            var population = new Population(chromosomes.ToArray());
            foreach (var chromosome in population)
                chromosome.Evaluation = chromosome.Chromosome.Evaluate();

            return population;
        }

        private IChromosome CreateNewChromosome(string name, double evaluation, Action onMutation)
        {
            var chromosome = A.Fake<IChromosome>();
            A.CallTo(() => chromosome.ToString()).Returns(name);
            A.CallTo(() => chromosome.Evaluate()).Returns(evaluation);
            A.CallTo(() => chromosome.Mutate()).Invokes(onMutation);
            return chromosome;
        }

        private void AssertIsWithinRange(int value, double probability, int tries, string valueName)
        {
            const double errorMargin = 0.05;
            var expected = probability * tries * 2;
            var min = expected - errorMargin * tries * 2;
            var max = expected + errorMargin * tries * 2;

            Assert.IsTrue(value > min, $"Value ({value}) in smaller than min ({min}) for {valueName}");
            Assert.IsTrue(value < max, $"Value ({value}) in greater than max ({max}) for {valueName}");
        }
    }
}
