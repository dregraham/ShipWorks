﻿using System;
using ShipWorks.Shipping.Carriers.UPS.Promo;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpsSetupWizard));
            this.wizardPageWelcomeOlt = new ShipWorks.UI.Wizard.WizardPage();
            this.helpLink1 = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.labelWelcome2 = new System.Windows.Forms.Label();
            this.accountNumberPanel = new System.Windows.Forms.Panel();
            this.account = new System.Windows.Forms.TextBox();
            this.labelUpsAccount = new System.Windows.Forms.Label();
            this.panelNewOrExisting = new System.Windows.Forms.Panel();
            this.existingAccount = new System.Windows.Forms.RadioButton();
            this.newAccount = new System.Windows.Forms.RadioButton();
            this.labelWelcome1 = new System.Windows.Forms.Label();
            this.wizardPageLicense = new ShipWorks.UI.Wizard.WizardPage();
            this.printAgreement = new System.Windows.Forms.Button();
            this.radioDeclineAgreement = new System.Windows.Forms.RadioButton();
            this.radioAcceptAgreement = new System.Windows.Forms.RadioButton();
            this.licenseAgreement = new System.Windows.Forms.RichTextBox();
            this.wizardPageAccount = new ShipWorks.UI.Wizard.WizardPage();
            this.personControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.wizardPageFinishOlt = new ShipWorks.UI.Wizard.WizardPage();
            this.upsPromoFailed = new System.Windows.Forms.Label();
            this.labelSetupCompleteNotifyTime = new System.Windows.Forms.Label();
            this.labelSetupComplete3 = new System.Windows.Forms.Label();
            this.labelSetupComplete2 = new System.Windows.Forms.Label();
            this.labelSetupComplete1 = new System.Windows.Forms.Label();
            this.wizardPageRates = new ShipWorks.UI.Wizard.WizardPage();
            this.upsRateTypeControl = new ShipWorks.Shipping.Carriers.UPS.UpsAccountRateTypeControl();
            this.wizardPageOptionsOlt = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControlOlt = new ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsOltOptionsControl();
            this.wizardPageWelcomeWorldShip = new ShipWorks.UI.Wizard.WizardPage();
            this.labelWsUpsAccountNumberLink = new System.Windows.Forms.Label();
            this.labelWsUpsOpenAccount = new System.Windows.Forms.Label();
            this.wsUpsAccountNumber = new System.Windows.Forms.TextBox();
            this.labelEnterWsUpsAccountNumber = new System.Windows.Forms.Label();
            this.linkWorldShipMaps = new ShipWorks.ApplicationCore.Interaction.HelpLink();
            this.worldShipAgree2 = new System.Windows.Forms.CheckBox();
            this.worldShipAgree1 = new System.Windows.Forms.CheckBox();
            this.labelWorldShipInfo2 = new System.Windows.Forms.Label();
            this.labelWorldShipInfo1 = new System.Windows.Forms.Label();
            this.labelWorldShipImportant = new System.Windows.Forms.Label();
            this.pictureBoxWorldShip = new System.Windows.Forms.PictureBox();
            this.labelWsWelcome1 = new System.Windows.Forms.Label();
            this.wizardPageOptionsWorldShip = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControlWorldShip = new ShipWorks.Shipping.Carriers.UPS.WorldShip.WorldShipOptionsControl();
            this.wizardPageAccountList = new ShipWorks.UI.Wizard.WizardPage();
            this.label3 = new System.Windows.Forms.Label();
            this.accountControl = new ShipWorks.Shipping.Carriers.UPS.UpsAccountManagerControl();
            this.wizardPageFinishAddAccount = new ShipWorks.UI.Wizard.WizardPage();
            this.congratsLabel = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.wizardPageFinishCreateAccountRegistrationFailed = new ShipWorks.UI.Wizard.WizardPage();
            this.labelCreateAccountRegistrationFailed4 = new System.Windows.Forms.Label();
            this.labelCreateAccountRegistrationFailed2 = new System.Windows.Forms.Label();
            this.labelCreateAccountRegistrationFailed3 = new System.Windows.Forms.Label();
            this.labelCreateAccountRegistrationFailed1 = new System.Windows.Forms.Label();
            this.wizardPagePromo = new ShipWorks.UI.Wizard.WizardPage();
            this.label2 = new System.Windows.Forms.Label();
            this.promoControls = new System.Windows.Forms.Panel();
            this.promoTermsLink = new System.Windows.Forms.LinkLabel();
            this.promoNo = new System.Windows.Forms.RadioButton();
            this.promoYes = new System.Windows.Forms.RadioButton();
            this.promoDescription = new System.Windows.Forms.Label();
            this.wizardPageInvoiceAuthentication = new ShipWorks.UI.Wizard.WizardPage();
            this.upsInvoiceAuthorizationControl = new ShipWorks.Shipping.Carriers.UPS.UpsInvoiceAuthorizationControl();
            this.invoiceAuthenticationInstructions = new System.Windows.Forms.Label();
            this.upsTrademarkInfo = new System.Windows.Forms.Label();
            this.upsFromShipWorksLogo = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcomeOlt.SuspendLayout();
            this.accountNumberPanel.SuspendLayout();
            this.panelNewOrExisting.SuspendLayout();
            this.wizardPageLicense.SuspendLayout();
            this.wizardPageAccount.SuspendLayout();
            this.wizardPageFinishOlt.SuspendLayout();
            this.wizardPageRates.SuspendLayout();
            this.wizardPageOptionsOlt.SuspendLayout();
            this.wizardPageWelcomeWorldShip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorldShip)).BeginInit();
            this.wizardPageOptionsWorldShip.SuspendLayout();
            this.wizardPageAccountList.SuspendLayout();
            this.wizardPageFinishAddAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.wizardPageFinishCreateAccountRegistrationFailed.SuspendLayout();
            this.wizardPagePromo.SuspendLayout();
            this.promoControls.SuspendLayout();
            this.wizardPageInvoiceAuthentication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upsFromShipWorksLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(411, 546);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(492, 546);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(330, 546);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageWelcomeOlt);
            this.mainPanel.Size = new System.Drawing.Size(579, 474);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 536);
            this.etchBottom.Size = new System.Drawing.Size(583, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.glo_ups_brandmark_pfv;
            this.pictureBox.Location = new System.Drawing.Point(534, 6);
            this.pictureBox.Size = new System.Drawing.Size(34, 44);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.upsFromShipWorksLogo);
            this.topPanel.Size = new System.Drawing.Size(579, 56);
            this.topPanel.Controls.SetChildIndex(this.pictureBox, 0);
            this.topPanel.Controls.SetChildIndex(this.upsFromShipWorksLogo, 0);
            // 
            // wizardPageWelcomeOlt
            // 
            this.wizardPageWelcomeOlt.Controls.Add(this.helpLink1);
            this.wizardPageWelcomeOlt.Controls.Add(this.labelWelcome2);
            this.wizardPageWelcomeOlt.Controls.Add(this.accountNumberPanel);
            this.wizardPageWelcomeOlt.Controls.Add(this.panelNewOrExisting);
            this.wizardPageWelcomeOlt.Controls.Add(this.labelWelcome1);
            this.wizardPageWelcomeOlt.Description = "Setup ShipWorks to work with your UPS account.";
            this.wizardPageWelcomeOlt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcomeOlt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcomeOlt.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcomeOlt.Name = "wizardPageWelcomeOlt";
            this.wizardPageWelcomeOlt.Size = new System.Drawing.Size(579, 474);
            this.wizardPageWelcomeOlt.TabIndex = 0;
            this.wizardPageWelcomeOlt.Title = "Account Registration";
            this.wizardPageWelcomeOlt.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextWelcome);
            this.wizardPageWelcomeOlt.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoWelcome);
            // 
            // helpLink1
            // 
            this.helpLink1.AutoSize = true;
            this.helpLink1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLink1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.helpLink1.ForeColor = System.Drawing.Color.Blue;
            this.helpLink1.Location = new System.Drawing.Point(184, 21);
            this.helpLink1.Name = "helpLink1";
            this.helpLink1.Size = new System.Drawing.Size(65, 13);
            this.helpLink1.TabIndex = 13;
            this.helpLink1.Text = "Learn More.";
            this.helpLink1.Url = "https://support.shipworks.com/hc/en-us/articles/360040291011";
            // 
            // labelWelcome2
            // 
            this.labelWelcome2.Location = new System.Drawing.Point(21, 47);
            this.labelWelcome2.Name = "labelWelcome2";
            this.labelWelcome2.Size = new System.Drawing.Size(516, 31);
            this.labelWelcome2.TabIndex = 12;
            this.labelWelcome2.Text = "Enable your free UPS from ShipWorks account today and start saving money on label" +
    "s. Setting up your account is easy and only takes about a minute.";
            // 
            // accountNumberPanel
            // 
            this.accountNumberPanel.Controls.Add(this.account);
            this.accountNumberPanel.Controls.Add(this.labelUpsAccount);
            this.accountNumberPanel.Location = new System.Drawing.Point(61, 135);
            this.accountNumberPanel.Name = "accountNumberPanel";
            this.accountNumberPanel.Size = new System.Drawing.Size(406, 50);
            this.accountNumberPanel.TabIndex = 11;
            // 
            // account
            // 
            this.account.Location = new System.Drawing.Point(0, 21);
            this.fieldLengthProvider.SetMaxLengthSource(this.account, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsAccountNumber);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(181, 21);
            this.account.TabIndex = 8;
            // 
            // labelUpsAccount
            // 
            this.labelUpsAccount.AutoSize = true;
            this.labelUpsAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpsAccount.Location = new System.Drawing.Point(-3, 0);
            this.labelUpsAccount.Name = "labelUpsAccount";
            this.labelUpsAccount.Size = new System.Drawing.Size(186, 13);
            this.labelUpsAccount.TabIndex = 7;
            this.labelUpsAccount.Text = "Enter Your UPS account number";
            // 
            // panelNewOrExisting
            // 
            this.panelNewOrExisting.Controls.Add(this.existingAccount);
            this.panelNewOrExisting.Controls.Add(this.newAccount);
            this.panelNewOrExisting.Location = new System.Drawing.Point(46, 76);
            this.panelNewOrExisting.Name = "panelNewOrExisting";
            this.panelNewOrExisting.Size = new System.Drawing.Size(458, 51);
            this.panelNewOrExisting.TabIndex = 6;
            // 
            // existingAccount
            // 
            this.existingAccount.AutoSize = true;
            this.existingAccount.Location = new System.Drawing.Point(0, 31);
            this.existingAccount.Name = "existingAccount";
            this.existingAccount.Size = new System.Drawing.Size(230, 17);
            this.existingAccount.TabIndex = 7;
            this.existingAccount.TabStop = true;
            this.existingAccount.Text = "Add my existing UPS account to ShipWorks";
            this.existingAccount.UseVisualStyleBackColor = true;
            this.existingAccount.CheckedChanged += new System.EventHandler(this.OnAccountOptionCheckChanged);
            // 
            // newAccount
            // 
            this.newAccount.AutoSize = true;
            this.newAccount.Checked = true;
            this.newAccount.Location = new System.Drawing.Point(0, 8);
            this.newAccount.Name = "newAccount";
            this.newAccount.Size = new System.Drawing.Size(335, 17);
            this.newAccount.TabIndex = 6;
            this.newAccount.TabStop = true;
            this.newAccount.Text = "Create my free UPS from ShipWorks account now to start saving";
            this.newAccount.UseVisualStyleBackColor = true;
            this.newAccount.CheckedChanged += new System.EventHandler(this.OnAccountOptionCheckChanged);
            // 
            // labelWelcome1
            // 
            this.labelWelcome1.Location = new System.Drawing.Point(21, 8);
            this.labelWelcome1.Name = "labelWelcome1";
            this.labelWelcome1.Size = new System.Drawing.Size(516, 26);
            this.labelWelcome1.TabIndex = 0;
            this.labelWelcome1.Text = "ShipWorks offers savings up to 62% on UPS Shipments, as well as waived fuel and r" +
    "esidential surcharges, through ShipWorks One Balance.";
            // 
            // wizardPageLicense
            // 
            this.wizardPageLicense.Controls.Add(this.printAgreement);
            this.wizardPageLicense.Controls.Add(this.radioDeclineAgreement);
            this.wizardPageLicense.Controls.Add(this.radioAcceptAgreement);
            this.wizardPageLicense.Controls.Add(this.licenseAgreement);
            this.wizardPageLicense.Description = "Please read the following important information before continuing.";
            this.wizardPageLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageLicense.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageLicense.Location = new System.Drawing.Point(0, 0);
            this.wizardPageLicense.Name = "wizardPageLicense";
            this.wizardPageLicense.Size = new System.Drawing.Size(579, 474);
            this.wizardPageLicense.TabIndex = 0;
            this.wizardPageLicense.Title = "Account Registration";
            this.wizardPageLicense.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnSteppingNextAgreement);
            this.wizardPageLicense.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAgreement);
            // 
            // printAgreement
            // 
            this.printAgreement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printAgreement.Location = new System.Drawing.Point(381, 385);
            this.printAgreement.Name = "printAgreement";
            this.printAgreement.Size = new System.Drawing.Size(113, 23);
            this.printAgreement.TabIndex = 3;
            this.printAgreement.Text = "Print Agreement...";
            this.printAgreement.UseVisualStyleBackColor = true;
            this.printAgreement.Click += new System.EventHandler(this.OnPrintAgreement);
            // 
            // radioDeclineAgreement
            // 
            this.radioDeclineAgreement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioDeclineAgreement.AutoSize = true;
            this.radioDeclineAgreement.Checked = true;
            this.radioDeclineAgreement.Location = new System.Drawing.Point(24, 411);
            this.radioDeclineAgreement.Name = "radioDeclineAgreement";
            this.radioDeclineAgreement.Size = new System.Drawing.Size(196, 17);
            this.radioDeclineAgreement.TabIndex = 2;
            this.radioDeclineAgreement.TabStop = true;
            this.radioDeclineAgreement.Text = "No, I do not accept the agreement.";
            this.radioDeclineAgreement.UseVisualStyleBackColor = true;
            this.radioDeclineAgreement.CheckedChanged += new System.EventHandler(this.OnChangeAcceptAgreement);
            // 
            // radioAcceptAgreement
            // 
            this.radioAcceptAgreement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioAcceptAgreement.AutoSize = true;
            this.radioAcceptAgreement.Location = new System.Drawing.Point(24, 388);
            this.radioAcceptAgreement.Name = "radioAcceptAgreement";
            this.radioAcceptAgreement.Size = new System.Drawing.Size(166, 17);
            this.radioAcceptAgreement.TabIndex = 1;
            this.radioAcceptAgreement.Text = "Yes, I accept the agreement.";
            this.radioAcceptAgreement.UseVisualStyleBackColor = true;
            this.radioAcceptAgreement.CheckedChanged += new System.EventHandler(this.OnChangeAcceptAgreement);
            // 
            // licenseAgreement
            // 
            this.licenseAgreement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.licenseAgreement.Location = new System.Drawing.Point(24, 3);
            this.licenseAgreement.Name = "licenseAgreement";
            this.licenseAgreement.ReadOnly = true;
            this.licenseAgreement.Size = new System.Drawing.Size(470, 379);
            this.licenseAgreement.TabIndex = 0;
            this.licenseAgreement.TabStop = false;
            this.licenseAgreement.Text = "";
            // 
            // wizardPageAccount
            // 
            this.wizardPageAccount.Controls.Add(this.personControl);
            this.wizardPageAccount.Description = "Enter your UPS account information.";
            this.wizardPageAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAccount.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccount.Name = "wizardPageAccount";
            this.wizardPageAccount.Size = new System.Drawing.Size(579, 474);
            this.wizardPageAccount.TabIndex = 0;
            this.wizardPageAccount.Title = "Account Registration";
            this.wizardPageAccount.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccount);
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
            | ShipWorks.Data.Controls.PersonFields.Website)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.Location = new System.Drawing.Point(23, 8);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 400);
            this.personControl.TabIndex = 6;
            // 
            // wizardPageFinishOlt
            // 
            this.wizardPageFinishOlt.Controls.Add(this.upsPromoFailed);
            this.wizardPageFinishOlt.Controls.Add(this.labelSetupCompleteNotifyTime);
            this.wizardPageFinishOlt.Controls.Add(this.labelSetupComplete3);
            this.wizardPageFinishOlt.Controls.Add(this.labelSetupComplete2);
            this.wizardPageFinishOlt.Controls.Add(this.labelSetupComplete1);
            this.wizardPageFinishOlt.Description = "You are now ready to use UPS with ShipWorks.";
            this.wizardPageFinishOlt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinishOlt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinishOlt.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinishOlt.Name = "wizardPageFinishOlt";
            this.wizardPageFinishOlt.Size = new System.Drawing.Size(579, 474);
            this.wizardPageFinishOlt.TabIndex = 0;
            this.wizardPageFinishOlt.Title = "Account Registration";
            // 
            // upsPromoFailed
            // 
            this.upsPromoFailed.Location = new System.Drawing.Point(21, 246);
            this.upsPromoFailed.Name = "upsPromoFailed";
            this.upsPromoFailed.Size = new System.Drawing.Size(438, 32);
            this.upsPromoFailed.TabIndex = 4;
            // 
            // labelSetupCompleteNotifyTime
            // 
            this.labelSetupCompleteNotifyTime.Location = new System.Drawing.Point(21, 135);
            this.labelSetupCompleteNotifyTime.Name = "labelSetupCompleteNotifyTime";
            this.labelSetupCompleteNotifyTime.Size = new System.Drawing.Size(438, 32);
            this.labelSetupCompleteNotifyTime.TabIndex = 3;
            // 
            // labelSetupComplete3
            // 
            this.labelSetupComplete3.Location = new System.Drawing.Point(21, 185);
            this.labelSetupComplete3.Name = "labelSetupComplete3";
            this.labelSetupComplete3.Size = new System.Drawing.Size(438, 32);
            this.labelSetupComplete3.TabIndex = 2;
            // 
            // labelSetupComplete2
            // 
            this.labelSetupComplete2.Location = new System.Drawing.Point(21, 88);
            this.labelSetupComplete2.Name = "labelSetupComplete2";
            this.labelSetupComplete2.Size = new System.Drawing.Size(438, 32);
            this.labelSetupComplete2.TabIndex = 1;
            // 
            // labelSetupComplete1
            // 
            this.labelSetupComplete1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSetupComplete1.Location = new System.Drawing.Point(20, 10);
            this.labelSetupComplete1.Name = "labelSetupComplete1";
            this.labelSetupComplete1.Size = new System.Drawing.Size(525, 28);
            this.labelSetupComplete1.TabIndex = 0;
            this.labelSetupComplete1.Text = "ShipWorks is now setup to connect to UPS!";
            // 
            // wizardPageRates
            // 
            this.wizardPageRates.Controls.Add(this.upsRateTypeControl);
            this.wizardPageRates.Description = "Configure how ShipWorks displays shipping rates.";
            this.wizardPageRates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRates.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageRates.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRates.Name = "wizardPageRates";
            this.wizardPageRates.Size = new System.Drawing.Size(579, 474);
            this.wizardPageRates.TabIndex = 0;
            this.wizardPageRates.Title = "Account Registration";
            this.wizardPageRates.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextWizardPageRates);
            this.wizardPageRates.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoRates);
            // 
            // upsRateTypeControl
            // 
            this.upsRateTypeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upsRateTypeControl.Location = new System.Drawing.Point(23, 10);
            this.upsRateTypeControl.Name = "upsRateTypeControl";
            this.upsRateTypeControl.Size = new System.Drawing.Size(389, 461);
            this.upsRateTypeControl.TabIndex = 0;
            // 
            // wizardPageOptionsOlt
            // 
            this.wizardPageOptionsOlt.Controls.Add(this.optionsControlOlt);
            this.wizardPageOptionsOlt.Description = "Configure UPS settings.";
            this.wizardPageOptionsOlt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptionsOlt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOptionsOlt.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptionsOlt.Name = "wizardPageOptionsOlt";
            this.wizardPageOptionsOlt.Size = new System.Drawing.Size(579, 474);
            this.wizardPageOptionsOlt.TabIndex = 0;
            this.wizardPageOptionsOlt.Title = "Account Registration";
            this.wizardPageOptionsOlt.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextOptionsOlt);
            // 
            // optionsControlOlt
            // 
            this.optionsControlOlt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControlOlt.Location = new System.Drawing.Point(23, 8);
            this.optionsControlOlt.Name = "optionsControlOlt";
            this.optionsControlOlt.Size = new System.Drawing.Size(287, 70);
            this.optionsControlOlt.TabIndex = 0;
            // 
            // wizardPageWelcomeWorldShip
            // 
            this.wizardPageWelcomeWorldShip.Controls.Add(this.labelWsUpsAccountNumberLink);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.labelWsUpsOpenAccount);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.wsUpsAccountNumber);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.labelEnterWsUpsAccountNumber);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.linkWorldShipMaps);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.worldShipAgree2);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.worldShipAgree1);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.labelWorldShipInfo2);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.labelWorldShipInfo1);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.labelWorldShipImportant);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.pictureBoxWorldShip);
            this.wizardPageWelcomeWorldShip.Controls.Add(this.labelWsWelcome1);
            this.wizardPageWelcomeWorldShip.Description = "Setup ShipWorks to work with WorldShip";
            this.wizardPageWelcomeWorldShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcomeWorldShip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcomeWorldShip.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcomeWorldShip.Name = "wizardPageWelcomeWorldShip";
            this.wizardPageWelcomeWorldShip.Size = new System.Drawing.Size(579, 474);
            this.wizardPageWelcomeWorldShip.TabIndex = 0;
            this.wizardPageWelcomeWorldShip.Title = "Account Registration";
            this.wizardPageWelcomeWorldShip.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextWelcome);
            this.wizardPageWelcomeWorldShip.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoWelcome);
            // 
            // labelWsUpsAccountNumberLink
            // 
            this.labelWsUpsAccountNumberLink.AutoSize = true;
            this.labelWsUpsAccountNumberLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelWsUpsAccountNumberLink.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWsUpsAccountNumberLink.ForeColor = System.Drawing.Color.RoyalBlue;
            this.labelWsUpsAccountNumberLink.Location = new System.Drawing.Point(330, 65);
            this.labelWsUpsAccountNumberLink.Name = "labelWsUpsAccountNumberLink";
            this.labelWsUpsAccountNumberLink.Size = new System.Drawing.Size(55, 13);
            this.labelWsUpsAccountNumberLink.TabIndex = 18;
            this.labelWsUpsAccountNumberLink.Text = "click here.";
            this.labelWsUpsAccountNumberLink.Click += new System.EventHandler(this.OnLinkOpenAccount);
            // 
            // labelWsUpsOpenAccount
            // 
            this.labelWsUpsOpenAccount.AutoSize = true;
            this.labelWsUpsOpenAccount.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelWsUpsOpenAccount.Location = new System.Drawing.Point(231, 65);
            this.labelWsUpsOpenAccount.Name = "labelWsUpsOpenAccount";
            this.labelWsUpsOpenAccount.Size = new System.Drawing.Size(108, 13);
            this.labelWsUpsOpenAccount.TabIndex = 17;
            this.labelWsUpsOpenAccount.Text = "To open an account  ";
            // 
            // wsUpsAccountNumber
            // 
            this.wsUpsAccountNumber.Location = new System.Drawing.Point(41, 62);
            this.fieldLengthProvider.SetMaxLengthSource(this.wsUpsAccountNumber, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsAccountNumber);
            this.wsUpsAccountNumber.Name = "wsUpsAccountNumber";
            this.wsUpsAccountNumber.Size = new System.Drawing.Size(181, 21);
            this.wsUpsAccountNumber.TabIndex = 16;
            // 
            // labelEnterWsUpsAccountNumber
            // 
            this.labelEnterWsUpsAccountNumber.AutoSize = true;
            this.labelEnterWsUpsAccountNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnterWsUpsAccountNumber.Location = new System.Drawing.Point(37, 46);
            this.labelEnterWsUpsAccountNumber.Name = "labelEnterWsUpsAccountNumber";
            this.labelEnterWsUpsAccountNumber.Size = new System.Drawing.Size(186, 13);
            this.labelEnterWsUpsAccountNumber.TabIndex = 15;
            this.labelEnterWsUpsAccountNumber.Text = "Enter Your UPS account number";
            // 
            // linkWorldShipMaps
            // 
            this.linkWorldShipMaps.AutoSize = true;
            this.linkWorldShipMaps.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkWorldShipMaps.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkWorldShipMaps.ForeColor = System.Drawing.Color.Blue;
            this.linkWorldShipMaps.Location = new System.Drawing.Point(149, 261);
            this.linkWorldShipMaps.Name = "linkWorldShipMaps";
            this.linkWorldShipMaps.Size = new System.Drawing.Size(82, 13);
            this.linkWorldShipMaps.TabIndex = 14;
            this.linkWorldShipMaps.Text = "explained here.";
            this.linkWorldShipMaps.Url = "https://shipworks.zendesk.com/hc/en-us/articles/360022466952";
            // 
            // worldShipAgree2
            // 
            this.worldShipAgree2.AutoSize = true;
            this.worldShipAgree2.Location = new System.Drawing.Point(40, 280);
            this.worldShipAgree2.Name = "worldShipAgree2";
            this.worldShipAgree2.Size = new System.Drawing.Size(400, 17);
            this.worldShipAgree2.TabIndex = 13;
            this.worldShipAgree2.Text = "I understand I need to setup import\\export maps correctly before processing.";
            this.worldShipAgree2.UseVisualStyleBackColor = true;
            // 
            // worldShipAgree1
            // 
            this.worldShipAgree1.AutoSize = true;
            this.worldShipAgree1.Location = new System.Drawing.Point(40, 173);
            this.worldShipAgree1.Name = "worldShipAgree1";
            this.worldShipAgree1.Size = new System.Drawing.Size(248, 17);
            this.worldShipAgree1.TabIndex = 12;
            this.worldShipAgree1.Text = "I understand and would rather use WorldShip.";
            this.worldShipAgree1.UseVisualStyleBackColor = true;
            // 
            // labelWorldShipInfo2
            // 
            this.labelWorldShipInfo2.Location = new System.Drawing.Point(38, 209);
            this.labelWorldShipInfo2.Name = "labelWorldShipInfo2";
            this.labelWorldShipInfo2.Size = new System.Drawing.Size(497, 68);
            this.labelWorldShipInfo2.TabIndex = 11;
            this.labelWorldShipInfo2.Text = resources.GetString("labelWorldShipInfo2.Text");
            // 
            // labelWorldShipInfo1
            // 
            this.labelWorldShipInfo1.Location = new System.Drawing.Point(38, 131);
            this.labelWorldShipInfo1.Name = "labelWorldShipInfo1";
            this.labelWorldShipInfo1.Size = new System.Drawing.Size(497, 47);
            this.labelWorldShipInfo1.TabIndex = 10;
            this.labelWorldShipInfo1.Text = resources.GetString("labelWorldShipInfo1.Text");
            // 
            // labelWorldShipImportant
            // 
            this.labelWorldShipImportant.AutoSize = true;
            this.labelWorldShipImportant.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWorldShipImportant.Location = new System.Drawing.Point(58, 99);
            this.labelWorldShipImportant.Name = "labelWorldShipImportant";
            this.labelWorldShipImportant.Size = new System.Drawing.Size(66, 13);
            this.labelWorldShipImportant.TabIndex = 9;
            this.labelWorldShipImportant.Text = "Important";
            // 
            // pictureBoxWorldShip
            // 
            this.pictureBoxWorldShip.Image = global::ShipWorks.Properties.Resources.warning32;
            this.pictureBoxWorldShip.Location = new System.Drawing.Point(23, 91);
            this.pictureBoxWorldShip.Name = "pictureBoxWorldShip";
            this.pictureBoxWorldShip.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxWorldShip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxWorldShip.TabIndex = 8;
            this.pictureBoxWorldShip.TabStop = false;
            // 
            // labelWsWelcome1
            // 
            this.labelWsWelcome1.Location = new System.Drawing.Point(21, 8);
            this.labelWsWelcome1.Name = "labelWsWelcome1";
            this.labelWsWelcome1.Size = new System.Drawing.Size(516, 42);
            this.labelWsWelcome1.TabIndex = 4;
            this.labelWsWelcome1.Text = "This wizard will assist you in setting up ShipWorks to work with UPS WorldShip.  " +
    "You should already have WorldShip installed on your local network before continu" +
    "ing.";
            // 
            // wizardPageOptionsWorldShip
            // 
            this.wizardPageOptionsWorldShip.Controls.Add(this.optionsControlWorldShip);
            this.wizardPageOptionsWorldShip.Description = "Configure settings for WorldShip.";
            this.wizardPageOptionsWorldShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptionsWorldShip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOptionsWorldShip.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptionsWorldShip.Name = "wizardPageOptionsWorldShip";
            this.wizardPageOptionsWorldShip.Size = new System.Drawing.Size(579, 474);
            this.wizardPageOptionsWorldShip.TabIndex = 0;
            this.wizardPageOptionsWorldShip.Title = "Account Registration";
            this.wizardPageOptionsWorldShip.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextOptionsWorldShip);
            // 
            // optionsControlWorldShip
            // 
            this.optionsControlWorldShip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControlWorldShip.Location = new System.Drawing.Point(24, 8);
            this.optionsControlWorldShip.Name = "optionsControlWorldShip";
            this.optionsControlWorldShip.Size = new System.Drawing.Size(473, 218);
            this.optionsControlWorldShip.TabIndex = 0;
            // 
            // wizardPageAccountList
            // 
            this.wizardPageAccountList.Controls.Add(this.label3);
            this.wizardPageAccountList.Controls.Add(this.accountControl);
            this.wizardPageAccountList.Description = "Setup UPS accounts to use from ShipWorks.";
            this.wizardPageAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccountList.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAccountList.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccountList.Name = "wizardPageAccountList";
            this.wizardPageAccountList.Size = new System.Drawing.Size(579, 474);
            this.wizardPageAccountList.TabIndex = 0;
            this.wizardPageAccountList.Title = "Account Registration";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Add or edit UPS accounts:";
            // 
            // accountControl
            // 
            this.accountControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountControl.Location = new System.Drawing.Point(41, 27);
            this.accountControl.Name = "accountControl";
            this.accountControl.Size = new System.Drawing.Size(400, 168);
            this.accountControl.TabIndex = 0;
            // 
            // wizardPageFinishAddAccount
            // 
            this.wizardPageFinishAddAccount.Controls.Add(this.congratsLabel);
            this.wizardPageFinishAddAccount.Description = "ShipWorks is now setup to use your account.";
            this.wizardPageFinishAddAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinishAddAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinishAddAccount.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinishAddAccount.Name = "wizardPageFinishAddAccount";
            this.wizardPageFinishAddAccount.Size = new System.Drawing.Size(579, 474);
            this.wizardPageFinishAddAccount.TabIndex = 0;
            this.wizardPageFinishAddAccount.Title = "Account Registration";
            // 
            // congratsLabel
            // 
            this.congratsLabel.AutoSize = true;
            this.congratsLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.congratsLabel.Location = new System.Drawing.Point(24, 9);
            this.congratsLabel.Name = "congratsLabel";
            this.congratsLabel.Size = new System.Drawing.Size(258, 13);
            this.congratsLabel.TabIndex = 1;
            this.congratsLabel.Text = "ShipWorks is now setup to use your account!";
            // 
            // wizardPageFinishCreateAccountRegistrationFailed
            // 
            this.wizardPageFinishCreateAccountRegistrationFailed.Controls.Add(this.labelCreateAccountRegistrationFailed4);
            this.wizardPageFinishCreateAccountRegistrationFailed.Controls.Add(this.labelCreateAccountRegistrationFailed2);
            this.wizardPageFinishCreateAccountRegistrationFailed.Controls.Add(this.labelCreateAccountRegistrationFailed3);
            this.wizardPageFinishCreateAccountRegistrationFailed.Controls.Add(this.labelCreateAccountRegistrationFailed1);
            this.wizardPageFinishCreateAccountRegistrationFailed.Description = "ShipWorks created your UPS Account, but couldn\'t add it to ShipWorks.";
            this.wizardPageFinishCreateAccountRegistrationFailed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinishCreateAccountRegistrationFailed.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinishCreateAccountRegistrationFailed.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinishCreateAccountRegistrationFailed.Name = "wizardPageFinishCreateAccountRegistrationFailed";
            this.wizardPageFinishCreateAccountRegistrationFailed.Size = new System.Drawing.Size(579, 474);
            this.wizardPageFinishCreateAccountRegistrationFailed.TabIndex = 0;
            this.wizardPageFinishCreateAccountRegistrationFailed.Title = "Account Registration";
            // 
            // labelCreateAccountRegistrationFailed4
            // 
            this.labelCreateAccountRegistrationFailed4.Location = new System.Drawing.Point(20, 216);
            this.labelCreateAccountRegistrationFailed4.Name = "labelCreateAccountRegistrationFailed4";
            this.labelCreateAccountRegistrationFailed4.Size = new System.Drawing.Size(438, 32);
            this.labelCreateAccountRegistrationFailed4.TabIndex = 6;
            this.labelCreateAccountRegistrationFailed4.Text = "Please watch your email for a confirmation from UPS with more information on how " +
    "to use your account.";
            // 
            // labelCreateAccountRegistrationFailed2
            // 
            this.labelCreateAccountRegistrationFailed2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCreateAccountRegistrationFailed2.Location = new System.Drawing.Point(20, 52);
            this.labelCreateAccountRegistrationFailed2.Name = "labelCreateAccountRegistrationFailed2";
            this.labelCreateAccountRegistrationFailed2.Size = new System.Drawing.Size(438, 51);
            this.labelCreateAccountRegistrationFailed2.TabIndex = 5;
            this.labelCreateAccountRegistrationFailed2.Text = "The new UPS account is currently not registered within the ShipWorks software. To" +
    " add this account later, select \"USE an existing UPS account\" and enter ###### a" +
    "s your UPS account number.";
            // 
            // labelCreateAccountRegistrationFailed3
            // 
            this.labelCreateAccountRegistrationFailed3.Location = new System.Drawing.Point(20, 150);
            this.labelCreateAccountRegistrationFailed3.Name = "labelCreateAccountRegistrationFailed3";
            this.labelCreateAccountRegistrationFailed3.Size = new System.Drawing.Size(438, 32);
            this.labelCreateAccountRegistrationFailed3.TabIndex = 4;
            this.labelCreateAccountRegistrationFailed3.Text = "Your new UPS account number: XXXXX";
            // 
            // labelCreateAccountRegistrationFailed1
            // 
            this.labelCreateAccountRegistrationFailed1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCreateAccountRegistrationFailed1.Location = new System.Drawing.Point(20, 5);
            this.labelCreateAccountRegistrationFailed1.Name = "labelCreateAccountRegistrationFailed1";
            this.labelCreateAccountRegistrationFailed1.Size = new System.Drawing.Size(525, 28);
            this.labelCreateAccountRegistrationFailed1.TabIndex = 3;
            this.labelCreateAccountRegistrationFailed1.Text = "You have successfully created a UPS account within ShipWorks!";
            // 
            // wizardPagePromo
            // 
            this.wizardPagePromo.Controls.Add(this.label2);
            this.wizardPagePromo.Controls.Add(this.promoControls);
            this.wizardPagePromo.Controls.Add(this.promoDescription);
            this.wizardPagePromo.Description = "ShipWorks exclusive promotion from UPS.";
            this.wizardPagePromo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePromo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePromo.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePromo.Name = "wizardPagePromo";
            this.wizardPagePromo.Size = new System.Drawing.Size(579, 474);
            this.wizardPagePromo.TabIndex = 0;
            this.wizardPagePromo.Title = "Account Registration";
            this.wizardPagePromo.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnWizardPagePromoStepNext);
            this.wizardPagePromo.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnWizardPagePromoSteppingInto);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(23, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(347, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "You are eligible for a ShipWorks exclusive promotional price!";
            // 
            // promoControls
            // 
            this.promoControls.Controls.Add(this.promoTermsLink);
            this.promoControls.Controls.Add(this.promoNo);
            this.promoControls.Controls.Add(this.promoYes);
            this.promoControls.Location = new System.Drawing.Point(23, 63);
            this.promoControls.Name = "promoControls";
            this.promoControls.Size = new System.Drawing.Size(328, 100);
            this.promoControls.TabIndex = 5;
            // 
            // promoTermsLink
            // 
            this.promoTermsLink.AutoSize = true;
            this.promoTermsLink.Location = new System.Drawing.Point(1, 4);
            this.promoTermsLink.Name = "promoTermsLink";
            this.promoTermsLink.Size = new System.Drawing.Size(110, 13);
            this.promoTermsLink.TabIndex = 1;
            this.promoTermsLink.TabStop = true;
            this.promoTermsLink.Text = "Terms and Conditions";
            this.promoTermsLink.VisitedLinkColor = System.Drawing.Color.Blue;
            this.promoTermsLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnPromoTermsLinkClicked);
            // 
            // promoNo
            // 
            this.promoNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.promoNo.AutoSize = true;
            this.promoNo.Checked = true;
            this.promoNo.Location = new System.Drawing.Point(3, 53);
            this.promoNo.Name = "promoNo";
            this.promoNo.Size = new System.Drawing.Size(243, 17);
            this.promoNo.TabIndex = 4;
            this.promoNo.TabStop = true;
            this.promoNo.Text = "No, I do not accept the Terms and Conditions";
            this.promoNo.UseVisualStyleBackColor = true;
            // 
            // promoYes
            // 
            this.promoYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.promoYes.AutoSize = true;
            this.promoYes.Location = new System.Drawing.Point(3, 30);
            this.promoYes.Name = "promoYes";
            this.promoYes.Size = new System.Drawing.Size(213, 17);
            this.promoYes.TabIndex = 3;
            this.promoYes.Text = "Yes, I accept the Terms and Conditions";
            this.promoYes.UseVisualStyleBackColor = true;
            // 
            // promoDescription
            // 
            this.promoDescription.AutoSize = true;
            this.promoDescription.Location = new System.Drawing.Point(23, 27);
            this.promoDescription.MaximumSize = new System.Drawing.Size(500, 0);
            this.promoDescription.Name = "promoDescription";
            this.promoDescription.Size = new System.Drawing.Size(90, 13);
            this.promoDescription.TabIndex = 0;
            this.promoDescription.Text = "promoDescription";
            // 
            // wizardPageInvoiceAuthentication
            // 
            this.wizardPageInvoiceAuthentication.Controls.Add(this.upsInvoiceAuthorizationControl);
            this.wizardPageInvoiceAuthentication.Controls.Add(this.invoiceAuthenticationInstructions);
            this.wizardPageInvoiceAuthentication.Description = "Account invoice authentication required";
            this.wizardPageInvoiceAuthentication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageInvoiceAuthentication.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageInvoiceAuthentication.Location = new System.Drawing.Point(0, 0);
            this.wizardPageInvoiceAuthentication.Name = "wizardPageInvoiceAuthentication";
            this.wizardPageInvoiceAuthentication.Size = new System.Drawing.Size(579, 474);
            this.wizardPageInvoiceAuthentication.TabIndex = 0;
            this.wizardPageInvoiceAuthentication.Title = "Account Registration";
            this.wizardPageInvoiceAuthentication.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextInvoiceAuthentication);
            this.wizardPageInvoiceAuthentication.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnStepIntoInvoiceAuthentication);
            // 
            // upsInvoiceAuthorizationControl
            // 
            this.upsInvoiceAuthorizationControl.Location = new System.Drawing.Point(23, 54);
            this.upsInvoiceAuthorizationControl.Name = "upsInvoiceAuthorizationControl";
            this.upsInvoiceAuthorizationControl.Size = new System.Drawing.Size(357, 135);
            this.upsInvoiceAuthorizationControl.TabIndex = 0;
            // 
            // invoiceAuthenticationInstructions
            // 
            this.invoiceAuthenticationInstructions.ForeColor = System.Drawing.Color.Black;
            this.invoiceAuthenticationInstructions.Location = new System.Drawing.Point(20, 5);
            this.invoiceAuthenticationInstructions.Name = "invoiceAuthenticationInstructions";
            this.invoiceAuthenticationInstructions.Size = new System.Drawing.Size(402, 49);
            this.invoiceAuthenticationInstructions.TabIndex = 21;
            this.invoiceAuthenticationInstructions.Text = "You must validate your account by providing information from a valid invoice.\r\n\r\n" +
    "You must use any of the last 3 invoices issued within the past {days} days.";
            // 
            // upsTrademarkInfo
            // 
            this.upsTrademarkInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.upsTrademarkInfo.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upsTrademarkInfo.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.upsTrademarkInfo.Location = new System.Drawing.Point(23, 500);
            this.upsTrademarkInfo.Name = "upsTrademarkInfo";
            this.upsTrademarkInfo.Size = new System.Drawing.Size(533, 22);
            this.upsTrademarkInfo.TabIndex = 7;
            this.upsTrademarkInfo.Text = "UPS, the UPS brandmark, UPS Ready®, and the color brown are trademarks of United Parcel Service of America, Inc. All Rights Reserved.";
            // 
            // upsFromShipWorksLogo
            // 
            this.upsFromShipWorksLogo.Image = global::ShipWorks.Properties.Resources.ups_from_shipworks;
            this.upsFromShipWorksLogo.Location = new System.Drawing.Point(387, 6);
            this.upsFromShipWorksLogo.Name = "upsFromShipWorksLogo";
            this.upsFromShipWorksLogo.Size = new System.Drawing.Size(181, 44);
            this.upsFromShipWorksLogo.TabIndex = 9;
            this.upsFromShipWorksLogo.TabStop = false;
            // 
            // UpsSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 581);
            this.Controls.Add(this.upsTrademarkInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UpsSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcomeOlt,
            this.wizardPageWelcomeWorldShip,
            this.wizardPageAccountList,
            this.wizardPageLicense,
            this.wizardPageAccount,
            this.wizardPageInvoiceAuthentication,
            this.wizardPageRates,
            this.wizardPageOptionsOlt,
            this.wizardPageOptionsWorldShip,
            this.wizardPagePromo,
            this.wizardPageFinishOlt,
            this.wizardPageFinishAddAccount,
            this.wizardPageFinishCreateAccountRegistrationFailed});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "UPS Setup Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Controls.SetChildIndex(this.next, 0);
            this.Controls.SetChildIndex(this.cancel, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.back, 0);
            this.Controls.SetChildIndex(this.mainPanel, 0);
            this.Controls.SetChildIndex(this.etchBottom, 0);
            this.Controls.SetChildIndex(this.upsTrademarkInfo, 0);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcomeOlt.ResumeLayout(false);
            this.wizardPageWelcomeOlt.PerformLayout();
            this.accountNumberPanel.ResumeLayout(false);
            this.accountNumberPanel.PerformLayout();
            this.panelNewOrExisting.ResumeLayout(false);
            this.panelNewOrExisting.PerformLayout();
            this.wizardPageLicense.ResumeLayout(false);
            this.wizardPageLicense.PerformLayout();
            this.wizardPageAccount.ResumeLayout(false);
            this.wizardPageFinishOlt.ResumeLayout(false);
            this.wizardPageRates.ResumeLayout(false);
            this.wizardPageOptionsOlt.ResumeLayout(false);
            this.wizardPageWelcomeWorldShip.ResumeLayout(false);
            this.wizardPageWelcomeWorldShip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorldShip)).EndInit();
            this.wizardPageOptionsWorldShip.ResumeLayout(false);
            this.wizardPageAccountList.ResumeLayout(false);
            this.wizardPageAccountList.PerformLayout();
            this.wizardPageFinishAddAccount.ResumeLayout(false);
            this.wizardPageFinishAddAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.wizardPageFinishCreateAccountRegistrationFailed.ResumeLayout(false);
            this.wizardPagePromo.ResumeLayout(false);
            this.wizardPagePromo.PerformLayout();
            this.promoControls.ResumeLayout(false);
            this.promoControls.PerformLayout();
            this.wizardPageInvoiceAuthentication.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.upsFromShipWorksLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcomeOlt;
        private ShipWorks.UI.Wizard.WizardPage wizardPageLicense;
        private System.Windows.Forms.Label labelWelcome1;
        private ShipWorks.UI.Wizard.WizardPage wizardPageAccount;
        private System.Windows.Forms.Button printAgreement;
        private System.Windows.Forms.RadioButton radioDeclineAgreement;
        private System.Windows.Forms.RadioButton radioAcceptAgreement;
        private System.Windows.Forms.RichTextBox licenseAgreement;
        private ShipWorks.UI.Wizard.WizardPage wizardPageFinishOlt;
        private ShipWorks.Data.Controls.AutofillPersonControl personControl;
        private System.Windows.Forms.Label labelSetupComplete1;
        private System.Windows.Forms.Label labelSetupComplete2;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRates;
        private ShipWorks.Shipping.Carriers.UPS.UpsAccountRateTypeControl upsRateTypeControl;
        private ShipWorks.UI.Wizard.WizardPage wizardPageOptionsOlt;
        private ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsOltOptionsControl optionsControlOlt;
        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcomeWorldShip;
        private ShipWorks.UI.Wizard.WizardPage wizardPageOptionsWorldShip;
        private ShipWorks.UI.Wizard.WizardPage wizardPageAccountList;
        private ShipWorks.Shipping.Carriers.UPS.WorldShip.WorldShipOptionsControl optionsControlWorldShip;
        private System.Windows.Forms.Label labelWsWelcome1;
        private System.Windows.Forms.Label label3;
        private UpsAccountManagerControl accountControl;
        private ShipWorks.UI.Wizard.WizardPage wizardPageFinishAddAccount;
        private System.Windows.Forms.Label congratsLabel;
        private System.Windows.Forms.PictureBox pictureBoxWorldShip;
        private System.Windows.Forms.Label labelWorldShipImportant;
        private System.Windows.Forms.Label labelWorldShipInfo2;
        private System.Windows.Forms.Label labelWorldShipInfo1;
        private System.Windows.Forms.CheckBox worldShipAgree1;
        private System.Windows.Forms.CheckBox worldShipAgree2;
        private ShipWorks.ApplicationCore.Interaction.HelpLink linkWorldShipMaps;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;

        private System.Windows.Forms.Panel panelNewOrExisting;
        private System.Windows.Forms.RadioButton existingAccount;
        private System.Windows.Forms.RadioButton newAccount;
        private System.Windows.Forms.TextBox account;
        private System.Windows.Forms.Label labelUpsAccount;
        private System.Windows.Forms.Label labelWsUpsAccountNumberLink;
        private System.Windows.Forms.Label labelWsUpsOpenAccount;
        private System.Windows.Forms.TextBox wsUpsAccountNumber;
        private System.Windows.Forms.Label labelEnterWsUpsAccountNumber;
        private System.Windows.Forms.Panel accountNumberPanel;
        private System.Windows.Forms.Label labelSetupComplete3;
        private UI.Wizard.WizardPage wizardPageFinishCreateAccountRegistrationFailed;
        private System.Windows.Forms.Label labelCreateAccountRegistrationFailed4;
        private System.Windows.Forms.Label labelCreateAccountRegistrationFailed2;
        private System.Windows.Forms.Label labelCreateAccountRegistrationFailed3;
        private System.Windows.Forms.Label labelCreateAccountRegistrationFailed1;
        private System.Windows.Forms.Label labelSetupCompleteNotifyTime;
        private UI.Wizard.WizardPage wizardPagePromo;
        private System.Windows.Forms.LinkLabel promoTermsLink;
        private System.Windows.Forms.Label promoDescription;
        private System.Windows.Forms.Label upsPromoFailed;
        private System.Windows.Forms.Panel promoControls;
        private System.Windows.Forms.RadioButton promoNo;
        private System.Windows.Forms.RadioButton promoYes;
        private UI.Wizard.WizardPage wizardPageInvoiceAuthentication;
        private UpsInvoiceAuthorizationControl upsInvoiceAuthorizationControl;
        private System.Windows.Forms.Label invoiceAuthenticationInstructions;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelWelcome2;
        private System.Windows.Forms.Label upsTrademarkInfo;
        private ApplicationCore.Interaction.HelpLink helpLink1;
        private System.Windows.Forms.PictureBox upsFromShipWorksLogo;
    }
}
