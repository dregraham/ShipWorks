namespace ShipWorks.Shipping.Carriers.Postal.Usps.Registration
{
    partial class UspsRegistrationSecuritySettingsControl
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
            this.labelUsername = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.labelCredentialsHeading = new System.Windows.Forms.Label();
            this.labelSecurityHeading = new System.Windows.Forms.Label();
            this.labelSecurityInfo2 = new System.Windows.Forms.Label();
            this.labelCredentialsInfo = new System.Windows.Forms.Label();
            this.firstCodewordType = new System.Windows.Forms.ComboBox();
            this.labelFirstQuestionType = new System.Windows.Forms.Label();
            this.labelFirstCodewordValue = new System.Windows.Forms.Label();
            this.firstCodewordValue = new System.Windows.Forms.TextBox();
            this.secondCodewordValue = new System.Windows.Forms.TextBox();
            this.labelSeconCodewordValue = new System.Windows.Forms.Label();
            this.labelSecondQuestionType = new System.Windows.Forms.Label();
            this.secondCodewordType = new System.Windows.Forms.ComboBox();
            this.infotipWeightFormat = new ShipWorks.UI.Controls.InfoTip();
            this.infoTip1 = new ShipWorks.UI.Controls.InfoTip();
            this.retypePassword = new System.Windows.Forms.TextBox();
            this.labelRetypePassword = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelUsername
            // 
            this.labelUsername.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUsername.Location = new System.Drawing.Point(34, 60);
            this.labelUsername.Name = "labelUsername";
            this.labelUsername.Size = new System.Drawing.Size(62, 23);
            this.labelUsername.TabIndex = 0;
            this.labelUsername.Text = "Username:";
            this.labelUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelPassword
            // 
            this.labelPassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPassword.Location = new System.Drawing.Point(37, 87);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(59, 23);
            this.labelPassword.TabIndex = 1;
            this.labelPassword.Text = "Password:";
            this.labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.Location = new System.Drawing.Point(102, 62);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(179, 21);
            this.username.TabIndex = 0;
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(102, 89);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(179, 21);
            this.password.TabIndex = 1;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelCredentialsHeading
            // 
            this.labelCredentialsHeading.AutoSize = true;
            this.labelCredentialsHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCredentialsHeading.Location = new System.Drawing.Point(3, 4);
            this.labelCredentialsHeading.Name = "labelCredentialsHeading";
            this.labelCredentialsHeading.Size = new System.Drawing.Size(144, 13);
            this.labelCredentialsHeading.TabIndex = 5;
            this.labelCredentialsHeading.Text = "Stamps.com Credentials";
            // 
            // labelSecurityHeading
            // 
            this.labelSecurityHeading.AutoSize = true;
            this.labelSecurityHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSecurityHeading.Location = new System.Drawing.Point(3, 150);
            this.labelSecurityHeading.Name = "labelSecurityHeading";
            this.labelSecurityHeading.Size = new System.Drawing.Size(186, 13);
            this.labelSecurityHeading.TabIndex = 6;
            this.labelSecurityHeading.Text = "Stamps.com Security Questions";
            // 
            // labelSecurityInfo2
            // 
            this.labelSecurityInfo2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSecurityInfo2.Location = new System.Drawing.Point(19, 171);
            this.labelSecurityInfo2.Name = "labelSecurityInfo2";
            this.labelSecurityInfo2.Size = new System.Drawing.Size(397, 45);
            this.labelSecurityInfo2.TabIndex = 7;
            this.labelSecurityInfo2.Text = "Every account has two codewords associated with it. These are used during passwor" +
    "d recovery and when identifying yourself to Stamps.com customer support. Questio" +
    "n 1 and Question 2 must be different.\r\n";
            // 
            // labelCredentialsInfo
            // 
            this.labelCredentialsInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCredentialsInfo.Location = new System.Drawing.Point(19, 27);
            this.labelCredentialsInfo.Name = "labelCredentialsInfo";
            this.labelCredentialsInfo.Size = new System.Drawing.Size(397, 32);
            this.labelCredentialsInfo.TabIndex = 8;
            this.labelCredentialsInfo.Text = "Please provide a username and password for your Stamps.com account. This informat" +
    "ion will be specific to Stamps.com.";
            // 
            // firstCodewordType
            // 
            this.firstCodewordType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.firstCodewordType.FormattingEnabled = true;
            this.firstCodewordType.Location = new System.Drawing.Point(130, 222);
            this.firstCodewordType.Name = "firstCodewordType";
            this.firstCodewordType.Size = new System.Drawing.Size(307, 21);
            this.firstCodewordType.TabIndex = 3;
            // 
            // labelFirstQuestionType
            // 
            this.labelFirstQuestionType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFirstQuestionType.Location = new System.Drawing.Point(34, 220);
            this.labelFirstQuestionType.Name = "labelFirstQuestionType";
            this.labelFirstQuestionType.Size = new System.Drawing.Size(90, 23);
            this.labelFirstQuestionType.TabIndex = 10;
            this.labelFirstQuestionType.Text = "Question 1:";
            this.labelFirstQuestionType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelFirstCodewordValue
            // 
            this.labelFirstCodewordValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFirstCodewordValue.Location = new System.Drawing.Point(20, 247);
            this.labelFirstCodewordValue.Name = "labelFirstCodewordValue";
            this.labelFirstCodewordValue.Size = new System.Drawing.Size(104, 23);
            this.labelFirstCodewordValue.TabIndex = 11;
            this.labelFirstCodewordValue.Text = "Question 1 answer:";
            this.labelFirstCodewordValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // firstCodewordValue
            // 
            this.firstCodewordValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstCodewordValue.Location = new System.Drawing.Point(130, 249);
            this.firstCodewordValue.Name = "firstCodewordValue";
            this.firstCodewordValue.Size = new System.Drawing.Size(307, 21);
            this.firstCodewordValue.TabIndex = 4;
            // 
            // secondCodewordValue
            // 
            this.secondCodewordValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.secondCodewordValue.Location = new System.Drawing.Point(130, 315);
            this.secondCodewordValue.Name = "secondCodewordValue";
            this.secondCodewordValue.Size = new System.Drawing.Size(307, 21);
            this.secondCodewordValue.TabIndex = 6;
            // 
            // labelSeconCodewordValue
            // 
            this.labelSeconCodewordValue.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSeconCodewordValue.Location = new System.Drawing.Point(20, 313);
            this.labelSeconCodewordValue.Name = "labelSeconCodewordValue";
            this.labelSeconCodewordValue.Size = new System.Drawing.Size(104, 23);
            this.labelSeconCodewordValue.TabIndex = 15;
            this.labelSeconCodewordValue.Text = "Question 2 answer:";
            this.labelSeconCodewordValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSecondQuestionType
            // 
            this.labelSecondQuestionType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSecondQuestionType.Location = new System.Drawing.Point(34, 286);
            this.labelSecondQuestionType.Name = "labelSecondQuestionType";
            this.labelSecondQuestionType.Size = new System.Drawing.Size(90, 23);
            this.labelSecondQuestionType.TabIndex = 14;
            this.labelSecondQuestionType.Text = "Question 2:";
            this.labelSecondQuestionType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // secondCodewordType
            // 
            this.secondCodewordType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.secondCodewordType.FormattingEnabled = true;
            this.secondCodewordType.Location = new System.Drawing.Point(130, 288);
            this.secondCodewordType.Name = "secondCodewordType";
            this.secondCodewordType.Size = new System.Drawing.Size(310, 21);
            this.secondCodewordType.TabIndex = 5;
            // 
            // infotipWeightFormat
            // 
            this.infotipWeightFormat.Caption = "Must be 2 to 14 characters.";
            this.infotipWeightFormat.Location = new System.Drawing.Point(287, 66);
            this.infotipWeightFormat.Name = "infotipWeightFormat";
            this.infotipWeightFormat.Size = new System.Drawing.Size(12, 12);
            this.infotipWeightFormat.TabIndex = 21;
            this.infotipWeightFormat.Title = "Username";
            // 
            // infoTip1
            // 
            this.infoTip1.Caption = "Must be 6 to 14 characters including one number and one letter.";
            this.infoTip1.Location = new System.Drawing.Point(287, 95);
            this.infoTip1.Name = "infoTip1";
            this.infoTip1.Size = new System.Drawing.Size(12, 12);
            this.infoTip1.TabIndex = 22;
            this.infoTip1.Title = "Password";
            // 
            // retypePassword
            // 
            this.retypePassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.retypePassword.Location = new System.Drawing.Point(102, 116);
            this.retypePassword.Name = "retypePassword";
            this.retypePassword.Size = new System.Drawing.Size(179, 21);
            this.retypePassword.TabIndex = 2;
            this.retypePassword.UseSystemPasswordChar = true;
            // 
            // labelRetypePassword
            // 
            this.labelRetypePassword.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRetypePassword.Location = new System.Drawing.Point(0, 114);
            this.labelRetypePassword.Name = "labelRetypePassword";
            this.labelRetypePassword.Size = new System.Drawing.Size(96, 23);
            this.labelRetypePassword.TabIndex = 23;
            this.labelRetypePassword.Text = "Retype Password:";
            this.labelRetypePassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // StampsRegistrationSecuritySettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.retypePassword);
            this.Controls.Add(this.labelRetypePassword);
            this.Controls.Add(this.infoTip1);
            this.Controls.Add(this.infotipWeightFormat);
            this.Controls.Add(this.secondCodewordValue);
            this.Controls.Add(this.labelSeconCodewordValue);
            this.Controls.Add(this.labelSecondQuestionType);
            this.Controls.Add(this.secondCodewordType);
            this.Controls.Add(this.firstCodewordValue);
            this.Controls.Add(this.labelFirstCodewordValue);
            this.Controls.Add(this.labelFirstQuestionType);
            this.Controls.Add(this.firstCodewordType);
            this.Controls.Add(this.labelCredentialsInfo);
            this.Controls.Add(this.labelSecurityInfo2);
            this.Controls.Add(this.labelSecurityHeading);
            this.Controls.Add(this.labelCredentialsHeading);
            this.Controls.Add(this.password);
            this.Controls.Add(this.username);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelUsername);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "StampsRegistrationSecuritySettingsControl";
            this.Size = new System.Drawing.Size(447, 354);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUsername;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelCredentialsHeading;
        private System.Windows.Forms.Label labelSecurityHeading;
        private System.Windows.Forms.Label labelSecurityInfo2;
        private System.Windows.Forms.Label labelCredentialsInfo;
        private System.Windows.Forms.ComboBox firstCodewordType;
        private System.Windows.Forms.Label labelFirstQuestionType;
        private System.Windows.Forms.Label labelFirstCodewordValue;
        private System.Windows.Forms.TextBox firstCodewordValue;
        private System.Windows.Forms.TextBox secondCodewordValue;
        private System.Windows.Forms.Label labelSeconCodewordValue;
        private System.Windows.Forms.Label labelSecondQuestionType;
        private System.Windows.Forms.ComboBox secondCodewordType;
        private UI.Controls.InfoTip infotipWeightFormat;
        private UI.Controls.InfoTip infoTip1;
        private System.Windows.Forms.TextBox retypePassword;
        private System.Windows.Forms.Label labelRetypePassword;
    }
}
