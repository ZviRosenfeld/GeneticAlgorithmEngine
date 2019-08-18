using System.Drawing;
using System.Windows.Forms;

namespace UserControls
{
    partial class SearchRunner
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
            this.buttonPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.runButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.puaseButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.populationRenewalPanel = new System.Windows.Forms.Panel();
            this.renewPopulationButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.renewPopulationInputBox = new System.Windows.Forms.MaskedTextBox();
            this.geneticResultDisplay = new UserControls.GeneticResultDisplay();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.buttonPanel.SuspendLayout();
            this.populationRenewalPanel.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.runButton);
            this.buttonPanel.Controls.Add(this.nextButton);
            this.buttonPanel.Controls.Add(this.puaseButton);
            this.buttonPanel.Controls.Add(this.restartButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonPanel.Location = new System.Drawing.Point(0, 0);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(450, 50);
            this.buttonPanel.TabIndex = 0;
            // 
            // runButton
            // 
            this.runButton.Location = new System.Drawing.Point(3, 3);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(88, 31);
            this.runButton.TabIndex = 0;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(97, 3);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(88, 31);
            this.nextButton.TabIndex = 1;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // puaseButton
            // 
            this.puaseButton.Location = new System.Drawing.Point(191, 3);
            this.puaseButton.Name = "puaseButton";
            this.puaseButton.Size = new System.Drawing.Size(88, 31);
            this.puaseButton.TabIndex = 2;
            this.puaseButton.Text = "Puase";
            this.puaseButton.UseVisualStyleBackColor = true;
            this.puaseButton.Click += new System.EventHandler(this.puaseButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(285, 3);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(88, 31);
            this.restartButton.TabIndex = 3;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // populationRenewalPanel
            // 
            this.populationRenewalPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.populationRenewalPanel.Controls.Add(this.renewPopulationButton);
            this.populationRenewalPanel.Controls.Add(this.label1);
            this.populationRenewalPanel.Controls.Add(this.renewPopulationInputBox);
            this.populationRenewalPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.populationRenewalPanel.Location = new System.Drawing.Point(0, 0);
            this.populationRenewalPanel.Name = "populationRenewalPanel";
            this.populationRenewalPanel.Size = new System.Drawing.Size(250, 50);
            this.populationRenewalPanel.TabIndex = 4;
            // 
            // renewPopulationButton
            // 
            this.renewPopulationButton.Location = new System.Drawing.Point(92, 7);
            this.renewPopulationButton.Name = "renewPopulationButton";
            this.renewPopulationButton.Size = new System.Drawing.Size(142, 34);
            this.renewPopulationButton.TabIndex = 2;
            this.renewPopulationButton.Text = "renewPopulationButton";
            this.renewPopulationButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.renewPopulationButton.UseVisualStyleBackColor = true;
            this.renewPopulationButton.Click += new System.EventHandler(this.renewPopulationButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "%";
            // 
            // renewPopulationInputBox
            // 
            this.renewPopulationInputBox.Location = new System.Drawing.Point(17, 11);
            this.renewPopulationInputBox.Mask = "00";
            this.renewPopulationInputBox.Name = "renewPopulationInputBox";
            this.renewPopulationInputBox.Size = new System.Drawing.Size(40, 26);
            this.renewPopulationInputBox.TabIndex = 0;
            this.renewPopulationInputBox.Text = "40";
            // 
            // geneticResultDisplay
            // 
            this.geneticResultDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geneticResultDisplay.Location = new System.Drawing.Point(0, 0);
            this.geneticResultDisplay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.geneticResultDisplay.Name = "geneticResultDisplay";
            this.geneticResultDisplay.Size = new System.Drawing.Size(650, 650);
            this.geneticResultDisplay.TabIndex = 1;
            // 
            // controlPanel
            // 
            this.controlPanel.Controls.Add(this.populationRenewalPanel);
            this.controlPanel.Controls.Add(this.buttonPanel);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(650, 50);
            this.controlPanel.TabIndex = 2;
            // 
            // SearchRunner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.geneticResultDisplay);
            this.MinimumSize = new System.Drawing.Size(630, 600);
            this.Name = "SearchRunner";
            this.Size = new System.Drawing.Size(650, 650);
            this.buttonPanel.ResumeLayout(false);
            this.populationRenewalPanel.ResumeLayout(false);
            this.populationRenewalPanel.PerformLayout();
            this.controlPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel buttonPanel;
        private GeneticResultDisplay geneticResultDisplay;
        private Button runButton;
        private Button nextButton;
        private Button puaseButton;
        private Button restartButton;
        private Panel populationRenewalPanel;
        private Button renewPopulationButton;
        private Label label1;
        private MaskedTextBox renewPopulationInputBox;
        private Panel controlPanel;
    }
}
