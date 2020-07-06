namespace GeneticAlgorithm.UnitTests.RaceConditionTests
{
    abstract class CommandRunner
    {
        protected int commandSucceeded = 0;
        public int CommandSucceeded => commandSucceeded;

        protected int commandAttemps = 0;
        public int CommandAttemps => commandAttemps;

        protected readonly string commandName;

        public CommandRunner(string commandName)
        {
            this.commandName = commandName;
        }

        public abstract void RunCommand();

        public string GetResults() =>
            $"{commandName}: {commandSucceeded} / {commandAttemps}";
    }
}
