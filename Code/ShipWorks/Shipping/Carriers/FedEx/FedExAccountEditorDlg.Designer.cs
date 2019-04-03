namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExAccountEditorDlg
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
            this.cancel = new System.Windows.Forms.Button();
            this.ok = new System.Windows.Forms.Button();
            this.labelAccount = new System.Windows.Forms.Label();
            this.accountNumber = new System.Windows.Forms.TextBox();
            this.labelFedExAccount = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelOptional = new System.Windows.Forms.Label();
            this.description = new ShipWorks.UI.Controls.PromptTextBox();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageAccount = new System.Windows.Forms.TabPage();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.settingsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExAccountSettingsControl();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.tabControl.SuspendLayout();
            this.tabPageAccount.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(339, 499);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 10;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(258, 499);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 9;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(30, 32);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 1;
            this.labelAccount.Text = "Account:";
            // 
            // accountNumber
            // 
            this.accountNumber.Location = new System.Drawing.Point(87, 29);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.ReadOnly = true;
            this.accountNumber.Size = new System.Drawing.Size(165, 21);
            this.accountNumber.TabIndex = 2;
            // 
            // labelFedExAccount
            // 
            this.labelFedExAccount.AutoSize = true;
            this.labelFedExAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFedExAccount.Location = new System.Drawing.Point(11, 11);
            this.labelFedExAccount.Name = "labelFedExAccount";
            this.labelFedExAccount.Size = new System.Drawing.Size(89, 13);
            this.labelFedExAccount.TabIndex = 0;
            this.labelFedExAccount.Text = "FedEx Account";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(16, 59);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 3;
            this.labelDescription.Text = "Description:";
            // 
            // labelOptional
            // 
            this.labelOptional.AutoSize = true;
            this.labelOptional.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelOptional.Location = new System.Drawing.Point(304, 59);
            this.labelOptional.Name = "labelOptional";
            this.labelOptional.Size = new System.Drawing.Size(53, 13);
            this.labelOptional.TabIndex = 12;
            this.labelOptional.Text = "(optional)";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(87, 56);
            this.fieldLengthProvider.SetMaxLengthSource(this.description, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExAccountDescription);
            this.description.Name = "description";
            this.description.PromptColor = System.Drawing.SystemColors.GrayText;
            this.description.PromptText = null;
            this.description.Size = new System.Drawing.Size(211, 21);
            this.description.TabIndex = 4;
            // 
            // personControl
            // 
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Website)));
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.Location = new System.Drawing.Point(8, 84);
            this.personControl.MaxStreetLines = 2;
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(358, 362);
            this.personControl.TabIndex = 8;
            this.personControl.ContentChanged += new System.EventHandler(this.OnPersonContentChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageAccount);
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(402, 477);
            this.tabControl.TabIndex = 13;
            // 
            // tabPageAccount
            // 
            this.tabPageAccount.Controls.Add(this.labelFedExAccount);
            this.tabPageAccount.Controls.Add(this.labelDescription);
            this.tabPageAccount.Controls.Add(this.accountNumber);
            this.tabPageAccount.Controls.Add(this.description);
            this.tabPageAccount.Controls.Add(this.personControl);
            this.tabPageAccount.Controls.Add(this.labelAccount);
            this.tabPageAccount.Controls.Add(this.labelOptional);
            this.tabPageAccount.Location = new System.Drawing.Point(4, 22);
            this.tabPageAccount.Name = "tabPageAccount";
            this.tabPageAccount.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAccount.Size = new System.Drawing.Size(394, 451);
            this.tabPageAccount.TabIndex = 0;
            this.tabPageAccount.Text = "Account";
            this.tabPageAccount.UseVisualStyleBackColor = true;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.settingsControl);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(394, 451);
            this.tabPageSettings.TabIndex = 1;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // settingsControl
            // 
            this.settingsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsControl.Location = new System.Drawing.Point(10, 9);
            this.settingsControl.Name = "settingsControl";
            this.settingsControl.Size = new System.Drawing.Size(331, 436);
            this.settingsControl.TabIndex = 8;
            // 
            // FedExAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(426, 534);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FedExAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FedEx Account";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.tabControl.ResumeLayout(false);
            this.tabPageAccount.ResumeLayout(false);
            this.tabPageAccount.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Label labelAccount;
        private ShipWorks.Data.Controls.PersonControl personControl;
        private System.Windows.Forms.TextBox accountNumber;
        private System.Windows.Forms.Label labelFedExAccount;
        private System.Windows.Forms.Label labelDescription;
        private ShipWorks.UI.Controls.PromptTextBox description;
        private System.Windows.Forms.Label labelOptional;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAccount;
        private System.Windows.Forms.TabPage tabPageSettings;
        private FedExAccountSettingsControl settingsControl;
    }
}
