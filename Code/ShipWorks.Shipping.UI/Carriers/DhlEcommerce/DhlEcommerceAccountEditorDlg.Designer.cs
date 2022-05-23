using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    partial class DhlEcommerceAccountEditorDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DhlEcommerceAccountEditorDlg));
            this.labelClientId = new System.Windows.Forms.Label();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.labelDhlEcommerceAccount = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.labelNote = new System.Windows.Forms.Label();
            this.description = new System.Windows.Forms.TextBox();
            this.contactInformation = new ShipWorks.Data.Controls.PersonControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.clientId = new System.Windows.Forms.TextBox();
            this.pickupNumber = new System.Windows.Forms.TextBox();
            this.labelPickupNumber = new System.Windows.Forms.Label();
            this.labelDistributionCenter = new System.Windows.Forms.Label();
            this.distributionCenter = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // labelClientId
            // 
            this.labelClientId.AutoSize = true;
            this.labelClientId.Location = new System.Drawing.Point(58, 33);
            this.labelClientId.Name = "labelClientId";
            this.labelClientId.Size = new System.Drawing.Size(52, 13);
            this.labelClientId.TabIndex = 0;
            this.labelClientId.Text = "Client ID:";
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(204, 456);
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
            this.cancel.Location = new System.Drawing.Point(285, 456);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 176;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // labelDhlEcommerceAccount
            // 
            this.labelDhlEcommerceAccount.AutoSize = true;
            this.labelDhlEcommerceAccount.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDhlEcommerceAccount.Location = new System.Drawing.Point(9, 9);
            this.labelDhlEcommerceAccount.Name = "labelDhlEcommerceAccount";
            this.labelDhlEcommerceAccount.Size = new System.Drawing.Size(149, 13);
            this.labelDhlEcommerceAccount.TabIndex = 178;
            this.labelDhlEcommerceAccount.Text = "DHL eCommerce Account";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(46, 114);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 179;
            this.labelDescription.Text = "Description:";
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(20, 415);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 183;
            this.pictureBox.TabStop = false;
            // 
            // labelNote
            // 
            this.labelNote.ForeColor = System.Drawing.Color.DimGray;
            this.labelNote.Location = new System.Drawing.Point(42, 417);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(325, 27);
            this.labelNote.TabIndex = 182;
            this.labelNote.Text = "Note: Any changes made to the address are only for ShipWorks.  Your address infor" +
    "mation with DHL eCommerce is not updated.";
            // 
            // description
            // 
            this.description.Location = new System.Drawing.Point(116, 111);
            this.fieldLengthProvider.SetMaxLengthSource(this.description, ShipWorks.Data.Utility.EntityFieldLengthSource.DhlEcommerceDescription);
            this.description.Name = "description";
            this.description.Size = new System.Drawing.Size(238, 21);
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
            this.contactInformation.Location = new System.Drawing.Point(12, 130);
            this.contactInformation.MaxStreetLines = 1;
            this.contactInformation.Name = "contactInformation";
            this.contactInformation.RequiredFields = ((ShipWorks.Data.Controls.PersonFields)((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal)));
            this.contactInformation.Size = new System.Drawing.Size(355, 279);
            this.contactInformation.TabIndex = 0;
            this.contactInformation.ValidatedAddressScope = null;
            this.contactInformation.ContentChanged += new System.EventHandler(this.OnPersonContentChanged);
            // 
            // clientId
            // 
            this.clientId.Location = new System.Drawing.Point(116, 30);
            this.fieldLengthProvider.SetMaxLengthSource(this.clientId, ShipWorks.Data.Utility.EntityFieldLengthSource.DhlEcommerceDescription);
            this.clientId.Name = "clientId";
            this.clientId.ReadOnly = true;
            this.clientId.Size = new System.Drawing.Size(238, 21);
            this.clientId.TabIndex = 1;
            // 
            // pickupNumber
            // 
            this.pickupNumber.Location = new System.Drawing.Point(116, 57);
            this.fieldLengthProvider.SetMaxLengthSource(this.pickupNumber, ShipWorks.Data.Utility.EntityFieldLengthSource.DhlEcommerceDescription);
            this.pickupNumber.Name = "pickupNumber";
            this.pickupNumber.ReadOnly = true;
            this.pickupNumber.Size = new System.Drawing.Size(238, 21);
            this.pickupNumber.TabIndex = 3;
            // 
            // labelPickupNumber
            // 
            this.labelPickupNumber.AutoSize = true;
            this.labelPickupNumber.Location = new System.Drawing.Point(29, 60);
            this.labelPickupNumber.Name = "labelPickupNumber";
            this.labelPickupNumber.Size = new System.Drawing.Size(81, 13);
            this.labelPickupNumber.TabIndex = 2;
            this.labelPickupNumber.Text = "Pickup Number:";
            // 
            // labelDistributionCenter
            // 
            this.labelDistributionCenter.AutoSize = true;
            this.labelDistributionCenter.Location = new System.Drawing.Point(9, 87);
            this.labelDistributionCenter.Name = "labelDistributionCenter";
            this.labelDistributionCenter.Size = new System.Drawing.Size(101, 13);
            this.labelDistributionCenter.TabIndex = 4;
            this.labelDistributionCenter.Text = "Distribution Center:";
            // 
            // distributionCenter
            // 
            this.distributionCenter.Location = new System.Drawing.Point(116, 84);
            this.fieldLengthProvider.SetMaxLengthSource(this.distributionCenter, ShipWorks.Data.Utility.EntityFieldLengthSource.DhlEcommerceDescription);
            this.distributionCenter.Name = "distributionCenter";
            this.distributionCenter.ReadOnly = true;
            this.distributionCenter.Size = new System.Drawing.Size(238, 21);
            this.distributionCenter.TabIndex = 5;
            // 
            // DhlEcommerceAccountEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(372, 491);
            this.Controls.Add(this.distributionCenter);
            this.Controls.Add(this.labelDistributionCenter);
            this.Controls.Add(this.labelPickupNumber);
            this.Controls.Add(this.pickupNumber);
            this.Controls.Add(this.clientId);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.description);
            this.Controls.Add(this.labelDhlEcommerceAccount);
            this.Controls.Add(this.contactInformation);
            this.Controls.Add(this.ok);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.labelClientId);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DhlEcommerceAccountEditorDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DHL eCommerce Account";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelClientId;
        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private Data.Controls.PersonControl contactInformation;
        private System.Windows.Forms.Label labelDhlEcommerceAccount;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox description;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.TextBox clientId;
        private System.Windows.Forms.TextBox pickupNumber;
        private System.Windows.Forms.Label labelPickupNumber;
        private System.Windows.Forms.Label labelDistributionCenter;
        private System.Windows.Forms.TextBox distributionCenter;
    }
}
