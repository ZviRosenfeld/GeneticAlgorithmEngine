using System;
using System.Collections.Generic;
using GeneticAlgorithm.Interfaces;

namespace Environment
{
    class PopulationGenerator : IPopulationGenerator
    {
        private readonly Random random = new Random();

        public IEnumerable<IChromosome> GeneratePopulation(int size)
        {
            var chromosomes = new IChromosome[size];
            for (int i = 0; i < size; i++)
                chromosomes[i] = new MyChromosome(random.NextDouble() < 0.5
                    ? ChromosomeType.OProducer
                    : ChromosomeType.Oc2Producer);

            return chromosomes;
        }
    }
}
