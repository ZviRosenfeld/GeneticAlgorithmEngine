using System.Drawing;
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
            this.Chromosome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Evaluation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.MutationInputBox = new GUI.DoubleInputBox();
            this.ElitismInputBox = new GUI.DoubleInputBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.RenewPopulationInputBox = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RenewPopulationButton = new System.Windows.Forms.Button();
            this.resultDisplay = new UserControls.GeneticResultDisplay();
            this.controlPanel.SuspendLayout();
            this.buttonPanel.SuspendLayout();
            this.settingsPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.CadetBlue;
            this.controlPanel.Controls.Add(this.buttonPanel);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(133, 0);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(2);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(554, 37);
            this.controlPanel.TabIndex = 0;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.RunButton);
            this.buttonPanel.Controls.Add(this.NextButton);
            this.buttonPanel.Controls.Add(this.PuaseButton);
            this.buttonPanel.Controls.Add(this.RestartButton);
            this.buttonPanel.Location = new System.Drawing.Point(3, 3);
            this.buttonPanel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(223, 34);
            this.buttonPanel.TabIndex = 7;
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(2, 2);
            this.RunButton.Margin = new System.Windows.Forms.Padding(2);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(50, 23);
            this.RunButton.TabIndex = 0;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(56, 2);
            this.NextButton.Margin = new System.Windows.Forms.Padding(2);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(50, 23);
            this.NextButton.TabIndex = 1;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // PuaseButton
            // 
            this.PuaseButton.Enabled = false;
            this.PuaseButton.Location = new System.Drawing.Point(110, 2);
            this.PuaseButton.Margin = new System.Windows.Forms.Padding(2);
            this.PuaseButton.Name = "PuaseButton";
            this.PuaseButton.Size = new System.Drawing.Size(50, 23);
            this.PuaseButton.TabIndex = 2;
            this.PuaseButton.Text = "Puase";
            this.PuaseButton.UseVisualStyleBackColor = true;
            this.PuaseButton.Click += new System.EventHandler(this.PuaseButton_Click);
            // 
            // RestartButton
            // 
            this.RestartButton.Location = new System.Drawing.Point(164, 2);
            this.RestartButton.Margin = new System.Windows.Forms.Padding(2);
            this.RestartButton.Name = "RestartButton";
            this.RestartButton.Size = new System.Drawing.Size(50, 23);
            this.RestartButton.TabIndex = 6;
            this.RestartButton.Text = "Restart";
            this.RestartButton.UseVisualStyleBackColor = true;
            this.RestartButton.Click += new System.EventHandler(this.RestartButton_Click);
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
            this.settingsPanel.Margin = new System.Windows.Forms.Padding(2);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(133, 510);
            this.settingsPanel.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.MutationInputBox);
            this.flowLayoutPanel1.Controls.Add(this.ElitismInputBox);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(133, 510);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // MutationInputBox
            // 
            this.MutationInputBox.LableText = "Mutation";
            this.MutationInputBox.Location = new System.Drawing.Point(1, 1);
            this.MutationInputBox.Margin = new System.Windows.Forms.Padding(1);
            this.MutationInputBox.Name = "MutationInputBox";
            this.MutationInputBox.Size = new System.Drawing.Size(132, 29);
            this.MutationInputBox.TabIndex = 0;
            // 
            // ElitismInputBox
            // 
            this.ElitismInputBox.LableText = "Elitism";
            this.ElitismInputBox.Location = new System.Drawing.Point(1, 32);
            this.ElitismInputBox.Margin = new System.Windows.Forms.Padding(1);
            this.ElitismInputBox.Name = "ElitismInputBox";
            this.ElitismInputBox.Size = new System.Drawing.Size(109, 32);
            this.ElitismInputBox.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.flowLayoutPanel2);
            this.panel1.Controls.Add(this.RenewPopulationButton);
            this.panel1.Location = new System.Drawing.Point(3, 68);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(125, 68);
            this.panel1.TabIndex = 2;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.RenewPopulationInputBox);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(21, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(85, 30);
            this.flowLayoutPanel2.TabIndex = 3;
            // 
            // RenewPopulationInputBox
            // 
            this.RenewPopulationInputBox.Location = new System.Drawing.Point(3, 3);
            this.RenewPopulationInputBox.Mask = "00";
            this.RenewPopulationInputBox.Name = "RenewPopulationInputBox";
            this.RenewPopulationInputBox.Size = new System.Drawing.Size(38, 20);
            this.RenewPopulationInputBox.TabIndex = 0;
            this.RenewPopulationInputBox.Text = "40";
            this.RenewPopulationInputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "%";
            // 
            // RenewPopulationButton
            // 
            this.RenewPopulationButton.Location = new System.Drawing.Point(3, 39);
            this.RenewPopulationButton.Name = "RenewPopulationButton";
            this.RenewPopulationButton.Size = new System.Drawing.Size(119, 23);
            this.RenewPopulationButton.TabIndex = 2;
            this.RenewPopulationButton.Text = "Renew Population";
            this.RenewPopulationButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RenewPopulationButton.UseVisualStyleBackColor = true;
            this.RenewPopulationButton.Click += new System.EventHandler(this.RenewPopulationButton_Click);
            // 
            // resultDisplay
            // 
            this.resultDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultDisplay.Location = new System.Drawing.Point(133, 37);
            this.resultDisplay.Name = "resultDisplay";
            this.resultDisplay.Size = new System.Drawing.Size(554, 473);
            this.resultDisplay.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 510);
            this.Controls.Add(this.resultDisplay);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.settingsPanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(672, 274);
            this.Name = "MainForm";
            this.Text = "GA";
            this.controlPanel.ResumeLayout(false);
            this.buttonPanel.ResumeLayout(false);
            this.settingsPanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel controlPanel;
        private Button RunButton;
        private Button PuaseButton;
        private Button NextButton;
        private DataGridViewTextBoxColumn Chromosome;
        private DataGridViewTextBoxColumn Evaluation;
        private Button RestartButton;
        private FlowLayoutPanel buttonPanel;
        private Panel settingsPanel;
        private DoubleInputBox MutationInputBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private DoubleInputBox ElitismInputBox;
        private Panel panel1;
        private Button RenewPopulationButton;
        private Label label2;
        private MaskedTextBox RenewPopulationInputBox;
        private FlowLayoutPanel flowLayoutPanel2;
        private UserControls.GeneticResultDisplay resultDisplay;
    }
}

