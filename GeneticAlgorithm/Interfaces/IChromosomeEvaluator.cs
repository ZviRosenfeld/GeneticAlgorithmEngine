namespace GeneticAlgorithm.Interfaces
{
    public interface IChromosomeEvaluator
    {
        /// <summary>
        /// Sometimes, it's impossible to evaluate a chromosome without knowing information about it's surroundings, such as the rest of the population.
        /// That's why SetEnvierment is called for every generation before the evaluation starts.
        /// It lets the IChromosomeEvaluator know about the Envierment, so it can take it into account.
        /// </summary>
        void SetEnvierment(IEnvironment envierment);

        double Evaluate(IChromosome chromosome);
    }
}
