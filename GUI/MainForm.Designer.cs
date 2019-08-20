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
            this.Chromosome = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Evaluation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.populationSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.maxGenerationsInput = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.applyButton = new System.Windows.Forms.Button();
            this.searchRunner1 = new UserControls.SearchRunner();
            this.MutationInputBox = new GUI.DoubleInputBox();
            this.ElitismInputBox = new GUI.DoubleInputBox();
            this.settingsPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.populationSize)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxGenerationsInput)).BeginInit();
            this.SuspendLayout();
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
            this.settingsPanel.Size = new System.Drawing.Size(200, 785);
            this.settingsPanel.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.MutationInputBox);
            this.flowLayoutPanel1.Controls.Add(this.ElitismInputBox);
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Controls.Add(this.applyButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 785);
            this.flowLayoutPanel1.TabIndex = 1;
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.populationSize);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 105);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 54);
            this.panel1.TabIndex = 2;
            // 
            // populationSize
            // 
            this.populationSize.Location = new System.Drawing.Point(133, 12);
            this.populationSize.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.populationSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.populationSize.Name = "populationSize";
            this.populationSize.Size = new System.Drawing.Size(58, 26);
            this.populationSize.TabIndex = 1;
            this.populationSize.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Population Size";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.maxGenerationsInput);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(3, 165);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 54);
            this.panel2.TabIndex = 3;
            // 
            // maxGenerationsInput
            // 
            this.maxGenerationsInput.Location = new System.Drawing.Point(112, 12);
            this.maxGenerationsInput.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.maxGenerationsInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.maxGenerationsInput.Name = "maxGenerationsInput";
            this.maxGenerationsInput.Size = new System.Drawing.Size(79, 26);
            this.maxGenerationsInput.TabIndex = 1;
            this.maxGenerationsInput.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Generations";
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(3, 225);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 186);
            this.panel3.TabIndex = 5;
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(3, 417);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(118, 40);
            this.applyButton.TabIndex = 4;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // searchRunner1
            // 
            this.searchRunner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchRunner1.engineBuilder = null;
            this.searchRunner1.Location = new System.Drawing.Point(200, 0);
            this.searchRunner1.MinimumSize = new System.Drawing.Size(630, 600);
            this.searchRunner1.Name = "searchRunner1";
            this.searchRunner1.Size = new System.Drawing.Size(830, 785);
            this.searchRunner1.TabIndex = 3;
            // 
            // MutationInputBox
            // 
            this.MutationInputBox.LableText = "Mutation";
            this.MutationInputBox.Location = new System.Drawing.Point(2, 2);
            this.MutationInputBox.Margin = new System.Windows.Forms.Padding(2);
            this.MutationInputBox.Name = "MutationInputBox";
            this.MutationInputBox.Size = new System.Drawing.Size(198, 45);
            this.MutationInputBox.TabIndex = 0;
            // 
            // ElitismInputBox
            // 
            this.ElitismInputBox.LableText = "Elitism";
            this.ElitismInputBox.Location = new System.Drawing.Point(2, 51);
            this.ElitismInputBox.Margin = new System.Windows.Forms.Padding(2);
            this.ElitismInputBox.Name = "ElitismInputBox";
            this.ElitismInputBox.Size = new System.Drawing.Size(164, 49);
            this.ElitismInputBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1030, 785);
            this.Controls.Add(this.searchRunner1);
            this.Controls.Add(this.settingsPanel);
            this.MinimumSize = new System.Drawing.Size(997, 391);
            this.Name = "MainForm";
            this.Text = "GA";
            this.settingsPanel.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.populationSize)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxGenerationsInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DataGridViewTextBoxColumn Chromosome;
        private DataGridViewTextBoxColumn Evaluation;
        private Panel settingsPanel;
        private DoubleInputBox MutationInputBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private DoubleInputBox ElitismInputBox;
        private UserControls.SearchRunner searchRunner1;
        private Panel panel1;
        private NumericUpDown populationSize;
        private Label label1;
        private Panel panel2;
        private NumericUpDown maxGenerationsInput;
        private Label label2;
        private Button applyButton;
        private Panel panel3;
    }
}

