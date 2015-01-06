using ShipWorks.Shipping.Carriers.Postal.Stamps;
namespace ShipWorks.Shipping.Carriers.OnTrac
{
    partial class OnTracSetupWizard
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
            this.components = new System.ComponentModel.Container();
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.linkOnTracWebsite = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.wizardPageCredentials = new ShipWorks.UI.Wizard.WizardPage();
            this.accountNumber = new ShipWorks.UI.Controls.NumericTextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelAccountNumber = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPageOptions = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControl = new ShipWorks.Shipping.Carriers.OnTrac.OnTracOptionsControl();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.contactInformation = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.wizardPageCredentials.SuspendLayout();
            this.wizardPageOptions.SuspendLayout();
            this.wizardPageContactInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(389, 509);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(470, 509);
            this.cancel.TabIndex = 2;
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(308, 509);
            this.back.TabIndex = 1;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageContactInfo);
            this.mainPanel.Size = new System.Drawing.Size(557, 437);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 499);
            this.etchBottom.Size = new System.Drawing.Size(561, 2);
            this.etchBottom.TabIndex = 3;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.OnTracLogo48;
            this.pictureBox.Location = new System.Drawing.Point(501, 3);
            this.pictureBox.Size = new System.Drawing.Size(53, 50);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(557, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.linkOnTracWebsite);
            this.wizardPageWelcome.Controls.Add(this.labelInfo2);
            this.wizardPageWelcome.Controls.Add(this.labelInfo1);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your OnTrac account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(557, 437);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup OnTrac Shipping";
            // 
            // linkOnTracWebsite
            // 
            this.linkOnTracWebsite.AutoSize = true;
            this.linkOnTracWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkOnTracWebsite.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkOnTracWebsite.ForeColor = System.Drawing.Color.Blue;
            this.linkOnTracWebsite.Location = new System.Drawing.Point(75, 80);
            this.linkOnTracWebsite.Name = "linkOnTracWebsite";
            this.linkOnTracWebsite.Size = new System.Drawing.Size(93, 13);
            this.linkOnTracWebsite.TabIndex = 5;
            this.linkOnTracWebsite.Text = "www.ontrac.com.";
            this.linkOnTracWebsite.Click += new System.EventHandler(this.OnLinkOnTracWebsite);
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(20, 67);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(410, 30);
            this.labelInfo2.TabIndex = 4;
            this.labelInfo2.Text = "You must have an OnTrac account before continuing. To get an OnTrac account, plea" +
    "se visit";
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(20, 9);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(454, 47);
            this.labelInfo1.TabIndex = 3;
            this.labelInfo1.Text = "This wizard will assist you in configuring your OnTrac account for use with ShipW" +
    "orks. This enables you to begin shipping, tracking, and printing OnTrac labels w" +
    "ith your OnTrac account.";
            // 
            // wizardPageCredentials
            // 
            this.wizardPageCredentials.Controls.Add(this.accountNumber);
            this.wizardPageCredentials.Controls.Add(this.password);
            this.wizardPageCredentials.Controls.Add(this.labelPassword);
            this.wizardPageCredentials.Controls.Add(this.labelAccountNumber);
            this.wizardPageCredentials.Controls.Add(this.label1);
            this.wizardPageCredentials.Description = "Enter your OnTrac account # and API password.";
            this.wizardPageCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageCredentials.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCredentials.Name = "wizardPageCredentials";
            this.wizardPageCredentials.Size = new System.Drawing.Size(557, 437);
            this.wizardPageCredentials.TabIndex = 0;
            this.wizardPageCredentials.Title = "OnTrac Credentials";
            this.wizardPageCredentials.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnNextStepCredentials);
            // 
            // accountNumber
            // 
            this.accountNumber.Location = new System.Drawing.Point(118, 33);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.Size = new System.Drawing.Size(162, 21);
            this.accountNumber.TabIndex = 1;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(118, 59);
            this.password.MaxLength = 50;
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(162, 21);
            this.password.TabIndex = 2;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(35, 62);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(77, 13);
            this.labelPassword.TabIndex = 165;
            this.labelPassword.Text = "API Password:";
            // 
            // labelAccountNumber
            // 
            this.labelAccountNumber.AutoSize = true;
            this.labelAccountNumber.Location = new System.Drawing.Point(51, 36);
            this.labelAccountNumber.Name = "labelAccountNumber";
            this.labelAccountNumber.Size = new System.Drawing.Size(61, 13);
            this.labelAccountNumber.TabIndex = 164;
            this.labelAccountNumber.Text = "Account #:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(324, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the account number and API password provided by OnTrac.";
            // 
            // wizardPageOptions
            // 
            this.wizardPageOptions.Controls.Add(this.optionsControl);
            this.wizardPageOptions.Description = "Configure OnTrac settings.";
            this.wizardPageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOptions.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptions.Name = "wizardPageOptions";
            this.wizardPageOptions.Size = new System.Drawing.Size(557, 437);
            this.wizardPageOptions.TabIndex = 0;
            this.wizardPageOptions.Title = "OnTrac Settings";
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(11, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(438, 75);
            this.optionsControl.TabIndex = 0;
            // 
            // wizardPageContactInfo
            // 
            this.wizardPageContactInfo.Controls.Add(this.contactInformation);
            this.wizardPageContactInfo.Description = "Enter your OnTrac contact information";
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
            this.contactInformation.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.contactInformation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactInformation.Location = new System.Drawing.Point(23, 10);
            this.contactInformation.MaxStreetLines = 1;
            this.contactInformation.Name = "contactInformation";
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal)));
            this.contactInformation.Size = new System.Drawing.Size(355, 381);
            this.contactInformation.TabIndex = 0;
            // 
            // OnTracSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 544);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LastPageCancelable = true;
            this.Name = "OnTracSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageCredentials,
            this.wizardPageContactInfo,
            this.wizardPageOptions});
            this.Text = "OnTrac Setup Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.wizardPageWelcome.PerformLayout();
            this.wizardPageCredentials.ResumeLayout(false);
            this.wizardPageCredentials.PerformLayout();
            this.wizardPageOptions.ResumeLayout(false);
            this.wizardPageContactInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private System.Windows.Forms.Label linkOnTracWebsite;
        private System.Windows.Forms.Label labelInfo2;
        private System.Windows.Forms.Label labelInfo1;
        private ShipWorks.UI.Wizard.WizardPage wizardPageCredentials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelAccountNumber;
        private UI.Wizard.WizardPage wizardPageOptions;
        private OnTracOptionsControl optionsControl;
        private UI.Wizard.WizardPage wizardPageContactInfo;
        private Data.Controls.AutofillPersonControl contactInformation;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.NumericTextBox accountNumber;
    }
}