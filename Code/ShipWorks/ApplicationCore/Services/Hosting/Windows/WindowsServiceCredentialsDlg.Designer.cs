namespace ShipWorks.ApplicationCore.Services.Hosting.Windows
{
    partial class WindowsServiceCredentialsDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.domainLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.domain = new System.Windows.Forms.TextBox();
            this.domainNote = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.radioLocalSystem = new System.Windows.Forms.RadioButton();
            this.radioWindowsUser = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(325, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the account to use to run the ShipWorks Scheduler Service:";
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(40, 84);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(62, 13);
            this.usernameLabel.TabIndex = 1;
            this.usernameLabel.Text = "User name:";
            this.usernameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // domainLabel
            // 
            this.domainLabel.AutoSize = true;
            this.domainLabel.Location = new System.Drawing.Point(56, 138);
            this.domainLabel.Name = "domainLabel";
            this.domainLabel.Size = new System.Drawing.Size(46, 13);
            this.domainLabel.TabIndex = 5;
            this.domainLabel.Text = "Domain:";
            this.domainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(45, 111);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(57, 13);
            this.passwordLabel.TabIndex = 3;
            this.passwordLabel.Text = "Password:";
            this.passwordLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // username
            // 
            this.username.Enabled = false;
            this.username.Location = new System.Drawing.Point(106, 81);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(216, 21);
            this.username.TabIndex = 2;
            // 
            // password
            // 
            this.password.Enabled = false;
            this.password.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(106, 108);
            this.password.Name = "password";
            this.password.PasswordChar = '•';
            this.password.Size = new System.Drawing.Size(216, 21);
            this.password.TabIndex = 4;
            // 
            // domain
            // 
            this.domain.Enabled = false;
            this.domain.Location = new System.Drawing.Point(106, 135);
            this.domain.Name = "domain";
            this.domain.Size = new System.Drawing.Size(216, 21);
            this.domain.TabIndex = 6;
            // 
            // domainNote
            // 
            this.domainNote.AutoSize = true;
            this.domainNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.domainNote.Location = new System.Drawing.Point(103, 159);
            this.domainNote.Name = "domainNote";
            this.domainNote.Size = new System.Drawing.Size(76, 13);
            this.domainNote.TabIndex = 7;
            this.domainNote.Text = "(blank if none)";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(261, 188);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 8;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // radioLocalSystem
            // 
            this.radioLocalSystem.AutoSize = true;
            this.radioLocalSystem.Checked = true;
            this.radioLocalSystem.Location = new System.Drawing.Point(23, 34);
            this.radioLocalSystem.Name = "radioLocalSystem";
            this.radioLocalSystem.Size = new System.Drawing.Size(87, 17);
            this.radioLocalSystem.TabIndex = 9;
            this.radioLocalSystem.TabStop = true;
            this.radioLocalSystem.Text = "Local System";
            this.radioLocalSystem.UseVisualStyleBackColor = true;
            this.radioLocalSystem.CheckedChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // radioButton1
            // 
            this.radioWindowsUser.AutoSize = true;
            this.radioWindowsUser.Location = new System.Drawing.Point(23, 58);
            this.radioWindowsUser.Name = "radioButton1";
            this.radioWindowsUser.Size = new System.Drawing.Size(93, 17);
            this.radioWindowsUser.TabIndex = 10;
            this.radioWindowsUser.Text = "Windows User";
            this.radioWindowsUser.UseVisualStyleBackColor = true;
            this.radioWindowsUser.CheckedChanged += new System.EventHandler(this.OnAccountChanged);
            // 
            // WindowsServiceCredentialsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 223);
            this.ControlBox = false;
            this.Controls.Add(this.radioWindowsUser);
            this.Controls.Add(this.radioLocalSystem);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.domainNote);
            this.Controls.Add(this.domain);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.domainLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(305, 219);
            this.Name = "WindowsServiceCredentialsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShipWorks Service Credentials";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label domainLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox domain;
        private System.Windows.Forms.Label domainNote;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.RadioButton radioLocalSystem;
        private System.Windows.Forms.RadioButton radioWindowsUser;
    }
}