namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    partial class AmazonSFPShipmentSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AmazonSFPShipmentSetupWizard));
            this.Instructions = new System.Windows.Forms.Label();
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.chkTermsAndConditions = new System.Windows.Forms.CheckBox();
            this.labelTermsAndConditions = new System.Windows.Forms.Label();
            this.txtTermsAndConditions = new System.Windows.Forms.RichTextBox();
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
            this.next.Location = new System.Drawing.Point(444, 509);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(525, 509);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(363, 509);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageWelcome);
            this.mainPanel.Size = new System.Drawing.Size(612, 437);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 499);
            this.etchBottom.Size = new System.Drawing.Size(616, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(461, 3);
            this.pictureBox.Size = new System.Drawing.Size(148, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(612, 56);
            // 
            // Instructions
            // 
            this.Instructions.AutoSize = true;
            this.Instructions.Location = new System.Drawing.Point(20, 10);
            this.Instructions.Name = "Instructions";
            this.Instructions.Size = new System.Drawing.Size(444, 26);
            this.Instructions.TabIndex = 0;
            this.Instructions.Text = "This wizard will assist you in configuring your Amazon account for use with ShipW" +
    "orks. This \r\nenables you to begin shipping, tracking, and printing labels with y" +
    "our Amazon account. ";
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.chkTermsAndConditions);
            this.wizardPageWelcome.Controls.Add(this.labelTermsAndConditions);
            this.wizardPageWelcome.Controls.Add(this.txtTermsAndConditions);
            this.wizardPageWelcome.Controls.Add(this.Instructions);
            this.wizardPageWelcome.Description = "Setup ShipWorks to work with your Amazon Buy Shipping API account.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(612, 437);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "Setup Amazon Buy Shipping API";
            this.wizardPageWelcome.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextWelcome);
            // 
            // chkTermsAndConditions
            // 
            this.chkTermsAndConditions.AutoSize = true;
            this.chkTermsAndConditions.Location = new System.Drawing.Point(60, 389);
            this.chkTermsAndConditions.Name = "chkTermsAndConditions";
            this.chkTermsAndConditions.Size = new System.Drawing.Size(223, 17);
            this.chkTermsAndConditions.TabIndex = 3;
            this.chkTermsAndConditions.Text = "I accept the above Terms and Conditions";
            this.chkTermsAndConditions.UseVisualStyleBackColor = true;
            // 
            // labelTermsAndConditions
            // 
            this.labelTermsAndConditions.AutoSize = true;
            this.labelTermsAndConditions.Location = new System.Drawing.Point(20, 48);
            this.labelTermsAndConditions.Name = "labelTermsAndConditions";
            this.labelTermsAndConditions.Size = new System.Drawing.Size(440, 13);
            this.labelTermsAndConditions.TabIndex = 2;
            this.labelTermsAndConditions.Text = "Please review the Terms and Conditions below and select the checkbox below to con" +
    "tinue.";
            // 
            // txtTermsAndConditions
            // 
            this.txtTermsAndConditions.Location = new System.Drawing.Point(60, 77);
            this.txtTermsAndConditions.Name = "txtTermsAndConditions";
            this.txtTermsAndConditions.ReadOnly = true;
            this.txtTermsAndConditions.Size = new System.Drawing.Size(474, 297);
            this.txtTermsAndConditions.TabIndex = 1;
            this.txtTermsAndConditions.Text = "";
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
            // AmazonSFPShipmentSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 544);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AmazonSFPShipmentSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageContactInfo});
            this.Text = "Amazon Buy Shipping API Setup Wizard";
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
        private System.Windows.Forms.Label Instructions;
        private System.Windows.Forms.RichTextBox txtTermsAndConditions;
        private System.Windows.Forms.Label labelTermsAndConditions;
        private System.Windows.Forms.CheckBox chkTermsAndConditions;
    }
}