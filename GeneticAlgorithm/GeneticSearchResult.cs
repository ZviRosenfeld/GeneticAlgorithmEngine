using System;
using System.Collections.Generic;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchResult
    {
        public GeneticSearchResult(IChromosome[] population, List<IChromosome[]> history, TimeSpan searchTime, int generations)
        {
            Population = population;
            History = history;
            SearchTime = searchTime;
            Generations = generations;
            BestChromosome = population.ChooseBest();
        }

        public IChromosome[] Population { get; }

        public IChromosome BestChromosome { get; }

        public int Generations { get; }

        /// <summary>
        /// Will only have a value if IncludeAllHistory is set to true
        /// </summary>
        public List<IChromosome[]> History { get; }

        public TimeSpan SearchTime { get; }
    }
}
