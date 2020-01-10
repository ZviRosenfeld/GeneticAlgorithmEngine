using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class PartiallyMappedCrossoverTests
    {
        private readonly ICrossoverManager crossoverManager = new PartiallyMappedCrossover<string>(null, null);

        [TestMethod]
        [DataRow(20)]
        public void PartiallyMatchedCrossover_AllElementsInEachVector(int vectors)
        {
            crossoverManager.TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        public void PartiallyMatchedCrossover_ChildChanged()
        {
            crossoverManager.TestThatChildChanged();
        }
    }
}
