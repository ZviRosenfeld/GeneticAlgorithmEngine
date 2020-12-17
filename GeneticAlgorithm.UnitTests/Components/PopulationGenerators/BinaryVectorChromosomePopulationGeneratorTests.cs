using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.PopulationGenerators
{
    [TestClass]
    public class BinaryVectorChromosomePopulationGeneratorTests
    {
        [TestMethod]
        public void BinaryVectorChromosomePopulationGenerator_AllValuesCreated()
        {
            var populationGenerator =
                new BinaryVectorChromosomePopulationGenerator(5, A.Fake<IMutationManager<bool>>(), A.Fake<IEvaluator>());
            var vector = ((VectorChromosome<bool>)populationGenerator.GeneratePopulation(1).First()).GetVector();
            Assert.IsTrue(vector.Contains(true), "Vector doesn't contain true");
            Assert.IsTrue(vector.Contains(false), "Vector doesn't contain false");
        }

        [TestMethod]
        public void BinaryVectorChromosomePopulationGenerator_VectorsAreRightSize()
        {
            var vecotrSize = 5;
            var populationGenerator =
                new BinaryVectorChromosomePopulationGenerator(vecotrSize, A.Fake<IMutationManager<bool>>(), A.Fake<IEvaluator>());
            var vector = ((VectorChromosome<bool>)populationGenerator.GeneratePopulation(1).First()).GetVector();
            Assert.AreEqual(vecotrSize, vector.Length);
        }
    }
}
