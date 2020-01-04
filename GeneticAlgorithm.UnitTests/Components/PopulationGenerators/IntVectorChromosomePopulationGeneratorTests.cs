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
    public class IntVectorChromosomePopulationGeneratorTests
    {
        [TestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void IntVectorChromosomePopulationGenerator_BadChromosomeSize_ThrowException(int chromosomeSize) =>
            new IntVectorChromosomePopulationGenerator(chromosomeSize, 0, 10, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void IntVectorChromosomePopulationGenerator_MinGenomeSmallerThanMaxGenome_ThrowException() =>
            new IntVectorChromosomePopulationGenerator(10, 10, 0, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());

        [TestMethod]
        [DataRow(1)]
        [DataRow(10)]
        public void IntVectorChromosomePopulationGenerator_CreatesChromosomeOfRightSize(int chromosomeSize)
        {
            var populationGenerator =
                new IntVectorChromosomePopulationGenerator(chromosomeSize, 0, 10, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            var vector = (VectorChromosome<int>)populationGenerator.GeneratePopulation(1).First();
            Assert.AreEqual(chromosomeSize, vector.GetVector().Length);
        }

        [TestMethod]
        [DataRow(-10, 10)]
        public void IntVectorChromosomePopulationGenerator_CreatesChromosomeOfWithRightValues(int minGenome, int maxGenome)
        {
            var populationGenerator =
                new IntVectorChromosomePopulationGenerator(10, minGenome, maxGenome, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            populationGenerator.TestChromosomes<int>(minGenome, maxGenome);
        }
    }
}
