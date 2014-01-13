namespace ShipWorks.Stores.Platforms.Ebay
{
    partial class EbayAccountSettingsControl
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
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tokenManageControl = new ShipWorks.Stores.Platforms.Ebay.Tokens.EbayTokenManageControl();
            this.SuspendLayout();
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "tkn";
            this.openFileDialog.Filter = "eBay Token File (*.tkn)|*.tkn";
            this.openFileDialog.Title = "Select an eBay Token File";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "tkn";
            this.saveFileDialog.Filter = "eBay Token File (*.tkn)|*.tkn";
            this.saveFileDialog.Title = "Save eBay Token";
            // 
            // tokenManageControl
            // 
            this.tokenManageControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.tokenManageControl.Location = new System.Drawing.Point(3, 3);
            this.tokenManageControl.Name = "tokenManageControl";
            this.tokenManageControl.Size = new System.Drawing.Size(524, 109);
            this.tokenManageControl.TabIndex = 0;
            // 
            // EbayAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tokenManageControl);
            this.Name = "EbayAccountSettingsControl";
            this.Size = new System.Drawing.Size(506, 122);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private ShipWorks.Stores.Platforms.Ebay.Tokens.EbayTokenManageControl tokenManageControl;
    }
}
