using System;
using System.Collections.Generic;
using GeneticAlgorithm.Interfaces;

namespace GreatestVectorTests
{
    /// <summary>
    /// Returns 10 vectors, where each has 10 in one cell, and one in all the others
    /// </summary>
    public class NumberVectorPopulationGenerator : IPopulationGenerator
    {
        private readonly Random random = new Random();
        public const int VECTOR_SIZE = 10;

        public IEnumerable<IChromosome> GeneratePopulation(int size)
        {
            var population = new IChromosome[size];

            for (int i = 0; i < size; i++)
                population[i] = GetChromosome();

            return population;
        }

        private NumberVectorChromosome GetChromosome()
        {
            var vector = new int[VECTOR_SIZE];
            for (int i = 0; i < VECTOR_SIZE; i++)
                vector[i] = random.NextDouble() < 0.5 ? 0 : 1;
            
            return new NumberVectorChromosome(vector);
        }
    }
}
