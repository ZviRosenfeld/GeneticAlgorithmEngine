using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    /// <summary>
    /// Creates a population of chromosomes of type VectorChromosome&lt;T&gt in which each chromosome contains every element exactly once.
    /// </summary>
    public class AllElementsVectorChromosomePopulationGenerator<T> : IPopulationGenerator
    {
        private readonly ICollection<T> elements;
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;
        private readonly Random random = new Random();

        /// <summary>
        /// Creates a population of chromosomes of type VectorChromosome&lt;T&gt in which each chromosome contains every element exactly once.
        /// </summary>
        public AllElementsVectorChromosomePopulationGenerator(ICollection<T> elements, IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            if (!elements.Any())
                throw new GeneticAlgorithmException($"{nameof(elements)} is empty");

            this.elements = elements;
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IEnumerable<IChromosome> GeneratePopulation(int size)
        {
            var population = new IChromosome[size];

            for (int i = 0; i < size; i++)
                population[i] = new VectorChromosome<T>(elements.Shuffle(random), mutationManager, evaluator);

            return population;
        }
    }
}
