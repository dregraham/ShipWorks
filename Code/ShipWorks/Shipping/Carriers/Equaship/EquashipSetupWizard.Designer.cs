namespace ShipWorks.Shipping.Carriers.EquaShip
{
    partial class EquashipSetupWizard
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
            this.wizardPageInitial = new ShipWorks.UI.Wizard.WizardPage();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.wizardPageAccount = new ShipWorks.UI.Wizard.WizardPage();
            this.password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.labelFedExAccount = new System.Windows.Forms.Label();
            this.personControl = new ShipWorks.Data.Controls.AutofillPersonControl();
            this.wizardPageSettings = new ShipWorks.UI.Wizard.WizardPage();
            this.optionsControl = new ShipWorks.Shipping.Carriers.EquaShip.EquaShipOptionsControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageInitial.SuspendLayout();
            this.wizardPageAccount.SuspendLayout();
            this.wizardPageSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(400, 542);
            this.next.TabIndex = 1;
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(481, 542);
            this.cancel.TabIndex = 2;
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(319, 542);
            this.back.TabIndex = 0;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageAccount);
            this.mainPanel.Size = new System.Drawing.Size(568, 470);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 532);
            this.etchBottom.Size = new System.Drawing.Size(572, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.EquashipLogo;
            this.pictureBox.Location = new System.Drawing.Point(388, 3);
            this.pictureBox.Size = new System.Drawing.Size(177, 49);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(568, 56);
            // 
            // wizardPageInitial
            // 
            this.wizardPageInitial.Controls.Add(this.labelInfo1);
            this.wizardPageInitial.Description = "Setup ShipWorks to work with your EquaShip account.";
            this.wizardPageInitial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageInitial.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageInitial.Location = new System.Drawing.Point(0, 0);
            this.wizardPageInitial.Name = "wizardPageInitial";
            this.wizardPageInitial.Size = new System.Drawing.Size(568, 470);
            this.wizardPageInitial.TabIndex = 0;
            this.wizardPageInitial.Title = "Setup EquaShip Shipping";
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(20, 9);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(454, 47);
            this.labelInfo1.TabIndex = 1;
            this.labelInfo1.Text = "This wizard will assist you in registering your EquaShip shipping account for use" +
    " with ShipWorks. This enables you to begin shipping, tracking, and printing Equa" +
    "ship labels directly from ShipWorks.";
            // 
            // wizardPageAccount
            // 
            this.wizardPageAccount.Controls.Add(this.password);
            this.wizardPageAccount.Controls.Add(this.label1);
            this.wizardPageAccount.Controls.Add(this.labelUsername);
            this.wizardPageAccount.Controls.Add(this.username);
            this.wizardPageAccount.Controls.Add(this.labelFedExAccount);
            this.wizardPageAccount.Controls.Add(this.personControl);
            this.wizardPageAccount.Description = "Enter your EquaShip account information.";
            this.wizardPageAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAccount.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccount.Name = "wizardPageAccount";
            this.wizardPageAccount.Size = new System.Drawing.Size(568, 470);
            this.wizardPageAccount.TabIndex = 0;
            this.wizardPageAccount.Title = "EquaShip Account";
            this.wizardPageAccount.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccount);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(99, 55);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(165, 21);
            this.password.TabIndex = 1;
            this.password.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Password:";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(36, 30);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 6;
            this.labelUsername.Text = "Username:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(99, 27);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(165, 21);
            this.username.TabIndex = 0;
            // 
            // labelFedExAccount
            // 
            this.labelFedExAccount.AutoSize = true;
            this.labelFedExAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFedExAccount.Location = new System.Drawing.Point(25, 9);
            this.labelFedExAccount.Name = "labelFedExAccount";
            this.labelFedExAccount.Size = new System.Drawing.Size(107, 13);
            this.labelFedExAccount.TabIndex = 5;
            this.labelFedExAccount.Text = "EquaShip Account";
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
            this.personControl.Location = new System.Drawing.Point(23, 80);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 375);
            this.personControl.TabIndex = 2;
            // 
            // wizardPageSettings
            // 
            this.wizardPageSettings.Controls.Add(this.optionsControl);
            this.wizardPageSettings.Description = "Configure EquaShip settings.";
            this.wizardPageSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageSettings.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSettings.Name = "wizardPageSettings";
            this.wizardPageSettings.Size = new System.Drawing.Size(568, 470);
            this.wizardPageSettings.TabIndex = 0;
            this.wizardPageSettings.Title = "EquaShip Settings";
            this.wizardPageSettings.Load += new System.EventHandler(this.OnStepNextSettings);
            // 
            // optionsControl
            // 
            this.optionsControl.Location = new System.Drawing.Point(23, 3);
            this.optionsControl.Name = "optionsControl";
            this.optionsControl.Size = new System.Drawing.Size(323, 83);
            this.optionsControl.TabIndex = 0;
            // 
            // EquashipSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 577);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EquashipSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageInitial,
            this.wizardPageAccount,
            this.wizardPageSettings});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "EquaShip Setup Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageInitial.ResumeLayout(false);
            this.wizardPageAccount.ResumeLayout(false);
            this.wizardPageAccount.PerformLayout();
            this.wizardPageSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageInitial;
        private System.Windows.Forms.Label labelInfo1;
        private UI.Wizard.WizardPage wizardPageAccount;
        private Data.Controls.AutofillPersonControl personControl;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelFedExAccount;
        private UI.Wizard.WizardPage wizardPageSettings;
        private EquaShipOptionsControl optionsControl;
    }
}