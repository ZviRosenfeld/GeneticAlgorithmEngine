using System.Linq;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.SelectionStrategies;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.SelectionStrategyTests
{
    [TestClass]
    public class SelectionStrategyUtilsTests
    {
        [TestMethod]
        public void GetNormilizeEvaluationsTest()
        {
            var population = (new double[] { 1, 2, 6, 1 }).ToPopulation().Evaluate();
            var normilizeEvaluations = population.GetNormilizeEvaluations();

            new []{0.1, 0.2, 0.6, 0.1}.AssertAreTheSame(normilizeEvaluations.ToArray());
        }

        [TestMethod]
        public void GetNormilizeEvaluations_OriginalPopulationNotChanged()
        {
            var originalEvaluations = new double[] {1, 2, 6, 1};
            var population = originalEvaluations.ToPopulation().Evaluate();
            population.GetNormilizeEvaluations();

            originalEvaluations.AssertAreTheSame(population.GetEvaluations());
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(10)]
        public void GetBestChromosomesTest(int numberOfBestChromosomes)
        {
            var population = new double[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9}.ToPopulation().Evaluate();
            var newPopulation = population.GetBestChromosomes(numberOfBestChromosomes);

            var newEvaluations = newPopulation.GetEvaluations();
            var minChromosomeInNewPopulation = 10 - numberOfBestChromosomes;
            for (int i = minChromosomeInNewPopulation; i >= minChromosomeInNewPopulation; i--)
                Assert.IsTrue(newEvaluations.Any(v => v == i), $"{nameof(newPopulation)} doesn't contain {i}");

            Assert.AreEqual(numberOfBestChromosomes ,newPopulation.Count(), $"To many chromosome in {nameof(newPopulation)}");
        }

        [TestMethod]
        public void GetBestChromosomes_OriginalPopulationNotChanged()
        {
            var originalEvaluations = new double[] { 1, 2, 6, 1 };
            var population = originalEvaluations.ToPopulation().Evaluate();
            population.GetBestChromosomes(1);

            Assert.AreEqual(4, population.Count());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(-2)]
        [DataRow(4)]
        [ExpectedException(typeof(InternalSearchException), "Code 1006")]
        public void GetBestChromosomes_BadNumberOfChromosomes_ThrowException(int numberOfBestChromosomes)
        {
            var population = new double[] { 1, 1, 1 }.ToPopulation().Evaluate();
            population.GetBestChromosomes(numberOfBestChromosomes);
        }
    }
}
