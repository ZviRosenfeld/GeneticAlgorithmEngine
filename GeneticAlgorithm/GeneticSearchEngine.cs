using System;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchEngine : IDisposable
    {
        private readonly RunAndPauseManager runAndPauseManager;
        private readonly InternalEngine engine;
        private readonly SearchContext searchContext;
        private readonly GeneticSearchOptions options;
        private readonly TimeSpan pauseTimeout = TimeSpan.FromMinutes(1);

        /// <summary>
        /// This event is risen once for every new generation.
        /// </summary>
        public event Action<Population, IEnvironment> OnNewGeneration; 

        public GeneticSearchEngine(GeneticSearchOptions options, IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator, IEnvironment environment)
        {
            this.options = options;
            searchContext = new SearchContext(options.IncludeAllHistory, environment);
            engine = new InternalEngine(populationGenerator, childrenGenerator, options);
            runAndPauseManager = new RunAndPauseManager(pauseTimeout);
        }

        public bool IsRunning => runAndPauseManager.IsRunning;

        /// <summary>
        /// Run's a complete search.
        /// </summary>
        public GeneticSearchResult Run()
        {
            return runAndPauseManager.RunAsCriticalBlock(() =>
            {
                while (!runAndPauseManager.ShouldPause)
                {
                    var result = engine.RunSingleGeneration(searchContext.LastGeneration, searchContext.Generation, searchContext.Environment);
                    UpdateNewGeneration(result.Population);
                    searchContext.AddGeneration(result);
                    if (result.IsCompleted) break;
                }
                return searchContext.BuildResult();
            });
        }

        /// <summary>
        /// Creates the next generation.
        /// </summary>
        public GeneticSearchResult Next()
        {
            return runAndPauseManager.RunAsCriticalBlock(() =>
            {
                var result = engine.RunSingleGeneration(searchContext.LastGeneration, searchContext.Generation, searchContext.Environment);
                UpdateNewGeneration(result.Population);
                searchContext.AddGeneration(result);
                return searchContext.BuildResult();
            });
        }

        /// <summary>
        /// Pauses the search (if it is running).
        /// Returns true if the search is running; false otherwise.
        /// </summary>
        /// <returns>True if the search is running; false otherwise</returns>
        public bool Pause() => runAndPauseManager.Pause();

        /// <summary>
        /// Renews a certain percentage of the population. This can only be called while the engine is paused.
        /// Note that the renewed population will be considered a new generation. 
        ///  </summary>
        public GeneticSearchResult RenewPopulation(double percentageToRenew)
        {
            if (searchContext.LastGeneration == null)
                throw new GeneticAlgorithmException("Can renew population before the search started.");

            if (percentageToRenew <= 0 || percentageToRenew > 1)
                throw new GeneticAlgorithmException($"{nameof(percentageToRenew)} must be between 0 (not including) and 1 (including).");

            return runAndPauseManager.RunAsCriticalBlock(() =>
            {
                var lastResult = engine.RenewPopulation(percentageToRenew, searchContext.LastGeneration, searchContext.Environment);
                UpdateNewGeneration(lastResult.Population);
                searchContext.AddGeneration(lastResult);
                return searchContext.BuildResult();
            });
        }

        /// <summary>
        /// Gets the current population. This can only be called while the engine is paused.
        /// </summary>
        public GeneticSearchResult GetCurrentPopulation()
        {
            if (IsRunning)
                throw new EngineAlreadyRunningException();

            return runAndPauseManager.RunAsCriticalBlock(() => searchContext.BuildResult());
        }

        /// <summary>
        /// Sets the current population. This can only be called while the engine is paused.
        /// Note that the renewed population will be considered a new generation. 
        /// </summary>
        public GeneticSearchResult SetCurrentPopulation(IChromosome[] newPopulation)
        {
            if (newPopulation.Length != options.PopulationSize)
                throw new GeneticAlgorithmException($"Population size isn't right. Expected {options.PopulationSize}; got {newPopulation.Length}");

            return runAndPauseManager.RunAsCriticalBlock(() =>
            {
                var lastResult = engine.ConvertPopulation(newPopulation, searchContext.Environment);
                UpdateNewGeneration(lastResult.Population);
                searchContext.AddGeneration(lastResult);
                return searchContext.BuildResult();
            });
        }

        /// <summary>
        /// Update everyone that needs to know about the new generation
        /// </summary>
        private void UpdateNewGeneration(Population population)
        {
            foreach (var stopManager in options.StopManagers)
                stopManager.AddGeneration(population);
            foreach (var populationRenwalManager in options.PopulationRenwalManagers)
                populationRenwalManager.AddGeneration(population);
            foreach (var populationConverter in options.PopulationConverters)
                populationConverter.AddGeneration(population);
            options.MutationManager.AddGeneration(population);

            OnNewGeneration?.Invoke(population, searchContext.Environment);
        }

        public void Dispose()
        {
            Pause();
            runAndPauseManager.Dispose();
        }
    }
}
