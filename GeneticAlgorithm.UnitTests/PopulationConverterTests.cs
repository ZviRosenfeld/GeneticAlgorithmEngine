using System.Linq;
using FakeItEasy;
using GeneticAlgorithm.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithm.UnitTests
{
    [TestClass]
    public class PopulationConverterTests
    {
        [TestMethod]
        public void ConvertPopulationViaManager_CheckPopulationConverted()
        {
            var testPopulationManager = new TestPopulationManager(new double[] {1, 1, 1, 1, 1});
            var populationConverter = A.Fake<IPopulationConverter>();
            var convertedPopulation = new double[] {2, 2, 2, 2, 2};
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._))
                .Returns(convertedPopulation.ToChromosomes("Converted Chromosomes"));
            var engine = new TestGeneticSearchEngineBuilder(convertedPopulation.Length, 10, testPopulationManager)
                .AddPopulationConverter(populationConverter).Build();

            var result = engine.Next();

            for (var i = 0; i < result.Population.GetChromosomes().Length; i++)
                Assert.AreEqual(convertedPopulation[i], result.Population.GetChromosomes()[i].Evaluate(), "Wrong chromosome");
        }

        [TestMethod]
        public void ConvertPopulationViaManager_CheckWeGetTheRightPopulationAndGeneration()
        {
            var generation = 0;
            var testPopulationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var populationConverter = A.Fake<IPopulationConverter>();
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._))
                .ReturnsLazily((IChromosome[] c, int g, IEnvironment e) =>
                {
                    generation++;
                    Assert.AreEqual(generation, g, "We got the wrong generation");
                    foreach (var chromosome in c)
                        Assert.AreEqual(1 , chromosome.Evaluate(), "Got wrong chromosome");
                    return c;
                });
            var engine = new TestGeneticSearchEngineBuilder(5, 10, testPopulationManager)
                .AddPopulationConverter(populationConverter).Build();

            for (int i = 0; i < 5; i++)
                engine.Next();
        }

        [TestMethod]
        public void MultipleConvertPopulationManagers_ManagersCalledAndInOrder()
        {
            var testPopulationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var populationConverter1 = GetOrderedPopulationManager(1);
            var populationConverter2 = GetOrderedPopulationManager(2);
            var populationConverter3 = GetOrderedPopulationManager(3);
            var engine = new TestGeneticSearchEngineBuilder(5, 10, testPopulationManager)
                .AddPopulationConverter(populationConverter1)
                .AddPopulationConverter(populationConverter2)
                .AddPopulationConverter(populationConverter3)
                .Build();

            var result = engine.Next();
            foreach (var chromosome in result.Population.GetChromosomes())
                Assert.AreEqual(4, chromosome.Evaluate(), "Ended up with wrong chromosome");
        }

        private IPopulationConverter GetOrderedPopulationManager(int number)
        {
            var populationConverter = A.Fake<IPopulationConverter>();
            A.CallTo(() => populationConverter.ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._))
                .ReturnsLazily((IChromosome[] c, int g, IEnvironment e) =>
                {
                    foreach (var chromosome in c)
                        Assert.AreEqual(number, chromosome.Evaluate(), "Got wrong chromosome");
                    
                    return c.Select(chromosome => chromosome.Evaluate() + 1).ToArray().ToChromosomes();
                });
            return populationConverter;
        }
        
        [TestMethod]
        public void ConvertPopulation_PopulationConverted()
        {
            var newPopulationEvaluation = new double[] {2, 2, 2};
            var populationManager = new TestPopulationManager(new double[] {1, 1, 1});
            var engine = new TestGeneticSearchEngineBuilder(3, 10, populationManager).Build();
            engine.Next();

            var newPopulation = engine.SetCurrentPopulation(newPopulationEvaluation.ToChromosomes("Converted"));
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);

            newPopulation = engine.GetCurrentPopulation();
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);
        }

        [TestMethod]
        public void ConvertPopulation_NextGetsRightPopulation()
        {
            var newPopulationEvaluation = new double[] { 2, 2, 2 };
            var populationManager = new TestPopulationManager(new double[] { 1, 1, 1 }, c => c.Evaluate());
            var engine = new TestGeneticSearchEngineBuilder(3, 10, populationManager).Build();
            engine.Next();

            var newPopulation = engine.SetCurrentPopulation(newPopulationEvaluation.ToChromosomes("Converted"));
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);

            newPopulation = engine.Next();
            newPopulation.Population.GetChromosomes().AssertHasEvaluation(newPopulationEvaluation);
        }

        [TestMethod]
        public void AddMultipleConvertPopulationManagers_AllManagersAreCalled()
        {
            bool manager1Called = false, manager2Called = false;
            var testPopulationManager = new TestPopulationManager(new double[] { 1, 1, 1, 1, 1 });
            var managers = new[] { A.Fake<IPopulationConverter>(), A.Fake<IPopulationConverter>() };
            A.CallTo(() => managers[0].ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._))
                .Invokes((IChromosome[] c, int g, IEnvironment e) => manager1Called = true);
            A.CallTo(() => managers[1].ConvertPopulation(A<IChromosome[]>._, A<int>._, A<IEnvironment>._))
                .Invokes((IChromosome[] c, int g, IEnvironment e) => manager2Called = true);

            var engine = new TestGeneticSearchEngineBuilder(5, 10, testPopulationManager)
                .AddPopulationConverters(managers)
                .Build();

            engine.Next();

            Assert.IsTrue(manager1Called);
            Assert.IsTrue(manager2Called);
        }
    }
}
