using System;
using System.Linq;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.SelectionStrategies
{
    /// <summary>
    /// Holds a pool of chromosomes.
    /// ChromosomePool give two guarantees:
    /// 1) If the pool contains n chromosomes, and the  GetChromosome method is called n times, every chromosome will be returns exactly once.
    /// 2) The chromosomes will be returned in a random order.
    /// 
    /// Note that GetChromosome can only be called chromosomes.Length times
    /// </summary>
    public class ChromosomePool
    {
        private IChromosome[] chromosomes;
        private int counter = -1;

        public ChromosomePool(IChromosome[] chromosomes)
        {
            this.chromosomes = Shuffle(chromosomes);
        }
        
        private IChromosome[] Shuffle(IChromosome[] chromosome)
        {
            var random = new Random();
            var n = chromosome.Length;
            var shuffledChromosomes = chromosome.ToArray(); // Clone
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                IChromosome value = shuffledChromosomes[k];
                shuffledChromosomes[k] = shuffledChromosomes[n];
                shuffledChromosomes[n] = value;
            }

            return shuffledChromosomes;
        }

        /// <summary>
        /// Note that GetChromosome can only be called chromosomes.Length times
        /// </summary>
        public IChromosome GetChromosome()
        {
            counter++;
            return chromosomes[counter];
        }
    }
}
