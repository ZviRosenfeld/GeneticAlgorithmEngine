using System.Collections.Generic;
using System.Threading;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.StopManagers;

namespace GeneticAlgorithm
{
    public class GeneticSearchEngineBuilder
    {
        protected readonly ICrossoverManager crossoverManager;
        protected readonly IPopulationGenerator populationGenerator;
        protected readonly int populationSize;
        protected double mutationProbability = 0;
        protected bool includeAllHistory = false;
        protected double elitPercentage = 0;
        protected List<IStopManager> stopManagers = new List<IStopManager>();
        protected List<IPopulationRenwalManager> populationRenwalManagers = new List<IPopulationRenwalManager>();

        public GeneticSearchEngineBuilder(int populationSize, int maxGenerations, ICrossoverManager crossoverManager,
            IPopulationGenerator populationGenerator)
        {
            this.populationSize = populationSize;
            this.crossoverManager = crossoverManager;
            this.populationGenerator = populationGenerator;

            stopManagers.Add(new StopAtGeneration(maxGenerations));
        }

        public GeneticSearchEngineBuilder SetMutationProbability(double probability)
        {
            mutationProbability = probability;
            return this;
        }

        /// <summary>
        /// The "percentage" of the best chromosome will be passed "as is" to the next generation
        /// </summary>
        public GeneticSearchEngineBuilder SetElitPercentage(double percentage)
        {
            elitPercentage = percentage;
            return this;
        }

        /// <summary>
        /// The GeneticSearchResult will include the entire history of the population
        /// </summary>
        public GeneticSearchEngineBuilder IncludeAllHistory()
        {
            includeAllHistory = true;
            return this;
        }

        public GeneticSearchEngineBuilder AddStopManager(IStopManager manager)
        {
            stopManagers.Add(manager);
            return this;
        }

        public GeneticSearchEngineBuilder SetCancellationToken(CancellationToken cancellationToken)
        {
            stopManagers.Add(new StopWithCancellationToken(cancellationToken));
            return this;
        }

        public GeneticSearchEngineBuilder AddPopulationRenwalManager(IPopulationRenwalManager manager)
        {
            populationRenwalManagers.Add(manager);
            return this;
        }
        
        public virtual GeneticSearchEngine Build()
        {
            var options = new GeneticSearchOptions(populationSize,
                mutationProbability, stopManagers, includeAllHistory, populationRenwalManagers, elitPercentage);
            var childrenGenerator = new ChildrenGenerator(options, crossoverManager);
            return new GeneticSearchEngine(options, populationGenerator, childrenGenerator);
        }
    }
}
