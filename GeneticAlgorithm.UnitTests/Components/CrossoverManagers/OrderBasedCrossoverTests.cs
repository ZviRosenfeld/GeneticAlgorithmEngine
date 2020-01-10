using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class OrderBassedCrossoverTests
    {
        private readonly ICrossoverManager crossoverManager = new OrderBasedCrossover<string>(null, null);

        [TestMethod]
        [DataRow(20)]
        public void OrderBasedCrossover_AllElementsInEachVector(int vectors)
        {
            crossoverManager.TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        public void OrderBasedCrossover_ChildChanged()
        {
            crossoverManager.TestThatChildChanged();
        }
    }
}
