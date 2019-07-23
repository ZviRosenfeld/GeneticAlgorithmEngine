using System;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class ParentChooser
    {
        private readonly Random random = new Random();

        public IChromosome GetParanet(IChromosome[] population, double[] evaluations)
        {
            var randomNumber = random.NextDouble();
            var sum = 0.0;
            var index = -1;
            while (sum < randomNumber)
            {
                index++;
                sum += evaluations[index];
            }

            return population[index];
        }
    }
}
