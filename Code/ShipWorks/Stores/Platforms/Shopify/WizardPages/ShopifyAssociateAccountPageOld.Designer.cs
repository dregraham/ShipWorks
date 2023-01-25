namespace ShipWorks.Stores.Platforms.Shopify.WizardPages
{
    partial class ShopifyAssociateAccountPageOld
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
            this.label1 = new System.Windows.Forms.Label();
            this.createTokenControl = new ShipWorks.Stores.Platforms.Shopify.ShopifyCreateTokenControl();
            this.linkImportTokenFile = new ShipWorks.UI.Controls.LinkControl();
            this.labelImport = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(518, 30);
            this.label1.TabIndex = 20;
            this.label1.Text = "Shopify requires you to authorize ShipWorks to connect to your Shopify account.  " +
                "This is done by logging into a special Shopify page that creates an Shopify Logi" +
                "n Token for ShipWorks.";
            // 
            // createTokenControl
            // 
            this.createTokenControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.createTokenControl.Location = new System.Drawing.Point(25, 47);
            this.createTokenControl.Name = "createTokenControl";
            this.createTokenControl.Size = new System.Drawing.Size(551, 31);
            this.createTokenControl.TabIndex = 0;
            // 
            // linkImportTokenFile
            // 
            this.linkImportTokenFile.AutoSize = true;
            this.linkImportTokenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportTokenFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportTokenFile.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportTokenFile.Location = new System.Drawing.Point(404, 94);
            this.linkImportTokenFile.Name = "linkImportTokenFile";
            this.linkImportTokenFile.Size = new System.Drawing.Size(55, 13);
            this.linkImportTokenFile.TabIndex = 22;
            this.linkImportTokenFile.Text = "click here.";
            this.linkImportTokenFile.Click += new System.EventHandler(this.OnImportTokenFile);
            // 
            // labelImport
            // 
            this.labelImport.AutoSize = true;
            this.labelImport.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelImport.Location = new System.Drawing.Point(10, 94);
            this.labelImport.Name = "labelImport";
            this.labelImport.Size = new System.Drawing.Size(397, 13);
            this.labelImport.TabIndex = 21;
            this.labelImport.Text = "If you have previously created and saved a token and need to import a token file\r" +
                "\n";
            // 
            // ShopifyAssociateAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkImportTokenFile);
            this.Controls.Add(this.labelImport);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.createTokenControl);
            this.Name = "ShopifyAssociateAccountPage";
            this.Size = new System.Drawing.Size(579, 255);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccountPage);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAccountPage);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShopifyCreateTokenControl createTokenControl;
        private System.Windows.Forms.Label label1;
        private UI.Controls.LinkControl linkImportTokenFile;
        private System.Windows.Forms.Label labelImport;

    }
}
