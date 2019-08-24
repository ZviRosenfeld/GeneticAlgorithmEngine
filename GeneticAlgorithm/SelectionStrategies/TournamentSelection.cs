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
        private readonly Random random = new Random();
        private Population population;

        public TournamentSelection(int tournamentSize)
        {
            this.tournamentSize = tournamentSize > 0
                ? tournamentSize
                : throw new GeneticAlgorithmException($"{nameof(tournamentSize)} must be greater then 0");
        }

        public void SetPopulation(Population population, int requestedChromosomes)
        {
            this.population = population;
        }

        public IChromosome SelectChromosome()
        {
            ChromosomeEvaluationPair bestChromosome = population[0];
            for (int i = 0; i < tournamentSize; i++)
            {
                var chromosome = population[random.Next(0, population.Count())];
                if (bestChromosome == null || chromosome.Evaluation > bestChromosome.Evaluation)
                    bestChromosome = chromosome;
            }

            return bestChromosome.Chromosome;
        }
    }
}
