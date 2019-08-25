using System.Windows.Forms;
using GeneticAlgorithm;
using GreatestVectorTests;

namespace GUI
{
    public partial class MainForm : Form
    {
        private const int POPULATION_SIZE = 20;
        private const int GENERATION = int.MaxValue;
        

        public MainForm()
        {
            InitializeComponent();
            InitializeEngine();
        }

        private void InitializeEngine()
        {
            var engineBuilder = new GeneticSearchEngineBuilder(POPULATION_SIZE, GENERATION,
                    new NumberVectorCrossoverManager(),
                    new NumberVectorBassicPopulationGenerator()).SetMutationProbability(MutationInputBox.GetValue)
                .SetElitePercentage(ElitismInputBox.GetValue);
            searchRunner1.SetEngineBuilder(engineBuilder);
        }

        private void applyButton_Click(object sender, System.EventArgs e)
        {
            InitializeEngine();
        }
    }
}
