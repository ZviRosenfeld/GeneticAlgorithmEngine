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

        /// <summary>
        /// This event is risen once for every new generation. It's arguments are the population and their evaluations.
        /// </summary>
        public event Action<IChromosome[], double[]> OnNewGeneration; 

        public GeneticSearchEngine(GeneticSearchOptions options, IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator)
        {
            resultBuilder = new ResultBuilder(options.IncludeAllHistory);
            engine = new InternalEngine(populationGenerator, childrenGenerator, options, (c, d) => OnNewGeneration?.Invoke(c, d));
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
    }
}
