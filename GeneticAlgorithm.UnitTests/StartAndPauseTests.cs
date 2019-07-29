using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class StartAndPauseTests
    {
        [TestMethod]
        public void RunTwice_ThrowsException()
        {
            var engine = GetEngine();
            Task.Run(() => engine.Run());

            AssertExceptionIsThrown(() => Task.Run(() => engine.Run()), typeof(EngineAlreadyRunningException));
        }

        [TestMethod]
        public void RunAndThenNext_ThrowsException()
        {
            var engine = GetEngine();
            Task.Run(() => engine.Run());

            AssertExceptionIsThrown(() => Task.Run(() => engine.Next()), typeof(EngineAlreadyRunningException));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void IsRunningTest(bool engineRunning)
        {
            var engine = GetEngine();
            if (engineRunning)
                Task.Run(() => engine.Run());

            Thread.Sleep(10);
            Assert.AreEqual(engineRunning, engine.IsRunning);
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void PuaseReturnValueTest(bool engineRunning)
        {
            var engine = GetEngine();
            if (engineRunning)
                Task.Run(() => engine.Run());

            Thread.Sleep(10);
            var running = engine.Puase();
            Assert.AreEqual(engineRunning, running);
        }

        [TestMethod]
        public void PuaseTest()
        {
            var engine = GetEngine();
            Task.Run(() => engine.Run());

            Thread.Sleep(10);
            engine.Puase();
            Thread.Sleep(10);
            Assert.AreEqual(false, engine.IsRunning);

            Task.Run(() => engine.Run()); // Assert that we can start a new scan
        }

        private void AssertExceptionIsThrown(Action func, Type exceptionType)
        {
            try
            {
                func();
            }
            catch (AggregateException e)
            {
                Assert.AreEqual(exceptionType, e.InnerExceptions.First().GetType());
            }
        }

        private GeneticSearchEngine GetEngine()
        {
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var engineBuilder = new TestGeneticSearchEngineBuilder(5, int.MaxValue, populationManager);
            return engineBuilder.Build();
        }
    }
}
