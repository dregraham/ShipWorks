namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaAccountEditorDlg
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
            this.labelEndicia = new System.Windows.Forms.Label();
            this.labelAccount = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelBalance = new System.Windows.Forms.Label();
            this.balance = new System.Windows.Forms.Label();
            this.buyPostage = new System.Windows.Forms.Button();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelOptional = new System.Windows.Forms.Label();
            this.description = new ShipWorks.UI.Controls.PromptTextBox();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.mailingPostalCode = new ShipWorks.UI.Controls.PromptTextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.infotipPassword = new ShipWorks.UI.Controls.InfoTip();
            this.labelMailingPostalCode = new System.Windows.Forms.Label();
            this.labelMailingPostOffice = new System.Windows.Forms.Label();
            this.changePassphrase = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageAccount = new System.Windows.Forms.TabPage();
            this.tabPageAddress = new System.Windows.Forms.TabPage();
            this.tabPageOptions = new System.Windows.Forms.TabPage();
            this.labelScanForm = new System.Windows.Forms.Label();
            this.labelScanAddress = new System.Windows.Forms.Label();
            this.comboScanAddress = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageAccount.SuspendLayout();
            this.tabPageAddress.SuspendLayout();
            this.tabPageOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(265, 508);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 10;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(346, 508);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 11;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelEndicia
            // 
            this.labelEndicia.AutoSize = true;
            this.labelEndicia.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEndicia.Location = new System.Drawing.Point(13, 14);
            this.labelEndicia.Name = "labelEndicia";
            this.labelEndicia.Size = new System.Drawing.Size(46, 13);
            this.labelEndicia.TabIndex = 1;
            this.labelEndicia.Text = "Endicia";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(34, 35);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 2;
            this.labelAccount.Text = "Account:";
            // 
            // labelNote
            // 
            this.labelNote.ForeColor = System.Drawing.Color.DimGray;
            this.labelNote.Location = new System.Drawing.Point(34, 411);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(325, 27);
            this.labelNote.TabIndex = 0;
            this.labelNote.Text = "Note: Any changes made to the address are only for ShipWorks.  Your address infor" +
    "mation with Endicia is not updated.";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBox.Location = new System.Drawing.Point(14, 413);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // labelBalance
            // 
            this.labelBalance.AutoSize = true;
            this.labelBalance.Location = new System.Drawing.Point(36, 90);
            this.labelBalance.Name = "labelBalance";
            this.labelBalance.Size = new System.Drawing.Size(48, 13);
            this.labelBalance.TabIndex = 6;
            this.labelBalance.Text = "Balance:";
            // 
            // balance
            // 
            this.balance.AutoSize = true;
            this.balance.Location = new System.Drawing.Point(89, 90);
            this.balance.Name = "balance";
            this.balance.Size = new System.Drawing.Size(62, 13);
            this.balance.TabIndex = 7;
            this.balance.Text = "Checking...";
            // 
            // buyPostage
            // 
            this.buyPostage.Location = new System.Drawing.Point(157, 85);
            this.buyPostage.Name = "buyPostage";
            this.buyPostage.Size = new System.Drawing.Size(88, 23);
            this.buyPostage.TabIndex = 8;
            this.buyPostage.Text = "Buy Postage...";
            this.buyPostage.UseVisualStyleBackColor = true;
            this.buyPostage.Click += new System.EventHandler(this.OnBuyPostage);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(20, 63);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description:";
            // 
            // labelOptional
            // 
            this.labelOptional.AutoSize = true;
            this.labelOptional.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelOptional.Location = new System.Drawing.Point(302, 63);
            this.labelOptional.Name = "labelOptional";
            this.labelOptional.Size = new System.Drawing.Size(53, 13);
            this.labelOptional.TabIndex = 15;
            this.labelOptional.Text = "(optional)";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(88, 60);
            this.fieldLengthProvider.SetMaxLengthSource(this.description, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaAccountDescription);
            this.description.Name = "description";
            this.description.PromptColor = System.Drawing.SystemColors.GrayText;
            this.description.PromptText = null;
            this.description.Size = new System.Drawing.Size(211, 21);
            this.description.TabIndex = 5;
            // 
            // accountNumber
            // 
            this.accountNumber.Location = new System.Drawing.Point(88, 32);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.ReadOnly = true;
            this.accountNumber.Size = new System.Drawing.Size(136, 21);
            this.accountNumber.TabIndex = 3;
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Fax)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.Location = new System.Drawing.Point(12, 8);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 328);
            this.personControl.TabIndex = 9;
            this.personControl.ContentChanged += new System.EventHandler(this.OnPersonContentChanged);
            // 
            // mailingPostalCode
            // 
            this.mailingPostalCode.Location = new System.Drawing.Point(112, 365);
            this.fieldLengthProvider.SetMaxLengthSource(this.mailingPostalCode, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaAccountDescription);
            this.mailingPostalCode.Name = "mailingPostalCode";
            this.mailingPostalCode.PromptColor = System.Drawing.SystemColors.GrayText;
            this.mailingPostalCode.PromptText = "(Same as Postal Code)";
            this.mailingPostalCode.Size = new System.Drawing.Size(243, 21);
            this.mailingPostalCode.TabIndex = 24;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(88, 114);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(211, 21);
            this.password.TabIndex = 19;
            this.password.UseSystemPasswordChar = true;
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(21, 117);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(66, 13);
            this.labelPassword.TabIndex = 18;
            this.labelPassword.Text = "Passphrase:";
            // 
            // infotipPassword
            // 
            this.infotipPassword.Caption = "This is not the passphrase you use to log on to your account at www.endicia.com. " +
    " This is the \r\n\"Software Passphrase\" that ShipWorks uses to access your account " +
    "on your behalf.";
            this.infotipPassword.Location = new System.Drawing.Point(305, 120);
            this.infotipPassword.Name = "infotipPassword";
            this.infotipPassword.Size = new System.Drawing.Size(12, 12);
            this.infotipPassword.TabIndex = 21;
            this.infotipPassword.Title = "Password";
            // 
            // labelMailingPostalCode
            // 
            this.labelMailingPostalCode.AutoSize = true;
            this.labelMailingPostalCode.Location = new System.Drawing.Point(34, 368);
            this.labelMailingPostalCode.Name = "labelMailingPostalCode";
            this.labelMailingPostalCode.Size = new System.Drawing.Size(68, 13);
            this.labelMailingPostalCode.TabIndex = 23;
            this.labelMailingPostalCode.Text = "Postal Code:";
            // 
            // labelMailingPostOffice
            // 
            this.labelMailingPostOffice.AutoSize = true;
            this.labelMailingPostOffice.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMailingPostOffice.Location = new System.Drawing.Point(9, 345);
            this.labelMailingPostOffice.Name = "labelMailingPostOffice";
            this.labelMailingPostOffice.Size = new System.Drawing.Size(110, 13);
            this.labelMailingPostOffice.TabIndex = 22;
            this.labelMailingPostOffice.Text = "Mailing Post Office";
            // 
            // changePassphrase
            // 
            this.changePassphrase.Location = new System.Drawing.Point(88, 141);
            this.changePassphrase.Name = "changePassphrase";
            this.changePassphrase.Size = new System.Drawing.Size(136, 23);
            this.changePassphrase.TabIndex = 25;
            this.changePassphrase.Text = "Change Passphrase...";
            this.changePassphrase.UseVisualStyleBackColor = true;
            this.changePassphrase.Click += new System.EventHandler(this.OnChangePassphrase);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageAccount);
            this.tabControl.Controls.Add(this.tabPageAddress);
            this.tabControl.Controls.Add(this.tabPageOptions);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(410, 486);
            this.tabControl.TabIndex = 26;
            // 
            // tabPageAccount
            // 
            this.tabPageAccount.Controls.Add(this.labelEndicia);
            this.tabPageAccount.Controls.Add(this.changePassphrase);
            this.tabPageAccount.Controls.Add(this.accountNumber);
            this.tabPageAccount.Controls.Add(this.labelAccount);
            this.tabPageAccount.Controls.Add(this.labelBalance);
            this.tabPageAccount.Controls.Add(this.balance);
            this.tabPageAccount.Controls.Add(this.infotipPassword);
            this.tabPageAccount.Controls.Add(this.buyPostage);
            this.tabPageAccount.Controls.Add(this.password);
            this.tabPageAccount.Controls.Add(this.labelOptional);
            this.tabPageAccount.Controls.Add(this.labelPassword);
            this.tabPageAccount.Controls.Add(this.description);
            this.tabPageAccount.Controls.Add(this.labelDescription);
            this.tabPageAccount.Location = new System.Drawing.Point(4, 22);
            this.tabPageAccount.Name = "tabPageAccount";
            this.tabPageAccount.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAccount.Size = new System.Drawing.Size(402, 460);
            this.tabPageAccount.TabIndex = 0;
            this.tabPageAccount.Text = "Account";
            this.tabPageAccount.UseVisualStyleBackColor = true;
            // 
            // tabPageAddress
            // 
            this.tabPageAddress.Controls.Add(this.personControl);
            this.tabPageAddress.Controls.Add(this.labelMailingPostalCode);
            this.tabPageAddress.Controls.Add(this.pictureBox);
            this.tabPageAddress.Controls.Add(this.mailingPostalCode);
            this.tabPageAddress.Controls.Add(this.labelNote);
            this.tabPageAddress.Controls.Add(this.labelMailingPostOffice);
            this.tabPageAddress.Location = new System.Drawing.Point(4, 22);
            this.tabPageAddress.Name = "tabPageAddress";
            this.tabPageAddress.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAddress.Size = new System.Drawing.Size(402, 460);
            this.tabPageAddress.TabIndex = 1;
            this.tabPageAddress.Text = "Address";
            this.tabPageAddress.UseVisualStyleBackColor = true;
            // 
            // tabPageOptions
            // 
            this.tabPageOptions.Controls.Add(this.label1);
            this.tabPageOptions.Controls.Add(this.comboScanAddress);
            this.tabPageOptions.Controls.Add(this.labelScanAddress);
            this.tabPageOptions.Controls.Add(this.labelScanForm);
            this.tabPageOptions.Location = new System.Drawing.Point(4, 22);
            this.tabPageOptions.Name = "tabPageOptions";
            this.tabPageOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOptions.Size = new System.Drawing.Size(402, 460);
            this.tabPageOptions.TabIndex = 2;
            this.tabPageOptions.Text = "Options";
            this.tabPageOptions.UseVisualStyleBackColor = true;
            // 
            // labelScanForm
            // 
            this.labelScanForm.AutoSize = true;
            this.labelScanForm.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScanForm.Location = new System.Drawing.Point(13, 14);
            this.labelScanForm.Name = "labelScanForm";
            this.labelScanForm.Size = new System.Drawing.Size(74, 13);
            this.labelScanForm.TabIndex = 2;
            this.labelScanForm.Text = "SCAN Forms";
            // 
            // labelScanAddress
            // 
            this.labelScanAddress.AutoSize = true;
            this.labelScanAddress.Location = new System.Drawing.Point(28, 36);
            this.labelScanAddress.Name = "labelScanAddress";
            this.labelScanAddress.Size = new System.Drawing.Size(86, 13);
            this.labelScanAddress.TabIndex = 3;
            this.labelScanAddress.Text = "Return Address:";
            // 
            // comboScanAddress
            // 
            this.comboScanAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboScanAddress.FormattingEnabled = true;
            this.comboScanAddress.Location = new System.Drawing.Point(120, 33);
            this.comboScanAddress.Name = "comboScanAddress";
            this.comboScanAddress.Size = new System.Drawing.Size(248, 21);
            this.comboScanAddress.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(119, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 50);
            this.label1.TabIndex = 5;
            this.label1.Text = "If you use the account address entered in ShipWorks, then origin postal code of a" +
    "ll shipments on the SCAN form must the account address.";
            // 
            // EndiciaAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(433, 543);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EndiciaAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Endicia Account";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Shown += new System.EventHandler(this.OnShown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageAccount.ResumeLayout(false);
            this.tabPageAccount.PerformLayout();
            this.tabPageAddress.ResumeLayout(false);
            this.tabPageAddress.PerformLayout();
            this.tabPageOptions.ResumeLayout(false);
            this.tabPageOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelEndicia;
        private System.Windows.Forms.TextBox accountNumber;
        private ShipWorks.Data.Controls.PersonControl personControl;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelBalance;
        private System.Windows.Forms.Label balance;
        private System.Windows.Forms.Button buyPostage;
        private System.Windows.Forms.Label labelDescription;
        private ShipWorks.UI.Controls.PromptTextBox description;
        private System.Windows.Forms.Label labelOptional;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private UI.Controls.InfoTip infotipPassword;
        private System.Windows.Forms.Label labelMailingPostalCode;
        private UI.Controls.PromptTextBox mailingPostalCode;
        private System.Windows.Forms.Label labelMailingPostOffice;
        private System.Windows.Forms.Button changePassphrase;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAccount;
        private System.Windows.Forms.TabPage tabPageAddress;
        private System.Windows.Forms.TabPage tabPageOptions;
        private System.Windows.Forms.Label labelScanForm;
        private System.Windows.Forms.ComboBox comboScanAddress;
        private System.Windows.Forms.Label labelScanAddress;
        private System.Windows.Forms.Label label1;
    }
}
