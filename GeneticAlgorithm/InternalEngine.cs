using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    /// <summary>
    /// This class contains the engine's actual logic
    /// </summary>
    class InternalEngine
    {
        private readonly IPopulationGenerator populationGenerator;
        private readonly IChildrenGenerator childrenGenerator;
        private readonly GeneticSearchOptions options;
        private readonly Action<Population, IEnvironment> onNewGeneration;
        private readonly IEnvironment environment;

        public InternalEngine(IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator,
            GeneticSearchOptions options, Action<Population, IEnvironment> onNewGeneration, IEnvironment environment)
        {
            this.populationGenerator = populationGenerator;
            this.childrenGenerator = childrenGenerator;
            this.options = options;
            this.onNewGeneration = onNewGeneration;
            this.environment = environment;
        }

        public InternalSearchResult RunSingleGeneration(Population population, int generation)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var nextGeneration = CreateNewGeneration(population, generation);
            foreach (var populationConverter in options.PopulationConverters)
                nextGeneration = populationConverter.ConvertPopulation(nextGeneration, generation, environment);
            population = new Population(nextGeneration);
            environment?.UpdateEnvierment(nextGeneration, generation);

            EvaluatePopulation(population);

            if (options.StopManagers.Any(stopManager =>
                stopManager.ShouldStop(population, environment, generation)))
            {
                UpdateNewGeneration(population);
                stopwatch.Stop();
                return new InternalSearchResult(population, stopwatch.Elapsed, true, environment);
            }

            var populationToRenew = GetPopulationToRenew(population, generation);
            if (populationToRenew > 0)
            {
                population = RenewPopulation(populationToRenew, population);
                EvaluatePopulation(population);
            }

            UpdateNewGeneration(population);
            stopwatch.Stop();
            return new InternalSearchResult(population, stopwatch.Elapsed, false, environment);
        }

        public InternalSearchResult RenewPopulationAndUpdatePopulation(double percantage, Population population)
        {
            var chromosomesToRenew = (int)Math.Ceiling(options.PopulationSize * percantage);
            var newPopulation = RenewPopulation(chromosomesToRenew, population);
            EvaluatePopulation(newPopulation);
            UpdateNewGeneration(newPopulation);
            return new InternalSearchResult(newPopulation, TimeSpan.Zero, false, environment);
        }

        public InternalSearchResult ConvertPopulationAndUpdatePopulation(IChromosome[] population)
        {
            var newPopulation = new Population(population);
            EvaluatePopulation(newPopulation);
            UpdateNewGeneration(newPopulation);
            return new InternalSearchResult(newPopulation, TimeSpan.Zero, false, environment);
        }

        private IChromosome[] CreateNewGeneration(Population population, int generation)
        {
            return generation == 0
                ? populationGenerator.GeneratePopulation(options.PopulationSize).ToArray()
                : GenerateChildren(population, generation);
        }

        private IChromosome[] GenerateChildren(Population population, int generation)
        {
            var eliteChromosomes = (int)Math.Ceiling(options.PopulationSize * options.ElitePercentage);
            var numberOfChildren = options.PopulationSize - eliteChromosomes;
            var children = childrenGenerator.GenerateChildren(population, numberOfChildren, generation, environment);
            var elite = GetBestChromosomes(eliteChromosomes, population);
            return SearchUtils.Combine(children, elite);
        }

        private int GetPopulationToRenew(Population population, int generation)
        {
            if (!options.PopulationRenwalManagers.Any())
                return 0;

            var percantage = options.PopulationRenwalManagers.Select(populationRenwalManager =>
                populationRenwalManager.ShouldRenew(population, environment, generation)).Max();

            if (percantage < 0)
                throw new PopulationRenewalException("percentage of the population to renew can't be less then 0");
            if (percantage > 1)
                throw new PopulationRenewalException("percentage of the population to renew can't be greater then 1");

            return (int)Math.Ceiling(options.PopulationSize * percantage);
        }

        private Population RenewPopulation(int populationToRenew, Population population)
        {
            var newPopulation = populationGenerator.GeneratePopulation(populationToRenew).ToArray();
            var oldPopulation = GetBestChromosomes(options.PopulationSize - populationToRenew, population);
            return new Population(SearchUtils.Combine(newPopulation, oldPopulation));
        }

        private static IChromosome[] GetBestChromosomes(int n, Population population)
        {
            if (n == 0)
                return new IChromosome[0];

            var min = population.GetEvaluations().OrderByDescending(x => x).Take(n).Last();
            var bestChromosomes = new IChromosome[n];
            int index = 0;
            foreach (var chromosome in population)
            {
                if (chromosome.Evaluation >= min)
                {
                    bestChromosomes[index] = chromosome.Chromosome;
                    index++;
                }
                if (index >= n)
                    return bestChromosomes;
            }

            throw new InternalSearchException("Code 1000 (not enough best chromosomes found)");
        }

        private void EvaluatePopulation(Population population)
        {
            options.ChromosomeEvaluator.SetEnvierment(environment);

            Parallel.ForEach(population, chromosome =>
            {
                var evaluation = options.ChromosomeEvaluator.Evaluate(chromosome.Chromosome);
                if (evaluation < 0)
                    throw new NegativeEvaluationException();
                chromosome.Evaluation = evaluation;
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

            onNewGeneration(population, environment);
        }
    }
}
