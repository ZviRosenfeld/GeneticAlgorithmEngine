namespace GeneticAlgorithm.Exceptions
{
    public class EngineAlreadyRunningException : GeneticAlgorithmException
    {
        public EngineAlreadyRunningException() : base("The engine is already running")
        {
        }
    }
}
