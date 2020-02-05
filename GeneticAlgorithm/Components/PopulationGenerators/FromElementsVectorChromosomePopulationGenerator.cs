using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    /// <summary>
    /// Creates a population of chromosomes of type VectorChromosome&lt;T&gt; by using the elements provided to it in its constructor.
    /// </summary>
    public class FromElementsVectorChromosomePopulationGenerator<T> : IPopulationGenerator
    {
        private readonly T[] elements;
        private readonly int length;
        private readonly bool repeatElements;
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;
        private readonly Random random = new Random();

        /// <summary>
        /// Creates a population of chromosomes of type VectorChromosome&lt;T&gt; by using the elements in element.
        /// </summary>
        public FromElementsVectorChromosomePopulationGenerator(T[] elements, int vectorLength, bool repeatElements, IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            if (elements.Length == 0)
                throw new GeneticAlgorithmException($"{nameof(elements)} is empty");

            if (vectorLength <= 0)
                throw new GeneticAlgorithmException($"{nameof(vectorLength)} must be greater than 0 (it was {vectorLength})");

            if (!repeatElements && elements.Length < vectorLength)
                throw new GeneticAlgorithmException($"{nameof(elements)}.Count is only {elements.Length}, while {nameof(vectorLength)} is {vectorLength}.");
            
            this.repeatElements = repeatElements;
            length = vectorLength;
            this.elements = elements;
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IEnumerable<IChromosome> GeneratePopulation(int size)
        {
            var population = new IChromosome[size];

            for (int i = 0; i < size; i++)
            {
                var vector = repeatElements
                    ? SelectKRandomElementsRepeating(elements, length)
                    : elements.SelectKRandomElementsNonRepeating(length).Shuffle(random);
                population[i] = new VectorChromosome<T>(vector, mutationManager, evaluator);
            }
            return population;
        }
        
        private T[] SelectKRandomElementsRepeating(T[] allElements, int k)
        {
            var selectedElements = new T[k];
            for (int i = 0; i < k; i++)
                selectedElements[i] = allElements[random.Next(allElements.Length)];

            return selectedElements;
        }
    }
}
