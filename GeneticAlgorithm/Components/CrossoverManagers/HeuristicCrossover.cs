using System;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers.Utilities;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// HeuristicCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, HeuristicCrossover may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// In HeuristicCrossover, the child is guaranteed to contain each genome exactly once.
    /// 
    /// HeuristicCrossover is almost the same as EdgeRecombinationCrossover. 
    /// The only diffrance is that in HeuristicCrossover we select the next neighbor at random from the neighbors of the current element.
    /// In EdgeRecombinationCrossover we take the current element's neighbor with the least neighbors.
    /// </summary>
    public class HeuristicCrossover<T> : ICrossoverManager
    {
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;
        private readonly Random random = new Random();

        /// <summary>
        /// HeuristicCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, HeuristicCrossover may throw an exception.
        /// 
        /// Also, the Equals method must be implemented for type T.
        /// </summary>
        public HeuristicCrossover(IMutationManager<T> mutationManager, IEvaluator evaluator)
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

            var childArray = new NonReapetingAdjacencyMatrix<T>(vector1, vector2, false).Crossover(firstElement, length);
            return new VectorChromosome<T>(childArray, mutationManager, evaluator);
        }
    }
}
