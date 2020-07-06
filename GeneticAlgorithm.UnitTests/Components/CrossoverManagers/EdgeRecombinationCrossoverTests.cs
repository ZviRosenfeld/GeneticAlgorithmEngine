using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    /// <summary>
    /// This class tests EdgeRecombinationCrossover and HeuristicCrossover - 
    /// since they use almost the same algorithm.
    /// </summary>
    [TestClass]
    public class EdgeRecombinationCrossoverTests
    {
        [TestMethod]
        [DataRow(20)]
        [DataRow(20)]
        public void EdgeRecombinationCrossover_AllElementsInEachVector(int vectors)
        {
            new EdgeRecombinationCrossover<string>(null, null).TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(20)]
        public void HeuristicCrossover_AllElementsInEachVector(int vectors)
        {
            new HeuristicCrossover<string>(null, null).TestThatAllElementsInEachVector(vectors);
        }

        [TestMethod]
        public void EdgeRecombinationCrossover_ChildChanged()
        {
            new EdgeRecombinationCrossover<string>(null, null).TestThatChildChanged();
        }

        [TestMethod]
        public void HeuristicCrossover_ChildChanged()
        {
            new HeuristicCrossover<string>(null, null).TestThatChildChanged();
        }

        [TestMethod]
        public void EdgeRecombinationCrossover_NextNodeIsNeighborsOfPreviousNode()
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            var parentChromosomes = generator.GeneratePopulation(2);

            var child = new EdgeRecombinationCrossover<string>(null, null).Crossover(parentChromosomes.ElementAt(0), parentChromosomes.ElementAt(1)).ToArray<string>();
            
            for (int i = 0; i < child.Length - 1; i++)
            {
                var neigbors = GetNeighbors(parentChromosomes.ElementAt(0).ToArray<string>(),
                    parentChromosomes.ElementAt(1).ToArray<string>(), child[i]);
                RemoveAlreadyVisitedNeigbors(neigbors, child, i);
                if (neigbors.Count == 0)
                    continue;

                Assert.IsTrue(neigbors.Contains(child[i + 1]), "Didn't jump to neighbor");
            }
        }

        [TestMethod]
        public void HeuristicCrossover_NextNodeIsNeighborsOfPreviousNode()
        {
            var elements = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new AllElementsVectorChromosomePopulationGenerator<string>(elements, null, null);
            var parentChromosomes = generator.GeneratePopulation(2);

            var child = new HeuristicCrossover<string>(null, null).Crossover(parentChromosomes.ElementAt(0), parentChromosomes.ElementAt(1)).ToArray<string>();

            for (int i = 0; i < child.Length - 1; i++)
            {
                var neigbors = GetNeighbors(parentChromosomes.ElementAt(0).ToArray<string>(),
                    parentChromosomes.ElementAt(1).ToArray<string>(), child[i]);
                RemoveAlreadyVisitedNeigbors(neigbors, child, i);
                if (neigbors.Count == 0)
                    continue;

                Assert.IsTrue(neigbors.Contains(child[i + 1]), "Didn't jump to neighbor");
            }
        }

        [TestMethod]
        public void GeneralEdgeRecombinationCrossover_NextNodeIsNeighborsOfPreviousNode()
        {
            var elements = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new FromElementsVectorChromosomePopulationGenerator<string>(elements, elements.Length, true, null, null);
            var parentChromosomes = generator.GeneratePopulation(2);

            var child = new GeneralEdgeRecombinationCrossover<string>(null, null).Crossover(parentChromosomes.ElementAt(0), parentChromosomes.ElementAt(1)).ToArray<string>();

            for (int i = 0; i < child.Length - 1; i++)
            {
                var neigbors = GetNeighbors(parentChromosomes.ElementAt(0).ToArray<string>(),
                    parentChromosomes.ElementAt(1).ToArray<string>(), child[i]);
                if (neigbors.Count == 0)
                    continue;

                Assert.IsTrue(neigbors.Contains(child[i + 1]), "Didn't jump to neighbor");
            }
        }

        [TestMethod]
        public void GeneralEdgeRecombinationCrossover_ElementsAreRepeated()
        {
            var elements = new [] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
            var generator = new FromElementsVectorChromosomePopulationGenerator<string>(elements, elements.Length, true, null, null);
            var parentChromosomes = generator.GeneratePopulation(2);

            var child = new GeneralEdgeRecombinationCrossover<string>(null, null).Crossover(parentChromosomes.ElementAt(0), parentChromosomes.ElementAt(1)).ToArray<string>();
            var distinctElements = child.Distinct().Count();

            Assert.IsTrue(distinctElements < 10);
        }

        private void RemoveAlreadyVisitedNeigbors(List<string> neighbors, string[] childArray, int currentIndex)
        {
            var alreadyVisitedElements = childArray.Take(currentIndex).ToArray();
            for (int i = 0; i < neighbors.Count; i++)
                if (alreadyVisitedElements.Contains(neighbors[i]))
                {
                    neighbors.Remove(neighbors[i]);
                    i--;
                }
        }

        private List<string> GetNeighbors(string[] array1, string[] array2, string element)
        {
            var result = new List<string>(GetNeighbors(array1, element));
            result.AddRange(GetNeighbors(array2, element));

            return result;
        }

        private List<string> GetNeighbors(string[] array, string element)
        {
            var indexs = GetIndexes(array, element);

            var neighbors = new List<string>();
            foreach (var index in indexs)
            {

                if (index == 0)
                {
                    neighbors.Add(array[1]);
                    neighbors.Add(array.Last());
                }
                else if (index == array.Length - 1)
                {
                    neighbors.Add(array[0]);
                    neighbors.Add(array[index - 1]);
                }
                else
                {
                    neighbors.Add(array[index + 1]);
                    neighbors.Add(array[index - 1]);
                }
            }
            return neighbors;
        }

        private List<int> GetIndexes(string[] array, string element)
        {
            var indexes = new List<int>();
            for (int i = 0; i < array.Length; i++)
                if (array[i] == element)
                    indexes.Add(i);
            return indexes;
        }
    }
}
