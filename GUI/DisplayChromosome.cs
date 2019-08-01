using GreatestVectorTests;

namespace GUI
{
    class DisplayChromosome
    {
        public DisplayChromosome(NumberVectorChromosome chromosome, double evaluation)
        {
            Evaluation = evaluation;
            Chromosome = chromosome;
        }
        
        public NumberVectorChromosome Chromosome { get; }
        public double Evaluation { get; }
    }
}
