using System;
using GeneticAlgorithm.Interfaces;

namespace Environment
{
    class MyChromosome : IChromosome
    {
        public ChromosomeType Type { get; private set; }

        public MyChromosome(ChromosomeType type)
        {
            Type = type;
        }

        public double Evaluate()
        {
            // We don't need to implement this. Evaluations will be handled by our custom ChromosomeEvaluator.
            throw new NotImplementedException();
        }

        /// <summary>
        /// A mutation will change the type of a chromosome
        /// </summary>
        public void Mutate()
        {
            Type = Type == ChromosomeType.OProducer ? ChromosomeType.Oc2Producer : ChromosomeType.OProducer;
        }

        public override string ToString() => Type.ToString();
    }
}
