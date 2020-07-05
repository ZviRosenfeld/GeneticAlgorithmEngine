using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers.Utilities;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// PartiallyMatchedCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, PartiallyMatchedCrossover may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// PartiallyMatchedCrossover guarantees that if no genome was repeated in the parents, no genome will be repeated in the child either.
    /// 
    /// See: http://www.rubicite.com/Tutorials/GeneticAlgorithms/CrossoverOperators/PMXCrossoverOperator.aspx
    /// </summary>
    public class PartiallyMappedCrossover<T> : ICrossoverManager
    {
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// PartiallyMatchedCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, PartiallyMatchedCrossover may throw an exception.
        /// 
        /// Also, the Equals method must be implemented for type T.
        /// </summary>
        public PartiallyMappedCrossover(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vector1 = ((VectorChromosome<T>)chromosome1).GetVector();
            var vector2 = ((VectorChromosome<T>)chromosome2).GetVector();
            var indexManager = new IndexManager<T>(vector2);
            var length = vector1.Length;

            (var start, var end) = ComponetsUtils.GetTwoRandomNumbers(length + 1);
            var addedIndexes = new List<int>();
            var genomesFromChromosome1 = new List<T>();
            for (int i = start; i < end; i++)
                genomesFromChromosome1.Add(vector1[i]);
            
            T[] newVector = new T[length];
            for (int i = start; i < end; i++)
            {
                addedIndexes.Add(i);
                newVector[i] = vector1[i];
            }
            for (int i = start; i < end; i++)
            {
                if (genomesFromChromosome1.Contains(vector2[i])) continue;
                var valueToReplace = vector1[i];
                var index = indexManager.GetIndex(valueToReplace);
                addedIndexes.Add(index);
                newVector[index] = vector2[i];
            }
            for (int i = 0; i < vector1.Length; i++)
                if (!addedIndexes.Contains(i))
                    newVector[i] = vector2[i];
            
            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }
    }
}
