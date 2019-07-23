using System;
using System.Collections.Concurrent;
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

        public GeneticSearchEngine(GeneticSearchOptions options, IPopulationGenerator populationGenerator, IChildrenGenerator childrenGenerator)
        {
            this.options = options;
            this.populationGenerator = populationGenerator;
            this.childrenGenerator = childrenGenerator;
            population = new IChromosome[options.PopulationSize];
            evaluations = new double[options.PopulationSize];
        }

        private IChromosome[] population;
        private List<IChromosome[]> history;
        private double[] evaluations;

        public GeneticSearchResult Search()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            history = new List<IChromosome[]>();
            population = populationGenerator.GeneratePopulation(options.PopulationSize).ToArray();
            if (options.IncludeAllHistory)
                history.Add(population);

            int generation;
            for (generation = 0; generation < options.MaxGenerations; generation++)
            {
                EvaluatePopulation();
                if (options.StopManagers.Any(stopManager => stopManager.ShouldStop(population, evaluations, generation)))
                    break;

                var populationToRenew = GetPopulationToRenew(generation);
                if (populationToRenew > 0)
                {
                    RenewPopulation(populationToRenew);
                    EvaluatePopulation();
                }
                
                NormilizeEvaluations();
                population = childrenGenerator.GenerateChildren(population, evaluations);
                if (options.IncludeAllHistory)
                    history.Add(population);
            }

            stopwatch.Stop();
            return new GeneticSearchResult(population, history, stopwatch.Elapsed, generation);
        }

        private int GetPopulationToRenew(int generation)
        {
            if (!options.PopulationRenwalManagers.Any())
                return 0;

            var percantage = options.PopulationRenwalManagers.Select(populationRenwalManager =>
                populationRenwalManager.ShouldRenew(population, evaluations, generation)).Max();

            if (percantage < 0)
                throw new PopulationRenewalException("percentage of the population to renew can't be less then 0");
            if (percantage > 1)
                throw new PopulationRenewalException("percentage of the population to renew can't be greater then 1");

            return (int) Math.Ceiling(options.PopulationSize * percantage);
        }

        private void RenewPopulation(int populationToRenew)
        {
            var newPopulation = populationGenerator.GeneratePopulation(populationToRenew).ToArray();
            for (int i = 0; i < populationToRenew; i++)
                population[i] = newPopulation[i];
        }

        private void EvaluatePopulation()
        {
            var index = 0;
            Parallel.ForEach(population, chromosome =>
            {
                var evaluation = chromosome.Evaluate();
                if (evaluation < 0)
                    throw new NegativeEvaluationException();
                evaluations[index] = evaluation;
                index++;
            });
        }

        private void NormilizeEvaluations()
        {
            var total = evaluations.Sum();
            for (int i = 0; i < options.PopulationSize; i++)
                evaluations[i] = evaluations[i] / total;
        }
    }
}
