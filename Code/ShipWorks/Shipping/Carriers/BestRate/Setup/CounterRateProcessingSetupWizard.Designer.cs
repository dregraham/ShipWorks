namespace ShipWorks.Shipping.Carriers.BestRate.Setup
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
            this.signUpButton = new System.Windows.Forms.Button();
            this.createCarrierAccountDescription = new System.Windows.Forms.Label();
            this.bestRateAmount = new System.Windows.Forms.Label();
            this.bestRateCarrierName = new System.Windows.Forms.Label();
            this.bestRateAccountCarrierLogo = new System.Windows.Forms.PictureBox();
            this.rateFoundDescription = new System.Windows.Forms.Label();
            this.createAccountHeading = new System.Windows.Forms.Label();
            this.useExistingAccountPanel = new System.Windows.Forms.Panel();
            this.useExistingAccountsForRemainingLabel = new System.Windows.Forms.Label();
            this.existingAccountRateDifference = new System.Windows.Forms.Label();
            this.existingAccountRateAmount = new System.Windows.Forms.Label();
            this.useExistingCarrierServiceDescription = new System.Windows.Forms.Label();
            this.useExistingCarrierLogo = new System.Windows.Forms.PictureBox();
            this.useExistingAccountHeader = new System.Windows.Forms.Label();
            this.useExistingAccountButton = new System.Windows.Forms.Button();
            this.useExistingAccountDescription = new System.Windows.Forms.Label();
            this.addExistingAccountPanel = new System.Windows.Forms.Panel();
            this.setupExistingProvider = new ShipWorks.UI.Controls.ImageComboBox();
            this.setupExistingAccountHeader = new System.Windows.Forms.Label();
            this.setupExistingAccountButton = new System.Windows.Forms.Button();
            this.setupExistingAccountDescription = new System.Windows.Forms.Label();
            this.setupExistingAccountProviderLabel = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageNoAccounts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bestRateAccountCarrierLogo)).BeginInit();
            this.useExistingAccountPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.useExistingCarrierLogo)).BeginInit();
            this.addExistingAccountPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.next.Location = new System.Drawing.Point(472, 489);
            this.next.Text = "Finish";
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(553, 489);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(391, 489);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageNoAccounts);
            this.mainPanel.Size = new System.Drawing.Size(640, 417);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 479);
            this.etchBottom.Size = new System.Drawing.Size(644, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.box_closed;
            this.pictureBox.Location = new System.Drawing.Point(587, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(640, 56);
            // 
            // wizardPageNoAccounts
            // 
            this.wizardPageNoAccounts.Controls.Add(this.signUpButton);
            this.wizardPageNoAccounts.Controls.Add(this.createCarrierAccountDescription);
            this.wizardPageNoAccounts.Controls.Add(this.bestRateAmount);
            this.wizardPageNoAccounts.Controls.Add(this.bestRateCarrierName);
            this.wizardPageNoAccounts.Controls.Add(this.bestRateAccountCarrierLogo);
            this.wizardPageNoAccounts.Controls.Add(this.rateFoundDescription);
            this.wizardPageNoAccounts.Controls.Add(this.createAccountHeading);
            this.wizardPageNoAccounts.Controls.Add(this.useExistingAccountPanel);
            this.wizardPageNoAccounts.Controls.Add(this.addExistingAccountPanel);
            this.wizardPageNoAccounts.Description = "You need to create a shipping account to process your shipment.";
            this.wizardPageNoAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageNoAccounts.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageNoAccounts.Location = new System.Drawing.Point(0, 0);
            this.wizardPageNoAccounts.Name = "wizardPageNoAccounts";
            this.wizardPageNoAccounts.Size = new System.Drawing.Size(640, 417);
            this.wizardPageNoAccounts.TabIndex = 0;
            this.wizardPageNoAccounts.Title = "Shipping Account";
            // 
            // signUpButton
            // 
            this.signUpButton.AutoSize = true;
            this.signUpButton.Location = new System.Drawing.Point(50, 132);
            this.signUpButton.Name = "signUpButton";
            this.signUpButton.Size = new System.Drawing.Size(165, 23);
            this.signUpButton.TabIndex = 14;
            this.signUpButton.Text = "Continue and sign up >";
            this.signUpButton.UseVisualStyleBackColor = true;
            this.signUpButton.Click += new System.EventHandler(this.OnSignUp);
            // 
            // createCarrierAccountDescription
            // 
            this.createCarrierAccountDescription.Location = new System.Drawing.Point(36, 99);
            this.createCarrierAccountDescription.Name = "createCarrierAccountDescription";
            this.createCarrierAccountDescription.Size = new System.Drawing.Size(480, 30);
            this.createCarrierAccountDescription.TabIndex = 13;
            this.createCarrierAccountDescription.Text = "USPS partners with Express1 to enable printing USPS shipping labels directly from" +
    " your printer. To continue you\'ll need an account with Express1. There is no mon" +
    "thly fee for the account.";
            // 
            // bestRateAmount
            // 
            this.bestRateAmount.AutoSize = true;
            this.bestRateAmount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bestRateAmount.ForeColor = System.Drawing.Color.Green;
            this.bestRateAmount.Location = new System.Drawing.Point(76, 77);
            this.bestRateAmount.Name = "bestRateAmount";
            this.bestRateAmount.Size = new System.Drawing.Size(38, 13);
            this.bestRateAmount.TabIndex = 12;
            this.bestRateAmount.Text = "$5.45";
            // 
            // bestRateCarrierName
            // 
            this.bestRateCarrierName.AutoSize = true;
            this.bestRateCarrierName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.bestRateCarrierName.Location = new System.Drawing.Point(71, 60);
            this.bestRateCarrierName.Name = "bestRateCarrierName";
            this.bestRateCarrierName.Size = new System.Drawing.Size(107, 13);
            this.bestRateCarrierName.TabIndex = 11;
            this.bestRateCarrierName.Text = "USPS Priority Mail";
            // 
            // bestRateAccountCarrierLogo
            // 
            this.bestRateAccountCarrierLogo.Image = global::ShipWorks.Properties.Resources.box_closed16;
            this.bestRateAccountCarrierLogo.Location = new System.Drawing.Point(50, 56);
            this.bestRateAccountCarrierLogo.Name = "bestRateAccountCarrierLogo";
            this.bestRateAccountCarrierLogo.Size = new System.Drawing.Size(20, 20);
            this.bestRateAccountCarrierLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bestRateAccountCarrierLogo.TabIndex = 10;
            this.bestRateAccountCarrierLogo.TabStop = false;
            // 
            // rateFoundDescription
            // 
            this.rateFoundDescription.AutoSize = true;
            this.rateFoundDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rateFoundDescription.Location = new System.Drawing.Point(37, 32);
            this.rateFoundDescription.Name = "rateFoundDescription";
            this.rateFoundDescription.Size = new System.Drawing.Size(126, 13);
            this.rateFoundDescription.TabIndex = 9;
            this.rateFoundDescription.Text = "The rate you selected is:";
            // 
            // createAccountHeading
            // 
            this.createAccountHeading.AutoSize = true;
            this.createAccountHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createAccountHeading.Location = new System.Drawing.Point(20, 9);
            this.createAccountHeading.Name = "createAccountHeading";
            this.createAccountHeading.Size = new System.Drawing.Size(155, 13);
            this.createAccountHeading.TabIndex = 8;
            this.createAccountHeading.Text = "Create a Shipping Account";
            // 
            // useExistingAccountPanel
            // 
            this.useExistingAccountPanel.Controls.Add(this.useExistingAccountsForRemainingLabel);
            this.useExistingAccountPanel.Controls.Add(this.existingAccountRateDifference);
            this.useExistingAccountPanel.Controls.Add(this.existingAccountRateAmount);
            this.useExistingAccountPanel.Controls.Add(this.useExistingCarrierServiceDescription);
            this.useExistingAccountPanel.Controls.Add(this.useExistingCarrierLogo);
            this.useExistingAccountPanel.Controls.Add(this.useExistingAccountHeader);
            this.useExistingAccountPanel.Controls.Add(this.useExistingAccountButton);
            this.useExistingAccountPanel.Controls.Add(this.useExistingAccountDescription);
            this.useExistingAccountPanel.Location = new System.Drawing.Point(17, 264);
            this.useExistingAccountPanel.Name = "useExistingAccountPanel";
            this.useExistingAccountPanel.Size = new System.Drawing.Size(591, 148);
            this.useExistingAccountPanel.TabIndex = 7;
            // 
            // useExistingAccountsForRemainingLabel
            // 
            this.useExistingAccountsForRemainingLabel.AutoSize = true;
            this.useExistingAccountsForRemainingLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.useExistingAccountsForRemainingLabel.Location = new System.Drawing.Point(30, 116);
            this.useExistingAccountsForRemainingLabel.Name = "useExistingAccountsForRemainingLabel";
            this.useExistingAccountsForRemainingLabel.Size = new System.Drawing.Size(396, 13);
            this.useExistingAccountsForRemainingLabel.TabIndex = 10;
            this.useExistingAccountsForRemainingLabel.Text = "(ShipWorks will use your existing accounts for the remaining selected shipments.)" +
    "";
            // 
            // existingAccountRateDifference
            // 
            this.existingAccountRateDifference.AutoSize = true;
            this.existingAccountRateDifference.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.existingAccountRateDifference.Location = new System.Drawing.Point(103, 63);
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
            this.existingAccountRateAmount.Location = new System.Drawing.Point(60, 63);
            this.existingAccountRateAmount.Name = "existingAccountRateAmount";
            this.existingAccountRateAmount.Size = new System.Drawing.Size(45, 13);
            this.existingAccountRateAmount.TabIndex = 8;
            this.existingAccountRateAmount.Text = "$17.82";
            // 
            // useExistingCarrierServiceDescription
            // 
            this.useExistingCarrierServiceDescription.AutoSize = true;
            this.useExistingCarrierServiceDescription.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.useExistingCarrierServiceDescription.Location = new System.Drawing.Point(60, 50);
            this.useExistingCarrierServiceDescription.Name = "useExistingCarrierServiceDescription";
            this.useExistingCarrierServiceDescription.Size = new System.Drawing.Size(40, 13);
            this.useExistingCarrierServiceDescription.TabIndex = 7;
            this.useExistingCarrierServiceDescription.Text = "FedEx";
            // 
            // useExistingCarrierLogo
            // 
            this.useExistingCarrierLogo.Image = global::ShipWorks.Properties.Resources.box_closed16;
            this.useExistingCarrierLogo.Location = new System.Drawing.Point(33, 46);
            this.useExistingCarrierLogo.Name = "useExistingCarrierLogo";
            this.useExistingCarrierLogo.Size = new System.Drawing.Size(20, 20);
            this.useExistingCarrierLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.useExistingCarrierLogo.TabIndex = 6;
            this.useExistingCarrierLogo.TabStop = false;
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
            this.useExistingAccountButton.Location = new System.Drawing.Point(33, 87);
            this.useExistingAccountButton.Name = "useExistingAccountButton";
            this.useExistingAccountButton.Size = new System.Drawing.Size(150, 23);
            this.useExistingAccountButton.TabIndex = 5;
            this.useExistingAccountButton.Text = "Use my existing account";
            this.useExistingAccountButton.UseVisualStyleBackColor = true;
            this.useExistingAccountButton.Click += new System.EventHandler(this.OnUseExistingAccount);
            // 
            // useExistingAccountDescription
            // 
            this.useExistingAccountDescription.AutoSize = true;
            this.useExistingAccountDescription.Location = new System.Drawing.Point(19, 22);
            this.useExistingAccountDescription.Name = "useExistingAccountDescription";
            this.useExistingAccountDescription.Size = new System.Drawing.Size(447, 13);
            this.useExistingAccountDescription.TabIndex = 2;
            this.useExistingAccountDescription.Text = "If you\'d rather not sign up right now, your {ProviderName} account had the next b" +
    "est rate:";
            // 
            // addExistingAccountPanel
            // 
            this.addExistingAccountPanel.Controls.Add(this.setupExistingProvider);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountHeader);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountButton);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountDescription);
            this.addExistingAccountPanel.Controls.Add(this.setupExistingAccountProviderLabel);
            this.addExistingAccountPanel.Location = new System.Drawing.Point(17, 174);
            this.addExistingAccountPanel.Name = "addExistingAccountPanel";
            this.addExistingAccountPanel.Size = new System.Drawing.Size(457, 88);
            this.addExistingAccountPanel.TabIndex = 6;
            // 
            // setupExistingProvider
            // 
            this.setupExistingProvider.FormattingEnabled = true;
            this.setupExistingProvider.Location = new System.Drawing.Point(130, 48);
            this.setupExistingProvider.Name = "setupExistingProvider";
            this.setupExistingProvider.Size = new System.Drawing.Size(189, 21);
            this.setupExistingProvider.TabIndex = 6;
            this.setupExistingProvider.SelectedIndexChanged += new System.EventHandler(this.OnSetupExistingProviderChanged);
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
            this.setupExistingAccountButton.Enabled = false;
            this.setupExistingAccountButton.Location = new System.Drawing.Point(325, 47);
            this.setupExistingAccountButton.Name = "setupExistingAccountButton";
            this.setupExistingAccountButton.Size = new System.Drawing.Size(99, 23);
            this.setupExistingAccountButton.TabIndex = 5;
            this.setupExistingAccountButton.Text = "Add my account";
            this.setupExistingAccountButton.UseVisualStyleBackColor = true;
            this.setupExistingAccountButton.Click += new System.EventHandler(this.OnAddExistingAccount);
            // 
            // setupExistingAccountDescription
            // 
            this.setupExistingAccountDescription.AutoSize = true;
            this.setupExistingAccountDescription.Location = new System.Drawing.Point(19, 22);
            this.setupExistingAccountDescription.Name = "setupExistingAccountDescription";
            this.setupExistingAccountDescription.Size = new System.Drawing.Size(390, 13);
            this.setupExistingAccountDescription.TabIndex = 2;
            this.setupExistingAccountDescription.Text = "If you already have a shipping account, you can have ShipWorks use it instead:";
            // 
            // setupExistingAccountProviderLabel
            // 
            this.setupExistingAccountProviderLabel.AutoSize = true;
            this.setupExistingAccountProviderLabel.Location = new System.Drawing.Point(31, 51);
            this.setupExistingAccountProviderLabel.Name = "setupExistingAccountProviderLabel";
            this.setupExistingAccountProviderLabel.Size = new System.Drawing.Size(93, 13);
            this.setupExistingAccountProviderLabel.TabIndex = 3;
            this.setupExistingAccountProviderLabel.Text = "Account Provider:";
            // 
            // CounterRateProcessingSetupWizard
            // 
            this.AcceptButton = null;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(640, 524);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CounterRateProcessingSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageNoAccounts});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks - Shipping Setup";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageNoAccounts.ResumeLayout(false);
            this.wizardPageNoAccounts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bestRateAccountCarrierLogo)).EndInit();
            this.useExistingAccountPanel.ResumeLayout(false);
            this.useExistingAccountPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.useExistingCarrierLogo)).EndInit();
            this.addExistingAccountPanel.ResumeLayout(false);
            this.addExistingAccountPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageNoAccounts;
        private System.Windows.Forms.Label setupExistingAccountHeader;
        private System.Windows.Forms.Button setupExistingAccountButton;
        private System.Windows.Forms.Label setupExistingAccountProviderLabel;
        private System.Windows.Forms.Label setupExistingAccountDescription;
        private System.Windows.Forms.Panel useExistingAccountPanel;
        private System.Windows.Forms.Label useExistingAccountHeader;
        private System.Windows.Forms.Button useExistingAccountButton;
        private System.Windows.Forms.Label useExistingAccountDescription;
        private System.Windows.Forms.Panel addExistingAccountPanel;
        private System.Windows.Forms.Label existingAccountRateAmount;
        private System.Windows.Forms.Label useExistingCarrierServiceDescription;
        private System.Windows.Forms.PictureBox useExistingCarrierLogo;
        private System.Windows.Forms.Label existingAccountRateDifference;
        private System.Windows.Forms.Button signUpButton;
        private System.Windows.Forms.Label createCarrierAccountDescription;
        private System.Windows.Forms.Label bestRateAmount;
        private System.Windows.Forms.Label bestRateCarrierName;
        private System.Windows.Forms.PictureBox bestRateAccountCarrierLogo;
        private System.Windows.Forms.Label rateFoundDescription;
        private System.Windows.Forms.Label createAccountHeading;
        private System.Windows.Forms.Label useExistingAccountsForRemainingLabel;
        private UI.Controls.ImageComboBox setupExistingProvider;
    }
}
