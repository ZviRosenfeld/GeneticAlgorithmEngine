using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class PositionBasedCrossoverManagerTests
    {
        [TestMethod]
        [DataRow(20)]
        public void PositionBasedCrossover_AllElementsInEachVector(int vectors)
        {
            new PositionBasedCrossoverManager<string>(null, null).TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        public void PositionBasedCrossover_ChildChanged()
        {
            new PositionBasedCrossoverManager<string>(null, null).TestThatChildChanged();
        }
    }
}
