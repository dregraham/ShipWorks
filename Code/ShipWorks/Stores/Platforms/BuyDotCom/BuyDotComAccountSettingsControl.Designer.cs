namespace ShipWorks.Stores.Platforms.BuyDotCom
{
    partial class BuyDotComAccountSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BuyDotComAccountSettingsControl));
            this.labelCredentials = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.emailAddress = new System.Windows.Forms.LinkLabel();
            this.labelInstructions = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelCredentials
            // 
            this.labelCredentials.AutoSize = true;
            this.labelCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelCredentials.Location = new System.Drawing.Point(9, 101);
            this.labelCredentials.Name = "labelCredentials";
            this.labelCredentials.Size = new System.Drawing.Size(96, 13);
            this.labelCredentials.TabIndex = 13;
            this.labelCredentials.Text = "FTP credentials:";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(85, 147);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(170, 21);
            this.password.TabIndex = 12;
            this.password.UseSystemPasswordChar = true;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(85, 120);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(170, 21);
            this.username.TabIndex = 11;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(21, 123);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 10;
            this.labelUsername.Text = "Username:";
            this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(21, 147);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 9;
            this.labelPassword.Text = "Password:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // emailAddress
            // 
            this.emailAddress.AutoSize = true;
            this.emailAddress.Location = new System.Drawing.Point(279, 47);
            this.emailAddress.Name = "emailAddress";
            this.emailAddress.Size = new System.Drawing.Size(128, 13);
            this.emailAddress.TabIndex = 8;
            this.emailAddress.TabStop = true;
            this.emailAddress.Text = "mp-integration@buy.com";
            this.emailAddress.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnEmailAddressLinkClicked);
            // 
            // labelInstructions
            // 
            this.labelInstructions.Location = new System.Drawing.Point(9, 8);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(502, 86);
            this.labelInstructions.TabIndex = 7;
            this.labelInstructions.Text = resources.GetString("labelInstructions.Text");
            // 
            // BuyDotComAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelCredentials);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.emailAddress);
            this.Controls.Add(this.labelInstructions);
            this.Name = "BuyDotComAccountSettingsControl";
            this.Size = new System.Drawing.Size(485, 182);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelCredentials;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.LinkLabel emailAddress;
        private System.Windows.Forms.Label labelInstructions;

    }
}
