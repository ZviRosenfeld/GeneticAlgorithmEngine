using System;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly Random random = new Random();
        private IChromosome[] chromosomes;
        private double[] evaluations;

        public void SetPopulation(Population population, int requestedChromosomes)
        {
            chromosomes = population.GetChromosomes();
            evaluations = NormilizeEvaluations(population);
        }

        public IChromosome SelectChromosome()
        {
            var randomNumber = random.NextDouble();
            var sum = 0.0;
            var index = -1;
            while (sum < randomNumber)
            {
                index++;
                sum += evaluations[index];
            }

            return chromosomes[index];
        }

        private double[] NormilizeEvaluations(Population population)
        {
            population = population.Clone();
            var total = population.GetEvaluations().Sum();
            Parallel.ForEach(population, chromosome =>
            {
                chromosome.Evaluation = chromosome.Evaluation / total;
            });
            return population.GetEvaluations();
        }
    }
}
