namespace ShipWorks.Stores.UI.Platforms.Odbc.Controls
{
    partial class OdbcOnDemandDownloadSettingsControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboAllowDownload = new ShipWorks.Stores.Management.ComputerDownloadAllowedComboBox();
            this.configureDownloadComputers = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // labelAllowDownload
            // 
            this.labelAllowDownload.AutoSize = true;
            this.labelAllowDownload.Location = new System.Drawing.Point(0, 3);
            this.labelAllowDownload.Name = "labelAllowDownload";
            this.labelAllowDownload.Size = new System.Drawing.Size(231, 13);
            this.labelAllowDownload.TabIndex = 7;
            this.labelAllowDownload.Text = "Allow this computer to download for this store:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(449, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "ShipWorks will only download orders from this ODBC store when searching by order " +
    "number.\r\n";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(403, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "You may download some or all of this store\'s orders into ShipWorks if you edit yo" +
    "ur";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(398, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Import Settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(495, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "in the";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(0, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Store Connection";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(78, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "tab to the left.";
            // 
            // comboAllowDownload
            // 
            this.comboAllowDownload.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAllowDownload.FormattingEnabled = true;
            this.comboAllowDownload.Location = new System.Drawing.Point(234, 0);
            this.comboAllowDownload.Name = "comboAllowDownload";
            this.comboAllowDownload.Size = new System.Drawing.Size(121, 21);
            this.comboAllowDownload.TabIndex = 9;
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
            this.configureDownloadComputers.TabIndex = 8;
            this.configureDownloadComputers.Text = "Configure other computers...";
            this.configureDownloadComputers.Click += new System.EventHandler(this.OnConfigureDownloadPolicy);
            // 
            // OdbcOnDemandDownloadSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelAllowDownload);
            this.Controls.Add(this.comboAllowDownload);
            this.Controls.Add(this.configureDownloadComputers);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OdbcOnDemandDownloadSettingsControl";
            this.Size = new System.Drawing.Size(558, 87);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAllowDownload;
        private Management.ComputerDownloadAllowedComboBox comboAllowDownload;
        private ShipWorks.UI.Controls.LinkControl configureDownloadComputers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
