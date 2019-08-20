using System;
using GeneticAlgorithm.Interfaces;

namespace Environment
{
    class MyChromosome : IChromosome
    {
        private readonly Random random = new Random();

        public ChromosomeType Type { get; private set; }

        public MyChromosome(ChromosomeType type)
        {
            Type = type;
        }

        public double Evaluate()
        {
            throw new NotImplementedException();
        }

        public void Mutate()
        {
            if (random.NextDouble() < 0.1)
                Type = Type == ChromosomeType.OProducer ? ChromosomeType.Oc2Producer : ChromosomeType.OProducer;

        }

        public override string ToString() => Type.ToString();
    }
}
