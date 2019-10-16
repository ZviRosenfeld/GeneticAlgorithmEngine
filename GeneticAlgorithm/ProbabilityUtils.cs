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

        /// <summary>
        /// returns a random number with a Gaussian distribution.
        /// </summary>
        public static double GaussianDistribution(double variance, double mean)
        {
            var t = GetStandardDistribution();
            return mean + variance * t;// GetStandardDistribution();
        }

        /// <summary>
        /// Returns random numbers with a standard distribution using Box-Muller Transformation.
        /// </summary>
        private static double GetStandardDistribution()
        {
            double u1 = 1.0 - random.NextDouble();
            double u2 = 1.0 - random.NextDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }
    }
}
