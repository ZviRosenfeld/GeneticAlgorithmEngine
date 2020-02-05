using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.PopulationGenerators
{
    [TestClass]
    public class FromElementsVectorChromosomePopulationGeneratorTests
    {
        private readonly string[] allElements = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"};

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void AllElementsVectorChromosomePopulationGenerator_EmptyElementList_ThrowException() =>
            new FromElementsVectorChromosomePopulationGenerator<string>(new string[] { }, 0, false, null, null);

        [TestMethod]
        [DataRow(-1, true)]
        [DataRow(0, true)]
        [DataRow(-1, false)]
        [DataRow(0, false)]
        [DataRow(11, false)]
        [DataRow(20, false)]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void AllElementsVectorChromosomePopulationGenerator_BadLength_ThrowException(int length, bool repeate) =>
            new FromElementsVectorChromosomePopulationGenerator<string>(allElements, length, repeate, null, null);

        [TestMethod]
        [DataRow(1, true)]
        [DataRow(5, true)]
        [DataRow(10, true)]
        [DataRow(1, false)]
        [DataRow(5, false)]
        [DataRow(10, false)]
        public void AllElementsVectorChromosomePopulationGenerator_RightElementsSelected(int vectorLength, bool repeate)
        {
            var populationGenerator = GetPopulationGenerator(vectorLength, repeate);
            var population = populationGenerator.GeneratePopulation(3);

            foreach (var chromosome in population)
            {
                var vector = chromosome.ToArray<string>();
                Assert.AreEqual(vectorLength, vector.Length, "Vector isn't right length");

                if (!repeate)
                    foreach (var element in vector)
                        Assert.AreEqual(1, vector.Count(n => n == element), "Found the same element more then once");
                foreach (var element in vector)
                    Assert.IsTrue(allElements.Contains(element),
                        $"Got {nameof(element)} {element} which isn't in {nameof(allElements)}");
            }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void AllElementsVectorChromosomePopulationGenerator_ElementsAreRandom(bool repeate)
        {
            var populationSize = allElements.Length * 5;
            var populationGenerator = GetPopulationGenerator(2, repeate);
            var population = populationGenerator.GeneratePopulation(populationSize);
            var slelectedElements = new List<string>();
            foreach (var chromosome in population)
                slelectedElements.AddRange(chromosome.ToArray<string>());

            // All elements are generated
            for (int i = 0; i < allElements.Length; i++)
                Assert.IsTrue(slelectedElements.Contains(allElements[i]),
                    $"{nameof(slelectedElements)} doesn't contain {slelectedElements[i]}");

            // No element is generated more than 20% of the time
            foreach (var element in slelectedElements)
                Assert.IsTrue(slelectedElements.Count(e => e.Equals(element)) < populationSize * 2 * 0.2,
                    $"{nameof(element)} {element} selected too many times");
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(12)]
        public void AllElementsVectorChromosomePopulationGenerator_RepeateElements(int length)
        {
            var populationGenerator = GetPopulationGenerator(length, true);
            var population = populationGenerator.GeneratePopulation(1);
            var chromosome = population.First();
            var vector = chromosome.ToArray<string>();

            Assert.IsTrue(vector.Any(e => (vector.Count(e2 => e2.Equals(e))) > 1));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void AllElementsVectorChromosomePopulationGenerator_ElementsScattered(bool repeate)
        {
            var populationGenerator = GetPopulationGenerator(7, repeate);
            var population = populationGenerator.GeneratePopulation(1);
            var chromosome = population.First();
            Assert.IsFalse(AreSameOrder(allElements, chromosome.ToArray<string>()));
        }

        private FromElementsVectorChromosomePopulationGenerator<string> GetPopulationGenerator(int vectorLength, bool repeateElements) =>
            new FromElementsVectorChromosomePopulationGenerator<string>(allElements, vectorLength, repeateElements, null, null);

        /// <summary>
        /// Checks if the elements in copyedArray appear in the same order as they did in mainArray
        /// </summary>
        private bool AreSameOrder<T>(T[] mainArray, T[] copiedArray)
        {
            for (int mainArrayIndex = 0, copiedArrayIndex = 0; copiedArrayIndex < copiedArray.Length;)
            {
                if (mainArrayIndex >= mainArray.Length)
                    return false;
                if (copiedArrayIndex >= copiedArray.Length)
                    return true;
                if (!copiedArray[copiedArrayIndex].Equals(mainArray[mainArrayIndex]))
                    mainArrayIndex++;
                else
                    copiedArrayIndex++;
            }

            throw new GeneticAlgorithmException("We should never get here");
        }
    }
}
