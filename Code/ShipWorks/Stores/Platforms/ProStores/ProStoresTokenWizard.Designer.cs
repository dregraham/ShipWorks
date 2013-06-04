namespace ShipWorks.Stores.Platforms.ProStores
{
    partial class ProStoresTokenWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProStoresTokenWizard));
            this.wizardPageApiEntryPoint = new ShipWorks.UI.Wizard.WizardPage();
            this.apiEntryPoint = new System.Windows.Forms.TextBox();
            this.labelApiEntryPoint = new System.Windows.Forms.Label();
            this.labelServerSettingsInfo = new System.Windows.Forms.Label();
            this.wizardPageToken = new ShipWorks.UI.Wizard.WizardPage();
            this.wizardPageFinish = new ShipWorks.UI.Wizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.wizardPageWelcome = new ShipWorks.UI.Wizard.WizardPage();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.createTokenControl = new ShipWorks.Stores.Platforms.ProStores.ProStoresTokenCreateControl();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageApiEntryPoint.SuspendLayout();
            this.wizardPageToken.SuspendLayout();
            this.wizardPageFinish.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).BeginInit();
            this.wizardPageWelcome.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(373, 360);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(454, 360);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(292, 360);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageToken);
            this.mainPanel.Size = new System.Drawing.Size(541, 288);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 350);
            this.etchBottom.Size = new System.Drawing.Size(545, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.server_id_card;
            this.pictureBox.Location = new System.Drawing.Point(488, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(541, 56);
            // 
            // wizardPageApiEntryPoint
            // 
            this.wizardPageApiEntryPoint.Controls.Add(this.apiEntryPoint);
            this.wizardPageApiEntryPoint.Controls.Add(this.labelApiEntryPoint);
            this.wizardPageApiEntryPoint.Controls.Add(this.labelServerSettingsInfo);
            this.wizardPageApiEntryPoint.Description = "Enter the API Entry Point of your store.";
            this.wizardPageApiEntryPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageApiEntryPoint.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageApiEntryPoint.Location = new System.Drawing.Point(0, 0);
            this.wizardPageApiEntryPoint.Name = "wizardPageApiEntryPoint";
            this.wizardPageApiEntryPoint.Size = new System.Drawing.Size(541, 288);
            this.wizardPageApiEntryPoint.TabIndex = 0;
            this.wizardPageApiEntryPoint.Title = "ProStores Authentication";
            this.wizardPageApiEntryPoint.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextApiEntryPoint);
            // 
            // apiEntryPoint
            // 
            this.apiEntryPoint.Location = new System.Drawing.Point(116, 45);
            this.apiEntryPoint.Name = "apiEntryPoint";
            this.apiEntryPoint.Size = new System.Drawing.Size(365, 21);
            this.apiEntryPoint.TabIndex = 8;
            // 
            // labelApiEntryPoint
            // 
            this.labelApiEntryPoint.AutoSize = true;
            this.labelApiEntryPoint.Location = new System.Drawing.Point(26, 48);
            this.labelApiEntryPoint.Name = "labelApiEntryPoint";
            this.labelApiEntryPoint.Size = new System.Drawing.Size(84, 13);
            this.labelApiEntryPoint.TabIndex = 7;
            this.labelApiEntryPoint.Text = "API Entry Point:";
            // 
            // labelServerSettingsInfo
            // 
            this.labelServerSettingsInfo.Location = new System.Drawing.Point(20, 8);
            this.labelServerSettingsInfo.Name = "labelServerSettingsInfo";
            this.labelServerSettingsInfo.Size = new System.Drawing.Size(461, 35);
            this.labelServerSettingsInfo.TabIndex = 6;
            this.labelServerSettingsInfo.Text = "Log on to your ProStores Store Administration and navigate to Store Settings -> S" +
                "erver.  Under \"URL Information\", look for a section called \"API Entry Point\".";
            // 
            // wizardPageToken
            // 
            this.wizardPageToken.Controls.Add(this.labelInfo1);
            this.wizardPageToken.Controls.Add(this.createTokenControl);
            this.wizardPageToken.Description = "Authorize ShipWorks to connect to your ProStores store.";
            this.wizardPageToken.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageToken.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageToken.Location = new System.Drawing.Point(0, 0);
            this.wizardPageToken.Name = "wizardPageToken";
            this.wizardPageToken.Size = new System.Drawing.Size(541, 288);
            this.wizardPageToken.TabIndex = 0;
            this.wizardPageToken.Title = "ProStores Authentication";
            this.wizardPageToken.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextCreateToken);
            // 
            // wizardPageFinish
            // 
            this.wizardPageFinish.Controls.Add(this.label1);
            this.wizardPageFinish.Controls.Add(this.iconSetupComplete);
            this.wizardPageFinish.Description = "ShipWorks is ready to connect to your store.";
            this.wizardPageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageFinish.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinish.Name = "wizardPageFinish";
            this.wizardPageFinish.Size = new System.Drawing.Size(530, 288);
            this.wizardPageFinish.TabIndex = 0;
            this.wizardPageFinish.Title = "Setup Complete";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "ShipWorks is ready to connect to your ProStores store.";
            // 
            // iconSetupComplete
            // 
            this.iconSetupComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.iconSetupComplete.Location = new System.Drawing.Point(22, 10);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 3;
            this.iconSetupComplete.TabStop = false;
            // 
            // wizardPageWelcome
            // 
            this.wizardPageWelcome.Controls.Add(this.labelInfo2);
            this.wizardPageWelcome.Controls.Add(this.labelInfo);
            this.wizardPageWelcome.Description = "Update how ShipWorks authenticates with ProStores.";
            this.wizardPageWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageWelcome.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.wizardPageWelcome.Location = new System.Drawing.Point(0, 0);
            this.wizardPageWelcome.Name = "wizardPageWelcome";
            this.wizardPageWelcome.Size = new System.Drawing.Size(541, 288);
            this.wizardPageWelcome.TabIndex = 0;
            this.wizardPageWelcome.Title = "ProStores Authentication";
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(23, 59);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(495, 32);
            this.labelInfo2.TabIndex = 1;
            this.labelInfo2.Text = "The new method is more secure, and also allows ShipWorks to take advantage of add" +
                "itional ProStores features such as shipment status updates.";
            // 
            // labelInfo
            // 
            this.labelInfo.Location = new System.Drawing.Point(24, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(479, 46);
            this.labelInfo.TabIndex = 0;
            this.labelInfo.Text = resources.GetString("labelInfo.Text");
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(22, 8);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(519, 34);
            this.labelInfo1.TabIndex = 17;
            this.labelInfo1.Text = "ProStores requires you to authorize ShipWorks to connect to your store.  This is " +
                "done by logging into a special ProStores page that creates an Login Token for Sh" +
                "ipWorks.";
            // 
            // createTokenControl
            // 
            this.createTokenControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.createTokenControl.Location = new System.Drawing.Point(19, 42);
            this.createTokenControl.Name = "createTokenControl";
            this.createTokenControl.Size = new System.Drawing.Size(411, 31);
            this.createTokenControl.TabIndex = 16;
            this.createTokenControl.TokenImported += new System.EventHandler(this.OnTokenCreated);
            // 
            // ProStoresTokenWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 395);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ProStoresTokenWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageWelcome,
            this.wizardPageApiEntryPoint,
            this.wizardPageToken,
            this.wizardPageFinish});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ProStoresTokenWizard";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageApiEntryPoint.ResumeLayout(false);
            this.wizardPageApiEntryPoint.PerformLayout();
            this.wizardPageToken.ResumeLayout(false);
            this.wizardPageFinish.ResumeLayout(false);
            this.wizardPageFinish.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.iconSetupComplete)).EndInit();
            this.wizardPageWelcome.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageApiEntryPoint;
        private ShipWorks.UI.Wizard.WizardPage wizardPageToken;
        private System.Windows.Forms.TextBox apiEntryPoint;
        private System.Windows.Forms.Label labelApiEntryPoint;
        private System.Windows.Forms.Label labelServerSettingsInfo;
        private ShipWorks.UI.Wizard.WizardPage wizardPageFinish;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private ShipWorks.UI.Wizard.WizardPage wizardPageWelcome;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label labelInfo2;
        private System.Windows.Forms.Label labelInfo1;
        private ProStoresTokenCreateControl createTokenControl;
    }
}