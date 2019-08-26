using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    class ResultBuilder
    {
        private readonly bool includeHistory;

        private TimeSpan searchTime = TimeSpan.Zero;
        private List<IChromosome[]> history = new List<IChromosome[]>();
        private bool isComplated = false;
        private Population lastGeneration = null;
        private IEnvironment environment = null;

        public int Generation { get; private set; }

        public ResultBuilder(bool includeHistory)
        {
            this.includeHistory = includeHistory;
        }

        public void AddGeneration(InternalSearchResult internalResult)
        {
            Generation++;
            searchTime += internalResult.SearchTime;
            if (includeHistory)
                history.Add(internalResult.Population.GetChromosomes().ToArray());
            isComplated = isComplated || internalResult.IsCompleted;
            lastGeneration = internalResult.Population;
            environment = internalResult.Environment;
        }

        public GeneticSearchResult Build()
        {
            if (lastGeneration == null)
                throw new InternalSearchException("Code 1001 (called build before adding any generations)");

            return new GeneticSearchResult(lastGeneration.ChooseBest(), lastGeneration.Clone(), history, searchTime, Generation, isComplated, environment);
        }
    }
}
