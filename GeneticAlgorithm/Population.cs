using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using GeneticAlgorithm.Exceptions;
using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public class Population : IEnumerable<ChromosomeEvaluationPair>
    {
        private readonly List<ChromosomeEvaluationPair> population;
        private readonly IChromosome[] chromosomes;
        private bool evaluationChanged = true;

        public Population(IChromosome[] population)
        {
            chromosomes = population;
            this.population = new List<ChromosomeEvaluationPair>(population.Length);
            foreach (IChromosome chromosome in population)
                this.population.Add(new ChromosomeEvaluationPair(chromosome, () => evaluationChanged = true));
        }

        public IEnumerator<ChromosomeEvaluationPair> GetEnumerator() => population.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IChromosome[] GetChromosomes() => chromosomes;
        
        private double[] evaluations;
        public double[] GetEvaluations()
        {
            if (!evaluationChanged)
                return evaluations ??
                       throw new InternalSearchException($"Code 1002 ({nameof(evaluations)} should not be null)");

            evaluations = new double[population.Count];
            for (int i = 0; i < population.Count; i++)
                evaluations[i] = population[i].Evaluation;
            evaluationChanged = false;
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
        private double evaluation;
        private Action onEvaluationChanged;
        
        public ChromosomeEvaluationPair(IChromosome chromosome, Action onEvaluationChanged)
        {
            Chromosome = chromosome;
            this.onEvaluationChanged = onEvaluationChanged;
        }

        public IChromosome Chromosome { get; }

        public double Evaluation
        {
            get => evaluation;
            set
            {
                onEvaluationChanged();
                evaluation = value;
            }
        }
    }
}
