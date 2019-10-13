using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.Chromosomes
{
    public class VectorChromosome<T> : IChromosome
    {
        private T[] vector;
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        public VectorChromosome(T[] vector, IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.vector = vector;
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public double Evaluate() => evaluator.Evaluate(this);

        public void Mutate() => vector = mutationManager.Mutate(vector);

        public T[] GetVector() => vector;
    }
}
