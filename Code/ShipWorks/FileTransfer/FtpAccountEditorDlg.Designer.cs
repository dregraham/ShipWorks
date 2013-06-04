﻿namespace ShipWorks.FileTransfer
{
    partial class FtpAccountEditorDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.host = new System.Windows.Forms.TextBox();
            this.labelHost = new System.Windows.Forms.Label();
            this.borderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.port = new System.Windows.Forms.TextBox();
            this.labelPort = new System.Windows.Forms.Label();
            this.securityMethod = new System.Windows.Forms.ComboBox();
            this.labelMethod = new System.Windows.Forms.Label();
            this.secureRequired = new System.Windows.Forms.CheckBox();
            this.labelSecurity = new System.Windows.Forms.Label();
            this.borderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelTransfer = new System.Windows.Forms.Label();
            this.transferMethod = new System.Windows.Forms.ComboBox();
            this.testConnection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(192, 220);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(273, 220);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(75, 68);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(148, 21);
            this.password.TabIndex = 13;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(12, 71);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 12;
            this.labelPassword.Text = "Password:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(75, 41);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(148, 21);
            this.username.TabIndex = 11;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(15, 44);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 10;
            this.labelUsername.Text = "Username:";
            // 
            // host
            // 
            this.host.Location = new System.Drawing.Point(75, 14);
            this.host.Name = "host";
            this.host.Size = new System.Drawing.Size(244, 21);
            this.host.TabIndex = 8;
            // 
            // labelHost
            // 
            this.labelHost.AutoSize = true;
            this.labelHost.Location = new System.Drawing.Point(15, 17);
            this.labelHost.Name = "labelHost";
            this.labelHost.Size = new System.Drawing.Size(54, 13);
            this.labelHost.TabIndex = 7;
            this.labelHost.Text = "FTP Host:";
            // 
            // borderEdge2
            // 
            this.borderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.borderEdge2.AutoSize = false;
            this.borderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.borderEdge2.Location = new System.Drawing.Point(15, 96);
            this.borderEdge2.Name = "borderEdge2";
            this.borderEdge2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.borderEdge2.Size = new System.Drawing.Size(328, 1);
            this.borderEdge2.TabIndex = 17;
            this.borderEdge2.Text = "kryptonBorderEdge1";
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(75, 103);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(148, 21);
            this.port.TabIndex = 19;
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(38, 106);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(31, 13);
            this.labelPort.TabIndex = 18;
            this.labelPort.Text = "Port:";
            // 
            // securityMethod
            // 
            this.securityMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.securityMethod.Enabled = false;
            this.securityMethod.FormattingEnabled = true;
            this.securityMethod.Items.AddRange(new object[] {
            "Explicit (TLS)",
            "Implicit (SSL)"});
            this.securityMethod.Location = new System.Drawing.Point(146, 150);
            this.securityMethod.Name = "securityMethod";
            this.securityMethod.Size = new System.Drawing.Size(150, 21);
            this.securityMethod.TabIndex = 22;
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Enabled = false;
            this.labelMethod.Location = new System.Drawing.Point(95, 153);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(47, 13);
            this.labelMethod.TabIndex = 21;
            this.labelMethod.Text = "Method:";
            // 
            // secureRequired
            // 
            this.secureRequired.AutoSize = true;
            this.secureRequired.Location = new System.Drawing.Point(75, 130);
            this.secureRequired.Name = "secureRequired";
            this.secureRequired.Size = new System.Drawing.Size(166, 17);
            this.secureRequired.TabIndex = 20;
            this.secureRequired.Text = "Require a secure connection:";
            this.secureRequired.UseVisualStyleBackColor = true;
            this.secureRequired.CheckedChanged += new System.EventHandler(this.OnChangeSecureConnection);
            // 
            // labelSecurity
            // 
            this.labelSecurity.AutoSize = true;
            this.labelSecurity.Location = new System.Drawing.Point(19, 131);
            this.labelSecurity.Name = "labelSecurity";
            this.labelSecurity.Size = new System.Drawing.Size(50, 13);
            this.labelSecurity.TabIndex = 23;
            this.labelSecurity.Text = "Security:";
            // 
            // borderEdge1
            // 
            this.borderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.borderEdge1.AutoSize = false;
            this.borderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.borderEdge1.Location = new System.Drawing.Point(15, 209);
            this.borderEdge1.Name = "borderEdge1";
            this.borderEdge1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Silver;
            this.borderEdge1.Size = new System.Drawing.Size(328, 1);
            this.borderEdge1.TabIndex = 24;
            this.borderEdge1.Text = "kryptonBorderEdge1";
            // 
            // labelTransfer
            // 
            this.labelTransfer.AutoSize = true;
            this.labelTransfer.Location = new System.Drawing.Point(17, 182);
            this.labelTransfer.Name = "labelTransfer";
            this.labelTransfer.Size = new System.Drawing.Size(52, 13);
            this.labelTransfer.TabIndex = 26;
            this.labelTransfer.Text = "Transfer:";
            // 
            // transferMethod
            // 
            this.transferMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transferMethod.FormattingEnabled = true;
            this.transferMethod.Items.AddRange(new object[] {
            "Active",
            "Passive"});
            this.transferMethod.Location = new System.Drawing.Point(75, 179);
            this.transferMethod.Name = "transferMethod";
            this.transferMethod.Size = new System.Drawing.Size(150, 21);
            this.transferMethod.TabIndex = 27;
            // 
            // testConnection
            // 
            this.testConnection.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.testConnection.Location = new System.Drawing.Point(12, 220);
            this.testConnection.Name = "testConnection";
            this.testConnection.Size = new System.Drawing.Size(111, 23);
            this.testConnection.TabIndex = 28;
            this.testConnection.Text = "Test Connection";
            this.testConnection.UseVisualStyleBackColor = true;
            this.testConnection.Click += new System.EventHandler(this.OnTestConnection);
            // 
            // FtpAccountEditorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 255);
            this.Controls.Add(this.testConnection);
            this.Controls.Add(this.transferMethod);
            this.Controls.Add(this.labelTransfer);
            this.Controls.Add(this.borderEdge1);
            this.Controls.Add(this.labelSecurity);
            this.Controls.Add(this.securityMethod);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.secureRequired);
            this.Controls.Add(this.port);
            this.Controls.Add(this.labelPort);
            this.Controls.Add(this.borderEdge2);
            this.Controls.Add(this.password);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.username);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.host);
            this.Controls.Add(this.labelHost);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FtpAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FTP Account";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox host;
        private System.Windows.Forms.Label labelHost;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderEdge2;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.ComboBox securityMethod;
        private System.Windows.Forms.Label labelMethod;
        private System.Windows.Forms.CheckBox secureRequired;
        private System.Windows.Forms.Label labelSecurity;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge borderEdge1;
        private System.Windows.Forms.Label labelTransfer;
        private System.Windows.Forms.ComboBox transferMethod;
        private System.Windows.Forms.Button testConnection;
    }
}