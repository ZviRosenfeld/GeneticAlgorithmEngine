using System;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchEngine
    {
        private readonly object lockObject = new object();
        private readonly InternalEngine engine;
        private readonly ResultBuilder resultBuilder;
        private readonly GeneticSearchOptions options;

        /// <summary>
        /// This event is risen once for every new generation. It's arguments are the population and their evaluations.
        /// </summary>
        public event Action<IChromosome[], double[]> OnNewGeneration; 

        public GeneticSearchEngine(GeneticSearchOptions options, IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator, IEnvironment environment)
        {
            this.options = options;
            resultBuilder = new ResultBuilder(options.IncludeAllHistory);
            engine = new InternalEngine(populationGenerator, childrenGenerator, options, (c, d) => OnNewGeneration?.Invoke(c, d), environment);
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
            TryToStart();
            try
            {
                while (!ShouldPause)
                {
                    generation++;
                    lastResult = engine.RunSingleGeneration(lastResult?.Population, generation);
                    resultBuilder.AddGeneration(lastResult);
                    if (lastResult.IsCompleted) break;
                }
                return resultBuilder.Build(generation);
            }
            finally
            {
                IsRunning = false;
            }
        }

        private void TryToStart()
        {
            lock (lockObject)
            {
                if (IsRunning)
                    throw new EngineAlreadyRunningException();
                IsRunning = true;
            }
        }

        /// <summary>
        /// Creates the next generation.
        /// </summary>
        public GeneticSearchResult Next()
        {
            TryToStart();
            try
            {
                generation++;
                lastResult = engine.RunSingleGeneration(lastResult?.Population, generation);
                resultBuilder.AddGeneration(lastResult);                
                return resultBuilder.Build(generation);
            }
            finally
            {
                IsRunning = false;
            }
        }

        /// <summary>
        /// Puases the search (if it is running).
        /// </summary>
        public bool Puase()
        {
            if (!IsRunning)
                return false;

            ShouldPause = true;
            return true;
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

            lock (lockObject)
            {
                if (IsRunning)
                    throw new EngineAlreadyRunningException();

                generation++;
                lastResult = engine.RenewPopulationAndUpdatePopulation(percentageToRenew, lastResult.Population);  
                resultBuilder.AddGeneration(lastResult);
                return resultBuilder.Build(generation);
            }
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

            lock (lockObject)
            {
                if (IsRunning)
                    throw new EngineAlreadyRunningException();

                generation++;
                lastResult = engine.ConvertPopulationAndUpdatePopulation(newPopulation);
                resultBuilder.AddGeneration(lastResult);
                return resultBuilder.Build(generation);
            }
        }
    }
}
