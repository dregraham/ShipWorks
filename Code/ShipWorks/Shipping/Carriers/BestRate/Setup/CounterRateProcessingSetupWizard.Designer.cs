namespace ShipWorks.Shipping.Carriers.BestRate
{
    partial class CounterRateProcessingSetupWizard
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
            this.wizardPageNoAccounts = new ShipWorks.UI.Wizard.WizardPage();
            this.useExistingAccountPanel = new System.Windows.Forms.Panel();
            this.existingAccountRateDifference = new System.Windows.Forms.Label();
            this.existingAccountRateAmount = new System.Windows.Forms.Label();
            this.carrierName = new System.Windows.Forms.Label();
            this.carrierLogo = new System.Windows.Forms.PictureBox();
            this.useExistingAccountHeader = new System.Windows.Forms.Label();
            this.useExistingAccountButton = new System.Windows.Forms.Button();
            this.useExistingAccountDescription = new System.Windows.Forms.Label();
            this.addExistingAccountPanel = new System.Windows.Forms.Panel();
            this.setupExistingAccountHeader = new System.Windows.Forms.Label();
            this.setupExistingAccountButton = new System.Windows.Forms.Button();
            this.setupExistingAccountDescription = new System.Windows.Forms.Label();
            this.setupExistingProvider = new System.Windows.Forms.ComboBox();
            this.setupExistingAccountProviderLabel = new System.Windows.Forms.Label();
            this.counterRateSignUpInformationControl1 = new ShipWorks.Shipping.Carriers.BestRate.Setup.CounterRateSignUpInformationControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageNoAccounts.SuspendLayout();
            this.useExistingAccountPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.carrierLogo)).BeginInit();
            this.addExistingAccountPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.next.Location = new System.Drawing.Point(539, 533);
            this.next.Text = "Finish";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(620, 533);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(458, 533);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageNoAccounts);
            this.mainPanel.Size = new System.Drawing.Size(707, 461);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 523);
            this.etchBottom.Size = new System.Drawing.Size(711, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.box_closed;
            this.pictureBox.Location = new System.Drawing.Point(654, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(707, 56);
            // 
            // wizardPageNoAccounts
            // 
            this.wizardPageNoAccounts.Controls.Add(this.useExistingAccountPanel);
            this.wizardPageNoAccounts.Controls.Add(this.addExistingAccountPanel);
            this.wizardPageNoAccounts.Controls.Add(this.counterRateSignUpInformationControl1);
            this.wizardPageNoAccounts.Description = "You need to create a shipping account to process your shipment.";
            this.wizardPageNoAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageNoAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageNoAccounts.Location = new System.Drawing.Point(0, 0);
            this.wizardPageNoAccounts.Name = "wizardPageNoAccounts";
            this.wizardPageNoAccounts.Size = new System.Drawing.Size(707, 461);
            this.wizardPageNoAccounts.TabIndex = 0;
            this.wizardPageNoAccounts.Title = "Shipping Account";
            // 
            // useExistingAccountPanel
            // 
            this.useExistingAccountPanel.Controls.Add(this.existingAccountRateDifference);
            this.useExistingAccountPanel.Controls.Add(this.existingAccountRateAmount);
            this.useExistingAccountPanel.Controls.Add(this.carrierName);
            this.useExistingAccountPanel.Controls.Add(this.carrierLogo);
            this.useExistingAccountPanel.Controls.Add(this.useExistingAccountHeader);
            this.useExistingAccountPanel.Controls.Add(this.useExistingAccountButton);
            this.useExistingAccountPanel.Controls.Add(this.useExistingAccountDescription);
            this.useExistingAccountPanel.Location = new System.Drawing.Point(23, 279);
            this.useExistingAccountPanel.Name = "useExistingAccountPanel";
            this.useExistingAccountPanel.Size = new System.Drawing.Size(591, 148);
            this.useExistingAccountPanel.TabIndex = 7;
            // 
            // existingAccountRateDifference
            // 
            this.existingAccountRateDifference.AutoSize = true;
            this.existingAccountRateDifference.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.existingAccountRateDifference.Location = new System.Drawing.Point(105, 64);
            this.existingAccountRateDifference.Name = "existingAccountRateDifference";
            this.existingAccountRateDifference.Size = new System.Drawing.Size(70, 13);
            this.existingAccountRateDifference.TabIndex = 9;
            this.existingAccountRateDifference.Text = "($2.37 more)";
            // 
            // existingAccountRateAmount
            // 
            this.existingAccountRateAmount.AutoSize = true;
            this.existingAccountRateAmount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.existingAccountRateAmount.ForeColor = System.Drawing.Color.Red;
            this.existingAccountRateAmount.Location = new System.Drawing.Point(66, 64);
            this.existingAccountRateAmount.Name = "existingAccountRateAmount";
            this.existingAccountRateAmount.Size = new System.Drawing.Size(38, 13);
            this.existingAccountRateAmount.TabIndex = 8;
            this.existingAccountRateAmount.Text = "$7.82";
            // 
            // carrierName
            // 
            this.carrierName.AutoSize = true;
            this.carrierName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.carrierName.Location = new System.Drawing.Point(66, 50);
            this.carrierName.Name = "carrierName";
            this.carrierName.Size = new System.Drawing.Size(40, 13);
            this.carrierName.TabIndex = 7;
            this.carrierName.Text = "FedEx";
            // 
            // carrierLogo
            // 
            this.carrierLogo.Image = global::ShipWorks.Properties.Resources.box_closed16;
            this.carrierLogo.Location = new System.Drawing.Point(36, 44);
            this.carrierLogo.Name = "carrierLogo";
            this.carrierLogo.Size = new System.Drawing.Size(24, 24);
            this.carrierLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.carrierLogo.TabIndex = 6;
            this.carrierLogo.TabStop = false;
            // 
            // useExistingAccountHeader
            // 
            this.useExistingAccountHeader.AutoSize = true;
            this.useExistingAccountHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.useExistingAccountHeader.Location = new System.Drawing.Point(3, 0);
            this.useExistingAccountHeader.Name = "useExistingAccountHeader";
            this.useExistingAccountHeader.Size = new System.Drawing.Size(141, 13);
            this.useExistingAccountHeader.TabIndex = 1;
            this.useExistingAccountHeader.Text = "Use an Existing Account";
            // 
            // useExistingAccountButton
            // 
            this.useExistingAccountButton.Location = new System.Drawing.Point(40, 90);
            this.useExistingAccountButton.Name = "useExistingAccountButton";
            this.useExistingAccountButton.Size = new System.Drawing.Size(150, 23);
            this.useExistingAccountButton.TabIndex = 5;
            this.useExistingAccountButton.Text = "Use my existing account";
            this.useExistingAccountButton.UseVisualStyleBackColor = true;
            // 
            // useExistingAccountDescription
            // 
            this.useExistingAccountDescription.AutoSize = true;
            this.useExistingAccountDescription.Location = new System.Drawing.Point(20, 22);
            this.useExistingAccountDescription.Name = "useExistingAccountDescription";
            this.useExistingAccountDescription.Size = new System.Drawing.Size(552, 13);
            this.useExistingAccountDescription.TabIndex = 2;
            this.useExistingAccountDescription.Text = "If you\'d rather not sign up right now, your {ProviderName} account {AccountDescri" +
    "ption} had the next best rate:";
            // 
            // addExistingAccountPanel
            // 
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountHeader);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountButton);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountDescription);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingProvider);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountProviderLabel);
            this.addExistingAccountPanel.Location = new System.Drawing.Point(23, 185);
            this.addExistingAccountPanel.Name = "addExistingAccountPanel";
            this.addExistingAccountPanel.Size = new System.Drawing.Size(591, 88);
            this.addExistingAccountPanel.TabIndex = 6;
            // 
            // setupExistingAccountHeader
            // 
            this.setupExistingAccountHeader.AutoSize = true;
            this.setupExistingAccountHeader.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.setupExistingAccountHeader.Location = new System.Drawing.Point(3, 0);
            this.setupExistingAccountHeader.Name = "setupExistingAccountHeader";
            this.setupExistingAccountHeader.Size = new System.Drawing.Size(141, 13);
            this.setupExistingAccountHeader.TabIndex = 1;
            this.setupExistingAccountHeader.Text = "Use an Existing Account";
            // 
            // setupExistingAccountButton
            // 
            this.setupExistingAccountButton.Location = new System.Drawing.Point(303, 47);
            this.setupExistingAccountButton.Name = "setupExistingAccountButton";
            this.setupExistingAccountButton.Size = new System.Drawing.Size(99, 23);
            this.setupExistingAccountButton.TabIndex = 5;
            this.setupExistingAccountButton.Text = "Add my account";
            this.setupExistingAccountButton.UseVisualStyleBackColor = true;
            // 
            // setupExistingAccountDescription
            // 
            this.setupExistingAccountDescription.AutoSize = true;
            this.setupExistingAccountDescription.Location = new System.Drawing.Point(20, 22);
            this.setupExistingAccountDescription.Name = "setupExistingAccountDescription";
            this.setupExistingAccountDescription.Size = new System.Drawing.Size(390, 13);
            this.setupExistingAccountDescription.TabIndex = 2;
            this.setupExistingAccountDescription.Text = "If you already have a shipping account, you can have ShipWorks use it instead:";
            // 
            // setupExistingProvider
            // 
            this.setupExistingProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.setupExistingProvider.FormattingEnabled = true;
            this.setupExistingProvider.Location = new System.Drawing.Point(136, 48);
            this.setupExistingProvider.Name = "setupExistingProvider";
            this.setupExistingProvider.Size = new System.Drawing.Size(161, 21);
            this.setupExistingProvider.TabIndex = 4;
            // 
            // setupExistingAccountProviderLabel
            // 
            this.setupExistingAccountProviderLabel.AutoSize = true;
            this.setupExistingAccountProviderLabel.Location = new System.Drawing.Point(37, 51);
            this.setupExistingAccountProviderLabel.Name = "setupExistingAccountProviderLabel";
            this.setupExistingAccountProviderLabel.Size = new System.Drawing.Size(93, 13);
            this.setupExistingAccountProviderLabel.TabIndex = 3;
            this.setupExistingAccountProviderLabel.Text = "Account Provider:";
            // 
            // counterRateSignUpInformationControl1
            // 
            this.counterRateSignUpInformationControl1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.counterRateSignUpInformationControl1.Location = new System.Drawing.Point(23, 3);
            this.counterRateSignUpInformationControl1.Name = "counterRateSignUpInformationControl1";
            this.counterRateSignUpInformationControl1.Size = new System.Drawing.Size(463, 191);
            this.counterRateSignUpInformationControl1.TabIndex = 0;
            // 
            // CounterRateProcessingSetupWizard
            // 
            this.AcceptButton = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(707, 568);
            this.Name = "CounterRateProcessingSetupWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageNoAccounts});
            this.Text = "ShipWorks - Shipping Setup";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageNoAccounts.ResumeLayout(false);
            this.useExistingAccountPanel.ResumeLayout(false);
            this.useExistingAccountPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.carrierLogo)).EndInit();
            this.addExistingAccountPanel.ResumeLayout(false);
            this.addExistingAccountPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageNoAccounts;
        private Setup.CounterRateSignUpInformationControl counterRateSignUpInformationControl1;
        private System.Windows.Forms.Label setupExistingAccountHeader;
        private System.Windows.Forms.Button setupExistingAccountButton;
        private System.Windows.Forms.ComboBox setupExistingProvider;
        private System.Windows.Forms.Label setupExistingAccountProviderLabel;
        private System.Windows.Forms.Label setupExistingAccountDescription;
        private System.Windows.Forms.Panel useExistingAccountPanel;
        private System.Windows.Forms.Label useExistingAccountHeader;
        private System.Windows.Forms.Button useExistingAccountButton;
        private System.Windows.Forms.Label useExistingAccountDescription;
        private System.Windows.Forms.Panel addExistingAccountPanel;
        private System.Windows.Forms.Label existingAccountRateAmount;
        private System.Windows.Forms.Label carrierName;
        private System.Windows.Forms.PictureBox carrierLogo;
        private System.Windows.Forms.Label existingAccountRateDifference;
    }
}
