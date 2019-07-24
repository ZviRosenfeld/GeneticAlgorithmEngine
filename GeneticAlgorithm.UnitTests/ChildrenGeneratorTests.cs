using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class ChildrenGeneratorTests
    {
        /*
        [TestMethod]
        public void ChooseParent_MostLieklyToChooseBestChromosome()
        {
            const int tries = 1000;
            const double chromosome1Probability = 0.1, chromosome2Probability = 0.3, chromosome3Probability = 0.6;
            int chromosome1Counter = 0, chromosome2Counter = 0, chromosome3Counter = 0;
            var evaluation = new[] { chromosome1Probability, chromosome2Probability, chromosome3Probability };
            var population = new[] { chromosome1, chromosome2, chromosome3 };

            for (int i = 0; i < tries; i++)
            {
                var result = ChildrenGenerator.ChooseParent(population, evaluation);
                if (result == chromosome1) chromosome1Counter++;
                if (result == chromosome2) chromosome2Counter++;
                if (result == chromosome3) chromosome3Counter++;
            }

            AssertIsWithinRange(chromosome1Counter, chromosome1Probability, tries, nameof(chromosome1Counter));
            AssertIsWithinRange(chromosome2Counter, chromosome2Probability, tries, nameof(chromosome2Counter));
            AssertIsWithinRange(chromosome3Counter, chromosome3Probability, tries, nameof(chromosome3Counter));
        }

        private void AssertIsWithinRange(int value, double probability, int tries, string valueName)
        {
            const double errorMargin = 0.05;
            var expected = probability * tries;
            var min = expected - errorMargin * tries;
            var max = expected + errorMargin * tries;

            Assert.IsTrue(value > min, $"Value ({value}) in smaller than min ({min}) for {valueName}");
            Assert.IsTrue(value < max, $"Value ({value}) in greater than max ({max}) for {valueName}");
        }*/
    }
}
