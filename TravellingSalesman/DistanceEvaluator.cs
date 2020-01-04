using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace TravellingSalesman
{
    class DistanceEvaluator : IEvaluator
    {
        private readonly DistanceCalclator distanceCalclator;
        private readonly double maxDistance;

        public DistanceEvaluator(IDictionary<string, Tuple<int, int>> locations)
        {
            distanceCalclator = new DistanceCalclator(locations);
            maxDistance = 1500 * locations.Count;
        }

        public double Evaluate(IChromosome chromosome) => 
            maxDistance - distanceCalclator.GetDistance(chromosome);
    }
}
