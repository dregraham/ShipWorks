namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExDangerousGoodsControl
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
            this.dangerousGoodsEnabled = new System.Windows.Forms.CheckBox();
            this.panelDangerousGoodsDetails = new System.Windows.Forms.Panel();
            this.aircraftLabel = new System.Windows.Forms.Label();
            this.dangerousGoodsCargoAircraftOnly = new System.Windows.Forms.CheckBox();
            this.packagingLabel = new System.Windows.Forms.Label();
            this.hazardousMaterialGroupBox = new System.Windows.Forms.GroupBox();
            this.quantityLabel = new System.Windows.Forms.Label();
            this.packingGroupLabel = new System.Windows.Forms.Label();
            this.hazardousMaterialProperNameLabel = new System.Windows.Forms.Label();
            this.hazardousMaterialCodeLabel = new System.Windows.Forms.Label();
            this.hazardClassLabel = new System.Windows.Forms.Label();
            this.offerorLabel = new System.Windows.Forms.Label();
            this.emergencyContactPhoneLabel = new System.Windows.Forms.Label();
            this.accessibilityLabel = new System.Windows.Forms.Label();
            this.dangerousGoodsTypeLabel = new System.Windows.Forms.Label();
            this.dangerousGoodsPackagingUnits = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.dangerousGoodsPackagingCounts = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.dangerousGoodsAccessibility = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.hazardousMaterialQuantityValue = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.hazardousMaterialPackingGroup = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.hazardousMaterialProperName = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.hazardousMaterialId = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.hazardClass = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.offeror = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.dangerousGoodsMaterialType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.emergencyContactPhone = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.panelDangerousGoodsDetails.SuspendLayout();
            this.hazardousMaterialGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dangerousGoodsEnabled
            // 
            this.dangerousGoodsEnabled.AutoSize = true;
            this.dangerousGoodsEnabled.Location = new System.Drawing.Point(0, 0);
            this.dangerousGoodsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.dangerousGoodsEnabled.Name = "dangerousGoodsEnabled";
            this.dangerousGoodsEnabled.Size = new System.Drawing.Size(219, 17);
            this.dangerousGoodsEnabled.TabIndex = 0;
            this.dangerousGoodsEnabled.Text = "This package contains dangerous goods";
            this.dangerousGoodsEnabled.UseVisualStyleBackColor = true;
            this.dangerousGoodsEnabled.CheckedChanged += new System.EventHandler(this.OnDangerousGoodsEnabledChanged);
            // 
            // panelDangerousGoodsDetails
            // 
            this.panelDangerousGoodsDetails.AutoSize = true;
            this.panelDangerousGoodsDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDangerousGoodsDetails.Controls.Add(this.aircraftLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.dangerousGoodsCargoAircraftOnly);
            this.panelDangerousGoodsDetails.Controls.Add(this.dangerousGoodsPackagingUnits);
            this.panelDangerousGoodsDetails.Controls.Add(this.dangerousGoodsPackagingCounts);
            this.panelDangerousGoodsDetails.Controls.Add(this.packagingLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.dangerousGoodsAccessibility);
            this.panelDangerousGoodsDetails.Controls.Add(this.hazardousMaterialGroupBox);
            this.panelDangerousGoodsDetails.Controls.Add(this.offerorLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.emergencyContactPhoneLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.accessibilityLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.dangerousGoodsTypeLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.offeror);
            this.panelDangerousGoodsDetails.Controls.Add(this.dangerousGoodsMaterialType);
            this.panelDangerousGoodsDetails.Controls.Add(this.emergencyContactPhone);
            this.panelDangerousGoodsDetails.Location = new System.Drawing.Point(0, 18);
            this.panelDangerousGoodsDetails.Name = "panelDangerousGoodsDetails";
            this.panelDangerousGoodsDetails.Size = new System.Drawing.Size(283, 349);
            this.panelDangerousGoodsDetails.TabIndex = 14;
            this.panelDangerousGoodsDetails.Visible = false;
            // 
            // aircraftLabel
            // 
            this.aircraftLabel.AutoSize = true;
            this.aircraftLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.aircraftLabel.Location = new System.Drawing.Point(56, 144);
            this.aircraftLabel.Name = "aircraftLabel";
            this.aircraftLabel.Size = new System.Drawing.Size(47, 13);
            this.aircraftLabel.TabIndex = 27;
            this.aircraftLabel.Text = "Aircraft:";
            this.aircraftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dangerousGoodsCargoAircraftOnly
            // 
            this.dangerousGoodsCargoAircraftOnly.AutoSize = true;
            this.dangerousGoodsCargoAircraftOnly.Location = new System.Drawing.Point(109, 143);
            this.dangerousGoodsCargoAircraftOnly.Name = "dangerousGoodsCargoAircraftOnly";
            this.dangerousGoodsCargoAircraftOnly.Size = new System.Drawing.Size(111, 17);
            this.dangerousGoodsCargoAircraftOnly.TabIndex = 26;
            this.dangerousGoodsCargoAircraftOnly.Text = "Cargo aircraft only";
            this.dangerousGoodsCargoAircraftOnly.UseVisualStyleBackColor = true;
            // 
            // packagingLabel
            // 
            this.packagingLabel.AutoSize = true;
            this.packagingLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.packagingLabel.Location = new System.Drawing.Point(46, 114);
            this.packagingLabel.Name = "packagingLabel";
            this.packagingLabel.Size = new System.Drawing.Size(59, 13);
            this.packagingLabel.TabIndex = 23;
            this.packagingLabel.Text = "Packaging:";
            this.packagingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardousMaterialGroupBox
            // 
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialQuantityValue);
            this.hazardousMaterialGroupBox.Controls.Add(this.quantityLabel);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialPackingGroup);
            this.hazardousMaterialGroupBox.Controls.Add(this.packingGroupLabel);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialProperName);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialProperNameLabel);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialId);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardClass);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialCodeLabel);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardClassLabel);
            this.hazardousMaterialGroupBox.Location = new System.Drawing.Point(9, 173);
            this.hazardousMaterialGroupBox.Name = "hazardousMaterialGroupBox";
            this.hazardousMaterialGroupBox.Size = new System.Drawing.Size(271, 173);
            this.hazardousMaterialGroupBox.TabIndex = 20;
            this.hazardousMaterialGroupBox.TabStop = false;
            this.hazardousMaterialGroupBox.Text = "Hazardous Material Details";
            // 
            // quantityLabel
            // 
            this.quantityLabel.AutoSize = true;
            this.quantityLabel.Location = new System.Drawing.Point(46, 137);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(49, 13);
            this.quantityLabel.TabIndex = 17;
            this.quantityLabel.Text = "Quantity:";
            this.quantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // packingGroupLabel
            // 
            this.packingGroupLabel.AutoSize = true;
            this.packingGroupLabel.Location = new System.Drawing.Point(13, 110);
            this.packingGroupLabel.Name = "packingGroupLabel";
            this.packingGroupLabel.Size = new System.Drawing.Size(81, 13);
            this.packingGroupLabel.TabIndex = 15;
            this.packingGroupLabel.Text = "Packing Group:";
            this.packingGroupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardousMaterialProperNameLabel
            // 
            this.hazardousMaterialProperNameLabel.AutoSize = true;
            this.hazardousMaterialProperNameLabel.Location = new System.Drawing.Point(22, 82);
            this.hazardousMaterialProperNameLabel.Name = "hazardousMaterialProperNameLabel";
            this.hazardousMaterialProperNameLabel.Size = new System.Drawing.Size(72, 13);
            this.hazardousMaterialProperNameLabel.TabIndex = 13;
            this.hazardousMaterialProperNameLabel.Text = "Proper Name:";
            this.hazardousMaterialProperNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardousMaterialCodeLabel
            // 
            this.hazardousMaterialCodeLabel.AutoSize = true;
            this.hazardousMaterialCodeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.hazardousMaterialCodeLabel.Location = new System.Drawing.Point(31, 31);
            this.hazardousMaterialCodeLabel.Name = "hazardousMaterialCodeLabel";
            this.hazardousMaterialCodeLabel.Size = new System.Drawing.Size(63, 13);
            this.hazardousMaterialCodeLabel.TabIndex = 9;
            this.hazardousMaterialCodeLabel.Text = "Material ID:";
            // 
            // hazardClassLabel
            // 
            this.hazardClassLabel.AutoSize = true;
            this.hazardClassLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.hazardClassLabel.Location = new System.Drawing.Point(22, 57);
            this.hazardClassLabel.Name = "hazardClassLabel";
            this.hazardClassLabel.Size = new System.Drawing.Size(73, 13);
            this.hazardClassLabel.TabIndex = 11;
            this.hazardClassLabel.Text = "Hazard Class:";
            // 
            // offerorLabel
            // 
            this.offerorLabel.AutoSize = true;
            this.offerorLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.offerorLabel.Location = new System.Drawing.Point(61, 87);
            this.offerorLabel.Name = "offerorLabel";
            this.offerorLabel.Size = new System.Drawing.Size(47, 13);
            this.offerorLabel.TabIndex = 21;
            this.offerorLabel.Text = "Offeror:";
            this.offerorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // emergencyContactPhoneLabel
            // 
            this.emergencyContactPhoneLabel.AutoSize = true;
            this.emergencyContactPhoneLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.emergencyContactPhoneLabel.Location = new System.Drawing.Point(6, 60);
            this.emergencyContactPhoneLabel.Name = "emergencyContactPhoneLabel";
            this.emergencyContactPhoneLabel.Size = new System.Drawing.Size(97, 13);
            this.emergencyContactPhoneLabel.TabIndex = 18;
            this.emergencyContactPhoneLabel.Text = "Emergency Phone:";
            this.emergencyContactPhoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // accessibilityLabel
            // 
            this.accessibilityLabel.AutoSize = true;
            this.accessibilityLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.accessibilityLabel.Location = new System.Drawing.Point(8, 32);
            this.accessibilityLabel.Name = "accessibilityLabel";
            this.accessibilityLabel.Size = new System.Drawing.Size(95, 13);
            this.accessibilityLabel.TabIndex = 16;
            this.accessibilityLabel.Text = "Accessibility Type:";
            this.accessibilityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dangerousGoodsTypeLabel
            // 
            this.dangerousGoodsTypeLabel.AutoSize = true;
            this.dangerousGoodsTypeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dangerousGoodsTypeLabel.Location = new System.Drawing.Point(29, 4);
            this.dangerousGoodsTypeLabel.Name = "dangerousGoodsTypeLabel";
            this.dangerousGoodsTypeLabel.Size = new System.Drawing.Size(76, 13);
            this.dangerousGoodsTypeLabel.TabIndex = 14;
            this.dangerousGoodsTypeLabel.Text = "Material Type:";
            this.dangerousGoodsTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dangerousGoodsPackagingUnits
            // 
            this.dangerousGoodsPackagingUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsPackagingUnits.FormattingEnabled = true;
            this.dangerousGoodsPackagingUnits.Location = new System.Drawing.Point(204, 111);
            this.dangerousGoodsPackagingUnits.Name = "dangerousGoodsPackagingUnits";
            this.dangerousGoodsPackagingUnits.PromptText = "(Multiple Values)";
            this.dangerousGoodsPackagingUnits.Size = new System.Drawing.Size(76, 21);
            this.dangerousGoodsPackagingUnits.TabIndex = 25;
            // 
            // dangerousGoodsPackagingCounts
            // 
            this.dangerousGoodsPackagingCounts.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dangerousGoodsPackagingCounts.Location = new System.Drawing.Point(109, 111);
            this.dangerousGoodsPackagingCounts.Name = "dangerousGoodsPackagingCounts";
            this.dangerousGoodsPackagingCounts.Size = new System.Drawing.Size(89, 21);
            this.dangerousGoodsPackagingCounts.TabIndex = 24;
            // 
            // dangerousGoodsAccessibility
            // 
            this.dangerousGoodsAccessibility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsAccessibility.FormattingEnabled = true;
            this.dangerousGoodsAccessibility.Location = new System.Drawing.Point(109, 29);
            this.dangerousGoodsAccessibility.Name = "dangerousGoodsAccessibility";
            this.dangerousGoodsAccessibility.PromptText = "(Multiple Values)";
            this.dangerousGoodsAccessibility.Size = new System.Drawing.Size(171, 21);
            this.dangerousGoodsAccessibility.TabIndex = 17;
            // 
            // hazardousMaterialQuantityValue
            // 
            this.hazardousMaterialQuantityValue.Location = new System.Drawing.Point(100, 134);
            this.hazardousMaterialQuantityValue.Name = "hazardousMaterialQuantityValue";
            this.hazardousMaterialQuantityValue.Size = new System.Drawing.Size(143, 20);
            this.hazardousMaterialQuantityValue.TabIndex = 18;
            // 
            // hazardousMaterialPackingGroup
            // 
            this.hazardousMaterialPackingGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hazardousMaterialPackingGroup.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.hazardousMaterialPackingGroup.FormattingEnabled = true;
            this.hazardousMaterialPackingGroup.Location = new System.Drawing.Point(100, 107);
            this.hazardousMaterialPackingGroup.Name = "hazardousMaterialPackingGroup";
            this.hazardousMaterialPackingGroup.PromptText = "(Multiple Values)";
            this.hazardousMaterialPackingGroup.Size = new System.Drawing.Size(143, 21);
            this.hazardousMaterialPackingGroup.TabIndex = 16;
            // 
            // hazardousMaterialProperName
            // 
            this.hazardousMaterialProperName.Location = new System.Drawing.Point(100, 79);
            this.hazardousMaterialProperName.Name = "hazardousMaterialProperName";
            this.hazardousMaterialProperName.Size = new System.Drawing.Size(143, 20);
            this.hazardousMaterialProperName.TabIndex = 14;
            // 
            // hazardousMaterialId
            // 
            this.hazardousMaterialId.Location = new System.Drawing.Point(100, 28);
            this.hazardousMaterialId.Name = "hazardousMaterialId";
            this.hazardousMaterialId.Size = new System.Drawing.Size(143, 20);
            this.hazardousMaterialId.TabIndex = 10;
            // 
            // hazardClass
            // 
            this.hazardClass.Location = new System.Drawing.Point(100, 54);
            this.hazardClass.Name = "hazardClass";
            this.hazardClass.Size = new System.Drawing.Size(143, 20);
            this.hazardClass.TabIndex = 12;
            // 
            // offeror
            // 
            this.offeror.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.offeror.Location = new System.Drawing.Point(109, 84);
            this.offeror.Name = "offeror";
            this.offeror.Size = new System.Drawing.Size(171, 21);
            this.offeror.TabIndex = 22;
            // 
            // dangerousGoodsMaterialType
            // 
            this.dangerousGoodsMaterialType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsMaterialType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dangerousGoodsMaterialType.FormattingEnabled = true;
            this.dangerousGoodsMaterialType.Location = new System.Drawing.Point(109, 1);
            this.dangerousGoodsMaterialType.Name = "dangerousGoodsMaterialType";
            this.dangerousGoodsMaterialType.PromptText = "(Multiple Values)";
            this.dangerousGoodsMaterialType.Size = new System.Drawing.Size(171, 21);
            this.dangerousGoodsMaterialType.TabIndex = 2;
            this.dangerousGoodsMaterialType.SelectedIndexChanged += new System.EventHandler(this.OnMaterialTypeChanged);
            // 
            // emergencyContactPhone
            // 
            this.emergencyContactPhone.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.emergencyContactPhone.Location = new System.Drawing.Point(109, 57);
            this.emergencyContactPhone.Name = "emergencyContactPhone";
            this.emergencyContactPhone.Size = new System.Drawing.Size(171, 21);
            this.emergencyContactPhone.TabIndex = 6;
            // 
            // FedExDangerousGoodsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelDangerousGoodsDetails);
            this.Controls.Add(this.dangerousGoodsEnabled);
            this.Name = "FedExDangerousGoodsControl";
            this.Size = new System.Drawing.Size(286, 370);
            this.panelDangerousGoodsDetails.ResumeLayout(false);
            this.panelDangerousGoodsDetails.PerformLayout();
            this.hazardousMaterialGroupBox.ResumeLayout(false);
            this.hazardousMaterialGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox dangerousGoodsEnabled;
        private System.Windows.Forms.Panel panelDangerousGoodsDetails;
        private System.Windows.Forms.Label aircraftLabel;
        private System.Windows.Forms.CheckBox dangerousGoodsCargoAircraftOnly;
        private UI.Controls.MultiValueComboBox dangerousGoodsPackagingUnits;
        private UI.Controls.MultiValueTextBox dangerousGoodsPackagingCounts;
        private System.Windows.Forms.Label packagingLabel;
        private UI.Controls.MultiValueComboBox dangerousGoodsAccessibility;
        private System.Windows.Forms.GroupBox hazardousMaterialGroupBox;
        private UI.Controls.MultiValueTextBox hazardousMaterialQuantityValue;
        private System.Windows.Forms.Label quantityLabel;
        private UI.Controls.MultiValueComboBox hazardousMaterialPackingGroup;
        private System.Windows.Forms.Label packingGroupLabel;
        private UI.Controls.MultiValueTextBox hazardousMaterialProperName;
        private System.Windows.Forms.Label hazardousMaterialProperNameLabel;
        private UI.Controls.MultiValueTextBox hazardousMaterialId;
        private UI.Controls.MultiValueTextBox hazardClass;
        private System.Windows.Forms.Label hazardousMaterialCodeLabel;
        private System.Windows.Forms.Label hazardClassLabel;
        private System.Windows.Forms.Label offerorLabel;
        private System.Windows.Forms.Label emergencyContactPhoneLabel;
        private System.Windows.Forms.Label accessibilityLabel;
        private System.Windows.Forms.Label dangerousGoodsTypeLabel;
        private UI.Controls.MultiValueTextBox offeror;
        private UI.Controls.MultiValueComboBox dangerousGoodsMaterialType;
        private UI.Controls.MultiValueTextBox emergencyContactPhone;
    }
}
