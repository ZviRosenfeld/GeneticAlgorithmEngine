using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// PositionBasedCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, PositionBasedCrossover may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// In PositionBasedCrossover, several positions are selected at random from the first parent. 
    /// The genomes in those positions are copied as-is from the first parent.
    /// The rest of the genomes are coped from the second parent in order as long as the genome hasn't already been copied from parent1.
    /// 
    /// In PositionBasedCrossover, the child is guaranteed to contain each genome exactly once.
    /// </summary>
    public class PositionBasedCrossoverManager<T> : ICrossoverManager
    {
        private readonly Random random = new Random();
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// PositionBasedCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, OrderBasedCrossover may throw an exception.
        ///
        /// Also, the Equals method must be implemented for type T.
        ///  </summary>
        public PositionBasedCrossoverManager(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vector1 = ((VectorChromosome<T>)chromosome1).GetVector();
            var vector2 = ((VectorChromosome<T>)chromosome2).GetVector();
            var length = vector1.Length;
            var indexesToTakeFromParent1 = ProbabilityUtils.SelectKRandomNumbers(length, random.Next(length));
            
            var genomesFromChromosome1 = new HashSet<T>();
            var newVector = new T[length];
            foreach (var index in indexesToTakeFromParent1)
            {
                newVector[index] = vector1[index];
                genomesFromChromosome1.Add(vector1[index]);
            }

            int lastAddedIndexFromChromsome2 = 0;
            for (int i = 0; i < length; i++)
            {
                if (indexesToTakeFromParent1.Contains(i)) continue;
                while (genomesFromChromosome1.Contains(vector2[lastAddedIndexFromChromsome2]))
                    lastAddedIndexFromChromsome2++;

                newVector[i] = vector2[lastAddedIndexFromChromsome2];
                lastAddedIndexFromChromsome2++;
            }

            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }
    }
}
