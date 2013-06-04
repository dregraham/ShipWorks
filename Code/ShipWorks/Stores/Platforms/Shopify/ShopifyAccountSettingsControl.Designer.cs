namespace ShipWorks.Stores.Platforms.Shopify
{
    partial class ShopifyAccountSettingsControl
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
            this.createTokenControl = new ShipWorks.Stores.Platforms.Shopify.ShopifyCreateTokenControl();
            this.shopDisplayName = new System.Windows.Forms.TextBox();
            this.labelShop = new System.Windows.Forms.Label();
            this.linkImportToken = new ShipWorks.UI.Controls.LinkControl();
            this.labelAdvanced = new System.Windows.Forms.Label();
            this.linkExportToken = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // createTokenControl
            // 
            this.createTokenControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.createTokenControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.createTokenControl.Location = new System.Drawing.Point(86, 32);
            this.createTokenControl.Name = "createTokenControl";
            this.createTokenControl.ShowTokenInfo = false;
            this.createTokenControl.Size = new System.Drawing.Size(513, 30);
            this.createTokenControl.TabIndex = 1;
            this.createTokenControl.TokenCreated += new System.EventHandler(this.OnTokenCreated);
            // 
            // shopDisplayName
            // 
            this.shopDisplayName.Location = new System.Drawing.Point(91, 7);
            this.shopDisplayName.Name = "shopDisplayName";
            this.shopDisplayName.ReadOnly = true;
            this.shopDisplayName.Size = new System.Drawing.Size(258, 21);
            this.shopDisplayName.TabIndex = 15;
            // 
            // labelShop
            // 
            this.labelShop.AutoSize = true;
            this.labelShop.Location = new System.Drawing.Point(14, 10);
            this.labelShop.Name = "labelShop";
            this.labelShop.Size = new System.Drawing.Size(74, 13);
            this.labelShop.TabIndex = 16;
            this.labelShop.Text = "Shopify Shop:";
            // 
            // linkImportToken
            // 
            this.linkImportToken.AutoSize = true;
            this.linkImportToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportToken.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportToken.Location = new System.Drawing.Point(180, 102);
            this.linkImportToken.Name = "linkImportToken";
            this.linkImportToken.Size = new System.Drawing.Size(86, 13);
            this.linkImportToken.TabIndex = 24;
            this.linkImportToken.Text = "Import token file";
            this.linkImportToken.Click += new System.EventHandler(this.OnImportToken);
            // 
            // labelAdvanced
            // 
            this.labelAdvanced.AutoSize = true;
            this.labelAdvanced.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelAdvanced.Location = new System.Drawing.Point(88, 85);
            this.labelAdvanced.Name = "labelAdvanced";
            this.labelAdvanced.Size = new System.Drawing.Size(59, 13);
            this.labelAdvanced.TabIndex = 23;
            this.labelAdvanced.Text = "Advanced:";
            // 
            // linkExportToken
            // 
            this.linkExportToken.AutoSize = true;
            this.linkExportToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkExportToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkExportToken.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkExportToken.Location = new System.Drawing.Point(88, 102);
            this.linkExportToken.Name = "linkExportToken";
            this.linkExportToken.Size = new System.Drawing.Size(86, 13);
            this.linkExportToken.TabIndex = 22;
            this.linkExportToken.Text = "Export token file";
            this.linkExportToken.Click += new System.EventHandler(this.OnExportToken);
            // 
            // ShopifyAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkImportToken);
            this.Controls.Add(this.labelAdvanced);
            this.Controls.Add(this.linkExportToken);
            this.Controls.Add(this.shopDisplayName);
            this.Controls.Add(this.labelShop);
            this.Controls.Add(this.createTokenControl);
            this.Name = "ShopifyAccountSettingsControl";
            this.Size = new System.Drawing.Size(602, 190);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShopifyCreateTokenControl createTokenControl;
        private System.Windows.Forms.TextBox shopDisplayName;
        private System.Windows.Forms.Label labelShop;
        private UI.Controls.LinkControl linkImportToken;
        private System.Windows.Forms.Label labelAdvanced;
        private UI.Controls.LinkControl linkExportToken;
    }
}
