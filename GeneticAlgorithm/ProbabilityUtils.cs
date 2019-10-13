using System;
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
    }
}
