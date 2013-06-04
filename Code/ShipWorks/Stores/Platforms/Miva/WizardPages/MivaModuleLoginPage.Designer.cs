namespace ShipWorks.Stores.Platforms.Miva.WizardPages
{
    partial class MivaModuleLoginPage
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
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelEnterCredentials = new System.Windows.Forms.Label();
            this.labelPassphrase = new System.Windows.Forms.Label();
            this.encryptionPassphrase = new System.Windows.Forms.TextBox();
            this.labelEncryptionInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(104, 60);
            this.password.MaxLength = 50;
            this.password.Name = "password";
            this.password.UseSystemPasswordChar = true;
            this.password.Size = new System.Drawing.Size(230, 21);
            this.password.TabIndex = 4;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(104, 34);
            this.username.MaxLength = 50;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(230, 21);
            this.username.TabIndex = 2;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(39, 63);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(37, 37);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 1;
            this.labelUsername.Text = "Username:";
            // 
            // labelEnterCredentials
            // 
            this.labelEnterCredentials.AutoSize = true;
            this.labelEnterCredentials.Location = new System.Drawing.Point(19, 12);
            this.labelEnterCredentials.Name = "labelEnterCredentials";
            this.labelEnterCredentials.Size = new System.Drawing.Size(418, 13);
            this.labelEnterCredentials.TabIndex = 0;
            this.labelEnterCredentials.Text = "Enter the administrator username and password you use to login to your online sto" +
                "re:";
            // 
            // labelPassphrase
            // 
            this.labelPassphrase.AutoSize = true;
            this.labelPassphrase.Location = new System.Drawing.Point(19, 104);
            this.labelPassphrase.Name = "labelPassphrase";
            this.labelPassphrase.Size = new System.Drawing.Size(120, 13);
            this.labelPassphrase.TabIndex = 5;
            this.labelPassphrase.Text = "Encryption Passphrase:";
            // 
            // encryptionPassphrase
            // 
            this.encryptionPassphrase.Location = new System.Drawing.Point(145, 101);
            this.encryptionPassphrase.Name = "encryptionPassphrase";
            this.encryptionPassphrase.Size = new System.Drawing.Size(268, 21);
            this.encryptionPassphrase.TabIndex = 6;
            // 
            // labelEncryptionInfo
            // 
            this.labelEncryptionInfo.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelEncryptionInfo.Location = new System.Drawing.Point(142, 125);
            this.labelEncryptionInfo.Name = "labelEncryptionInfo";
            this.labelEncryptionInfo.Size = new System.Drawing.Size(308, 38);
            this.labelEncryptionInfo.TabIndex = 7;
            this.labelEncryptionInfo.Text = "(Optional.  The passphrase is used to decrypt payment details when encryption is " +
                "enabled for Merchant 4.14 and higher.)";
            // 
            // MivaModuleLoginPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelEncryptionInfo);
            this.Controls.Add(this.encryptionPassphrase);
            this.Controls.Add(this.labelPassphrase);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelEnterCredentials);
            this.Description = "Enter the following information about your online store.";
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "MivaModuleLoginPage";
            this.Size = new System.Drawing.Size(471, 260);
            this.Title = "Store Setup";
            this.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNext);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelEnterCredentials;
        private System.Windows.Forms.Label labelPassphrase;
        private System.Windows.Forms.TextBox encryptionPassphrase;
        private System.Windows.Forms.Label labelEncryptionInfo;
    }
}
