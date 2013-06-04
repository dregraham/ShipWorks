namespace ShipWorks.Email.Accounts
{
    partial class EmailAccountSettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelEmailAddress = new System.Windows.Forms.Label();
            this.labelDisplayName = new System.Windows.Forms.Label();
            this.labelIncomingServer = new System.Windows.Forms.Label();
            this.labelOutgoingServer = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelUserName = new System.Windows.Forms.Label();
            this.sendTest = new System.Windows.Forms.Button();
            this.borderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.borderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.popSettings = new System.Windows.Forms.Button();
            this.smtpSettings = new System.Windows.Forms.Button();
            this.borderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelSendTest = new System.Windows.Forms.Label();
            this.labelAlias = new System.Windows.Forms.Label();
            this.infotipAccountAlias = new ShipWorks.UI.Controls.InfoTip();
            this.alias = new System.Windows.Forms.TextBox();
            this.testEmailAddress = new System.Windows.Forms.TextBox();
            this.incomingPassword = new System.Windows.Forms.TextBox();
            this.incomingUserName = new System.Windows.Forms.TextBox();
            this.smtpServer = new System.Windows.Forms.TextBox();
            this.incomingServer = new System.Windows.Forms.TextBox();
            this.emailAddress = new System.Windows.Forms.TextBox();
            this.displayName = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.incomingServerType = new System.Windows.Forms.ComboBox();
            this.labelServerType = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelEmailAddress
            // 
            this.labelEmailAddress.AutoSize = true;
            this.labelEmailAddress.Location = new System.Drawing.Point(68, 32);
            this.labelEmailAddress.Name = "labelEmailAddress";
            this.labelEmailAddress.Size = new System.Drawing.Size(77, 13);
            this.labelEmailAddress.TabIndex = 2;
            this.labelEmailAddress.Text = "Email Address:";
            // 
            // labelDisplayName
            // 
            this.labelDisplayName.AutoSize = true;
            this.labelDisplayName.Location = new System.Drawing.Point(83, 6);
            this.labelDisplayName.Name = "labelDisplayName";
            this.labelDisplayName.Size = new System.Drawing.Size(62, 13);
            this.labelDisplayName.TabIndex = 0;
            this.labelDisplayName.Text = "Your name:";
            // 
            // labelIncomingServer
            // 
            this.labelIncomingServer.AutoSize = true;
            this.labelIncomingServer.Location = new System.Drawing.Point(36, 98);
            this.labelIncomingServer.Name = "labelIncomingServer";
            this.labelIncomingServer.Size = new System.Drawing.Size(109, 13);
            this.labelIncomingServer.TabIndex = 7;
            this.labelIncomingServer.Text = "Incoming mail server:";
            // 
            // labelOutgoingServer
            // 
            this.labelOutgoingServer.AutoSize = true;
            this.labelOutgoingServer.Location = new System.Drawing.Point(2, 244);
            this.labelOutgoingServer.Name = "labelOutgoingServer";
            this.labelOutgoingServer.Size = new System.Drawing.Size(147, 13);
            this.labelOutgoingServer.TabIndex = 17;
            this.labelOutgoingServer.Text = "Outgoing mail server (SMTP):";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(89, 177);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 13;
            this.labelPassword.Text = "Password:";
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(83, 152);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(63, 13);
            this.labelUserName.TabIndex = 11;
            this.labelUserName.Text = "User Name:";
            // 
            // sendTest
            // 
            this.sendTest.Location = new System.Drawing.Point(151, 328);
            this.sendTest.Name = "sendTest";
            this.sendTest.Size = new System.Drawing.Size(86, 23);
            this.sendTest.TabIndex = 23;
            this.sendTest.Text = "Send Test";
            this.sendTest.UseVisualStyleBackColor = true;
            this.sendTest.Click += new System.EventHandler(this.OnSendTest);
            // 
            // borderEdge1
            // 
            this.borderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.borderEdge1.AutoSize = false;
            this.borderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.borderEdge1.Location = new System.Drawing.Point(3, 86);
            this.borderEdge1.Name = "borderEdge1";
            this.borderEdge1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.borderEdge1.Size = new System.Drawing.Size(308, 1);
            this.borderEdge1.TabIndex = 6;
            this.borderEdge1.Text = "kryptonBorderEdge1";
            // 
            // borderEdge2
            // 
            this.borderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.borderEdge2.AutoSize = false;
            this.borderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.borderEdge2.Location = new System.Drawing.Point(6, 233);
            this.borderEdge2.Name = "borderEdge2";
            this.borderEdge2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.borderEdge2.Size = new System.Drawing.Size(308, 1);
            this.borderEdge2.TabIndex = 16;
            this.borderEdge2.Text = "kryptonBorderEdge1";
            // 
            // popSettings
            // 
            this.popSettings.Location = new System.Drawing.Point(151, 199);
            this.popSettings.Name = "popSettings";
            this.popSettings.Size = new System.Drawing.Size(121, 23);
            this.popSettings.TabIndex = 15;
            this.popSettings.Text = "Incoming Settings...";
            this.popSettings.UseVisualStyleBackColor = true;
            this.popSettings.Click += new System.EventHandler(this.OnIncomingSettings);
            // 
            // smtpSettings
            // 
            this.smtpSettings.Location = new System.Drawing.Point(150, 266);
            this.smtpSettings.Name = "smtpSettings";
            this.smtpSettings.Size = new System.Drawing.Size(122, 23);
            this.smtpSettings.TabIndex = 19;
            this.smtpSettings.Text = "SMTP Settings...";
            this.smtpSettings.UseVisualStyleBackColor = true;
            this.smtpSettings.Click += new System.EventHandler(this.OnSmtpSettings);
            // 
            // borderEdge3
            // 
            this.borderEdge3.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.borderEdge3.AutoSize = false;
            this.borderEdge3.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.borderEdge3.Location = new System.Drawing.Point(6, 297);
            this.borderEdge3.Name = "borderEdge3";
            this.borderEdge3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.borderEdge3.Size = new System.Drawing.Size(308, 1);
            this.borderEdge3.TabIndex = 20;
            this.borderEdge3.Text = "kryptonBorderEdge2";
            // 
            // labelSendTest
            // 
            this.labelSendTest.AutoSize = true;
            this.labelSendTest.Location = new System.Drawing.Point(22, 307);
            this.labelSendTest.Name = "labelSendTest";
            this.labelSendTest.Size = new System.Drawing.Size(124, 13);
            this.labelSendTest.TabIndex = 21;
            this.labelSendTest.Text = "Send a test message to:";
            // 
            // labelAlias
            // 
            this.labelAlias.AutoSize = true;
            this.labelAlias.Location = new System.Drawing.Point(70, 58);
            this.labelAlias.Name = "labelAlias";
            this.labelAlias.Size = new System.Drawing.Size(75, 13);
            this.labelAlias.TabIndex = 4;
            this.labelAlias.Text = "Account Alias:";
            // 
            // infotipAccountAlias
            // 
            this.infotipAccountAlias.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.infotipAccountAlias.Caption = "This is the name ShipWorks will use when displaying your email account.  It can b" +
                "e anything you want, and is not used in the messages you send.";
            this.infotipAccountAlias.Location = new System.Drawing.Point(315, 60);
            this.infotipAccountAlias.Name = "infotipAccountAlias";
            this.infotipAccountAlias.Size = new System.Drawing.Size(12, 12);
            this.infotipAccountAlias.TabIndex = 231;
            this.infotipAccountAlias.Title = "Account Alias";
            // 
            // alias
            // 
            this.alias.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.alias.Location = new System.Drawing.Point(151, 55);
            this.fieldLengthProvider.SetMaxLengthSource(this.alias, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailAccountName);
            this.alias.Name = "alias";
            this.alias.Size = new System.Drawing.Size(160, 21);
            this.alias.TabIndex = 5;
            // 
            // testEmailAddress
            // 
            this.testEmailAddress.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.testEmailAddress.Location = new System.Drawing.Point(151, 304);
            this.testEmailAddress.Name = "testEmailAddress";
            this.testEmailAddress.Size = new System.Drawing.Size(160, 21);
            this.testEmailAddress.TabIndex = 22;
            // 
            // incomingPassword
            // 
            this.incomingPassword.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.incomingPassword.Location = new System.Drawing.Point(151, 174);
            this.fieldLengthProvider.SetMaxLengthSource(this.incomingPassword, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailPassword);
            this.incomingPassword.Name = "incomingPassword";
            this.incomingPassword.Size = new System.Drawing.Size(160, 21);
            this.incomingPassword.TabIndex = 14;
            this.incomingPassword.UseSystemPasswordChar = true;
            // 
            // incomingUserName
            // 
            this.incomingUserName.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.incomingUserName.Location = new System.Drawing.Point(151, 149);
            this.fieldLengthProvider.SetMaxLengthSource(this.incomingUserName, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailUsername);
            this.incomingUserName.Name = "incomingUserName";
            this.incomingUserName.Size = new System.Drawing.Size(160, 21);
            this.incomingUserName.TabIndex = 12;
            // 
            // smtpServer
            // 
            this.smtpServer.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.smtpServer.Location = new System.Drawing.Point(151, 241);
            this.fieldLengthProvider.SetMaxLengthSource(this.smtpServer, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailServer);
            this.smtpServer.Name = "smtpServer";
            this.smtpServer.Size = new System.Drawing.Size(160, 21);
            this.smtpServer.TabIndex = 18;
            // 
            // incomingServer
            // 
            this.incomingServer.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.incomingServer.Location = new System.Drawing.Point(151, 95);
            this.fieldLengthProvider.SetMaxLengthSource(this.incomingServer, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailServer);
            this.incomingServer.Name = "incomingServer";
            this.incomingServer.Size = new System.Drawing.Size(160, 21);
            this.incomingServer.TabIndex = 8;
            // 
            // emailAddress
            // 
            this.emailAddress.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.emailAddress.Location = new System.Drawing.Point(151, 29);
            this.fieldLengthProvider.SetMaxLengthSource(this.emailAddress, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailAddress);
            this.emailAddress.Name = "emailAddress";
            this.emailAddress.Size = new System.Drawing.Size(160, 21);
            this.emailAddress.TabIndex = 3;
            // 
            // displayName
            // 
            this.displayName.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.displayName.Location = new System.Drawing.Point(151, 3);
            this.fieldLengthProvider.SetMaxLengthSource(this.displayName, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailAccountName);
            this.displayName.Name = "displayName";
            this.displayName.Size = new System.Drawing.Size(160, 21);
            this.displayName.TabIndex = 1;
            // 
            // comboIncomingServerType
            // 
            this.incomingServerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.incomingServerType.FormattingEnabled = true;
            this.incomingServerType.Location = new System.Drawing.Point(151, 122);
            this.incomingServerType.Name = "comboIncomingServerType";
            this.incomingServerType.Size = new System.Drawing.Size(121, 21);
            this.incomingServerType.TabIndex = 10;
            // 
            // labelServerType
            // 
            this.labelServerType.AutoSize = true;
            this.labelServerType.Location = new System.Drawing.Point(75, 125);
            this.labelServerType.Name = "labelServerType";
            this.labelServerType.Size = new System.Drawing.Size(70, 13);
            this.labelServerType.TabIndex = 9;
            this.labelServerType.Text = "Server Type:";
            // 
            // EmailAccountSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelServerType);
            this.Controls.Add(this.incomingServerType);
            this.Controls.Add(this.infotipAccountAlias);
            this.Controls.Add(this.alias);
            this.Controls.Add(this.labelAlias);
            this.Controls.Add(this.testEmailAddress);
            this.Controls.Add(this.labelSendTest);
            this.Controls.Add(this.borderEdge3);
            this.Controls.Add(this.smtpSettings);
            this.Controls.Add(this.popSettings);
            this.Controls.Add(this.borderEdge2);
            this.Controls.Add(this.borderEdge1);
            this.Controls.Add(this.sendTest);
            this.Controls.Add(this.incomingPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.incomingUserName);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.smtpServer);
            this.Controls.Add(this.labelOutgoingServer);
            this.Controls.Add(this.incomingServer);
            this.Controls.Add(this.labelIncomingServer);
            this.Controls.Add(this.emailAddress);
            this.Controls.Add(this.labelEmailAddress);
            this.Controls.Add(this.displayName);
            this.Controls.Add(this.labelDisplayName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.Name = "EmailAccountSettingsControl";
            this.Size = new System.Drawing.Size(333, 362);
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox emailAddress;
        private System.Windows.Forms.Label labelEmailAddress;
        private System.Windows.Forms.TextBox displayName;
        private System.Windows.Forms.Label labelDisplayName;
        private System.Windows.Forms.Label labelIncomingServer;
        private System.Windows.Forms.TextBox incomingServer;
        private System.Windows.Forms.TextBox smtpServer;
        private System.Windows.Forms.Label labelOutgoingServer;
        private System.Windows.Forms.TextBox incomingPassword;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox incomingUserName;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Button sendTest;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderEdge2;
        private System.Windows.Forms.Button popSettings;
        private System.Windows.Forms.Button smtpSettings;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderEdge3;
        private System.Windows.Forms.Label labelSendTest;
        private System.Windows.Forms.TextBox testEmailAddress;
        private System.Windows.Forms.Label labelAlias;
        private System.Windows.Forms.TextBox alias;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infotipAccountAlias;
        private System.Windows.Forms.ComboBox incomingServerType;
        private System.Windows.Forms.Label labelServerType;
    }
}
