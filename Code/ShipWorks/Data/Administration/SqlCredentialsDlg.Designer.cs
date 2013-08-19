namespace ShipWorks.Data.Administration
{
    partial class SqlCredentialsDlg
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
            this.labelWindowsAuthDescription = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.sqlServerAuth = new System.Windows.Forms.RadioButton();
            this.windowsAuth = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelConnectUsing = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(214, 189);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(295, 189);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelWindowsAuthDescription
            // 
            this.labelWindowsAuthDescription.ForeColor = System.Drawing.Color.DimGray;
            this.labelWindowsAuthDescription.Location = new System.Drawing.Point(50, 140);
            this.labelWindowsAuthDescription.Name = "labelWindowsAuthDescription";
            this.labelWindowsAuthDescription.Size = new System.Drawing.Size(336, 28);
            this.labelWindowsAuthDescription.TabIndex = 23;
            this.labelWindowsAuthDescription.Text = "You must be currently logged in to Windows as a user that has access to SQL Serve" +
    "r.";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(108, 59);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(217, 21);
            this.username.TabIndex = 19;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(108, 85);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(217, 21);
            this.password.TabIndex = 21;
            this.password.UseSystemPasswordChar = true;
            // 
            // sqlServerAuth
            // 
            this.sqlServerAuth.Location = new System.Drawing.Point(32, 31);
            this.sqlServerAuth.Name = "sqlServerAuth";
            this.sqlServerAuth.Size = new System.Drawing.Size(236, 24);
            this.sqlServerAuth.TabIndex = 17;
            this.sqlServerAuth.Text = "SQL Server authentication";
            this.sqlServerAuth.CheckedChanged += new System.EventHandler(this.OnChangeSqlAuthType);
            // 
            // windowsAuth
            // 
            this.windowsAuth.Location = new System.Drawing.Point(32, 118);
            this.windowsAuth.Name = "windowsAuth";
            this.windowsAuth.Size = new System.Drawing.Size(228, 24);
            this.windowsAuth.TabIndex = 22;
            this.windowsAuth.Text = "Windows authentication";
            this.windowsAuth.CheckedChanged += new System.EventHandler(this.OnChangeSqlAuthType);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(48, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Username:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(50, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 20;
            this.label8.Text = "Password:";
            // 
            // labelConnectUsing
            // 
            this.labelConnectUsing.AutoSize = true;
            this.labelConnectUsing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelConnectUsing.Location = new System.Drawing.Point(12, 9);
            this.labelConnectUsing.Name = "labelConnectUsing";
            this.labelConnectUsing.Size = new System.Drawing.Size(160, 13);
            this.labelConnectUsing.TabIndex = 16;
            this.labelConnectUsing.Text = "Log on to SQL Server using:";
            // 
            // SqlCredentialsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(382, 224);
            this.Controls.Add(this.labelWindowsAuthDescription);
            this.Controls.Add(this.username);
            this.Controls.Add(this.password);
            this.Controls.Add(this.sqlServerAuth);
            this.Controls.Add(this.windowsAuth);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.labelConnectUsing);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SqlCredentialsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SQL Server Account";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelWindowsAuthDescription;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.RadioButton sqlServerAuth;
        private System.Windows.Forms.RadioButton windowsAuth;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelConnectUsing;
    }
}