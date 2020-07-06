using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.PopulationGenerators
{
    static class Utils
    {
        /// <summary>
        /// Test that the genomes in the chromosome are within range, and that they are dispersed.
        /// </summary>
        public static void TestChromosomes<T>(this IPopulationGenerator populationGenerator, int minGenome, int maxGenome)
        {
            var chromosomes = populationGenerator.GeneratePopulation(20);
            var recivedGenomes = new HashSet<int>();
            foreach (var chromosome in chromosomes)
            {
                var vectorChromosome = (VectorChromosome<T>)chromosome;
                foreach (var genome in vectorChromosome.GetVector())
                {
                    var intGenome = genome.ToInt();
                    Assert.IsTrue(intGenome >= minGenome, $"Got a genome smaller than {minGenome} ({intGenome})");
                    Assert.IsTrue(intGenome <= maxGenome, $"Got a genome bigger than {maxGenome} ({intGenome})");
                    recivedGenomes.Add(intGenome);
                }
            }
            for (int i = minGenome; i < maxGenome; i++)
                Assert.IsTrue(recivedGenomes.Contains(i), $"Didn't get {i}");
        }
    }
}
