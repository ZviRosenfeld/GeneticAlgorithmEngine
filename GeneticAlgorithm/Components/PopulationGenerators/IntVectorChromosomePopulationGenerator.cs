using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    public class IntVectorChromosomePopulationGenerator : IPopulationGenerator
    {
        private readonly Random random = new Random();
        private readonly int vectorSize;
        private readonly int minGenome;
        private readonly int maxGenome;
        private readonly IMutationManager<int> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// This class will create chromosomes of type VectorChromosome&lt;int&gt;
        /// </summary>
        /// <param name="vectorSize">The size of the generated chromosomes</param>
        /// <param name="minGenome">The genomes will be equal to or greater than minGenom</param>
        /// <param name="maxGenome">The genomes will be equal to or smaller than minGenom</param>
        /// <param name="mutationManager">A mutation manager to use</param>
        /// <param name="evaluator">An evaluator to use</param>
        public IntVectorChromosomePopulationGenerator(int vectorSize, int minGenome, int maxGenome, IMutationManager<int> mutationManager, IEvaluator evaluator)
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

        private VectorChromosome<int> GetChromosome()
        {
            var vector = new int[vectorSize];
            for (int i = 0; i < vectorSize; i++)
                vector[i] = random.Next(minGenome, maxGenome + 1);

            return new VectorChromosome<int>(vector, mutationManager, evaluator);
        }
    }
}
