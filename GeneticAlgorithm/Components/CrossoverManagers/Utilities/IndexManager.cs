using System.Collections.Generic;

namespace GeneticAlgorithm.Components.CrossoverManagers.Utilities
{
    /// <summary>
    /// This class gets an array, and can answer "GetIndexOfElement" in O(1)
    /// </summary>
    class IndexManager<T> : IIndexManager<T>
    {
        private readonly Dictionary<T, int> indexes = new Dictionary<T, int>();

        public IndexManager(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
                indexes[array[i]] = i;
        }

        public int GetIndex(T element) => indexes[element];
    }
}
