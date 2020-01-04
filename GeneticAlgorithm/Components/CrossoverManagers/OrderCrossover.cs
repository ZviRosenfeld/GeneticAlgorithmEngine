using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// Ordered crossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, OrderCrossover may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// In ordered crossover, we randomly select a subset of the first parent string
    /// and then fill the remainder of the route with the genes from the second parent in the order in which they appear.
    /// This insures that if no genome was repeated in the parents, no genome will be repeated in the child either.
    /// 
    /// See: https://stackoverflow.com/questions/26518393/order-crossover-ox-genetic-algorithm/26521576
    /// </summary>
    public class OrderCrossover<T> : ICrossoverManager
    {
        private readonly Random random = new Random();
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// Ordered crossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, OrderCrossover may throw an exception.
        ///
        /// Also, the Equals method must be implemented for type T.
        ///  </summary>
        public OrderCrossover(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vectorChromosome1 = (VectorChromosome<T>) chromosome1;
            var vectorChromosome2 = (VectorChromosome<T>) chromosome2;
            var length = vectorChromosome1.Length;

            (var start, var end) = random.GetTwoRandomNumbers(length + 1);
            var genomesFromChromosome1 = new List<T>();
            for (int i = start; i < end; i++)
                genomesFromChromosome1.Add(vectorChromosome1[i]);
            
            T[] newVector = new T[length];
            int lastAddedIndexFromChromsome2 = 0;
            for (int i = 0; i < start; i++)
            {
                while (genomesFromChromosome1.Contains(vectorChromosome2[lastAddedIndexFromChromsome2]))
                    lastAddedIndexFromChromsome2++;

                newVector[i] = vectorChromosome2[lastAddedIndexFromChromsome2];
                lastAddedIndexFromChromsome2++;
            }
            for (int i = start; i < end; i++)
                newVector[i] = vectorChromosome1[i];

            for (int i = end; i < length; i++)
            {
                while (genomesFromChromosome1.Contains(vectorChromosome2[lastAddedIndexFromChromsome2]))
                    lastAddedIndexFromChromsome2++;

                newVector[i] = vectorChromosome2[lastAddedIndexFromChromsome2];
                lastAddedIndexFromChromsome2++;
            }

            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }
    }
}
