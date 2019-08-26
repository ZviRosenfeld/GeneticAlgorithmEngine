using System;
using System.Threading;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchEngine : IDisposable
    {
        private readonly TimeSpan pauseTimeout = TimeSpan.FromMinutes(1);
        private readonly object runLock = new object();
        private readonly object pauseLock = new object();
        private readonly ManualResetEvent engineFinishedEvent = new ManualResetEvent(true);
        private readonly InternalEngine engine;
        private readonly SearchContext searchContext;
        private readonly GeneticSearchOptions options;

        /// <summary>
        /// This event is risen once for every new generation.
        /// </summary>
        public event Action<Population, IEnvironment> OnNewGeneration; 

        public GeneticSearchEngine(GeneticSearchOptions options, IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator, IEnvironment environment)
        {
            this.options = options;
            searchContext = new SearchContext(options.IncludeAllHistory, environment);
            engine = new InternalEngine(populationGenerator, childrenGenerator, options);
        }
        
        private bool ShouldPause = false;
        
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Run's a complete search.
        /// </summary>
        public GeneticSearchResult Run()
        {
            return RunAsCriticalBlock(() =>
            {
                while (!ShouldPause)
                {
                    var lastResult = engine.RunSingleGeneration(searchContext?.LastGeneration, searchContext.Generation, searchContext.Environment);
                    UpdateNewGeneration(lastResult.Population);
                    searchContext.AddGeneration(lastResult);
                    if (lastResult.IsCompleted) break;
                }
                return searchContext.BuildResult();
            });
        }

        /// <summary>
        /// Creates the next generation.
        /// </summary>
        public GeneticSearchResult Next()
        {
            return RunAsCriticalBlock(() =>
            {
                var lastResult = engine.RunSingleGeneration(searchContext?.LastGeneration, searchContext.Generation, searchContext.Environment);
                UpdateNewGeneration(lastResult.Population);
                searchContext.AddGeneration(lastResult);
                return searchContext.BuildResult();
            });
        }

        private T RunAsCriticalBlock<T>(Func<T> func)
        {
            lock (pauseLock)
            lock (runLock)
            {
                if (IsRunning)
                    throw new EngineAlreadyRunningException();
                engineFinishedEvent.Reset();
                IsRunning = true;
            }

            try
            {
                return func();
            }
            finally
            {
                lock (runLock)
                {
                    IsRunning = false;
                    engineFinishedEvent.Set();
                }
            }
        }
        
        /// <summary>
        /// Pauses the search (if it is running).
        /// Returns true if the search is running; false otherwise.
        /// </summary>
        /// <returns>True if the search is running; false otherwise</returns>
        public bool Pause()
        {
            if (!IsRunning)
                return false;

            lock (pauseLock)
            {
                if (!IsRunning)
                    return false;

                try
                {
                    ShouldPause = true;
                    if (!engineFinishedEvent.WaitOne(pauseTimeout))
                        throw new CouldntStopEngineException();
                    return true;
                }
                finally
                {
                    ShouldPause = false;
                }
            }
        }

        /// <summary>
        /// Renews a certain percentage of the population. This can only be called while the engine is paused.
        /// Note that the renewed population will be considered a new generation. 
        ///  </summary>
        public GeneticSearchResult RenewPopulation(double percentageToRenew)
        {
            if (searchContext?.LastGeneration == null)
                throw new GeneticAlgorithmException("Can renew population before the search started.");

            if (percentageToRenew <= 0 || percentageToRenew > 1)
                throw new GeneticAlgorithmException($"{nameof(percentageToRenew)} must be between 0 (not including) and 1 (including).");

            return RunAsCriticalBlock(() =>
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

            return RunAsCriticalBlock(() => searchContext.BuildResult());
        }

        /// <summary>
        /// Sets the current population. This can only be called while the engine is paused.
        /// Note that the renewed population will be considered a new generation. 
        /// </summary>
        public GeneticSearchResult SetCurrentPopulation(IChromosome[] newPopulation)
        {
            if (newPopulation.Length != options.PopulationSize)
                throw new GeneticAlgorithmException($"Population size isn't right. Expected {options.PopulationSize}; got {newPopulation.Length}");

            return RunAsCriticalBlock(() =>
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
            engineFinishedEvent?.Dispose();
        }
    }
}
