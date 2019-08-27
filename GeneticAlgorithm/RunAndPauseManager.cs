using System;
using System.Threading;
using GeneticAlgorithm.Exceptions;

namespace GeneticAlgorithm
{
    /// <summary>
    /// A class to protect race condition with running and pausing the engine
    /// </summary>
    public class RunAndPauseManager : IDisposable
    {
        private readonly object runLock = new object();
        private readonly object pauseLock = new object();
        private readonly ManualResetEvent engineFinishedEvent = new ManualResetEvent(true);
        private readonly TimeSpan pauseTimeout;

        public bool ShouldPause { get; private set; }

        public RunAndPauseManager(TimeSpan pauseTimeout)
        {
            this.pauseTimeout = pauseTimeout;
        }

        public bool IsRunning { get; private set; }

        public T RunAsCriticalBlock<T>(Func<T> func)
        {
            lock (pauseLock)
            lock (runLock)
            {
                if (IsRunning)
                    throw new EngineAlreadyRunningException();
                engineFinishedEvent.Reset();
                IsRunning = true;
            }

            try
            {
                return func();
            }
            finally
            {
                lock (runLock)
                {
                    IsRunning = false;
                    engineFinishedEvent.Set();
                }
            }
        }
        
        public bool Pause()
        {
            if (!IsRunning)
                return false;

            lock (pauseLock)
            {
                if (!IsRunning)
                    return false;

                try
                {
                    ShouldPause = true;
                    if (!engineFinishedEvent.WaitOne(pauseTimeout))
                        throw new CouldntStopEngineException();
                    return true;
                }
                finally
                {
                    ShouldPause = false;
                }
            }
        }

        public void Dispose()
        {
            engineFinishedEvent?.Dispose();
        }
    }
}
