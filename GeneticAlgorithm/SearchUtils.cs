﻿using System;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public static class SearchUtils
    {
        private static readonly Random random = new Random();

        public static IChromosome ChooseBest(this IChromosome[] population)
        {
            double bestEvaluation = -1;
            IChromosome bestChromosome = null;
            foreach (var chromosome in population)
            {
                var evaluation = chromosome.Evaluate();
                if (evaluation > bestEvaluation)
                {
                    bestEvaluation = evaluation;
                    bestChromosome = chromosome;
                }
            }

            return bestChromosome;
        }

        public static IChromosome ChooseParent(IChromosome[] population, double[] evaluations)
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
