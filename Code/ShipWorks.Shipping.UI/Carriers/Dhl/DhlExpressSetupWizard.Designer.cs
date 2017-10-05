namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    partial class DhlExpressSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DhlExpressSetupWizard));
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.openAccountLink = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.labelFedExAccount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.contactInformation = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageWelcome.SuspendLayout();
            this.wizardPageContactInfo.SuspendLayout();
            this.SuspendLayout();
            //
            // next
            //
            this.next.Location = new System.Drawing.Point(389, 509);
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
            this.mainPanel.Controls.Add(this.wizardPageWelcome);
            this.mainPanel.Size = new System.Drawing.Size(557, 437);
            //
            // etchBottom
            //
            this.etchBottom.Location = new System.Drawing.Point(0, 499);
            this.etchBottom.Size = new System.Drawing.Size(561, 2);
            //
            // pictureBox
            //
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(389, 3);
            this.pictureBox.Size = new System.Drawing.Size(165, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            //
            // topPanel
            //
            this.topPanel.Size = new System.Drawing.Size(557, 56);
            //
            // wizardPageWelcome
            //
            this.wizardPageWelcome.Controls.Add(this.openAccountLink);
            this.wizardPageWelcome.Controls.Add(this.label2);
            this.wizardPageWelcome.Controls.Add(this.accountNumber);
            this.wizardPageWelcome.Controls.Add(this.labelFedExAccount);
            this.wizardPageWelcome.Controls.Add(this.label1);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your DHL Express account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(557, 437);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup DHL Express Shipping";
            //
            // openAccountLink
            //
            this.openAccountLink.AutoSize = true;
            this.openAccountLink.Location = new System.Drawing.Point(123, 87);
            this.openAccountLink.Name = "openAccountLink";
            this.openAccountLink.Size = new System.Drawing.Size(55, 13);
            this.openAccountLink.TabIndex = 16;
            this.openAccountLink.TabStop = true;
            this.openAccountLink.Text = "click here.";
            this.openAccountLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnOpenAccountLinkClicked);
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "To open an account, ";
            //
            // accountNumber
            //
            this.accountNumber.Location = new System.Drawing.Point(23, 63);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.Size = new System.Drawing.Size(230, 21);
            this.accountNumber.TabIndex = 13;
            //
            // labelFedExAccount
            //
            this.labelFedExAccount.AutoSize = true;
            this.labelFedExAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFedExAccount.Location = new System.Drawing.Point(19, 47);
            this.labelFedExAccount.Name = "labelFedExAccount";
            this.labelFedExAccount.Size = new System.Drawing.Size(233, 13);
            this.labelFedExAccount.TabIndex = 14;
            this.labelFedExAccount.Text = "Enter your DHL Express account number";
            //
            // label1
            //
            this.label1.Location = new System.Drawing.Point(21, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(508, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "This wizard will assist you in configuring your DHL Express account for use with " +
    "ShipWorks. This enables you to begin shipping, tracking, and printing DHL Expres" +
    "s labels with your DHL Express account.";
            //
            // wizardPageContactInfo
            //
            this.wizardPageContactInfo.Controls.Add(this.contactInformation);
            this.wizardPageContactInfo.Description = "Enter your DHL Express contact information";
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
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company)
            | ShipWorks.Data.Controls.PersonFields.Street)
            | ShipWorks.Data.Controls.PersonFields.City)
            | ShipWorks.Data.Controls.PersonFields.State)
            | ShipWorks.Data.Controls.PersonFields.Postal)
            | ShipWorks.Data.Controls.PersonFields.Country)
            | ShipWorks.Data.Controls.PersonFields.Email)
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.contactInformation.Size = new System.Drawing.Size(355, 381);
            this.contactInformation.TabIndex = 1;
            //
            // DhlExpressSetupWizard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 544);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DhlExpressSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageContactInfo});
            this.Text = "DHL Express Setup Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageWelcome.ResumeLayout(false);
            this.wizardPageWelcome.PerformLayout();
            this.wizardPageContactInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private ShipWorks.UI.Wizard.WizardPage wizardPageContactInfo;
        private Data.Controls.AutofillPersonControl contactInformation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label labelFedExAccount;
        private System.Windows.Forms.LinkLabel openAccountLink;
        private System.Windows.Forms.Label label2;
    }
}