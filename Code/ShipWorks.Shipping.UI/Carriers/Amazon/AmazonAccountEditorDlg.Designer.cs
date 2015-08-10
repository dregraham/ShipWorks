using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    partial class AmazonAccountEditorDlg
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
            this.labelAuthToken = new System.Windows.Forms.Label();
            this.labelMerchantId = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelAmazonAccount = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelOptional = new System.Windows.Forms.Label();
            this.description = new ShipWorks.UI.Controls.PromptTextBox();
            this.contactInformation = new ShipWorks.Data.Controls.PersonControl();
            this.merchantId = new System.Windows.Forms.TextBox();
            this.authToken = new System.Windows.Forms.TextBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelAuthToken
            // 
            this.labelAuthToken.AutoSize = true;
            this.labelAuthToken.Location = new System.Drawing.Point(22, 58);
            this.labelAuthToken.Name = "labelAuthToken";
            this.labelAuthToken.Size = new System.Drawing.Size(66, 13);
            this.labelAuthToken.TabIndex = 172;
            this.labelAuthToken.Text = "Auth Token:";
            // 
            // labelMerchantId
            // 
            this.labelMerchantId.AutoSize = true;
            this.labelMerchantId.Location = new System.Drawing.Point(19, 31);
            this.labelMerchantId.Name = "labelMerchantId";
            this.labelMerchantId.Size = new System.Drawing.Size(69, 13);
            this.labelMerchantId.TabIndex = 171;
            this.labelMerchantId.Text = "Merchant Id:";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(201, 414);
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
            this.cancel.Location = new System.Drawing.Point(282, 414);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 176;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelAmazonAccount
            // 
            this.labelAmazonAccount.AutoSize = true;
            this.labelAmazonAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAmazonAccount.Location = new System.Drawing.Point(12, 9);
            this.labelAmazonAccount.Name = "labelAmazonAccount";
            this.labelAmazonAccount.Size = new System.Drawing.Size(102, 13);
            this.labelAmazonAccount.TabIndex = 178;
            this.labelAmazonAccount.Text = "Amazon Account";
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
            this.contactInformation.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Website)));
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
            this.contactInformation.Size = new System.Drawing.Size(355, 312);
            this.contactInformation.TabIndex = 0;
            // 
            // merchantId
            // 
            this.merchantId.BackColor = System.Drawing.SystemColors.Control;
            this.merchantId.Enabled = false;
            this.merchantId.Location = new System.Drawing.Point(92, 28);
            this.merchantId.Name = "merchantId";
            this.merchantId.Size = new System.Drawing.Size(265, 21);
            this.merchantId.TabIndex = 169;
            // 
            // authToken
            // 
            this.authToken.Location = new System.Drawing.Point(92, 55);
            this.authToken.MaxLength = 50;
            this.authToken.Name = "authToken";
            this.authToken.Size = new System.Drawing.Size(265, 21);
            this.authToken.TabIndex = 170;
            // 
            // AmazonAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(369, 449);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.description);
            this.Controls.Add(this.labelOptional);
            this.Controls.Add(this.labelAmazonAccount);
            this.Controls.Add(this.contactInformation);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.merchantId);
            this.Controls.Add(this.authToken);
            this.Controls.Add(this.labelAuthToken);
            this.Controls.Add(this.labelMerchantId);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AmazonAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Amazon Account";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox authToken;
        private System.Windows.Forms.Label labelAuthToken;
        private System.Windows.Forms.Label labelMerchantId;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private Data.Controls.PersonControl contactInformation;
        private System.Windows.Forms.Label labelAmazonAccount;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.TextBox merchantId;
        private System.Windows.Forms.Label labelDescription;
        private PromptTextBox description;
        private System.Windows.Forms.Label labelOptional;
    }
}
