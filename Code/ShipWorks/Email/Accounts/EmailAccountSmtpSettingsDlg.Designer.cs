namespace ShipWorks.Email.Accounts
{
    partial class EmailAccountSmtpSettingsDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.port = new System.Windows.Forms.TextBox();
            this.labelOutgoingPort = new System.Windows.Forms.Label();
            this.smtpAuthentication = new System.Windows.Forms.CheckBox();
            this.borderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.authenticateAsIncoming = new System.Windows.Forms.RadioButton();
            this.authenticateSpecifyLogon = new System.Windows.Forms.RadioButton();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.authenticatePopBeforeSmtp = new System.Windows.Forms.RadioButton();
            this.panelAuthentication = new System.Windows.Forms.Panel();
            this.securityMethod = new System.Windows.Forms.ComboBox();
            this.labelMethod = new System.Windows.Forms.Label();
            this.secureRequired = new System.Windows.Forms.CheckBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.panelAuthentication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(194, 251);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 8;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(275, 251);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 9;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(149, 12);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(66, 21);
            this.port.TabIndex = 1;
            // 
            // labelOutgoingPort
            // 
            this.labelOutgoingPort.AutoSize = true;
            this.labelOutgoingPort.Location = new System.Drawing.Point(11, 15);
            this.labelOutgoingPort.Name = "labelOutgoingPort";
            this.labelOutgoingPort.Size = new System.Drawing.Size(133, 13);
            this.labelOutgoingPort.TabIndex = 0;
            this.labelOutgoingPort.Text = "Outgoing mail server port:";
            // 
            // smtpAuthentication
            // 
            this.smtpAuthentication.AutoSize = true;
            this.smtpAuthentication.Location = new System.Drawing.Point(12, 98);
            this.smtpAuthentication.Name = "smtpAuthentication";
            this.smtpAuthentication.Size = new System.Drawing.Size(283, 17);
            this.smtpAuthentication.TabIndex = 6;
            this.smtpAuthentication.Text = "My outgoing SMTP mail server requires authentication";
            this.smtpAuthentication.UseVisualStyleBackColor = true;
            this.smtpAuthentication.CheckedChanged += new System.EventHandler(this.OnChangeAuthenticationType);
            // 
            // borderEdge2
            // 
            this.borderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.borderEdge2.AutoSize = false;
            this.borderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.borderEdge2.Location = new System.Drawing.Point(12, 89);
            this.borderEdge2.Name = "borderEdge2";
            this.borderEdge2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.borderEdge2.Size = new System.Drawing.Size(340, 1);
            this.borderEdge2.TabIndex = 5;
            this.borderEdge2.Text = "kryptonBorderEdge1";
            // 
            // authenticateAsIncoming
            // 
            this.authenticateAsIncoming.AutoSize = true;
            this.authenticateAsIncoming.Location = new System.Drawing.Point(3, 3);
            this.authenticateAsIncoming.Name = "authenticateAsIncoming";
            this.authenticateAsIncoming.Size = new System.Drawing.Size(307, 17);
            this.authenticateAsIncoming.TabIndex = 0;
            this.authenticateAsIncoming.TabStop = true;
            this.authenticateAsIncoming.Text = "Use same user name and password as incoming mail server";
            this.authenticateAsIncoming.UseVisualStyleBackColor = true;
            // 
            // authenticateSpecifyLogon
            // 
            this.authenticateSpecifyLogon.AutoSize = true;
            this.authenticateSpecifyLogon.Location = new System.Drawing.Point(3, 26);
            this.authenticateSpecifyLogon.Name = "authenticateSpecifyLogon";
            this.authenticateSpecifyLogon.Size = new System.Drawing.Size(85, 17);
            this.authenticateSpecifyLogon.TabIndex = 1;
            this.authenticateSpecifyLogon.TabStop = true;
            this.authenticateSpecifyLogon.Text = "Log on using";
            this.authenticateSpecifyLogon.UseVisualStyleBackColor = true;
            this.authenticateSpecifyLogon.CheckedChanged += new System.EventHandler(this.OnChangeAuthenticationType);
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(97, 73);
            this.fieldLengthProvider.SetMaxLengthSource(this.password, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailPassword);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(145, 21);
            this.password.TabIndex = 5;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(35, 76);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 4;
            this.labelPassword.Text = "Password:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(97, 48);
            this.fieldLengthProvider.SetMaxLengthSource(this.username, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailUsername);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(145, 21);
            this.username.TabIndex = 3;
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(29, 51);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(63, 13);
            this.labelUserName.TabIndex = 2;
            this.labelUserName.Text = "User Name:";
            // 
            // authenticatePopBeforeSmtp
            // 
            this.authenticatePopBeforeSmtp.AutoSize = true;
            this.authenticatePopBeforeSmtp.Location = new System.Drawing.Point(3, 100);
            this.authenticatePopBeforeSmtp.Name = "authenticatePopBeforeSmtp";
            this.authenticatePopBeforeSmtp.Size = new System.Drawing.Size(265, 17);
            this.authenticatePopBeforeSmtp.TabIndex = 6;
            this.authenticatePopBeforeSmtp.TabStop = true;
            this.authenticatePopBeforeSmtp.Text = "Log on to incoming mail server before sending mail";
            this.authenticatePopBeforeSmtp.UseVisualStyleBackColor = true;
            // 
            // panelAuthentication
            // 
            this.panelAuthentication.Controls.Add(this.authenticateAsIncoming);
            this.panelAuthentication.Controls.Add(this.authenticatePopBeforeSmtp);
            this.panelAuthentication.Controls.Add(this.authenticateSpecifyLogon);
            this.panelAuthentication.Controls.Add(this.password);
            this.panelAuthentication.Controls.Add(this.labelUserName);
            this.panelAuthentication.Controls.Add(this.labelPassword);
            this.panelAuthentication.Controls.Add(this.username);
            this.panelAuthentication.Location = new System.Drawing.Point(27, 116);
            this.panelAuthentication.Name = "panelAuthentication";
            this.panelAuthentication.Size = new System.Drawing.Size(323, 128);
            this.panelAuthentication.TabIndex = 7;
            // 
            // securityMethod
            // 
            this.securityMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.securityMethod.Enabled = false;
            this.securityMethod.FormattingEnabled = true;
            this.securityMethod.Items.AddRange(new object[] {
            "Explicit (TLS)",
            "Implicit (SSL)"});
            this.securityMethod.Location = new System.Drawing.Point(85, 59);
            this.securityMethod.Name = "securityMethod";
            this.securityMethod.Size = new System.Drawing.Size(150, 21);
            this.securityMethod.TabIndex = 4;
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Enabled = false;
            this.labelMethod.Location = new System.Drawing.Point(34, 62);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(47, 13);
            this.labelMethod.TabIndex = 3;
            this.labelMethod.Text = "Method:";
            // 
            // secureRequired
            // 
            this.secureRequired.AutoSize = true;
            this.secureRequired.Location = new System.Drawing.Point(14, 39);
            this.secureRequired.Name = "secureRequired";
            this.secureRequired.Size = new System.Drawing.Size(253, 17);
            this.secureRequired.TabIndex = 2;
            this.secureRequired.Text = "Require secure connection when receiving mail.";
            this.secureRequired.UseVisualStyleBackColor = true;
            this.secureRequired.CheckedChanged += new System.EventHandler(this.OnChangeSecureConnection);
            // 
            // EmailAccountSmtpSettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(362, 284);
            this.Controls.Add(this.securityMethod);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.secureRequired);
            this.Controls.Add(this.panelAuthentication);
            this.Controls.Add(this.borderEdge2);
            this.Controls.Add(this.smtpAuthentication);
            this.Controls.Add(this.port);
            this.Controls.Add(this.labelOutgoingPort);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailAccountSmtpSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Outgoing Mail (SMTP) Settings";
            this.panelAuthentication.ResumeLayout(false);
            this.panelAuthentication.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label labelOutgoingPort;
        private System.Windows.Forms.CheckBox smtpAuthentication;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderEdge2;
        private System.Windows.Forms.RadioButton authenticateAsIncoming;
        private System.Windows.Forms.RadioButton authenticateSpecifyLogon;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.RadioButton authenticatePopBeforeSmtp;
        private System.Windows.Forms.Panel panelAuthentication;
        private System.Windows.Forms.ComboBox securityMethod;
        private System.Windows.Forms.Label labelMethod;
        private System.Windows.Forms.CheckBox secureRequired;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}