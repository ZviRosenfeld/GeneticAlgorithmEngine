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
        private readonly ResultBuilder resultBuilder;
        private readonly GeneticSearchOptions options;

        /// <summary>
        /// This event is risen once for every new generation.
        /// </summary>
        public event Action<Population, IEnvironment> OnNewGeneration; 

        public GeneticSearchEngine(GeneticSearchOptions options, IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator, IEnvironment environment)
        {
            this.options = options;
            resultBuilder = new ResultBuilder(options.IncludeAllHistory);
            engine = new InternalEngine(populationGenerator, childrenGenerator, options, (p, e) => OnNewGeneration?.Invoke(p, e), environment);
        }
        
        private int generation = 0;
        private InternalSearchResult lastResult = null;
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
                    generation++;
                    lastResult = engine.RunSingleGeneration(lastResult?.Population, generation);
                    resultBuilder.AddGeneration(lastResult);
                    if (lastResult.IsCompleted) break;
                }
                return resultBuilder.Build(generation);
            });
        }

        /// <summary>
        /// Creates the next generation.
        /// </summary>
        public GeneticSearchResult Next()
        {
            return RunAsCriticalBlock(() =>
            {
                generation++;
                lastResult = engine.RunSingleGeneration(lastResult?.Population, generation);
                resultBuilder.AddGeneration(lastResult);
                return resultBuilder.Build(generation);
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
            if (lastResult?.Population == null)
                throw new GeneticAlgorithmException("Can renew population before the search started.");

            if (percentageToRenew <= 0 || percentageToRenew > 1)
                throw new GeneticAlgorithmException($"{nameof(percentageToRenew)} must be between 0 (not including) and 1 (including).");

            return RunAsCriticalBlock(() =>
            {
                generation++;
                lastResult = engine.RenewPopulationAndUpdatePopulation(percentageToRenew, lastResult.Population);
                resultBuilder.AddGeneration(lastResult);
                return resultBuilder.Build(generation);
            });
        }

        /// <summary>
        /// Gets the current population. This can only be called while the engine is paused.
        /// </summary>
        public GeneticSearchResult GetCurrentPopulation()
        {
            if (IsRunning)
                throw new EngineAlreadyRunningException();

            return resultBuilder.Build(generation);
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
                generation++;
                lastResult = engine.ConvertPopulationAndUpdatePopulation(newPopulation);
                resultBuilder.AddGeneration(lastResult);
                return resultBuilder.Build(generation);
            });
        }

        public void Dispose()
        {
            engineFinishedEvent?.Dispose();
        }
    }
}
