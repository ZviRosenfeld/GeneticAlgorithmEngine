using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    public class BinaryVectorChromosomePopulationGenerator : IntVectorChromosomePopulationGenerator
    {
        public BinaryVectorChromosomePopulationGenerator(int vectorSize, IMutationManager<int> mutationManager,
            IEvaluator evaluator) : base(vectorSize, 0, 1, mutationManager, evaluator)
        {
        }
    }
}
