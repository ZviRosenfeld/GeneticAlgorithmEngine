using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components
{
    [TestClass]
    public class PopulationGeneratorsTests
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
            var vector = (VectorChromosome<int>) populationGenerator.GeneratePopulation(1).First();
            Assert.AreEqual(chromosomeSize, vector.GetVector().Length);
        }

        [TestMethod]
        [DataRow(-10, 10)]
        public void IntVectorChromosomePopulationGenerator_CreatesChromosomeOfWithRightValues(int minGenome, int maxGenome)
        {
            var populationGenerator =
                new IntVectorChromosomePopulationGenerator(10, minGenome, maxGenome, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            TestChromosomes<int>(minGenome, maxGenome, populationGenerator);
        }

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
            TestChromosomes<double>(minGenome + 1, maxGenome - 1, populationGenerator);
        }

        [TestMethod]
        public void BinaryVectorChromosomePopulationGenerator_AllValuesCreated()
        {
            var populationGenerator =
                new BinaryVectorChromosomePopulationGenerator(5, A.Fake<IMutationManager<bool>>(), A.Fake<IEvaluator>());
            var vector = ((VectorChromosome<bool>) populationGenerator.GeneratePopulation(1).First()).GetVector();
            Assert.IsTrue(vector.Contains(true), "Vector dosn't contain true");
            Assert.IsTrue(vector.Contains(false), "Vector dosn't contain false");
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

        /// <summary>
        /// Test that the genomes in the chromosome are within range, and that they are dispersed.
        /// </summary>
        private static void TestChromosomes<T>(int minGenome, int maxGenome, IPopulationGenerator populationGenerator)
        {
            var chromosomes = populationGenerator.GeneratePopulation(10);
            var recivedGenomes = new HashSet<int>();
            foreach (var chromosome in chromosomes)
            {
                var vectorChromosome = (VectorChromosome<T>)chromosome;
                foreach (var genome in vectorChromosome.GetVector())
                {
                    var intGenome = genome.ToInt();
                    Assert.IsTrue(intGenome >= minGenome, $"Got a genome smaller than {minGenome} ({intGenome})");
                    Assert.IsTrue(intGenome <= maxGenome, $"Got a genome bigger than {maxGenome} ({intGenome})");
                    recivedGenomes.Add(intGenome);
                }
            }
            for (int i = minGenome; i < maxGenome; i++)
                Assert.IsTrue(recivedGenomes.Contains(i), $"Didn't get {i}");
        }
    }
}
