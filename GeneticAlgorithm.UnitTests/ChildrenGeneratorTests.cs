using System;
using System.Collections.Generic;
using System.Threading;
using FakeItEasy;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.MutationManagers;
using GeneticAlgorithm.SelectionStrategies;
using GeneticAlgorithm.UnitTests.TestUtils;
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
            const int populationSize = 1500;
            int mutationCounter = 0;
            var population = GetPopulation(populationSize, () => Interlocked.Increment(ref mutationCounter));
            var crossoverManager = A.Fake<ICrossoverManager>();
            A.CallTo(() => crossoverManager.Crossover(A<IChromosome>._, A<IChromosome>._))
                .ReturnsLazily((IChromosome c1, IChromosome c2) => c1);
            var childrenGenerator = new ChildrenGenerator(crossoverManager, new BasicMutationProbabilityManager(mutationProbability), new RouletteWheelSelection());
            childrenGenerator.GenerateChildren(population, populationSize, 0, null);

            const double errorMargin = 0.05;
            Assert.IsTrue(mutationCounter < populationSize * mutationProbability + errorMargin * populationSize, $"Too few mutations ({mutationCounter})");
            Assert.IsTrue(mutationCounter > populationSize * mutationProbability - errorMargin * populationSize, $"Too many mutations ({mutationCounter})");
        }


        [TestMethod]
        [DataRow(10)]
        [DataRow(1)]
        [DataRow(2000)]
        public void RetusnRightNumberOfChromosomes(int childrenCount)
        {
            const int count = 1500;
            var crossoverManager = A.Fake<ICrossoverManager>();
            var childrenGenerator = new ChildrenGenerator(crossoverManager, new BasicMutationProbabilityManager(0), new RouletteWheelSelection());
            var children = childrenGenerator.GenerateChildren(GetPopulation(count), childrenCount, 0, null);
            Assert.AreEqual(childrenCount, children.Length, "Didn't get enough children");
            foreach (var chromosome in children)
                Assert.IsNotNull(chromosome, "No children should be null");
        }

        [TestMethod]
        [DataRow(-0.5)]
        [DataRow(1.1)]
        [ExpectedException(typeof(BadMutationProbabilityException))]
        public void BadMutationProbability_ThrowException(double probability)
        {
            var mutationManager = A.Fake<IMutationProbabilityManager>();
            A.CallTo(() => mutationManager.MutationProbability(A<Population>._, A<IEnvironment>._, A<int>._))
                .Returns(probability);
            
            var childrenGenerator = new ChildrenGenerator(A.Fake<ICrossoverManager>(), mutationManager, new RouletteWheelSelection());
            childrenGenerator.GenerateChildren(GetPopulation(1), 1, 1, null);
        }

        [TestMethod]
        [ExpectedException(typeof(InternalSearchException))]
        [DataRow(0)]
        [DataRow(-1)]
        public void RequestBadNumberOfChildren_ThrowsException(int childrenCount)
        {
            var childrenGenerator = new ChildrenGenerator(A.Fake<ICrossoverManager>(), new BasicMutationProbabilityManager(0),
                new RouletteWheelSelection());
            childrenGenerator.GenerateChildren(GetPopulation(1), childrenCount, 0, null);
        }

        [TestMethod]
        public void SelectionStrategyReturnsNull_ThrowsException()
        {
            Assertions.AssertThrowAggretateExceptionOfType(() =>
            {
                var selectionStrategy = A.Fake<ISelectionStrategy>();
                A.CallTo(() => selectionStrategy.SelectChromosome()).Returns(null);
                var childrenGenerator = new ChildrenGenerator(A.Fake<ICrossoverManager>(), new BasicMutationProbabilityManager(0),
                    selectionStrategy);
                childrenGenerator.GenerateChildren(GetPopulation(1), 1, 0, null);
            }, typeof(GeneticAlgorithmException));
        }

        /// <summary>
        /// Returns a population with count chromosome
        /// </summary>
        /// <returns></returns>
        private Population GetPopulation(int count, Action onMutation = null)
        {
            var chromosomes = new List<IChromosome>();
            for (int i = 0; i < count / 3; i++)
                chromosomes.Add(CreateNewChromosome(onMutation));

            var population = new Population(chromosomes.ToArray());
            foreach (var chromosome in population)
                chromosome.Evaluation = chromosome.Chromosome.Evaluate();

            return population;
        }

        private IChromosome CreateNewChromosome(Action onMutation)
        {
            var chromosome = A.Fake<IChromosome>();
            A.CallTo(() => chromosome.Evaluate()).Returns(1);
            A.CallTo(() => chromosome.Mutate()).Invokes(onMutation);
            return chromosome;
        }
    }
}
