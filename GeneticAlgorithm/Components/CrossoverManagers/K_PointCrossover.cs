using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// K crossover points are picked randomly from the parent chromosomes. The bits in between these points are swapped between the parent organisms.
    /// See: https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm)#Two-point_and_k-point_crossover
    /// </summary>
    public class K_PointCrossover<T> : ICrossoverManager
    {
        private readonly int k;
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        public K_PointCrossover(int k, IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.k = k;
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var shortVectorChromosome = (VectorChromosome<T>)chromosome1;
            var longVectorChromosome = (VectorChromosome<T>)chromosome2;
            if (shortVectorChromosome.GetVector().Length > longVectorChromosome.GetVector().Length)
            {
                var tempChromosome = longVectorChromosome;
                longVectorChromosome = shortVectorChromosome;
                shortVectorChromosome = tempChromosome;
            }

            var crossoverPionts = ChooseCrossoverPoints(k, shortVectorChromosome.GetVector().Length);

            var takingFromShortChromosome = true;
            var newVector = new T[longVectorChromosome.GetVector().Length];
            var index = 0;
            for (; index < shortVectorChromosome.GetVector().Length; index++)
            {
                newVector[index] = takingFromShortChromosome
                    ? shortVectorChromosome.GetVector()[index]
                    : longVectorChromosome.GetVector()[index];
                if (crossoverPionts.Contains(index))
                    takingFromShortChromosome = !takingFromShortChromosome;
            }
            for (; index < longVectorChromosome.GetVector().Length; index++)
                newVector[index] = longVectorChromosome.GetVector()[index];

            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }

        private List<int> ChooseCrossoverPoints(int k, int length)
        {
            var crossoverPoints = k < length ? k : length - 1;
            return ProbabilityUtils.SelectKRandomNumbers(length - 1, crossoverPoints);
        }
    }
}
