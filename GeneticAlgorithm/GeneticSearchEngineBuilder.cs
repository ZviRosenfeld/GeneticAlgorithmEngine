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
        protected IMutationManager mutationManager = new BassicMutationManager(0); // By default, mutation probability is 0
        protected IChromosomeEvaluator chromosomeEvaluator = new BassicChromosomeEvaluator();
        protected IEnvironment environment = null;
        protected IPopulationConverter populationConverter = null;
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
            mutationManager = new BassicMutationManager(probability);
            return this;
        }

        /// <summary>
        /// A Custom Mutation Manager lets you dynamically determine the probability of a mutation based on the current population.
        /// For instance, you might want to set a high mutation probability for a few generations if the population is homogeneous, and lower it while the population is diversified.
        /// </summary>
        public GeneticSearchEngineBuilder SetCustomMutationManager(IMutationManager manager)
        {
            mutationManager = manager;
            return this;
        }

        /// <summary>
        /// Sets the environment. If no environment is set, we'll use the DefaultEnvironment class.
        /// </summary>
        public GeneticSearchEngineBuilder SetEnvironment(IEnvironment environment)
        {
            this.environment = environment;
            return this;
        }

        /// <summary>
        /// If you set an IChromosomeEvaluator, the engine will use your ChromosomeEvaluator's evaluate method (and not the chromosome's default evaluate method).
        /// Since the IChromosomeEvaluator's SetEnvierment is called before the evaluation starts, your ChromosomeEvaluator can use use the information in the environment to evaluate the chromosomes.
        /// </summary>
        public GeneticSearchEngineBuilder SetCustomChromosomeEvaluator(IChromosomeEvaluator evaluator)
        {
            chromosomeEvaluator = evaluator;
            return this;
        }

        /// <summary>
        /// The IPopulationConverter interface allows you to add Lamarckian Evolution to your algorithm - 
        /// that is, let the chromosomes improve themselves before generating the children.
        /// </summary>
        public GeneticSearchEngineBuilder SetPopulationConverter(IPopulationConverter populationConverter)
        {
            this.populationConverter = populationConverter;
            return this;
        }

        /// <summary>
        /// The "percentage" of the best chromosomes will be passed "as is" to the next generation.
        /// </summary>
        public GeneticSearchEngineBuilder SetElitPercentage(double percentage)
        {
            elitPercentage = percentage;
            return this;
        }

        /// <summary>
        /// The GeneticSearchResult will include the entire history of the population.
        /// </summary>
        public GeneticSearchEngineBuilder IncludeAllHistory()
        {
            includeAllHistory = true;
            return this;
        }

        /// <summary>
        /// StopManagers let you configure when you want the search to stop.
        /// You can create your own managers by implementing the IStopManager class, or use one of the existing managers.
        /// Note that there is no limit to the number of StopManagers you can add to your search engine.
        /// </summary>
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

        /// <summary>
        /// PopulationRenwalManagers will renew a certain percentage of the population if some condition is met.
        /// You can create your own managers by implementing the IPopulationRenwalManager class, or use one of the existing managers.
        /// Note that there is no limit to the number of PopulationRenwalManagers you can add to your search engine.
        /// </summary>
        public GeneticSearchEngineBuilder AddPopulationRenwalManager(IPopulationRenwalManager manager)
        {
            populationRenwalManagers.Add(manager);
            return this;
        }

        protected void PreBuildActions()
        {
            if (environment == null && chromosomeEvaluator.GetType() != typeof(BassicChromosomeEvaluator))
                environment = new DefaultEnvironment();
        }

        public virtual GeneticSearchEngine Build()
        {
            PreBuildActions();

            var options = new GeneticSearchOptions(populationSize, stopManagers, includeAllHistory,
                populationRenwalManagers, elitPercentage, mutationManager, populationConverter, chromosomeEvaluator);
            var childrenGenerator = new ChildrenGenerator(crossoverManager, mutationManager);
            return new GeneticSearchEngine(options, populationGenerator, childrenGenerator, environment);
        }
    }
}
