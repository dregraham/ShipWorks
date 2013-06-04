namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    partial class ProStoresProbeSettingsPage
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
            this.labelEnterUrl = new System.Windows.Forms.Label();
            this.labelUrl = new System.Windows.Forms.Label();
            this.storeUrl = new System.Windows.Forms.TextBox();
            this.labelImportToken1 = new System.Windows.Forms.Label();
            this.linkImportToken1 = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // labelEnterUrl
            // 
            this.labelEnterUrl.Location = new System.Drawing.Point(22, 14);
            this.labelEnterUrl.Name = "labelEnterUrl";
            this.labelEnterUrl.Size = new System.Drawing.Size(484, 32);
            this.labelEnterUrl.TabIndex = 1;
            this.labelEnterUrl.Text = "Enter the URL of your ProStores store home page.  ShipWorks will use this to dete" +
                "rmine the proper settings for connecting to your ProStores store.";
            // 
            // labelUrl
            // 
            this.labelUrl.AutoSize = true;
            this.labelUrl.Location = new System.Drawing.Point(36, 55);
            this.labelUrl.Name = "labelUrl";
            this.labelUrl.Size = new System.Drawing.Size(59, 13);
            this.labelUrl.TabIndex = 2;
            this.labelUrl.Text = "Store URL:";
            // 
            // storeUrl
            // 
            this.storeUrl.Location = new System.Drawing.Point(101, 52);
            this.storeUrl.Name = "storeUrl";
            this.storeUrl.Size = new System.Drawing.Size(392, 21);
            this.storeUrl.TabIndex = 3;
            // 
            // labelImportToken1
            // 
            this.labelImportToken1.AutoSize = true;
            this.labelImportToken1.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelImportToken1.Location = new System.Drawing.Point(22, 97);
            this.labelImportToken1.Name = "labelImportToken1";
            this.labelImportToken1.Size = new System.Drawing.Size(397, 13);
            this.labelImportToken1.TabIndex = 10;
            this.labelImportToken1.Text = "If you have previously created and saved a token and need to import a token file\r" +
                "\n";
            // 
            // linkImportToken1
            // 
            this.linkImportToken1.AutoSize = true;
            this.linkImportToken1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportToken1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportToken1.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportToken1.Location = new System.Drawing.Point(416, 97);
            this.linkImportToken1.Name = "linkImportToken1";
            this.linkImportToken1.Size = new System.Drawing.Size(55, 13);
            this.linkImportToken1.TabIndex = 11;
            this.linkImportToken1.Text = "click here.";
            this.linkImportToken1.Click += new System.EventHandler(this.OnImportTokenFile);
            // 
            // ProStoresProbeSettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelImportToken1);
            this.Controls.Add(this.linkImportToken1);
            this.Controls.Add(this.storeUrl);
            this.Controls.Add(this.labelUrl);
            this.Controls.Add(this.labelEnterUrl);
            this.Name = "ProStoresProbeSettingsPage";
            this.Size = new System.Drawing.Size(509, 267);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEnterUrl;
        private System.Windows.Forms.Label labelUrl;
        private System.Windows.Forms.TextBox storeUrl;
        private System.Windows.Forms.Label labelImportToken1;
        private ShipWorks.UI.Controls.LinkControl linkImportToken1;
    }
}
