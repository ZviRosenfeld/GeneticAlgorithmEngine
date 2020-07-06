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

        private CommandRunner GetRunCommand(GeneticSearchEngine engine) =>
            new EngineRunner("Run", () => engine.Run());

        private CommandRunner GetNextCommand(GeneticSearchEngine engine) =>
            new EngineRunner("Next", () => engine.Next());

        private CommandRunner GetPauseCommand(GeneticSearchEngine engine) =>
            new PauseEngine("Pause", () => engine.Pause());

        private CommandRunner GetSetPopulationCommand(GeneticSearchEngine engine) =>
            new EngineRunner("SetPopulation", () => engine.SetCurrentPopulation(new double[] { 1, 2, 3, 4, 5 }.ToChromosomes()));
        private CommandRunner GetRenewPopulationCommand(GeneticSearchEngine engine) =>
            new EngineRunner("RenewPopulation", () => engine.RenewPopulation(0.5));

        private CommandRunner GetGetPopulationCommand(GeneticSearchEngine engine) =>
           new EngineRunner("GetPopulation", () => engine.GetCurrentPopulation());


        [TestMethod]
        public void AllCommands()
        {
            using (var engine = Utils.GetBasicEngine())
            {
                engine.Next();
                var actions = new List<CommandRunner>
                {
                    GetPauseCommand(engine),
                    GetRunCommand(engine),
                    GetNextCommand(engine),
                    GetSetPopulationCommand(engine),
                    GetGetPopulationCommand(engine),
                    GetRenewPopulationCommand(engine),
                };
                var engineTasks = RunOverAndOver(actions);

                Thread.Sleep(TEST_TIME); // Give some time for things to run

                PrintResults(actions);
                WaitOnTasks(engineTasks);
                AssertCommandsWereCalled(actions);
            }
        }

        [TestMethod]
        public void ManyRunPauseAndNextCommands()
        {
            using (var engine = Utils.GetBasicEngine())
            {
                var actions = new List<CommandRunner>
                {
                    GetPauseCommand(engine),
                    GetRunCommand(engine),
                    GetNextCommand(engine)
                };
                var engineTasks = RunOverAndOver(actions);

                Thread.Sleep(TEST_TIME); // Give some time for things to run

                PrintResults(actions);
                WaitOnTasks(engineTasks);
                AssertCommandsWereCalled(actions);
            }
        }

        [TestMethod]
        public void ManyRunAndPauseCommands()
        {
            using (var engine = Utils.GetBasicEngine())
            {
                var runEngine = GetRunCommand(engine);
                var pauseEngine = GetPauseCommand(engine);
                var actions = new List<CommandRunner> { pauseEngine, runEngine };
                var engineTasks = RunOverAndOver(actions);

                Thread.Sleep(TEST_TIME); // Give some time for things to run

                PrintResults(actions);
                WaitOnTasks(engineTasks);
                AssertCommandsWereCalled(actions);
                Assert.AreEqual(runEngine.CommandSucceeded, pauseEngine.CommandSucceeded, $"{nameof(runEngine)} != {nameof(pauseEngine)}");
            }
        }

        [TestMethod]
        public void ManyNextCommands()
        {
            using (var engine = Utils.GetBasicEngine())
            {
                var actions = new List<CommandRunner> { GetNextCommand(engine) };
                var engineTasks = RunOverAndOver(actions);

                Thread.Sleep(TEST_TIME); // Give some time for things to run

                PrintResults(actions);
                WaitOnTasks(engineTasks);
                AssertCommandsWereCalled(actions);
            }
        }

        [TestMethod]
        public void ManyNextAndPauseCommands()
        {
            using (var engine = Utils.GetBasicEngine())
            {
                var actions = new List<CommandRunner> { GetPauseCommand(engine), GetNextCommand(engine) };
                var engineTasks = RunOverAndOver(actions);

                Thread.Sleep(TEST_TIME); // Give some time for things to run

                PrintResults(actions);
                WaitOnTasks(engineTasks);
                AssertCommandsWereCalled(actions);
            }
        }

        [TestMethod]
        public void EngineCanOnlyRunOnce()
        {
            using (var engine = Utils.GetBasicEngine())
            {
                var runEngine = GetRunCommand(engine);
                var actions = new List<CommandRunner> { runEngine };
                var engineTasks = RunOverAndOver(actions);

                Thread.Sleep(50); // Give some time for things to run

                WaitOnTasks(engineTasks);
                engine.Pause();

                Assert.AreEqual(1, runEngine.CommandSucceeded, "Engine should have only ran once");
            }
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
