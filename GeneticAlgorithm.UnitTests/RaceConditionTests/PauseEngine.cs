using System;
using System.Threading;

namespace GeneticAlgorithm.UnitTests.RaceConditionTests
{
    class PauseEngine : CommandRunner
    {
        private readonly Func<bool> command;

        public PauseEngine(string commandName, Func<bool> command) : base(commandName)
        {
            this.command = command;
        }

        public override void RunCommand()
        {
            Interlocked.Increment(ref commandAttemps);
            if (command())
                Interlocked.Increment(ref commandSucceeded);
        }
    }
}
