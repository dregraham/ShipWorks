namespace ShipWorks.Stores.Platforms.Etsy
{
    partial class EtsyAccountSettingsControl
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
            this.tokenControl = new ShipWorks.Stores.Platforms.Etsy.EtsyTokenManageControl();
            this.labelToken = new System.Windows.Forms.Label();
            this.tokenInfoBox = new System.Windows.Forms.TextBox();
            this.linkImportToken = new ShipWorks.UI.Controls.LinkControl();
            this.labelAdvanced = new System.Windows.Forms.Label();
            this.linkExportToken = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // tokenControl
            // 
            this.tokenControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tokenControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.tokenControl.Location = new System.Drawing.Point(98, 30);
            this.tokenControl.Name = "tokenControl";
            this.tokenControl.ShowTokenInfo = false;
            this.tokenControl.Size = new System.Drawing.Size(535, 33);
            this.tokenControl.TabIndex = 0;
            this.tokenControl.TokenImported += new System.EventHandler(this.OnTokenImported);
            // 
            // labelToken
            // 
            this.labelToken.AutoSize = true;
            this.labelToken.Location = new System.Drawing.Point(4, 7);
            this.labelToken.Name = "labelToken";
            this.labelToken.Size = new System.Drawing.Size(92, 13);
            this.labelToken.TabIndex = 1;
            this.labelToken.Text = "Etsy Login Token:";
            // 
            // tokenInfoBox
            // 
            this.tokenInfoBox.Location = new System.Drawing.Point(102, 4);
            this.tokenInfoBox.Name = "tokenInfoBox";
            this.tokenInfoBox.ReadOnly = true;
            this.tokenInfoBox.Size = new System.Drawing.Size(389, 21);
            this.tokenInfoBox.TabIndex = 2;
            // 
            // linkImportToken
            // 
            this.linkImportToken.AutoSize = true;
            this.linkImportToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportToken.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportToken.Location = new System.Drawing.Point(191, 92);
            this.linkImportToken.Name = "linkImportToken";
            this.linkImportToken.Size = new System.Drawing.Size(86, 13);
            this.linkImportToken.TabIndex = 18;
            this.linkImportToken.Text = "Import token file";
            this.linkImportToken.Click += new System.EventHandler(this.OnLinkImportTokenClicked);
            // 
            // labelAdvanced
            // 
            this.labelAdvanced.AutoSize = true;
            this.labelAdvanced.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelAdvanced.Location = new System.Drawing.Point(99, 75);
            this.labelAdvanced.Name = "labelAdvanced";
            this.labelAdvanced.Size = new System.Drawing.Size(59, 13);
            this.labelAdvanced.TabIndex = 17;
            this.labelAdvanced.Text = "Advanced:";
            // 
            // linkExportToken
            // 
            this.linkExportToken.AutoSize = true;
            this.linkExportToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkExportToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkExportToken.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkExportToken.Location = new System.Drawing.Point(99, 92);
            this.linkExportToken.Name = "linkExportToken";
            this.linkExportToken.Size = new System.Drawing.Size(86, 13);
            this.linkExportToken.TabIndex = 16;
            this.linkExportToken.Text = "Export token file";
            this.linkExportToken.Click += new System.EventHandler(this.OnLinkExportTokenClicked);
            // 
            // EtsyAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkImportToken);
            this.Controls.Add(this.labelAdvanced);
            this.Controls.Add(this.linkExportToken);
            this.Controls.Add(this.tokenInfoBox);
            this.Controls.Add(this.labelToken);
            this.Controls.Add(this.tokenControl);
            this.Name = "EtsyAccountSettingsControl";
            this.Size = new System.Drawing.Size(636, 119);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private EtsyTokenManageControl tokenControl;
        private System.Windows.Forms.Label labelToken;
        private System.Windows.Forms.TextBox tokenInfoBox;
        private UI.Controls.LinkControl linkImportToken;
        private System.Windows.Forms.Label labelAdvanced;
        private UI.Controls.LinkControl linkExportToken;
    }
}
