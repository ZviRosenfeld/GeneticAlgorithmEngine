using System;
using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Interfaces;

namespace TravellingSalesman
{
    class DistanceCalclator
    {
        private readonly IDictionary<string, Tuple<int, int>> locations;

        public DistanceCalclator(IDictionary<string, Tuple<int, int>> locations)
        {
            this.locations = locations;
        }

        public double GetDistance(IChromosome chromosome)
        {
            var totalDistance = 0.0;
            var cityVector = ((VectorChromosome<string>)chromosome).GetVector();
            for (int i = 0; i < cityVector.Length - 1; i++)
                totalDistance += GetDistance(cityVector[i], cityVector[i + 1]);

            return totalDistance;
        }

        private double GetDistance(string city1, string city2) =>
            Math.Sqrt(Math.Pow(locations[city1].Item1 - locations[city2].Item1, 2) + Math.Pow(locations[city1].Item2 - locations[city2].Item2, 2));
    }
}
