namespace ShipWorks.Shipping.UI.Carriers.Asendia
{
    partial class AsendiaSetupWizard
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (shippingWizardPageFinish != null)
                {
                    shippingWizardPageFinish.Dispose();
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsendiaSetupWizard));
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelAccountNumber = new System.Windows.Forms.Label();
            this.openAccountLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.labelAccountInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.contactInformation = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.wizardPageContactInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(389, 509);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(470, 509);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(308, 509);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageWelcome);
            this.mainPanel.Size = new System.Drawing.Size(557, 437);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 499);
            this.etchBottom.Size = new System.Drawing.Size(561, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(389, 3);
            this.pictureBox.Size = new System.Drawing.Size(165, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(557, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.username);
            this.wizardPageWelcome.Controls.Add(this.password);
            this.wizardPageWelcome.Controls.Add(this.labelPassword);
            this.wizardPageWelcome.Controls.Add(this.labelUsername);
            this.wizardPageWelcome.Controls.Add(this.labelAccountNumber);
            this.wizardPageWelcome.Controls.Add(this.openAccountLink);
            this.wizardPageWelcome.Controls.Add(this.label2);
            this.wizardPageWelcome.Controls.Add(this.accountNumber);
            this.wizardPageWelcome.Controls.Add(this.labelAccountInfo);
            this.wizardPageWelcome.Controls.Add(this.label1);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your Asendia account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(557, 437);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup Asendia Shipping";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(129, 104);
            this.username.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(230, 21);
            this.username.TabIndex = 21;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(129, 135);
            this.password.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(230, 21);
            this.password.TabIndex = 20;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(68, 138);
            this.labelPassword.Margin = new System.Windows.Forms.Padding(3, 0, 10, 0);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 19;
            this.labelPassword.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(66, 107);
            this.labelUsername.Margin = new System.Windows.Forms.Padding(3, 0, 10, 0);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 18;
            this.labelUsername.Text = "Username:";
            // 
            // labelAccountNumber
            // 
            this.labelAccountNumber.AutoSize = true;
            this.labelAccountNumber.Location = new System.Drawing.Point(35, 76);
            this.labelAccountNumber.Margin = new System.Windows.Forms.Padding(3, 0, 10, 0);
            this.labelAccountNumber.Name = "labelAccountNumber";
            this.labelAccountNumber.Size = new System.Drawing.Size(90, 13);
            this.labelAccountNumber.TabIndex = 17;
            this.labelAccountNumber.Text = "Account Number:";
            // 
            // openAccountLink
            // 
            this.openAccountLink.AutoSize = true;
            this.openAccountLink.Location = new System.Drawing.Point(229, 159);
            this.openAccountLink.Name = "openAccountLink";
            this.openAccountLink.Size = new System.Drawing.Size(55, 13);
            this.openAccountLink.TabIndex = 16;
            this.openAccountLink.TabStop = true;
            this.openAccountLink.Text = "click here.";
            this.openAccountLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnOpenAccountLinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(126, 159);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "To open an account, ";
            // 
            // accountNumber
            // 
            this.accountNumber.Location = new System.Drawing.Point(129, 73);
            this.accountNumber.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.Size = new System.Drawing.Size(230, 21);
            this.accountNumber.TabIndex = 13;
            // 
            // labelAccountInfo
            // 
            this.labelAccountInfo.AutoSize = true;
            this.labelAccountInfo.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelAccountInfo.Location = new System.Drawing.Point(20, 50);
            this.labelAccountInfo.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelAccountInfo.Name = "labelAccountInfo";
            this.labelAccountInfo.Size = new System.Drawing.Size(197, 13);
            this.labelAccountInfo.TabIndex = 14;
            this.labelAccountInfo.Text = "Enter your Asendia account information";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(20, 10, 3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(508, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "This wizard will assist you in configuring your Asendia account for use with Ship" +
    "Works. This enables you to begin shipping, tracking, and printing Asendia labels" +
    " with your Asendia account.";
            // 
            // wizardPageContactInfo
            // 
            this.wizardPageContactInfo.Controls.Add(this.contactInformation);
            this.wizardPageContactInfo.Description = "Enter your Asendia contact information";
            this.wizardPageContactInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageContactInfo.Location = new System.Drawing.Point(0, 0);
            this.wizardPageContactInfo.Name = "wizardPageContactInfo";
            this.wizardPageContactInfo.Size = new System.Drawing.Size(557, 437);
            this.wizardPageContactInfo.TabIndex = 0;
            this.wizardPageContactInfo.Title = "Contact Information";
            this.wizardPageContactInfo.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextContactInfo);
            // 
            // contactInformation
            // 
            this.contactInformation.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Website)));
            this.contactInformation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactInformation.Location = new System.Drawing.Point(23, 10);
            this.contactInformation.MaxStreetLines = 1;
            this.contactInformation.Name = "contactInformation";
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.contactInformation.Size = new System.Drawing.Size(355, 381);
            this.contactInformation.TabIndex = 1;
            // 
            // AsendiaSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 544);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AsendiaSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageContactInfo});
            this.Text = "Asendia Setup Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.wizardPageWelcome.PerformLayout();
            this.wizardPageContactInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private ShipWorks.UI.Wizard.WizardPage wizardPageContactInfo;
        private Data.Controls.AutofillPersonControl contactInformation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label labelAccountInfo;
        private System.Windows.Forms.LinkLabel openAccountLink;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelAccountNumber;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
    }
}