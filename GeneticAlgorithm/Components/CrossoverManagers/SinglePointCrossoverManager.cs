using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// A point on both parents' chromosomes is picked randomly, and designated a 'crossover point'. Bits to the right of that point are swapped between the two parent chromosomes.
    /// See: https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm)#Single-point_crossover
    /// </summary>
    public class SinglePointCrossoverManager<T> : ICrossoverManager
    {
        private readonly K_PointCrossoverManager<T> kPointCrossover;

        public SinglePointCrossoverManager(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            kPointCrossover = new K_PointCrossoverManager<T>(1, mutationManager, evaluator);
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2) =>
            kPointCrossover.Crossover(chromosome1, chromosome2);
    }
}
