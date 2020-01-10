using System;
using System.Linq;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.SelectionStrategies
{
    /// <summary>
    /// StochasticUniversalSampling (SUS) is very similar to RouletteWheelSelection.
    /// For more information see: https://en.wikipedia.org/wiki/Stochastic_universal_sampling
    /// </summary>
    public class StochasticUniversalSampling : ISelectionStrategy
    {
        private readonly Random random = new Random();
        private ChromosomePool pool;
        private readonly double percentage;

        public StochasticUniversalSampling()
        {
            percentage = 1;
        }

        /// <param name="percentage">A double between 0 (not including) and 1 (including). If set, the selection will only consider the n-percent best chromosomes (0 means will consider no chromosomes, and 1 means we'll consider all chromosomes).</param>
        public StochasticUniversalSampling(double percentage)
        {
            if (percentage <= 0 || percentage > 1)
                throw new GeneticAlgorithmException($"{nameof(percentage)} must be between 0 (not including) and 1 (including). Was {percentage}.");

            this.percentage = percentage;
        }

        public void SetPopulation(Population population, int requestedChromosomes)
        {
            population = population.GetBestChromosomes((int) Math.Ceiling(population.Count() * percentage));

            var chromosomes = population.GetChromosomes();
            var evaluations = population.GetNormilizeEvaluations();

            FillPool(requestedChromosomes, chromosomes, evaluations);
        }

        private void FillPool(int requestedChromosomes, IChromosome[] chromosomes, double[] evaluations)
        {
            var pointer = random.NextDouble() / requestedChromosomes;
            var increment = 1.0 / requestedChromosomes;
            var sum = 0.0;
            var chromosomeIndex = -1;
            var poolChromosomes = new IChromosome[requestedChromosomes];
            for (int i = 0; i < requestedChromosomes; i++)
            {
                while (sum <= pointer)
                {
                    chromosomeIndex++;
                    sum += evaluations[chromosomeIndex];
                }
                poolChromosomes[i] = chromosomes[chromosomeIndex];
                pointer += increment;
            }

            pool = new ChromosomePool(poolChromosomes);
        }

        public IChromosome SelectChromosome() => pool.GetChromosome();
    }
}
