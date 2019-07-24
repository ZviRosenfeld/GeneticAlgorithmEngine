using System;
using System.Text;
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

        public static IChromosome[] Combine(IChromosome[] chromosomes1, IChromosome[] chromosomes2)
        {
            var firstChromosomesLength = chromosomes1.Length;
            var newChromosomes = new IChromosome[firstChromosomesLength + chromosomes2.Length];
            var i = 0;
            for (; i < firstChromosomesLength; i++)
                newChromosomes[i] = chromosomes1[i];
            for (; i < firstChromosomesLength + chromosomes2.Length; i++)
                newChromosomes[i] = chromosomes2[i - firstChromosomesLength];

            return newChromosomes;
        }

        public static string MyToString(this IChromosome[] chromosomes)
        {
            var stringBuilder = new StringBuilder();
            foreach (var chromosome in chromosomes)
                stringBuilder.AppendLine(chromosome + ", ");
            return stringBuilder.ToString();
        }
    }
}
