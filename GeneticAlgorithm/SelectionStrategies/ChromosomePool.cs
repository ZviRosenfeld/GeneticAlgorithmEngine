using System;
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
        private readonly Random random = new Random();
        private readonly IChromosome[] chromosomes;
        private int counter = -1;

        public ChromosomePool(IChromosome[] chromosomes)
        {
            this.chromosomes = chromosomes.Shuffle(random);
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
