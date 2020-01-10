using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class CycleCrossoverManagerTests
    {
        private readonly ICrossoverManager crossoverManager = new CycleCrossoverManager<string>(null, null);

        [TestMethod]
        public void CycleCrossoverTest1()
        {
            var parent1 = new[] {"A", "B", "C", "D", "E"};
            var parent2 = new[] {"D", "E", "C", "A", "B"};
            var expectedChild = new[] {"A", "E", "C", "D", "B"};

            TestCycleCrossover(parent1, parent2, expectedChild);
        }

        [TestMethod]
        public void CycleCrossoverTest2()
        {
            var parent1 = new[] { "A", "B", "C", "D", "E" };
            var parent2 = new[] { "A", "B", "C", "D", "E" };
            var expectedChild = new[] { "A", "B", "C", "D", "E" };

            TestCycleCrossover(parent1, parent2, expectedChild);
        }

        [TestMethod]
        public void CycleCrossoverTest3()
        {
            var parent1 = new[] { "A", "B", "C", "D", "E" };
            var parent2 = new[] { "D", "A", "E", "C", "B" };
            var expectedChild = new[] { "A", "B", "C", "D", "E" };

            TestCycleCrossover(parent1, parent2, expectedChild);
        }

        [TestMethod]
        public void CycleCrossoverTest()
        {
            var parent1 = new[] { "A", "B", "C", "D", "E", "F" };
            var parent2 = new[] { "D", "E", "F", "A", "B", "C" };
            var expectedChild = new[] { "A", "E", "C", "D", "B", "F" };

            TestCycleCrossover(parent1, parent2, expectedChild);
        }

        [TestMethod]
        [DataRow(20)]
        public void CycleCrossover_AllElementsInEachVector(int vectors)
        {
            crossoverManager.TestThatAllElementsInEachVector(vectors);
        }
        
        private void TestCycleCrossover(string[] parent1, string[] parent2, string[] expectedChild)
        {
            var chromosome1 = new VectorChromosome<string>(parent1, null, null);
            var chromosome2 = new VectorChromosome<string>(parent2, null, null);

            var child = (VectorChromosome<string>)crossoverManager.Crossover(chromosome1, chromosome2);
            child.AssertAreTheSame(expectedChild);
        }
    }
}
