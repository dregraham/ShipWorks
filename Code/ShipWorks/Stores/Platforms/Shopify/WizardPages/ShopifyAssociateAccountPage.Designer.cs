namespace ShipWorks.Stores.Platforms.Shopify.WizardPages
{
    partial class ShopifyAssociateAccountPage
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
            this.shopifyManageToken = new ShipWorks.Stores.Platforms.Etsy.EtsyTokenManageControl();
            this.label7 = new System.Windows.Forms.Label();
            this.importTokenFile = new ShipWorks.UI.Controls.LinkControl();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // shopifyManageToken
            // 
            this.shopifyManageToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shopifyManageToken.Location = new System.Drawing.Point(47, 50);
            this.shopifyManageToken.Name = "shopifyManageToken";
            this.shopifyManageToken.Size = new System.Drawing.Size(469, 81);
            this.shopifyManageToken.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label7.Location = new System.Drawing.Point(24, 134);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(397, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "If you have previously created and saved a token and need to import a token file\r" +
    "\n";
            // 
            // importTokenFile
            // 
            this.importTokenFile.AutoSize = true;
            this.importTokenFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.importTokenFile.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.importTokenFile.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.importTokenFile.Location = new System.Drawing.Point(424, 134);
            this.importTokenFile.Name = "importTokenFile";
            this.importTokenFile.Size = new System.Drawing.Size(55, 13);
            this.importTokenFile.TabIndex = 13;
            this.importTokenFile.Text = "click here.";
            this.importTokenFile.Click += new System.EventHandler(this.OnImportTokenFileClick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 14);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(518, 33);
            this.label1.TabIndex = 14;
            this.label1.Text = "Shopify requires you to authorize ShipWorks to connect to your Etsy account.  Thi" +
    "s is done by logging into a special Etsy page that creates an Etsy Login Token f" +
    "or ShipWorks.";
            // 
            // ShopifyAssociateAccountPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.importTokenFile);
            this.Controls.Add(this.shopifyManageToken);
            this.Name = "ShopifyAssociateAccountPage";
            this.Size = new System.Drawing.Size(566, 238);
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextShopifyAssociateAccountPage);
            this.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAccountPage);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Etsy.EtsyTokenManageControl shopifyManageToken;
        private System.Windows.Forms.Label label7;
        private UI.Controls.LinkControl importTokenFile;
        private System.Windows.Forms.Label label1;

    }
}
