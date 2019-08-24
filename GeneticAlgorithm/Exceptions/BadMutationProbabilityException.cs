namespace GeneticAlgorithm.Exceptions
{
    public class BadMutationProbabilityException : GeneticAlgorithmException
    {
        public BadMutationProbabilityException(double probability) : 
            base($"Mutation probability must be between 0.0 to 1.0 (including). Probability was {probability}")
        {
        }
    }
}
