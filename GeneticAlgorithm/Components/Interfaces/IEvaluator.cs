using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.Components.Interfaces
{
    public interface IEvaluator
    {
        double Evaluate(IChromosome chromosome);
    }
}
