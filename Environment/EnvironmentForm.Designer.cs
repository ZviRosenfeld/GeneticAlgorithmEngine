using System.Windows.Forms;

namespace Environment
{
    partial class EnvironmentForm
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
            this.searchRunner1 = new UserControls.SearchRunner();
            this.SuspendLayout();
            // 
            // searchRunner1
            // 
            this.searchRunner1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchRunner1.engineBuilder = null;
            this.searchRunner1.Location = new System.Drawing.Point(0, 0);
            this.searchRunner1.MinimumSize = new System.Drawing.Size(630, 600);
            this.searchRunner1.Name = "searchRunner1";
            this.searchRunner1.Size = new System.Drawing.Size(800, 600);
            this.searchRunner1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.searchRunner1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.SearchRunner searchRunner1;
    }
}

