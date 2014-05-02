namespace ShipWorks.Shipping.Editing
{
    partial class ServiceControlBase
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
            ShipWorks.AddressValidation.EntityGridAddressSelector entityGridAddressSelector1 = new ShipWorks.AddressValidation.EntityGridAddressSelector();
            this.sectionReturns = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.returnsPanel = new System.Windows.Forms.Panel();
            this.returnShipment = new System.Windows.Forms.CheckBox();
            this.sectionRecipient = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.residentialDetermination = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelAddress = new System.Windows.Forms.Label();
            this.labelResidentialCommercial = new System.Windows.Forms.Label();
            this.personControl = new ShipWorks.Data.Controls.PersonControl();
            this.sectionShipment = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).BeginInit();
            this.sectionReturns.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // sectionReturns
            // 
            this.sectionReturns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionReturns.Collapsed = true;
            // 
            // sectionReturns.ContentPanel
            // 
            this.sectionReturns.ContentPanel.Controls.Add(this.returnsPanel);
            this.sectionReturns.ContentPanel.Controls.Add(this.returnShipment);
            this.sectionReturns.ExpandedHeight = 205;
            this.sectionReturns.ExtraText = "";
            this.sectionReturns.Location = new System.Drawing.Point(3, 114);
            this.sectionReturns.Name = "sectionReturns";
            this.sectionReturns.SectionName = "Return Shipment";
            this.sectionReturns.SettingsKey = "{CCEDFAA6-0D40-4786-8F24-4F89E357825D}";
            this.sectionReturns.Size = new System.Drawing.Size(385, 24);
            this.sectionReturns.TabIndex = 2;
            // 
            // returnsPanel
            // 
            this.returnsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.returnsPanel.AutoSize = true;
            this.returnsPanel.BackColor = System.Drawing.Color.White;
            this.returnsPanel.Location = new System.Drawing.Point(26, 31);
            this.returnsPanel.Name = "returnsPanel";
            this.returnsPanel.Size = new System.Drawing.Size(344, 134);
            this.returnsPanel.TabIndex = 76;
            // 
            // returnShipment
            // 
            this.returnShipment.AutoSize = true;
            this.returnShipment.BackColor = System.Drawing.Color.White;
            this.returnShipment.Location = new System.Drawing.Point(13, 12);
            this.returnShipment.Name = "returnShipment";
            this.returnShipment.Size = new System.Drawing.Size(143, 17);
            this.returnShipment.TabIndex = 75;
            this.returnShipment.Text = "This is a return shipment";
            this.returnShipment.UseVisualStyleBackColor = false;
            this.returnShipment.CheckedChanged += new System.EventHandler(this.OnReturnShipmentChanged);
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionRecipient.Collapsed = true;
            // 
            // sectionRecipient.ContentPanel
            // 
            this.sectionRecipient.ContentPanel.Controls.Add(this.residentialDetermination);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelAddress);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelResidentialCommercial);
            this.sectionRecipient.ContentPanel.Controls.Add(this.personControl);
            this.sectionRecipient.ExpandedHeight = 414;
            this.sectionRecipient.ExtraText = "";
            this.sectionRecipient.Location = new System.Drawing.Point(3, 3);
            this.sectionRecipient.Name = "sectionRecipient";
            this.sectionRecipient.SectionName = "To";
            this.sectionRecipient.SettingsKey = "{0ed19c0f-fcec-4c51-8fb8-9b01f4ad9937}";
            this.sectionRecipient.Size = new System.Drawing.Size(385, 24);
            this.sectionRecipient.TabIndex = 0;
            // 
            // residentialDetermination
            // 
            this.residentialDetermination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.residentialDetermination.FormattingEnabled = true;
            this.residentialDetermination.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.residentialDetermination.Location = new System.Drawing.Point(79, 355);
            this.residentialDetermination.Name = "residentialDetermination";
            this.residentialDetermination.PromptText = "(Multiple Values)";
            this.residentialDetermination.Size = new System.Drawing.Size(190, 21);
            this.residentialDetermination.TabIndex = 71;
            // 
            // labelAddress
            // 
            this.labelAddress.AutoSize = true;
            this.labelAddress.BackColor = System.Drawing.Color.White;
            this.labelAddress.Location = new System.Drawing.Point(23, 358);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(50, 13);
            this.labelAddress.TabIndex = 2;
            this.labelAddress.Text = "Address:";
            // 
            // labelResidentialCommercial
            // 
            this.labelResidentialCommercial.AutoSize = true;
            this.labelResidentialCommercial.BackColor = System.Drawing.Color.White;
            this.labelResidentialCommercial.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelResidentialCommercial.Location = new System.Drawing.Point(10, 335);
            this.labelResidentialCommercial.Name = "labelResidentialCommercial";
            this.labelResidentialCommercial.Size = new System.Drawing.Size(149, 13);
            this.labelResidentialCommercial.TabIndex = 1;
            this.labelResidentialCommercial.Text = "Residential \\ Commercial";
            // 
            // personControl
            // 
            this.personControl.AddressSelector = entityGridAddressSelector1;
            this.personControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.personControl.BackColor = System.Drawing.Color.Transparent;
            this.personControl.EnableValidationControls = false;
            this.personControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.personControl.Location = new System.Drawing.Point(3, 2);
            this.personControl.Name = "personControl";
            this.personControl.Size = new System.Drawing.Size(273, 330);
            this.personControl.TabIndex = 0;
            this.personControl.DestinationChanged += new System.EventHandler(this.OnRecipientDestinationChanged);
            // 
            // sectionShipment
            // 
            this.sectionShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionShipment.ExtraText = "";
            this.sectionShipment.Location = new System.Drawing.Point(3, 33);
            this.sectionShipment.Name = "sectionShipment";
            this.sectionShipment.SectionName = "Shipment Details";
            this.sectionShipment.SettingsKey = "{b1ef9b57-045b-4881-b290-4dbf6f070eff}";
            this.sectionShipment.Size = new System.Drawing.Size(385, 75);
            this.sectionShipment.TabIndex = 3;
            // 
            // ServiceControlBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScrollMargin = new System.Drawing.Size(0, 5);
            this.Controls.Add(this.sectionShipment);
            this.Controls.Add(this.sectionReturns);
            this.Controls.Add(this.sectionRecipient);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ServiceControlBase";
            this.Size = new System.Drawing.Size(391, 211);
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).EndInit();
            this.sectionReturns.ContentPanel.ResumeLayout(false);
            this.sectionReturns.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected ShipWorks.UI.Controls.CollapsibleGroupControl sectionRecipient;
        protected ShipWorks.Data.Controls.PersonControl personControl;
        protected System.Windows.Forms.Label labelResidentialCommercial;
        protected System.Windows.Forms.Label labelAddress;
        protected UI.Controls.MultiValueComboBox residentialDetermination;
        private System.Windows.Forms.CheckBox returnShipment;
        private System.Windows.Forms.Panel returnsPanel;
        protected UI.Controls.CollapsibleGroupControl sectionReturns;
        protected UI.Controls.CollapsibleGroupControl sectionShipment;
    }
}
