using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    public class BinaryVectorChromosomePopulationGenerator : IPopulationGenerator
    {
        private readonly int vectorSize;
        private readonly IMutationManager<bool> mutationManager;
        private readonly IEvaluator evaluator;

        public BinaryVectorChromosomePopulationGenerator(int vectorSize, IMutationManager<bool> mutationManager, IEvaluator evaluator)
        {
            this.vectorSize = vectorSize;
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IEnumerable<IChromosome> GeneratePopulation(int size)
        {
            var population = new IChromosome[size];

            for (int i = 0; i < size; i++)
                population[i] = GetChromosome();

            return population;
        }

        private VectorChromosome<bool> GetChromosome()
        {
            var vector = new bool[vectorSize];
            for (int i = 0; i < vectorSize; i++)
                vector[i] = ProbabilityUtils.P(0.5);

            return new VectorChromosome<bool>(vector, mutationManager, evaluator);
        }
    }
}
