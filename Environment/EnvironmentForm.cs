using System.Windows.Forms;
using GeneticAlgorithm;

namespace Environment
{
    public partial class EnvironmentForm : Form
    {
        public const int POPULATION_SIZE = 100;
        public const int GENERATIONS = 100;


        public EnvironmentForm()
        {
            InitializeComponent();
            InitializeEngine();
        }

        private void InitializeEngine()
        {
            var engineBuidler = new GeneticSearchEngineBuilder(POPULATION_SIZE, GENERATIONS, new CrossoverManager(),
                    new PopulationGenerator()).SetCustomChromosomeEvaluator(new ChromosomeEvaluator())
                .SetEnvironment(new MyEnvironment()).SetMutationProbability(0.01);
            searchRunner1.SetEngineBuilder(engineBuidler);
        }
    }
}
