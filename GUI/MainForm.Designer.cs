using System.Windows.Forms;

namespace GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.controlPanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.RunButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.PuaseButton = new System.Windows.Forms.Button();
            this.RestartButton = new System.Windows.Forms.Button();
            this.infoPanel = new System.Windows.Forms.Panel();
            this.SearchTimeLable = new System.Windows.Forms.Label();
            this.generationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chromosomesDisplay = new System.Windows.Forms.DataGridView();
            this.Chromosome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Evaluation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.MutationInputBox = new GUI.DoubleInputBox();
            this.ElitismInputBox = new GUI.DoubleInputBox();
            this.controlPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.infoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chromosomesDisplay)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.CadetBlue;
            this.controlPanel.Controls.Add(this.buttonPanel);
            this.controlPanel.Controls.Add(this.infoPanel);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(200, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(800, 57);
            this.controlPanel.TabIndex = 0;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.RunButton);
            this.buttonPanel.Controls.Add(this.NextButton);
            this.buttonPanel.Controls.Add(this.PuaseButton);
            this.buttonPanel.Controls.Add(this.RestartButton);
            this.buttonPanel.Location = new System.Drawing.Point(4, 4);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(334, 53);
            this.buttonPanel.TabIndex = 7;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(3, 3);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 35);
            this.RunButton.TabIndex = 0;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(84, 3);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 35);
            this.NextButton.TabIndex = 1;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PuaseButton
            // 
            this.PuaseButton.Enabled = false;
            this.PuaseButton.Location = new System.Drawing.Point(165, 3);
            this.PuaseButton.Name = "PuaseButton";
            this.PuaseButton.Size = new System.Drawing.Size(75, 35);
            this.PuaseButton.TabIndex = 2;
            this.PuaseButton.Text = "Puase";
            this.PuaseButton.UseVisualStyleBackColor = true;
            this.PuaseButton.Click += new System.EventHandler(this.PuaseButton_Click);
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(246, 3);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(75, 35);
            this.RestartButton.TabIndex = 6;
            this.RestartButton.Text = "Restart";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.SearchTimeLable);
            this.infoPanel.Controls.Add(this.generationLabel);
            this.infoPanel.Controls.Add(this.label1);
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.infoPanel.Location = new System.Drawing.Point(429, 0);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(371, 57);
            this.infoPanel.TabIndex = 5;
            // 
            // SearchTimeLable
            // 
            this.SearchTimeLable.AutoSize = true;
            this.SearchTimeLable.Dock = System.Windows.Forms.DockStyle.Right;
            this.SearchTimeLable.Location = new System.Drawing.Point(238, 0);
            this.SearchTimeLable.Name = "SearchTimeLable";
            this.SearchTimeLable.Size = new System.Drawing.Size(133, 20);
            this.SearchTimeLable.TabIndex = 5;
            this.SearchTimeLable.Text = "SearchTimeLable";
            // 
            // generationLabel
            // 
            this.generationLabel.AutoSize = true;
            this.generationLabel.Location = new System.Drawing.Point(108, 9);
            this.generationLabel.Name = "generationLabel";
            this.generationLabel.Size = new System.Drawing.Size(18, 20);
            this.generationLabel.TabIndex = 4;
            this.generationLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Generation:";
            // 
            // chromosomesDisplay
            // 
            this.chromosomesDisplay.AllowUserToAddRows = false;
            this.chromosomesDisplay.AllowUserToDeleteRows = false;
            this.chromosomesDisplay.AllowUserToOrderColumns = true;
            this.chromosomesDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chromosomesDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromosomesDisplay.Location = new System.Drawing.Point(200, 57);
            this.chromosomesDisplay.Name = "chromosomesDisplay";
            this.chromosomesDisplay.ReadOnly = true;
            this.chromosomesDisplay.RowTemplate.Height = 28;
            this.chromosomesDisplay.Size = new System.Drawing.Size(800, 543);
            this.chromosomesDisplay.TabIndex = 1;
            // 
            // Chromosome
            // 
            this.Chromosome.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Chromosome.HeaderText = "Chromosome";
            this.Chromosome.Name = "Chromosome";
            this.Chromosome.ReadOnly = true;
            // 
            // Evaluation
            // 
            this.Evaluation.HeaderText = "Evaluation";
            this.Evaluation.Name = "Evaluation";
            this.Evaluation.ReadOnly = true;
            // 
            // settingsPanel
            // 
            this.settingsPanel.Controls.Add(this.flowLayoutPanel1);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(200, 600);
            this.settingsPanel.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.MutationInputBox);
            this.flowLayoutPanel1.Controls.Add(this.ElitismInputBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 600);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // MutationInputBox
            // 
            this.MutationInputBox.LableText = "Mutation";
            this.MutationInputBox.Location = new System.Drawing.Point(3, 3);
            this.MutationInputBox.Name = "MutationInputBox";
            this.MutationInputBox.Size = new System.Drawing.Size(164, 44);
            this.MutationInputBox.TabIndex = 0;
            // 
            // ElitismInputBox
            // 
            this.ElitismInputBox.LableText = "Elitism";
            this.ElitismInputBox.Location = new System.Drawing.Point(3, 53);
            this.ElitismInputBox.Name = "ElitismInputBox";
            this.ElitismInputBox.Size = new System.Drawing.Size(164, 49);
            this.ElitismInputBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 600);
            this.Controls.Add(this.chromosomesDisplay);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.settingsPanel);
            this.MinimumSize = new System.Drawing.Size(1000, 400);
            this.Name = "MainForm";
            this.Text = "GA";
            this.controlPanel.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chromosomesDisplay)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel controlPanel;
        private Button RunButton;
        private Button PuaseButton;
        private Button NextButton;
        private Label label1;
        private Label generationLabel;
        private Panel infoPanel;
        private DataGridView chromosomesDisplay;
        private DataGridViewTextBoxColumn Chromosome;
        private DataGridViewTextBoxColumn Evaluation;
        private Button RestartButton;
        private Label SearchTimeLable;
        private FlowLayoutPanel buttonPanel;
        private Panel settingsPanel;
        private DoubleInputBox MutationInputBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private DoubleInputBox ElitismInputBox;
    }
}

