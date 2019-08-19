using System;
using GeneticAlgorithm.Interfaces;

namespace Environment
{
    class CrossoverManager : ICrossoverManager
    {
        private readonly Random random = new Random();

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var type1 = ((MyChromosome) chromosome1).Type;
            var type2 = ((MyChromosome) chromosome2).Type;

            if (type1 == ChromosomeType.OProducer && type2 == ChromosomeType.OProducer)
                return new MyChromosome(ChromosomeType.OProducer);
            if (type1 == ChromosomeType.Oc2Producer && type2 == ChromosomeType.Oc2Producer)
                return new MyChromosome(ChromosomeType.Oc2Producer);

            return new MyChromosome(random.NextDouble() < 0.5 ? ChromosomeType.OProducer : ChromosomeType.Oc2Producer);
        }
    }
}
