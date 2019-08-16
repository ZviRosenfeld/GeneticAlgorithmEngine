using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class DefaultEnvironment : IEnvironment
    {
        public void UpdateEnvierment(IChromosome[] chromosomes, int generation)
        {
            Chromosomes = chromosomes;
            Generation = generation;
        }

        public IChromosome[] Chromosomes { get; private set; }

        public int Generation { get; private set; }
    }
}
