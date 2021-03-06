﻿using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class SearchUtilsTests
    {
        private static IChromosome chromosome1 = A.Fake<IChromosome>();
        private static IChromosome chromosome2 = A.Fake<IChromosome>();
        private static IChromosome chromosome3 = A.Fake<IChromosome>();

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            A.CallTo(() => chromosome1.ToString()).Returns(nameof(chromosome1));
            A.CallTo(() => chromosome2.ToString()).Returns(nameof(chromosome2));
            A.CallTo(() => chromosome3.ToString()).Returns(nameof(chromosome3));
        }

        [TestMethod]
        public void ChooseBestTest()
        {
            A.CallTo(() => chromosome1.Evaluate()).Returns(1);
            A.CallTo(() => chromosome2.Evaluate()).Returns(8);
            A.CallTo(() => chromosome3.Evaluate()).Returns(0.5);

            var population = new Population(new[] {chromosome1, chromosome2, chromosome3}).Evaluate();
            var best = population.ChooseBest();

            Assert.AreEqual(chromosome2, best);
        }

        [TestMethod]
        public void CombineTest_BothArraysContainElements()
        {
            var population1 = new[] { chromosome1, chromosome2 };
            var population2 = new[] {chromosome3};

            var newPopulation = SearchUtils.Combine(population1, population2);

            Assert.AreEqual(newPopulation[0], chromosome1);
            Assert.AreEqual(newPopulation[1], chromosome2);
            Assert.AreEqual(newPopulation[2], chromosome3);
        }

        [TestMethod]
        public void CombineTest_FirstArrayEmpty()
        {
            var population1 = new IChromosome[] {};
            var population2 = new[] { chromosome1, chromosome2 , chromosome3 };

            var newPopulation = SearchUtils.Combine(population1, population2);

            Assert.AreEqual(newPopulation[0], chromosome1);
            Assert.AreEqual(newPopulation[1], chromosome2);
            Assert.AreEqual(newPopulation[2], chromosome3);
        }

        [TestMethod]
        public void CombineTest_SecondArrayEmty()
        {
            var population1 = new[] { chromosome1, chromosome2, chromosome3 };
            var population2 = new IChromosome[] { };

            var newPopulation = SearchUtils.Combine(population1, population2);

            Assert.AreEqual(newPopulation[0], chromosome1);
            Assert.AreEqual(newPopulation[1], chromosome2);
            Assert.AreEqual(newPopulation[2], chromosome3);
        }

        [TestMethod]
        public void ClonePopulation_PopulationCloned()
        {
            var population = (new double[] { 1, 2, 1, 3 }).ToPopulation("Chromosomes").Evaluate();
            var populationClone = population.Clone();

            population.AssertIsSame(populationClone);
        }

        [TestMethod]
        public void ClonePopulation_ChangingCloneDosntChangeOriginal()
        {
            var population = (new double[] { 1, 1, 1, 1 }).ToPopulation("Chromosomes");

            var populationClone = population.Clone();
            var fakeChromosome = ChromosomeFactory.CreateChromosome(10, "New");
            populationClone.GetChromosomes()[0] = fakeChromosome;

            foreach (var chromosome in population.GetChromosomes())
                Assert.AreNotEqual(fakeChromosome, chromosome);
        }
    }
}
