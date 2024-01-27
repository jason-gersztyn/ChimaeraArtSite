namespace Chimaera.Body
{
    partial class SeriesEdit
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
            this.imageUrl = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.active = new System.Windows.Forms.CheckBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.imagePreview = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).BeginInit();
            this.SuspendLayout();
            // 
            // imageUrl
            // 
            this.imageUrl.Enabled = false;
            this.imageUrl.Location = new System.Drawing.Point(12, 230);
            this.imageUrl.Name = "imageUrl";
            this.imageUrl.Size = new System.Drawing.Size(169, 20);
            this.imageUrl.TabIndex = 0;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(12, 257);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(169, 20);
            this.name.TabIndex = 1;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(187, 230);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            // 
            // active
            // 
            this.active.AutoSize = true;
            this.active.Location = new System.Drawing.Point(101, 283);
            this.active.Name = "active";
            this.active.Size = new System.Drawing.Size(56, 17);
            this.active.TabIndex = 3;
            this.active.Text = "Active";
            this.active.UseVisualStyleBackColor = true;
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(101, 306);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 4;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            // 
            // imagePreview
            // 
            this.imagePreview.Location = new System.Drawing.Point(12, 13);
            this.imagePreview.Name = "imagePreview";
            this.imagePreview.Size = new System.Drawing.Size(250, 211);
            this.imagePreview.TabIndex = 5;
            this.imagePreview.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(187, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Image Files (*.PNG;*.JPG)|*.PNG;*.JPG";
            this.openFileDialog1.FileOk += OpenFileDialog1_FileOk;
            // 
            // SeriesEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(275, 341);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imagePreview);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.active);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.name);
            this.Controls.Add(this.imageUrl);
            this.Name = "SeriesEdit";
            this.Text = "Chimaera; Series";
            ((System.ComponentModel.ISupportInitialize)(this.imagePreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox imageUrl;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.CheckBox active;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.PictureBox imagePreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}