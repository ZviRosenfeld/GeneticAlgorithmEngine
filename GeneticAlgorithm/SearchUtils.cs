using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm
{
    public static class SearchUtils
    {
        public static IChromosome ChooseBest(this Population population)
        {
            double bestEvaluation = -1;
            IChromosome bestChromosome = null;
            foreach (var chromosome in population)
            {
                var evaluation = chromosome.Evaluation;
                if (evaluation > bestEvaluation)
                {
                    bestEvaluation = evaluation;
                    bestChromosome = chromosome.Chromosome;
                }
            }

            return bestChromosome;
        }
        
        public static IChromosome[] Combine(IChromosome[] chromosomes1, IChromosome[] chromosomes2)
        {
            var firstChromosomesLength = chromosomes1.Length;
            var newChromosomes = new IChromosome[firstChromosomesLength + chromosomes2.Length];
            var i = 0;
            for (; i < firstChromosomesLength; i++)
                newChromosomes[i] = chromosomes1[i];
            for (; i < firstChromosomesLength + chromosomes2.Length; i++)
                newChromosomes[i] = chromosomes2[i - firstChromosomesLength];

            return newChromosomes;
        }
    }
}
