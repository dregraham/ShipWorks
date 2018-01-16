namespace ShipWorks.Stores.Management
{
    partial class DownloadSettingsControl
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
            this.labelAllowDownload = new System.Windows.Forms.Label();
            this.comboAllowDownload = new ShipWorks.Stores.Management.ComputerDownloadAllowedComboBox();
            this.configureDownloadComputers = new ShipWorks.UI.Controls.LinkControl();
            this.automaticDownloadControl = new ShipWorks.Stores.Management.AutomaticDownloadControl();
            this.SuspendLayout();
            // 
            // labelAllowDownload
            // 
            this.labelAllowDownload.AutoSize = true;
            this.labelAllowDownload.Location = new System.Drawing.Point(0, 3);
            this.labelAllowDownload.Name = "labelAllowDownload";
            this.labelAllowDownload.Size = new System.Drawing.Size(222, 13);
            this.labelAllowDownload.TabIndex = 3;
            this.labelAllowDownload.Text = "Allow this computer to download for this store:";
            // 
            // comboAllowDownload
            // 
            this.comboAllowDownload.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAllowDownload.FormattingEnabled = true;
            this.comboAllowDownload.Location = new System.Drawing.Point(234, 0);
            this.comboAllowDownload.Name = "comboAllowDownload";
            this.comboAllowDownload.Size = new System.Drawing.Size(121, 21);
            this.comboAllowDownload.TabIndex = 5;
            // 
            // configureDownloadComputers
            // 
            this.configureDownloadComputers.AutoSize = true;
            this.configureDownloadComputers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.configureDownloadComputers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.configureDownloadComputers.ForeColor = System.Drawing.Color.Blue;
            this.configureDownloadComputers.Location = new System.Drawing.Point(362, 3);
            this.configureDownloadComputers.Name = "configureDownloadComputers";
            this.configureDownloadComputers.Size = new System.Drawing.Size(148, 13);
            this.configureDownloadComputers.TabIndex = 4;
            this.configureDownloadComputers.Text = "Configure other computers...";
            // 
            // automaticDownloadControl
            // 
            this.automaticDownloadControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.automaticDownloadControl.Location = new System.Drawing.Point(0, 27);
            this.automaticDownloadControl.Name = "automaticDownloadControl";
            this.automaticDownloadControl.Size = new System.Drawing.Size(292, 51);
            this.automaticDownloadControl.TabIndex = 6;
            // 
            // DownloadSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelAllowDownload);
            this.Controls.Add(this.comboAllowDownload);
            this.Controls.Add(this.configureDownloadComputers);
            this.Controls.Add(this.automaticDownloadControl);
            this.Name = "DownloadSettingsControl";
            this.Size = new System.Drawing.Size(544, 73);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAllowDownload;
        private ComputerDownloadAllowedComboBox comboAllowDownload;
        private UI.Controls.LinkControl configureDownloadComputers;
        private AutomaticDownloadControl automaticDownloadControl;
    }
}
