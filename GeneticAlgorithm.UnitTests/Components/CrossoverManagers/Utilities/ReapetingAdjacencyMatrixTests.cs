using GeneticAlgorithm.Components.CrossoverManagers.Utilities;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers.Utilities
{
    [TestClass]
    public class ReapetingAdjacencyMatrixTests
    {
        [TestMethod]
        public void NeighborTest()
        {
            var adjacencyMatrix = GetAdjacencyMatrix();

            adjacencyMatrix.GetNeighbors(1).AssertContainSameElements(new[] { 2, 5, 4 });
            adjacencyMatrix.GetNeighbors(2).AssertContainSameElements(new[] { 1, 3, 4 });
            adjacencyMatrix.GetNeighbors(3).AssertContainSameElements(new[] { 2, 4, 5 });
            adjacencyMatrix.GetNeighbors(4).AssertContainSameElements(new[] { 3, 5, 2, 1 });
            adjacencyMatrix.GetNeighbors(5).AssertContainSameElements(new[] { 3, 1, 4 });
        }

        [TestMethod]
        public void GetNeigbor_OriginalElementNotRemoved()
        {
            var adjacencyMatrix = GetAdjacencyMatrix();
            adjacencyMatrix.GetNeighbor(1);

            Assert.IsTrue(adjacencyMatrix.ContainsElement(1));
            adjacencyMatrix.GetNeighbors(1).AssertContainSameElements(new[] { 2, 5, 4 });
            adjacencyMatrix.GetNeighbors(2).AssertContainSameElements(new[] { 1, 3, 4 });
            adjacencyMatrix.GetNeighbors(3).AssertContainSameElements(new[] { 2, 4, 5 });
            adjacencyMatrix.GetNeighbors(4).AssertContainSameElements(new[] { 3, 5, 2, 1 });
            adjacencyMatrix.GetNeighbors(5).AssertContainSameElements(new[] { 3, 1, 4 });
        }

        [TestMethod]
        public void GetNeigbor_SelectRandomNeighbor_Test1()
        {
            var adjacencyMatrix = GetAdjacencyMatrix();
            var neighbor = adjacencyMatrix.GetNeighbor(1);
            Assert.IsTrue(neighbor == 2 || neighbor == 4 || neighbor == 5);
        }
        
        [TestMethod]
        public void GetNeigbor_GetRandomNeigbor()
        {
            // This test checks that we don't get the same neigbor over and over again.
            int got2 = 0, got4 = 0, got5 = 0;
            for (int i = 0; i < 100; i++)
            {
                var adjacencyMatrix = GetAdjacencyMatrix();
                var neighbor = adjacencyMatrix.GetNeighbor(1);
                if (neighbor == 2) got2++;
                else if (neighbor == 5) got5++;
                else if (neighbor == 4) got4++;
                else Assert.Fail("We didn't get 2, 4 or 5. Got " + neighbor);
            }

            Assert.IsTrue(25 < got2 , $"Got 2 {got2} times");
            Assert.IsTrue(25 < got5, $"Got 5 {got5} times");
            Assert.IsTrue(25 < got4, $"Got 4 {got4} times");
        }

        [TestMethod]
        public void CreateAdjacencyMatrixWithArrayOfOneElement()
        {
            new ReapetingAdjacencyMatrix<int>(new[] { 1 }, new[] { 1 });
        }

        [TestMethod]
        public void CreateAdjacencyMatrixWithArrayOfTwoElements()
        {
            new ReapetingAdjacencyMatrix<int>(new[] { 1, 2 }, new[] { 1, 2 });
        }

        /// <summary>
        /// Creates an AdjacencyMatrix from the arrays {1, 2, 3, 4, 5} and {1, 5, 3, 2, 4}
        /// </summary>
        /// <returns></returns>
        private static ReapetingAdjacencyMatrix<int> GetAdjacencyMatrix()
        {
            var array1 = new[] { 1, 2, 3, 4, 5 };
            var array2 = new[] { 1, 5, 3, 2, 4 };

            return new ReapetingAdjacencyMatrix<int>(array1, array2);
        }
    }
}
