using GeneticAlgorithm.SelectionStrategies;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.SelectionStrategyTests
{
    [TestClass]
    public class ChromosomePoolTests
    {
        [TestMethod]
        public void ChromosomePoolReturnsAllChromosomesInPool()
        {
            var chromosomes = (new double[] { 1, 2, 3, 4 }).ToChromosomes();
            var pool = new ChromosomePool(chromosomes);

            bool chromosome1 = false, chromosome2 = false, chromosome3 = false, chromosome4 = false;
            for (int i = 0; i < chromosomes.Length; i++)
            {
                var chromosome = pool.GetChromosome();
                if (chromosome.Evaluate() == chromosomes[0].Evaluate())
                    chromosome1 = true;
                if (chromosome.Evaluate() == chromosomes[1].Evaluate())
                    chromosome2 = true;
                if (chromosome.Evaluate() == chromosomes[2].Evaluate())
                    chromosome3 = true;
                if (chromosome.Evaluate() == chromosomes[3].Evaluate())
                    chromosome4 = true;
            }

            Assert.IsTrue(chromosome1, "Didn't get any " + nameof(chromosome1));
            Assert.IsTrue(chromosome2, "Didn't get any " + nameof(chromosome2));
            Assert.IsTrue(chromosome3, "Didn't get any " + nameof(chromosome3));
            Assert.IsTrue(chromosome4, "Didn't get any " + nameof(chromosome4));
        }

        [TestMethod]
        public void ChromosomePoolReturnsScattersGeneratedChromosomes()
        {
            var chromosomes = (new double[] { 1, 2, 3, 4 }).ToChromosomes();
            var pool = new ChromosomePool(chromosomes);
            
            for (int i = 0; i < chromosomes.Length; i++)
            {
                var chromosome = pool.GetChromosome();
                if (chromosome.Evaluate() != chromosomes[i].Evaluate())
                    return;
            }

            Assert.Fail("The chromosomes were returned in the order in which they were inserted");
        }
    }
}
