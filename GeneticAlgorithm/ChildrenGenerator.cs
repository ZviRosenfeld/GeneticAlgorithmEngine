using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class ChildrenGenerator : IChildrenGenerator
    {
        private readonly Random random = new Random();
        private readonly ICrossoverManager crossoverManager;
        private readonly GeneticSearchOptions options;

        public ChildrenGenerator(GeneticSearchOptions options, ICrossoverManager crossoverManager)
        {
            this.crossoverManager = crossoverManager;
            this.options = options;
        }

        public IChromosome[] GenerateChildren(IChromosome[] population, double[] evaluations, int number)
        {
            var children = new ConcurrentBag<IChromosome>();
            var tasks = new Task[number];
            for (int i = 0; i < number; i++)
                tasks[i] = Task.Run(() =>
                {
                    var parent1 = SearchUtils.ChooseParent(population, evaluations);
                    var parent2 = SearchUtils.ChooseParent(population, evaluations);
                    var child = crossoverManager.Crossover(parent1, parent2);
                    if (random.NextDouble() < options.MutationProbability)
                        child.Mutate();
                    children.Add(child);
                });

            foreach (var task in tasks)
                task.Wait();
            
            return children.ToArray();
        }
    }
}
