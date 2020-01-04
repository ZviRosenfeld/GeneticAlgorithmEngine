using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.PopulationGenerators
{
    [TestClass]
    public class DoubleVectorChromosomePopulationGeneratorTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void DoubleVectorChromosomePopulationGenerator_BadChromosomeSize_ThrowException(int chromosomeSize) =>
            new DoubleVectorChromosomePopulationGenerator(chromosomeSize, 0, 10, A.Fake<IMutationManager<double>>(),
                A.Fake<IEvaluator>());

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void DoubleVectorChromosomePopulationGenerator_MinGenomeSmallerThanMaxGenome_ThrowException() =>
            new DoubleVectorChromosomePopulationGenerator(10, 10, 0, A.Fake<IMutationManager<double>>(),
                A.Fake<IEvaluator>());

        [TestMethod]
        [DataRow(1)]
        [DataRow(10)]
        public void DoubleVectorChromosomePopulationGenerator_CreatesChromosomeOfRightSize(int chromosomeSize)
        {
            var populationGenerator =
                new DoubleVectorChromosomePopulationGenerator(chromosomeSize, 0, 10, A.Fake<IMutationManager<double>>(), A.Fake<IEvaluator>());
            var vector = (VectorChromosome<double>)populationGenerator.GeneratePopulation(1).First();
            Assert.AreEqual(chromosomeSize, vector.GetVector().Length);
        }

        [TestMethod]
        [DataRow(-10, 10)]
        public void DoubleVectorChromosomePopulationGenerator_CreatesChromosomeOfWithRightValues(int minGenome, int maxGenome)
        {
            var populationGenerator =
                new DoubleVectorChromosomePopulationGenerator(10, minGenome, maxGenome, A.Fake<IMutationManager<double>>(), A.Fake<IEvaluator>());
            populationGenerator.TestChromosomes<double>(minGenome + 1, maxGenome - 1);
        }
    }
}
