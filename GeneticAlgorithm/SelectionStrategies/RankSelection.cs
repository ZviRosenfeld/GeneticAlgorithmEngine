using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
using System;
using System.Linq;

namespace GeneticAlgorithm.SelectionStrategies
{
    /// <summary>
    /// RankSelection firsts ranks the chromosomes based on their evaluation. The worst will have fitness 1, second worst 2 etc. and the best will have fitness N (number of chromosomes in population).
    /// RankSelection is very similar to RouletteWheelSelection, but can lead to slower convergence, because the best chromosomes do not differ so much from other ones.
    /// </summary>
    public class RankSelection : ISelectionStrategy
    {
        private ChromosomeEvaluationPair[] SortedPopulation;
        private readonly double percentage;

        public RankSelection()
        {
            percentage = 1;
        }

        /// <param name="percentage">A double between 0 (not including) and 1 (including). If set, the selection will only consider the n-percent best chromosomes (0 means will consider no chromosomes, and 1 means we'll consider all chromosomes).</param>
        public RankSelection(double percentage)
        {
            if (percentage <= 0 || percentage > 1)
                throw new GeneticAlgorithmException($"{nameof(percentage)} must be between 0 (not including) and 1 (including). Was {percentage}.");

            this.percentage = percentage;
        }

        public IChromosome SelectChromosome()
        {
            var randomNumber = ProbabilityUtils.GetRandomDouble();
            var sum = 0.0;
            var index = -1;
            while (sum < randomNumber)
            {
                index++;
                sum += SortedPopulation[index].Evaluation;
            }

            return SortedPopulation[index].Chromosome;
        }

        public void SetPopulation(Population population, int requestedChromosomes)
        {
            population = population.GetBestChromosomes((int)Math.Ceiling(population.Count() * percentage));
            SortedPopulation = population.OrderBy(p => p.Evaluation).ToArray();
            var n = SortedPopulation.Length;
            var ranksSum = (Math.Pow(n, 2) + n) / 2;
            for (var i = 0; i < n; i++)
                SortedPopulation[i].Evaluation = (i + 1) / ranksSum;
        }
    }
}
