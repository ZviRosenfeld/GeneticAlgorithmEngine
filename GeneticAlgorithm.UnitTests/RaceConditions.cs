using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
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

        private int puaseSucceedTimes;
        private int runSucceedTimes;
        private int nextSucceedTimes;
        private int renewPopulationTimes;
        private int setPopulationTimes;

        [TestInitialize]
        public void TestInitialize()
        {
            puaseSucceedTimes = 0;
            runSucceedTimes = 0;
            nextSucceedTimes = 0;
            renewPopulationTimes = 0;
            setPopulationTimes = 0;
        }

        [TestMethod]
        public void AllCommands()
        {
            var engine = TestUtils.GetBassicEngine();
            engine.Next();
            var actions = new List<Action>
            {
                () => PauseEngine(engine),
                () => RunEngine(engine),
                () => EngineNext(engine),
                () => SetPopulation(engine),
                () => RenewPopulation(engine)
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults();
            WaitOnTasks(engineTasks);
        }

        [TestMethod]
        public void ManyRunPauseAndNextCommands()
        {
            var engine = TestUtils.GetBassicEngine();
            var actions = new List<Action>
            {
                () => PauseEngine(engine),
                () => RunEngine(engine),
                () => EngineNext(engine)
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults();
            WaitOnTasks(engineTasks);
        }

        [TestMethod]
        public void ManyRunAndPauseCommands()
        {
            var engine = TestUtils.GetBassicEngine();
            var actions = new List<Action>
            {
                () => PauseEngine(engine),
                () => RunEngine(engine),
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults();
            WaitOnTasks(engineTasks);
            Assert.AreEqual(runSucceedTimes, puaseSucceedTimes, $"{nameof(puaseSucceedTimes)} != {nameof(runSucceedTimes)}");
        }
        
        [TestMethod]
        public void ManyNextCommands()
        {
            var engine = TestUtils.GetBassicEngine();
            var actions = new List<Action>
            {
                () => EngineNext(engine)
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults();
            WaitOnTasks(engineTasks);
        }

        [TestMethod]
        public void ManyNextAndPauseCommands()
        {
            var engine = TestUtils.GetBassicEngine();
            var actions = new List<Action>
            {
                () => PauseEngine(engine),
                () => EngineNext(engine)
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(TEST_TIME); // Give some time for things to run

            PrintResults();
            WaitOnTasks(engineTasks);
        }

        [TestMethod]
        public void EngineCanOnlyRunOnce()
        {
            var engine = TestUtils.GetBassicEngine();
            var actions = new List<Action>
            {
                () => RunEngine(engine),
            };
            var engineTasks = RunOverAndOver(actions);

            Thread.Sleep(50); // Give some time for things to run

            WaitOnTasks(engineTasks);
            engine.Pause();

            Assert.AreEqual(1, runSucceedTimes, "Engine should have only ran once");
        }

        private void PauseEngine(GeneticSearchEngine engine)
        {
            if (engine.Pause())
                Interlocked.Increment(ref puaseSucceedTimes);
        }

        private void EngineNext(GeneticSearchEngine engine)
        {
            try
            {
                engine.Next();
                Interlocked.Increment(ref nextSucceedTimes);
            }
            catch (EngineAlreadyRunningException)
            {
                // Do nothing
            }
        }

        private void RunEngine(GeneticSearchEngine engine)
        {
            try
            {
                engine.Run();
                Interlocked.Increment(ref runSucceedTimes);
            }
            catch (EngineAlreadyRunningException)
            {
                // Do nothing
            }
        }

        private void SetPopulation(GeneticSearchEngine engine)
        {
            try
            {
                engine.SetCurrentPopulation(new double[]{2,2,2,2,2}.ToChromosomes());
                Interlocked.Increment(ref setPopulationTimes);
            }
            catch (EngineAlreadyRunningException)
            {
                // Do nothing
            }
        }

        private void RenewPopulation(GeneticSearchEngine engine)
        {
            try
            {
                engine.RenewPopulation(0.5);
                Interlocked.Increment(ref renewPopulationTimes);
            }
            catch (EngineAlreadyRunningException)
            {
                // Do nothing
            }
        }

        private void PrintResults()
        {
            Console.WriteLine($"{nameof(runSucceedTimes)} = {runSucceedTimes}");
            Console.WriteLine($"{nameof(nextSucceedTimes)} = {nextSucceedTimes}");
            Console.WriteLine($"{nameof(puaseSucceedTimes)} = {puaseSucceedTimes}");
            Console.WriteLine($"{nameof(setPopulationTimes)} = {setPopulationTimes}");
            Console.WriteLine($"{nameof(renewPopulationTimes)} = {renewPopulationTimes}");
        }

        private List<Task> RunOverAndOver(List<Action> actions)
        {
            var tasks = new List<Task>();
            for (int i = 0; i < RUN_TIMES; i++)
                tasks.Add(Task.Run(ChooseRandomTask(actions)));

            return tasks;
        }

        private Action ChooseRandomTask(List<Action> actions)
        {
            var randomNumber = random.Next(0, actions.Count);
            return actions[randomNumber];
        }
        
        private void WaitOnTasks(List<Task> tasks)
        {
            foreach (var task in tasks)
            {
                task.Wait(10);
            }
        }
    }
}
