using System;
using System.Linq;
using System.Text;
using GeneticAlgorithm.Interfaces;

namespace GreatestVectorTests
{
    class NumberVectorChromosome : IChromosome
    {
        private readonly int[] vector;

        public NumberVectorChromosome(int[] vector)
        {
            this.vector = vector;
        }

        public double Evaluate() => vector.Sum();

        public void Mutate()
        {
            var random = new Random();
            for (int i = 0; i < vector.Length; i++)
                if (random.NextDouble() < 0.1)
                    vector[i] += random.Next(0, 10);
        }

        public int this[int i] => vector[i];

        public int VectorSize => vector.Length;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var value in vector)
                stringBuilder.Append(value + " ");

            return stringBuilder.ToString();
        }
    }
}
