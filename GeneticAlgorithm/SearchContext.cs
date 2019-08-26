using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    class SearchContext
    {
        private readonly bool includeHistory;

        private TimeSpan searchTime = TimeSpan.Zero;
        private List<IChromosome[]> history = new List<IChromosome[]>();
        private bool isComplated = false;

        public  Population LastGeneration { get; private set; }

        public IEnvironment Environment { get; }

        public int Generation { get; private set; }

        public SearchContext(bool includeHistory, IEnvironment environment)
        {
            this.includeHistory = includeHistory;
            Environment = environment;
        }

        public void AddGeneration(InternalSearchResult internalResult)
        {
            Generation++;
            searchTime += internalResult.SearchTime;
            if (includeHistory)
                history.Add(internalResult.Population.GetChromosomes().ToArray());
            isComplated = isComplated || internalResult.IsCompleted;
            LastGeneration = internalResult.Population;
        }

        public GeneticSearchResult BuildResult()
        {
            if (LastGeneration == null)
                throw new InternalSearchException("Code 1001 (called build before adding any generations)");

            return new GeneticSearchResult(LastGeneration.ChooseBest(), LastGeneration.Clone(), history, searchTime, Generation, isComplated, Environment);
        }
    }
}
