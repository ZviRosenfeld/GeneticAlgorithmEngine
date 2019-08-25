using System;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.SelectionStrategies;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.SelectionStrategyTests
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
            population = ChromosomeFactory.ToPopulation(new [] { chromosome1Probability * 2, chromosome2Probability * 2, chromosome3Probability * 2});
            Utils.Evaluate(population);
        }

        [TestMethod]
        public void RouletteWheelSelection_MostLieklyToChooseBestChromosome()
        {
            MostLikelyToChooseBestChromosome(new RouletteWheelSelection(), chromosome1Probability,
                chromosome2Probability, chromosome3Probability);
        }

        [TestMethod]
        public void RouletteWheelSelection_OnlyUsesLastPopulation() =>
            AssertSelectionStrategyUsesLatestPopulation(new RouletteWheelSelection());

        [TestMethod]
        public void RouletteWheelSelection_AssertChromosomesAreScattered() =>
            AssertChromosomesAreScattered(new RouletteWheelSelection());

        [TestMethod]
        [DataRow(2)]
        [DataRow(3)]
        public void TournamentSelection_MostLieklyToChooseBestChromosome(int tournamentSize)
        {
            var probability1 = Math.Pow((double) 1 / 3, tournamentSize);
            var probability3 = 1 - Math.Pow((double) 2 / 3, tournamentSize);
            var probability2 = 1 - probability1 - probability3;
            MostLikelyToChooseBestChromosome(new TournamentSelection(tournamentSize), probability1, probability2, probability3);
        }

        [TestMethod]
        public void TournamentSelection_OnlyUsesLastPopulation() =>
            AssertSelectionStrategyUsesLatestPopulation(new TournamentSelection(2));

        [TestMethod]
        public void TournamentSelection_AssertChromosomesAreScattered() =>
            AssertChromosomesAreScattered(new TournamentSelection(2));

        [TestMethod]
        public void StochasticUniversalSampling_MostLieklyToChooseBestChromosome()
        {
            MostLikelyToChooseBestChromosome(new StochasticUniversalSampling(), chromosome1Probability,
                chromosome2Probability, chromosome3Probability);
        }

        [TestMethod]
        public void StochasticUniversalSampling_OnlyUsesLastPopulation() =>
            AssertSelectionStrategyUsesLatestPopulation(new StochasticUniversalSampling());

        [TestMethod]
        public void StochasticUniversalSampling_AssertChromosomesAreScattered() =>
            AssertChromosomesAreScattered(new StochasticUniversalSampling());

        private void MostLikelyToChooseBestChromosome(ISelectionStrategy selection, double chromosome1Probability, double chromosome2Probability, double chromosome3Probability)
        {
            selection.SetPopulation(population, runs);

            int chromosome1Counter = 0, chromosome2Counter = 0, chromosome3Counter = 0;
            for (int i = 0; i < runs; i++)
            {
                var chromosome = selection.SelectChromosome();
                if (chromosome.Evaluate() == SelectionStrategyTests.chromosome1Probability * 2)
                    chromosome1Counter++;
                if (chromosome.Evaluate() == SelectionStrategyTests.chromosome2Probability * 2)
                    chromosome2Counter++;
                if (chromosome.Evaluate() == SelectionStrategyTests.chromosome3Probability * 2)
                    chromosome3Counter++;
            }

            AssertIsWithinRange(chromosome1Counter, chromosome1Probability, runs, "Chromosome1");
            AssertIsWithinRange(chromosome2Counter, chromosome2Probability, runs, "Chromosome2");
            AssertIsWithinRange(chromosome3Counter, chromosome3Probability, runs, "Chromosome3");
        }

        private void AssertIsWithinRange(int value, double probability, int tries, string valueName)
        {
            const double errorMargin = 0.05;
            var expected = probability * runs;
            var min = expected - errorMargin * tries;
            var max = expected + errorMargin * tries;

            Assert.IsTrue(value > min, $"Value ({value}) in smaller than min ({min}) for {valueName}");
            Assert.IsTrue(value < max, $"Value ({value}) in greater than max ({max}) for {valueName}");
        }

        private void AssertSelectionStrategyUsesLatestPopulation(ISelectionStrategy selectionStrategy)
        {
            selectionStrategy.SetPopulation(population, runs);
            selectionStrategy.SetPopulation(new Population(new double[]{2, 0, 0}.ToChromosomes()), runs);
            for (int i = 0; i < runs; i++)
            {
                var chromosome = selectionStrategy.SelectChromosome();
                Assert.AreEqual(2, chromosome.Evaluate(), "Got wrong chromosome");
            }
        }

        /// <summary>
        /// Requests 8 chromosomes, and makes sure we get all 4 at least once.
        /// This test checks that the chromosomes are well distributed.
        /// </summary>
        private void AssertChromosomesAreScattered(ISelectionStrategy selectionStrategy)
        {
            var population = new[] { 0.23, 0.24, 0.26, 0.27 }.ToPopulation();
            population.Evaluate();
            selectionStrategy.SetPopulation(population, 100);

            bool chromosome1 = false, chromosome2 = false, chromosome3 = false, chromosome4 = false;
            for (int i = 0; i < 8; i++)
            {
                var chromosome = selectionStrategy.SelectChromosome();
                if (chromosome.Evaluate() == 0.23)
                    chromosome1 = true;
                if (chromosome.Evaluate() == 0.24)
                    chromosome2 = true;
                if (chromosome.Evaluate() == 0.26)
                    chromosome3 = true;
                if (chromosome.Evaluate() == 0.27)
                    chromosome4 = true;
            }

            Assert.IsTrue(chromosome1, "Didn't get any " + nameof(chromosome1));
            Assert.IsTrue(chromosome2, "Didn't get any " + nameof(chromosome2));
            Assert.IsTrue(chromosome3, "Didn't get any " + nameof(chromosome3));
            Assert.IsTrue(chromosome4, "Didn't get any " + nameof(chromosome4));
        }
    }
}
