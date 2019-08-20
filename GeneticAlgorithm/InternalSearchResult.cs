using System;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    class InternalSearchResult
    {
        public Population Population { get; }

        public TimeSpan SearchTime { get; }

        public bool IsCompleted { get; }

        public IEnvironment Environment { get; }

        public InternalSearchResult(Population population, TimeSpan searchTime, bool isCompleted, IEnvironment environment)
        {
            SearchTime = searchTime;
            IsCompleted = isCompleted;
            Environment = environment;
            Population = population;
        }
    }
}
