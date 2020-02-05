using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.CrossoverManagers.Utilities;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers.Utilities
{
    [TestClass]
    public class UtilsTests
    {
        [TestMethod]
        public void EdgeRecombinationCrossover_GetNeighborsAsDefinedByTheAdjacencyMatrix()
        {
            var expectedChildArray = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var adjacencyMatrix = A.Fake<IAdjacencyMatrix<string>>();
            A.CallTo(() => adjacencyMatrix.GetNeighbor(A<string>._)).ReturnsNextFromSequence(expectedChildArray.Skip(1).ToArray());
            var child = adjacencyMatrix.Crossover(expectedChildArray.First(), 10);

            child.AssertAreTheSame(expectedChildArray);
        }
    }
}
