namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    partial class UpsOltSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpsOltSetupWizard));
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.linkUpsOpenAccount = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelWelcome2 = new System.Windows.Forms.Label();
            this.labelWelcome1 = new System.Windows.Forms.Label();
            this.wizardPageLicense = new ShipWorks.UI.Wizard.WizardPage();
            this.printAgreement = new System.Windows.Forms.Button();
            this.radioDeclineAgreement = new System.Windows.Forms.RadioButton();
            this.radioAcceptAgreement = new System.Windows.Forms.RadioButton();
            this.licenseAgreement = new System.Windows.Forms.RichTextBox();
            this.wizardPageAccount = new ShipWorks.UI.Wizard.WizardPage();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.linkGetAccount = new System.Windows.Forms.Label();
            this.labelGetAcount = new System.Windows.Forms.Label();
            this.account = new System.Windows.Forms.TextBox();
            this.labelUpsAccount = new System.Windows.Forms.Label();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.wizardPageFinish = new ShipWorks.UI.Wizard.WizardPage();
            this.linkUpsOlt = new System.Windows.Forms.Label();
            this.linkUps = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelUps1 = new System.Windows.Forms.Label();
            this.labelSetupComplete = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPageRates = new ShipWorks.UI.Wizard.WizardPage();
            this.upsRateTypeControl = new ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsAccountRateTypeControl();
            this.wizardPageOptions = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControl = new ShipWorks.Shipping.Carriers.UPS.OnLineTools.UpsOltOptionsControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.wizardPageLicense.SuspendLayout();
            this.wizardPageAccount.SuspendLayout();
            this.wizardPageFinish.SuspendLayout();
            this.wizardPageRates.SuspendLayout();
            this.wizardPageOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(411, 487);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(492, 487);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(330, 487);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageOptions);
            this.mainPanel.Size = new System.Drawing.Size(579, 415);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 477);
            this.etchBottom.Size = new System.Drawing.Size(583, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.glo_ups_brandmark_pfv;
            this.pictureBox.Location = new System.Drawing.Point(526, 1);
            this.pictureBox.Size = new System.Drawing.Size(42, 53);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(579, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.linkUpsOpenAccount);
            this.wizardPageWelcome.Controls.Add(this.labelInfo2);
            this.wizardPageWelcome.Controls.Add(this.labelWelcome2);
            this.wizardPageWelcome.Controls.Add(this.labelWelcome1);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your UPS account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(579, 415);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup UPS OnLine® Tools";
            this.wizardPageWelcome.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextWelcome);
            // 
            // linkUpsOpenAccount
            // 
            this.linkUpsOpenAccount.AutoSize = true;
            this.linkUpsOpenAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkUpsOpenAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkUpsOpenAccount.ForeColor = System.Drawing.Color.Blue;
            this.linkUpsOpenAccount.Location = new System.Drawing.Point(55, 123);
            this.linkUpsOpenAccount.Name = "linkUpsOpenAccount";
            this.linkUpsOpenAccount.Size = new System.Drawing.Size(55, 13);
            this.linkUpsOpenAccount.TabIndex = 3;
            this.linkUpsOpenAccount.Text = "click here.";
            this.linkUpsOpenAccount.Click += new System.EventHandler(this.OnLinkOpenAccount);
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(21, 110);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(425, 30);
            this.labelInfo2.TabIndex = 2;
            this.labelInfo2.Text = "You must have a UPS shipping account before continuing. To get a UPS account, ple" +
                "ase ";
            // 
            // labelWelcome2
            // 
            this.labelWelcome2.Location = new System.Drawing.Point(21, 48);
            this.labelWelcome2.Name = "labelWelcome2";
            this.labelWelcome2.Size = new System.Drawing.Size(516, 59);
            this.labelWelcome2.TabIndex = 1;
            this.labelWelcome2.Text = resources.GetString("labelWelcome2.Text");
            // 
            // labelWelcome1
            // 
            this.labelWelcome1.Location = new System.Drawing.Point(21, 8);
            this.labelWelcome1.Name = "labelWelcome1";
            this.labelWelcome1.Size = new System.Drawing.Size(516, 42);
            this.labelWelcome1.TabIndex = 0;
            this.labelWelcome1.Text = "This wizard will assist you in completing the necessary licensing and registratio" +
                "n requirements to activate and use the UPS OnLine Tools from ShipWorks.";
            // 
            // wizardPageLicense
            // 
            this.wizardPageLicense.Controls.Add(this.printAgreement);
            this.wizardPageLicense.Controls.Add(this.radioDeclineAgreement);
            this.wizardPageLicense.Controls.Add(this.radioAcceptAgreement);
            this.wizardPageLicense.Controls.Add(this.licenseAgreement);
            this.wizardPageLicense.Description = "Please read the following important information before continuing.";
            this.wizardPageLicense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageLicense.Location = new System.Drawing.Point(0, 0);
            this.wizardPageLicense.Name = "wizardPageLicense";
            this.wizardPageLicense.Size = new System.Drawing.Size(579, 415);
            this.wizardPageLicense.TabIndex = 0;
            this.wizardPageLicense.Title = "License Agreement";
            this.wizardPageLicense.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAgreement);
            // 
            // printAgreement
            // 
            this.printAgreement.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.printAgreement.Location = new System.Drawing.Point(381, 356);
            this.printAgreement.Name = "printAgreement";
            this.printAgreement.Size = new System.Drawing.Size(113, 23);
            this.printAgreement.TabIndex = 3;
            this.printAgreement.Text = "Print Agreement...";
            this.printAgreement.UseVisualStyleBackColor = true;
            this.printAgreement.Click += new System.EventHandler(this.OnPrintAgreement);
            // 
            // radioDeclineAgreement
            // 
            this.radioDeclineAgreement.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioDeclineAgreement.AutoSize = true;
            this.radioDeclineAgreement.Checked = true;
            this.radioDeclineAgreement.Location = new System.Drawing.Point(24, 382);
            this.radioDeclineAgreement.Name = "radioDeclineAgreement";
            this.radioDeclineAgreement.Size = new System.Drawing.Size(191, 17);
            this.radioDeclineAgreement.TabIndex = 2;
            this.radioDeclineAgreement.TabStop = true;
            this.radioDeclineAgreement.Text = "No, I do not accept the agreement.";
            this.radioDeclineAgreement.UseVisualStyleBackColor = true;
            this.radioDeclineAgreement.CheckedChanged += new System.EventHandler(this.OnChangeAcceptAgreement);
            // 
            // radioAcceptAgreement
            // 
            this.radioAcceptAgreement.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioAcceptAgreement.AutoSize = true;
            this.radioAcceptAgreement.Location = new System.Drawing.Point(24, 359);
            this.radioAcceptAgreement.Name = "radioAcceptAgreement";
            this.radioAcceptAgreement.Size = new System.Drawing.Size(162, 17);
            this.radioAcceptAgreement.TabIndex = 1;
            this.radioAcceptAgreement.Text = "Yes, I accept the agreement.";
            this.radioAcceptAgreement.UseVisualStyleBackColor = true;
            this.radioAcceptAgreement.CheckedChanged += new System.EventHandler(this.OnChangeAcceptAgreement);
            // 
            // licenseAgreement
            // 
            this.licenseAgreement.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.licenseAgreement.Location = new System.Drawing.Point(24, 3);
            this.licenseAgreement.Name = "licenseAgreement";
            this.licenseAgreement.ReadOnly = true;
            this.licenseAgreement.Size = new System.Drawing.Size(470, 350);
            this.licenseAgreement.TabIndex = 0;
            this.licenseAgreement.Text = "";
            // 
            // wizardPageAccount
            // 
            this.wizardPageAccount.Controls.Add(this.personControl);
            this.wizardPageAccount.Controls.Add(this.linkGetAccount);
            this.wizardPageAccount.Controls.Add(this.labelGetAcount);
            this.wizardPageAccount.Controls.Add(this.account);
            this.wizardPageAccount.Controls.Add(this.labelUpsAccount);
            this.wizardPageAccount.Description = "Enter your UPS account information.";
            this.wizardPageAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccount.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccount.Name = "wizardPageAccount";
            this.wizardPageAccount.Size = new System.Drawing.Size(579, 415);
            this.wizardPageAccount.TabIndex = 0;
            this.wizardPageAccount.Title = "UPS Account";
            this.wizardPageAccount.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccount);
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields) (((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company)
                        | ShipWorks.Data.Controls.PersonFields.Street)
                        | ShipWorks.Data.Controls.PersonFields.City)
                        | ShipWorks.Data.Controls.PersonFields.State)
                        | ShipWorks.Data.Controls.PersonFields.Postal)
                        | ShipWorks.Data.Controls.PersonFields.Country)
                        | ShipWorks.Data.Controls.PersonFields.Residential)
                        | ShipWorks.Data.Controls.PersonFields.Email)
                        | ShipWorks.Data.Controls.PersonFields.Phone)
                        | ShipWorks.Data.Controls.PersonFields.Website)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.personControl.Location = new System.Drawing.Point(23, 47);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 363);
            this.personControl.TabIndex = 6;
            // 
            // linkGetAccount
            // 
            this.linkGetAccount.AutoSize = true;
            this.linkGetAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkGetAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkGetAccount.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkGetAccount.Location = new System.Drawing.Point(327, 28);
            this.linkGetAccount.Name = "linkGetAccount";
            this.linkGetAccount.Size = new System.Drawing.Size(55, 13);
            this.linkGetAccount.TabIndex = 3;
            this.linkGetAccount.Text = "click here.";
            this.linkGetAccount.Click += new System.EventHandler(this.OnLinkOpenAccount);
            // 
            // labelGetAcount
            // 
            this.labelGetAcount.AutoSize = true;
            this.labelGetAcount.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelGetAcount.Location = new System.Drawing.Point(228, 28);
            this.labelGetAcount.Name = "labelGetAcount";
            this.labelGetAcount.Size = new System.Drawing.Size(110, 13);
            this.labelGetAcount.TabIndex = 2;
            this.labelGetAcount.Text = "To open an account  ";
            // 
            // account
            // 
            this.account.Location = new System.Drawing.Point(41, 25);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(181, 20);
            this.account.TabIndex = 1;
            // 
            // labelUpsAccount
            // 
            this.labelUpsAccount.AutoSize = true;
            this.labelUpsAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUpsAccount.Location = new System.Drawing.Point(24, 7);
            this.labelUpsAccount.Name = "labelUpsAccount";
            this.labelUpsAccount.Size = new System.Drawing.Size(78, 13);
            this.labelUpsAccount.TabIndex = 0;
            this.labelUpsAccount.Text = "UPS Account";
            // 
            // printDialog
            // 
            this.printDialog.AllowPrintToFile = false;
            this.printDialog.Document = this.printDocument;
            this.printDialog.UseEXDialog = true;
            // 
            // printDocument
            // 
            this.printDocument.DocumentName = "ShipWorks - UPS License Agreement";
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.OnPrintAgreementPage);
            // 
            // wizardPageFinish
            // 
            this.wizardPageFinish.Controls.Add(this.linkUpsOlt);
            this.wizardPageFinish.Controls.Add(this.linkUps);
            this.wizardPageFinish.Controls.Add(this.label2);
            this.wizardPageFinish.Controls.Add(this.labelUps1);
            this.wizardPageFinish.Controls.Add(this.labelSetupComplete);
            this.wizardPageFinish.Description = "You are now ready to use UPS with ShpWorks.";
            this.wizardPageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinish.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinish.Name = "wizardPageFinish";
            this.wizardPageFinish.Size = new System.Drawing.Size(579, 415);
            this.wizardPageFinish.TabIndex = 0;
            this.wizardPageFinish.Title = "Setup Complete";
            this.wizardPageFinish.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoFinish);
            // 
            // linkUpsOlt
            // 
            this.linkUpsOlt.AutoSize = true;
            this.linkUpsOlt.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkUpsOlt.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkUpsOlt.ForeColor = System.Drawing.Color.Blue;
            this.linkUpsOlt.Location = new System.Drawing.Point(427, 108);
            this.linkUpsOlt.Name = "linkUpsOlt";
            this.linkUpsOlt.Size = new System.Drawing.Size(55, 13);
            this.linkUpsOlt.TabIndex = 4;
            this.linkUpsOlt.Text = "click here.";
            this.linkUpsOlt.Click += new System.EventHandler(this.OnLinkUpsOnlineTools);
            // 
            // linkUps
            // 
            this.linkUps.AutoSize = true;
            this.linkUps.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkUps.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.linkUps.ForeColor = System.Drawing.Color.Blue;
            this.linkUps.Location = new System.Drawing.Point(55, 51);
            this.linkUps.Name = "linkUps";
            this.linkUps.Size = new System.Drawing.Size(79, 13);
            this.linkUps.TabIndex = 3;
            this.linkUps.Text = "www.ups.com.";
            this.linkUps.Click += new System.EventHandler(this.OnLinkUpsHome);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(438, 71);
            this.label2.TabIndex = 2;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // labelUps1
            // 
            this.labelUps1.Location = new System.Drawing.Point(21, 38);
            this.labelUps1.Name = "labelUps1";
            this.labelUps1.Size = new System.Drawing.Size(438, 32);
            this.labelUps1.TabIndex = 1;
            this.labelUps1.Text = "Thank you for registering for UPS OnLine® Tools. To learn more about UPS services" +
                ", please";
            // 
            // labelSetupComplete
            // 
            this.labelSetupComplete.AutoSize = true;
            this.labelSetupComplete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSetupComplete.Location = new System.Drawing.Point(20, 10);
            this.labelSetupComplete.Name = "labelSetupComplete";
            this.labelSetupComplete.Size = new System.Drawing.Size(246, 13);
            this.labelSetupComplete.TabIndex = 0;
            this.labelSetupComplete.Text = "ShipWorks is now setup to connect to UPS!";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Location = new System.Drawing.Point(4, 489);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "UPS®, UPS && Shield Design® and UNITED PARCEL SERVICE® are registered trademarks " +
                "of United Parcel Service of America, Inc.";
            // 
            // wizardPageRates
            // 
            this.wizardPageRates.Controls.Add(this.upsRateTypeControl);
            this.wizardPageRates.Description = "Configure how ShipWorks displays shipping rates.";
            this.wizardPageRates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRates.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRates.Name = "wizardPageRates";
            this.wizardPageRates.Size = new System.Drawing.Size(579, 415);
            this.wizardPageRates.TabIndex = 0;
            this.wizardPageRates.Title = "UPS Rate Display";
            this.wizardPageRates.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextRates);
            // 
            // upsRateTypeControl
            // 
            this.upsRateTypeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.upsRateTypeControl.Location = new System.Drawing.Point(23, 10);
            this.upsRateTypeControl.Name = "upsRateTypeControl";
            this.upsRateTypeControl.Size = new System.Drawing.Size(389, 318);
            this.upsRateTypeControl.TabIndex = 0;
            // 
            // wizardPageOptions
            // 
            this.wizardPageOptions.Controls.Add(this.optionsControl);
            this.wizardPageOptions.Description = "Configure UPS settings.";
            this.wizardPageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptions.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptions.Name = "wizardPageOptions";
            this.wizardPageOptions.Size = new System.Drawing.Size(579, 415);
            this.wizardPageOptions.TabIndex = 0;
            this.wizardPageOptions.Title = "UPS Settings";
            this.wizardPageOptions.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextOptions);
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.optionsControl.Location = new System.Drawing.Point(23, 8);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(287, 70);
            this.optionsControl.TabIndex = 0;
            // 
            // UpsSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 522);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "UpsSetupWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageLicense,
            this.wizardPageAccount,
            this.wizardPageRates,
            this.wizardPageOptions,
            this.wizardPageFinish});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "UPS Setup Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.next, 0);
            this.Controls.SetChildIndex(this.cancel, 0);
            this.Controls.SetChildIndex(this.topPanel, 0);
            this.Controls.SetChildIndex(this.back, 0);
            this.Controls.SetChildIndex(this.mainPanel, 0);
            this.Controls.SetChildIndex(this.etchBottom, 0);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.wizardPageWelcome.PerformLayout();
            this.wizardPageLicense.ResumeLayout(false);
            this.wizardPageLicense.PerformLayout();
            this.wizardPageAccount.ResumeLayout(false);
            this.wizardPageAccount.PerformLayout();
            this.wizardPageFinish.ResumeLayout(false);
            this.wizardPageFinish.PerformLayout();
            this.wizardPageRates.ResumeLayout(false);
            this.wizardPageOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private ShipWorks.UI.Wizard.WizardPage wizardPageLicense;
        private System.Windows.Forms.Label labelWelcome2;
        private System.Windows.Forms.Label labelWelcome1;
        private ShipWorks.UI.Wizard.WizardPage wizardPageAccount;
        private System.Windows.Forms.Button printAgreement;
        private System.Windows.Forms.RadioButton radioDeclineAgreement;
        private System.Windows.Forms.RadioButton radioAcceptAgreement;
        private System.Windows.Forms.RichTextBox licenseAgreement;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
        private ShipWorks.UI.Wizard.WizardPage wizardPageFinish;
        private System.Windows.Forms.Label linkGetAccount;
        private System.Windows.Forms.Label labelGetAcount;
        private System.Windows.Forms.TextBox account;
        private System.Windows.Forms.Label labelUpsAccount;
        private ShipWorks.Data.Controls.PersonControl personControl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelInfo2;
        private System.Windows.Forms.Label linkUpsOpenAccount;
        private System.Windows.Forms.Label labelSetupComplete;
        private System.Windows.Forms.Label labelUps1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label linkUpsOlt;
        private System.Windows.Forms.Label linkUps;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRates;
        private UpsAccountRateTypeControl upsRateTypeControl;
        private ShipWorks.UI.Wizard.WizardPage wizardPageOptions;
        private UpsOltOptionsControl optionsControl;
    }
}