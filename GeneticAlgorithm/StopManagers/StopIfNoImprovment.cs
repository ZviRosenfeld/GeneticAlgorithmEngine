using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.StopManagers
{
    public class StopIfNoImprovment : IStopManager
    {
        private readonly int generations;
        private readonly double minImprovment;
        private readonly List<double> oldEvaluations = new List<double>();

        /// <summary>
        /// Stop if there isn't an improvement of at least "minImprovment" after "generations" generations
        /// </summary>
        public StopIfNoImprovment(int generations, double minImprovment)
        {
            this.generations = generations;
            this.minImprovment = minImprovment;
        }

        public bool ShouldStop(IChromosome[] population, double[] evaluations, int generation)
        {
            var currentEvaluation = evaluations.Max();
            if (generation < generations)
                return false;
            
            var min = oldEvaluations.Skip(generation - generations).Take(generations).Min();
            oldEvaluations.Add(currentEvaluation);

            return Math.Abs(currentEvaluation - min) <= minImprovment;
        }

        public void AddGeneration(IChromosome[] population, double[] evaluations)
        {
            oldEvaluations.Add(evaluations.Max());
        }
    }
}
