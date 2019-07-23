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

        public bool ShouldStop(IChromosome[] population, double[] evaluations, int generation) =>
            cancellationToken.IsCancellationRequested;
    }
}
