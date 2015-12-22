namespace ShipWorks.Stores.Platforms.BigCommerce
{
    partial class BigCommerceAccountSettingsControl
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
            this.apiUsername = new System.Windows.Forms.TextBox();
            this.lblApiUsername = new System.Windows.Forms.Label();
            this.apiUrl = new System.Windows.Forms.TextBox();
            this.lblApiPath = new System.Windows.Forms.Label();
            this.lblEnterCredentials = new System.Windows.Forms.Label();
            this.apiToken = new System.Windows.Forms.TextBox();
            this.lblApiToken = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.helpLink = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.SuspendLayout();
            // 
            // apiUsername
            // 
            this.apiUsername.Location = new System.Drawing.Point(97, 56);
            this.apiUsername.Name = "apiUsername";
            this.apiUsername.Size = new System.Drawing.Size(357, 21);
            this.apiUsername.TabIndex = 19;
            // 
            // lblApiUsername
            // 
            this.lblApiUsername.AutoSize = true;
            this.lblApiUsername.Location = new System.Drawing.Point(14, 59);
            this.lblApiUsername.Name = "lblApiUsername";
            this.lblApiUsername.Size = new System.Drawing.Size(79, 13);
            this.lblApiUsername.TabIndex = 18;
            this.lblApiUsername.Text = "API Username:";
            this.lblApiUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // apiUrl
            // 
            this.apiUrl.Location = new System.Drawing.Point(97, 30);
            this.apiUrl.Name = "apiUrl";
            this.apiUrl.Size = new System.Drawing.Size(357, 21);
            this.apiUrl.TabIndex = 17;
            // 
            // lblApiPath
            // 
            this.lblApiPath.AutoSize = true;
            this.lblApiPath.Location = new System.Drawing.Point(40, 33);
            this.lblApiPath.Name = "lblApiPath";
            this.lblApiPath.Size = new System.Drawing.Size(53, 13);
            this.lblApiPath.TabIndex = 16;
            this.lblApiPath.Text = "API Path:";
            this.lblApiPath.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEnterCredentials
            // 
            this.lblEnterCredentials.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblEnterCredentials.AutoSize = true;
            this.lblEnterCredentials.Location = new System.Drawing.Point(3, 6);
            this.lblEnterCredentials.MaximumSize = new System.Drawing.Size(450, 0);
            this.lblEnterCredentials.Name = "lblEnterCredentials";
            this.lblEnterCredentials.Size = new System.Drawing.Size(439, 13);
            this.lblEnterCredentials.TabIndex = 15;
            this.lblEnterCredentials.Text = "Enter your API Path, API Username, and the API Token provided to you by BigCommer" +
    "ce.";
            // 
            // apiToken
            // 
            this.apiToken.Location = new System.Drawing.Point(97, 81);
            this.apiToken.Name = "apiToken";
            this.apiToken.Size = new System.Drawing.Size(357, 21);
            this.apiToken.TabIndex = 22;
            // 
            // lblApiToken
            // 
            this.lblApiToken.AutoSize = true;
            this.lblApiToken.Location = new System.Drawing.Point(33, 84);
            this.lblApiToken.Name = "lblApiToken";
            this.lblApiToken.Size = new System.Drawing.Size(60, 13);
            this.lblApiToken.TabIndex = 21;
            this.lblApiToken.Text = "API Token:";
            this.lblApiToken.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(95, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(266, 18);
            this.label5.TabIndex = 23;
            this.label5.Text = "For help finding your API Path, Username or Token, ";
            // 
            // helpLink
            // 
            this.helpLink.AutoSize = true;
            this.helpLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink.ForeColor = System.Drawing.Color.Blue;
            this.helpLink.Location = new System.Drawing.Point(345, 105);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(55, 18);
            this.helpLink.TabIndex = 24;
            this.helpLink.TabStop = true;
            this.helpLink.Text = "click here.";
            this.helpLink.Url = "http://support.shipworks.com/support/solutions/articles/155262-adding-a-bigcommer" +
    "ce-store-";
            this.helpLink.UseCompatibleTextRendering = true;
            // 
            // BigCommerceAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.apiToken);
            this.Controls.Add(this.lblApiToken);
            this.Controls.Add(this.apiUsername);
            this.Controls.Add(this.lblApiUsername);
            this.Controls.Add(this.apiUrl);
            this.Controls.Add(this.lblApiPath);
            this.Controls.Add(this.lblEnterCredentials);
            this.Name = "BigCommerceAccountSettingsControl";
            this.Size = new System.Drawing.Size(463, 127);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox apiUsername;
        private System.Windows.Forms.Label lblApiUsername;
        private System.Windows.Forms.TextBox apiUrl;
        private System.Windows.Forms.Label lblApiPath;
        private System.Windows.Forms.Label lblEnterCredentials;
        private System.Windows.Forms.TextBox apiToken;
        private System.Windows.Forms.Label lblApiToken;
        protected System.Windows.Forms.Label label5;
        protected ApplicationCore.Interaction.HelpLink helpLink;
    }
}
