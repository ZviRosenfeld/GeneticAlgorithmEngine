using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class GeneticSearchEngine
    {
        private readonly IPopulationGenerator populationGenerator;
        private readonly IChildrenGenerator childrenGenerator;
        private readonly GeneticSearchOptions options;

        /// <summary>
        /// This even is risen once for every new generation. It's arguments are the population and their evaluations.
        /// </summary>
        public event Action<IChromosome[], double[]> OnNewGeneration; 

        public GeneticSearchEngine(GeneticSearchOptions options, IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator)
        {
            this.options = options;
            this.populationGenerator = populationGenerator;
            this.childrenGenerator = childrenGenerator;
        }

        private Population population;
        private List<IChromosome[]> history;

        public GeneticSearchResult Search()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            history = new List<IChromosome[]>();
            int generation;
            for (generation = 0; generation < options.MaxGenerations; generation++)
            {
                if (generation == 0)
                    population = new Population(populationGenerator.GeneratePopulation(options.PopulationSize).ToArray());
                else
                    GenerateChildren();
                EvaluatePopulation();

                if (options.StopManagers.Any(stopManager => stopManager.ShouldStop(population.GetChromosomes(), population.GetEvaluations(), generation)))
                {
                    UpdateEventsAndHistory(population.GetChromosomes(), population.GetEvaluations());
                    break;
                }

                var populationToRenew = GetPopulationToRenew(generation);
                if (populationToRenew > 0)
                {
                    RenewPopulation(populationToRenew);
                    EvaluatePopulation();
                }

                UpdateNewGeneration();
                NormilizeEvaluations();
            }

            stopwatch.Stop();
            return new GeneticSearchResult(population.ChooseBest(), population.GetChromosomes(), history, stopwatch.Elapsed, generation);
        }

        /// <summary>
        /// Update everyone that needs to know about the new generation
        /// </summary>
        private void UpdateNewGeneration()
        {
            var chromosomes = population.GetChromosomes();
            var evaluations = population.GetEvaluations();

            foreach (var stopManager in options.StopManagers)
                stopManager.AddGeneration(chromosomes, evaluations);
            foreach (var populationRenwalManager in options.PopulationRenwalManagers)
                populationRenwalManager.AddGeneration(chromosomes, evaluations);

            UpdateEventsAndHistory(chromosomes, evaluations);
        }

        private void UpdateEventsAndHistory(IChromosome[] chromosomes, double[] evaluations)
        {
            OnNewGeneration?.Invoke(chromosomes, evaluations);

            if (options.IncludeAllHistory)
                history.Add(population.GetChromosomes());
        }

        private void GenerateChildren()
        {
            var eliteChromosomes = (int) Math.Ceiling(options.PopulationSize * options.ElitPercentage);
            var numberOfChildren = options.PopulationSize - eliteChromosomes;
            var children = childrenGenerator.GenerateChildren(population, numberOfChildren);
            var elite = GetBestChromosomes(eliteChromosomes);
            population = new Population(SearchUtils.Combine(children, elite));
        }

        private int GetPopulationToRenew(int generation)
        {
            if (!options.PopulationRenwalManagers.Any())
                return 0;

            var percantage = options.PopulationRenwalManagers.Select(populationRenwalManager =>
                populationRenwalManager.ShouldRenew(population.GetChromosomes(), population.GetEvaluations(), generation)).Max();

            if (percantage < 0)
                throw new PopulationRenewalException("percentage of the population to renew can't be less then 0");
            if (percantage > 1)
                throw new PopulationRenewalException("percentage of the population to renew can't be greater then 1");

            return (int) Math.Ceiling(options.PopulationSize * percantage);
        }

        private void RenewPopulation(int populationToRenew)
        {
            var newPopulation = populationGenerator.GeneratePopulation(populationToRenew).ToArray();
            var oldPopulation = GetBestChromosomes(options.PopulationSize - populationToRenew);
            population = new Population(SearchUtils.Combine(newPopulation, oldPopulation));
        }

        private IChromosome[] GetBestChromosomes(int n)
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

        private void EvaluatePopulation()
        {
            Parallel.ForEach(population, chromosome =>
            {
                var evaluation = chromosome.Chromosome.Evaluate();
                if (evaluation < 0)
                    throw new NegativeEvaluationException();
                chromosome.Evaluation = evaluation;
            });
        }

        private void NormilizeEvaluations()
        {
            var total = population.GetEvaluations().Sum();
            Parallel.ForEach(population, chromosome =>
            {
                chromosome.Evaluation = chromosome.Evaluation / total;
            });
        }
    }
}
