using System;
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
            MostLikelyToChooseBestChromosome(new RouletteWheelSelection(), chromosome1Probability,
                chromosome2Probability, chromosome3Probability);
        }

        [TestMethod]
        public void RouletteWheelSelection_OnlyUsesLastPopulation() =>
            AssertSelectionStrategyUsesLatestPopulation(new RouletteWheelSelection());

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

        private void AssertIsWithinRange(int value, double probability, int tries, string valueName)
        {
            const double errorMargin = 0.05;
            var expected = probability * runs;
            var min = expected - errorMargin * tries;
            var max = expected + errorMargin * tries;

            Assert.IsTrue(value > min, $"Value ({value}) in smaller than min ({min}) for {valueName}");
            Assert.IsTrue(value < max, $"Value ({value}) in greater than max ({max}) for {valueName}");
        }
    }
}
