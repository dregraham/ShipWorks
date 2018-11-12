namespace ShipWorks.Users
{
    partial class AddUserWizard
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
            this.wizardPageAccount = new ShipWorks.UI.Wizard.WizardPage();
            this.label3 = new System.Windows.Forms.Label();
            this.uiModeSelectionControl = new ShipWorks.Users.UIModeSelectionControl();
            this.helpUserEmail = new ShipWorks.UI.Controls.InfoTip();
            this.panelAccountType = new System.Windows.Forms.Panel();
            this.accountAdmin = new System.Windows.Forms.RadioButton();
            this.accountStandard = new System.Windows.Forms.RadioButton();
            this.labelAccountType = new System.Windows.Forms.Label();
            this.labelUserAccount = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.email = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.passwordAgain = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.wizardPageRights = new ShipWorks.UI.Wizard.WizardPage();
            this.copyRightsFrom = new ShipWorks.UI.Controls.DropDownButton();
            this.menuCopyRightsFrom = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.placeholderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asdfToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asdfToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.panelAdminAllRights = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.permissionEditor = new ShipWorks.Users.Security.PermissionEditor();
            this.wizardPageComplete = new ShipWorks.UI.Wizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.iconSetupComplete = new System.Windows.Forms.PictureBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageAccount.SuspendLayout();
            this.panelAccountType.SuspendLayout();
            this.wizardPageRights.SuspendLayout();
            this.menuCopyRightsFrom.SuspendLayout();
            this.panelAdminAllRights.SuspendLayout();
            this.wizardPageComplete.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSetupComplete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(313, 411);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(394, 411);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(232, 411);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageAccount);
            this.mainPanel.Size = new System.Drawing.Size(481, 339);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 401);
            this.etchBottom.Size = new System.Drawing.Size(485, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.dude33;
            this.pictureBox.Location = new System.Drawing.Point(428, 3);
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(481, 56);
            // 
            // wizardPageAccount
            // 
            this.wizardPageAccount.Controls.Add(this.label3);
            this.wizardPageAccount.Controls.Add(this.uiModeSelectionControl);
            this.wizardPageAccount.Controls.Add(this.helpUserEmail);
            this.wizardPageAccount.Controls.Add(this.panelAccountType);
            this.wizardPageAccount.Controls.Add(this.labelAccountType);
            this.wizardPageAccount.Controls.Add(this.labelUserAccount);
            this.wizardPageAccount.Controls.Add(this.label6);
            this.wizardPageAccount.Controls.Add(this.password);
            this.wizardPageAccount.Controls.Add(this.label8);
            this.wizardPageAccount.Controls.Add(this.email);
            this.wizardPageAccount.Controls.Add(this.label7);
            this.wizardPageAccount.Controls.Add(this.passwordAgain);
            this.wizardPageAccount.Controls.Add(this.username);
            this.wizardPageAccount.Controls.Add(this.label9);
            this.wizardPageAccount.Description = "Setup the account details of the new user.";
            this.wizardPageAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAccount.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAccount.Name = "wizardPageAccount";
            this.wizardPageAccount.Size = new System.Drawing.Size(481, 339);
            this.wizardPageAccount.TabIndex = 0;
            this.wizardPageAccount.Title = "Add New User";
            this.wizardPageAccount.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAccountPage);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(21, 224);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 191;
            this.label3.Text = "UI Mode";
            // 
            // uiModeSelectionControl
            // 
            this.uiModeSelectionControl.Location = new System.Drawing.Point(32, 239);
            this.uiModeSelectionControl.Name = "uiModeSelectionControl";
            this.uiModeSelectionControl.Size = new System.Drawing.Size(235, 56);
            this.uiModeSelectionControl.TabIndex = 190;
            // 
            // helpUserEmail
            // 
            this.helpUserEmail.Caption = "The email address will be used to send the user a new password if its forgotten.";
            this.helpUserEmail.Location = new System.Drawing.Point(373, 62);
            this.helpUserEmail.Name = "helpUserEmail";
            this.helpUserEmail.Size = new System.Drawing.Size(12, 12);
            this.helpUserEmail.TabIndex = 189;
            this.helpUserEmail.Title = "Email Address";
            // 
            // panelAccountType
            // 
            this.panelAccountType.Controls.Add(this.accountAdmin);
            this.panelAccountType.Controls.Add(this.accountStandard);
            this.panelAccountType.Location = new System.Drawing.Point(42, 167);
            this.panelAccountType.Name = "panelAccountType";
            this.panelAccountType.Size = new System.Drawing.Size(200, 49);
            this.panelAccountType.TabIndex = 10;
            // 
            // accountAdmin
            // 
            this.accountAdmin.AutoSize = true;
            this.accountAdmin.Location = new System.Drawing.Point(3, 26);
            this.accountAdmin.Name = "accountAdmin";
            this.accountAdmin.Size = new System.Drawing.Size(89, 17);
            this.accountAdmin.TabIndex = 1;
            this.accountAdmin.Text = "Administrator";
            this.accountAdmin.UseVisualStyleBackColor = true;
            // 
            // accountStandard
            // 
            this.accountStandard.AutoSize = true;
            this.accountStandard.Checked = true;
            this.accountStandard.Location = new System.Drawing.Point(3, 5);
            this.accountStandard.Name = "accountStandard";
            this.accountStandard.Size = new System.Drawing.Size(94, 17);
            this.accountStandard.TabIndex = 0;
            this.accountStandard.TabStop = true;
            this.accountStandard.Text = "Standard User";
            this.accountStandard.UseVisualStyleBackColor = true;
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAccountType.Location = new System.Drawing.Point(21, 150);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(84, 13);
            this.labelAccountType.TabIndex = 9;
            this.labelAccountType.Text = "Account Type";
            // 
            // labelUserAccount
            // 
            this.labelUserAccount.AutoSize = true;
            this.labelUserAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUserAccount.Location = new System.Drawing.Point(21, 8);
            this.labelUserAccount.Name = "labelUserAccount";
            this.labelUserAccount.Size = new System.Drawing.Size(82, 13);
            this.labelUserAccount.TabIndex = 0;
            this.labelUserAccount.Text = "User Account";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Retype password:";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(126, 84);
            this.fieldLengthProvider.SetMaxLengthSource(this.password, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(243, 21);
            this.password.TabIndex = 6;
            this.password.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(63, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Password:";
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(126, 57);
            this.fieldLengthProvider.SetMaxLengthSource(this.email, ShipWorks.Data.Utility.EntityFieldLengthSource.UserEmail);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(243, 21);
            this.email.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 60);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Email address:";
            // 
            // passwordAgain
            // 
            this.passwordAgain.Location = new System.Drawing.Point(126, 111);
            this.fieldLengthProvider.SetMaxLengthSource(this.passwordAgain, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.passwordAgain.Name = "passwordAgain";
            this.passwordAgain.Size = new System.Drawing.Size(243, 21);
            this.passwordAgain.TabIndex = 8;
            this.passwordAgain.UseSystemPasswordChar = true;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(126, 30);
            this.fieldLengthProvider.SetMaxLengthSource(this.username, ShipWorks.Data.Utility.EntityFieldLengthSource.UserName);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(243, 21);
            this.username.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(61, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Username:";
            // 
            // wizardPageRights
            // 
            this.wizardPageRights.Controls.Add(this.copyRightsFrom);
            this.wizardPageRights.Controls.Add(this.panelAdminAllRights);
            this.wizardPageRights.Controls.Add(this.permissionEditor);
            this.wizardPageRights.Description = "Configure what rights the user has when using ShipWorks.";
            this.wizardPageRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageRights.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageRights.Location = new System.Drawing.Point(0, 0);
            this.wizardPageRights.Name = "wizardPageRights";
            this.wizardPageRights.Size = new System.Drawing.Size(481, 339);
            this.wizardPageRights.TabIndex = 0;
            this.wizardPageRights.Title = "User Rights";
            this.wizardPageRights.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextRightsPage);
            this.wizardPageRights.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoRightsPage);
            // 
            // copyRightsFrom
            // 
            this.copyRightsFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyRightsFrom.AutoSize = true;
            this.copyRightsFrom.ContextMenuStrip = this.menuCopyRightsFrom;
            this.copyRightsFrom.Image = global::ShipWorks.Properties.Resources.id_card;
            this.copyRightsFrom.Location = new System.Drawing.Point(23, 313);
            this.copyRightsFrom.Name = "copyRightsFrom";
            this.copyRightsFrom.Size = new System.Drawing.Size(148, 23);
            this.copyRightsFrom.SplitButton = false;
            this.copyRightsFrom.SplitContextMenu = this.menuCopyRightsFrom;
            this.copyRightsFrom.TabIndex = 4;
            this.copyRightsFrom.Text = "Copy Rights From";
            this.copyRightsFrom.UseVisualStyleBackColor = true;
            // 
            // menuCopyRightsFrom
            // 
            this.menuCopyRightsFrom.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.menuCopyRightsFrom.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.placeholderToolStripMenuItem});
            this.menuCopyRightsFrom.Name = "contextMenuStrip";
            this.menuCopyRightsFrom.Size = new System.Drawing.Size(141, 26);
            this.menuCopyRightsFrom.Opening += new System.ComponentModel.CancelEventHandler(this.OnOpeningCopyFromMenu);
            // 
            // placeholderToolStripMenuItem
            // 
            this.placeholderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asdfToolStripMenuItem,
            this.asdfToolStripMenuItem1});
            this.placeholderToolStripMenuItem.Name = "placeholderToolStripMenuItem";
            this.placeholderToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.placeholderToolStripMenuItem.Text = "(Placeholder)";
            // 
            // asdfToolStripMenuItem
            // 
            this.asdfToolStripMenuItem.Name = "asdfToolStripMenuItem";
            this.asdfToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.asdfToolStripMenuItem.Text = "asdf";
            // 
            // asdfToolStripMenuItem1
            // 
            this.asdfToolStripMenuItem1.Name = "asdfToolStripMenuItem1";
            this.asdfToolStripMenuItem1.Size = new System.Drawing.Size(96, 22);
            this.asdfToolStripMenuItem1.Text = "asdf";
            // 
            // panelAdminAllRights
            // 
            this.panelAdminAllRights.Controls.Add(this.label2);
            this.panelAdminAllRights.Location = new System.Drawing.Point(23, 7);
            this.panelAdminAllRights.Name = "panelAdminAllRights";
            this.panelAdminAllRights.Size = new System.Drawing.Size(433, 26);
            this.panelAdminAllRights.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "As an administrator, this user has rights to do everything.";
            // 
            // permissionEditor
            // 
            this.permissionEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.permissionEditor.AutoScroll = true;
            this.permissionEditor.BackColor = System.Drawing.Color.White;
            this.permissionEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.permissionEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.permissionEditor.Location = new System.Drawing.Point(23, 7);
            this.permissionEditor.Name = "permissionEditor";
            this.permissionEditor.Size = new System.Drawing.Size(433, 300);
            this.permissionEditor.TabIndex = 0;
            // 
            // wizardPageComplete
            // 
            this.wizardPageComplete.Controls.Add(this.label1);
            this.wizardPageComplete.Controls.Add(this.iconSetupComplete);
            this.wizardPageComplete.Description = "The user has been added to ShipWorks.";
            this.wizardPageComplete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageComplete.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageComplete.Location = new System.Drawing.Point(0, 0);
            this.wizardPageComplete.Name = "wizardPageComplete";
            this.wizardPageComplete.Size = new System.Drawing.Size(481, 339);
            this.wizardPageComplete.TabIndex = 0;
            this.wizardPageComplete.Title = "Setup Complete";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(259, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "The user has been successfully added to ShipWorks.";
            // 
            // iconSetupComplete
            // 
            this.iconSetupComplete.Image = global::ShipWorks.Properties.Resources.check16;
            this.iconSetupComplete.Location = new System.Drawing.Point(25, 9);
            this.iconSetupComplete.Name = "iconSetupComplete";
            this.iconSetupComplete.Size = new System.Drawing.Size(16, 16);
            this.iconSetupComplete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.iconSetupComplete.TabIndex = 3;
            this.iconSetupComplete.TabStop = false;
            // 
            // AddUserWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 446);
            this.Name = "AddUserWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageAccount,
            this.wizardPageRights,
            this.wizardPageComplete});
            this.Text = "Add User Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
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
            this.wizardPageAccount.ResumeLayout(false);
            this.wizardPageAccount.PerformLayout();
            this.panelAccountType.ResumeLayout(false);
            this.panelAccountType.PerformLayout();
            this.wizardPageRights.ResumeLayout(false);
            this.wizardPageRights.PerformLayout();
            this.menuCopyRightsFrom.ResumeLayout(false);
            this.panelAdminAllRights.ResumeLayout(false);
            this.panelAdminAllRights.PerformLayout();
            this.wizardPageComplete.ResumeLayout(false);
            this.wizardPageComplete.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSetupComplete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageAccount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox passwordAgain;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label labelUserAccount;
        private System.Windows.Forms.Panel panelAccountType;
        private System.Windows.Forms.RadioButton accountAdmin;
        private System.Windows.Forms.RadioButton accountStandard;
        private System.Windows.Forms.Label labelAccountType;
        private ShipWorks.UI.Wizard.WizardPage wizardPageRights;
        private ShipWorks.UI.Wizard.WizardPage wizardPageComplete;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox iconSetupComplete;
        private ShipWorks.Users.Security.PermissionEditor permissionEditor;
        private System.Windows.Forms.Panel panelAdminAllRights;
        private System.Windows.Forms.Label label2;
        private ShipWorks.UI.Controls.DropDownButton copyRightsFrom;
        private System.Windows.Forms.ContextMenuStrip menuCopyRightsFrom;
        private System.Windows.Forms.ToolStripMenuItem placeholderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asdfToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asdfToolStripMenuItem1;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip helpUserEmail;
        private UIModeSelectionControl uiModeSelectionControl;
        private System.Windows.Forms.Label label3;
    }
}