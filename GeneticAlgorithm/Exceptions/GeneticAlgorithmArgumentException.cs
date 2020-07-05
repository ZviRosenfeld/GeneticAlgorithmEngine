namespace GeneticAlgorithm.Exceptions
{
    public class GeneticAlgorithmArgumentException : GeneticAlgorithmException
    {
        public GeneticAlgorithmArgumentException(string message) :
            base(message)
        {
        }

        /// <summary>
        /// Returns an exception stating that argument name was smaller than zero.
        /// </summary>
        public static GeneticAlgorithmArgumentException SmallerThanZeroException(string argumentName, double value) =>
            new GeneticAlgorithmArgumentException($"{argumentName} was {value}. {argumentName} must be at least zero!");
    }
}
