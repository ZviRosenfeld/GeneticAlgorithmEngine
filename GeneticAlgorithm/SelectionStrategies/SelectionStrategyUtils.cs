using System.Linq;
using System.Threading.Tasks;

namespace GeneticAlgorithm.SelectionStrategies
{
    static class SelectionStrategyUtils
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
    }
}
