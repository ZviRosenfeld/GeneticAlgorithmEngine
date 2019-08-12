using System.Collections.Generic;
using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

        public static void AssertHasEvaluation(this List<IChromosome[]> chromosomes, double[][] evaluations)
        {
            for (int i = 0; i < chromosomes.Count; i++)
                chromosomes[i].AssertHasEvaluation(evaluations[i]);
        }

        public static void AssertHasEvaluation(this IChromosome[] chromosomes, double[] evaluations)
        {
            for (int i = 0; i < chromosomes.Length; i++)
                Assert.AreEqual(evaluations[i], chromosomes[i].Evaluate());
        }

        public static void AssertAreTheSame(this List<double[]> collection1, double[][] collection2)
        {
            for (int i = 0; i < collection1.Count; i++)
                collection1[i].AssertAreTheSame(collection2[i]);
        }

        public static void AssertAreTheSame(this double[] collection1, double[] collection2)
        {
            for (int i = 0; i < collection1.Length; i++)
                Assert.AreEqual(collection1[i], collection2[i]);
        }

        public static void AssertAreTheSame(GeneticSearchResult result1, GeneticSearchResult result2)
        {
            Assert.AreEqual(result1.Generations, result2.Generations, "Diffrent number of generations");
            Assert.AreEqual(result1.IsCompleted, result2.IsCompleted, "Diffrent 'IsComplate' value");
            Assert.AreEqual(result1.SearchTime, result2.SearchTime, "Diffrent searchTime");
            result1.Population.AssertIsSame(result2.Population);

            for (int i = 0 ; i < result1.History.Count; i++)
                result1.History[i].AssertIsSame(result2.History[i]);
        }

        private static void AssertIsSame(this Population population1, Population population2)
        {
            for (int i = 0; i < population1.GetChromosomes().Length; i++)
                Assert.AreEqual(population1[i].Evaluation, population2[i].Evaluation); 
        }

        private static void AssertIsSame(this IChromosome[] population1, IChromosome[] population2)
        {
            for (int i = 0; i < population1.Length; i++)
                Assert.AreEqual(population1[i].Evaluate(), population2[i].Evaluate());
        }
    }
}
