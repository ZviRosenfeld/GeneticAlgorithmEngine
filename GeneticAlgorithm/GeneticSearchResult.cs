using System;
using System.Collections.Generic;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchResult
    {
        public GeneticSearchResult(IChromosome bestChromosome, Population population, List<IChromosome[]> history,
            TimeSpan searchTime, int generations, bool isCompleted, IEnvironment environment)
        {
            Population = population;
            History = history;
            SearchTime = searchTime;
            Generations = generations;
            IsCompleted = isCompleted;
            Environment = environment;
            BestChromosome = bestChromosome;
        }

        public Population Population { get; }

        public IChromosome BestChromosome { get; }

        public int Generations { get; }

        /// <summary>
        /// Will only have a value if IncludeAllHistory is set to true
        /// </summary>
        public List<IChromosome[]> History { get; }
        
        public TimeSpan SearchTime { get; }
        
        public bool IsCompleted { get; }

        public IEnvironment Environment { get; }
    }
}
