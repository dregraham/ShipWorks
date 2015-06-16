namespace ShipWorks.Stores.Platforms.Newegg
{
    partial class NeweggStoreMarketplaceControl
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
            this.lblEnterCredentials = new System.Windows.Forms.Label();
            this.marketplace = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lblEnterCredentials
            // 
            this.lblEnterCredentials.AutoSize = true;
            this.lblEnterCredentials.Location = new System.Drawing.Point(3, 6);
            this.lblEnterCredentials.Name = "lblEnterCredentials";
            this.lblEnterCredentials.Size = new System.Drawing.Size(243, 13);
            this.lblEnterCredentials.TabIndex = 10;
            this.lblEnterCredentials.Text = "What type of NewEgg store do you want to add?";
            // 
            // marketplace
            // 
            this.marketplace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.marketplace.FormattingEnabled = true;
            this.marketplace.Location = new System.Drawing.Point(38, 36);
            this.marketplace.Name = "marketplace";
            this.marketplace.Size = new System.Drawing.Size(208, 21);
            this.marketplace.TabIndex = 11;
            // 
            // NeweggStoreMarketplaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.marketplace);
            this.Controls.Add(this.lblEnterCredentials);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NeweggStoreMarketplaceControl";
            this.Size = new System.Drawing.Size(375, 114);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEnterCredentials;
        private System.Windows.Forms.ComboBox marketplace;

    }
}
