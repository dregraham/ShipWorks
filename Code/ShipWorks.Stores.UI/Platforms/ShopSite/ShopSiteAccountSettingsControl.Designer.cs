namespace ShipWorks.Stores.UI.Platforms.ShopSite
{
    partial class ShopSiteAccountSettingsControl
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
            this.connectUnsecure = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.apiUrl = new System.Windows.Forms.TextBox();
            this.labelModuleUrl = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.infoTipUnsecure = new ShipWorks.UI.Controls.InfoTip();
            this.SuspendLayout();
            //
            // connectUnsecure
            //
            this.connectUnsecure.Location = new System.Drawing.Point(90, 150);
            this.connectUnsecure.Name = "connectUnsecure";
            this.connectUnsecure.Size = new System.Drawing.Size(194, 22);
            this.connectUnsecure.TabIndex = 8;
            this.connectUnsecure.Text = "Use unsecure http:// connection.";
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(9, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(368, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter your ShopSite Merchant ID and password.";
            //
            // apiUrl
            //
            this.apiUrl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.apiUrl.Location = new System.Drawing.Point(90, 126);
            this.apiUrl.Name = "apiUrl";
            this.apiUrl.Size = new System.Drawing.Size(338, 21);
            this.apiUrl.TabIndex = 7;
            //
            // labelModuleUrl
            //
            this.labelModuleUrl.AutoSize = true;
            this.labelModuleUrl.Location = new System.Drawing.Point(31, 129);
            this.labelModuleUrl.Name = "labelModuleUrl";
            this.labelModuleUrl.Size = new System.Drawing.Size(51, 13);
            this.labelModuleUrl.TabIndex = 6;
            this.labelModuleUrl.Text = "CGI URL:";
            //
            // password
            //
            this.password.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.password.Location = new System.Drawing.Point(93, 54);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(206, 21);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            //
            // labelPassword
            //
            this.labelPassword.AutoSize = true;
            this.labelPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelPassword.Location = new System.Drawing.Point(24, 57);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password:";
            //
            // username
            //
            this.username.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.username.Location = new System.Drawing.Point(93, 28);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(206, 21);
            this.username.TabIndex = 2;
            //
            // labelUsername
            //
            this.labelUsername.AutoSize = true;
            this.labelUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUsername.Location = new System.Drawing.Point(11, 31);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(70, 13);
            this.labelUsername.TabIndex = 1;
            this.labelUsername.Text = "Merchant ID:";
            //
            // label2
            //
            this.label2.Location = new System.Drawing.Point(9, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(479, 34);
            this.label2.TabIndex = 5;
            this.label2.Text = "Enter the URL to the CGI script for downloading orders.  This is usually the same" +
                " as your store\'s start.cgi file, but replace \"start.cgi\" with \"db_xml.cgi\".";
            //
            // infoTipUnsecure
            //
            this.infoTipUnsecure.Caption = "Order payment details will not be downloaded on an unsecure connection.";
            this.infoTipUnsecure.Location = new System.Drawing.Point(272, 153);
            this.infoTipUnsecure.Name = "infoTipUnsecure";
            this.infoTipUnsecure.Size = new System.Drawing.Size(12, 12);
            this.infoTipUnsecure.TabIndex = 97;
            this.infoTipUnsecure.Title = "Unsecure Connection";
            //
            // ShopSiteAccountSettingsControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.infoTipUnsecure);
            this.Controls.Add(this.connectUnsecure);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.apiUrl);
            this.Controls.Add(this.labelModuleUrl);
            this.Controls.Add(this.password);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.username);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.label2);
            this.Name = "ShopSiteAccountSettingsControl";
            this.Size = new System.Drawing.Size(494, 189);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox connectUnsecure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox apiUrl;
        private System.Windows.Forms.Label labelModuleUrl;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label label2;
        private ShipWorks.UI.Controls.InfoTip infoTipUnsecure;
    }
}
