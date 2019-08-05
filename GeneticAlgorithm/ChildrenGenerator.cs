using System;
using System.Collections.Concurrent;
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

        public ChildrenGenerator(ICrossoverManager crossoverManager, IMutationManager mutationManager)
        {
            this.crossoverManager = crossoverManager;
            this.mutationManager = mutationManager;
        }

        public IChromosome[] GenerateChildren(Population population, int number, int generation)
        {
            var mutationProbability = mutationManager.MutationProbability(population.GetChromosomes(),
                population.GetEvaluations(), generation);

            if (mutationProbability > 1 || mutationProbability < 0)
                throw new GeneticAlgorithmException(nameof(mutationProbability) + " must be between 0.0 to 1.0 (including)");

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
                    if (random.NextDouble() < mutationProbability)
                        child.Mutate();
                    children.Add(child);
                });

            foreach (var task in tasks)
                task.Wait();
            
            return children.ToArray();
        }

        private IChromosome ChooseParent(IChromosome[] population, double[] evaluations)
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
