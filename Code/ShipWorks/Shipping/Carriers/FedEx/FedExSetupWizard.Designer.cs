﻿namespace ShipWorks.Shipping.Carriers.FedEx
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
            this.linkFedExWebsite = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.labelAccount = new System.Windows.Forms.Label();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.labelFedExAccount = new System.Windows.Forms.Label();
            this.wizardPageSettings = new ShipWorks.UI.Wizard.WizardPage();
            this.accountSettingsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExAccountSettingsControl();
            this.optionsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExOptionsControl();
            this.label1 = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
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
            this.mainPanel.Controls.Add(this.wizardPageInitial);
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
            this.pictureBox.Size = new System.Drawing.Size(136, 49);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(584, 56);
            // 
            // wizardPageInitial
            // 
            this.wizardPageInitial.Controls.Add(this.linkFedExWebsite);
            this.wizardPageInitial.Controls.Add(this.labelInfo2);
            this.wizardPageInitial.Controls.Add(this.labelInfo1);
            this.wizardPageInitial.Description = "Setup ShipWorks to work with your FedEx account.";
            this.wizardPageInitial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageInitial.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageInitial.Location = new System.Drawing.Point(0, 0);
            this.wizardPageInitial.Name = "wizardPageInitial";
            this.wizardPageInitial.Size = new System.Drawing.Size(584, 474);
            this.wizardPageInitial.TabIndex = 0;
            this.wizardPageInitial.Title = "Setup FedEx® Shipping";
            // 
            // linkFedExWebsite
            // 
            this.linkFedExWebsite.AutoSize = true;
            this.linkFedExWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkFedExWebsite.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkFedExWebsite.ForeColor = System.Drawing.Color.Blue;
            this.linkFedExWebsite.Location = new System.Drawing.Point(128, 79);
            this.linkFedExWebsite.Name = "linkFedExWebsite";
            this.linkFedExWebsite.Size = new System.Drawing.Size(62, 13);
            this.linkFedExWebsite.TabIndex = 2;
            this.linkFedExWebsite.Text = "fedex.com.";
            this.linkFedExWebsite.Click += new System.EventHandler(this.OnClickLinkFedEx);
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(21, 66);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(410, 30);
            this.labelInfo2.TabIndex = 1;
            this.labelInfo2.Text = "You must have a FedEx shipping account before continuing. To get a FedEx account," +
    " please go to";
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
            this.wizardPageContactInfo.Controls.Add(this.labelAccount);
            this.wizardPageContactInfo.Controls.Add(this.personControl);
            this.wizardPageContactInfo.Controls.Add(this.accountNumber);
            this.wizardPageContactInfo.Controls.Add(this.labelFedExAccount);
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
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(50, 31);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 1;
            this.labelAccount.Text = "Account:";
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
            this.personControl.Location = new System.Drawing.Point(30, 51);
            this.personControl.MaxStreetLines = 2;
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 420);
            this.personControl.TabIndex = 3;
            // 
            // accountNumber
            // 
            this.accountNumber.Location = new System.Drawing.Point(107, 28);
            this.fieldLengthProvider.SetMaxLengthSource(this.accountNumber, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExAccountNumber);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.Size = new System.Drawing.Size(165, 21);
            this.accountNumber.TabIndex = 2;
            // 
            // labelFedExAccount
            // 
            this.labelFedExAccount.AutoSize = true;
            this.labelFedExAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFedExAccount.Location = new System.Drawing.Point(31, 10);
            this.labelFedExAccount.Name = "labelFedExAccount";
            this.labelFedExAccount.Size = new System.Drawing.Size(136, 13);
            this.labelFedExAccount.TabIndex = 0;
            this.labelFedExAccount.Text = "FedEx Account Number";
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
            this.wizardPageContactInfo.PerformLayout();
            this.wizardPageSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageInitial;
        private System.Windows.Forms.Label labelInfo1;
        private System.Windows.Forms.Label linkFedExWebsite;
        private System.Windows.Forms.Label labelInfo2;
        private ShipWorks.UI.Wizard.WizardPage wizardPageContactInfo;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label labelFedExAccount;
        private System.Windows.Forms.Label labelAccount;
        private ShipWorks.Data.Controls.PersonControl personControl;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSettings;
        private FedExOptionsControl optionsControl;
        private System.Windows.Forms.Label label1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private FedExAccountSettingsControl accountSettingsControl;
        private UI.Wizard.WizardPage wizardPage1;
    }
}
