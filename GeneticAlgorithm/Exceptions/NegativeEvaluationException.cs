namespace GeneticAlgorithm.Exceptions
{
    public class NegativeEvaluationException : GeneticAlgorithmException
    {
        public NegativeEvaluationException() : base("Evaluation must be greater then 0")
        {
        }
    }
}
