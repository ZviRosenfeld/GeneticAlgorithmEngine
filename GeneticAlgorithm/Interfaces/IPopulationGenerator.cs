using System.Collections.Generic;

namespace GeneticAlgorithm.Interfaces
{
    public interface IPopulationGenerator
    {
        /// <summary>
        /// size is the number of chromosomes we want to generate
        /// </summary>
        IEnumerable<IChromosome> GeneratePopulation(int size);
    }
}
