using System.ComponentModel.DataAnnotations;
using FakeItEasy;
using GeneticAlgorithm;
using GeneticAlgorithm.Interfaces;
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

            var population = new[] {chromosome1, chromosome2, chromosome3};
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
        public void CombineTest_FirstArrayEmty()
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
        public void ChooseParent_MostLieklyToChooseBestChromosome()
        {
            const int tries = 1000;
            const double chromosome1Probability = 0.1, chromosome2Probability = 0.3, chromosome3Probability = 0.6;
            int chromosome1Counter = 0, chromosome2Counter = 0, chromosome3Counter = 0;
            var evaluation = new[] {chromosome1Probability, chromosome2Probability, chromosome3Probability};
            var population = new[] {chromosome1, chromosome2, chromosome3};

            for (int i = 0; i < tries; i++)
            {
                var result = SearchUtils.ChooseParent(population, evaluation);
                if (result == chromosome1) chromosome1Counter++;
                if (result == chromosome2) chromosome2Counter++;
                if (result == chromosome3) chromosome3Counter++;
            }

            AssertIsWithinRange(chromosome1Counter, chromosome1Probability, tries, nameof(chromosome1Counter));
            AssertIsWithinRange(chromosome2Counter, chromosome2Probability, tries, nameof(chromosome2Counter));
            AssertIsWithinRange(chromosome3Counter, chromosome3Probability, tries, nameof(chromosome3Counter));
        }

        private void AssertIsWithinRange(int value, double probability, int tries, string valueName)
        {
            const double errorMargin = 0.05;
            var expected = probability * tries;
            var min = expected - errorMargin * tries;
            var max = expected + errorMargin * tries;

            Assert.IsTrue(value > min, $"Value ({value}) in smaller than min ({min}) for {valueName}");
            Assert.IsTrue(value < max, $"Value ({value}) in greater than max ({max}) for {valueName}");
        }
    }
}
