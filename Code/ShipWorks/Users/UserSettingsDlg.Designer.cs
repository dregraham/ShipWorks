namespace ShipWorks.Users
{
    partial class UserSettingsDlg
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
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.titleUsername = new System.Windows.Forms.Label();
            this.titleUserDescription = new System.Windows.Forms.Label();
            this.imageUser = new System.Windows.Forms.PictureBox();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageUser = new ShipWorks.UI.Controls.OptionPage();
            this.infoTipUserEmail = new ShipWorks.UI.Controls.InfoTip();
            this.panelNoChangeAccount = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelUserAccount = new System.Windows.Forms.Label();
            this.changePassword = new System.Windows.Forms.Button();
            this.panelAccountType = new System.Windows.Forms.Panel();
            this.accountAdmin = new System.Windows.Forms.RadioButton();
            this.accountStandard = new System.Windows.Forms.RadioButton();
            this.labelPassword = new System.Windows.Forms.Label();
            this.statusDeleted = new System.Windows.Forms.RadioButton();
            this.labelAccountType = new System.Windows.Forms.Label();
            this.labelUsername = new System.Windows.Forms.Label();
            this.statusActive = new System.Windows.Forms.RadioButton();
            this.email = new System.Windows.Forms.TextBox();
            this.username = new System.Windows.Forms.TextBox();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelAccountStatus = new System.Windows.Forms.Label();
            this.optionPageRights = new ShipWorks.UI.Controls.OptionPage();
            this.permissionEditor = new ShipWorks.Users.Security.PermissionEditor();
            this.panelNotUntilNextLogon = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelAdminAllRights = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.optionPageAudit = new ShipWorks.UI.Controls.OptionPage();
            this.auditControl = new ShipWorks.Users.Audit.AuditControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).BeginInit();
            this.optionControl.SuspendLayout();
            this.optionPageUser.SuspendLayout();
            this.panelNoChangeAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).BeginInit();
            this.panelAccountType.SuspendLayout();
            this.optionPageRights.SuspendLayout();
            this.panelNotUntilNextLogon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).BeginInit();
            this.panelAdminAllRights.SuspendLayout();
            this.optionPageAudit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(573, 524);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(492, 524);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 2;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // titleUsername
            // 
            this.titleUsername.AutoSize = true;
            this.titleUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.titleUsername.Location = new System.Drawing.Point(48, 8);
            this.titleUsername.Name = "titleUsername";
            this.titleUsername.Size = new System.Drawing.Size(31, 13);
            this.titleUsername.TabIndex = 5;
            this.titleUsername.Text = "Wes";
            // 
            // titleUserDescription
            // 
            this.titleUserDescription.AutoSize = true;
            this.titleUserDescription.ForeColor = System.Drawing.Color.DimGray;
            this.titleUserDescription.Location = new System.Drawing.Point(48, 24);
            this.titleUserDescription.Name = "titleUserDescription";
            this.titleUserDescription.Size = new System.Drawing.Size(76, 13);
            this.titleUserDescription.TabIndex = 6;
            this.titleUserDescription.Text = "Standard User";
            // 
            // imageUser
            // 
            this.imageUser.Image = global::ShipWorks.Properties.Resources.dude31;
            this.imageUser.Location = new System.Drawing.Point(12, 7);
            this.imageUser.Name = "imageUser";
            this.imageUser.Size = new System.Drawing.Size(32, 32);
            this.imageUser.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageUser.TabIndex = 4;
            this.imageUser.TabStop = false;
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPageUser);
            this.optionControl.Controls.Add(this.optionPageRights);
            this.optionControl.Controls.Add(this.optionPageAudit);
            this.optionControl.Location = new System.Drawing.Point(12, 45);
            this.optionControl.MenuListWidth = 100;
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(633, 472);
            this.optionControl.TabIndex = 0;
            // 
            // optionPageUser
            // 
            this.optionPageUser.BackColor = System.Drawing.Color.White;
            this.optionPageUser.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageUser.Controls.Add(this.infoTipUserEmail);
            this.optionPageUser.Controls.Add(this.panelNoChangeAccount);
            this.optionPageUser.Controls.Add(this.labelUserAccount);
            this.optionPageUser.Controls.Add(this.changePassword);
            this.optionPageUser.Controls.Add(this.panelAccountType);
            this.optionPageUser.Controls.Add(this.labelPassword);
            this.optionPageUser.Controls.Add(this.statusDeleted);
            this.optionPageUser.Controls.Add(this.labelAccountType);
            this.optionPageUser.Controls.Add(this.labelUsername);
            this.optionPageUser.Controls.Add(this.statusActive);
            this.optionPageUser.Controls.Add(this.email);
            this.optionPageUser.Controls.Add(this.username);
            this.optionPageUser.Controls.Add(this.labelEmail);
            this.optionPageUser.Controls.Add(this.labelAccountStatus);
            this.optionPageUser.Location = new System.Drawing.Point(103, 0);
            this.optionPageUser.Name = "optionPageUser";
            this.optionPageUser.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageUser.Size = new System.Drawing.Size(530, 472);
            this.optionPageUser.TabIndex = 1;
            this.optionPageUser.Text = "User";
            // 
            // infoTipUserEmail
            // 
            this.infoTipUserEmail.Caption = "The email address will be used to send the user a new password if it is forgotten" +
                ".";
            this.infoTipUserEmail.Location = new System.Drawing.Point(339, 57);
            this.infoTipUserEmail.Name = "infoTipUserEmail";
            this.infoTipUserEmail.Size = new System.Drawing.Size(12, 12);
            this.infoTipUserEmail.TabIndex = 196;
            this.infoTipUserEmail.Title = "Email Address";
            // 
            // panelNoChangeAccount
            // 
            this.panelNoChangeAccount.Controls.Add(this.label8);
            this.panelNoChangeAccount.Controls.Add(this.pictureBox1);
            this.panelNoChangeAccount.Location = new System.Drawing.Point(11, 242);
            this.panelNoChangeAccount.Name = "panelNoChangeAccount";
            this.panelNoChangeAccount.Size = new System.Drawing.Size(382, 38);
            this.panelNoChangeAccount.TabIndex = 195;
            this.panelNoChangeAccount.Visible = false;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.Color.DimGray;
            this.label8.Location = new System.Drawing.Point(21, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(361, 30);
            this.label8.TabIndex = 1;
            this.label8.Text = "There has to be at least one Administrator user.  Since this is the only Administ" +
                "rator user, you cannot delete user or change the account type.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBox1.Location = new System.Drawing.Point(3, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // labelUserAccount
            // 
            this.labelUserAccount.AutoSize = true;
            this.labelUserAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUserAccount.Location = new System.Drawing.Point(7, 7);
            this.labelUserAccount.Name = "labelUserAccount";
            this.labelUserAccount.Size = new System.Drawing.Size(82, 13);
            this.labelUserAccount.TabIndex = 0;
            this.labelUserAccount.Text = "User Account";
            // 
            // changePassword
            // 
            this.changePassword.Location = new System.Drawing.Point(90, 80);
            this.changePassword.Name = "changePassword";
            this.changePassword.Size = new System.Drawing.Size(75, 23);
            this.changePassword.TabIndex = 6;
            this.changePassword.Text = "Change...";
            this.changePassword.UseVisualStyleBackColor = true;
            this.changePassword.Click += new System.EventHandler(this.OnChangePassword);
            // 
            // panelAccountType
            // 
            this.panelAccountType.Controls.Add(this.accountAdmin);
            this.panelAccountType.Controls.Add(this.accountStandard);
            this.panelAccountType.Location = new System.Drawing.Point(28, 131);
            this.panelAccountType.Name = "panelAccountType";
            this.panelAccountType.Size = new System.Drawing.Size(200, 49);
            this.panelAccountType.TabIndex = 8;
            // 
            // accountAdmin
            // 
            this.accountAdmin.AutoSize = true;
            this.accountAdmin.Location = new System.Drawing.Point(3, 25);
            this.accountAdmin.Name = "accountAdmin";
            this.accountAdmin.Size = new System.Drawing.Size(89, 17);
            this.accountAdmin.TabIndex = 1;
            this.accountAdmin.Text = "Administrator";
            this.accountAdmin.UseVisualStyleBackColor = true;
            this.accountAdmin.CheckedChanged += new System.EventHandler(this.OnChangeAccountType);
            // 
            // accountStandard
            // 
            this.accountStandard.AutoSize = true;
            this.accountStandard.Location = new System.Drawing.Point(3, 5);
            this.accountStandard.Name = "accountStandard";
            this.accountStandard.Size = new System.Drawing.Size(94, 17);
            this.accountStandard.TabIndex = 0;
            this.accountStandard.Text = "Standard User";
            this.accountStandard.UseVisualStyleBackColor = true;
            this.accountStandard.CheckedChanged += new System.EventHandler(this.OnChangeAccountType);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(27, 85);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 5;
            this.labelPassword.Text = "Password:";
            // 
            // statusDeleted
            // 
            this.statusDeleted.AutoSize = true;
            this.statusDeleted.Location = new System.Drawing.Point(30, 219);
            this.statusDeleted.Name = "statusDeleted";
            this.statusDeleted.Size = new System.Drawing.Size(62, 17);
            this.statusDeleted.TabIndex = 11;
            this.statusDeleted.Text = "Deleted";
            this.statusDeleted.UseVisualStyleBackColor = true;
            // 
            // labelAccountType
            // 
            this.labelAccountType.AutoSize = true;
            this.labelAccountType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAccountType.Location = new System.Drawing.Point(7, 115);
            this.labelAccountType.Name = "labelAccountType";
            this.labelAccountType.Size = new System.Drawing.Size(84, 13);
            this.labelAccountType.TabIndex = 7;
            this.labelAccountType.Text = "Account Type";
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(25, 29);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 1;
            this.labelUsername.Text = "Username:";
            // 
            // statusActive
            // 
            this.statusActive.AutoSize = true;
            this.statusActive.Location = new System.Drawing.Point(30, 199);
            this.statusActive.Name = "statusActive";
            this.statusActive.Size = new System.Drawing.Size(55, 17);
            this.statusActive.TabIndex = 10;
            this.statusActive.Text = "Active";
            this.statusActive.UseVisualStyleBackColor = true;
            // 
            // email
            // 
            this.email.Location = new System.Drawing.Point(90, 53);
            this.fieldLengthProvider.SetMaxLengthSource(this.email, ShipWorks.Data.Utility.EntityFieldLengthSource.UserEmail);
            this.email.Name = "email";
            this.email.Size = new System.Drawing.Size(243, 21);
            this.email.TabIndex = 4;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(90, 26);
            this.fieldLengthProvider.SetMaxLengthSource(this.username, ShipWorks.Data.Utility.EntityFieldLengthSource.UserName);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(243, 21);
            this.username.TabIndex = 2;
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(8, 56);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(76, 13);
            this.labelEmail.TabIndex = 3;
            this.labelEmail.Text = "Email address:";
            // 
            // labelAccountStatus
            // 
            this.labelAccountStatus.AutoSize = true;
            this.labelAccountStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAccountStatus.Location = new System.Drawing.Point(7, 183);
            this.labelAccountStatus.Name = "labelAccountStatus";
            this.labelAccountStatus.Size = new System.Drawing.Size(93, 13);
            this.labelAccountStatus.TabIndex = 9;
            this.labelAccountStatus.Text = "Account Status";
            // 
            // optionPageRights
            // 
            this.optionPageRights.Controls.Add(this.permissionEditor);
            this.optionPageRights.Controls.Add(this.panelNotUntilNextLogon);
            this.optionPageRights.Controls.Add(this.panelAdminAllRights);
            this.optionPageRights.Location = new System.Drawing.Point(103, 0);
            this.optionPageRights.Name = "optionPageRights";
            this.optionPageRights.Size = new System.Drawing.Size(530, 472);
            this.optionPageRights.TabIndex = 2;
            this.optionPageRights.Text = "Rights";
            // 
            // permissionEditor
            // 
            this.permissionEditor.AutoScroll = true;
            this.permissionEditor.BackColor = System.Drawing.Color.White;
            this.permissionEditor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.permissionEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permissionEditor.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.permissionEditor.Location = new System.Drawing.Point(0, 48);
            this.permissionEditor.Margin = new System.Windows.Forms.Padding(0);
            this.permissionEditor.Name = "permissionEditor";
            this.permissionEditor.Size = new System.Drawing.Size(530, 424);
            this.permissionEditor.TabIndex = 3;
            // 
            // panelNotUntilNextLogon
            // 
            this.panelNotUntilNextLogon.Controls.Add(this.pictureBox2);
            this.panelNotUntilNextLogon.Controls.Add(this.label1);
            this.panelNotUntilNextLogon.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNotUntilNextLogon.Location = new System.Drawing.Point(0, 26);
            this.panelNotUntilNextLogon.Name = "panelNotUntilNextLogon";
            this.panelNotUntilNextLogon.Size = new System.Drawing.Size(530, 22);
            this.panelNotUntilNextLogon.TabIndex = 5;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox2.Location = new System.Drawing.Point(-1, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(265, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Changes are not effective until the user next logs on.";
            // 
            // panelAdminAllRights
            // 
            this.panelAdminAllRights.Controls.Add(this.label2);
            this.panelAdminAllRights.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAdminAllRights.Location = new System.Drawing.Point(0, 0);
            this.panelAdminAllRights.Name = "panelAdminAllRights";
            this.panelAdminAllRights.Size = new System.Drawing.Size(530, 26);
            this.panelAdminAllRights.TabIndex = 4;
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
            // optionPageAudit
            // 
            this.optionPageAudit.Controls.Add(this.auditControl);
            this.optionPageAudit.Location = new System.Drawing.Point(103, 0);
            this.optionPageAudit.Name = "optionPageAudit";
            this.optionPageAudit.Size = new System.Drawing.Size(530, 472);
            this.optionPageAudit.TabIndex = 3;
            this.optionPageAudit.Text = "Audit";
            // 
            // auditControl
            // 
            this.auditControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.auditControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.auditControl.Location = new System.Drawing.Point(0, 0);
            this.auditControl.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.auditControl.Name = "auditControl";
            this.auditControl.Size = new System.Drawing.Size(530, 472);
            this.auditControl.TabIndex = 0;
            // 
            // UserSettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(660, 558);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.titleUserDescription);
            this.Controls.Add(this.titleUsername);
            this.Controls.Add(this.imageUser);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(458, 410);
            this.Name = "UserSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "User Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.imageUser)).EndInit();
            this.optionControl.ResumeLayout(false);
            this.optionPageUser.ResumeLayout(false);
            this.optionPageUser.PerformLayout();
            this.panelNoChangeAccount.ResumeLayout(false);
            this.panelNoChangeAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox1)).EndInit();
            this.panelAccountType.ResumeLayout(false);
            this.panelAccountType.PerformLayout();
            this.optionPageRights.ResumeLayout(false);
            this.panelNotUntilNextLogon.ResumeLayout(false);
            this.panelNotUntilNextLogon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox2)).EndInit();
            this.panelAdminAllRights.ResumeLayout(false);
            this.panelAdminAllRights.PerformLayout();
            this.optionPageAudit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.TextBox email;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Button changePassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.PictureBox imageUser;
        private System.Windows.Forms.Label titleUsername;
        private System.Windows.Forms.RadioButton accountAdmin;
        private System.Windows.Forms.RadioButton accountStandard;
        private System.Windows.Forms.Label labelAccountType;
        private System.Windows.Forms.Label titleUserDescription;
        private System.Windows.Forms.RadioButton statusDeleted;
        private System.Windows.Forms.RadioButton statusActive;
        private System.Windows.Forms.Label labelAccountStatus;
        private System.Windows.Forms.Panel panelAccountType;
        private System.Windows.Forms.Label labelUserAccount;
        private System.Windows.Forms.Panel panelNoChangeAccount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageUser;
        private ShipWorks.UI.Controls.OptionPage optionPageRights;
        private ShipWorks.UI.Controls.OptionPage optionPageAudit;
        private ShipWorks.Users.Security.PermissionEditor permissionEditor;
        private System.Windows.Forms.Panel panelNotUntilNextLogon;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelAdminAllRights;
        private System.Windows.Forms.Label label2;
        private ShipWorks.Users.Audit.AuditControl auditControl;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infoTipUserEmail;
    }
}