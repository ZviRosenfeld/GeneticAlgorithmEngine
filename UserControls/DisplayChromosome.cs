using GeneticAlgorithm.Interfaces;

namespace UserControls
{
    class DisplayChromosome
    {
        public DisplayChromosome(IChromosome chromosome, double evaluation)
        {
            Evaluation = evaluation;
            Chromosome = chromosome.ToString();
        }

        public string Chromosome { get; }
        public double Evaluation { get; }
    }
}
