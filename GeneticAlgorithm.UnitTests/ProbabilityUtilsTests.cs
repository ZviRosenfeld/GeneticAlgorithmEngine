using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            trueCount.AssertIsWithinRange(attempts * probability - margernOnError, attempts * probability + margernOnError);
            falseCount.AssertIsWithinRange(attempts * (1 - probability) - margernOnError, attempts * (1 - probability) + margernOnError);
        }

        [TestMethod]
        [DataRow(-0.1)]
        [DataRow(1.1)]
        [ExpectedException(typeof(InternalSearchException), "code 1008")]
        public void P_ExceptionsTest(double probability) =>
            ProbabilityUtils.P(probability);
    }
}
