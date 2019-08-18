using System;
using FakeItEasy;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.UnitTests
{
    class TestPopulationManager
    {
        private readonly IPopulationGenerator populationGenerator = A.Fake<IPopulationGenerator>();
        private readonly IChildrenGenerator childrenGenerator = A.Fake<IChildrenGenerator>();
        private int initialPopulationSize;

        public TestPopulationManager(double[][] populationEvaluation)
        {
            GenerateInitailPopulation(populationEvaluation[0]);

            int index = 0;
            A.CallTo(() => childrenGenerator.GenerateChildren(A<Population>._, A<int>._, A<int>._, A<IEnvironment>._)).ReturnsLazily(
                (Population p, int n, int g, IEnvironment e) =>
                {
                    index++;
                    return populationEvaluation[index].ToChromosomes($"Gen{index}");
                });
        }
        
        public TestPopulationManager(double[] populationEvaluation, Func<IChromosome, double> nextGenerationEvaluationFunc = null)
        {
            GenerateInitailPopulation(populationEvaluation);

            nextGenerationEvaluationFunc = nextGenerationEvaluationFunc ?? (c => c.Evaluate());

            int index = 0;
            A.CallTo(() => childrenGenerator.GenerateChildren(A<Population>._, A<int>._, A<int>._, A<IEnvironment>._)).ReturnsLazily(
                (Population p, int n, int g, IEnvironment e) =>
                {
                    index++;

                    var newEvaluation = new double[populationEvaluation.Length];
                    for (int i = 0; i < populationEvaluation.Length; i++)
                        newEvaluation[i] = nextGenerationEvaluationFunc(p[i].Chromosome);

                    return newEvaluation.ToChromosomes("Gen" + index);
                });
        }
        
        private void GenerateInitailPopulation(double[] populationEvaluation)
        {
            initialPopulationSize = populationEvaluation.Length;
            var initailPopulation = new IChromosome[initialPopulationSize];
            for (int i = 0; i < initailPopulation.Length; i++)
            {
                initailPopulation[i] = A.Fake<IChromosome>();
                A.CallTo(() => initailPopulation[i].ToString()).Returns("Initial Chromosome");
                A.CallTo(() => initailPopulation[i].Evaluate()).Returns(populationEvaluation[i]);
            }
            A.CallTo(() => populationGenerator.GeneratePopulation(A<int>._)).Returns(initailPopulation);
        }
        
        public void SetPopulationGenerated(double[][] populationEvaluation)
        {
            var initialPopulation = populationGenerator.GeneratePopulation(initialPopulationSize);

            var index = -2;
            A.CallTo(() => populationGenerator.GeneratePopulation(A<int>._)).ReturnsLazily((int s) =>
            {
                index++;
                if (index == -1) return initialPopulation;

                return populationEvaluation[index].ToChromosomes("Renewal" + index, s);
            });
        }

        public IPopulationGenerator GetPopulationGenerator() => populationGenerator;

        public ICrossoverManager GetCrossoverManager() => A.Fake<ICrossoverManager>();

        public IChildrenGenerator GetChildrenGenerator() => childrenGenerator;
    }
}
