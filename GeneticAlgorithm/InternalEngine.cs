using System;
using System.Collections.Generic;
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
        private readonly List<IChromosome[]> history = new List<IChromosome[]>();
        private readonly Action<IChromosome[], double[]> onNewGeneration;

        public InternalEngine(IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator,
            GeneticSearchOptions options, Action<IChromosome[], double[]> onNewGeneration)
        {
            this.populationGenerator = populationGenerator;
            this.childrenGenerator = childrenGenerator;
            this.options = options;
            this.onNewGeneration = onNewGeneration;
        }

        public InternalSearchResult RunSingleGeneration(Population population, int generation)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            population = CreateNewGeneration(population, generation);
            EvaluatePopulation(population);

            if (options.StopManagers.Any(stopManager =>
                stopManager.ShouldStop(population.GetChromosomes(), population.GetEvaluations(), generation)))
            {
                UpdateNewGeneration(population);
                stopwatch.Stop();
                return new InternalSearchResult(population, stopwatch.Elapsed, true);
            }

            var populationToRenew = GetPopulationToRenew(population, generation);
            if (populationToRenew > 0)
            {
                population = RenewPopulation(populationToRenew, population);
                EvaluatePopulation(population);
            }

            UpdateNewGeneration(population);
            stopwatch.Stop();
            return new InternalSearchResult(population, stopwatch.Elapsed, false);
        }

        public InternalSearchResult RenewPopulationAndUpdatePopulation(double percantage, Population population)
        {
            var chromosomesToRenew = (int)Math.Ceiling(options.PopulationSize * percantage);
            var newPopulation = RenewPopulation(chromosomesToRenew, population);
            EvaluatePopulation(newPopulation);
            UpdateNewGeneration(newPopulation);
            return new InternalSearchResult(newPopulation, TimeSpan.Zero, false);
        }
        
        private Population CreateNewGeneration(Population population, int generation)
        {
            return generation == 1
                ? new Population(populationGenerator.GeneratePopulation(options.PopulationSize).ToArray())
                : GenerateChildren(population, generation);
        }

        private Population GenerateChildren(Population population, int generation)
        {
            NormilizeEvaluations(population);
            var eliteChromosomes = (int)Math.Ceiling(options.PopulationSize * options.ElitPercentage);
            var numberOfChildren = options.PopulationSize - eliteChromosomes;
            var children = childrenGenerator.GenerateChildren(population, numberOfChildren, generation);
            var elite = GetBestChromosomes(eliteChromosomes, population);
            return new Population(SearchUtils.Combine(children, elite));
        }

        private int GetPopulationToRenew(Population population, int generation)
        {
            if (!options.PopulationRenwalManagers.Any())
                return 0;

            var percantage = options.PopulationRenwalManagers.Select(populationRenwalManager =>
                populationRenwalManager.ShouldRenew(population.GetChromosomes(), population.GetEvaluations(), generation)).Max();

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

        private static void EvaluatePopulation(Population population)
        {
            Parallel.ForEach(population, chromosome =>
            {
                var evaluation = chromosome.Chromosome.Evaluate();
                if (evaluation < 0)
                    throw new NegativeEvaluationException();
                chromosome.Evaluation = evaluation;
            });
        }

        private void NormilizeEvaluations(Population population)
        {
            var total = population.GetEvaluations().Sum();
            Parallel.ForEach(population, chromosome =>
            {
                chromosome.Evaluation = chromosome.Evaluation / total;
            });
        }

        /// <summary>
        /// Update everyone that needs to know about the new generation
        /// </summary>
        private void UpdateNewGeneration(Population population)
        {
            var chromosomes = population.GetChromosomes();
            var evaluations = population.GetEvaluations();

            foreach (var stopManager in options.StopManagers)
                stopManager.AddGeneration(chromosomes, evaluations);
            foreach (var populationRenwalManager in options.PopulationRenwalManagers)
                populationRenwalManager.AddGeneration(chromosomes, evaluations);
            options.MutationManager.AddGeneration(chromosomes, evaluations);

            onNewGeneration(chromosomes, evaluations);

            if (options.IncludeAllHistory)
                history.Add(chromosomes);
        }
    }
}
