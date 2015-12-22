using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    partial class OnTracAccountEditorDlg
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
            this.components = new System.ComponentModel.Container();
            this.labelPassword = new System.Windows.Forms.Label();
            this.labelAccountNumber = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelOnTracAccount = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelOptional = new System.Windows.Forms.Label();
            this.description = new ShipWorks.UI.Controls.PromptTextBox();
            this.contactInformation = new ShipWorks.Data.Controls.PersonControl();
            this.accountNumber = new ShipWorks.UI.Controls.NumericTextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(9, 58);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(77, 13);
            this.labelPassword.TabIndex = 172;
            this.labelPassword.Text = "API Password:";
            // 
            // labelAccountNumber
            // 
            this.labelAccountNumber.AutoSize = true;
            this.labelAccountNumber.Location = new System.Drawing.Point(25, 31);
            this.labelAccountNumber.Name = "labelAccountNumber";
            this.labelAccountNumber.Size = new System.Drawing.Size(61, 13);
            this.labelAccountNumber.TabIndex = 171;
            this.labelAccountNumber.Text = "Account #:";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(201, 389);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 175;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(282, 389);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 176;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelOnTracAccount
            // 
            this.labelOnTracAccount.AutoSize = true;
            this.labelOnTracAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOnTracAccount.Location = new System.Drawing.Point(12, 9);
            this.labelOnTracAccount.Name = "labelOnTracAccount";
            this.labelOnTracAccount.Size = new System.Drawing.Size(96, 13);
            this.labelOnTracAccount.TabIndex = 178;
            this.labelOnTracAccount.Text = "OnTrac Account";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(24, 85);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 179;
            this.labelDescription.Text = "Description:";
            // 
            // labelOptional
            // 
            this.labelOptional.AutoSize = true;
            this.labelOptional.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelOptional.Location = new System.Drawing.Point(260, 85);
            this.labelOptional.Name = "labelOptional";
            this.labelOptional.Size = new System.Drawing.Size(53, 13);
            this.labelOptional.TabIndex = 181;
            this.labelOptional.Text = "(optional)";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(92, 82);
            this.fieldLengthProvider.SetMaxLengthSource(this.description, ShipWorks.Data.Utility.EntityFieldLengthSource.EndiciaAccountDescription);
            this.description.Name = "description";
            this.description.PromptColor = System.Drawing.SystemColors.GrayText;
            this.description.PromptText = null;
            this.description.Size = new System.Drawing.Size(162, 21);
            this.description.TabIndex = 180;
            // 
            // contactInformation
            // 
            this.contactInformation.AddressSelector = null;
            this.contactInformation.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.contactInformation.EnableValidationControls = false;
            this.contactInformation.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contactInformation.FullName = "";
            this.contactInformation.Location = new System.Drawing.Point(15, 101);
            this.contactInformation.MaxStreetLines = 1;
            this.contactInformation.Name = "contactInformation";
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal)));
            this.contactInformation.Size = new System.Drawing.Size(355, 279);
            this.contactInformation.TabIndex = 0;
            this.contactInformation.ContentChanged += new System.EventHandler(this.OnPersonContentChanged);
            // 
            // accountNumber
            // 
            this.accountNumber.BackColor = System.Drawing.SystemColors.Control;
            this.accountNumber.Enabled = false;
            this.accountNumber.Location = new System.Drawing.Point(92, 28);
            this.accountNumber.Name = "accountNumber";
            this.accountNumber.Size = new System.Drawing.Size(162, 21);
            this.accountNumber.TabIndex = 169;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(92, 55);
            this.password.MaxLength = 50;
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(162, 21);
            this.password.TabIndex = 170;
            this.password.UseSystemPasswordChar = true;
            // 
            // OnTracAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(369, 424);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.description);
            this.Controls.Add(this.labelOptional);
            this.Controls.Add(this.labelOnTracAccount);
            this.Controls.Add(this.contactInformation);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.accountNumber);
            this.Controls.Add(this.password);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.labelAccountNumber);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OnTracAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "OnTrac Account";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.Label labelAccountNumber;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private Data.Controls.PersonControl contactInformation;
        private System.Windows.Forms.Label labelOnTracAccount;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private NumericTextBox accountNumber;
        private System.Windows.Forms.Label labelDescription;
        private PromptTextBox description;
        private System.Windows.Forms.Label labelOptional;
    }
}
