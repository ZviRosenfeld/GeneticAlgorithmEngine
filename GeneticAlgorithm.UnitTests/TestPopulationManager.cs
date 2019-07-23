using FakeItEasy;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.UnitTests
{
    class TestPopulationManager
    {
        private readonly IPopulationGenerator populationGenerator = A.Fake<IPopulationGenerator>();
        private readonly IChildrenGenerator childrenGenerator = A.Fake<IChildrenGenerator>();
        private readonly int initialPopulationSize;

        public TestPopulationManager(double[][] populationEvaluation)
        {
            initialPopulationSize = populationEvaluation[0].Length;
            var initailPopulation = new IChromosome[initialPopulationSize];
            for (int i = 0; i < initailPopulation.Length; i++)
            {
                initailPopulation[i] = A.Fake<IChromosome>();
                A.CallTo(() => initailPopulation[i].ToString()).Returns("Initial Chromosome");
                A.CallTo(() => initailPopulation[i].Evaluate()).Returns(populationEvaluation[0][i]);
            }

            int index = 0;
            A.CallTo(() => populationGenerator.GeneratePopulation(A<int>._)).Returns(initailPopulation);
            A.CallTo(() => childrenGenerator.GenerateChildren(A<IChromosome[]>._, A<double[]>._)).ReturnsLazily(
                (IChromosome[] c, double[] d) =>
                {
                    index++;
                    return GetNextGeneration(populationEvaluation[index], index, "Gen");
                });
        }

        private IChromosome[] GetNextGeneration(double[] generationEvaluations, int index, string type)
        {
            var population = new IChromosome[generationEvaluations.Length];
            for (int i = 0; i < generationEvaluations.Length; i++)
            {
                var newChromosome = A.Fake<IChromosome>();
                var evaluation = generationEvaluations[i];
                A.CallTo(() => newChromosome.Evaluate()).Returns(evaluation);
                A.CallTo(() => newChromosome.ToString()).Returns($"{type}{index} (Eval={evaluation})");
                population[i] = newChromosome;
            }

            return population;
        }

        public TestPopulationManager(double[] populationEvaluation, bool repeat)
        {
            initialPopulationSize = populationEvaluation.Length;
            var initailPopulation = new IChromosome[initialPopulationSize];
            for (int i = 0; i < initailPopulation.Length; i++)
            {
                initailPopulation[i] = A.Fake<IChromosome>();
                A.CallTo(() => initailPopulation[i].ToString()).Returns("Initial Chromosome");
                A.CallTo(() => initailPopulation[i].Evaluate()).Returns(populationEvaluation[i]);
            }

            int index = 0;
            A.CallTo(() => populationGenerator.GeneratePopulation(A<int>._)).Returns(initailPopulation);
            A.CallTo(() => childrenGenerator.GenerateChildren(A<IChromosome[]>._, A<double[]>._)).ReturnsLazily(
                (IChromosome[] c, double[] d) =>
                {
                    index++;
                    
                    if (!repeat)
                        for (int i = 0; i < populationEvaluation.Length; i++)
                            populationEvaluation[i] = c[i].Evaluate();

                    return GetNextGeneration(populationEvaluation, index, "Gen");
                });
        }

        public void SetPopulationGenerated(double[][] populationEvaluation)
        {
            var initialPopulation = populationGenerator.GeneratePopulation(initialPopulationSize);

            var index = -2;
            A.CallTo(() => populationGenerator.GeneratePopulation(A<int>._)).ReturnsLazily((int s) =>
            {
                index++;
                if (index == -1) return initialPopulation;

                return GetNextGeneration(populationEvaluation[index], index, "Renewal");
            });
        }

        public IPopulationGenerator GetPopulationGenerator() => populationGenerator;

        public ICrossoverManager GetCrossoverManager() => A.Fake<ICrossoverManager>();

        public IChildrenGenerator GetChildrenGenerator() => childrenGenerator;
    }
}
