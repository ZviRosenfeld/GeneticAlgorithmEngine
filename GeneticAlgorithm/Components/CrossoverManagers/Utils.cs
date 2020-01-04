using System;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    static class Utils
    {
        /// <summary>
        /// Returns the shroter chromorose as the first, and the longer chromosome second
        /// </summary>
        public static (VectorChromosome<T>, VectorChromosome<T>) OrderChromosomes<T>(IChromosome chromosome1,
            IChromosome chromosome2)
        {
            var vectorChromosome1 = (VectorChromosome<T>) chromosome1;
            var vectorChromosome2 = (VectorChromosome<T>) chromosome2;
            return vectorChromosome1.GetVector().Length <= vectorChromosome2.GetVector().Length
                ? (vectorChromosome1, vectorChromosome2)
                : (vectorChromosome2, vectorChromosome1);
        }
        
        public static (int, int) GetTwoRandomNumbers(this Random random, int max)
        {
            var num1 = random.Next(max);
            var num2 = random.Next(max);

            return (Math.Min(num1, num2), Math.Max(num1, num2));
        }
    }
}
