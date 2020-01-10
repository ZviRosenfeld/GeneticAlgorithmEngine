using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class PositionBasedCrossoverManagerTests
    {
        private readonly ICrossoverManager crossoverManager = new PositionBasedCrossoverManager<string>(null, null);

        [TestMethod]
        [DataRow(20)]
        public void PositionBasedCrossover_AllElementsInEachVector(int vectors)
        {
            crossoverManager.TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        public void PositionBasedCrossover_ChildChanged()
        {
            crossoverManager.TestThatChildChanged();
        }
    }
}
