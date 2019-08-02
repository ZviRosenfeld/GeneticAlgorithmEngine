using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    partial class DoubleInputBox
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
            this.Lable = new System.Windows.Forms.Label();
            this.InputBox = new System.Windows.Forms.MaskedTextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lable
            // 
            this.Lable.AutoSize = true;
            this.Lable.Location = new System.Drawing.Point(3, 0);
            this.Lable.Name = "Lable";
            this.Lable.Size = new System.Drawing.Size(48, 20);
            this.Lable.TabIndex = 0;
            this.Lable.Text = "Lable";
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(57, 3);
            this.InputBox.Mask = "\\0.00";
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(49, 26);
            this.InputBox.TabIndex = 1;
            this.InputBox.Text = "0.00";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.Lable);
            this.flowLayoutPanel1.Controls.Add(this.InputBox);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(283, 70);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // DoubleInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "DoubleInputBox";
            this.Size = new System.Drawing.Size(283, 70);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Label Lable;
        private System.Windows.Forms.MaskedTextBox InputBox;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}
