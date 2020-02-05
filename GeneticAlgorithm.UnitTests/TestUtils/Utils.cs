using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.UnitTests.TestUtils
{
    static class Utils
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

        public static  Population Evaluate(this Population population)
        {
            foreach (var chromosome in population)
                chromosome.Evaluation = chromosome.Chromosome.Evaluate();

            return population;
        }

        public static GeneticSearchEngine GetBassicEngine()
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var engineBuilder = new TestGeneticSearchEngineBuilder(5, int.MaxValue, populationManager);
            return engineBuilder.Build();
        }

        /// <summary>
        /// Converts a value that we know is a double or an int to an int.
        /// </summary>
        public static int ToInt<T>(this T value)
        {
            var intGenome = value as int?;
            var doubleGenoe = value as double?;
            return intGenome ?? (int) doubleGenoe.Value;
        }

        /// <summary>
        /// Works on chromosomes of type VectorChromosome. Returns their vector.
        /// </summary>
        public static T[] ToArray<T>(this IChromosome chromosome) =>
            ((VectorChromosome<T>)chromosome).GetVector();
    }
}
