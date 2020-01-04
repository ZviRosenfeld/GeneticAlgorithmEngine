using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    [TestClass]
    public class UniformCrossoverManagerTests
    {
        private const int TEST_RUNS = 20;
        private const int CHROMOSOME_SIZE = 10;

        private readonly IPopulationGenerator smallPopulationGenerator1 =
            new IntVectorChromosomePopulationGenerator(CHROMOSOME_SIZE, 0, 10, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());
        private readonly IPopulationGenerator smallPopulationGenerator2 =
            new IntVectorChromosomePopulationGenerator(CHROMOSOME_SIZE, 11, 20, A.Fake<IMutationManager<int>>(),
                A.Fake<IEvaluator>());
        
        [TestMethod]
        public void UniformCrossoverManagerTest()
        {
            var crossoverManager = new UniformCrossoverManager<int>(A.Fake<IMutationManager<int>>(), A.Fake<IEvaluator>());
            for (int i = 0; i < TEST_RUNS; i++)
            {
                var chromosome1 = (VectorChromosome<int>)smallPopulationGenerator1.GeneratePopulation(1).First();
                var chromosome2 = (VectorChromosome<int>)smallPopulationGenerator2.GeneratePopulation(1).First();
                var newChromosome = (VectorChromosome<int>)crossoverManager.Crossover(chromosome1, chromosome2);

                Utils.K_CrossoverGetCrossoverPointsAndAssertThatGenomesAreRight(newChromosome, chromosome2, chromosome1);
            }
        }
    }
}
