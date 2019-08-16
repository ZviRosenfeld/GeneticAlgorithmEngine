namespace GeneticAlgorithm.Interfaces
{
    public interface IEnvironment
    {
        /// <summary>
        /// UpdateEnvierment is called before the evaluation of a generation begins, and lets you configuration your environment. 
        /// UpdateEnvierment is guaranteed to be called once per generation.
        /// </summary>
        void UpdateEnvierment(IChromosome[] chromosomes, int generation); 
    }
}
