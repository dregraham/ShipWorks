namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    partial class DhlEcommerceSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DhlEcommerceSetupWizard));
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.linkDhlWebsite = new System.Windows.Forms.Label();
            this.linkDhlECommerceConfigArticle = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelInfo1a = new System.Windows.Forms.Label();
            this.labelInfo1b = new System.Windows.Forms.Label();
            this.wizardPageCredentials = new ShipWorks.UI.Wizard.WizardPage();
            this.accountDescription = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.ancillaryEndorsement = new System.Windows.Forms.ComboBox();
            this.labelAncillaryEndorsement = new System.Windows.Forms.Label();
            this.soldTo = new ShipWorks.UI.Controls.NumericTextBox();
            this.labelSoldTo = new System.Windows.Forms.Label();
            this.labelDistributionCenter = new System.Windows.Forms.Label();
            this.distributionCenters = new System.Windows.Forms.ComboBox();
            this.labelPickupNumber = new System.Windows.Forms.Label();
            this.clientId = new System.Windows.Forms.TextBox();
            this.pickupNumber = new ShipWorks.UI.Controls.NumericTextBox();
            this.apiSecret = new System.Windows.Forms.TextBox();
            this.labelApiSecret = new System.Windows.Forms.Label();
            this.labelClientId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPageOptions = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControl = new ShipWorks.Shipping.UI.Carriers.DhlEcommerce.DhlEcommerceOptionsControl();
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
            this.next.Location = new System.Drawing.Point(399, 509);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(480, 509);
            this.cancel.TabIndex = 2;
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(318, 509);
            this.back.TabIndex = 1;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageWelcome);
            this.mainPanel.Size = new System.Drawing.Size(567, 437);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 499);
            this.etchBottom.Size = new System.Drawing.Size(571, 2);
            this.etchBottom.TabIndex = 3;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(347, 3);
            this.pictureBox.Size = new System.Drawing.Size(217, 50);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(567, 56);
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.linkDhlWebsite);
            this.wizardPageWelcome.Controls.Add(this.linkDhlECommerceConfigArticle);
            this.wizardPageWelcome.Controls.Add(this.labelInfo2);
            this.wizardPageWelcome.Controls.Add(this.labelInfo1a);
            this.wizardPageWelcome.Controls.Add(this.labelInfo1b);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your DHL eCommerce account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(567, 437);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup DHL eCommerce Shipping";
            // 
            // linkDhlWebsite
            // 
            this.linkDhlWebsite.AutoSize = true;
            this.linkDhlWebsite.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkDhlWebsite.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkDhlWebsite.ForeColor = System.Drawing.Color.Blue;
            this.linkDhlWebsite.Location = new System.Drawing.Point(180, 80);
            this.linkDhlWebsite.Name = "linkDhlWebsite";
            this.linkDhlWebsite.Size = new System.Drawing.Size(76, 13);
            this.linkDhlWebsite.TabIndex = 5;
            this.linkDhlWebsite.Text = "www.dhl.com.";
            this.linkDhlWebsite.Click += new System.EventHandler(this.OnLinkDhlWebsite);
            // 
            // linkDhlECommerceConfigArticle
            // 
            this.linkDhlECommerceConfigArticle.AutoSize = true;
            this.linkDhlECommerceConfigArticle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkDhlECommerceConfigArticle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkDhlECommerceConfigArticle.ForeColor = System.Drawing.Color.Blue;
            this.linkDhlECommerceConfigArticle.Location = new System.Drawing.Point(155, 9);
            this.linkDhlECommerceConfigArticle.Name = "linkDhlECommerceConfigArticle";
            this.linkDhlECommerceConfigArticle.Size = new System.Drawing.Size(207, 13);
            this.linkDhlECommerceConfigArticle.TabIndex = 5;
            this.linkDhlECommerceConfigArticle.Text = "configuring your DHL eCommerce account";
            this.linkDhlECommerceConfigArticle.Click += new System.EventHandler(this.OnLinkDhlECommerceConfigArticle);
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(20, 67);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(410, 30);
            this.labelInfo2.TabIndex = 4;
            this.labelInfo2.Text = "You must have a DHL eCommerce account before continuing. To get a DHL eCommerce a" +
    "ccount, please visit";
            // 
            // labelInfo1a
            // 
            this.labelInfo1a.Location = new System.Drawing.Point(20, 9);
            this.labelInfo1a.Name = "labelInfo1a";
            this.labelInfo1a.Size = new System.Drawing.Size(140, 13);
            this.labelInfo1a.TabIndex = 3;
            this.labelInfo1a.Text = "This wizard will assist you in ";
            // 
            // labelInfo1b
            // 
            this.labelInfo1b.Location = new System.Drawing.Point(20, 22);
            this.labelInfo1b.Name = "labelInfo1b";
            this.labelInfo1b.Size = new System.Drawing.Size(374, 47);
            this.labelInfo1b.TabIndex = 3;
            this.labelInfo1b.Text = "for use with ShipWorks. This enables you to begin shipping, tracking, and printin" +
    "g labels with your DHL eCommerce account.";
            // 
            // wizardPageCredentials
            // 
            this.wizardPageCredentials.Controls.Add(this.accountDescription);
            this.wizardPageCredentials.Controls.Add(this.labelDescription);
            this.wizardPageCredentials.Controls.Add(this.ancillaryEndorsement);
            this.wizardPageCredentials.Controls.Add(this.labelAncillaryEndorsement);
            this.wizardPageCredentials.Controls.Add(this.soldTo);
            this.wizardPageCredentials.Controls.Add(this.labelSoldTo);
            this.wizardPageCredentials.Controls.Add(this.labelDistributionCenter);
            this.wizardPageCredentials.Controls.Add(this.distributionCenters);
            this.wizardPageCredentials.Controls.Add(this.labelPickupNumber);
            this.wizardPageCredentials.Controls.Add(this.clientId);
            this.wizardPageCredentials.Controls.Add(this.pickupNumber);
            this.wizardPageCredentials.Controls.Add(this.apiSecret);
            this.wizardPageCredentials.Controls.Add(this.labelApiSecret);
            this.wizardPageCredentials.Controls.Add(this.labelClientId);
            this.wizardPageCredentials.Controls.Add(this.label1);
            this.wizardPageCredentials.Description = "Enter your DHL eCommerce account credentials.";
            this.wizardPageCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageCredentials.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCredentials.Name = "wizardPageCredentials";
            this.wizardPageCredentials.Size = new System.Drawing.Size(567, 437);
            this.wizardPageCredentials.TabIndex = 0;
            this.wizardPageCredentials.Title = "DHL eCommerce Credentials";
            // 
            // accountDescription
            // 
            this.accountDescription.Location = new System.Drawing.Point(156, 195);
            this.accountDescription.MaxLength = 50;
            this.accountDescription.Name = "accountDescription";
            this.accountDescription.Size = new System.Drawing.Size(162, 21);
            this.accountDescription.TabIndex = 14;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(86, 198);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 13;
            this.labelDescription.Text = "Description:";
            // 
            // ancillaryEndorsement
            // 
            this.ancillaryEndorsement.FormattingEnabled = true;
            this.ancillaryEndorsement.Location = new System.Drawing.Point(156, 141);
            this.ancillaryEndorsement.Name = "ancillaryEndorsement";
            this.ancillaryEndorsement.Size = new System.Drawing.Size(162, 21);
            this.ancillaryEndorsement.TabIndex = 10;
            // 
            // labelAncillaryEndorsement
            // 
            this.labelAncillaryEndorsement.AutoSize = true;
            this.labelAncillaryEndorsement.Location = new System.Drawing.Point(33, 144);
            this.labelAncillaryEndorsement.Name = "labelAncillaryEndorsement";
            this.labelAncillaryEndorsement.Size = new System.Drawing.Size(117, 13);
            this.labelAncillaryEndorsement.TabIndex = 9;
            this.labelAncillaryEndorsement.Text = "Ancillary Endorsement:";
            // 
            // soldTo
            // 
            this.soldTo.Location = new System.Drawing.Point(156, 114);
            this.soldTo.Name = "soldTo";
            this.soldTo.Size = new System.Drawing.Size(162, 21);
            this.soldTo.TabIndex = 8;
            // 
            // labelSoldTo
            // 
            this.labelSoldTo.AutoSize = true;
            this.labelSoldTo.Location = new System.Drawing.Point(22, 117);
            this.labelSoldTo.Name = "labelSoldTo";
            this.labelSoldTo.Size = new System.Drawing.Size(128, 13);
            this.labelSoldTo.TabIndex = 7;
            this.labelSoldTo.Text = "Sold To Account Number:";
            // 
            // labelDistributionCenter
            // 
            this.labelDistributionCenter.AutoSize = true;
            this.labelDistributionCenter.Location = new System.Drawing.Point(49, 171);
            this.labelDistributionCenter.Name = "labelDistributionCenter";
            this.labelDistributionCenter.Size = new System.Drawing.Size(101, 13);
            this.labelDistributionCenter.TabIndex = 11;
            this.labelDistributionCenter.Text = "Distribution Center:";
            // 
            // distributionCenters
            // 
            this.distributionCenters.FormattingEnabled = true;
            this.distributionCenters.Location = new System.Drawing.Point(156, 168);
            this.distributionCenters.Name = "distributionCenters";
            this.distributionCenters.Size = new System.Drawing.Size(162, 21);
            this.distributionCenters.TabIndex = 12;
            // 
            // labelPickupNumber
            // 
            this.labelPickupNumber.AutoSize = true;
            this.labelPickupNumber.Location = new System.Drawing.Point(27, 90);
            this.labelPickupNumber.Name = "labelPickupNumber";
            this.labelPickupNumber.Size = new System.Drawing.Size(123, 13);
            this.labelPickupNumber.TabIndex = 5;
            this.labelPickupNumber.Text = "Pickup Account Number:";
            // 
            // clientId
            // 
            this.clientId.Location = new System.Drawing.Point(156, 31);
            this.clientId.MaxLength = 50;
            this.clientId.Name = "clientId";
            this.clientId.Size = new System.Drawing.Size(162, 21);
            this.clientId.TabIndex = 2;
            // 
            // pickupNumber
            // 
            this.pickupNumber.Location = new System.Drawing.Point(156, 87);
            this.pickupNumber.Name = "pickupNumber";
            this.pickupNumber.Size = new System.Drawing.Size(162, 21);
            this.pickupNumber.TabIndex = 6;
            // 
            // apiSecret
            // 
            this.apiSecret.Location = new System.Drawing.Point(156, 58);
            this.apiSecret.MaxLength = 50;
            this.apiSecret.Name = "apiSecret";
            this.apiSecret.Size = new System.Drawing.Size(162, 21);
            this.apiSecret.TabIndex = 4;
            this.apiSecret.UseSystemPasswordChar = true;
            // 
            // labelApiSecret
            // 
            this.labelApiSecret.AutoSize = true;
            this.labelApiSecret.Location = new System.Drawing.Point(88, 61);
            this.labelApiSecret.Name = "labelApiSecret";
            this.labelApiSecret.Size = new System.Drawing.Size(62, 13);
            this.labelApiSecret.TabIndex = 3;
            this.labelApiSecret.Text = "API Secret:";
            // 
            // labelClientId
            // 
            this.labelClientId.AutoSize = true;
            this.labelClientId.Location = new System.Drawing.Point(98, 34);
            this.labelClientId.Name = "labelClientId";
            this.labelClientId.Size = new System.Drawing.Size(52, 13);
            this.labelClientId.TabIndex = 1;
            this.labelClientId.Text = "Client ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(335, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the credentials provided by DHL eCommerce for your account.";
            // 
            // wizardPageOptions
            // 
            this.wizardPageOptions.Controls.Add(this.optionsControl);
            this.wizardPageOptions.Description = "Configure DHL eCommerce settings.";
            this.wizardPageOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOptions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOptions.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOptions.Name = "wizardPageOptions";
            this.wizardPageOptions.Size = new System.Drawing.Size(567, 437);
            this.wizardPageOptions.TabIndex = 0;
            this.wizardPageOptions.Title = "DHL eCommerce Settings";
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
            this.wizardPageContactInfo.Description = "Enter your DHL eCommerce contact information";
            this.wizardPageContactInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageContactInfo.Location = new System.Drawing.Point(0, 0);
            this.wizardPageContactInfo.Name = "wizardPageContactInfo";
            this.wizardPageContactInfo.Size = new System.Drawing.Size(567, 437);
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
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal)));
            this.contactInformation.Size = new System.Drawing.Size(355, 381);
            this.contactInformation.TabIndex = 0;
            // 
            // DhlEcommerceSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 544);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LastPageCancelable = true;
            this.Name = "DhlEcommerceSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageCredentials,
            this.wizardPageContactInfo,
            this.wizardPageOptions});
            this.Text = "DHL eCommerce Setup Wizard";
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
        private System.Windows.Forms.Label linkDhlECommerceConfigArticle;
        private System.Windows.Forms.Label linkDhlWebsite;
        private System.Windows.Forms.Label labelInfo2;
        private System.Windows.Forms.Label labelInfo1a;
        private System.Windows.Forms.Label labelInfo1b;
        private ShipWorks.UI.Wizard.WizardPage wizardPageCredentials;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox apiSecret;
        private System.Windows.Forms.Label labelApiSecret;
        private System.Windows.Forms.Label labelClientId;
        private ShipWorks.UI.Wizard.WizardPage wizardPageOptions;
        private DhlEcommerceOptionsControl optionsControl;
        private ShipWorks.UI.Wizard.WizardPage wizardPageContactInfo;
        private Data.Controls.AutofillPersonControl contactInformation;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private ShipWorks.UI.Controls.NumericTextBox pickupNumber;
        private System.Windows.Forms.Label labelDistributionCenter;
        private System.Windows.Forms.Label labelPickupNumber;
        private System.Windows.Forms.TextBox clientId;
        private System.Windows.Forms.ComboBox distributionCenters;
        private ShipWorks.UI.Controls.NumericTextBox soldTo;
        private System.Windows.Forms.Label labelSoldTo;
        private System.Windows.Forms.ComboBox ancillaryEndorsement;
        private System.Windows.Forms.Label labelAncillaryEndorsement;
        private System.Windows.Forms.TextBox accountDescription;
        private System.Windows.Forms.Label labelDescription;
    }
}