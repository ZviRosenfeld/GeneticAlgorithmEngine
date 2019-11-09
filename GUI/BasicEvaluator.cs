using System.Linq;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Interfaces;

namespace GUI
{
    class BasicEvaluator : IEvaluator
    {
        public double Evaluate(IChromosome chromosome) =>
            ((VectorChromosome<int>) chromosome).GetVector().Sum();
    }
}
