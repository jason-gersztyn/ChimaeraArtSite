using System;
using System.Windows.Forms;

namespace Chimaera.Body
{
    partial class Home
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
            this.comicButton = new System.Windows.Forms.Button();
            this.shopButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comicButton
            // 
            this.comicButton.Location = new System.Drawing.Point(12, 12);
            this.comicButton.Name = "comicButton";
            this.comicButton.Size = new System.Drawing.Size(75, 23);
            this.comicButton.TabIndex = 0;
            this.comicButton.Text = "Comic";
            this.comicButton.UseVisualStyleBackColor = true;
            this.comicButton.Click += comicButton_Click;
            // 
            // shopButton
            // 
            this.shopButton.Location = new System.Drawing.Point(117, 12);
            this.shopButton.Name = "shopButton";
            this.shopButton.Size = new System.Drawing.Size(75, 23);
            this.shopButton.TabIndex = 1;
            this.shopButton.Text = "Shop";
            this.shopButton.UseVisualStyleBackColor = true;
            this.shopButton.Click += shopButton_Click;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(206, 50);
            this.Controls.Add(this.shopButton);
            this.Controls.Add(this.comicButton);
            this.Name = "Home";
            this.Text = "Chimaera";
            this.Load += new System.EventHandler(this.Home_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button comicButton;
        private System.Windows.Forms.Button shopButton;
    }
}