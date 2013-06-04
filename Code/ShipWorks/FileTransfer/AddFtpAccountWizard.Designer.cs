namespace ShipWorks.FileTransfer
{
    partial class AddFtpAccountWizard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wizardPageAccount = new ShipWorks.UI.Wizard.WizardPage();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelHostExample = new System.Windows.Forms.Label();
            this.host = new System.Windows.Forms.TextBox();
            this.labelHost = new System.Windows.Forms.Label();
            this.wizardPageSuccess = new ShipWorks.UI.Wizard.WizardPage();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.wizardPageFolder = new ShipWorks.UI.Wizard.WizardPage();
            this.labelFolder = new System.Windows.Forms.Label();
            this.folderBrowser = new ShipWorks.FileTransfer.FtpFolderBrowserControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageAccount.SuspendLayout();
            this.wizardPageSuccess.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).BeginInit();
            this.wizardPageFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.next.Location = new System.Drawing.Point(317, 404);
            this.next.Text = "Finish";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(398, 404);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(236, 404);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageSuccess);
            this.mainPanel.Size = new System.Drawing.Size(485, 332);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 394);
            this.etchBottom.Size = new System.Drawing.Size(489, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.server_client1;
            this.pictureBox.Location = new System.Drawing.Point(432, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(485, 56);
            // 
            // wizardPageAccount
            // 
            this.wizardPageAccount.Controls.Add(this.password);
            this.wizardPageAccount.Controls.Add(this.labelPassword);
            this.wizardPageAccount.Controls.Add(this.username);
            this.wizardPageAccount.Controls.Add(this.labelUsername);
            this.wizardPageAccount.Controls.Add(this.labelHostExample);
            this.wizardPageAccount.Controls.Add(this.host);
            this.wizardPageAccount.Controls.Add(this.labelHost);
            this.wizardPageAccount.Description = "ShipWorks will help you configure your FTP account settings.";
            this.wizardPageAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageAccount.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccount.Name = "wizardPageAccount";
            this.wizardPageAccount.Size = new System.Drawing.Size(485, 332);
            this.wizardPageAccount.TabIndex = 0;
            this.wizardPageAccount.Title = "Account Setup";
            this.wizardPageAccount.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccount);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(100, 68);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(148, 21);
            this.password.TabIndex = 6;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(37, 71);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 5;
            this.labelPassword.Text = "Password:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(100, 41);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(148, 21);
            this.username.TabIndex = 4;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(40, 44);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 3;
            this.labelUsername.Text = "Username:";
            // 
            // labelHostExample
            // 
            this.labelHostExample.AutoSize = true;
            this.labelHostExample.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelHostExample.Location = new System.Drawing.Point(350, 17);
            this.labelHostExample.Name = "labelHostExample";
            this.labelHostExample.Size = new System.Drawing.Size(96, 13);
            this.labelHostExample.TabIndex = 2;
            this.labelHostExample.Text = "(ftp.example.com)";
            // 
            // host
            // 
            this.host.Location = new System.Drawing.Point(100, 14);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(244, 21);
            this.host.TabIndex = 1;
            // 
            // labelHost
            // 
            this.labelHost.AutoSize = true;
            this.labelHost.Location = new System.Drawing.Point(40, 17);
            this.labelHost.Name = "labelHost";
            this.labelHost.Size = new System.Drawing.Size(54, 13);
            this.labelHost.TabIndex = 0;
            this.labelHost.Text = "FTP Host:";
            // 
            // wizardPageSuccess
            // 
            this.wizardPageSuccess.Controls.Add(this.iconSetupComplete);
            this.wizardPageSuccess.Controls.Add(this.label6);
            this.wizardPageSuccess.Description = "Your FTP account has been added to ShipWorks.";
            this.wizardPageSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSuccess.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageSuccess.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSuccess.Name = "wizardPageSuccess";
            this.wizardPageSuccess.Size = new System.Drawing.Size(485, 332);
            this.wizardPageSuccess.TabIndex = 0;
            this.wizardPageSuccess.Title = "Setup Complete";
            this.wizardPageSuccess.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoSuccess);
            // 
            // iconSetupComplete
            // 
            this.iconSetupComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.iconSetupComplete.Location = new System.Drawing.Point(23, 11);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 6;
            this.iconSetupComplete.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(42, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(245, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "ShipWorks is now ready to use your FTP account.";
            // 
            // wizardPageFolder
            // 
            this.wizardPageFolder.Controls.Add(this.labelFolder);
            this.wizardPageFolder.Controls.Add(this.folderBrowser);
            this.wizardPageFolder.Description = "Select the initial folder to use for your FTP account.";
            this.wizardPageFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFolder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageFolder.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFolder.Name = "wizardPageFolder";
            this.wizardPageFolder.Size = new System.Drawing.Size(485, 332);
            this.wizardPageFolder.TabIndex = 0;
            this.wizardPageFolder.Title = "FTP Folder";
            this.wizardPageFolder.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextFolderBrowse);
            this.wizardPageFolder.PageShown += new System.EventHandler<ShipWorks.UI.Wizard.WizardPageShownEventArgs>(this.OnShownFolderBrowse);
            // 
            // labelFolder
            // 
            this.labelFolder.AutoSize = true;
            this.labelFolder.Location = new System.Drawing.Point(21, 9);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(123, 13);
            this.labelFolder.TabIndex = 1;
            this.labelFolder.Text = "Select the folder to use:";
            // 
            // folderBrowser
            // 
            this.folderBrowser.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.folderBrowser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.folderBrowser.Location = new System.Drawing.Point(23, 25);
            this.folderBrowser.Name = "folderBrowser";
            this.folderBrowser.Size = new System.Drawing.Size(434, 297);
            this.folderBrowser.TabIndex = 0;
            // 
            // AddFtpAccountWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 439);
            this.ControlBox = false;
            this.MinimumSize = new System.Drawing.Size(501, 477);
            this.Name = "AddFtpAccountWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageAccount,
            this.wizardPageFolder,
            this.wizardPageSuccess});
            this.Text = "Add FTP Account";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageAccount.ResumeLayout(false);
            this.wizardPageAccount.PerformLayout();
            this.wizardPageSuccess.ResumeLayout(false);
            this.wizardPageSuccess.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).EndInit();
            this.wizardPageFolder.ResumeLayout(false);
            this.wizardPageFolder.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageAccount;
        private UI.Wizard.WizardPage wizardPageSuccess;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private UI.Wizard.WizardPage wizardPageFolder;
        private System.Windows.Forms.TextBox host;
        private System.Windows.Forms.Label labelHost;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelHostExample;
        private FtpFolderBrowserControl folderBrowser;
        private System.Windows.Forms.Label labelFolder;
    }
}