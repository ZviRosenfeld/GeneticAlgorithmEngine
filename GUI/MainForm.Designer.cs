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
            this.MutationInputBox = new GUI.DoubleInputBox();
            this.ElitismInputBox = new GUI.DoubleInputBox();
            this.searchRunner1 = new UserControls.SearchRunner();
            this.settingsPanel.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 785);
            this.flowLayoutPanel1.TabIndex = 1;
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
            // searchRunner1
            // 
            this.searchRunner1.engineBuilder = null;
            this.searchRunner1.Location = new System.Drawing.Point(232, 22);
            this.searchRunner1.MinimumSize = new System.Drawing.Size(630, 600);
            this.searchRunner1.Name = "searchRunner1";
            this.searchRunner1.Size = new System.Drawing.Size(650, 650);
            this.searchRunner1.TabIndex = 3;
            searchRunner1.Dock = DockStyle.Fill;
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
    }
}

