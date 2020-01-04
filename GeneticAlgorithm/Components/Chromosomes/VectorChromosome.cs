using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.Chromosomes
{
    public class VectorChromosome<T> : IChromosome, IEnumerable<T>
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

        public T this[int index] => vector[index];

        public int Length => vector.Length;

        public IEnumerator<T> GetEnumerator() => vector.Cast<T>().GetEnumerator();

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var value in vector)
                stringBuilder.Append(value + ", ");
            
            return stringBuilder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
