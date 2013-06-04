namespace ShipWorks.Stores.Management
{
    partial class AutomaticDownloadControl
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
            this.autoDownloadWhenAway = new System.Windows.Forms.CheckBox();
            this.labelAutoDownloadMinutes = new System.Windows.Forms.Label();
            this.autoDownloadMinutes = new System.Windows.Forms.NumericUpDown();
            this.autoDownload = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize) (this.autoDownloadMinutes)).BeginInit();
            this.SuspendLayout();
            // 
            // autoDownloadWhenAway
            // 
            this.autoDownloadWhenAway.AutoSize = true;
            this.autoDownloadWhenAway.Location = new System.Drawing.Point(23, 30);
            this.autoDownloadWhenAway.Name = "autoDownloadWhenAway";
            this.autoDownloadWhenAway.Size = new System.Drawing.Size(212, 17);
            this.autoDownloadWhenAway.TabIndex = 3;
            this.autoDownloadWhenAway.Text = "Only when I am away from ShipWorks.";
            this.autoDownloadWhenAway.UseVisualStyleBackColor = true;
            // 
            // labelAutoDownloadMinutes
            // 
            this.labelAutoDownloadMinutes.AutoSize = true;
            this.labelAutoDownloadMinutes.Location = new System.Drawing.Point(230, 4);
            this.labelAutoDownloadMinutes.Name = "labelAutoDownloadMinutes";
            this.labelAutoDownloadMinutes.Size = new System.Drawing.Size(48, 13);
            this.labelAutoDownloadMinutes.TabIndex = 2;
            this.labelAutoDownloadMinutes.Text = "minutes.";
            // 
            // autoDownloadMinutes
            // 
            this.autoDownloadMinutes.Location = new System.Drawing.Point(171, 2);
            this.autoDownloadMinutes.Maximum = new decimal(new int[] {
            480,
            0,
            0,
            0});
            this.autoDownloadMinutes.Name = "autoDownloadMinutes";
            this.autoDownloadMinutes.Size = new System.Drawing.Size(55, 21);
            this.autoDownloadMinutes.TabIndex = 1;
            // 
            // autoDownload
            // 
            this.autoDownload.AutoSize = true;
            this.autoDownload.Location = new System.Drawing.Point(3, 3);
            this.autoDownload.Name = "autoDownload";
            this.autoDownload.Size = new System.Drawing.Size(173, 17);
            this.autoDownload.TabIndex = 0;
            this.autoDownload.Text = "Automatically download every ";
            this.autoDownload.UseVisualStyleBackColor = true;
            this.autoDownload.CheckedChanged += new System.EventHandler(this.OnChangeAutoDownload);
            // 
            // AutomaticDownloadControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.autoDownloadWhenAway);
            this.Controls.Add(this.labelAutoDownloadMinutes);
            this.Controls.Add(this.autoDownloadMinutes);
            this.Controls.Add(this.autoDownload);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "AutomaticDownloadControl";
            this.Size = new System.Drawing.Size(279, 50);
            ((System.ComponentModel.ISupportInitialize) (this.autoDownloadMinutes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoDownloadWhenAway;
        private System.Windows.Forms.Label labelAutoDownloadMinutes;
        private System.Windows.Forms.NumericUpDown autoDownloadMinutes;
        private System.Windows.Forms.CheckBox autoDownload;
    }
}
