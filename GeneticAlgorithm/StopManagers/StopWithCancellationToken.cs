using System.Threading;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.StopManagers
{
    class StopWithCancellationToken : IStopManager
    {
        private readonly CancellationToken cancellationToken;

        public StopWithCancellationToken(CancellationToken cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }

        public bool ShouldStop(Population population, IEnvironment environment, int generation) =>
            cancellationToken.IsCancellationRequested;

        public void AddGeneration(Population population)
        {
            // Do nothing
        }
    }
}
