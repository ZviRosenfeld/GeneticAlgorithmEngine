using System;
using GeneticAlgorithm.Interfaces;

namespace Environment
{
    class MyEnvironment : IEnvironment
    {
        public void UpdateEnvierment(IChromosome[] chromosomes, int generation)
        {
            O = 0;
            OC2 = 0;

            foreach (var chromosome in chromosomes)
            {
                var myChromosome = (MyChromosome) chromosome;
                if (myChromosome.Type == ChromosomeType.OProducer)
                {
                    O += 2;
                    OC2 -= 1;
                }
                else
                {
                    OC2 += 2;
                    O -= 1;
                }
            }

            O = Math.Max(0, O);
            OC2 = Math.Max(0, OC2);
        }

        public int O { get; private set; }

        public int OC2 { get; private set; }

        public override string ToString() => $"O: {O}; OC2 {OC2}";
    }
}
