using System;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    class InternalSearchResult
    {
        public Population Population { get; }

        public TimeSpan SearchTime { get; }

        public bool IsCompleted { get; }
        
        public InternalSearchResult(Population population, TimeSpan searchTime, bool isCompleted)
        {
            SearchTime = searchTime;
            IsCompleted = isCompleted;
            Population = population;
        }
    }
}
