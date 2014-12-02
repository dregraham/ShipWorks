namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FedExSetupWizard));
            this.wizardPageInitial = new ShipWorks.UI.Wizard.WizardPage();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.personControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.wizardPageSettings = new ShipWorks.UI.Wizard.WizardPage();
            this.accountSettingsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExAccountSettingsControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExOptionsControl();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.labelFedExAccount = new System.Windows.Forms.Label();
            this.linkGetAccount = new System.Windows.Forms.Label();
            this.labelGetAcount = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageInitial.SuspendLayout();
            this.wizardPageContactInfo.SuspendLayout();
            this.wizardPageSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(416, 546);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(497, 546);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(335, 546);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageContactInfo);
            this.mainPanel.Size = new System.Drawing.Size(584, 474);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 536);
            this.etchBottom.Size = new System.Drawing.Size(588, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(445, 3);
            this.pictureBox.Size = new System.Drawing.Size(113, 40);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(584, 56);
            // 
            // wizardPageInitial
            // 
            this.wizardPageInitial.Controls.Add(this.linkGetAccount);
            this.wizardPageInitial.Controls.Add(this.labelGetAcount);
            this.wizardPageInitial.Controls.Add(this.accountNumber);
            this.wizardPageInitial.Controls.Add(this.labelFedExAccount);
            this.wizardPageInitial.Controls.Add(this.labelInfo1);
            this.wizardPageInitial.Description = "Set up ShipWorks to work with your FedEx account.";
            this.wizardPageInitial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageInitial.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageInitial.Location = new System.Drawing.Point(0, 0);
            this.wizardPageInitial.Name = "wizardPageInitial";
            this.wizardPageInitial.Size = new System.Drawing.Size(584, 474);
            this.wizardPageInitial.TabIndex = 0;
            this.wizardPageInitial.Title = "Set up FedEx® Shipping";
            this.wizardPageInitial.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextInitialPage);
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(21, 8);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(454, 47);
            this.labelInfo1.TabIndex = 0;
            this.labelInfo1.Text = "This wizard will assist you in registering your FedEx shipping account for use wi" +
    "th ShipWorks. This enables you to begin shipping, tracking, and printing FedEx l" +
    "abels directly from ShipWorks.";
            // 
            // wizardPageContactInfo
            // 
            this.wizardPageContactInfo.Controls.Add(this.personControl);
            this.wizardPageContactInfo.Description = "Enter your FedEx account information.";
            this.wizardPageContactInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageContactInfo.Location = new System.Drawing.Point(0, 0);
            this.wizardPageContactInfo.Name = "wizardPageContactInfo";
            this.wizardPageContactInfo.Size = new System.Drawing.Size(584, 474);
            this.wizardPageContactInfo.TabIndex = 0;
            this.wizardPageContactInfo.Title = "FedEx Account";
            this.wizardPageContactInfo.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccountInfo);
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
            this.personControl.Location = new System.Drawing.Point(26, 10);
            this.personControl.MaxStreetLines = 2;
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 420);
            this.personControl.TabIndex = 3;
            // 
            // wizardPageSettings
            // 
            this.wizardPageSettings.Controls.Add(this.accountSettingsControl);
            this.wizardPageSettings.Controls.Add(this.optionsControl);
            this.wizardPageSettings.Description = "Configure FedEx settings.";
            this.wizardPageSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageSettings.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSettings.Name = "wizardPageSettings";
            this.wizardPageSettings.Size = new System.Drawing.Size(584, 474);
            this.wizardPageSettings.TabIndex = 0;
            this.wizardPageSettings.Title = "FedEx Settings";
            this.wizardPageSettings.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSettings);
            // 
            // accountSettingsControl
            // 
            this.accountSettingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountSettingsControl.Location = new System.Drawing.Point(26, 147);
            this.accountSettingsControl.Name = "accountSettingsControl";
            this.accountSettingsControl.Size = new System.Drawing.Size(326, 201);
            this.accountSettingsControl.TabIndex = 1;
            // 
            // optionsControl
            // 
            this.optionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionsControl.Location = new System.Drawing.Point(23, 6);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(409, 190);
            this.optionsControl.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Font = new System.Drawing.Font("Tahoma", 8F);
            this.label1.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label1.Location = new System.Drawing.Point(3, 545);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 33);
            this.label1.TabIndex = 6;
            this.label1.Text = "The FedEx service marks are owned by Federal Express Corporation and are used by " +
    "permission";
            // 
            // wizardPage1
            // 
            this.wizardPage1.Description = "The description of the page.";
            this.wizardPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage1.Location = new System.Drawing.Point(0, 0);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(584, 474);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Title = "Wizard page 4.";
            // 
            // accountNumber
            // 
            this.accountNumber.Location = new System.Drawing.Point(23, 77);
            this.fieldLengthProvider.SetMaxLengthSource(this.accountNumber, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExAccountNumber);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.Size = new System.Drawing.Size(217, 21);
            this.accountNumber.TabIndex = 5;
            // 
            // labelFedExAccount
            // 
            this.labelFedExAccount.AutoSize = true;
            this.labelFedExAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFedExAccount.Location = new System.Drawing.Point(20, 61);
            this.labelFedExAccount.Name = "labelFedExAccount";
            this.labelFedExAccount.Size = new System.Drawing.Size(197, 13);
            this.labelFedExAccount.TabIndex = 3;
            this.labelFedExAccount.Text = "Enter your FedEx account number";
            // 
            // linkGetAccount
            // 
            this.linkGetAccount.AutoSize = true;
            this.linkGetAccount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkGetAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkGetAccount.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkGetAccount.Location = new System.Drawing.Point(345, 80);
            this.linkGetAccount.Name = "linkGetAccount";
            this.linkGetAccount.Size = new System.Drawing.Size(55, 13);
            this.linkGetAccount.TabIndex = 12;
            this.linkGetAccount.Text = "click here.";
            this.linkGetAccount.Click += new System.EventHandler(this.OnClickLinkFedEx);
            // 
            // labelGetAcount
            // 
            this.labelGetAcount.AutoSize = true;
            this.labelGetAcount.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelGetAcount.Location = new System.Drawing.Point(246, 80);
            this.labelGetAcount.Name = "labelGetAcount";
            this.labelGetAcount.Size = new System.Drawing.Size(108, 13);
            this.labelGetAcount.TabIndex = 11;
            this.labelGetAcount.Text = "To open an account  ";
            // 
            // FedExSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 581);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FedExSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageInitial,
            this.wizardPageContactInfo,
            this.wizardPageSettings});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "FedEx Setup Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Controls.SetChildIndex(this.label1, 0);
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
            this.wizardPageInitial.ResumeLayout(false);
            this.wizardPageInitial.PerformLayout();
            this.wizardPageContactInfo.ResumeLayout(false);
            this.wizardPageSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageInitial;
        private System.Windows.Forms.Label labelInfo1;
        private ShipWorks.UI.Wizard.WizardPage wizardPageContactInfo;
        private ShipWorks.Data.Controls.AutofillPersonControl personControl;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSettings;
        private FedExOptionsControl optionsControl;
        private System.Windows.Forms.Label label1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private FedExAccountSettingsControl accountSettingsControl;
        private UI.Wizard.WizardPage wizardPage1;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label labelFedExAccount;
        private System.Windows.Forms.Label linkGetAccount;
        private System.Windows.Forms.Label labelGetAcount;
    }
}
