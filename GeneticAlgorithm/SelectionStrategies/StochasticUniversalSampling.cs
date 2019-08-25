using System;
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
        
        public void SetPopulation(Population population, int requestedChromosomes)
        {
            var chromosomes = population.GetChromosomes();
            var evaluations = population.GetNormilizeEvaluations();

            FillPool(requestedChromosomes, chromosomes, evaluations);
        }

        private void FillPool(int requestedChromosomes, IChromosome[] chromosomes, double[] evaluations)
        {
            var pointer = random.NextDouble() / requestedChromosomes;
            var increment = 1.0 / (double) requestedChromosomes;
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
