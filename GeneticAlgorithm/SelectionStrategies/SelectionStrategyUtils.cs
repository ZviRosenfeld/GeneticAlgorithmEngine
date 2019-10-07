using System.Linq;
using System.Threading.Tasks;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.SelectionStrategies
{
    public static class SelectionStrategyUtils
    {
        public static double[] GetNormilizeEvaluations(this Population population)
        {
            population = population.Clone();
            var total = population.GetEvaluations().Sum();
            Parallel.ForEach(population, chromosome =>
            {
                chromosome.Evaluation = chromosome.Evaluation / total;
            });
            return population.GetEvaluations();
        }

        /// <summary>
        /// Returns a population object with only the best n chromosomes
        /// </summary>
        public static Population GetBestChromosomes(this Population population, int n)
        {
            if (n == population.Count())
                return population;

            if (n <= 0 || n > population.Count())
                throw new InternalSearchException($"Code 1006 (requested {n} best chromosomes; population size is {population.Count()})");

            var min = population.GetEvaluations().OrderByDescending(x => x).Take(n).Last();
            var bestChromosomes = new IChromosome[n];
            int index = 0;
            double[] evaluations = new double[n];
            foreach (var chromosome in population)
            {
                if (chromosome.Evaluation >= min)
                {
                    bestChromosomes[index] = chromosome.Chromosome;
                    evaluations[index] = chromosome.Evaluation;
                    index++;
                }
                if (index >= n)
                {
                    var newPopulation = new Population(bestChromosomes);
                    for (int i = 0; i < n; i++)
                        newPopulation[i].Evaluation = evaluations[i];

                    return newPopulation;
                }
            }

            throw new InternalSearchException("Code 1007 (not enough best chromosomes found)");
        }
    }
}
