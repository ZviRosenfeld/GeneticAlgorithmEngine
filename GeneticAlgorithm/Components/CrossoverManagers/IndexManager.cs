using System.Collections.Generic;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// This calss gets an array, and can answer "GetIndexOfElement" in O(1)
    /// </summary>
    class IndexManager<T> : IIndexManager<T>
    {
        private Dictionary<T, int> indexes = new Dictionary<T, int>();

        public IndexManager(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
                indexes[array[i]] = i;
        }

        public int GetIndex(T element) => indexes[element];
    }
}
