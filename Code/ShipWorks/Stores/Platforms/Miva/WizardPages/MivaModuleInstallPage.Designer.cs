namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    partial class MivaModuleInstallPage
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
            this.labelDownload = new System.Windows.Forms.Label();
            this.labelDownloadInfo = new System.Windows.Forms.Label();
            this.linkDownloadPage = new ShipWorks.UI.Controls.LinkControl();
            this.linkInstructions = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelProceed = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelDownload
            // 
            this.labelDownload.AutoSize = true;
            this.labelDownload.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelDownload.Location = new System.Drawing.Point(19, 52);
            this.labelDownload.Name = "labelDownload";
            this.labelDownload.Size = new System.Drawing.Size(75, 13);
            this.labelDownload.TabIndex = 0;
            this.labelDownload.Text = "1. Download";
            // 
            // labelDownloadInfo
            // 
            this.labelDownloadInfo.AutoSize = true;
            this.labelDownloadInfo.Location = new System.Drawing.Point(33, 71);
            this.labelDownloadInfo.Name = "labelDownloadInfo";
            this.labelDownloadInfo.Size = new System.Drawing.Size(154, 13);
            this.labelDownloadInfo.TabIndex = 1;
            this.labelDownloadInfo.Text = "Download the module from the";
            // 
            // linkDownloadPage
            // 
            this.linkDownloadPage.AutoSize = true;
            this.linkDownloadPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkDownloadPage.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkDownloadPage.ForeColor = System.Drawing.Color.Blue;
            this.linkDownloadPage.Location = new System.Drawing.Point(184, 71);
            this.linkDownloadPage.Name = "linkDownloadPage";
            this.linkDownloadPage.Size = new System.Drawing.Size(137, 13);
            this.linkDownloadPage.TabIndex = 2;
            this.linkDownloadPage.Text = "ShipWorks download page.";
            this.linkDownloadPage.Click += new System.EventHandler(this.OnLinkDownloadPage);
            // 
            // linkInstructions
            // 
            this.linkInstructions.AutoSize = true;
            this.linkInstructions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkInstructions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkInstructions.ForeColor = System.Drawing.Color.Blue;
            this.linkInstructions.Location = new System.Drawing.Point(304, 117);
            this.linkInstructions.Name = "linkInstructions";
            this.linkInstructions.Size = new System.Drawing.Size(96, 13);
            this.linkInstructions.TabIndex = 5;
            this.linkInstructions.Text = "these instructions.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(276, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Install the module on your Miva Merchant website using ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label4.Location = new System.Drawing.Point(19, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "2. Install";
            // 
            // labelProceed
            // 
            this.labelProceed.AutoSize = true;
            this.labelProceed.Location = new System.Drawing.Point(20, 153);
            this.labelProceed.Name = "labelProceed";
            this.labelProceed.Size = new System.Drawing.Size(258, 13);
            this.labelProceed.TabIndex = 6;
            this.labelProceed.Text = "Proceed to the next step after installing the module.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(402, 27);
            this.label1.TabIndex = 7;
            this.label1.Text = "Before using ShipWorks with Miva Merchant you must download and install the ShipW" +
                "orks Miva Module.";
            // 
            // MivaModuleInstallPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelProceed);
            this.Controls.Add(this.linkInstructions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkDownloadPage);
            this.Controls.Add(this.labelDownloadInfo);
            this.Controls.Add(this.labelDownload);
            this.Description = "Enter the following information about your online store.";
            this.Name = "MivaModuleInstallPage";
            this.Size = new System.Drawing.Size(444, 207);
            this.Title = "Store Setup";
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingInto);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelDownload;
        private System.Windows.Forms.Label labelDownloadInfo;
        private ShipWorks.UI.Controls.LinkControl linkDownloadPage;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkInstructions;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelProceed;
        private System.Windows.Forms.Label label1;
    }
}
