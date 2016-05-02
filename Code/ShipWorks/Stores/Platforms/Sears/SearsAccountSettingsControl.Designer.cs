namespace ShipWorks.Stores.Platforms.Sears
{
    partial class SearsAccountSettingsControl
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
            this.email = new System.Windows.Forms.TextBox();
            this.labelHeader = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelSellerID = new System.Windows.Forms.Label();
            this.labelSecretKey = new System.Windows.Forms.Label();
            this.labelHelp = new System.Windows.Forms.Label();
            this.sellerID = new System.Windows.Forms.TextBox();
            this.secretKey = new System.Windows.Forms.TextBox();
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.SuspendLayout();
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(99, 29);
            this.email.MaxLength = 75;
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(230, 21);
            this.email.TabIndex = 6;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.Location = new System.Drawing.Point(2, 3);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(401, 13);
            this.labelHeader.TabIndex = 5;
            this.labelHeader.Text = "Please enter the email, seller ID, and secret key associated with your Sears stor" +
    "e:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(49, 32);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(35, 13);
            this.labelEmail.TabIndex = 8;
            this.labelEmail.Text = "Email:";
            // 
            // labelSellerID
            // 
            this.labelSellerID.AutoSize = true;
            this.labelSellerID.Location = new System.Drawing.Point(33, 59);
            this.labelSellerID.Name = "labelSellerID";
            this.labelSellerID.Size = new System.Drawing.Size(51, 13);
            this.labelSellerID.TabIndex = 9;
            this.labelSellerID.Text = "Seller ID:";
            // 
            // labelSecretKey
            // 
            this.labelSecretKey.AutoSize = true;
            this.labelSecretKey.Location = new System.Drawing.Point(21, 87);
            this.labelSecretKey.Name = "labelSecretKey";
            this.labelSecretKey.Size = new System.Drawing.Size(63, 13);
            this.labelSecretKey.TabIndex = 10;
            this.labelSecretKey.Text = "Secret Key:";
            // 
            // labelHelp
            // 
            this.labelHelp.AutoSize = true;
            this.labelHelp.Location = new System.Drawing.Point(96, 108);
            this.labelHelp.Name = "labelHelp";
            this.labelHelp.Size = new System.Drawing.Size(167, 13);
            this.labelHelp.TabIndex = 11;
            this.labelHelp.Text = "For help getting this information, ";
            // 
            // sellerID
            // 
            this.sellerID.Location = new System.Drawing.Point(99, 56);
            this.sellerID.MaxLength = 15;
            this.sellerID.Name = "sellerID";
            this.sellerID.Size = new System.Drawing.Size(230, 21);
            this.sellerID.TabIndex = 12;
            // 
            // secretKey
            // 
            this.secretKey.Location = new System.Drawing.Point(99, 84);
            this.secretKey.MaxLength = 200;
            this.secretKey.Name = "secretKey";
            this.secretKey.Size = new System.Drawing.Size(230, 21);
            this.secretKey.TabIndex = 13;
            // 
            // helpLink
            // 
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(256, 108);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(55, 13);
            this.helpLink.TabIndex = 14;
            this.helpLink.Text = "click here.";
            this.helpLink.Url = "http://www.interapptive.com/shipworks/help";
            // 
            // SearsAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.secretKey);
            this.Controls.Add(this.sellerID);
            this.Controls.Add(this.labelHelp);
            this.Controls.Add(this.labelSecretKey);
            this.Controls.Add(this.labelSellerID);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.email);
            this.Controls.Add(this.labelHeader);
            this.Name = "SearsAccountSettingsControl";
            this.Size = new System.Drawing.Size(439, 179);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelSellerID;
        private System.Windows.Forms.Label labelSecretKey;
        private System.Windows.Forms.Label labelHelp;
        private System.Windows.Forms.TextBox sellerID;
        private System.Windows.Forms.TextBox secretKey;
        private ApplicationCore.Interaction.HelpLink helpLink;
    }
}
