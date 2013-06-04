namespace ShipWorks.Users.Logon
{
    partial class LogonDlg
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
            this.headerImage = new System.Windows.Forms.PictureBox();
            this.labelUsername = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.automaticLogon = new System.Windows.Forms.CheckBox();
            this.logon = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.userlist = new System.Windows.Forms.ComboBox();
            this.forgotUsername = new System.Windows.Forms.Label();
            this.forgotPassword = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.infoTipLogOn = new ShipWorks.UI.Controls.InfoTip();
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // headerImage
            // 
            this.headerImage.Cursor = System.Windows.Forms.Cursors.Default;
            this.headerImage.Image = global::ShipWorks.Properties.Resources.user_lock_48;
            this.headerImage.Location = new System.Drawing.Point(9, 18);
            this.headerImage.Name = "headerImage";
            this.headerImage.Size = new System.Drawing.Size(48, 48);
            this.headerImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.headerImage.TabIndex = 5;
            this.headerImage.TabStop = false;
            // 
            // labelUsername
            // 
            this.labelUsername.AutoSize = true;
            this.labelUsername.Location = new System.Drawing.Point(71, 18);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(59, 13);
            this.labelUsername.TabIndex = 160;
            this.labelUsername.Text = "Username:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(136, 15);
            this.fieldLengthProvider.SetMaxLengthSource(this.username, ShipWorks.Data.Utility.EntityFieldLengthSource.EmailUsername);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(245, 21);
            this.username.TabIndex = 0;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(136, 42);
            this.fieldLengthProvider.SetMaxLengthSource(this.password, ShipWorks.Data.Utility.EntityFieldLengthSource.UserPassword);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(245, 21);
            this.password.TabIndex = 1;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(73, 45);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 162;
            this.labelPassword.Text = "Password:";
            // 
            // automaticLogon
            // 
            this.automaticLogon.AutoSize = true;
            this.automaticLogon.Location = new System.Drawing.Point(136, 69);
            this.automaticLogon.Name = "automaticLogon";
            this.automaticLogon.Size = new System.Drawing.Size(228, 17);
            this.automaticLogon.TabIndex = 2;
            this.automaticLogon.Text = "Log me on automatically on this computer.";
            this.automaticLogon.UseVisualStyleBackColor = true;
            // 
            // logon
            // 
            this.logon.Location = new System.Drawing.Point(240, 99);
            this.logon.Name = "logon";
            this.logon.Size = new System.Drawing.Size(75, 23);
            this.logon.TabIndex = 3;
            this.logon.Text = "Log On";
            this.logon.UseVisualStyleBackColor = true;
            this.logon.Click += new System.EventHandler(this.OnLogon);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(322, 99);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 4;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // userlist
            // 
            this.userlist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userlist.FormattingEnabled = true;
            this.userlist.Location = new System.Drawing.Point(136, 15);
            this.userlist.Name = "userlist";
            this.userlist.Size = new System.Drawing.Size(245, 21);
            this.userlist.TabIndex = 0;
            // 
            // forgotUsername
            // 
            this.forgotUsername.AutoSize = true;
            this.forgotUsername.Cursor = System.Windows.Forms.Cursors.Hand;
            this.forgotUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.forgotUsername.ForeColor = System.Drawing.Color.Blue;
            this.forgotUsername.Location = new System.Drawing.Point(6, 109);
            this.forgotUsername.Name = "forgotUsername";
            this.forgotUsername.Size = new System.Drawing.Size(90, 13);
            this.forgotUsername.TabIndex = 168;
            this.forgotUsername.Text = "Forgot Username";
            this.forgotUsername.Click += new System.EventHandler(this.OnForgotUsername);
            // 
            // forgotPassword
            // 
            this.forgotPassword.AutoSize = true;
            this.forgotPassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.forgotPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.forgotPassword.ForeColor = System.Drawing.Color.Blue;
            this.forgotPassword.Location = new System.Drawing.Point(94, 109);
            this.forgotPassword.Name = "forgotPassword";
            this.forgotPassword.Size = new System.Drawing.Size(88, 13);
            this.forgotPassword.TabIndex = 169;
            this.forgotPassword.Text = "Forgot Password";
            this.forgotPassword.Click += new System.EventHandler(this.OnForgotPassword);
            // 
            // infoTipLogOn
            // 
            this.infoTipLogOn.Caption = "To stop automatically logging on, Log Off using the main application menu.";
            this.infoTipLogOn.Location = new System.Drawing.Point(362, 72);
            this.infoTipLogOn.Name = "infoTipLogOn";
            this.infoTipLogOn.Size = new System.Drawing.Size(12, 12);
            this.infoTipLogOn.TabIndex = 170;
            this.infoTipLogOn.Title = "Automatic Log On";
            // 
            // LogonDlg
            // 
            this.AcceptButton = this.logon;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(409, 134);
            this.Controls.Add(this.infoTipLogOn);
            this.Controls.Add(this.forgotPassword);
            this.Controls.Add(this.forgotUsername);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.logon);
            this.Controls.Add(this.automaticLogon);
            this.Controls.Add(this.password);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.headerImage);
            this.Controls.Add(this.labelUsername);
            this.Controls.Add(this.userlist);
            this.Controls.Add(this.username);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogonDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log On to ShipWorks";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.headerImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox headerImage;
        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.CheckBox automaticLogon;
        private System.Windows.Forms.Button logon;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.ComboBox userlist;
        private System.Windows.Forms.Label forgotUsername;
        private System.Windows.Forms.Label forgotPassword;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infoTipLogOn;
    }
}