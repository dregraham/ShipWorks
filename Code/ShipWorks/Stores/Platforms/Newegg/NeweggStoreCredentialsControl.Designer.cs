namespace ShipWorks.Stores.Platforms.Newegg
{
    partial class NeweggStoreCredentialsControl
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
            this.secretKey = new System.Windows.Forms.TextBox();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.sellerId = new System.Windows.Forms.TextBox();
            this.lblSellerId = new System.Windows.Forms.Label();
            this.lblEnterCredentials = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // secretKey
            // 
            this.secretKey.Location = new System.Drawing.Point(87, 64);
            this.secretKey.Name = "secretKey";
            this.secretKey.Size = new System.Drawing.Size(229, 21);
            this.secretKey.TabIndex = 14;
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(19, 67);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(63, 13);
            this.lblSecretKey.TabIndex = 13;
            this.lblSecretKey.Text = "Secret Key:";
            this.lblSecretKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // sellerId
            // 
            this.sellerId.Location = new System.Drawing.Point(87, 31);
            this.sellerId.Name = "sellerId";
            this.sellerId.Size = new System.Drawing.Size(229, 21);
            this.sellerId.TabIndex = 12;
            // 
            // lblSellerId
            // 
            this.lblSellerId.AutoSize = true;
            this.lblSellerId.Location = new System.Drawing.Point(31, 34);
            this.lblSellerId.Name = "lblSellerId";
            this.lblSellerId.Size = new System.Drawing.Size(51, 13);
            this.lblSellerId.TabIndex = 11;
            this.lblSellerId.Text = "Seller ID:";
            this.lblSellerId.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEnterCredentials
            // 
            this.lblEnterCredentials.AutoSize = true;
            this.lblEnterCredentials.Location = new System.Drawing.Point(3, 6);
            this.lblEnterCredentials.Name = "lblEnterCredentials";
            this.lblEnterCredentials.Size = new System.Drawing.Size(333, 13);
            this.lblEnterCredentials.TabIndex = 10;
            this.lblEnterCredentials.Text = "Enter your seller ID and the secret key provided to you by Newegg:";
            // 
            // NeweggStoreSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.secretKey);
            this.Controls.Add(this.lblSecretKey);
            this.Controls.Add(this.sellerId);
            this.Controls.Add(this.lblSellerId);
            this.Controls.Add(this.lblEnterCredentials);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NeweggStoreSettingsControl";
            this.Size = new System.Drawing.Size(375, 114);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox secretKey;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.TextBox sellerId;
        private System.Windows.Forms.Label lblSellerId;
        private System.Windows.Forms.Label lblEnterCredentials;

    }
}
