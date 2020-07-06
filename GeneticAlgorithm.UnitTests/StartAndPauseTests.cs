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
            using (var engine = TestUtils.Utils.GetBasicEngine())
            {
                Task.Run(() => engine.Run());
                while (!engine.IsRunning) ;

                AssertExceptionIsThrown(() => engine.Run(), typeof(EngineAlreadyRunningException));
            }
        }

        [TestMethod]
        public void RunAndThenNext_ThrowsException()
        {
            using (var engine = TestUtils.Utils.GetBasicEngine())
            {
                Task.Run(() => engine.Run());
                while (!engine.IsRunning) ;

                AssertExceptionIsThrown(() => engine.Run(), typeof(EngineAlreadyRunningException));
            }
        }

        [TestMethod]
        [DataRow(true)]
        [DataRow(false)]
        public void IsRunningTest(bool engineRunning)
        {
            using (var engine = TestUtils.Utils.GetBasicEngine())
            {
                if (engineRunning)
                    Task.Run(() => engine.Run());

                Thread.Sleep(100);
                Assert.AreEqual(engineRunning, engine.IsRunning);
            }
        }

        [TestMethod]
        public void PauseTest()
        {
            using (var engine = TestUtils.Utils.GetBasicEngine())
            {
                Task.Run(() => engine.Run());

                Thread.Sleep(10);
                engine.Pause();
                Assert.AreEqual(false, engine.IsRunning);

                Task.Run(() => engine.Run()); // Assert that we can start a new scan
            }
        }

        [TestMethod]
        public void RunAfterPauseTest()
        {
            TestUtils.Utils.RunTimedTest(() =>
            {
                using (var engine = TestUtils.Utils.GetBasicEngine())
                {
                    Task.Run(() => engine.Run());
                    Thread.Sleep(100);

                    engine.Pause();

                    Task.Run(() => engine.Run());
                    Thread.Sleep(100);

                    Assert.AreEqual(true, engine.IsRunning, "Engine should be running");
                }
            });
        }

        [TestMethod]
        public void RunAfterPauseTest2()
        {
            TestUtils.Utils.RunTimedTest(() =>
            {
                using (var engine = TestUtils.Utils.GetBasicEngine())
                {
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
            });
        }

        [TestMethod]
        public void PauseReturnValueTest()
        {
            TestUtils.Utils.RunTimedTest(() =>
            {
                using (var engine = TestUtils.Utils.GetBasicEngine())
                {
                    var result = engine.Pause();
                    Assert.IsFalse(result, "Engine shouldn't have been running");

                    Task.Run(() => engine.Run());
                    Thread.Sleep(10);
                    result = engine.Pause();
                    Assert.IsTrue(result, "Engine should have been running");
                }
            });
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
