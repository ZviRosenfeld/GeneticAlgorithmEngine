using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.SelectionStrategies;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class SelectionStrategyTests
    {
        private const int runs = 3000;
        private const double chromosome1Probability = 0.1, chromosome2Probability = 0.3, chromosome3Probability = 0.6;
        private Population population;

        [TestInitialize]
        public void TestInitialize()
        {
            population = new Population(new [] { chromosome1Probability * 2, chromosome2Probability * 2, chromosome3Probability * 2}.ToChromosomes());
            population.Evaluate();
        }

        [TestMethod]
        public void RouletteWheelSelection_MostLieklyToChooseBestChromosome()
        {
            var rouletteWheelSelection = new RouletteWheelSelection();
            rouletteWheelSelection.SetPopulation(population);

            int chromosome1Counter = 0, chromosome2Counter = 0, chromosome3Counter = 0;
            for (int i = 0; i < runs; i++)
            {
                var chromosome = rouletteWheelSelection.SelectChromosome();
                if (chromosome.Evaluate() == chromosome1Probability * 2)
                    chromosome1Counter++;
                if (chromosome.Evaluate() == chromosome2Probability * 2)
                    chromosome2Counter++;
                if (chromosome.Evaluate() == chromosome3Probability * 2)
                    chromosome3Counter++;
            }

            AssertIsWithinRange(chromosome1Counter, chromosome1Probability, runs, "Chromosome1");
            AssertIsWithinRange(chromosome2Counter, chromosome2Probability, runs, "Chromosome2");
            AssertIsWithinRange(chromosome3Counter, chromosome3Probability, runs, "Chromosome3");
        }

        [TestMethod]
        public void RouletteWheelSelection_OnlyUsesLastPopulation() =>
            AssertSelectionStrategyUsesLatestPopulation(new RouletteWheelSelection());

        private void AssertSelectionStrategyUsesLatestPopulation(ISelectionStrategy selectionStrategy)
        {
            selectionStrategy.SetPopulation(population);
            selectionStrategy.SetPopulation(new Population(new double[]{2, 0, 0}.ToChromosomes()));
            for (int i = 0; i < runs; i++)
            {
                var chromosome = selectionStrategy.SelectChromosome();
                Assert.AreEqual(2, chromosome.Evaluate(), "Got wrong chromosome");
            }
        }

        private void AssertIsWithinRange(int value, double probability, int tries, string valueName)
        {
            const double errorMargin = 0.05;
            var expected = probability * runs;
            var min = expected - errorMargin * tries * 2;
            var max = expected + errorMargin * tries * 2;

            Assert.IsTrue(value > min, $"Value ({value}) in smaller than min ({min}) for {valueName}");
            Assert.IsTrue(value < max, $"Value ({value}) in greater than max ({max}) for {valueName}");
        }
    }
}
