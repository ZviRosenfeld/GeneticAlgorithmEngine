using System.Collections.Generic;
using GeneticAlgorithm.Components.Chromosomes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.CrossoverManagers
{
    static class Utils
    {
        public static List<int> K_CrossoverGetCrossoverPointsAndAssertThatGenomesAreRight(VectorChromosome<int> newChromosome, VectorChromosome<int> chromosome2, VectorChromosome<int> chromosome1)
        {
            var crossoverPoints = new List<int>();
            var takingFromFirstChromosome = true;
            for (int i = 0; i < chromosome1.Length; i++)
            {
                if (takingFromFirstChromosome)
                {
                    if (newChromosome[i] == chromosome2[i])
                    {
                        crossoverPoints.Add(i);
                        takingFromFirstChromosome = !takingFromFirstChromosome;
                    }
                    else if (newChromosome[i] != chromosome1[i])
                        Assert.Fail($"Got Genome that dosn't seem to have came from anywhere. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome} ");
                }
                else
                {
                    if (newChromosome[i] == chromosome1[i])
                    {
                        crossoverPoints.Add(i);
                        takingFromFirstChromosome = !takingFromFirstChromosome;
                    }
                    else if (newChromosome[i] != chromosome2[i])
                        Assert.Fail($"Got Genome that dosn't seem to have came from anywhere. 1: {chromosome1}; 2 {chromosome2}; newChromosome {newChromosome} ");
                }
            }
            for (int j = chromosome1.Length; j < chromosome2.Length && j < newChromosome.GetVector().Length; j++)
                Assert.AreEqual(chromosome2[j], newChromosome[j]);

            return crossoverPoints;
        }
    }
}
