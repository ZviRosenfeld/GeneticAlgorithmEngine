using System;
using System.Collections.Generic;
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

        public ResultBuilder(bool includeHistory)
        {
            this.includeHistory = includeHistory;
        }

        public void AddGeneration(InternalSearchResult internalResult)
        {
            searchTime += internalResult.SearchTime;
            if (includeHistory)
                history.Add(internalResult.Population.GetChromosomes());
            isComplated = isComplated || internalResult.IsCompleted;
            lastGeneration = internalResult.Population;
        }

        public GeneticSearchResult Build(int generations)
        {
            if (lastGeneration == null)
                throw new InternalSearchException("Code 1001 (called build before adding any generations)");

            return new GeneticSearchResult(lastGeneration.ChooseBest(), lastGeneration.GetChromosomes(), history, searchTime, generations, isComplated);
        }
    }
}
