using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class ChildrenGenerator : IChildrenGenerator
    {
        private readonly Random random = new Random();
        private readonly ICrossoverManager crossoverManager;
        private readonly IMutationManager mutationManager;
        private readonly ISelectionStrategy selectionStrategy;

        public ChildrenGenerator(ICrossoverManager crossoverManager, IMutationManager mutationManager, ISelectionStrategy selectionStrategy)
        {
            this.crossoverManager = crossoverManager;
            this.mutationManager = mutationManager;
            this.selectionStrategy = selectionStrategy;
        }

        public IChromosome[] GenerateChildren(Population population, int number, int generation, IEnvironment environment)
        {
            if (number < 1)
                throw new InternalSearchException("Code 1003 (requested 0 children)");

            selectionStrategy.SetPopulation(population);
            var mutationProbability = mutationManager.MutationProbability(population, environment, generation);

            if (mutationProbability > 1 || mutationProbability < 0)
                throw new GeneticAlgorithmException(nameof(mutationProbability) + " must be between 0.0 to 1.0 (including)");

            var children = new ConcurrentBag<IChromosome>();
            var tasks = new Task[number];
            for (int i = 0; i < number; i++)
                tasks[i] = Task.Run(() =>
                {
                    var parent1 = selectionStrategy.SelectChromosome();
                    var parent2 = selectionStrategy.SelectChromosome();
                    var child = crossoverManager.Crossover(parent1, parent2);
                    if (random.NextDouble() < mutationProbability)
                        child.Mutate();
                    children.Add(child);
                });

            foreach (var task in tasks)
                task.Wait();
            
            return children.ToArray();
        }
    }
}
