using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Components.CrossoverManagers.Utilities
{
    /// <summary>
    /// A class the holds the neighbors for each element from two chromosomes.
    /// This class assumes that the elements in each vector are the same and no reapeting.
    /// </summary>
    public class NonReapetingAdjacencyMatrix<T> : IAdjacencyMatrix<T>
    {
        private readonly Random random = new Random();
        private readonly Dictionary<T, LinkedList<T>> adjacencyMatrix = new Dictionary<T, LinkedList<T>>();
        
        /// <summary>
        /// If true, we'll select the neighbor that has the lease neighbors.
        /// If fasle, we'll just select a neighbor at random.
        /// </summary>
        private readonly bool selectNeighborWithLeastNeighbors;

        public NonReapetingAdjacencyMatrix(T[] vector1, T[] vector2, bool selectNeighborWithLeastNeighbors)
        {
            this.selectNeighborWithLeastNeighbors = selectNeighborWithLeastNeighbors;
            AddChromosomeToMartix(vector1);
            AddChromosomeToMartix(vector2);
        }

        /// <summary>
        /// Returns the neighbor with the smallest set of neighbors, and removes the original element.
        /// </summary>
        public T GetNeighbor(T element)
        {
            var neighbors = adjacencyMatrix[element];
            Remove(element, neighbors);
            if (neighbors.Count == 0)
                return adjacencyMatrix.Keys.ElementAt(random.Next(adjacencyMatrix.Count));

            if (!selectNeighborWithLeastNeighbors)
                return neighbors.ElementAt(random.Next(neighbors.Count));

            var elemenetsWithMinNeighbors = FindNeighborsWithLeaseNeighbors(neighbors);
            return elemenetsWithMinNeighbors[random.Next(elemenetsWithMinNeighbors.Count)];
        }

        private List<T> FindNeighborsWithLeaseNeighbors(LinkedList<T> neighbors)
        {
            var minNeigbors = 10;
            var elemenetsWithMinNeighbors = new List<T>();
            foreach (var neighbor in neighbors)
            {
                var neigbors = adjacencyMatrix[neighbor].Count;
                if (neigbors < minNeigbors)
                {
                    minNeigbors = neigbors;
                    elemenetsWithMinNeighbors = new List<T> {neighbor};
                }
                else if (neigbors == minNeigbors)
                    elemenetsWithMinNeighbors.Add(neighbor);
            }
            return elemenetsWithMinNeighbors;
        }

        public LinkedList<T> GetNeighbors(T element) => adjacencyMatrix[element];

        public bool ContainsElement(T element) => adjacencyMatrix.ContainsKey(element);

        private void Remove(T element, LinkedList<T> neighbors)
        {
            foreach (var neighbor in neighbors)
                adjacencyMatrix[neighbor].Remove(element);

            adjacencyMatrix.Remove(element);
        }

        private void AddChromosomeToMartix(T[] vector)
        {
            for (int i = 1; i < vector.Length - 1; i++)
                AddToMatric(vector[i], vector[i - 1], vector[i + 1]);
            if (vector.Length > 1)
                AddToMatric(vector[0],vector[1], vector[vector.Length - 1]);
            if (vector.Length > 1)
                AddToMatric(vector[vector.Length - 1], vector[0], vector[vector.Length - 2]);
        }

        private void AddToMatric(T index, params T[] values)
        {
            if (!adjacencyMatrix.ContainsKey(index))
                adjacencyMatrix[index] = new LinkedList<T>();

            var neighborList = adjacencyMatrix[index];
            foreach (var value in values)
                if (!neighborList.Contains(value))
                    neighborList.AddLast(value);
        }
    }
}
