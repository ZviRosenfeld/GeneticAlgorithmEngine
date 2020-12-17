using System;
using System.Linq;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.SelectionStrategies
{
    /// <summary>
    /// With TournamentSelection, we choose a random n chromosomes from the population, and of them select the chromosome with the highest evaluation.
    /// In TournamentSelection, selection pressure will grow as the tournament size grows. 
    /// </summary>
    public class TournamentSelection : ISelectionStrategy
    {
        private readonly int tournamentSize;
        private Population population;
        private readonly double percentage;


        public TournamentSelection(int tournamentSize)
        {
            this.tournamentSize = tournamentSize > 0
                ? tournamentSize
                : throw new GeneticAlgorithmException($"{nameof(tournamentSize)} must be greater then 0");

            percentage = 1;
        }

        /// <param name="percentage">A double between 0 (not including) and 1 (including). If set, the selection will only consider the n-percent best chromosomes (0 means will consider no chromosomes, and 1 means we'll consider all chromosomes).</param>
        public TournamentSelection(int tournamentSize, double percentage) : this(tournamentSize)
        {
            if (percentage <= 0 || percentage > 1)
                throw new GeneticAlgorithmException($"{nameof(percentage)} must be between 0 (not including) and 1 (including). Was {percentage}.");

            this.percentage = percentage;
        }

        public void SetPopulation(Population population, int requestedChromosomes)
        {
            this.population = population.GetBestChromosomes((int)Math.Ceiling(population.Count() * percentage));
        }

        public IChromosome SelectChromosome()
        {
            ChromosomeEvaluationPair bestChromosome = null;
            for (int i = 0; i < tournamentSize; i++)
            {
                var chromosome = population[ProbabilityUtils.GetRandomInt(0, population.Count())];
                if (bestChromosome == null || chromosome.Evaluation > bestChromosome.Evaluation)
                    bestChromosome = chromosome;
            }

            return bestChromosome.Chromosome;
        }
    }
}
