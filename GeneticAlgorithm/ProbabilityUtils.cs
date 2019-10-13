using System;
using System.Collections.Generic;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm
{
    public static class ProbabilityUtils
    {
        private static readonly Random random = new Random();

        /// <summary>
        /// Returns true with a probability of probability - where probability is between
        /// </summary>
        public static bool P(double probability)
        {
            if (probability > 1 || probability < 0)
                throw new InternalSearchException($"Code 1008 (probability is {probability})");

            return random.NextDouble() < probability;
        }

        /// <summary>
        /// Selects K random indexes between 0 and 'till'
        /// </summary>
        public static List<int> SelectKRandomNumbers(int till, int k)
        {
            var selectedIndexes = new List<int>();
            for (int i = 0; i < till; i++)
            {
                if (P(k / (double) (till - i)))
                {
                    selectedIndexes.Add(i);
                    k--;
                    if (k == 0) break;
                }
            }
            return selectedIndexes;
        }
    }
}
