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
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Lable
            // 
            this.Lable.AutoSize = true;
            this.Lable.Location = new System.Drawing.Point(2, 0);
            this.Lable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Lable.Name = "Lable";
            this.Lable.Size = new System.Drawing.Size(33, 13);
            this.Lable.TabIndex = 0;
            this.Lable.Text = "Lable";
            // 
            // InputBox
            // 
            this.InputBox.Location = new System.Drawing.Point(39, 2);
            this.InputBox.Margin = new System.Windows.Forms.Padding(2);
            this.InputBox.Mask = "00";
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(34, 20);
            this.InputBox.TabIndex = 1;
            this.InputBox.Text = "00";
            this.InputBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.Lable);
            this.flowLayoutPanel1.Controls.Add(this.InputBox);
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(115, 45);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(78, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "%";
            // 
            // DoubleInputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "DoubleInputBox";
            this.Size = new System.Drawing.Size(115, 45);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Label Lable;
        private System.Windows.Forms.MaskedTextBox InputBox;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label1;
    }
}
