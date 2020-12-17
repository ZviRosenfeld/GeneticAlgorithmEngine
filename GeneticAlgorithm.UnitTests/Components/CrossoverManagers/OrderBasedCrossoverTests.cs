using GeneticAlgorithm.Components.CrossoverManagers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class OrderBassedCrossoverTests
    {
        [TestMethod]
        [DataRow(20)]
        public void OrderBasedCrossover_AllElementsInEachVector(int vectors)
        {
            new OrderBasedCrossover<string>(null, null).TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        public void OrderBasedCrossover_ChildChanged()
        {
            new OrderBasedCrossover<string>(null, null).TestThatChildChanged();
        }
    }
}
