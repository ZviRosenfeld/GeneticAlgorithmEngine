using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.Chromosomes
{
    public class VectorChromosome : IVectorChromosome
    {
        private readonly int minValue;
        private readonly int maxValue;
        private readonly int[] vector;

        public VectorChromosome(int[] vector, int minValue, int maxValue)
        {
            this.vector = vector;
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public double Evaluate()
        {
            return 0;
        }

        public virtual void Mutate()
        {
            
        }

        public int[] GetVector() => vector;
    }
}
