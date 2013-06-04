namespace ShipWorks.Stores.Platforms.Ebay
{
    partial class EbayTokenUpdateDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tokenManageControl = new ShipWorks.Stores.Platforms.Ebay.EbayTokenManageControl();
            this.cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tokenManageControl
            // 
            this.tokenManageControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.tokenManageControl.Location = new System.Drawing.Point(2, 3);
            this.tokenManageControl.Name = "tokenManageControl";
            this.tokenManageControl.Size = new System.Drawing.Size(524, 109);
            this.tokenManageControl.TabIndex = 0;
            this.tokenManageControl.TokenImported += new System.EventHandler(this.OnTokenImported);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(417, 111);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Close";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // EbayTokenUpdateDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(504, 146);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.tokenManageControl);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EbayTokenUpdateDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update eBay Login Token";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private EbayTokenManageControl tokenManageControl;
        private System.Windows.Forms.Button cancel;
    }
}