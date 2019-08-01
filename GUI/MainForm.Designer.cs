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
            this.infoPanel = new System.Windows.Forms.Panel();
            this.generationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PuaseButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.chromosomesDisplay = new System.Windows.Forms.DataGridView();
            this.Chromosome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Evaluation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.controlPanel.SuspendLayout();
            this.infoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chromosomesDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.Color.CadetBlue;
            this.controlPanel.Controls.Add(this.infoPanel);
            this.controlPanel.Controls.Add(this.PuaseButton);
            this.controlPanel.Controls.Add(this.NextButton);
            this.controlPanel.Controls.Add(this.RunButton);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(800, 57);
            this.controlPanel.TabIndex = 0;
            // 
            // infoPanel
            // 
            this.infoPanel.Controls.Add(this.generationLabel);
            this.infoPanel.Controls.Add(this.label1);
            this.infoPanel.Location = new System.Drawing.Point(504, 3);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Size = new System.Drawing.Size(256, 45);
            this.infoPanel.TabIndex = 5;
            // 
            // generationLabel
            // 
            this.generationLabel.AutoSize = true;
            this.generationLabel.Location = new System.Drawing.Point(102, 6);
            this.generationLabel.Name = "generationLabel";
            this.generationLabel.Size = new System.Drawing.Size(0, 20);
            this.generationLabel.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Generation:";
            // 
            // PuaseButton
            // 
            this.PuaseButton.Enabled = false;
            this.PuaseButton.Location = new System.Drawing.Point(166, 13);
            this.PuaseButton.Name = "PuaseButton";
            this.PuaseButton.Size = new System.Drawing.Size(75, 35);
            this.PuaseButton.TabIndex = 2;
            this.PuaseButton.Text = "Puase";
            this.PuaseButton.UseVisualStyleBackColor = true;
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(85, 13);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(75, 35);
            this.NextButton.TabIndex = 1;
            this.NextButton.Text = "Next";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Location = new System.Drawing.Point(4, 13);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(75, 35);
            this.RunButton.TabIndex = 0;
            this.RunButton.Text = "Run";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // chromosomesDisplay
            // 
            this.chromosomesDisplay.AllowUserToAddRows = false;
            this.chromosomesDisplay.AllowUserToDeleteRows = false;
            this.chromosomesDisplay.AllowUserToOrderColumns = true;
            this.chromosomesDisplay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chromosomesDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromosomesDisplay.Location = new System.Drawing.Point(0, 57);
            this.chromosomesDisplay.Name = "chromosomesDisplay";
            this.chromosomesDisplay.ReadOnly = true;
            this.chromosomesDisplay.RowTemplate.Height = 28;
            this.chromosomesDisplay.Size = new System.Drawing.Size(800, 393);
            this.chromosomesDisplay.TabIndex = 1;
            // 
            // Chromosome
            // 
            this.Chromosome.HeaderText = "Chromosome";
            this.Chromosome.Name = "Chromosome";
            this.Chromosome.ReadOnly = true;
            Chromosome.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            // 
            // Evaluation
            // 
            this.Evaluation.HeaderText = "Evaluation";
            this.Evaluation.Name = "Evaluation";
            this.Evaluation.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.chromosomesDisplay);
            this.Controls.Add(this.controlPanel);
            this.Name = "MainForm";
            this.Text = "GA";
            this.controlPanel.ResumeLayout(false);
            this.infoPanel.ResumeLayout(false);
            this.infoPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chromosomesDisplay)).EndInit();
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
    }
}

