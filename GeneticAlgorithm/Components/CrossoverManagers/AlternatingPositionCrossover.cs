using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// AlternatingPositionCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, AlternatingPositionCrossover may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// In AlternatingPositionCrossover, we alternately select the next element of the first parent and the next element of the second parent,
    /// omitting the elements already present in the offspring. 
    /// This guarantees that the child contains each genome exactly once.
    /// </summary>
    public class AlternatingPositionCrossover<T> : ICrossoverManager
    {
        private readonly Random random = new Random();
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// AlternatingPositionCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, AlternatingPositionCrossover may throw an exception.
        ///
        /// Also, the Equals method must be implemented for type T.
        ///  </summary>
        public AlternatingPositionCrossover(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vector1 = ((VectorChromosome<T>)chromosome1).GetVector();
            var vector2 = ((VectorChromosome<T>)chromosome2).GetVector();
            var length = vector1.Length;

            var addedElements = new HashSet<T>();
            var newVector = new T[length];
            var indexToAddAt = 0;
            for (int i = 0; i < length; i++)
            {
                if (!addedElements.Contains(vector1[i]))
                {
                    newVector[indexToAddAt] = vector1[i];
                    addedElements.Add(vector1[i]);
                    indexToAddAt++;
                }
                if (!addedElements.Contains(vector2[i]))
                {
                    newVector[indexToAddAt] = vector2[i];
                    addedElements.Add(vector2[i]);
                    indexToAddAt++;
                }
            }

            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }
    }
}
