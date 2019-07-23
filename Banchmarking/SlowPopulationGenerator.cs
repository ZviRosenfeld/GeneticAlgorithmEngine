using System.Collections.Generic;
using GeneticAlgorithm.Interfaces;

namespace Benchmarking
{
    class SlowPopulationGenerator : IPopulationGenerator
    {
        private readonly int sleepTime;

        public SlowPopulationGenerator(int sleepTime)
        {
            this.sleepTime = sleepTime;
        }

        public IEnumerable<IChromosome> GeneratePopulation(int size)
        {
            var population = new IChromosome[size];
            for (int i = 0; i < size; i++)
                population[i] = new SlowChromosome(1, sleepTime);
            
            return population;
        }
    }
}
