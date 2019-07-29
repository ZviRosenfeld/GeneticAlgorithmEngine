using System;
using GeneticAlgorithm.Interfaces;

namespace GreatestVectorTests
{
    public class NumberVectorCrossoverManager : ICrossoverManager
    {
        private readonly Random random = new Random();

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var numberChromosome1 = (NumberVectorChromosome) chromosome1;
            var numberChromosome2 = (NumberVectorChromosome) chromosome2;

            var splitLocation = random.Next(0, numberChromosome1.VectorSize + 1);
            var newVector = new int[numberChromosome1.VectorSize];
            for (int i = 0; i < numberChromosome1.VectorSize; i++)
                newVector[i] = i < splitLocation ? numberChromosome1[i] : numberChromosome2[i];

            return new NumberVectorChromosome(newVector);
        }
    }
}
