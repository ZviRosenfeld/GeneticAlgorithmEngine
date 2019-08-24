using FakeItEasy;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.UnitTests.TestUtils
{
    public static class ChromosomeFactory
    {
        public static IChromosome[] ToChromosomes(this double[] generationEvaluations, string tag = "Chromo", int? number = null)
        {
            var size = number ?? generationEvaluations.Length;
            var population = new IChromosome[size];
            for (int i = 0; i < size; i++)
                population[i] = CreateChromosome(generationEvaluations[i], tag);

            return population;
        }

        public static IChromosome CreateChromosome(this double evaluation, string tag)
        {
            var newChromosome = A.Fake<IChromosome>();
            A.CallTo(() => newChromosome.Evaluate()).Returns(evaluation);
            A.CallTo(() => newChromosome.ToString()).Returns($"{tag} (Eval={evaluation})");
            return newChromosome;
        }
    }
}
