using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class OrderCrossoverTests
    {
        [TestMethod]
        [DataRow(20)]
        public void OrderCrossover_AllElementsInEachVector(int vectors)
        {
            new OrderCrossover<string>(null, null).TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        public void OrderCrossover_ChildChanged()
        {
            new OrderCrossover<string>(null, null).TestThatChildChanged();
        }
    }
}
