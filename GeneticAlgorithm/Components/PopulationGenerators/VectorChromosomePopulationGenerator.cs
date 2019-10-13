using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    public class VectorChromosomePopulationGenerator : IPopulationGenerator
    {
        private readonly Random random = new Random();
        private readonly int vectorSize;
        private readonly int minGenome;
        private readonly int maxGenome;
        private readonly IMutationManager mutationManager;
        private readonly IEvaluator evaluator;

        public VectorChromosomePopulationGenerator(int vectorSize, int minGenome, int maxGenome, IMutationManager mutationManager, IEvaluator evaluator)
        {
            if (vectorSize <= 0)
                throw new GeneticAlgorithmException($"{nameof(vectorSize)} must be bigger than 0. It was {vectorSize}");
            if (maxGenome < minGenome)
                throw new GeneticAlgorithmException($"{nameof(maxGenome)} ({maxGenome}) must be bigger than {nameof(minGenome)} ({minGenome})");

            this.vectorSize = vectorSize;
            this.minGenome = minGenome;
            this.maxGenome = maxGenome;
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IEnumerable<IChromosome> GeneratePopulation(int size)
        {
            var population = new IChromosome[size];

            for (int i = 0; i < size; i++)
                population[i] = GetChromosome();

            return population;
        }

        private VectorChromosome GetChromosome()
        {
            var vector = new int[vectorSize];
            for (int i = 0; i < vectorSize; i++)
                vector[i] = random.Next(minGenome, maxGenome + 1);

            return new VectorChromosome(vector, mutationManager, evaluator);
        }
    }
}
