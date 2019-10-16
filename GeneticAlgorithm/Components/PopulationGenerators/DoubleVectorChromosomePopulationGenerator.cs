using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    public class DoubleVectorChromosomePopulationGenerator : IPopulationGenerator
    {
        private readonly Random random = new Random();
        private readonly int vectorSize;
        private readonly double minGenome;
        private readonly double range;
        private readonly IMutationManager<double> mutationManager;
        private readonly IEvaluator evaluator;

        public DoubleVectorChromosomePopulationGenerator(int vectorSize, double minGenome, double maxGenome, IMutationManager<double> mutationManager, IEvaluator evaluator)
        {
            if (vectorSize <= 0)
                throw new GeneticAlgorithmException($"{nameof(vectorSize)} must be bigger than 0. It was {vectorSize}");
            if (maxGenome < minGenome)
                throw new GeneticAlgorithmException($"{nameof(maxGenome)} ({maxGenome}) must be bigger than {nameof(minGenome)} ({minGenome})");

            this.vectorSize = vectorSize;
            this.minGenome = minGenome;
            range = maxGenome - minGenome;
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

        private VectorChromosome<double> GetChromosome()
        {
            var vector = new double[vectorSize];
            for (int i = 0; i < vectorSize; i++)
                vector[i] = minGenome + random.NextDouble() * range;

            return new VectorChromosome<double>(vector, mutationManager, evaluator);
        }
    }
}
