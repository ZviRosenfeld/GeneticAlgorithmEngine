using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.StopManagers
{
    public class StopIfNoImprovment : IStopManager
    {
        private readonly int generationsToConsider;
        private readonly double minImprovment;
        private readonly List<double> oldEvaluations = new List<double>();

        /// <summary>
        /// Stop if there isn't an improvement of at least "minImprovment" after "generationsToConsider" generations
        /// </summary>
        public StopIfNoImprovment(int generationsToConsider, double minImprovment)
        {
            this.generationsToConsider = generationsToConsider;
            this.minImprovment = minImprovment;
        }

        public bool ShouldStop(Population population, IEnvironment environment, int generation)
        {
            var currentEvaluation = population.GetEvaluations().Max();
            if (oldEvaluations.Count < generationsToConsider)
                return false;
            
            var min = oldEvaluations.Skip(generation - generationsToConsider).Take(generationsToConsider).Min();
            
            return Math.Abs(currentEvaluation - min) <= minImprovment;
        }

        public void AddGeneration(Population population)
        {
            oldEvaluations.Add(population.GetEvaluations().Max());
        }
    }
}
