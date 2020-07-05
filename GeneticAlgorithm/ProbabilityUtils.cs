using System;
using System.Collections.Generic;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm
{
    public static class ProbabilityUtils
    {
        private static Random random = new Random();
        private static object randomLockObject = new object();

        /// <summary>
        /// Returens a random double between 0 and 1.
        /// </summary>
        public static double GetRandomDouble()
        {
            // We need this lock, since random isn't thread-safe
            lock (randomLockObject)
                return random.NextDouble();
        }

        /// <summary>
        /// Returens a random double between min (inclusive) and max (exclusive).
        /// </summary>
        public static int GetRandomInt(int min, int max)
        {
            // We need this lock, since random isn't thread-safe
            lock (randomLockObject)
                return random.Next(min, max);
        }

        /// <summary>
        /// Returens a random double between 0 (inclusive) and max (exclusive).
        /// </summary>
        public static int GetRandomInt(int max)
        {
            // We need this lock, since random isn't thread-safe
            lock (randomLockObject)
                return random.Next(max);
        }

        /// <summary>
        /// Returns true with a probability of probability - where probability is between 0 to 1 (including)
        /// </summary>
        public static bool P(double probability)
        {
            if (probability > 1 || probability < 0)
                throw new InternalSearchException($"Code 1008 (probability is {probability})");

            return GetRandomDouble() < probability;
        }

        /// <summary>
        /// Selects K random indexes between 0 and 'till' not repeating.
        /// </summary>
        public static List<int> SelectKRandomNumbersNonRepeating(int till, int k)
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
        /// Selects K random elements from the collection not repeating.
        /// </summary>
        public static T[] SelectKRandomElementsNonRepeating<T>(this T[] collection, int k)
        {
            var selectedIndexes = new T[k];
            for (int i = 0, added = 0; i < collection.Length; i++)
            {
                if (P((k - added) / (double)(collection.Length - i)))
                {
                    selectedIndexes[added] = collection[i];
                    added++;
                    if (k == added) break;
                }
            }
            return selectedIndexes;
        }

        /// <summary>
        /// returns a random number with a Gaussian distribution.
        /// </summary>
        public static double GaussianDistribution(double sd, double mean)
        {
            return mean + sd * GetStandardDistribution();
        }

        /// <summary>
        /// Returns random numbers with a standard distribution using Box-Muller Transformation.
        /// </summary>
        private static double GetStandardDistribution()
        {
            var u1 = 1.0 - GetRandomDouble();
            var u2 = 1.0 - GetRandomDouble();
            return Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        }
    }
}
