using System.Collections.Generic;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class ProbabilityUtilsTests
    {
        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(0.2)]
        [DataRow(0.5)]
        public void P_Test(double probability)
        {
            var trueCount = 0;
            var falseCount = 0;
            var attempts = 1000;
            var margernOnError = attempts * 0.1;
            for (int i = 0; i < attempts; i++)
            {
                if (ProbabilityUtils.P(probability))
                    trueCount++;
                else
                    falseCount++;
            }

            trueCount.AssertIsWithinRange(attempts * probability, margernOnError);
            falseCount.AssertIsWithinRange(attempts * (1 - probability), margernOnError);
        }

        [TestMethod]
        [DataRow(-0.1)]
        [DataRow(1.1)]
        [ExpectedException(typeof(InternalSearchException), "code 1008")]
        public void P_ExceptionsTest(double probability) =>
            ProbabilityUtils.P(probability);

        [TestMethod]
        [DataRow(5, 5)]
        [DataRow(10, 1)]
        [DataRow(10, 3)]
        public void SelectKRandomNumbers_KNumbersSelectedTillTill(int till, int k)
        {
            var numbers = ProbabilityUtils.SelectKRandomNumbers(till, k);

            Assert.AreEqual(k, numbers.Count, "Didn't get enough numbers");
            foreach (var number in numbers)
                Assert.AreEqual(1, numbers.Count(n => n == number), "Found the same index more then once");
        }

        [TestMethod]
        public void SelectKRandomNumbers_NumbersAreRandom()
        {
            var till = 20;
            var allNumbers = new List<int>();
            for (int i = 0; i < till * 5; i++)
                allNumbers.AddRange(ProbabilityUtils.SelectKRandomNumbers(till, 2));

            for (int i = 0; i < till; i++)
                Assert.IsTrue(allNumbers.Contains(i), $"{nameof(allNumbers)} dosn't contain {i}");
        }
    }
}
