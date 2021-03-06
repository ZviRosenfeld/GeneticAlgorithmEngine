﻿namespace GeneticAlgorithm.Interfaces
{
    /// <summary>
    /// StopManagers lets you configure when the search to stop
    /// </summary>
    public interface IStopManager
    {
        /// <summary>
        /// This method is will be called once per generation (after the ShouldStop method for that generation), so you can use it to remember old data.
        /// </summary>
        void AddGeneration(Population population);

        bool ShouldStop(Population population, IEnvironment environment, int generation);
    }
}
