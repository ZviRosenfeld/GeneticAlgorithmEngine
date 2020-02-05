using System;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers.Utilities;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// For a genome at location i, the genomes at locations (i - 1) and (i + 1) are it's neighbors.
    /// In GeneralEdgeRecombinationCrossover we start with a random geone in one of the pantns.
    /// From there, we randomlly select one of the element's neigbors in either the first or second parant.
    /// We repeat this process will we reach the length of the first parent.
    /// </summary>
    public class GeneralEdgeRecombinationCrossover<T> : ICrossoverManager
    {
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;
        private readonly Random random = new Random();

        public GeneralEdgeRecombinationCrossover(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vector1 = ((VectorChromosome<T>)chromosome1).GetVector();
            var vector2 = ((VectorChromosome<T>)chromosome2).GetVector();
            var length = vector1.Length;
            var firstElement = vector1[random.Next(0, vector1.Length)];

            var childArray = new ReapetingAdjacencyMatrix<T>(vector1, vector2).Crossover(firstElement, length);
            return new VectorChromosome<T>(childArray, mutationManager, evaluator);
        }
    }
}
