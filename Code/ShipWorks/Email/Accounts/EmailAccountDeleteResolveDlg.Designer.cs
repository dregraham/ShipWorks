namespace ShipWorks.Email.Accounts
{
    partial class EmailAccountDeleteResolveDlg
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
            this.delete = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.panelUnsentMessages = new System.Windows.Forms.Panel();
            this.labelUnsentMessages = new System.Windows.Forms.Label();
            this.radioUnsentDelete = new System.Windows.Forms.RadioButton();
            this.accountsForUnsent = new System.Windows.Forms.ComboBox();
            this.radioUnsentChange = new System.Windows.Forms.RadioButton();
            this.labelUnsentMessagesInfo = new System.Windows.Forms.Label();
            this.labelAccountName = new System.Windows.Forms.Label();
            this.labelAccountEmail = new System.Windows.Forms.Label();
            this.panelTemplates = new System.Windows.Forms.Panel();
            this.labelTemplates = new System.Windows.Forms.Label();
            this.radioTemplatesDelete = new System.Windows.Forms.RadioButton();
            this.accountsForTemplates = new System.Windows.Forms.ComboBox();
            this.radioTemplatesChange = new System.Windows.Forms.RadioButton();
            this.labelTemplatesInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.panelUnsentMessages.SuspendLayout();
            this.panelTemplates.SuspendLayout();
            this.SuspendLayout();
            // 
            // delete
            // 
            this.delete.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.delete.Location = new System.Drawing.Point(244, 291);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(75, 23);
            this.delete.TabIndex = 4;
            this.delete.Text = "Delete";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.OnDelete);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(325, 291);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 5;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.mail_server_error1;
            this.pictureBox.Location = new System.Drawing.Point(12, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(32, 32);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // panelUnsentMessages
            // 
            this.panelUnsentMessages.Controls.Add(this.labelUnsentMessages);
            this.panelUnsentMessages.Controls.Add(this.radioUnsentDelete);
            this.panelUnsentMessages.Controls.Add(this.accountsForUnsent);
            this.panelUnsentMessages.Controls.Add(this.radioUnsentChange);
            this.panelUnsentMessages.Controls.Add(this.labelUnsentMessagesInfo);
            this.panelUnsentMessages.Location = new System.Drawing.Point(11, 53);
            this.panelUnsentMessages.Name = "panelUnsentMessages";
            this.panelUnsentMessages.Size = new System.Drawing.Size(378, 111);
            this.panelUnsentMessages.TabIndex = 2;
            // 
            // labelUnsentMessages
            // 
            this.labelUnsentMessages.AutoSize = true;
            this.labelUnsentMessages.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelUnsentMessages.Location = new System.Drawing.Point(2, 6);
            this.labelUnsentMessages.Name = "labelUnsentMessages";
            this.labelUnsentMessages.Size = new System.Drawing.Size(106, 13);
            this.labelUnsentMessages.TabIndex = 0;
            this.labelUnsentMessages.Text = "Unsent Messages";
            // 
            // radioUnsentDelete
            // 
            this.radioUnsentDelete.AutoSize = true;
            this.radioUnsentDelete.Location = new System.Drawing.Point(34, 86);
            this.radioUnsentDelete.Name = "radioUnsentDelete";
            this.radioUnsentDelete.Size = new System.Drawing.Size(165, 17);
            this.radioUnsentDelete.TabIndex = 4;
            this.radioUnsentDelete.TabStop = true;
            this.radioUnsentDelete.Text = "Delete the unsent messages.";
            this.radioUnsentDelete.UseVisualStyleBackColor = true;
            this.radioUnsentDelete.Click += new System.EventHandler(this.OnChangeRadioOption);
            // 
            // accountsForUnsent
            // 
            this.accountsForUnsent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountsForUnsent.FormattingEnabled = true;
            this.accountsForUnsent.Location = new System.Drawing.Point(53, 62);
            this.accountsForUnsent.Name = "accountsForUnsent";
            this.accountsForUnsent.Size = new System.Drawing.Size(253, 21);
            this.accountsForUnsent.TabIndex = 3;
            // 
            // radioUnsentChange
            // 
            this.radioUnsentChange.AutoSize = true;
            this.radioUnsentChange.Location = new System.Drawing.Point(34, 43);
            this.radioUnsentChange.Name = "radioUnsentChange";
            this.radioUnsentChange.Size = new System.Drawing.Size(244, 17);
            this.radioUnsentChange.TabIndex = 2;
            this.radioUnsentChange.TabStop = true;
            this.radioUnsentChange.Text = "Send the messages with this account instead:";
            this.radioUnsentChange.UseVisualStyleBackColor = true;
            this.radioUnsentChange.Click += new System.EventHandler(this.OnChangeRadioOption);
            // 
            // labelUnsentMessagesInfo
            // 
            this.labelUnsentMessagesInfo.AutoSize = true;
            this.labelUnsentMessagesInfo.Location = new System.Drawing.Point(19, 25);
            this.labelUnsentMessagesInfo.Name = "labelUnsentMessagesInfo";
            this.labelUnsentMessagesInfo.Size = new System.Drawing.Size(296, 13);
            this.labelUnsentMessagesInfo.TabIndex = 1;
            this.labelUnsentMessagesInfo.Text = "This account is used by one or more unsent email messages.";
            // 
            // labelAccountName
            // 
            this.labelAccountName.AutoSize = true;
            this.labelAccountName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelAccountName.Location = new System.Drawing.Point(49, 13);
            this.labelAccountName.Name = "labelAccountName";
            this.labelAccountName.Size = new System.Drawing.Size(47, 13);
            this.labelAccountName.TabIndex = 0;
            this.labelAccountName.Text = "Account";
            // 
            // labelAccountEmail
            // 
            this.labelAccountEmail.AutoSize = true;
            this.labelAccountEmail.Location = new System.Drawing.Point(50, 30);
            this.labelAccountEmail.Name = "labelAccountEmail";
            this.labelAccountEmail.Size = new System.Drawing.Size(143, 13);
            this.labelAccountEmail.TabIndex = 1;
            this.labelAccountEmail.Text = "someaddress@example.com";
            // 
            // panelTemplates
            // 
            this.panelTemplates.Controls.Add(this.labelTemplates);
            this.panelTemplates.Controls.Add(this.radioTemplatesDelete);
            this.panelTemplates.Controls.Add(this.accountsForTemplates);
            this.panelTemplates.Controls.Add(this.radioTemplatesChange);
            this.panelTemplates.Controls.Add(this.labelTemplatesInfo);
            this.panelTemplates.Location = new System.Drawing.Point(12, 170);
            this.panelTemplates.Name = "panelTemplates";
            this.panelTemplates.Size = new System.Drawing.Size(396, 111);
            this.panelTemplates.TabIndex = 3;
            // 
            // labelTemplates
            // 
            this.labelTemplates.AutoSize = true;
            this.labelTemplates.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelTemplates.Location = new System.Drawing.Point(2, 6);
            this.labelTemplates.Name = "labelTemplates";
            this.labelTemplates.Size = new System.Drawing.Size(67, 13);
            this.labelTemplates.TabIndex = 0;
            this.labelTemplates.Text = "Templates";
            // 
            // radioTemplatesDelete
            // 
            this.radioTemplatesDelete.AutoSize = true;
            this.radioTemplatesDelete.Location = new System.Drawing.Point(33, 86);
            this.radioTemplatesDelete.Name = "radioTemplatesDelete";
            this.radioTemplatesDelete.Size = new System.Drawing.Size(244, 17);
            this.radioTemplatesDelete.TabIndex = 4;
            this.radioTemplatesDelete.TabStop = true;
            this.radioTemplatesDelete.Text = "Set the templates to use the default account.";
            this.radioTemplatesDelete.UseVisualStyleBackColor = true;
            this.radioTemplatesDelete.Click += new System.EventHandler(this.OnChangeRadioOption);
            // 
            // accountsForTemplates
            // 
            this.accountsForTemplates.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountsForTemplates.FormattingEnabled = true;
            this.accountsForTemplates.Location = new System.Drawing.Point(52, 61);
            this.accountsForTemplates.Name = "accountsForTemplates";
            this.accountsForTemplates.Size = new System.Drawing.Size(253, 21);
            this.accountsForTemplates.TabIndex = 3;
            // 
            // radioTemplatesChange
            // 
            this.radioTemplatesChange.AutoSize = true;
            this.radioTemplatesChange.Location = new System.Drawing.Point(33, 42);
            this.radioTemplatesChange.Name = "radioTemplatesChange";
            this.radioTemplatesChange.Size = new System.Drawing.Size(246, 17);
            this.radioTemplatesChange.TabIndex = 2;
            this.radioTemplatesChange.TabStop = true;
            this.radioTemplatesChange.Text = "Set the templates to use this account instead:";
            this.radioTemplatesChange.UseVisualStyleBackColor = true;
            this.radioTemplatesChange.Click += new System.EventHandler(this.OnChangeRadioOption);
            // 
            // labelTemplatesInfo
            // 
            this.labelTemplatesInfo.AutoSize = true;
            this.labelTemplatesInfo.Location = new System.Drawing.Point(18, 24);
            this.labelTemplatesInfo.Name = "labelTemplatesInfo";
            this.labelTemplatesInfo.Size = new System.Drawing.Size(372, 13);
            this.labelTemplatesInfo.TabIndex = 1;
            this.labelTemplatesInfo.Text = "This account is configured as the default account by one or more templates.";
            // 
            // EmailAccountDeleteResolveDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(412, 326);
            this.Controls.Add(this.panelTemplates);
            this.Controls.Add(this.labelAccountEmail);
            this.Controls.Add(this.labelAccountName);
            this.Controls.Add(this.panelUnsentMessages);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.delete);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EmailAccountDeleteResolveDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Delete Email Account";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.panelUnsentMessages.ResumeLayout(false);
            this.panelUnsentMessages.PerformLayout();
            this.panelTemplates.ResumeLayout(false);
            this.panelTemplates.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Panel panelUnsentMessages;
        private System.Windows.Forms.Label labelAccountName;
        private System.Windows.Forms.Label labelAccountEmail;
        private System.Windows.Forms.RadioButton radioUnsentDelete;
        private System.Windows.Forms.ComboBox accountsForUnsent;
        private System.Windows.Forms.RadioButton radioUnsentChange;
        private System.Windows.Forms.Label labelUnsentMessagesInfo;
        private System.Windows.Forms.Label labelUnsentMessages;
        private System.Windows.Forms.Panel panelTemplates;
        private System.Windows.Forms.Label labelTemplates;
        private System.Windows.Forms.RadioButton radioTemplatesDelete;
        private System.Windows.Forms.ComboBox accountsForTemplates;
        private System.Windows.Forms.RadioButton radioTemplatesChange;
        private System.Windows.Forms.Label labelTemplatesInfo;
    }
}