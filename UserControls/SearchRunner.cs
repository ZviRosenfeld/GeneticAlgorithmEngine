using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeneticAlgorithm;

namespace UserControls
{
    public partial class SearchRunner : UserControl
    {
        public GeneticSearchEngineBuilder engineBuilder { get; set; }
        public GeneticSearchEngine engine;
        private bool shouldPause = false;
        private readonly object shouldPauseLock = new object();
        
        public SearchRunner()
        {
            InitializeComponent();

            puaseButton.Enabled = false;
            runButton.Enabled = false;
            nextButton.Enabled = false;
            restartButton.Enabled = false;
            renewPopulationButton.Enabled = false;
        }

        public void SetEngineBuilder(GeneticSearchEngineBuilder engineBuilder)
        {
            this.engineBuilder = engineBuilder;
            InitializeEngine();
            SetButtonsState(EngineState.Puased);
        }

        private void InitializeEngine()
        {
            engine = engineBuilder.Build();
            var result = engine.Next(); // Create the initial population;
            geneticResultDisplay.SetResult(result);
        }

        private async void runButton_Click(object sender, EventArgs e)
        {
            SetButtonsState(EngineState.Running);

            lock (shouldPauseLock)
                shouldPause = false;

            while (!shouldPause)
            {
                var result = await Task.Run(() => engine.Next());
                lock (shouldPauseLock)
                    shouldPause = shouldPause || result.IsCompleted;
                geneticResultDisplay.SetResult(result);
            }

            SetButtonsState(EngineState.Puased);
        }
        private void SetButtonsState(EngineState engineState)
        {
            puaseButton.Enabled = engineState == EngineState.Running;
            runButton.Enabled = engineState == EngineState.Puased;
            nextButton.Enabled = engineState == EngineState.Puased;
            restartButton.Enabled = engineState == EngineState.Puased;
            renewPopulationButton.Enabled = engineState == EngineState.Puased;
        }

        private void nextButton_Click(object sender, EventArgs e)
        {
            var result = engine.Next();
            geneticResultDisplay.SetResult(result);
        }

        private void puaseButton_Click(object sender, EventArgs e)
        {
            lock (shouldPauseLock)
                shouldPause = true;
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            InitializeEngine();
        }

        private void renewPopulationButton_Click(object sender, EventArgs e)
        {
            try
            {
                var result = engine.RenewPopulation(double.Parse(renewPopulationInputBox.Text) / 100);
                geneticResultDisplay.SetResult(result);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Oops");
            }
        }
    }
}
