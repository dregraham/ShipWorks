using System.Security.AccessControl;

namespace ShipWorks.Shipping.Carriers.Postal.Express1.Registration
{
    partial class Express1SetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Express1SetupWizard));
            this.wizardPageAccountType = new ShipWorks.UI.Wizard.WizardPage();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.radioNewAccount = new System.Windows.Forms.RadioButton();
            this.radioExistingAccount = new System.Windows.Forms.RadioButton();
            this.wizardPageAddress = new ShipWorks.UI.Wizard.WizardPage();
            this.personControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.wizardPageAgreement = new ShipWorks.UI.Wizard.WizardPage();
            this.termsCheckBox = new System.Windows.Forms.CheckBox();
            this.licenseAgreement = new System.Windows.Forms.RichTextBox();
            this.wizardPagePayment = new ShipWorks.UI.Wizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.creditCardDetailsPanel = new System.Windows.Forms.Panel();
            this.labelCardType = new System.Windows.Forms.Label();
            this.cardExpireYear = new System.Windows.Forms.ComboBox();
            this.cardType = new System.Windows.Forms.ComboBox();
            this.cardExpireMonth = new System.Windows.Forms.ComboBox();
            this.labelCardNumber = new System.Windows.Forms.Label();
            this.labelCardExpiration = new System.Windows.Forms.Label();
            this.cardNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.personCreditCard = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.wizardPageFinish = new ShipWorks.UI.Wizard.WizardPage();
            this.panelAccountCredentials = new System.Windows.Forms.Panel();
            this.accountDetailsTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panelAccountNumber = new System.Windows.Forms.Panel();
            this.buyPostage = new System.Windows.Forms.Button();
            this.labelBuyPostage = new System.Windows.Forms.Label();
            this.pictureComplete = new System.Windows.Forms.PictureBox();
            this.labelFinish = new System.Windows.Forms.Label();
            this.wizardPageOptions = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControlPanel = new System.Windows.Forms.Panel();
            this.wizardPageExisting = new ShipWorks.UI.Wizard.WizardPage();
            this.label6 = new System.Windows.Forms.Label();
            this.passwordExisting = new System.Windows.Forms.TextBox();
            this.labelPasswordExisting = new System.Windows.Forms.Label();
            this.accountExisting = new System.Windows.Forms.TextBox();
            this.labelAccountExisting = new System.Windows.Forms.Label();
            this.wizardPageAccountList = new ShipWorks.UI.Wizard.WizardPage();
            this.accountControlPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageAccountType.SuspendLayout();
            this.wizardPageAddress.SuspendLayout();
            this.wizardPageAgreement.SuspendLayout();
            this.wizardPagePayment.SuspendLayout();
            this.creditCardDetailsPanel.SuspendLayout();
            this.wizardPageFinish.SuspendLayout();
            this.panelAccountCredentials.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelAccountNumber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureComplete)).BeginInit();
            this.wizardPageOptions.SuspendLayout();
            this.wizardPageExisting.SuspendLayout();
            this.wizardPageAccountList.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(451, 532);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(532, 532);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(370, 532);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageAddress);
            this.mainPanel.Size = new System.Drawing.Size(625, 460);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 522);
            this.etchBottom.Size = new System.Drawing.Size(623, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(514, 3);
            this.pictureBox.Size = new System.Drawing.Size(102, 50);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(619, 56);
            // 
            // wizardPageAccountType
            // 
            this.wizardPageAccountType.Controls.Add(this.labelInfo1);
            this.wizardPageAccountType.Controls.Add(this.radioNewAccount);
            this.wizardPageAccountType.Controls.Add(this.radioExistingAccount);
            this.wizardPageAccountType.Description = "Setup ShipWorks to work with an Express1 acccount.";
            this.wizardPageAccountType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAccountType.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccountType.Name = "wizardPageAccountType";
            this.wizardPageAccountType.Size = new System.Drawing.Size(625, 460);
            this.wizardPageAccountType.TabIndex = 0;
            this.wizardPageAccountType.Title = "Setup Express1 Shipping";
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(20, 8);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(483, 47);
            this.labelInfo1.TabIndex = 5;
            this.labelInfo1.Text = "This wizard will assist you in creating an Express1 account for use with ShipWork" +
    "s. This enables you to begin shipping, tracking, and printing USPS labels with p" +
    "ostage directly from ShipWorks.";
            // 
            // radioNewAccount
            // 
            this.radioNewAccount.AutoSize = true;
            this.radioNewAccount.Checked = true;
            this.radioNewAccount.Location = new System.Drawing.Point(58, 58);
            this.radioNewAccount.Name = "radioNewAccount";
            this.radioNewAccount.Size = new System.Drawing.Size(295, 17);
            this.radioNewAccount.TabIndex = 2;
            this.radioNewAccount.TabStop = true;
            this.radioNewAccount.Text = "Create a new Express1 account for use with ShipWorks.";
            this.radioNewAccount.UseVisualStyleBackColor = true;
            // 
            // radioExistingAccount
            // 
            this.radioExistingAccount.AutoSize = true;
            this.radioExistingAccount.Location = new System.Drawing.Point(58, 85);
            this.radioExistingAccount.Name = "radioExistingAccount";
            this.radioExistingAccount.Size = new System.Drawing.Size(190, 17);
            this.radioExistingAccount.TabIndex = 3;
            this.radioExistingAccount.Text = "Use an existing Express1 account.";
            this.radioExistingAccount.UseVisualStyleBackColor = true;
            // 
            // wizardPageAddress
            // 
            this.wizardPageAddress.Controls.Add(this.personControl);
            this.wizardPageAddress.Description = "Enter the address for your Express1 account.";
            this.wizardPageAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAddress.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAddress.Name = "wizardPageAddress";
            this.wizardPageAddress.Size = new System.Drawing.Size(625, 460);
            this.wizardPageAddress.TabIndex = 0;
            this.wizardPageAddress.Title = "Account Registration";
            this.wizardPageAddress.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAddress);
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Fax)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.Location = new System.Drawing.Point(23, 3);
            this.personControl.Name = "personControl";
            this.personControl.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.personControl.Size = new System.Drawing.Size(358, 392);
            this.personControl.TabIndex = 1;
            // 
            // wizardPageAgreement
            // 
            this.wizardPageAgreement.Controls.Add(this.termsCheckBox);
            this.wizardPageAgreement.Controls.Add(this.licenseAgreement);
            this.wizardPageAgreement.Description = "Express1 Terms and Conditions";
            this.wizardPageAgreement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAgreement.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAgreement.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAgreement.Name = "wizardPageAgreement";
            this.wizardPageAgreement.Size = new System.Drawing.Size(625, 460);
            this.wizardPageAgreement.TabIndex = 0;
            this.wizardPageAgreement.Title = "Express1 Agreement";
            this.wizardPageAgreement.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAgreement);
            // 
            // termsCheckBox
            // 
            this.termsCheckBox.AutoSize = true;
            this.termsCheckBox.Location = new System.Drawing.Point(12, 434);
            this.termsCheckBox.Name = "termsCheckBox";
            this.termsCheckBox.Size = new System.Drawing.Size(199, 17);
            this.termsCheckBox.TabIndex = 1;
            this.termsCheckBox.Text = "I agree to the terms and conditions.";
            this.termsCheckBox.UseVisualStyleBackColor = true;
            // 
            // licenseAgreement
            // 
            this.licenseAgreement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.licenseAgreement.BackColor = System.Drawing.SystemColors.Control;
            this.licenseAgreement.Location = new System.Drawing.Point(12, 8);
            this.licenseAgreement.Name = "licenseAgreement";
            this.licenseAgreement.ReadOnly = true;
            this.licenseAgreement.Size = new System.Drawing.Size(521, 420);
            this.licenseAgreement.TabIndex = 6;
            this.licenseAgreement.Text = resources.GetString("licenseAgreement.Text");
            // 
            // wizardPagePayment
            // 
            this.wizardPagePayment.Controls.Add(this.label1);
            this.wizardPagePayment.Controls.Add(this.creditCardDetailsPanel);
            this.wizardPagePayment.Controls.Add(this.label3);
            this.wizardPagePayment.Controls.Add(this.personCreditCard);
            this.wizardPagePayment.Description = "Enter your payment information for Postage.";
            this.wizardPagePayment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePayment.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePayment.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePayment.Name = "wizardPagePayment";
            this.wizardPagePayment.Size = new System.Drawing.Size(625, 460);
            this.wizardPagePayment.TabIndex = 0;
            this.wizardPagePayment.Title = "Account Registration";
            this.wizardPagePayment.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextPayment);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(487, 44);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // creditCardDetailsPanel
            // 
            this.creditCardDetailsPanel.Controls.Add(this.labelCardType);
            this.creditCardDetailsPanel.Controls.Add(this.cardExpireYear);
            this.creditCardDetailsPanel.Controls.Add(this.cardType);
            this.creditCardDetailsPanel.Controls.Add(this.cardExpireMonth);
            this.creditCardDetailsPanel.Controls.Add(this.labelCardNumber);
            this.creditCardDetailsPanel.Controls.Add(this.labelCardExpiration);
            this.creditCardDetailsPanel.Controls.Add(this.cardNumber);
            this.creditCardDetailsPanel.Location = new System.Drawing.Point(54, 236);
            this.creditCardDetailsPanel.Name = "creditCardDetailsPanel";
            this.creditCardDetailsPanel.Size = new System.Drawing.Size(363, 100);
            this.creditCardDetailsPanel.TabIndex = 17;
            // 
            // labelCardType
            // 
            this.labelCardType.AutoSize = true;
            this.labelCardType.Location = new System.Drawing.Point(14, 8);
            this.labelCardType.Name = "labelCardType";
            this.labelCardType.Size = new System.Drawing.Size(59, 13);
            this.labelCardType.TabIndex = 10;
            this.labelCardType.Text = "Card type:";
            // 
            // cardExpireYear
            // 
            this.cardExpireYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cardExpireYear.FormattingEnabled = true;
            this.cardExpireYear.Items.AddRange(new object[] {
            "2009",
            "2010",
            "2011",
            "2012",
            "2013",
            "2014",
            "2015",
            "2016",
            "2017",
            "2018",
            "2019",
            "2020"});
            this.cardExpireYear.Location = new System.Drawing.Point(182, 60);
            this.cardExpireYear.Name = "cardExpireYear";
            this.cardExpireYear.Size = new System.Drawing.Size(85, 21);
            this.cardExpireYear.TabIndex = 16;
            // 
            // cardType
            // 
            this.cardType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cardType.FormattingEnabled = true;
            this.cardType.Location = new System.Drawing.Point(79, 5);
            this.cardType.Name = "cardType";
            this.cardType.Size = new System.Drawing.Size(144, 21);
            this.cardType.TabIndex = 11;
            // 
            // cardExpireMonth
            // 
            this.cardExpireMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cardExpireMonth.FormattingEnabled = true;
            this.cardExpireMonth.Items.AddRange(new object[] {
            "01 - January",
            "02 - February",
            "03 - March",
            "04 - April",
            "05 - May",
            "06 - June",
            "07 - July",
            "08 - August",
            "09 - September",
            "10 - October",
            "11 - November",
            "12 - December"});
            this.cardExpireMonth.Location = new System.Drawing.Point(79, 60);
            this.cardExpireMonth.Name = "cardExpireMonth";
            this.cardExpireMonth.Size = new System.Drawing.Size(97, 21);
            this.cardExpireMonth.TabIndex = 15;
            // 
            // labelCardNumber
            // 
            this.labelCardNumber.AutoSize = true;
            this.labelCardNumber.Location = new System.Drawing.Point(0, 36);
            this.labelCardNumber.Name = "labelCardNumber";
            this.labelCardNumber.Size = new System.Drawing.Size(73, 13);
            this.labelCardNumber.TabIndex = 12;
            this.labelCardNumber.Text = "Card number:";
            // 
            // labelCardExpiration
            // 
            this.labelCardExpiration.AutoSize = true;
            this.labelCardExpiration.Location = new System.Drawing.Point(14, 63);
            this.labelCardExpiration.Name = "labelCardExpiration";
            this.labelCardExpiration.Size = new System.Drawing.Size(59, 13);
            this.labelCardExpiration.TabIndex = 14;
            this.labelCardExpiration.Text = "Expiration:";
            // 
            // cardNumber
            // 
            this.cardNumber.Location = new System.Drawing.Point(79, 33);
            this.cardNumber.Name = "cardNumber";
            this.cardNumber.Size = new System.Drawing.Size(206, 21);
            this.cardNumber.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 4;
            // 
            // personCreditCard
            // 
            this.personCreditCard.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((ShipWorks.Data.Controls.PersonFields.Street | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal)));
            this.personCreditCard.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personCreditCard.Location = new System.Drawing.Point(57, 59);
            this.personCreditCard.Name = "personCreditCard";
            this.personCreditCard.Size = new System.Drawing.Size(358, 183);
            this.personCreditCard.TabIndex = 3;
            this.personCreditCard.Resize += new System.EventHandler(this.OnPersonCreditCardResize);
            // 
            // wizardPageFinish
            // 
            this.wizardPageFinish.Controls.Add(this.panelAccountCredentials);
            this.wizardPageFinish.Controls.Add(this.panelAccountNumber);
            this.wizardPageFinish.Controls.Add(this.pictureComplete);
            this.wizardPageFinish.Controls.Add(this.labelFinish);
            this.wizardPageFinish.Description = "Registration Complete!";
            this.wizardPageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinish.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinish.Name = "wizardPageFinish";
            this.wizardPageFinish.Size = new System.Drawing.Size(545, 460);
            this.wizardPageFinish.TabIndex = 0;
            this.wizardPageFinish.Title = "Express1 Account";
            this.wizardPageFinish.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoFinish);
            // 
            // panelAccountCredentials
            // 
            this.panelAccountCredentials.Controls.Add(this.accountDetailsTextBox);
            this.panelAccountCredentials.Controls.Add(this.pictureBox1);
            this.panelAccountCredentials.Controls.Add(this.label4);
            this.panelAccountCredentials.Location = new System.Drawing.Point(22, 97);
            this.panelAccountCredentials.Name = "panelAccountCredentials";
            this.panelAccountCredentials.Size = new System.Drawing.Size(496, 97);
            this.panelAccountCredentials.TabIndex = 23;
            // 
            // accountDetailsTextBox
            // 
            this.accountDetailsTextBox.Location = new System.Drawing.Point(44, 28);
            this.accountDetailsTextBox.Multiline = true;
            this.accountDetailsTextBox.Name = "accountDetailsTextBox";
            this.accountDetailsTextBox.ReadOnly = true;
            this.accountDetailsTextBox.Size = new System.Drawing.Size(397, 66);
            this.accountDetailsTextBox.TabIndex = 25;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox1.Location = new System.Drawing.Point(4, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(25, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(460, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Your new account details are found below.  Please save this information for futur" +
    "e reference.";
            // 
            // panelAccountNumber
            // 
            this.panelAccountNumber.Controls.Add(this.buyPostage);
            this.panelAccountNumber.Controls.Add(this.labelBuyPostage);
            this.panelAccountNumber.Location = new System.Drawing.Point(23, 26);
            this.panelAccountNumber.Name = "panelAccountNumber";
            this.panelAccountNumber.Size = new System.Drawing.Size(495, 58);
            this.panelAccountNumber.TabIndex = 9;
            // 
            // buyPostage
            // 
            this.buyPostage.Location = new System.Drawing.Point(43, 27);
            this.buyPostage.Name = "buyPostage";
            this.buyPostage.Size = new System.Drawing.Size(91, 23);
            this.buyPostage.TabIndex = 7;
            this.buyPostage.Text = "Buy Postage...";
            this.buyPostage.UseVisualStyleBackColor = true;
            this.buyPostage.Click += new System.EventHandler(this.OnClickBuyPostage);
            // 
            // labelBuyPostage
            // 
            this.labelBuyPostage.AutoSize = true;
            this.labelBuyPostage.Location = new System.Drawing.Point(24, 9);
            this.labelBuyPostage.Name = "labelBuyPostage";
            this.labelBuyPostage.Size = new System.Drawing.Size(322, 13);
            this.labelBuyPostage.TabIndex = 6;
            this.labelBuyPostage.Text = "Before you begin shipping you must buy postage for the account:";
            // 
            // pictureComplete
            // 
            this.pictureComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureComplete.Location = new System.Drawing.Point(26, 5);
            this.pictureComplete.Name = "pictureComplete";
            this.pictureComplete.Size = new System.Drawing.Size(16, 16);
            this.pictureComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureComplete.TabIndex = 8;
            this.pictureComplete.TabStop = false;
            // 
            // labelFinish
            // 
            this.labelFinish.AutoSize = true;
            this.labelFinish.Location = new System.Drawing.Point(46, 6);
            this.labelFinish.Name = "labelFinish";
            this.labelFinish.Size = new System.Drawing.Size(255, 13);
            this.labelFinish.TabIndex = 7;
            this.labelFinish.Text = "Registration for your Express1 account is complete.";
            // 
            // wizardPageOptions
            // 
            this.wizardPageOptions.Controls.Add(this.optionsControlPanel);
            this.wizardPageOptions.Description = "Configure Express1 settings.";
            this.wizardPageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOptions.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptions.Name = "wizardPageOptions";
            this.wizardPageOptions.Size = new System.Drawing.Size(625, 460);
            this.wizardPageOptions.TabIndex = 0;
            this.wizardPageOptions.Title = "Express1 Settings";
            this.wizardPageOptions.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSettings);
            this.wizardPageOptions.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoOptions);
            // 
            // optionsControlPanel
            // 
            this.optionsControlPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControlPanel.Location = new System.Drawing.Point(23, 4);
            this.optionsControlPanel.Name = "optionsControlPanel";
            this.optionsControlPanel.Size = new System.Drawing.Size(493, 234);
            this.optionsControlPanel.TabIndex = 1;
            // 
            // wizardPageExisting
            // 
            this.wizardPageExisting.Controls.Add(this.label6);
            this.wizardPageExisting.Controls.Add(this.passwordExisting);
            this.wizardPageExisting.Controls.Add(this.labelPasswordExisting);
            this.wizardPageExisting.Controls.Add(this.accountExisting);
            this.wizardPageExisting.Controls.Add(this.labelAccountExisting);
            this.wizardPageExisting.Description = "Enter your existing account information.";
            this.wizardPageExisting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageExisting.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageExisting.Location = new System.Drawing.Point(0, 0);
            this.wizardPageExisting.Name = "wizardPageExisting";
            this.wizardPageExisting.Size = new System.Drawing.Size(625, 460);
            this.wizardPageExisting.TabIndex = 0;
            this.wizardPageExisting.Title = "Express1 Account";
            this.wizardPageExisting.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepExistingNext);
            this.wizardPageExisting.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoExisting);
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.DimGray;
            this.label6.Location = new System.Drawing.Point(159, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(326, 39);
            this.label6.TabIndex = 15;
            this.label6.Text = "Please contact Express1 at 1-800-399-3971 to retrieve your account number and pas" +
    "sword.";
            // 
            // passwordExisting
            // 
            this.passwordExisting.Location = new System.Drawing.Point(162, 35);
            this.passwordExisting.MaxLength = 36;
            this.passwordExisting.Name = "passwordExisting";
            this.passwordExisting.Size = new System.Drawing.Size(313, 21);
            this.passwordExisting.TabIndex = 14;
            this.passwordExisting.UseSystemPasswordChar = true;
            // 
            // labelPasswordExisting
            // 
            this.labelPasswordExisting.AutoSize = true;
            this.labelPasswordExisting.Location = new System.Drawing.Point(99, 39);
            this.labelPasswordExisting.Name = "labelPasswordExisting";
            this.labelPasswordExisting.Size = new System.Drawing.Size(57, 13);
            this.labelPasswordExisting.TabIndex = 13;
            this.labelPasswordExisting.Text = "Password:";
            // 
            // accountExisting
            // 
            this.accountExisting.Location = new System.Drawing.Point(162, 8);
            this.accountExisting.Name = "accountExisting";
            this.accountExisting.Size = new System.Drawing.Size(313, 21);
            this.accountExisting.TabIndex = 12;
            // 
            // labelAccountExisting
            // 
            this.labelAccountExisting.AutoSize = true;
            this.labelAccountExisting.Location = new System.Drawing.Point(21, 11);
            this.labelAccountExisting.Name = "labelAccountExisting";
            this.labelAccountExisting.Size = new System.Drawing.Size(135, 13);
            this.labelAccountExisting.TabIndex = 11;
            this.labelAccountExisting.Text = "Express1 account number:";
            // 
            // wizardPageAccountList
            // 
            this.wizardPageAccountList.Controls.Add(this.accountControlPanel);
            this.wizardPageAccountList.Controls.Add(this.label5);
            this.wizardPageAccountList.Description = "Setup Express1 accounts to use from ShipWorks.";
            this.wizardPageAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccountList.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAccountList.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccountList.Name = "wizardPageAccountList";
            this.wizardPageAccountList.Size = new System.Drawing.Size(625, 460);
            this.wizardPageAccountList.TabIndex = 0;
            this.wizardPageAccountList.Title = "Express1 Accounts";
            // 
            // accountControlPanel
            // 
            this.accountControlPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControlPanel.Location = new System.Drawing.Point(36, 27);
            this.accountControlPanel.Name = "accountControlPanel";
            this.accountControlPanel.Size = new System.Drawing.Size(450, 160);
            this.accountControlPanel.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Add or edit Express1 accounts:";
            // 
            // Express1SetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 567);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MinimumSize = new System.Drawing.Size(635, 605);
            this.Name = "Express1SetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageAccountList,
            this.wizardPageAccountType,
            this.wizardPageAddress,
            this.wizardPageAgreement,
            this.wizardPagePayment,
            this.wizardPageExisting,
            this.wizardPageOptions,
            this.wizardPageFinish});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Auto;
            this.Text = "Express1 Setup Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageAccountType.ResumeLayout(false);
            this.wizardPageAccountType.PerformLayout();
            this.wizardPageAddress.ResumeLayout(false);
            this.wizardPageAgreement.ResumeLayout(false);
            this.wizardPageAgreement.PerformLayout();
            this.wizardPagePayment.ResumeLayout(false);
            this.creditCardDetailsPanel.ResumeLayout(false);
            this.creditCardDetailsPanel.PerformLayout();
            this.wizardPageFinish.ResumeLayout(false);
            this.wizardPageFinish.PerformLayout();
            this.panelAccountCredentials.ResumeLayout(false);
            this.panelAccountCredentials.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelAccountNumber.ResumeLayout(false);
            this.panelAccountNumber.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureComplete)).EndInit();
            this.wizardPageOptions.ResumeLayout(false);
            this.wizardPageExisting.ResumeLayout(false);
            this.wizardPageExisting.PerformLayout();
            this.wizardPageAccountList.ResumeLayout(false);
            this.wizardPageAccountList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageAccountType;
        private System.Windows.Forms.RadioButton radioNewAccount;
        private System.Windows.Forms.RadioButton radioExistingAccount;
        private System.Windows.Forms.Label labelInfo1;
        private UI.Wizard.WizardPage wizardPageAddress;
        private Data.Controls.AutofillPersonControl personControl;
        private UI.Wizard.WizardPage wizardPageAgreement;
        private UI.Wizard.WizardPage wizardPagePayment;
        private System.Windows.Forms.Label label1;
        private Data.Controls.AutofillPersonControl personCreditCard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cardExpireYear;
        private System.Windows.Forms.ComboBox cardExpireMonth;
        private System.Windows.Forms.Label labelCardExpiration;
        private System.Windows.Forms.TextBox cardNumber;
        private System.Windows.Forms.Label labelCardNumber;
        private System.Windows.Forms.ComboBox cardType;
        private System.Windows.Forms.Label labelCardType;
        private UI.Wizard.WizardPage wizardPageFinish;
        private System.Windows.Forms.Panel panelAccountNumber;
        private System.Windows.Forms.Button buyPostage;
        private System.Windows.Forms.Label labelBuyPostage;
        private System.Windows.Forms.PictureBox pictureComplete;
        private System.Windows.Forms.Label labelFinish;
        private UI.Wizard.WizardPage wizardPageOptions;
        private System.Windows.Forms.Panel optionsControlPanel;
        private UI.Wizard.WizardPage wizardPageExisting;
        private System.Windows.Forms.TextBox passwordExisting;
        private System.Windows.Forms.Label labelPasswordExisting;
        private System.Windows.Forms.TextBox accountExisting;
        private System.Windows.Forms.Label labelAccountExisting;
        private System.Windows.Forms.CheckBox termsCheckBox;
        private System.Windows.Forms.RichTextBox licenseAgreement;
        private System.Windows.Forms.Panel panelAccountCredentials;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox accountDetailsTextBox;
        private UI.Wizard.WizardPage wizardPageAccountList;
        private System.Windows.Forms.Panel accountControlPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel creditCardDetailsPanel;
    }
}