﻿namespace ShipWorks.Stores.Platforms.ProStores
{
    partial class ProStoresTokenManageControl
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
            this.labelToken = new System.Windows.Forms.Label();
            this.labelAdvanced = new System.Windows.Forms.Label();
            this.linkImportToken = new ShipWorks.UI.Controls.LinkControl();
            this.linkExportToken = new ShipWorks.UI.Controls.LinkControl();
            this.tokenBox = new System.Windows.Forms.TextBox();
            this.changeLoginToken = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelToken
            // 
            this.labelToken.AutoSize = true;
            this.labelToken.Location = new System.Drawing.Point(6, 9);
            this.labelToken.Name = "labelToken";
            this.labelToken.Size = new System.Drawing.Size(90, 13);
            this.labelToken.TabIndex = 11;
            this.labelToken.Text = "ProStores Token:";
            // 
            // labelAdvanced
            // 
            this.labelAdvanced.AutoSize = true;
            this.labelAdvanced.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelAdvanced.Location = new System.Drawing.Point(102, 70);
            this.labelAdvanced.Name = "labelAdvanced";
            this.labelAdvanced.Size = new System.Drawing.Size(59, 13);
            this.labelAdvanced.TabIndex = 14;
            this.labelAdvanced.Text = "Advanced:";
            // 
            // linkImportToken
            // 
            this.linkImportToken.AutoSize = true;
            this.linkImportToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkImportToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkImportToken.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkImportToken.Location = new System.Drawing.Point(189, 86);
            this.linkImportToken.Name = "linkImportToken";
            this.linkImportToken.Size = new System.Drawing.Size(86, 13);
            this.linkImportToken.TabIndex = 15;
            this.linkImportToken.Text = "Import token file";
            this.linkImportToken.Click += new System.EventHandler(this.OnImportTokenFile);
            // 
            // linkExportToken
            // 
            this.linkExportToken.AutoSize = true;
            this.linkExportToken.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkExportToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkExportToken.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.linkExportToken.Location = new System.Drawing.Point(102, 86);
            this.linkExportToken.Name = "linkExportToken";
            this.linkExportToken.Size = new System.Drawing.Size(86, 13);
            this.linkExportToken.TabIndex = 13;
            this.linkExportToken.Text = "Export token file";
            this.linkExportToken.Click += new System.EventHandler(this.OnExportTokenFile);
            // 
            // tokenBox
            // 
            this.tokenBox.Location = new System.Drawing.Point(102, 6);
            this.tokenBox.Name = "tokenBox";
            this.tokenBox.ReadOnly = true;
            this.tokenBox.Size = new System.Drawing.Size(388, 21);
            this.tokenBox.TabIndex = 12;
            // 
            // changeLoginToken
            // 
            this.changeLoginToken.Location = new System.Drawing.Point(102, 33);
            this.changeLoginToken.Name = "changeLoginToken";
            this.changeLoginToken.Size = new System.Drawing.Size(136, 23);
            this.changeLoginToken.TabIndex = 16;
            this.changeLoginToken.Text = "Change Login Token...";
            this.changeLoginToken.UseVisualStyleBackColor = true;
            this.changeLoginToken.Click += new System.EventHandler(this.OnChangeToken);
            // 
            // ProStoresTokenManageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.changeLoginToken);
            this.Controls.Add(this.linkImportToken);
            this.Controls.Add(this.labelAdvanced);
            this.Controls.Add(this.linkExportToken);
            this.Controls.Add(this.tokenBox);
            this.Controls.Add(this.labelToken);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "ProStoresTokenManageControl";
            this.Size = new System.Drawing.Size(524, 109);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelToken;
        private ShipWorks.UI.Controls.LinkControl linkExportToken;
        private System.Windows.Forms.Label labelAdvanced;
        private ShipWorks.UI.Controls.LinkControl linkImportToken;
        private System.Windows.Forms.TextBox tokenBox;
        private System.Windows.Forms.Button changeLoginToken;

    }
}
