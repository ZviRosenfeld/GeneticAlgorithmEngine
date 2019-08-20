using GeneticAlgorithm.Interfaces;

namespace GeneticAlgorithm.PopulationConverters
{
    class SamplePopulationConverter : IPopulationConverter
    {
        /// <summary>
        /// This method is will be called once per generation (after the ConvertPopulation method for that generation), so you can use it to remember old data.
        /// </summary>
        public void AddGeneration(Population population)
        {
            double[] evaluations = population.GetEvaluations();
            IChromosome[] chromosomes = population.GetChromosomes();
        }

        /// <summary>
        /// This method will be called every generation after the population is created. 
        /// In this method you can change the population in any way you want.
        /// This allows you to add Lamarckian evolution to your algorithm - that is, let the chromosomes improve themselves before generating the children.
        /// </summary>
        public IChromosome[] ConvertPopulation(IChromosome[] population, int generation, IEnvironment environment)
        {
            IChromosome[] newChromosomes = new IChromosome[population.Length];
            for (int i = 0; i < population.Length; i++)
                newChromosomes[i] = ImproveChromosome(population[i]);

            return newChromosomes;
        }

        private IChromosome ImproveChromosome(IChromosome chromosome)
        {
            // Improve the chromosome in some way here
            return chromosome;
        }
    }
}
