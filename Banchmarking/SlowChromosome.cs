using System.Threading;
using GeneticAlgorithm.Interfaces;

namespace Benchmarking
{
    class SlowChromosome : IChromosome
    {
        private readonly int value;
        private readonly int sleepTime;

        public SlowChromosome(int value, int sleepTime)
        {
            this.value = value;
            this.sleepTime = sleepTime;
        }

        public double Evaluate()
        {
            Thread.Sleep(sleepTime);
            return value;
        }

        public void Mutate()
        {
            // Do nothing
        }
    }
}
