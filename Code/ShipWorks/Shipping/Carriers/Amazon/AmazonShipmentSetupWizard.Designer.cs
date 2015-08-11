namespace ShipWorks.Shipping.Carriers.Amazon
{
    partial class AmazonShipmentSetupWizard
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
            this.wizardPageCredentials = new ShipWorks.UI.Wizard.WizardPage();
            this.merchantId = new System.Windows.Forms.TextBox();
            this.authToken = new System.Windows.Forms.TextBox();
            this.labelAuthToken = new System.Windows.Forms.Label();
            this.labelMerchantId = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.contactInformation = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageCredentials.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.wizardPageContactInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.next.Location = new System.Drawing.Point(389, 509);
            this.next.Text = "Finish";
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
            this.mainPanel.Controls.Add(this.wizardPageContactInfo);
            this.mainPanel.Size = new System.Drawing.Size(557, 437);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 499);
            this.etchBottom.Size = new System.Drawing.Size(561, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.amazon_large;
            this.pictureBox.Location = new System.Drawing.Point(389, 3);
            this.pictureBox.Size = new System.Drawing.Size(165, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(557, 56);
            // 
            // wizardPageCredentials
            // 
            this.wizardPageCredentials.Controls.Add(this.merchantId);
            this.wizardPageCredentials.Controls.Add(this.authToken);
            this.wizardPageCredentials.Controls.Add(this.labelAuthToken);
            this.wizardPageCredentials.Controls.Add(this.labelMerchantId);
            this.wizardPageCredentials.Controls.Add(this.label1);
            this.wizardPageCredentials.Description = "Enter your Amazon Seller ID and Auth Token.";
            this.wizardPageCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCredentials.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageCredentials.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCredentials.Name = "wizardPageCredentials";
            this.wizardPageCredentials.Size = new System.Drawing.Size(557, 437);
            this.wizardPageCredentials.TabIndex = 0;
            this.wizardPageCredentials.Title = "Amazon Credentials";
            this.wizardPageCredentials.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnNextStepCredentials);
            // 
            // merchantId
            // 
            this.merchantId.Location = new System.Drawing.Point(118, 33);
            this.merchantId.Name = "merchantId";
            this.merchantId.Size = new System.Drawing.Size(162, 21);
            this.merchantId.TabIndex = 167;
            // 
            // authToken
            // 
            this.authToken.Location = new System.Drawing.Point(118, 59);
            this.authToken.MaxLength = 50;
            this.authToken.Name = "authToken";
            this.authToken.Size = new System.Drawing.Size(162, 21);
            this.authToken.TabIndex = 168;
            // 
            // labelAuthToken
            // 
            this.labelAuthToken.AutoSize = true;
            this.labelAuthToken.Location = new System.Drawing.Point(46, 62);
            this.labelAuthToken.Name = "labelAuthToken";
            this.labelAuthToken.Size = new System.Drawing.Size(66, 13);
            this.labelAuthToken.TabIndex = 170;
            this.labelAuthToken.Text = "Auth Token:";
            // 
            // labelMerchantId
            // 
            this.labelMerchantId.AutoSize = true;
            this.labelMerchantId.Location = new System.Drawing.Point(62, 36);
            this.labelMerchantId.Name = "labelMerchantId";
            this.labelMerchantId.Size = new System.Drawing.Size(50, 13);
            this.labelMerchantId.TabIndex = 169;
            this.labelMerchantId.Text = "Seller ID:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(279, 13);
            this.label1.TabIndex = 166;
            this.label1.Text = "Enter the Seller ID and Auth Token provided by Amazon.";
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.labelInfo1);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your Amazon account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(557, 437);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup Amazon Shipping";
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(20, 9);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(454, 47);
            this.labelInfo1.TabIndex = 4;
            this.labelInfo1.Text = "This wizard will assist you in configuring your Amazon account for use with ShipW" +
    "orks. This enables you to begin shipping, tracking, and printing labels with you" +
    "r Amazon account.";
            // 
            // wizardPageContactInfo
            // 
            this.wizardPageContactInfo.Controls.Add(this.contactInformation);
            this.wizardPageContactInfo.Description = "Enter your Amazon contact information";
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
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal)));
            this.contactInformation.Size = new System.Drawing.Size(355, 381);
            this.contactInformation.TabIndex = 1;
            // 
            // AmazonShipmentSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 544);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AmazonShipmentSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageCredentials,
            this.wizardPageContactInfo});
            this.Text = "Amazon Setup Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageCredentials.ResumeLayout(false);
            this.wizardPageCredentials.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.wizardPageContactInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageCredentials;
        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private System.Windows.Forms.TextBox merchantId;
        private System.Windows.Forms.TextBox authToken;
        private System.Windows.Forms.Label labelAuthToken;
        private System.Windows.Forms.Label labelMerchantId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelInfo1;
        private UI.Wizard.WizardPage wizardPageContactInfo;
        private Data.Controls.AutofillPersonControl contactInformation;
    }
}