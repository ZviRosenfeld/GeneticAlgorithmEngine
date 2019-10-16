using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// In uniform crossover, each bit is chosen from either parent with equal probability.
    /// </summary>
    public class UniformCrossoverManager<T> : ICrossoverManager
    {
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        public UniformCrossoverManager(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            (var shortVectorChromosome, var longVectorChromosome) =
                Utils.OrderChromosomes<T>(chromosome1, chromosome2);

            var newVector = new T[longVectorChromosome.GetVector().Length];
            var index = 0;
            for (; index < shortVectorChromosome.GetVector().Length; index++)
            {
                newVector[index] = ProbabilityUtils.P(0.5)
                    ? shortVectorChromosome.GetVector()[index]
                    : longVectorChromosome.GetVector()[index];
            }
            for (; index < longVectorChromosome.GetVector().Length; index++)
                newVector[index] = longVectorChromosome.GetVector()[index];

            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }
    }
}
