using System;
using System.Threading;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm.UnitTests.RaceConditionTests
{
    /// <summary>
    /// This class tries to run some command that can't be run if the engine is running.
    /// The class records how many times the command succeeded.
    /// </summary>
    class EngineRunner : CommandRunner
    {
        private readonly Action command;

        public EngineRunner(string commandName, Action command) : base(commandName)
        {
            this.command = command;
        }

        public override void RunCommand()
        {
            try
            {
                Interlocked.Increment(ref commandAttemps);
                command();
                Interlocked.Increment(ref commandSucceeded);
            }
            catch (EngineAlreadyRunningException)
            {
                // Do nothing
            }
        }
    }
}
