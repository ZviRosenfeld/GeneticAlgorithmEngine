using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.PopulationGenerators
{
    [TestClass]
    public class AllElementsVectorChromosomePopulationGeneratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void AllElementsVectorChromosomePopulationGenerator_EmptyElementList_ThrowException() =>
            new AllElementsVectorChromosomePopulationGenerator<int>(new List<int>(), null, null);

        [TestMethod]
        [DataRow(20)]
        public void AllElementsVectorChromosomePopulationGenerator_AllElementsInEachVector(int vectors)
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements,
                A.Fake<IMutationManager<string>>(), A.Fake<IEvaluator>());
            var chromosomes = generator.GeneratePopulation(vectors);
            foreach (var chromosome in chromosomes)
                ((VectorChromosome<string>)chromosome).AssertContainSameElements(elements);
        }

        [TestMethod]
        public void AllElementsVectorChromosomePopulationGenerator_ElementsScattered()
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements,
                A.Fake<IMutationManager<string>>(), A.Fake<IEvaluator>());
            var chromosomes = generator.GeneratePopulation(1);
            ((VectorChromosome<string>)chromosomes.First()).AssertAreNotTheSame(elements);
        }
    }
}
