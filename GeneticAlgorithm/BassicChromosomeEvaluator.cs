using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    class BassicChromosomeEvaluator : IChromosomeEvaluator
    {

        public void SetEnvierment(IEnvironment envierment)
        {
            // Do nothing
        }

        public double Evaluate(IChromosome chromosome) => chromosome.Evaluate();
    }
}
