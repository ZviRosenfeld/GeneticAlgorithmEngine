using System;
using System.Linq;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.SelectionStrategies
{
    /// <summary>
    /// In this strategy, the chance of choosing a chromosome is equal to the chromosome's fitness divided by the total fitness.
    /// In other words, if we have two chromosomes, A and B, where A.Evaluation == 6 and B.Evaluation == 4,
    /// there's a 60% change of choosing A, and a 40% change of choosing B.
    /// </summary>
    public class RouletteWheelSelection : ISelectionStrategy
    {
        private IChromosome[] chromosomes;
        private double[] evaluations;
        private readonly double percentage;
        
        public RouletteWheelSelection()
        {
            percentage = 1;
        }

        /// <param name="percentage">A double between 0 (not including) and 1 (including). If set, the selection will only consider the n-percent best chromosomes (0 means will consider no chromosomes, and 1 means we'll consider all chromosomes).</param>
        public RouletteWheelSelection(double percentage)
        {
            if (percentage <= 0 || percentage > 1)
                throw new GeneticAlgorithmException($"{nameof(percentage)} must be between 0 (not including) and 1 (including). Was {percentage}.");

            this.percentage = percentage;
        }

        public void SetPopulation(Population population, int requestedChromosomes)
        {
            population = population.GetBestChromosomes((int) Math.Ceiling(population.Count() * percentage));

            chromosomes = population.GetChromosomes();
            evaluations = population.GetNormilizeEvaluations();
        }

        public IChromosome SelectChromosome()
        {
            var randomNumber = ProbabilityUtils.GetRandomDouble();
            var sum = 0.0;
            var index = -1;
            while (sum < randomNumber)
            {
                index++;
                sum += evaluations[index];
            }

            return chromosomes[index];
        }
    }
}
