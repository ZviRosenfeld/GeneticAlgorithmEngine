using GeneticAlgorithm.Interfaces;

namespace Environment
{
    class ChromosomeEvaluator : IChromosomeEvaluator
    {
        private MyEnvironment environment;

        public void SetEnvierment(IEnvironment environment)
        {
            this.environment = (MyEnvironment)environment;
        }

        public double Evaluate(IChromosome chromosome)
        {
            var type = ((MyChromosome) chromosome).Type;

            double baseEvaluation = type == ChromosomeType.OProducer ? environment.OC2 : environment.O;
            return (baseEvaluation + 1) / EnvironmentForm.POPULATION_SIZE;
        }
    }
}
