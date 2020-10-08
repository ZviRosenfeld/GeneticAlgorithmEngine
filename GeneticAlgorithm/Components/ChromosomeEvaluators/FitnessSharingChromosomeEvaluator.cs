using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
using System;

namespace GeneticAlgorithm.Components.ChromosomeEvaluators
{
    /// <summary>
    /// A ChromosomeEvaluator that implements fitness sharing (a technique to prevent premature conversion in which part of a chromosome's fitness depends on it's similarity to the rest of the population).
    /// </summary>
    public class FitnessSharingChromosomeEvaluator : IChromosomeEvaluator
    {
        private readonly double minDistance;
        private readonly double distanceScale;
        private readonly Func<IChromosome, IChromosome, double> distanceCalculator;
        private IChromosome[] population;

        /// <summary>
        /// A ChromosomeEvaluator that implements fitness sharing.
        /// Please note that FitnessSharingChromosomeEvaluator expects to get an environment of type DefaultEnvironment, so don't set a different environment when using it.
        /// </summary>
        /// <param name="minDistance">Only distances smaller than minDistance will be considered when calculating the distance.</param>
        /// <param name="distanceScale">A parameter that defines how much influence sharing has. Higher = more sharing.</param>
        /// <param name="distanceCalculator">A function that calculates the distance between 2 chromosomes. Note that the distance must be between 0 to 1 (not including).</param>
        public FitnessSharingChromosomeEvaluator(double minDistance, double distanceScale, Func<IChromosome, IChromosome, double> distanceCalculator)
        {
            if (minDistance < 0 || minDistance > 1)
                throw new GeneticAlgorithmArgumentException($"{nameof(minDistance)} must be greater than 0 and smaller than 1 (including)");
            if (distanceScale < 1)
                throw new GeneticAlgorithmArgumentException($"{nameof(distanceScale)} must be greater or equalt to 1");

            this.minDistance = minDistance;
            this.distanceScale = distanceScale;
            this.distanceCalculator = distanceCalculator ?? throw new GeneticAlgorithmNullArgumentException(nameof(distanceCalculator));
        }

        public double Evaluate(IChromosome chromosome)
        {
            var fitness = chromosome.Evaluate();
            var distance = Distance(chromosome);
            return fitness / distance;
        }

        private double Distance(IChromosome chromosome)
        {
            var sum = 0.0;
            foreach (var c in population)
            {
                var distance = distanceCalculator(chromosome, c);
                if (distance < 0 || distance > 1)
                    throw new GeneticAlgorithmArgumentException($"the {nameof(distance)} returned by {nameof(distanceCalculator)} in {nameof(FitnessSharingChromosomeEvaluator)} must be greater than 0 and smaller than 1 (including).");

                if (distance < minDistance)
                    sum += 1 - distance / distanceScale;
            }

            return sum;
        }

        public void SetEnvierment(IEnvironment envierment)
        {
            if (!(envierment is DefaultEnvironment defaultEnvironment))
                throw new GeneticAlgorithmException($"{nameof(FitnessSharingChromosomeEvaluator)} only works when the envierment is of type {nameof(DefaultEnvironment)}");

            population = defaultEnvironment.Chromosomes;
        }
    }
}
