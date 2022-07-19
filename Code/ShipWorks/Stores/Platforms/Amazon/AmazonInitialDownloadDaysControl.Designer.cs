namespace ShipWorks.Stores.Platforms.Amazon
{
    partial class AmazonInitialDownloadDaysControl
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
            this.pictureBoxDownload = new System.Windows.Forms.PictureBox();
            this.labelDownload = new System.Windows.Forms.Label();
            this.textBoxInitialDays = new System.Windows.Forms.TextBox();
            this.labelInitialDays = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownload)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxDownload
            // 
            this.pictureBoxDownload.Image = global::ShipWorks.Properties.Resources.nav_down_green1;
            this.pictureBoxDownload.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxDownload.Name = "pictureBoxDownload";
            this.pictureBoxDownload.Size = new System.Drawing.Size(24, 24);
            this.pictureBoxDownload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxDownload.TabIndex = 1;
            this.pictureBoxDownload.TabStop = false;
            // 
            // labelDownload
            // 
            this.labelDownload.AutoSize = true;
            this.labelDownload.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDownload.Location = new System.Drawing.Point(33, 11);
            this.labelDownload.Name = "labelDownload";
            this.labelDownload.Size = new System.Drawing.Size(226, 13);
            this.labelDownload.TabIndex = 2;
            this.labelDownload.Text = "ShipWorks will download orders starting from:";
            // 
            // textBoxInitialDays
            // 
            this.textBoxInitialDays.Location = new System.Drawing.Point(49, 30);
            this.textBoxInitialDays.Name = "textBoxInitialDays";
            this.textBoxInitialDays.Size = new System.Drawing.Size(55, 21);
            this.textBoxInitialDays.TabIndex = 3;
            // 
            // labelInitialDays
            // 
            this.labelInitialDays.AutoSize = true;
            this.labelInitialDays.Location = new System.Drawing.Point(110, 33);
            this.labelInitialDays.Name = "labelInitialDays";
            this.labelInitialDays.Size = new System.Drawing.Size(55, 13);
            this.labelInitialDays.TabIndex = 4;
            this.labelInitialDays.Text = "days ago.";
            // 
            // AmazonInitialDownloadDaysControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxDownload);
            this.Controls.Add(this.labelDownload);
            this.Controls.Add(this.textBoxInitialDays);
            this.Controls.Add(this.labelInitialDays);
            this.Name = "AmazonInitialDownloadDaysControl";
            this.Size = new System.Drawing.Size(266, 60);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownload)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDownload;
        private System.Windows.Forms.Label labelDownload;
        private System.Windows.Forms.TextBox textBoxInitialDays;
        private System.Windows.Forms.Label labelInitialDays;
    }
}
