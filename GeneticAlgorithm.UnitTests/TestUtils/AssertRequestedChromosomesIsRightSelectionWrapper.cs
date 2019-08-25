using System;
using System.Threading;
using GeneticAlgorithm.Interfaces;
using GeneticAlgorithm.SelectionStrategies;

namespace GeneticAlgorithm.UnitTests.TestUtils
{
    public class AssertRequestedChromosomesIsRightSelectionWrapper : ISelectionStrategy
    {
        private readonly RouletteWheelSelection innerSelectionStrategy = new RouletteWheelSelection();
        private int expectedChromosomes;
        private int selectedChromosomes;
        private bool firstGeneration = true;

        public void SetPopulation(Population population, int requestedChromosomes)
        {
            if (!firstGeneration && requestedChromosomes != selectedChromosomes)
                throw new Exception($"Didn't select enough chromosomes. Expected {expectedChromosomes}; selected {selectedChromosomes}");

            innerSelectionStrategy.SetPopulation(population, requestedChromosomes);
            selectedChromosomes = 0;
            expectedChromosomes = requestedChromosomes;
            firstGeneration = false;
        }

        public IChromosome SelectChromosome()
        {
            Interlocked.Increment(ref selectedChromosomes);

            if (selectedChromosomes > expectedChromosomes)
                throw new Exception($"Selected too many chromosomes. Expected {expectedChromosomes}; selected {selectedChromosomes}");

            return innerSelectionStrategy.SelectChromosome();
        }
    }
}
