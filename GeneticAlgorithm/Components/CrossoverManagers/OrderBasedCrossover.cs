using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// OrderBasedCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, OrderBasedCrossover may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// In OrderBasedCrossover, several positions are selected at random from the second parent. 
    /// Let A be the list of elements at the selected indexes in parent2.
    /// All elements that aren't in A are copied as is from parent1.
    /// The missing elements are added in the order in which they appear in parent2
    /// 
    /// In OrderBasedCrossover, the child is guaranteed to contain each genome exactly once.
    /// </summary>
    public class OrderBasedCrossover<T> : ICrossoverManager
    {
        private readonly Random random = new Random();
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// OrderBasedCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, OrderBasedCrossover may throw an exception.
        ///
        /// Also, the Equals method must be implemented for type T.
        ///  </summary>
        public OrderBasedCrossover(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vector1 = ((VectorChromosome<T>)chromosome1).GetVector();
            var vector2 = ((VectorChromosome<T>)chromosome2).GetVector();
            var length = vector1.Length;
            var indexes = ProbabilityUtils.SelectKRandomNumbersNonRepeating(length, random.Next(length));

            var elementsFromParent2 = new HashSet<T>();
            var elementsFromParent2OrderedByIndex = new List<T>();
            foreach (var index in indexes)
            {
                elementsFromParent2.Add(vector2[index]);
                elementsFromParent2OrderedByIndex.Add(vector2[index]);
            }

            var newVector = new T[length];
            for (int index = 0, addedElementsFromParent2 = 0; index < length; index++)
            {
                var elementInParent1 = vector1[index];
                if (!elementsFromParent2.Contains(elementInParent1))
                    newVector[index] = elementInParent1;
                else
                {
                    newVector[index] = elementsFromParent2OrderedByIndex[addedElementsFromParent2];
                    addedElementsFromParent2++;
                }
            }

            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }
    }
}
