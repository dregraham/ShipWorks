using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    partial class UspsSetupWizard
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
            this.accountTypePanel = new System.Windows.Forms.Panel();
            this.linkSpecialOffer = new ShipWorks.UI.Controls.LinkControl();
            this.labelSpecialOffer2 = new System.Windows.Forms.Label();
            this.labelSpecialOffer = new System.Windows.Forms.Label();
            this.radioExistingAccount = new System.Windows.Forms.RadioButton();
            this.radioNewAccount = new System.Windows.Forms.RadioButton();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.wizardPageExistingAccountCredentials = new ShipWorks.UI.Wizard.WizardPage();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPageStampsAccount = new ShipWorks.UI.Wizard.WizardPage();
            this.uspsAccountInfo = new ShipWorks.Shipping.Carriers.Postal.Usps.UspsAccountInfoControl();
            this.wizardPageOptions = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControl = new ShipWorks.Shipping.Carriers.Postal.Usps.UspsOptionsControl();
            this.wizardPageAccountAddress = new ShipWorks.UI.Wizard.WizardPage();
            this.panelTerms = new System.Windows.Forms.Panel();
            this.labelTerms2 = new System.Windows.Forms.Label();
            this.linkTerms = new ShipWorks.UI.Controls.LinkControl();
            this.labelTerms1 = new System.Windows.Forms.Label();
            this.termsCheckBox = new System.Windows.Forms.CheckBox();
            this.labelTerms = new System.Windows.Forms.Label();
            this.panelAccountType = new System.Windows.Forms.Panel();
            this.uspsUsageType = new System.Windows.Forms.ComboBox();
            this.labelUsageType = new System.Windows.Forms.Label();
            this.labelAccount = new System.Windows.Forms.Label();
            this.personControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.wizardPageNewAccountCredentials = new ShipWorks.UI.Wizard.WizardPage();
            this.uspsRegistrationSecuritySettingsControl = new ShipWorks.Shipping.Carriers.Postal.Usps.Registration.UspsRegistrationSecuritySettingsControl();
            this.wizardPageNewAccountPayment = new ShipWorks.UI.Wizard.WizardPage();
            this.uspsPaymentControl = new ShipWorks.Shipping.Carriers.Postal.Usps.Registration.UspsPaymentControl();
            this.linkStampsPrivacy = new ShipWorks.UI.Controls.LinkControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.accountTypePanel.SuspendLayout();
            this.wizardPageExistingAccountCredentials.SuspendLayout();
            this.wizardPageStampsAccount.SuspendLayout();
            this.wizardPageOptions.SuspendLayout();
            this.wizardPageAccountAddress.SuspendLayout();
            this.panelTerms.SuspendLayout();
            this.panelAccountType.SuspendLayout();
            this.wizardPageNewAccountCredentials.SuspendLayout();
            this.wizardPageNewAccountPayment.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(565, 612);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(646, 612);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(484, 612);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageNewAccountCredentials);
            this.mainPanel.Size = new System.Drawing.Size(733, 540);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 602);
            this.etchBottom.Size = new System.Drawing.Size(737, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.logo_sdc;
            this.pictureBox.Location = new System.Drawing.Point(580, 3);
            this.pictureBox.Size = new System.Drawing.Size(150, 50);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(733, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.accountTypePanel);
            this.wizardPageWelcome.Controls.Add(this.labelInfo1);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your Stamps.com account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(733, 540);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup Stamps.com Shipping";
            this.wizardPageWelcome.StepNext += OnStepNextWelcome;
            // 
            // accountTypePanel
            // 
            this.accountTypePanel.Controls.Add(this.linkSpecialOffer);
            this.accountTypePanel.Controls.Add(this.labelSpecialOffer2);
            this.accountTypePanel.Controls.Add(this.labelSpecialOffer);
            this.accountTypePanel.Controls.Add(this.radioExistingAccount);
            this.accountTypePanel.Controls.Add(this.radioNewAccount);
            this.accountTypePanel.Location = new System.Drawing.Point(23, 51);
            this.accountTypePanel.Name = "accountTypePanel";
            this.accountTypePanel.Size = new System.Drawing.Size(490, 91);
            this.accountTypePanel.TabIndex = 9;
            this.accountTypePanel.Visible = false;
            // 
            // linkSpecialOffer
            // 
            this.linkSpecialOffer.AutoSize = true;
            this.linkSpecialOffer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkSpecialOffer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkSpecialOffer.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkSpecialOffer.Location = new System.Drawing.Point(63, 35);
            this.linkSpecialOffer.Name = "linkSpecialOffer";
            this.linkSpecialOffer.Size = new System.Drawing.Size(132, 13);
            this.linkSpecialOffer.TabIndex = 10;
            this.linkSpecialOffer.Text = "special Stamps.com offers";
            this.linkSpecialOffer.Click += new System.EventHandler(this.OnLinkUspsSpecialOffer);
            // 
            // labelSpecialOffer2
            // 
            this.labelSpecialOffer2.AutoSize = true;
            this.labelSpecialOffer2.ForeColor = System.Drawing.Color.Gray;
            this.labelSpecialOffer2.Location = new System.Drawing.Point(39, 35);
            this.labelSpecialOffer2.Name = "labelSpecialOffer2";
            this.labelSpecialOffer2.Size = new System.Drawing.Size(29, 13);
            this.labelSpecialOffer2.TabIndex = 10;
            this.labelSpecialOffer2.Text = "(See";
            // 
            // labelSpecialOffer
            // 
            this.labelSpecialOffer.AutoSize = true;
            this.labelSpecialOffer.ForeColor = System.Drawing.Color.Gray;
            this.labelSpecialOffer.Location = new System.Drawing.Point(191, 36);
            this.labelSpecialOffer.Name = "labelSpecialOffer";
            this.labelSpecialOffer.Size = new System.Drawing.Size(130, 13);
            this.labelSpecialOffer.TabIndex = 2;
            this.labelSpecialOffer.Text = "for ShipWorks customers)";
            // 
            // radioExistingAccount
            // 
            this.radioExistingAccount.AutoSize = true;
            this.radioExistingAccount.Location = new System.Drawing.Point(20, 57);
            this.radioExistingAccount.Name = "radioExistingAccount";
            this.radioExistingAccount.Size = new System.Drawing.Size(204, 17);
            this.radioExistingAccount.TabIndex = 1;
            this.radioExistingAccount.Text = "Use an existing Stamps.com account.";
            this.radioExistingAccount.UseVisualStyleBackColor = true;
            // 
            // radioNewAccount
            // 
            this.radioNewAccount.AutoSize = true;
            this.radioNewAccount.Checked = true;
            this.radioNewAccount.Location = new System.Drawing.Point(20, 15);
            this.radioNewAccount.Name = "radioNewAccount";
            this.radioNewAccount.Size = new System.Drawing.Size(309, 17);
            this.radioNewAccount.TabIndex = 0;
            this.radioNewAccount.TabStop = true;
            this.radioNewAccount.Text = "Create a new Stamps.com account for use with ShipWorks.";
            this.radioNewAccount.UseVisualStyleBackColor = true;
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(20, 9);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(454, 47);
            this.labelInfo1.TabIndex = 3;
            this.labelInfo1.Text = "This wizard will assist you in configuring your Stamps.com account for use with S" +
    "hipWorks. This enables you to begin shipping, tracking, and printing USPS labels" +
    " with postage directly from ShipWorks.";
            // 
            // wizardPageExistingAccountCredentials
            // 
            this.wizardPageExistingAccountCredentials.Controls.Add(this.username);
            this.wizardPageExistingAccountCredentials.Controls.Add(this.password);
            this.wizardPageExistingAccountCredentials.Controls.Add(this.labelPassword);
            this.wizardPageExistingAccountCredentials.Controls.Add(this.labelUsername);
            this.wizardPageExistingAccountCredentials.Controls.Add(this.label1);
            this.wizardPageExistingAccountCredentials.Description = "Enter your Stamps.com username and password.";
            this.wizardPageExistingAccountCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageExistingAccountCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageExistingAccountCredentials.Location = new System.Drawing.Point(0, 0);
            this.wizardPageExistingAccountCredentials.Name = "wizardPageExistingAccountCredentials";
            this.wizardPageExistingAccountCredentials.Size = new System.Drawing.Size(542, 540);
            this.wizardPageExistingAccountCredentials.TabIndex = 0;
            this.wizardPageExistingAccountCredentials.Title = "Stamps.com Credentials";
            this.wizardPageExistingAccountCredentials.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextExistingCredentials);
            this.wizardPageExistingAccountCredentials.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoExistingCredentials);
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(105, 33);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(162, 21);
            this.username.TabIndex = 0;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(105, 60);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(162, 21);
            this.password.TabIndex = 1;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(42, 63);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 165;
            this.labelPassword.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(40, 36);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 164;
            this.labelUsername.Text = "Username:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the username and password provided by Stamps.com.";
            // 
            // wizardPageStampsAccount
            // 
            this.wizardPageStampsAccount.Controls.Add(this.uspsAccountInfo);
            this.wizardPageStampsAccount.Description = "Your Stamps.com account information.";
            this.wizardPageStampsAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageStampsAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageStampsAccount.Location = new System.Drawing.Point(0, 0);
            this.wizardPageStampsAccount.Name = "wizardPageStampsAccount";
            this.wizardPageStampsAccount.Size = new System.Drawing.Size(542, 540);
            this.wizardPageStampsAccount.TabIndex = 0;
            this.wizardPageStampsAccount.Title = "Stamps.com Account";
            this.wizardPageStampsAccount.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAccountInfo);
            // 
            // uspsAccountInfo
            // 
            this.uspsAccountInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uspsAccountInfo.Location = new System.Drawing.Point(23, 10);
            this.uspsAccountInfo.Name = "uspsAccountInfo";
            this.uspsAccountInfo.Size = new System.Drawing.Size(487, 143);
            this.uspsAccountInfo.TabIndex = 0;
            // 
            // wizardPageOptions
            // 
            this.wizardPageOptions.Controls.Add(this.optionsControl);
            this.wizardPageOptions.Description = "Configure Stamps.com settings.";
            this.wizardPageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOptions.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptions.Name = "wizardPageOptions";
            this.wizardPageOptions.Size = new System.Drawing.Size(542, 540);
            this.wizardPageOptions.TabIndex = 0;
            this.wizardPageOptions.Title = "Stamps.com Settings";
            this.wizardPageOptions.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoOptions);
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(16, 5);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.ShipmentTypeCode = ShipWorks.Shipping.ShipmentTypeCode.Usps;
            this.optionsControl.Size = new System.Drawing.Size(435, 112);
            this.optionsControl.TabIndex = 0;
            // 
            // wizardPageAccountAddress
            // 
            this.wizardPageAccountAddress.Controls.Add(this.panelTerms);
            this.wizardPageAccountAddress.Controls.Add(this.panelAccountType);
            this.wizardPageAccountAddress.Controls.Add(this.personControl);
            this.wizardPageAccountAddress.Description = "Enter the address for your Stamps.com account.";
            this.wizardPageAccountAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccountAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAccountAddress.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccountAddress.Name = "wizardPageAccountAddress";
            this.wizardPageAccountAddress.Size = new System.Drawing.Size(542, 540);
            this.wizardPageAccountAddress.TabIndex = 0;
            this.wizardPageAccountAddress.Title = "Account Registration";
            this.wizardPageAccountAddress.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccountAddress);
            this.wizardPageAccountAddress.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAccountAddress);
            // 
            // panelTerms
            // 
            this.panelTerms.Controls.Add(this.labelTerms2);
            this.panelTerms.Controls.Add(this.linkTerms);
            this.panelTerms.Controls.Add(this.labelTerms1);
            this.panelTerms.Controls.Add(this.termsCheckBox);
            this.panelTerms.Controls.Add(this.labelTerms);
            this.panelTerms.Location = new System.Drawing.Point(23, 409);
            this.panelTerms.Name = "panelTerms";
            this.panelTerms.Size = new System.Drawing.Size(494, 59);
            this.panelTerms.TabIndex = 2;
            // 
            // labelTerms2
            // 
            this.labelTerms2.AutoSize = true;
            this.labelTerms2.Location = new System.Drawing.Point(48, 39);
            this.labelTerms2.Name = "labelTerms2";
            this.labelTerms2.Size = new System.Drawing.Size(195, 13);
            this.labelTerms2.TabIndex = 8;
            this.labelTerms2.Text = "have provided is accurate and truthful.";
            // 
            // linkTerms
            // 
            this.linkTerms.AutoSize = true;
            this.linkTerms.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkTerms.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkTerms.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkTerms.Location = new System.Drawing.Point(181, 22);
            this.linkTerms.Name = "linkTerms";
            this.linkTerms.Size = new System.Drawing.Size(110, 13);
            this.linkTerms.TabIndex = 7;
            this.linkTerms.Text = "Terms and Conditions";
            this.linkTerms.Click += new System.EventHandler(this.OnLinkUspsTermsConditions);
            // 
            // labelTerms1
            // 
            this.labelTerms1.AutoSize = true;
            this.labelTerms1.Location = new System.Drawing.Point(290, 22);
            this.labelTerms1.Name = "labelTerms1";
            this.labelTerms1.Size = new System.Drawing.Size(176, 13);
            this.labelTerms1.TabIndex = 3;
            this.labelTerms1.Text = "and I confirm that the information I";
            // 
            // termsCheckBox
            // 
            this.termsCheckBox.AutoSize = true;
            this.termsCheckBox.Location = new System.Drawing.Point(32, 21);
            this.termsCheckBox.Name = "termsCheckBox";
            this.termsCheckBox.Size = new System.Drawing.Size(153, 17);
            this.termsCheckBox.TabIndex = 2;
            this.termsCheckBox.Text = "I\'ve read and agree to the";
            this.termsCheckBox.UseVisualStyleBackColor = true;
            // 
            // labelTerms
            // 
            this.labelTerms.AutoSize = true;
            this.labelTerms.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTerms.Location = new System.Drawing.Point(3, 2);
            this.labelTerms.Name = "labelTerms";
            this.labelTerms.Size = new System.Drawing.Size(129, 13);
            this.labelTerms.TabIndex = 0;
            this.labelTerms.Text = "Terms and Conditions";
            this.labelTerms.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelAccountType
            // 
            this.panelAccountType.Controls.Add(this.uspsUsageType);
            this.panelAccountType.Controls.Add(this.labelUsageType);
            this.panelAccountType.Controls.Add(this.labelAccount);
            this.panelAccountType.Location = new System.Drawing.Point(23, 8);
            this.panelAccountType.Name = "panelAccountType";
            this.panelAccountType.Size = new System.Drawing.Size(345, 46);
            this.panelAccountType.TabIndex = 0;
            // 
            // uspsUsageType
            // 
            this.uspsUsageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uspsUsageType.FormattingEnabled = true;
            this.uspsUsageType.Location = new System.Drawing.Point(100, 19);
            this.uspsUsageType.Name = "uspsUsageType";
            this.uspsUsageType.Size = new System.Drawing.Size(143, 21);
            this.uspsUsageType.TabIndex = 4;
            // 
            // labelUsageType
            // 
            this.labelUsageType.AutoSize = true;
            this.labelUsageType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsageType.Location = new System.Drawing.Point(14, 22);
            this.labelUsageType.Name = "labelUsageType";
            this.labelUsageType.Size = new System.Drawing.Size(80, 13);
            this.labelUsageType.TabIndex = 3;
            this.labelUsageType.Text = "Primary Usage:";
            this.labelUsageType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccount.Location = new System.Drawing.Point(3, 2);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(53, 13);
            this.labelAccount.TabIndex = 0;
            this.labelAccount.Text = "Account";
            this.labelAccount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.Location = new System.Drawing.Point(23, 57);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(355, 352);
            this.personControl.TabIndex = 1;
            this.personControl.Resize += new System.EventHandler(this.OnPersonControlResize);
            // 
            // wizardPageNewAccountCredentials
            // 
            this.wizardPageNewAccountCredentials.Controls.Add(this.uspsRegistrationSecuritySettingsControl);
            this.wizardPageNewAccountCredentials.Description = "Create a new Stamps.com account for use with ShipWorks.";
            this.wizardPageNewAccountCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageNewAccountCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageNewAccountCredentials.Location = new System.Drawing.Point(0, 0);
            this.wizardPageNewAccountCredentials.Name = "wizardPageNewAccountCredentials";
            this.wizardPageNewAccountCredentials.Size = new System.Drawing.Size(733, 540);
            this.wizardPageNewAccountCredentials.TabIndex = 0;
            this.wizardPageNewAccountCredentials.Title = "Account Registration";
            this.wizardPageNewAccountCredentials.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextRegistrationCredentials);
            // 
            // uspsRegistrationSecuritySettingsControl
            // 
            this.uspsRegistrationSecuritySettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uspsRegistrationSecuritySettingsControl.Location = new System.Drawing.Point(20, 7);
            this.uspsRegistrationSecuritySettingsControl.Name = "uspsRegistrationSecuritySettingsControl";
            this.uspsRegistrationSecuritySettingsControl.Size = new System.Drawing.Size(458, 395);
            this.uspsRegistrationSecuritySettingsControl.TabIndex = 0;
            // 
            // wizardPageNewAccountPayment
            // 
            this.wizardPageNewAccountPayment.Controls.Add(this.uspsPaymentControl);
            this.wizardPageNewAccountPayment.Description = "Create a new Stamps.com account for use with ShipWorks.";
            this.wizardPageNewAccountPayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageNewAccountPayment.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageNewAccountPayment.Location = new System.Drawing.Point(0, 0);
            this.wizardPageNewAccountPayment.Name = "wizardPageNewAccountPayment";
            this.wizardPageNewAccountPayment.Size = new System.Drawing.Size(542, 540);
            this.wizardPageNewAccountPayment.TabIndex = 0;
            this.wizardPageNewAccountPayment.Title = "Account Registration";
            this.wizardPageNewAccountPayment.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextNewAccountPayment);
            // 
            // uspsPaymentControl
            // 
            this.uspsPaymentControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uspsPaymentControl.Location = new System.Drawing.Point(21, 11);
            this.uspsPaymentControl.Name = "uspsPaymentControl";
            this.uspsPaymentControl.Size = new System.Drawing.Size(461, 310);
            this.uspsPaymentControl.TabIndex = 0;
            // 
            // linkStampsPrivacy
            // 
            this.linkStampsPrivacy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkStampsPrivacy.AutoSize = true;
            this.linkStampsPrivacy.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkStampsPrivacy.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkStampsPrivacy.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkStampsPrivacy.Location = new System.Drawing.Point(20, 617);
            this.linkStampsPrivacy.Name = "linkStampsPrivacy";
            this.linkStampsPrivacy.Size = new System.Drawing.Size(133, 13);
            this.linkStampsPrivacy.TabIndex = 6;
            this.linkStampsPrivacy.Text = "Stamps.com Privacy Policy";
            this.linkStampsPrivacy.Click += new System.EventHandler(this.OnLinkUspsPrivacyPolicy);
            // 
            // UspsSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 647);
            this.Controls.Add(this.linkStampsPrivacy);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LastPageCancelable = true;
            this.Name = "UspsSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageAccountAddress,
            this.wizardPageNewAccountCredentials,
            this.wizardPageNewAccountPayment,
            this.wizardPageExistingAccountCredentials,
            this.wizardPageOptions,
            this.wizardPageStampsAccount});
            this.Text = "Stamps.com Setup Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Controls.SetChildIndex(this.linkStampsPrivacy, 0);
            this.Controls.SetChildIndex(this.next, 0);
            this.Controls.SetChildIndex(this.cancel, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.back, 0);
            this.Controls.SetChildIndex(this.mainPanel, 0);
            this.Controls.SetChildIndex(this.etchBottom, 0);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.accountTypePanel.ResumeLayout(false);
            this.accountTypePanel.PerformLayout();
            this.wizardPageExistingAccountCredentials.ResumeLayout(false);
            this.wizardPageExistingAccountCredentials.PerformLayout();
            this.wizardPageStampsAccount.ResumeLayout(false);
            this.wizardPageOptions.ResumeLayout(false);
            this.wizardPageAccountAddress.ResumeLayout(false);
            this.panelTerms.ResumeLayout(false);
            this.panelTerms.PerformLayout();
            this.panelAccountType.ResumeLayout(false);
            this.panelAccountType.PerformLayout();
            this.wizardPageNewAccountCredentials.ResumeLayout(false);
            this.wizardPageNewAccountPayment.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private System.Windows.Forms.Label labelInfo1;
        private ShipWorks.UI.Wizard.WizardPage wizardPageExistingAccountCredentials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelUsername;
        private ShipWorks.UI.Wizard.WizardPage wizardPageStampsAccount;
        private UspsAccountInfoControl uspsAccountInfo;
        private UI.Wizard.WizardPage wizardPageOptions;
        private UspsOptionsControl optionsControl;
        private System.Windows.Forms.Panel accountTypePanel;
        private System.Windows.Forms.RadioButton radioExistingAccount;
        private System.Windows.Forms.RadioButton radioNewAccount;
        private UI.Wizard.WizardPage wizardPageAccountAddress;
        private UI.Wizard.WizardPage wizardPageNewAccountCredentials;
        private UspsRegistrationSecuritySettingsControl uspsRegistrationSecuritySettingsControl;
        private UI.Wizard.WizardPage wizardPageNewAccountPayment;
        private UspsPaymentControl uspsPaymentControl;
        private System.Windows.Forms.ComboBox uspsUsageType;
        private System.Windows.Forms.Label labelAccount;
        private UI.Controls.LinkControl linkSpecialOffer;
        private System.Windows.Forms.Label labelSpecialOffer2;
        private System.Windows.Forms.Label labelSpecialOffer;
        private System.Windows.Forms.Label labelUsageType;
        private System.Windows.Forms.Panel panelAccountType;
        private UI.Controls.LinkControl linkStampsPrivacy;
        private System.Windows.Forms.Panel panelTerms;
        private System.Windows.Forms.Label labelTerms2;
        private UI.Controls.LinkControl linkTerms;
        private System.Windows.Forms.Label labelTerms1;
        private System.Windows.Forms.CheckBox termsCheckBox;
        private System.Windows.Forms.Label labelTerms;
        private ShipWorks.Data.Controls.AutofillPersonControl personControl;
    }
}
