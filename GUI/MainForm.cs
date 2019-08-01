using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using GeneticAlgorithm;
using GreatestVectorTests;

namespace GUI
{
    public partial class MainForm : Form
    {
        private const int POPULATION_SIZE = 20;
        
        private readonly GeneticSearchEngine engine;
        private IList<DisplayChromosome> displayChromosomesCollection = new BindingList<DisplayChromosome>();

        public MainForm()
        {
            engine = new GeneticSearchEngineBuilder(POPULATION_SIZE, int.MaxValue, new NumberVectorCrossoverManager(),
                new NumberVectorBassicPopulationGenerator()).Build();
            InitializeComponent();
        }

        private void RunButton_Click(object sender, System.EventArgs e)
        {
            
        }

        private void NextButton_Click(object sender, System.EventArgs e)
        {
            var result = engine.Next();
            Update(result);
        }

        private void Update(GeneticSearchResult result)
        {
            generationLabel.Text = result.Generations.ToString();
            displayChromosomesCollection = new BindingList<DisplayChromosome>();
            foreach (var population in result.Population)
                displayChromosomesCollection.Add(new DisplayChromosome((NumberVectorChromosome) population.Chromosome,
                    population.Evaluation));
            chromosomesDisplay.DataSource = displayChromosomesCollection;
            chromosomesDisplay.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
    }
}
