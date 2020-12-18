using System;
using System.Collections.Generic;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

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
            var marginOfError = attempts * 0.1;
            for (int i = 0; i < attempts; i++)
            {
                if (ProbabilityUtils.P(probability))
                    trueCount++;
                else
                    falseCount++;
            }

            trueCount.AssertIsWithinRange(attempts * probability, marginOfError);
            falseCount.AssertIsWithinRange(attempts * (1 - probability), marginOfError);
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
            var numbers = ProbabilityUtils.SelectKRandomNumbersNonRepeating(till, k);

            Assert.AreEqual(k, numbers.Count, "Didn't get enough numbers");
            foreach (var number in numbers)
                Assert.AreEqual(1, numbers.Count(n => n == number), "Found the same index more then once");
            foreach (var number in numbers)
                Assert.IsTrue(number < till, $"{nameof(number)} = {number}, which is bigger than {nameof(till)} = {till}");
        }

        [TestMethod]
        public void SelectKRandomNumbers_NumbersAreRandom()
        {
            var till = 20;
            var allNumbers = new List<int>();
            for (int i = 0; i < till * 5; i++)
                allNumbers.AddRange(ProbabilityUtils.SelectKRandomNumbersNonRepeating(till, 2));

            // All numbers are generated
            for (int i = 0; i < till; i++)
                Assert.IsTrue(allNumbers.Contains(i), $"{nameof(allNumbers)} doesn't contain {i}");

            // No number is generated more than 20% of the time
            foreach (var number in allNumbers)
                Assert.IsTrue(allNumbers.Count(n => n == number) < till * 5 * 2 * 0.2);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(10)]
        public void SelectKRandomElements_KElementsSelectedFromElements(int k)
        {
            var allElements = new[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            var selectedElements = allElements.SelectKRandomElementsNonRepeating(k);

            Assert.AreEqual(k, selectedElements.Length, "Didn't get enough elements");
            foreach (var element in selectedElements)
                Assert.AreEqual(1, selectedElements.Count(n => n == element), "Found the same element more then once");
            foreach (var element in selectedElements)
                Assert.IsTrue(allElements.Contains(element), $"Got {nameof(element)} {element} which isn't in {nameof(allElements)}");
        }

        [TestMethod]
        public void SelectKRandomElements_ElementsAreRandom()
        {
            var allElements = new[] {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"};
            var length = allElements.Length;
            var slelectedElements = new List<string>();
            for (int i = 0; i < length * 5; i++)
                slelectedElements.AddRange(allElements.SelectKRandomElementsNonRepeating(2));

            // All elements are generated
            for (int i = 0; i < length; i++)
                Assert.IsTrue(slelectedElements.Contains(allElements[i]), $"{nameof(slelectedElements)} doesn't contain {slelectedElements[i]}");

            // No element is generated more than 20% of the time
            foreach (var element in slelectedElements)
            {
                var timesSelected = slelectedElements.Count(e => e.Equals(element));
                Assert.IsTrue(timesSelected < length * 5 * 2 * 0.2, $"{nameof(element)} {element} selected too many times ({timesSelected}).");
            }
        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(10, 5)]
        [DataRow(40, 5)]
        public void GaussianDistribution_CommonNumbersAreMoreLikely(double mean, double sd)
        {
            var smallCount = 0;
            var bigCount = 0;
            for (int i = 0; i < 100; i++)
            {
                var value = ProbabilityUtils.GaussianDistribution(sd, mean);
                Console.WriteLine(value);
                if (value >= mean + sd || value <= mean - sd) bigCount++;
                else smallCount++;
            }

            Assert.IsTrue(smallCount > bigCount, $"Got to many big genomes. Big numbers = {bigCount}; small numbers = {smallCount}");
        }

        [TestMethod]
        public void GetRandomDouble_CheckIsThreadSafe()
        {
            var receivedValues = new ConcurrentDictionary<double, byte>();
            var tasks = new Task[1000];
            for (var i = 0; i < 1000; i++)
            {
                tasks[i] = Task.Run(() => 
                {
                    var value = ProbabilityUtils.GetRandomDouble();
                    var addedValue = receivedValues.TryAdd(value, 1);
                    Assert.IsTrue(addedValue, $"Value {value} already in dictionary");
                });
            }
            Task.WaitAll(tasks);
        }

        [TestMethod]
        public void P_CheckIsThreadSafe()
        {
            var attempts = 1000;
            var trueCount = 0;
            var falseCount = 0;
            var tasks = new Task[attempts];
            for (var i = 0; i < attempts; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    var value = ProbabilityUtils.P(0.5);
                    if (value) Interlocked.Increment(ref trueCount);
                    else Interlocked.Increment(ref falseCount);
                });
            }
            Task.WaitAll(tasks);

            var marginOfError = attempts * 0.1;
            trueCount.AssertIsWithinRange(attempts * 0.5, marginOfError);
            falseCount.AssertIsWithinRange(attempts * 0.5, marginOfError);
        }
    }
}
