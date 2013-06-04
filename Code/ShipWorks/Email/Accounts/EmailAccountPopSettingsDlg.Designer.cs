namespace ShipWorks.Email.Accounts
{
    partial class EmailAccountPopSettingsDlg
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
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.secureRequired = new System.Windows.Forms.CheckBox();
            this.labelIncomingPort = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.labelMethod = new System.Windows.Forms.Label();
            this.securityMethod = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(236, 96);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 6;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(155, 96);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 5;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // secureRequired
            // 
            this.secureRequired.AutoSize = true;
            this.secureRequired.Location = new System.Drawing.Point(9, 39);
            this.secureRequired.Name = "secureRequired";
            this.secureRequired.Size = new System.Drawing.Size(253, 17);
            this.secureRequired.TabIndex = 2;
            this.secureRequired.Text = "Require secure connection when receiving mail.";
            this.secureRequired.UseVisualStyleBackColor = true;
            this.secureRequired.CheckedChanged += new System.EventHandler(this.OnChangeSecureConnection);
            // 
            // labelIncomingPort
            // 
            this.labelIncomingPort.AutoSize = true;
            this.labelIncomingPort.Location = new System.Drawing.Point(8, 15);
            this.labelIncomingPort.Name = "labelIncomingPort";
            this.labelIncomingPort.Size = new System.Drawing.Size(132, 13);
            this.labelIncomingPort.TabIndex = 0;
            this.labelIncomingPort.Text = "Incoming mail server port:";
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(146, 12);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(66, 21);
            this.port.TabIndex = 1;
            // 
            // labelMethod
            // 
            this.labelMethod.AutoSize = true;
            this.labelMethod.Enabled = false;
            this.labelMethod.Location = new System.Drawing.Point(29, 62);
            this.labelMethod.Name = "labelMethod";
            this.labelMethod.Size = new System.Drawing.Size(47, 13);
            this.labelMethod.TabIndex = 3;
            this.labelMethod.Text = "Method:";
            // 
            // securityMethod
            // 
            this.securityMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.securityMethod.Enabled = false;
            this.securityMethod.FormattingEnabled = true;
            this.securityMethod.Items.AddRange(new object[] {
            "Explicit (TLS)",
            "Implicit (SSL)"});
            this.securityMethod.Location = new System.Drawing.Point(80, 59);
            this.securityMethod.Name = "securityMethod";
            this.securityMethod.Size = new System.Drawing.Size(150, 21);
            this.securityMethod.TabIndex = 4;
            // 
            // EmailAccountPopSettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(323, 131);
            this.Controls.Add(this.securityMethod);
            this.Controls.Add(this.labelMethod);
            this.Controls.Add(this.port);
            this.Controls.Add(this.labelIncomingPort);
            this.Controls.Add(this.secureRequired);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailAccountPopSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Incoming Mail Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.CheckBox secureRequired;
        private System.Windows.Forms.Label labelIncomingPort;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label labelMethod;
        private System.Windows.Forms.ComboBox securityMethod;
    }
}