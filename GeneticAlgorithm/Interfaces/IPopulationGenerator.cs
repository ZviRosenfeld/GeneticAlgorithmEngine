using System.Collections.Generic;

namespace GeneticAlgorithm.Interfaces
{
    public interface IPopulationGenerator
    {
        IEnumerable<IChromosome> GeneratePopulation(int size);
    }
}
