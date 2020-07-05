using System;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers.Utilities;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// EdgeRecombinationCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, EdgeRecombinationCrossover may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// In EdgeRecombinationCrossover, the child is guaranteed to contain each genome exactly once.
    /// 
    /// Every genome has two neighborers in each chromosome - or between 2 to 4 neighbors between both its parents.
    /// In EdgeRecombinationCrossover we randomly chose a neighbor from one of these and continue with it.
    /// See: https://en.wikipedia.org/wiki/Edge_recombination_operator
    /// </summary>
    public class EdgeRecombinationCrossover<T> : ICrossoverManager
    {
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// EdgeRecombinationCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, EdgeRecombinationCrossover may throw an exception.
        /// 
        /// Also, the Equals method must be implemented for type T.
        /// </summary>
        public EdgeRecombinationCrossover(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vector1 = ((VectorChromosome<T>)chromosome1).GetVector();
            var vector2 = ((VectorChromosome<T>)chromosome2).GetVector();
            var length = vector1.Length;
            var firstElement = vector1[ProbabilityUtils.GetRandomInt(0, vector1.Length)];

            var childArray = new NonReapetingAdjacencyMatrix<T>(vector1, vector2, true).Crossover(firstElement, length);
            return new VectorChromosome<T>(childArray, mutationManager, evaluator);
        }
    }
}
