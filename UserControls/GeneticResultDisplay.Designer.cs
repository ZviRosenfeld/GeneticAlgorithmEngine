using System.Windows.Forms;

namespace UserControls
{
    partial class GeneticResultDisplay
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.controlPanel = new System.Windows.Forms.Panel();
            this.searchTimeLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.generationLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.environmentPanel = new System.Windows.Forms.Panel();
            this.environmentLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chromosomesView = new System.Windows.Forms.DataGridView();
            this.controlPanel.SuspendLayout();
            this.environmentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chromosomesView)).BeginInit();
            this.SuspendLayout();
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.searchTimeLabel);
            this.controlPanel.Controls.Add(this.label5);
            this.controlPanel.Controls.Add(this.generationLabel);
            this.controlPanel.Controls.Add(this.label1);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(1200, 58);
            this.controlPanel.TabIndex = 0;
            // 
            // searchTimeLabel
            // 
            this.searchTimeLabel.AutoSize = true;
            this.searchTimeLabel.Location = new System.Drawing.Point(497, 6);
            this.searchTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.searchTimeLabel.Name = "searchTimeLabel";
            this.searchTimeLabel.Size = new System.Drawing.Size(18, 20);
            this.searchTimeLabel.TabIndex = 3;
            this.searchTimeLabel.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(387, 6);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Search Time:";
            // 
            // generationLabel
            // 
            this.generationLabel.AutoSize = true;
            this.generationLabel.Location = new System.Drawing.Point(108, 6);
            this.generationLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.generationLabel.Name = "generationLabel";
            this.generationLabel.Size = new System.Drawing.Size(18, 20);
            this.generationLabel.TabIndex = 1;
            this.generationLabel.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Generation:";
            // 
            // environmentPanel
            // 
            this.environmentPanel.Controls.Add(this.environmentLabel);
            this.environmentPanel.Controls.Add(this.label2);
            this.environmentPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.environmentPanel.Location = new System.Drawing.Point(0, 58);
            this.environmentPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.environmentPanel.Name = "environmentPanel";
            this.environmentPanel.Size = new System.Drawing.Size(1200, 58);
            this.environmentPanel.TabIndex = 1;
            // 
            // environmentLabel
            // 
            this.environmentLabel.AutoSize = true;
            this.environmentLabel.Location = new System.Drawing.Point(118, 11);
            this.environmentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.environmentLabel.Name = "environmentLabel";
            this.environmentLabel.Size = new System.Drawing.Size(33, 20);
            this.environmentLabel.TabIndex = 1;
            this.environmentLabel.Text = "null";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Environment:";
            // 
            // chromosomesView
            // 
            this.chromosomesView.AllowUserToAddRows = false;
            this.chromosomesView.AllowUserToDeleteRows = false;
            this.chromosomesView.AllowUserToOrderColumns = true;
            this.chromosomesView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chromosomesView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chromosomesView.Location = new System.Drawing.Point(0, 116);
            this.chromosomesView.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chromosomesView.Name = "chromosomesView";
            this.chromosomesView.ReadOnly = true;
            this.chromosomesView.Size = new System.Drawing.Size(1200, 576);
            this.chromosomesView.TabIndex = 2;
            // 
            // GeneticResultDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chromosomesView);
            this.Controls.Add(this.environmentPanel);
            this.Controls.Add(this.controlPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GeneticResultDisplay";
            this.Size = new System.Drawing.Size(1200, 692);
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.environmentPanel.ResumeLayout(false);
            this.environmentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chromosomesView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel controlPanel;
        private Panel environmentPanel;
        private DataGridView chromosomesView;
        private Label searchTimeLabel;
        private Label label5;
        private Label generationLabel;
        private Label label1;
        private Label environmentLabel;
        private Label label2;
    }
}
