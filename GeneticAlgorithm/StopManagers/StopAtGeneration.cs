using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.StopManagers
{
    public class StopAtGeneration : IStopManager
    {
        private readonly int generationToStopAt;

        public StopAtGeneration(int generationToStopAt)
        {
            this.generationToStopAt = generationToStopAt > 1
                ? generationToStopAt
                : throw new GeneticAlgorithmArgumentException(nameof(generationToStopAt) + " must be greater then one");
        }

        public bool ShouldStop(Population population, IEnvironment environment, int generation) =>
            generation >= generationToStopAt - 1;

        public void AddGeneration(Population population)
        {
            // Do nothing
        }
    }
}
