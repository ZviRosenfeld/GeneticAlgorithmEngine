namespace GeneticAlgorithm.Exceptions
{
    public class GeneticAlgorithmNullArgumentException : GeneticAlgorithmException
    {
        public GeneticAlgorithmNullArgumentException(string argumentName) :
            base($"{argumentName} can't be null")
        {
        }
    }
}
