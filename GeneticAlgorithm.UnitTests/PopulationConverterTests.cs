using System;
using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class PopulationConverterTests
    {
        [TestMethod]
        public void ConvertPopulation_CheckPopulationConverted()
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
    }
}
