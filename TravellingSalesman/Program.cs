using System;
using System.Collections.Generic;
using GeneticAlgorithm;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.Interfaces;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Components.PopulationGenerators;
using GeneticAlgorithm.Interfaces;

namespace TravellingSalesman
{
    class Program
    {
        private const int POPULATION_SIZE = 100;
        private const int GENERATIONS = 100;
        private static string[] cities = {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j"};
        private static Dictionary<string, Tuple<int, int>> locations = new Dictionary<string, Tuple<int, int>>
        {
            {"a", new Tuple<int, int>(10, 10)},
            {"b", new Tuple<int, int>(10, 200)},
            {"c", new Tuple<int, int>(1000, 10)},
            {"d", new Tuple<int, int>(500, 476)},
            {"e", new Tuple<int, int>(200, 800)},
            {"f", new Tuple<int, int>(80, 199)},
            {"g", new Tuple<int, int>(455, 78)},
            {"h", new Tuple<int, int>(511, 907)},
            {"i", new Tuple<int, int>(230, 400)},
            {"j", new Tuple<int, int>(611, 801)},
        };
        private static DistanceCalclator distanceCalclator = new DistanceCalclator(locations);

        static void Main(string[] args)
        {
            Console.WriteLine("Started!");

            IMutationManager<string> mutationManager = new ExchangeMutationManager<string>();
            IEvaluator evaluator = new DistanceEvaluator(locations);
            ICrossoverManager crossoverManager = new OrderCrossover<string>(mutationManager, evaluator);
            IPopulationGenerator populationGenerator =
                new AllElementsVectorChromosomePopulationGenerator<string>(cities, mutationManager, evaluator);

            GeneticSearchEngine engine =
                new GeneticSearchEngineBuilder(POPULATION_SIZE, GENERATIONS, crossoverManager, populationGenerator)
                    .SetMutationProbability(0.1).SetElitePercentage(0.02).Build();
            engine.OnNewGeneration += (Population p, IEnvironment e) => PrintBestChromosome(p);

            GeneticSearchResult result = engine.Run();

            Console.WriteLine("Finished!");
            Console.WriteLine(result.BestChromosome + ": " + distanceCalclator.GetDistance(result.BestChromosome));
            
            Console.ReadLine();
        }

        private static void PrintBestChromosome(Population population)
        {
            var bestEvaluation = 0.0;
            ChromosomeEvaluationPair bestPair = null;
            foreach (ChromosomeEvaluationPair chromosomeEvaluationPair in population)
                if (chromosomeEvaluationPair.Evaluation > bestEvaluation)
                {
                    bestEvaluation = chromosomeEvaluationPair.Evaluation;
                    bestPair = chromosomeEvaluationPair;
                }

            Console.WriteLine(bestPair.Chromosome + ": " + distanceCalclator.GetDistance(bestPair.Chromosome));
        }
    }
}
