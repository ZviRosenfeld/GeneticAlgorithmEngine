using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Exceptions;
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
        public void VectorChromosomePopulationGenerator_BadChromosomeSize_ThrowException(int chromosomeSize) =>
            new IntVectorChromosomePopulationGenerator(chromosomeSize, 0, 10, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());

        [TestMethod]
        [ExpectedException(typeof(GeneticAlgorithmException))]
        public void VectorChromosomePopulationGenerator_MinGenomeSmallerThanMaxGenome_ThrowException() =>
            new IntVectorChromosomePopulationGenerator(10, 10, 0, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());

        [TestMethod]
        [DataRow(1)]
        [DataRow(10)]
        public void VectorChromosomePopulationGenerator_CreatesChromosomeOfRightSize(int chromosomeSize)
        {
            var populationGenerator =
                new IntVectorChromosomePopulationGenerator(chromosomeSize, 0, 10, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            var vector = (VectorChromosome<int>) populationGenerator.GeneratePopulation(1).First();
            Assert.AreEqual(chromosomeSize, vector.GetVector().Length);
        }

        [TestMethod]
        [DataRow(-10, 10)]
        public void VectorChromosomePopulationGenerator_CreatesChromosomeOfWithRightValues(int minGenome, int maxGenome)
        {
            var populationGenerator =
                new IntVectorChromosomePopulationGenerator(10, minGenome, maxGenome, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            var chromosomes = populationGenerator.GeneratePopulation(1000);
            foreach (var chromosome in chromosomes)
            {
                var vectorChromosome = (VectorChromosome<int>) chromosome;
                foreach (var genome in vectorChromosome.GetVector())
                {
                    Assert.IsTrue(genome >= minGenome, $"Got a genome smaller than {minGenome}");
                    Assert.IsTrue(genome <= maxGenome, $"Got a genome bigger than {maxGenome}");
                }
            }
        }

        [TestMethod]
        public void BinaryVectorChromosomePopulationGenerator_CreatesChromosomeOfWithRightValues()
        {
            var populationGenerator =
                new BinaryVectorChromosomePopulationGenerator(10, A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            var chromosomes = populationGenerator.GeneratePopulation(1000);
            foreach (var chromosome in chromosomes)
            {
                var vectorChromosome = (VectorChromosome<int>)chromosome;
                foreach (var genome in vectorChromosome.GetVector())
                {
                    Assert.IsTrue(genome >= 0, $"Got a genome smaller than {0}");
                    Assert.IsTrue(genome <= 1, $"Got a genome bigger than {1}");
                }
            }
        }
    }
}
