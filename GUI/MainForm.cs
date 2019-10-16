using System.Windows.Forms;
using GeneticAlgorithm;
using GeneticAlgorithm.Components.CrossoverManagers;
using GeneticAlgorithm.Components.MutationManagers;
using GeneticAlgorithm.Components.PopulationGenerators;

namespace GUI
{
    public partial class MainForm : Form
    {
        private const int POPULATION_SIZE = 20;
        private const int VECTOR_SIZE = 10;
        private const int GENERATION = int.MaxValue;
        

        public MainForm()
        {
            InitializeComponent();
            InitializeEngine();
        }

        private void InitializeEngine()
        {
            var mutationManager = new UniformMutationManager(0, 100);
            var evaluator = new BasicEvaluator();
            var populationGenerator =
                new IntVectorChromosomePopulationGenerator(VECTOR_SIZE, 0, 1, mutationManager, evaluator);
            var crossoverManager = new SinglePointCrossoverManager<int>(mutationManager, evaluator);
            var engineBuilder =
                new GeneticSearchEngineBuilder(POPULATION_SIZE, GENERATION, crossoverManager, populationGenerator)
                    .SetMutationProbability(MutationInputBox.GetValue).SetElitePercentage(ElitismInputBox.GetValue);
            searchRunner1.SetEngineBuilder(engineBuilder);
        }

        private void applyButton_Click(object sender, System.EventArgs e)
        {
            InitializeEngine();
        }
    }
}
