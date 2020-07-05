using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.MutationManagers;
using GeneticAlgorithm.SelectionStrategies;

namespace GeneticAlgorithm
{
    public class ChildrenGenerator : IChildrenGenerator
    {
        private readonly List<Type> officalSelectionStrategies = new List<Type>
        {
            typeof(RouletteWheelSelection),
            typeof(StochasticUniversalSampling),
            typeof(TournamentSelection)
        };
        private readonly ICrossoverManager crossoverManager;
        private readonly IMutationProbabilityManager mutationManager;
        private readonly ISelectionStrategy selectionStrategy;

        public ChildrenGenerator(ICrossoverManager crossoverManager, IMutationProbabilityManager mutationManager, ISelectionStrategy selectionStrategy)
        {
            this.crossoverManager = crossoverManager;
            this.mutationManager = mutationManager;
            this.selectionStrategy = selectionStrategy;
        }

        public IChromosome[] GenerateChildren(Population population, int number, int generation, IEnvironment environment)
        {
            if (number < 1)
                throw new InternalSearchException("Code 1003 (requested 0 children)");

            selectionStrategy.SetPopulation(population, number * 2);
            var mutationProbability = mutationManager.MutationProbability(population, environment, generation);
            CheckMuationProbability(mutationProbability);
            
            var children = new ConcurrentBag<IChromosome>();
            var tasks = new Task[number];
            for (int i = 0; i < number; i++)
                tasks[i] = Task.Run(() =>
                {
                    var parent1 = AssertNotNull(selectionStrategy.SelectChromosome());
                    var parent2 = AssertNotNull(selectionStrategy.SelectChromosome());
                    var child = crossoverManager.Crossover(parent1, parent2);
                    if (ProbabilityUtils.P(mutationProbability))
                        child.Mutate();
                    children.Add(child);
                });

            foreach (var task in tasks)
                task.Wait();
            
            return children.ToArray();
        }

        private void CheckMuationProbability(double probability)
        {
            if (probability >= 0 && probability <= 1) return;

            if (mutationManager.GetType() == typeof(BasicMutationProbabilityManager))
                throw new InternalSearchException(
                    $"Code 1004 (Bad mutation value for manager {mutationManager.GetType()})");
            throw new BadMutationProbabilityException(probability);
        }

        private IChromosome AssertNotNull(IChromosome chromosome)
        {
            if (chromosome != null) return chromosome;

            var message = $"Selected chromosome was null. Manager = {mutationManager.GetType()}";
            if (officalSelectionStrategies.Contains(mutationManager.GetType()))
                throw new InternalSearchException($"Code 1005 ({message})");

            throw new GeneticAlgorithmException(message);
        }
    }
}
