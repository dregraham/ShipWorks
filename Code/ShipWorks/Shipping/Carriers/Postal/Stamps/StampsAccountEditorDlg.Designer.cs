namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    partial class StampsAccountEditorDlg
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
            this.changePassword = new System.Windows.Forms.Button();
            this.labelMailingPostalCode = new System.Windows.Forms.Label();
            this.labelMailingPostOffice = new System.Windows.Forms.Label();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelStamps = new System.Windows.Forms.Label();
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.accountPanel = new System.Windows.Forms.Panel();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.mailingPostalCode = new ShipWorks.UI.Controls.PromptTextBox();
            this.accountInfoControl = new ShipWorks.Shipping.Carriers.Postal.Stamps.UspsAccountInfoControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.accountPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // changePassword
            // 
            this.changePassword.Location = new System.Drawing.Point(85, 3);
            this.changePassword.Name = "changePassword";
            this.changePassword.Size = new System.Drawing.Size(136, 23);
            this.changePassword.TabIndex = 46;
            this.changePassword.Text = "Update Password...";
            this.changePassword.UseVisualStyleBackColor = true;
            this.changePassword.Click += new System.EventHandler(this.OnUpdatePassword);
            // 
            // labelMailingPostalCode
            // 
            this.labelMailingPostalCode.AutoSize = true;
            this.labelMailingPostalCode.Location = new System.Drawing.Point(13, 391);
            this.labelMailingPostalCode.Name = "labelMailingPostalCode";
            this.labelMailingPostalCode.Size = new System.Drawing.Size(68, 13);
            this.labelMailingPostalCode.TabIndex = 44;
            this.labelMailingPostalCode.Text = "Postal Code:";
            // 
            // labelMailingPostOffice
            // 
            this.labelMailingPostOffice.AutoSize = true;
            this.labelMailingPostOffice.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMailingPostOffice.Location = new System.Drawing.Point(10, 368);
            this.labelMailingPostOffice.Name = "labelMailingPostOffice";
            this.labelMailingPostOffice.Size = new System.Drawing.Size(110, 13);
            this.labelMailingPostOffice.TabIndex = 43;
            this.labelMailingPostOffice.Text = "Mailing Post Office";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(10, 8);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(57, 13);
            this.labelPassword.TabIndex = 40;
            this.labelPassword.Text = "Password:";
            // 
            // labelNote
            // 
            this.labelNote.ForeColor = System.Drawing.Color.DimGray;
            this.labelNote.Location = new System.Drawing.Point(38, 420);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(325, 27);
            this.labelNote.TabIndex = 26;
            this.labelNote.Text = "Note: Any changes made to the address are only for ShipWorks.  Your address infor" +
    "mation with Stamps.com is not updated.";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.exclamation16;
            this.pictureBox.Location = new System.Drawing.Point(16, 420);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 36;
            this.pictureBox.TabStop = false;
            // 
            // labelStamps
            // 
            this.labelStamps.AutoSize = true;
            this.labelStamps.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStamps.Location = new System.Drawing.Point(12, 9);
            this.labelStamps.Name = "labelStamps";
            this.labelStamps.Size = new System.Drawing.Size(50, 13);
            this.labelStamps.TabIndex = 27;
            this.labelStamps.Text = "Stamps";
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(290, 465);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 38;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(209, 465);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 37;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // accountPanel
            // 
            this.accountPanel.Controls.Add(this.changePassword);
            this.accountPanel.Controls.Add(this.personControl);
            this.accountPanel.Controls.Add(this.pictureBox);
            this.accountPanel.Controls.Add(this.cancel);
            this.accountPanel.Controls.Add(this.ok);
            this.accountPanel.Controls.Add(this.labelMailingPostalCode);
            this.accountPanel.Controls.Add(this.labelNote);
            this.accountPanel.Controls.Add(this.mailingPostalCode);
            this.accountPanel.Controls.Add(this.labelPassword);
            this.accountPanel.Controls.Add(this.labelMailingPostOffice);
            this.accountPanel.Location = new System.Drawing.Point(3, 169);
            this.accountPanel.Name = "accountPanel";
            this.accountPanel.Size = new System.Drawing.Size(368, 491);
            this.accountPanel.TabIndex = 48;
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.FullName = "";
            this.personControl.Location = new System.Drawing.Point(8, 37);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 328);
            this.personControl.TabIndex = 35;
            // 
            // mailingPostalCode
            // 
            this.mailingPostalCode.Location = new System.Drawing.Point(85, 388);
            this.fieldLengthProvider.SetMaxLengthSource(this.mailingPostalCode, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaAccountDescription);
            this.mailingPostalCode.Name = "mailingPostalCode";
            this.mailingPostalCode.PromptColor = System.Drawing.SystemColors.GrayText;
            this.mailingPostalCode.PromptText = "(Same as Postal Code)";
            this.mailingPostalCode.Size = new System.Drawing.Size(271, 21);
            this.mailingPostalCode.TabIndex = 45;
            // 
            // accountInfoControl
            // 
            this.accountInfoControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountInfoControl.Location = new System.Drawing.Point(23, 25);
            this.accountInfoControl.Name = "accountInfoControl";
            this.accountInfoControl.Size = new System.Drawing.Size(348, 144);
            this.accountInfoControl.TabIndex = 47;
            // 
            // StampsAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(385, 672);
            this.Controls.Add(this.accountPanel);
            this.Controls.Add(this.accountInfoControl);
            this.Controls.Add(this.labelStamps);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "StampsAccountEditorDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Stamps Account";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.accountPanel.ResumeLayout(false);
            this.accountPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button changePassword;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.PromptTextBox mailingPostalCode;
        private System.Windows.Forms.Label labelMailingPostalCode;
        private System.Windows.Forms.Label labelMailingPostOffice;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelStamps;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private Data.Controls.PersonControl personControl;
        private UspsAccountInfoControl accountInfoControl;
        private System.Windows.Forms.Panel accountPanel;
    }
}