using System.ComponentModel;
using System.Windows.Forms;
using GeneticAlgorithm;

namespace UserControls
{
    public partial class GeneticResultDisplay: UserControl
    {
        public GeneticResultDisplay()
        {
            InitializeComponent();
        }

        public void SetResult(GeneticSearchResult result)
        {
            generationLabel.Text = result.Generations.ToString();
            searchTimeLabel.Text = result.SearchTime.ToString();
            environmentLabel.Text = result.Environment?.ToString();

            var displayChromosomesCollection = new BindingList<DisplayChromosome>();
            foreach (var population in result.Population)
                displayChromosomesCollection.Add(new DisplayChromosome(population.Chromosome, population.Evaluation));
            chromosomesView.DataSource = displayChromosomesCollection;
            chromosomesView.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if (result.Environment == null)
                environmentPanel.Hide();
            else
                environmentPanel.Show();

            Refresh();
        }
    }
}
