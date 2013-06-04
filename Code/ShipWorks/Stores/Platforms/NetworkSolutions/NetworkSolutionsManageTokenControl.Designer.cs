namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    partial class NetworkSolutionsManageTokenControl
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
            this.tokenTextBox = new System.Windows.Forms.TextBox();
            this.createTokenControl = new ShipWorks.Stores.Platforms.NetworkSolutions.NetworkSolutionsCreateTokenControl();
            this.linkImportToken = new ShipWorks.UI.Controls.LinkControl();
            this.labelAdvanced = new System.Windows.Forms.Label();
            this.linkExportToken = new ShipWorks.UI.Controls.LinkControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(130, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Network Solutions Token:";
            // 
            // tokenTextBox
            // 
            this.tokenTextBox.Location = new System.Drawing.Point(139, 10);
            this.tokenTextBox.Name = "tokenTextBox";
            this.tokenTextBox.ReadOnly = true;
            this.tokenTextBox.Size = new System.Drawing.Size(350, 20);
            this.tokenTextBox.TabIndex = 1;
            // 
            // createTokenControl
            // 
            this.createTokenControl.Location = new System.Drawing.Point(135, 30);
            this.createTokenControl.Name = "createTokenControl";
            this.createTokenControl.Size = new System.Drawing.Size(410, 28);
            this.createTokenControl.SuccessText = "Your Network Solutions Token has been imported!";
            this.createTokenControl.TabIndex = 2;
            this.createTokenControl.TokenButtonText = "Update Login Token...";
            this.createTokenControl.TokenImported += new System.EventHandler(this.OnUpdateTokenCompleted);
            // 
            // linkImportToken
            // 
            this.linkImportToken.AutoSize = true;
            this.linkImportToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportToken.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportToken.Location = new System.Drawing.Point(223, 84);
            this.linkImportToken.Name = "linkImportToken";
            this.linkImportToken.Size = new System.Drawing.Size(86, 13);
            this.linkImportToken.TabIndex = 18;
            this.linkImportToken.Text = "Import token file";
            this.linkImportToken.Click += new System.EventHandler(this.OnImportTokenFile);
            // 
            // labelAdvanced
            // 
            this.labelAdvanced.AutoSize = true;
            this.labelAdvanced.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelAdvanced.Location = new System.Drawing.Point(136, 68);
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
            this.linkExportToken.Location = new System.Drawing.Point(136, 84);
            this.linkExportToken.Name = "linkExportToken";
            this.linkExportToken.Size = new System.Drawing.Size(86, 13);
            this.linkExportToken.TabIndex = 16;
            this.linkExportToken.Text = "Export token file";
            this.linkExportToken.Click += new System.EventHandler(this.OnExportTokenFile);
            // 
            // NetworkSolutionsManageTokenControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.linkImportToken);
            this.Controls.Add(this.labelAdvanced);
            this.Controls.Add(this.linkExportToken);
            this.Controls.Add(this.createTokenControl);
            this.Controls.Add(this.tokenTextBox);
            this.Controls.Add(this.label1);
            this.Name = "NetworkSolutionsManageTokenControl";
            this.Size = new System.Drawing.Size(524, 106);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tokenTextBox;
        private NetworkSolutionsCreateTokenControl createTokenControl;
        private ShipWorks.UI.Controls.LinkControl linkImportToken;
        private System.Windows.Forms.Label labelAdvanced;
        private ShipWorks.UI.Controls.LinkControl linkExportToken;
    }
}
