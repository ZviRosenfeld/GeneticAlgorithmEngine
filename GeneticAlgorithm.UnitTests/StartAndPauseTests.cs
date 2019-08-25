using System;
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
            var engine = TestUtils.Utils.GetBassicEngine();
            Task.Run(() => engine.Run());
            while (!engine.IsRunning) ;

            AssertExceptionIsThrown(() => engine.Run(), typeof(EngineAlreadyRunningException));
        }

        [TestMethod]
        public void RunAndThenNext_ThrowsException()
        {
            var engine = TestUtils.Utils.GetBassicEngine();
            Task.Run(() => engine.Run());
            while (!engine.IsRunning) ;

            AssertExceptionIsThrown(() => engine.Run(), typeof(EngineAlreadyRunningException));
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void IsRunningTest(bool engineRunning)
        {
            var engine = TestUtils.Utils.GetBassicEngine();
            if (engineRunning)
                Task.Run(() => engine.Run());

            Thread.Sleep(10);
            Assert.AreEqual(engineRunning, engine.IsRunning);
        }
        
        [TestMethod]
        public void PauseTest()
        {
            var engine = TestUtils.Utils.GetBassicEngine();
            Task.Run(() => engine.Run());

            Thread.Sleep(10);
            engine.Pause();
            Assert.AreEqual(false, engine.IsRunning);

            Task.Run(() => engine.Run()); // Assert that we can start a new scan
        }

        [TestMethod]
        public void RunAfterPauseTest()
        {
            var engine = TestUtils.Utils.GetBassicEngine();
            Task.Run(() => engine.Run());
            Thread.Sleep(10);

            engine.Pause();

            Task.Run(() => engine.Run());
            Thread.Sleep(10);

            Assert.AreEqual(true, engine.IsRunning, "Engine should be running");
        }

        [TestMethod]
        public void RunAfterPauseTest2()
        {
            var engine = TestUtils.Utils.GetBassicEngine();
            Task.Run(() => engine.Run());
            Thread.Sleep(10);

            engine.Pause();

            Task.Run(() => engine.Run());
            Thread.Sleep(10);

            engine.Pause();

            Task.Run(() => engine.Run());
            Thread.Sleep(10);

            Assert.AreEqual(true, engine.IsRunning, "Engine should be running");
        }

        [TestMethod]
        public void PauseReturnValueTest()
        {
            var engine = TestUtils.Utils.GetBassicEngine();
            var result = engine.Pause();
            Assert.IsFalse(result, "Engine shouldn't have been running");

            Task.Run(() => engine.Run());
            Thread.Sleep(10);
            result = engine.Pause();
            Assert.IsTrue(result, "Engine should have been running");
        }

        private void AssertExceptionIsThrown(Action func, Type exceptionType)
        {
            try
            {
                func();
                Assert.Fail("Didn't throw an exception");
            }
            catch (Exception e)
            {
                Assert.AreEqual(exceptionType, e.GetType());
            }
        }
    }
}
