using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public static class SearchUtils
    {
        public static IChromosome ChooseBest(this Population population)
        {
            double bestEvaluation = -1;
            IChromosome bestChromosome = null;
            foreach (var chromosome in population)
            {
                var evaluation = chromosome.Evaluation;
                if (evaluation > bestEvaluation)
                {
                    bestEvaluation = evaluation;
                    bestChromosome = chromosome.Chromosome;
                }
            }

            return bestChromosome;
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
        
        public static Population Clone(this Population population)
        {
            var newPopulation = new Population(population.GetChromosomes().ToArray());
            for (int i = 0; i < population.Count(); i++)
                newPopulation[i].Evaluation = population[i].Evaluation;

            return newPopulation;
        }

        /// <summary>
        /// Clips value if it is smaller than min or greater than max.
        /// </summary>
        public static double Clip(this double value, double min, double max)
        {
            if (value > max) return max;
            if (value < min) return min;
            return value;
        }
        
        public static T[] Shuffle<T>(this ICollection<T> collection, Random random)
        {
            var array = collection.ToArray();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }

            return array;
        }
    }
}
