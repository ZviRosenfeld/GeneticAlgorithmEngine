using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.RaceConditionTests
{
    /// <summary>
    /// This class checks for race conditions in the command of GeneticSearchEngine
    /// </summary>
    [TestClass]
    [TestCategory("RaceConditions")]
    public class RaceConditions
    {
        private readonly Random random = new Random();
        private const int RUN_TIMES = 1000;
        private const int TEST_TIME = 3000;

        private GeneticSearchEngine engine;
        private CommandRunner runEngine;
        private CommandRunner engineNext;
        private CommandRunner pauseEngine;
        private CommandRunner getPopulation;
        private CommandRunner setPopulation;
        private CommandRunner renewPopulation;

        [TestInitialize]
        public void TestInitialize()
        {
            engine = TestUtils.Utils.GetBassicEngine();
            runEngine = new EngineRunner("Run", () => engine.Run());
            engineNext = new EngineRunner("Next", () => engine.Next());
            pauseEngine = new PauseEngine("Pause", () => engine.Pause());
            setPopulation = new EngineRunner("SetPopulation",
                () => engine.SetCurrentPopulation(new double[] {1, 2, 3, 4, 5}.ToChromosomes()));
            getPopulation = new EngineRunner("GetPopulation", () => engine.GetCurrentPopulation());
            renewPopulation = new EngineRunner("RenewPopulation", () => engine.RenewPopulation(0.5));
            engine.Next();
        }

        [TestMethod]
        public void AllCommands()
        {
            var actions = new List<CommandRunner>
            {
                pauseEngine,
                runEngine,
                engineNext,
                setPopulation,
                getPopulation,
                renewPopulation
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults(actions);
            WaitOnTasks(engineTasks);
            AssertCommandsWereCalled(actions);
        }

        [TestMethod]
        public void ManyRunPauseAndNextCommands()
        {
            var actions = new List<CommandRunner>
            {
                pauseEngine,
                runEngine,
                engineNext
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults(actions);
            WaitOnTasks(engineTasks);
            AssertCommandsWereCalled(actions);
        }

        [TestMethod]
        public void ManyRunAndPauseCommands()
        {
            var actions = new List<CommandRunner> {pauseEngine, runEngine};
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults(actions);
            WaitOnTasks(engineTasks);
            AssertCommandsWereCalled(actions);
            Assert.AreEqual(runEngine.CommandSucceeded, pauseEngine.CommandSucceeded, $"{nameof(runEngine)} != {nameof(pauseEngine)}");
        }
        
        [TestMethod]
        public void ManyNextCommands()
        {
            var actions = new List<CommandRunner> {engineNext};
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults(actions);
            WaitOnTasks(engineTasks);
            AssertCommandsWereCalled(actions);
        }

        [TestMethod]
        public void ManyNextAndPauseCommands()
        {
            var actions = new List<CommandRunner> {pauseEngine, engineNext};
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults(actions);
            WaitOnTasks(engineTasks);
            AssertCommandsWereCalled(actions);
        }

        [TestMethod]
        public void EngineCanOnlyRunOnce()
        {
            var actions = new List<CommandRunner> {runEngine};
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(50); // Give some time for things to run

            WaitOnTasks(engineTasks);
            engine.Pause();

            Assert.AreEqual(1, runEngine.CommandSucceeded, "Engine should have only ran once");
        }
        
        private void PrintResults(List<CommandRunner> commands)
        {
            foreach (var commandRunner in commands)
                Console.WriteLine(commandRunner.GetResults());         
        }

        private void AssertCommandsWereCalled(List<CommandRunner> commands)
        {
            foreach (var commandRunner in commands)
                Assert.IsTrue(commandRunner.CommandSucceeded > 10, $"Command didn't succeed enough times (succeeded {commandRunner.CommandSucceeded} times)");
        }

        private List<Task> RunOverAndOver(List<CommandRunner> commands)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < RUN_TIMES; i++)
                tasks.Add(Task.Run(ChooseRandomTask(commands)));

            return tasks;
        }

        private Action ChooseRandomTask(List<CommandRunner> actions)
        {
            var randomNumber = random.Next(0, actions.Count);
            return actions[randomNumber].RunCommand;
        }
        
        private void WaitOnTasks(List<Task> tasks)
        {
            foreach (var task in tasks)
                task.Wait(10);
        }
    }
}
