using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Components.CrossoverManagers.Utilities
{
    /// <summary>
    /// A class the holds the neighbors for each element from two chromosomes.
    /// This class makes no assumptions about the vectors.
    /// </summary>
    public class ReapetingAdjacencyMatrix<T> : IAdjacencyMatrix<T>
    {
        private readonly Random random = new Random();
        private readonly Dictionary<T, List<T>> adjacencyMatrix = new Dictionary<T, List<T>>();


        public T GetNeighbor(T element)
        {
            var neighbors = adjacencyMatrix[element];
            if (neighbors.Count == 0)
                return adjacencyMatrix.Keys.ElementAt(random.Next(adjacencyMatrix.Count));

            return neighbors.ElementAt(random.Next(neighbors.Count));
        }

        public ReapetingAdjacencyMatrix(T[] vector1, T[] vector2)
        {
            AddChromosomeToMartix(vector1);
            AddChromosomeToMartix(vector2);

            var keys = adjacencyMatrix.Keys.ToArray();
            foreach (var key in keys)
                adjacencyMatrix[key] = adjacencyMatrix[key].Distinct().ToList();
        }

        private void AddChromosomeToMartix(T[] vector)
        {
            for (int i = 1; i < vector.Length - 1; i++)
                AddToMatric(vector[i], vector[i - 1], vector[i + 1]);
            if (vector.Length > 1)
                AddToMatric(vector[0], vector[1], vector[vector.Length - 1]);
            if (vector.Length > 1)
                AddToMatric(vector[vector.Length - 1], vector[0], vector[vector.Length - 2]);
        }

        private void AddToMatric(T index, params T[] values)
        {
            if (!adjacencyMatrix.ContainsKey(index))
                adjacencyMatrix[index] = new List<T>();

            var neighbors = adjacencyMatrix[index];
            foreach (var value in values)
                neighbors.Add(value);
        }

        public List<T> GetNeighbors(T element) => adjacencyMatrix[element];

        public bool ContainsElement(T element) => adjacencyMatrix.ContainsKey(element);

    }
}
