using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.Chromosomes
{
    public class VectorChromosome : IVectorChromosome
    {
        private int[] vector;
        private readonly IMutationManager mutationManager;
        private readonly IEvaluator evaluator;

        public VectorChromosome(int[] vector, IMutationManager mutationManager, IEvaluator evaluator)
        {
            this.vector = vector;
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public double Evaluate() => evaluator.Evaluate(this);

        public void Mutate() => vector = mutationManager.Mutate(vector);

        public int[] GetVector() => vector;
    }
}
