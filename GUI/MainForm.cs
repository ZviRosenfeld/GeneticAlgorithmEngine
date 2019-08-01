using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithm;
using GreatestVectorTests;

namespace GUI
{
    public partial class MainForm : Form
    {
        private const int POPULATION_SIZE = 20;
        private const int GENERATION = int.MaxValue;

        private bool shouldPause = false;
        private readonly object shouldPauseLock = new object(); 
        private readonly GeneticSearchEngine engine;
        private IList<DisplayChromosome> displayChromosomesCollection = new BindingList<DisplayChromosome>();

        public MainForm()
        {
            engine = new GeneticSearchEngineBuilder(POPULATION_SIZE, GENERATION, new NumberVectorCrossoverManager(),
                new NumberVectorBassicPopulationGenerator()).Build();
            InitializeComponent();
        }

        private async void RunButton_Click(object sender, System.EventArgs e)
        {
            SetButtonsState(EngineState.Running);

            lock (shouldPauseLock)
                shouldPause = false;
            
            while (!shouldPause)
            {
                var result = await Task.Run(() => engine.Next());
                lock (shouldPauseLock)
                    shouldPause = shouldPause || result.IsCompleted;
                Update(result);
            }

            SetButtonsState(EngineState.Puased);
        }

        private GeneticSearchResult RunSingleGeneration()
        {
            var result = engine.Next();
            lock (shouldPauseLock)
                shouldPause = shouldPause || result.IsCompleted;
            return result;
        }

        private void SetButtonsState(EngineState engineState)
        {
            PuaseButton.Enabled = engineState == EngineState.Running;
            RunButton.Enabled = engineState == EngineState.Puased;
            NextButton.Enabled = engineState == EngineState.Puased;
        }

        private void NextButton_Click(object sender, System.EventArgs e)
        {
            var result = engine.Next();
            Update(result);
        }

        private void Update(GeneticSearchResult result)
        {
            generationLabel.Text = result.Generations.ToString();
            generationLabel.Refresh();
            displayChromosomesCollection = new BindingList<DisplayChromosome>();
            foreach (var population in result.Population)
                displayChromosomesCollection.Add(new DisplayChromosome((NumberVectorChromosome) population.Chromosome,
                    population.Evaluation));
            chromosomesDisplay.DataSource = displayChromosomesCollection;
            chromosomesDisplay.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void PuaseButton_Click(object sender, System.EventArgs e)
        {
            lock (shouldPauseLock)
                shouldPause = true;
        }
    }
}
