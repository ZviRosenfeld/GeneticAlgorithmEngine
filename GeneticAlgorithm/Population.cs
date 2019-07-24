using System.Collections;
using System.Collections.Generic;
using System.Text;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class Population : IEnumerable<ChromosomeEvaluationPair>
    {
        private readonly List<ChromosomeEvaluationPair> population;
        private readonly IChromosome[] chromosomes;

        public Population(IChromosome[] population)
        {
            chromosomes = population;
            this.population = new List<ChromosomeEvaluationPair>(population.Length);
            foreach (IChromosome chromosome in population)
                this.population.Add(new ChromosomeEvaluationPair(chromosome));
        }

        public IEnumerator<ChromosomeEvaluationPair> GetEnumerator() => population.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IChromosome[] GetChromosomes() => chromosomes;

        public double[] GetEvaluations()
        {
            var evaluations = new double[population.Count];
            for (int i = 0; i < population.Count; i++)
                evaluations[i] = population[i].Evaluation;
            return evaluations;
        }

        public ChromosomeEvaluationPair this[int i] => population[i];

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var chromosome in chromosomes)
                stringBuilder.AppendLine(chromosome + ", ");
            return stringBuilder.ToString();
        }
    }

    public class ChromosomeEvaluationPair
    {
        public ChromosomeEvaluationPair(IChromosome chromosome)
        {
            Chromosome = chromosome;
        }

        public IChromosome Chromosome { get; set; }
        public double Evaluation { get; set; }
    }
}
