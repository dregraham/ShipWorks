namespace ShipWorks.Shipping.Carriers.iParcel
{
    partial class iParcelSetupWizard
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
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.linkiParcelRegistration = new ShipWorks.UI.Controls.LinkControl();
            this.labelWelcomeExistingAccountInfo = new System.Windows.Forms.Label();
            this.labelWelcomeIntro = new System.Windows.Forms.Label();
            this.wizardPageCredentials = new ShipWorks.UI.Wizard.WizardPage();
            this.password = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelCredentialsInstructions = new System.Windows.Forms.Label();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.contactInformation = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.wizardPageOptions = new ShipWorks.UI.Wizard.WizardPage();
            this.iParcelOptionsControl = new ShipWorks.Shipping.Carriers.iParcel.iParcelOptionsControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.wizardPageCredentials.SuspendLayout();
            this.wizardPageContactInfo.SuspendLayout();
            this.wizardPageOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(422, 496);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(503, 496);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(341, 496);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageContactInfo);
            this.mainPanel.Size = new System.Drawing.Size(590, 424);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 486);
            this.etchBottom.Size = new System.Drawing.Size(594, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.iparcelLogo;
            this.pictureBox.Location = new System.Drawing.Point(503, 3);
            this.pictureBox.Size = new System.Drawing.Size(84, 50);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(590, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.linkiParcelRegistration);
            this.wizardPageWelcome.Controls.Add(this.labelWelcomeExistingAccountInfo);
            this.wizardPageWelcome.Controls.Add(this.labelWelcomeIntro);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your i-parcel account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(590, 424);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup i-parcel Shipping";
            // 
            // linkiParcelRegistration
            // 
            this.linkiParcelRegistration.AutoSize = true;
            this.linkiParcelRegistration.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkiParcelRegistration.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkiParcelRegistration.ForeColor = System.Drawing.Color.Blue;
            this.linkiParcelRegistration.Location = new System.Drawing.Point(127, 79);
            this.linkiParcelRegistration.Name = "linkiParcelRegistration";
            this.linkiParcelRegistration.Size = new System.Drawing.Size(160, 13);
            this.linkiParcelRegistration.TabIndex = 2;
            this.linkiParcelRegistration.Text = "https://www.i-parcel.com/en/contact-us";
            this.linkiParcelRegistration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkiParcelRegistration.Click += new System.EventHandler(this.OnClickRegistrationLink);
            // 
            // labelWelcomeExistingAccountInfo
            // 
            this.labelWelcomeExistingAccountInfo.Location = new System.Drawing.Point(26, 66);
            this.labelWelcomeExistingAccountInfo.Name = "labelWelcomeExistingAccountInfo";
            this.labelWelcomeExistingAccountInfo.Size = new System.Drawing.Size(364, 34);
            this.labelWelcomeExistingAccountInfo.TabIndex = 1;
            this.labelWelcomeExistingAccountInfo.Text = "You must have an i-parcel account before continuing. To get an i-parcel account, " +
    "please visit ";
            // 
            // labelWelcomeIntro
            // 
            this.labelWelcomeIntro.Location = new System.Drawing.Point(27, 11);
            this.labelWelcomeIntro.Name = "labelWelcomeIntro";
            this.labelWelcomeIntro.Size = new System.Drawing.Size(445, 55);
            this.labelWelcomeIntro.TabIndex = 0;
            this.labelWelcomeIntro.Text = "This wizard will assist you in configuring your i-parcel account for use with Shi" +
    "pWorks. This enables you to begin shipping, tracking, and printing i-parcel labe" +
    "ls with your i-parcel account.";
            // 
            // wizardPageCredentials
            // 
            this.wizardPageCredentials.Controls.Add(this.password);
            this.wizardPageCredentials.Controls.Add(this.username);
            this.wizardPageCredentials.Controls.Add(this.labelPassword);
            this.wizardPageCredentials.Controls.Add(this.labelUsername);
            this.wizardPageCredentials.Controls.Add(this.labelCredentialsInstructions);
            this.wizardPageCredentials.Description = "Enter your i-parcel username and password.";
            this.wizardPageCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageCredentials.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCredentials.Name = "wizardPageCredentials";
            this.wizardPageCredentials.Size = new System.Drawing.Size(590, 424);
            this.wizardPageCredentials.TabIndex = 0;
            this.wizardPageCredentials.Title = "i-parcel Credentials";
            this.wizardPageCredentials.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCredentials);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(108, 65);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(171, 21);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(108, 39);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(171, 21);
            this.username.TabIndex = 2;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(45, 68);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(45, 42);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 1;
            this.labelUsername.Text = "Username:";
            this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCredentialsInstructions
            // 
            this.labelCredentialsInstructions.AutoSize = true;
            this.labelCredentialsInstructions.Location = new System.Drawing.Point(27, 11);
            this.labelCredentialsInstructions.Name = "labelCredentialsInstructions";
            this.labelCredentialsInstructions.Size = new System.Drawing.Size(357, 13);
            this.labelCredentialsInstructions.TabIndex = 0;
            this.labelCredentialsInstructions.Text = "Enter the username and password used to log into your i-parcel account.";
            // 
            // wizardPageContactInfo
            // 
            this.wizardPageContactInfo.Controls.Add(this.contactInformation);
            this.wizardPageContactInfo.Description = "Enter your i-parcel contact information.";
            this.wizardPageContactInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageContactInfo.Location = new System.Drawing.Point(0, 0);
            this.wizardPageContactInfo.Name = "wizardPageContactInfo";
            this.wizardPageContactInfo.Size = new System.Drawing.Size(590, 424);
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
            this.contactInformation.Location = new System.Drawing.Point(26, 10);
            this.contactInformation.MaxStreetLines = 2;
            this.contactInformation.Name = "contactInformation";
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Website)));
            this.contactInformation.Size = new System.Drawing.Size(355, 369);
            this.contactInformation.TabIndex = 0;
            // 
            // wizardPageOptions
            // 
            this.wizardPageOptions.Controls.Add(this.iParcelOptionsControl);
            this.wizardPageOptions.Description = "Configure i-parcel settings.";
            this.wizardPageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOptions.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptions.Name = "wizardPageOptions";
            this.wizardPageOptions.Size = new System.Drawing.Size(590, 424);
            this.wizardPageOptions.TabIndex = 0;
            this.wizardPageOptions.Title = "i-parcel Settings";
            // 
            // iParcelOptionsControl
            // 
            this.iParcelOptionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iParcelOptionsControl.Location = new System.Drawing.Point(26, 10);
            this.iParcelOptionsControl.Name = "iParcelOptionsControl";
            this.iParcelOptionsControl.Size = new System.Drawing.Size(439, 87);
            this.iParcelOptionsControl.TabIndex = 0;
            // 
            // iParcelSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 531);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LastPageCancelable = true;
            this.Name = "iParcelSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageCredentials,
            this.wizardPageContactInfo,
            this.wizardPageOptions});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "i-parcel Setup Wizard";
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
            this.wizardPageContactInfo.ResumeLayout(false);
            this.wizardPageOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageWelcome;
        private System.Windows.Forms.Label labelWelcomeExistingAccountInfo;
        private System.Windows.Forms.Label labelWelcomeIntro;
        private UI.Controls.LinkControl linkiParcelRegistration;
        private UI.Wizard.WizardPage wizardPageCredentials;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelCredentialsInstructions;
        private UI.Wizard.WizardPage wizardPageContactInfo;
        private Data.Controls.AutofillPersonControl contactInformation;
        private UI.Wizard.WizardPage wizardPageOptions;
        private iParcelOptionsControl iParcelOptionsControl;
    }
}