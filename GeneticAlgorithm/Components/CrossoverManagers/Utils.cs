using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    static class Utils
    {
        /// <summary>
        /// Returns the shorter chromosome as the first, and the longer chromosome second
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
    }
}
