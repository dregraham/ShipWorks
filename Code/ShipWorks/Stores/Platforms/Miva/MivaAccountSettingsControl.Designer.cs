namespace ShipWorks.Stores.Platforms.Miva
{
    partial class MivaAccountSettingsControl
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
            this.labelEncryptionInfo = new System.Windows.Forms.Label();
            this.encryptionPassphrase = new System.Windows.Forms.TextBox();
            this.labelPassphrase = new System.Windows.Forms.Label();
            this.labelStoreCode = new System.Windows.Forms.Label();
            this.storeCode = new System.Windows.Forms.TextBox();
            this.infoStoreCode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            this.helpLink.Location = new System.Drawing.Point(36, 180);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(24, 206);
            // 
            // moduleUrl
            // 
            this.moduleUrl.Location = new System.Drawing.Point(97, 203);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 168);
            // 
            // labelEncryptionInfo
            // 
            this.labelEncryptionInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelEncryptionInfo.Location = new System.Drawing.Point(94, 127);
            this.labelEncryptionInfo.Name = "labelEncryptionInfo";
            this.labelEncryptionInfo.Size = new System.Drawing.Size(405, 38);
            this.labelEncryptionInfo.TabIndex = 12;
            this.labelEncryptionInfo.Text = "(Optional.  The passphrase is used to decrypt payment details when encryption is " +
                "enabled for Merchant 4.14 and higher.)";
            // 
            // encryptionPassphrase
            // 
            this.encryptionPassphrase.Location = new System.Drawing.Point(97, 103);
            this.encryptionPassphrase.Name = "encryptionPassphrase";
            this.encryptionPassphrase.Size = new System.Drawing.Size(268, 21);
            this.encryptionPassphrase.TabIndex = 11;
            this.encryptionPassphrase.UseSystemPasswordChar = true;
            // 
            // labelPassphrase
            // 
            this.labelPassphrase.AutoSize = true;
            this.labelPassphrase.Location = new System.Drawing.Point(24, 83);
            this.labelPassphrase.Name = "labelPassphrase";
            this.labelPassphrase.Size = new System.Drawing.Size(120, 13);
            this.labelPassphrase.TabIndex = 10;
            this.labelPassphrase.Text = "Encryption Passphrase:";
            // 
            // labelStoreCode
            // 
            this.labelStoreCode.AutoSize = true;
            this.labelStoreCode.Location = new System.Drawing.Point(26, 233);
            this.labelStoreCode.Name = "labelStoreCode";
            this.labelStoreCode.Size = new System.Drawing.Size(65, 13);
            this.labelStoreCode.TabIndex = 13;
            this.labelStoreCode.Text = "Store Code:";
            // 
            // storeCode
            // 
            this.storeCode.Location = new System.Drawing.Point(97, 230);
            this.storeCode.Name = "storeCode";
            this.storeCode.Size = new System.Drawing.Size(100, 21);
            this.storeCode.TabIndex = 14;
            // 
            // infoStoreCode
            // 
            this.infoStoreCode.AutoSize = true;
            this.infoStoreCode.ForeColor = System.Drawing.SystemColors.GrayText;
            this.infoStoreCode.Location = new System.Drawing.Point(94, 254);
            this.infoStoreCode.Name = "infoStoreCode";
            this.infoStoreCode.Size = new System.Drawing.Size(387, 13);
            this.infoStoreCode.TabIndex = 15;
            this.infoStoreCode.Text = "(This can be blank of you only have one store on your Miva Merchant website.)";
            // 
            // MivaAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infoStoreCode);
            this.Controls.Add(this.storeCode);
            this.Controls.Add(this.labelStoreCode);
            this.Controls.Add(this.labelEncryptionInfo);
            this.Controls.Add(this.encryptionPassphrase);
            this.Controls.Add(this.labelPassphrase);
            this.Name = "MivaAccountSettingsControl";
            this.Size = new System.Drawing.Size(527, 401);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.helpLink, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.moduleUrl, 0);
            this.Controls.SetChildIndex(this.labelPassphrase, 0);
            this.Controls.SetChildIndex(this.encryptionPassphrase, 0);
            this.Controls.SetChildIndex(this.labelEncryptionInfo, 0);
            this.Controls.SetChildIndex(this.labelStoreCode, 0);
            this.Controls.SetChildIndex(this.storeCode, 0);
            this.Controls.SetChildIndex(this.infoStoreCode, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelEncryptionInfo;
        private System.Windows.Forms.TextBox encryptionPassphrase;
        private System.Windows.Forms.Label labelPassphrase;
        private System.Windows.Forms.Label labelStoreCode;
        private System.Windows.Forms.TextBox storeCode;
        private System.Windows.Forms.Label infoStoreCode;
    }
}
