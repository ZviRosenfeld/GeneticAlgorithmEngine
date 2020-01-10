using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.CrossoverManagers
{
    /// <summary>
    /// CycleCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
    /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
    /// and that both parents contain the same genomes (but probably in different orders).
    /// If one of these conditions isn't met, CycleCrossoverManager may throw an exception.
    /// Also, the Equals method must be implemented for type T.
    /// 
    /// The Cycle Crossover operator identifies a number of so-called cycles between two parent chromosomes. 
    /// Then, to form Child 1, cycle one is copied from parent 1, cycle 2 from parent 2, cycle 3 from parent 1, and so on.
    /// 
    /// In Cycle Crossover, the child is guaranteed to contain each genome exactly once.
    /// 
    /// See: http://www.rubicite.com/Tutorials/GeneticAlgorithms/CrossoverOperators/CycleCrossoverOperator.aspx
    /// </summary>
    public class CycleCrossoverManager<T> : ICrossoverManager
    {
        private readonly IMutationManager<T> mutationManager;
        private readonly IEvaluator evaluator;

        /// <summary>
        /// CycleCrossover Works on chromosomes of type VectorChromosome&lt;T&gt;.
        /// It assumes that both parents are of the same length, that every genome appears only once in each parent, 
        /// and that both parents contain the same genomes (but probably in different orders).
        /// If one of these conditions isn't met, CycleCrossoverManager may throw an exception.
        /// 
        /// Also, the Equals method must be implemented for type T.
        /// </summary>
        public CycleCrossoverManager(IMutationManager<T> mutationManager, IEvaluator evaluator)
        {
            this.mutationManager = mutationManager;
            this.evaluator = evaluator;
        }

        public IChromosome Crossover(IChromosome chromosome1, IChromosome chromosome2)
        {
            var vector1 = ((VectorChromosome<T>)chromosome1).GetVector();
            var vector2 = ((VectorChromosome<T>)chromosome2).GetVector();
            var indexManager = new IndexManager<T>(vector1);
            var length = vector1.Length;

            var newVector = new T[length];
            var addedIndexes = new bool[length];
            var takeFromVector1 = true;
            for (int i = 0; i < length; i++)
            {
                if (addedIndexes[i]) continue;
                var cycle = FindCycle(i, vector1, vector2, indexManager);
                MarkIndexes(addedIndexes, cycle);
                Fill(newVector, takeFromVector1 ? vector1 : vector2, cycle);
                takeFromVector1 = !takeFromVector1;
            }

            return new VectorChromosome<T>(newVector, mutationManager, evaluator);
        }

        private void MarkIndexes(bool[] collection, List<int> indexes)
        {
            foreach (var index in indexes)
                collection[index] = true;
        }

        private void Fill(T[] vectorToFill, T[] fillFrom, List<int> indexes)
        {
            foreach (var index in indexes)
                vectorToFill[index] = fillFrom[index];
        }

        private List<int> FindCycle(int index, T[] vector1, T[] vector2, IIndexManager<T> indexManager)
        {
            var cycleIndexes = new List<int>();
            var endCycleValue = vector1[index];
            while (!vector2[index].Equals(endCycleValue))
            {
                cycleIndexes.Add(index);
                index = indexManager.GetIndex(vector2[index]);
            }
            cycleIndexes.Add(index);

            return cycleIndexes;
        }
    }
}
