using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class PopulationConverterTests
    {
        [TestMethod]
        public void ConvertPopulationViaManager_CheckPopulationConverted()
        {
            var testPopulationManager = new TestPopulationManager(new double[] {1, 1, 1, 1, 1});
            var populationConverter = A.Fake<IPopulationConverter>();
            var convertedPopulation = new double[] {2, 2, 2, 2, 2};
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._))
                .Returns(convertedPopulation.ToChromosomes("Converted Chromosomes"));
            var engine = new TestGeneticSearchEngineBuilder(convertedPopulation.Length, 10, testPopulationManager)
                .SetPopulationConverter(populationConverter).Build();

            var result = engine.Next();

            for (var i = 0; i < result.Population.GetChromosomes().Length; i++)
                Assert.AreEqual(convertedPopulation[i], result.Population.GetChromosomes()[i].Evaluate(), "Wrong chromosome");
        }

        [TestMethod]
        public void ConvertPopulationViaManager_CheckWeGetTheRightPopulationAndGeneration()
        {
            var generation = 0;
            var testPopulationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var populationConverter = A.Fake<IPopulationConverter>();
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._))
                .ReturnsLazily((IChromosome[] c, int g) =>
                {
                    generation++;
                    Assert.AreEqual(generation, g, "We got the wrong generation");
                    foreach (var chromosome in c)
                        Assert.AreEqual(1 , chromosome.Evaluate(), "Got wrong chromosome");
                    return c;
                });
            var engine = new TestGeneticSearchEngineBuilder(5, 10, testPopulationManager)
                .SetPopulationConverter(populationConverter).Build();

            for (int i = 0; i < 5; i++)
                engine.Next();
        }

        [TestMethod]
        public void ConvertPopulation_PopulationConverted()
        {
            var newPopulationEvaluation = new double[] {2, 2, 2};
            var populationManager = new TestPopulationManager(new double[] {1, 1, 1});
            var engine = new TestGeneticSearchEngineBuilder(3, 10, populationManager).Build();
            engine.Next();

            var newPopulation = engine.SetCurrentPopulation(newPopulationEvaluation.ToChromosomes("Converted"));
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);

            newPopulation = engine.GetCurrentPopulation();
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);
        }

        [TestMethod]
        public void ConvertPopulation_NextGetsRightPopulation()
        {
            var newPopulationEvaluation = new double[] { 2, 2, 2 };
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 }, c => c.Evaluate());
            var engine = new TestGeneticSearchEngineBuilder(3, 10, populationManager).Build();
            engine.Next();

            var newPopulation = engine.SetCurrentPopulation(newPopulationEvaluation.ToChromosomes("Converted"));
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);

            newPopulation = engine.Next();
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);
        }
    }
}
