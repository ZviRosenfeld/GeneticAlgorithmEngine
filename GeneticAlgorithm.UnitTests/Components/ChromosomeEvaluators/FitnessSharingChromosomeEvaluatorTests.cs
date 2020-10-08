using GeneticAlgorithm.Components.ChromosomeEvaluators;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.UnitTests.TestUtils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests.Components.ChromosomeEvaluators
{
    [TestClass]
    public class FitnessSharingChromosomeEvaluatorTests
    {
        [TestMethod]
        [DataRow(-0.1)]
        [DataRow(1.1)]
        [ExpectedException(typeof(GeneticAlgorithmArgumentException), "minDistance")]
        public void FitnessSharingChromosomeEvaluator_BadMinDistance_ThrowException(double minDistance) =>
            new FitnessSharingChromosomeEvaluator(minDistance, 1, (c1, c2) => 0.5);

        [TestMethod]
        [DataRow(0.9)]
        [ExpectedException(typeof(GeneticAlgorithmArgumentException), "distanceScale")]
        public void FitnessSharingChromosomeEvaluator_BadDistanceScale_ThrowException(double distanceScale) =>
            new FitnessSharingChromosomeEvaluator(0.8, distanceScale, (c1, c2) => 0.5);

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        public void FitnessSharingChromosomeEvaluator_RightFitnessWeight(double distanceScale)
        {
            var evaluator = new FitnessSharingChromosomeEvaluator(0.8, distanceScale, (c1, c2) => 0.5);
            evaluator.SetEnvierment(GetEnvironmentWith10Chromosomes());
            var evaluation = evaluator.Evaluate(1.0.CreateChromosome(""));

            Assert.AreEqual(1.0 / (10 * (1 - 0.5 / distanceScale)), evaluation);
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(5)]
        [DataRow(7)]
        public void FitnessSharingChromosomeEvaluator_IgnoreFarChromosomes(int chromosomesToIgnore)
        {
            var evaluator = new FitnessSharingChromosomeEvaluator(0.8, 1, (c1, c2) => c2.Evaluate() <= chromosomesToIgnore ? 0.9 : 0.5);
            evaluator.SetEnvierment(GetEnvironmentWith10Chromosomes());
            var evaluation = evaluator.Evaluate(1.0.CreateChromosome(""));

            Assert.AreEqual(1.0 / (0.5 * (10 - chromosomesToIgnore)),  evaluation);
        }

        private DefaultEnvironment GetEnvironmentWith10Chromosomes()
        {
            var environment = new DefaultEnvironment();
            environment.UpdateEnvierment(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ToChromosomes(), 1);
            return environment;
        }
    }
}
