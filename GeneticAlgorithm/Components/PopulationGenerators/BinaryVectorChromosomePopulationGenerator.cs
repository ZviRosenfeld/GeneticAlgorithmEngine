using GeneticAlgorithm.Components.Interfaces;

namespace GeneticAlgorithm.Components.PopulationGenerators
{
    public class BinaryVectorChromosomePopulationGenerator : VectorChromosomePopulationGenerator
    {
        public BinaryVectorChromosomePopulationGenerator(int vectorSize, IMutationManager mutationManager,
            IEvaluator evaluator) : base(vectorSize, 0, 1, mutationManager, evaluator)
        {
        }
    }
}
