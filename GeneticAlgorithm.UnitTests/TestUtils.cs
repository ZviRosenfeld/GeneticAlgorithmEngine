using FakeItEasy;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.UnitTests
{
    static class TestUtils
    {
        public static GeneticSearchResult Run(this GeneticSearchEngine engine, RunType runType)
        {
            if (runType == RunType.Run)
                return engine.Run();

            GeneticSearchResult result = null;
            while (result == null || !result.IsCompleted)
                result = engine.Next();

            return result;
        }

        public static IChromosome[] ToChromosomes(this double[] generationEvaluations, string tag, int? number = null)
        {
            var size = number ?? generationEvaluations.Length;
            var population = new IChromosome[size];
            for (int i = 0; i < size; i++)
            {
                var newChromosome = A.Fake<IChromosome>();
                var evaluation = generationEvaluations[i];
                A.CallTo(() => newChromosome.Evaluate()).Returns(evaluation);
                A.CallTo(() => newChromosome.ToString()).Returns($"{tag} (Eval={evaluation})");
                population[i] = newChromosome;
            }

            return population;
        }
    }
}
