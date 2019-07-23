using System.Collections.Generic;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchEngineBuilder
    {
        protected readonly ICrossoverManager crossoverManager;
        protected readonly IPopulationGenerator populationGenerator;
        protected readonly int populationSize;
        protected readonly int maxGenerations;
        protected double mutationProbability = 0;
        protected bool includeAllHistory = false;
        protected List<IStopManager> stopManagers = new List<IStopManager>();
        protected List<IPopulationRenwalManager> populationRenwalManagers = new List<IPopulationRenwalManager>();

        public GeneticSearchEngineBuilder(int populationSize, int maxGenerations, ICrossoverManager crossoverManager,
            IPopulationGenerator populationGenerator)
        {
            this.populationSize = populationSize;
            this.maxGenerations = maxGenerations;
            this.crossoverManager = crossoverManager;
            this.populationGenerator = populationGenerator;
        }

        public GeneticSearchEngineBuilder SetMutationProbability(double probability)
        {
            mutationProbability = probability;
            return this;
        }

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

        public GeneticSearchEngineBuilder AddPopulationRenwalManager(IPopulationRenwalManager manager)
        {
            populationRenwalManagers.Add(manager);
            return this;
        }
        
        public virtual GeneticSearchEngine Build()
        {
            var options = new GeneticSearchOptions(populationSize, maxGenerations,
                mutationProbability, stopManagers, includeAllHistory, populationRenwalManagers);
            var childrenGenerator = new ChildrenGenerator(options, crossoverManager);
            return new GeneticSearchEngine(options, populationGenerator, childrenGenerator);
        }
    }
}
