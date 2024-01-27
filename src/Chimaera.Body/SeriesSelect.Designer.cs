using System.Windows.Forms;

namespace Chimaera.Body
{
    partial class SeriesSelect
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
            this.createSeriesButton = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.selectSeriesButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.createSeriesButton.Location = new System.Drawing.Point(297, 10);
            this.createSeriesButton.Name = "createSeriesButton";
            this.createSeriesButton.Size = new System.Drawing.Size(103, 23);
            this.createSeriesButton.TabIndex = 0;
            this.createSeriesButton.Text = "Create New Series";
            this.createSeriesButton.UseVisualStyleBackColor = true;
            this.createSeriesButton.Click += new System.EventHandler(this.createSeriesButton_Click);

            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(12, 12);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(209, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "OR";
            // 
            // button2
            // 
            this.selectSeriesButton.Location = new System.Drawing.Point(54, 39);
            this.selectSeriesButton.Name = "selectSeriesButton";
            this.selectSeriesButton.Size = new System.Drawing.Size(112, 23);
            this.selectSeriesButton.TabIndex = 4;
            this.selectSeriesButton.Text = "Select Series";
            this.selectSeriesButton.UseVisualStyleBackColor = true;
            this.selectSeriesButton.Click += new System.EventHandler(this.selectSeriesButton_Click);
            // 
            // SeriesSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 76);
            this.Controls.Add(this.selectSeriesButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.createSeriesButton);
            this.Name = "SeriesSelect";
            this.Text = "Chimaera; Series";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button createSeriesButton;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button selectSeriesButton;
    }
}