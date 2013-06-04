namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor
{
    partial class MarketplaceAdvisorAccountSettingsControl
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
            this.panelLegacy = new System.Windows.Forms.Panel();
            this.labelLegacyServer = new System.Windows.Forms.Label();
            this.radioCorporate = new System.Windows.Forms.RadioButton();
            this.radioStandard = new System.Windows.Forms.RadioButton();
            this.labelUpgradeOms = new System.Windows.Forms.Label();
            this.buttonOms = new System.Windows.Forms.Button();
            this.panelLegacy.SuspendLayout();
            this.SuspendLayout();
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(103, 63);
            this.password.MaxLength = 50;
            this.password.Name = "password";
            this.password.UseSystemPasswordChar = true;
            this.password.Size = new System.Drawing.Size(230, 21);
            this.password.TabIndex = 14;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(103, 37);
            this.username.MaxLength = 50;
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(230, 21);
            this.username.TabIndex = 12;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(38, 66);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 13;
            this.labelPassword.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(36, 40);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 11;
            this.labelUsername.Text = "Username:";
            // 
            // labelEnterCredentials
            // 
            this.labelEnterCredentials.AutoSize = true;
            this.labelEnterCredentials.Location = new System.Drawing.Point(17, 13);
            this.labelEnterCredentials.Name = "labelEnterCredentials";
            this.labelEnterCredentials.Size = new System.Drawing.Size(246, 13);
            this.labelEnterCredentials.TabIndex = 10;
            this.labelEnterCredentials.Text = "Enter your MarketplaceAdvisor username and password:";
            // 
            // panelLegacy
            // 
            this.panelLegacy.Controls.Add(this.buttonOms);
            this.panelLegacy.Controls.Add(this.labelUpgradeOms);
            this.panelLegacy.Controls.Add(this.radioCorporate);
            this.panelLegacy.Controls.Add(this.radioStandard);
            this.panelLegacy.Controls.Add(this.labelLegacyServer);
            this.panelLegacy.Location = new System.Drawing.Point(3, 90);
            this.panelLegacy.Name = "panelLegacy";
            this.panelLegacy.Size = new System.Drawing.Size(475, 155);
            this.panelLegacy.TabIndex = 15;
            // 
            // labelLegacyServer
            // 
            this.labelLegacyServer.AutoSize = true;
            this.labelLegacyServer.Location = new System.Drawing.Point(14, 13);
            this.labelLegacyServer.Name = "labelLegacyServer";
            this.labelLegacyServer.Size = new System.Drawing.Size(248, 13);
            this.labelLegacyServer.TabIndex = 0;
            this.labelLegacyServer.Text = "Select the MarketplaceAdvisor server your account uses:";
            // 
            // radioCorporate
            // 
            this.radioCorporate.AutoSize = true;
            this.radioCorporate.Location = new System.Drawing.Point(36, 56);
            this.radioCorporate.Name = "radioCorporate";
            this.radioCorporate.Size = new System.Drawing.Size(109, 17);
            this.radioCorporate.TabIndex = 15;
            this.radioCorporate.TabStop = true;
            this.radioCorporate.Text = "Corporate Server";
            this.radioCorporate.UseVisualStyleBackColor = true;
            // 
            // radioStandard
            // 
            this.radioStandard.AutoSize = true;
            this.radioStandard.Location = new System.Drawing.Point(36, 33);
            this.radioStandard.Name = "radioStandard";
            this.radioStandard.Size = new System.Drawing.Size(104, 17);
            this.radioStandard.TabIndex = 14;
            this.radioStandard.TabStop = true;
            this.radioStandard.Text = "Standard Server";
            this.radioStandard.UseVisualStyleBackColor = true;
            // 
            // labelUpgradeOms
            // 
            this.labelUpgradeOms.Location = new System.Drawing.Point(14, 87);
            this.labelUpgradeOms.Name = "labelUpgradeOms";
            this.labelUpgradeOms.Size = new System.Drawing.Size(440, 35);
            this.labelUpgradeOms.TabIndex = 16;
            this.labelUpgradeOms.Text = "If you upgrade your MarketplaceAdvisor account to use OMS, click the button below to upd" +
                "ated ShipWorks.  Do this after your MarketplaceAdvisor account is upgraded.";
            // 
            // buttonOms
            // 
            this.buttonOms.Location = new System.Drawing.Point(38, 119);
            this.buttonOms.Name = "buttonOms";
            this.buttonOms.Size = new System.Drawing.Size(102, 23);
            this.buttonOms.TabIndex = 17;
            this.buttonOms.Text = "Change to OMS...";
            this.buttonOms.UseVisualStyleBackColor = true;
            this.buttonOms.Click += new System.EventHandler(this.OnChangeToOms);
            // 
            // MarketplaceAdvisorAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelLegacy);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.labelEnterCredentials);
            this.Name = "MarketplaceAdvisorAccountSettingsControl";
            this.Size = new System.Drawing.Size(497, 351);
            this.panelLegacy.ResumeLayout(false);
            this.panelLegacy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelEnterCredentials;
        private System.Windows.Forms.Panel panelLegacy;
        private System.Windows.Forms.Label labelLegacyServer;
        private System.Windows.Forms.RadioButton radioCorporate;
        private System.Windows.Forms.RadioButton radioStandard;
        private System.Windows.Forms.Label labelUpgradeOms;
        private System.Windows.Forms.Button buttonOms;
    }
}
