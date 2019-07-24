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

        public IChromosome[] GenerateChildren(Population population, int number)
        {
            var children = new ConcurrentBag<IChromosome>();
            var tasks = new Task[number];
            for (int i = 0; i < number; i++)
                tasks[i] = Task.Run(() =>
                {
                    var evaluation = population.GetEvaluations();
                    var chromosomes = population.GetChromosomes();
                    var parent1 = ChooseParent(chromosomes, evaluation);
                    var parent2 = ChooseParent(chromosomes, evaluation);
                    var child = crossoverManager.Crossover(parent1, parent2);
                    if (random.NextDouble() < options.MutationProbability)
                        child.Mutate();
                    children.Add(child);
                });

            foreach (var task in tasks)
                task.Wait();
            
            return children.ToArray();
        }

        public IChromosome ChooseParent(IChromosome[] population, double[] evaluations)
        {
            var randomNumber = random.NextDouble();
            var sum = 0.0;
            var index = -1;
            while (sum < randomNumber)
            {
                index++;
                sum += evaluations[index];
            }

            return population[index];
        }
    }
}
