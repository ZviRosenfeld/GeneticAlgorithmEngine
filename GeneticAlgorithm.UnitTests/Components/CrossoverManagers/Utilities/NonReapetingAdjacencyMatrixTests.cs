using GeneticAlgorithm.Components.CrossoverManagers.Utilities;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers.Utilities
{
    [TestClass]
    public class NonReapetingAdjacencyMatrixTests
    {
        [TestMethod]
        public void NeighborTest()
        {
            var adjacencyMatrix = GetAdjacencyMatrix(false);

            adjacencyMatrix.GetNeighbors(1).AssertContainSameElements(new[] {2, 5, 4});
            adjacencyMatrix.GetNeighbors(2).AssertContainSameElements(new[] { 1, 3, 4 });
            adjacencyMatrix.GetNeighbors(3).AssertContainSameElements(new[] { 2, 4, 5 });
            adjacencyMatrix.GetNeighbors(4).AssertContainSameElements(new[] { 3, 5, 2, 1 });
            adjacencyMatrix.GetNeighbors(5).AssertContainSameElements(new[] { 3, 1, 4 });
        }

        [TestMethod]
        public void GetNeigbor_OriginalElementRemoved()
        {
            var adjacencyMatrix = GetAdjacencyMatrix(false);
            adjacencyMatrix.GetNeighbor(1);

            Assert.IsFalse(adjacencyMatrix.ContainsElement(1));
            adjacencyMatrix.GetNeighbors(2).AssertContainSameElements(new[] { 3, 4 });
            adjacencyMatrix.GetNeighbors(3).AssertContainSameElements(new[] { 2, 4, 5 });
            adjacencyMatrix.GetNeighbors(4).AssertContainSameElements(new[] { 3, 5, 2});
            adjacencyMatrix.GetNeighbors(5).AssertContainSameElements(new[] { 3, 4 });
        }

        [TestMethod]
        public void GetNeigbor_SelectRandomNeighbor_Test1()
        {
            var adjacencyMatrix = GetAdjacencyMatrix(false);
            var neighbor = adjacencyMatrix.GetNeighbor(1);
            Assert.IsTrue(neighbor == 2 || neighbor == 4 || neighbor == 5);
        }

        [TestMethod]
        public void GetNeigbor_SelectNeighborWithLeastNeighbors_Test1()
        {
            var adjacencyMatrix = GetAdjacencyMatrix(true);
            var neighbor = adjacencyMatrix.GetNeighbor(1);
            Assert.IsTrue(neighbor == 2 || neighbor == 5);
        }

        [TestMethod]
        public void GetNeigbor_SelectNeighborWithLeastNeighbors_Test2()
        {
            var array1 = new[] { 2, 1, 3, 4, 5 };
            var array2 = new[] { 1, 2, 3, 4, 5 };

            var adjacencyMatrix = new NonReapetingAdjacencyMatrix<int>(array1, array2, true);
            var neighbor = adjacencyMatrix.GetNeighbor(3);
            Assert.IsTrue(neighbor == 4);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void GetNeigborForElementWithNoNeighbors_ReturnRandomElement(bool selectNeighborWithLeastNeighbors)
        {
            var array1 = new[] {1, 2, 3, 4, 5};
            var array2 = new[] {1, 2, 3, 4, 5};

            int got4 = 0, got5 = 0;
            for (int i = 0; i < 100; i++)
            {
                var adjacencyMatrix = new NonReapetingAdjacencyMatrix<int>(array1, array2, selectNeighborWithLeastNeighbors);
                adjacencyMatrix.GetNeighbor(1);
                adjacencyMatrix.GetNeighbor(3);

                var neighbor = adjacencyMatrix.GetNeighbor(2);
                if (neighbor == 4) got4++;
                else if (neighbor == 5) got5++;
                else Assert.Fail("We didn't get 4 or 5. Got " + neighbor);
            }

            Assert.IsTrue(35 < got4);
            Assert.IsTrue(35 < got4);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void GetNeigbor_GetRandomNeigbor(bool selectNeighborWithLeastNeighbors)
        {
            // This test checks that we don't get the same neighbor over and over again.
            // If selectNeighborWithLeastNeighbors = true, we expect to get neighbors 2 and 5 and not 4 - since 4 has more neighbors than 2 and 5.
            int got2 = 0, got4 = 0, got5 = 0;
            for (int i = 0; i < 100; i++)
            {
                var adjacencyMatrix = GetAdjacencyMatrix(selectNeighborWithLeastNeighbors);
                var neighbor = adjacencyMatrix.GetNeighbor(1);
                if (neighbor == 2) got2++;
                else if (neighbor == 5) got5++;
                else if (neighbor == 4 && !selectNeighborWithLeastNeighbors) got4++;
                else Assert.Fail("We didn't get 4 or 5. Got " + neighbor);
            }

            var excpectedToGetAtLeast = selectNeighborWithLeastNeighbors ? 35 : 25;
            Assert.IsTrue(excpectedToGetAtLeast < got2, $"Got 2 only {got2} times");
            Assert.IsTrue(excpectedToGetAtLeast < got5, $"Got 5 only {got5} times");
            if (!selectNeighborWithLeastNeighbors)
                Assert.IsTrue(excpectedToGetAtLeast < got4, $"Got 4 only {got4} times");
        }

        [TestMethod]
        public void CreateAdjacencyMatrixWithArrayOfOneElement()
        {
            new NonReapetingAdjacencyMatrix<int>(new []{1}, new []{1}, false);
        }

        [TestMethod]
        public void CreateAdjacencyMatrixWithArrayOfTwoElements()
        {
            new NonReapetingAdjacencyMatrix<int>(new[] { 1, 2 }, new[] { 1, 2 }, false);
        }

        /// <summary>
        /// Creates an AdjacencyMatrix from the arrays {1, 2, 3, 4, 5} and {1, 5, 3, 2, 4}
        /// </summary>
        /// <returns></returns>
        private static NonReapetingAdjacencyMatrix<int> GetAdjacencyMatrix(bool selectNeighborWithLeastNeighbors)
        {
            var array1 = new[] {1, 2, 3, 4, 5};
            var array2 = new[] {1, 5, 3, 2, 4};

            return new NonReapetingAdjacencyMatrix<int>(array1, array2, selectNeighborWithLeastNeighbors);
        }
    }
}
