using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.StopManagers
{
    class StopAtGeneration : IStopManager
    {
        private readonly int generationToStopAt;

        public StopAtGeneration(int generationToStopAt)
        {
            this.generationToStopAt = generationToStopAt > 0
                ? generationToStopAt
                : throw new GeneticAlgorithmException(nameof(generationToStopAt) + " must be greater then zero");
        }

        public bool ShouldStop(IChromosome[] population, double[] evaluations, int generation) =>
            generation >= generationToStopAt;

        public void AddGeneration(IChromosome[] population, double[] evaluations)
        {
            // Do nothing
        }
    }
}
