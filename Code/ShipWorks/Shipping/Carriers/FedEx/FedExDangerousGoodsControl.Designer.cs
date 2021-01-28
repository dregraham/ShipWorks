﻿namespace ShipWorks.Shipping.Carriers.FedEx
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
            this.batteryDetailsGroup = new System.Windows.Forms.GroupBox();
            this.batteryRegulatorySubtype = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.batteryPacking = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.batteryMaterial = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.batteryMaterialLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.batteryPackingLabel = new System.Windows.Forms.Label();
            this.containerTypeLabel = new System.Windows.Forms.Label();
            this.containerType = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.numberOfContainersLabel = new System.Windows.Forms.Label();
            this.numberOfContainers = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.signatoryPlace = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.signatoryContactNameLabel = new System.Windows.Forms.Label();
            this.signatoryContactName = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.signatoryPlaceLabel = new System.Windows.Forms.Label();
            this.signatoryTitleLabel = new System.Windows.Forms.Label();
            this.signatoryTitle = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.aircraftLabel = new System.Windows.Forms.Label();
            this.dangerousGoodsCargoAircraftOnly = new System.Windows.Forms.CheckBox();
            this.dangerousGoodsPackagingUnits = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.dangerousGoodsPackagingCounts = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.packagingLabel = new System.Windows.Forms.Label();
            this.dangerousGoodsAccessibility = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.hazardousMaterialGroupBox = new System.Windows.Forms.GroupBox();
            this.packingInstructions = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.dangerousGoodsAuthorization = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.packingInstructionsLabel = new System.Windows.Forms.Label();
            this.dangerousGoodsAuthorizationLabel = new System.Windows.Forms.Label();
            this.packingDetailsAircraftOnlyLabel = new System.Windows.Forms.Label();
            this.packingDetailsAircraftOnly = new System.Windows.Forms.CheckBox();
            this.hazardousMaterialTechnicalName = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.hazardousMaterialTechnicalNameLabel = new System.Windows.Forms.Label();
            this.hazardousMaterialQuantityValue = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.quantityLabel = new System.Windows.Forms.Label();
            this.hazardousMaterialPackingGroup = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.packingGroupLabel = new System.Windows.Forms.Label();
            this.hazardousMaterialProperName = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.hazardousMaterialProperNameLabel = new System.Windows.Forms.Label();
            this.hazardousMaterialId = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.hazardClass = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.hazardousMaterialCodeLabel = new System.Windows.Forms.Label();
            this.hazardClassLabel = new System.Windows.Forms.Label();
            this.offerorLabel = new System.Windows.Forms.Label();
            this.emergencyContactPhoneLabel = new System.Windows.Forms.Label();
            this.accessibilityLabel = new System.Windows.Forms.Label();
            this.dangerousGoodsTypeLabel = new System.Windows.Forms.Label();
            this.offeror = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.dangerousGoodsMaterialType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.emergencyContactPhone = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.panelDangerousGoodsDetails.SuspendLayout();
            this.batteryDetailsGroup.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.hazardousMaterialGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // dangerousGoodsEnabled
            // 
            this.dangerousGoodsEnabled.AutoSize = true;
            this.dangerousGoodsEnabled.Location = new System.Drawing.Point(0, 0);
            this.dangerousGoodsEnabled.Margin = new System.Windows.Forms.Padding(0);
            this.dangerousGoodsEnabled.Name = "dangerousGoodsEnabled";
            this.dangerousGoodsEnabled.Size = new System.Drawing.Size(217, 17);
            this.dangerousGoodsEnabled.TabIndex = 0;
            this.dangerousGoodsEnabled.Text = "This package contains dangerous goods";
            this.dangerousGoodsEnabled.UseVisualStyleBackColor = true;
            this.dangerousGoodsEnabled.CheckedChanged += new System.EventHandler(this.OnDangerousGoodsEnabledChanged);
            // 
            // panelDangerousGoodsDetails
            // 
            this.panelDangerousGoodsDetails.AutoSize = true;
            this.panelDangerousGoodsDetails.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelDangerousGoodsDetails.Controls.Add(this.batteryDetailsGroup);
            this.panelDangerousGoodsDetails.Controls.Add(this.containerTypeLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.containerType);
            this.panelDangerousGoodsDetails.Controls.Add(this.numberOfContainersLabel);
            this.panelDangerousGoodsDetails.Controls.Add(this.numberOfContainers);
            this.panelDangerousGoodsDetails.Controls.Add(this.groupBox1);
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
            this.panelDangerousGoodsDetails.Size = new System.Drawing.Size(306, 707);
            this.panelDangerousGoodsDetails.TabIndex = 1;
            this.panelDangerousGoodsDetails.Visible = false;
            // 
            // batteryDetailsGroup
            // 
            this.batteryDetailsGroup.Controls.Add(this.batteryRegulatorySubtype);
            this.batteryDetailsGroup.Controls.Add(this.batteryPacking);
            this.batteryDetailsGroup.Controls.Add(this.batteryMaterial);
            this.batteryDetailsGroup.Controls.Add(this.batteryMaterialLabel);
            this.batteryDetailsGroup.Controls.Add(this.label2);
            this.batteryDetailsGroup.Controls.Add(this.batteryPackingLabel);
            this.batteryDetailsGroup.Location = new System.Drawing.Point(9, 588);
            this.batteryDetailsGroup.Name = "batteryDetailsGroup";
            this.batteryDetailsGroup.Size = new System.Drawing.Size(294, 116);
            this.batteryDetailsGroup.TabIndex = 19;
            this.batteryDetailsGroup.TabStop = false;
            this.batteryDetailsGroup.Text = "Battery Details";
            // 
            // batteryRegulatorySubtype
            // 
            this.batteryRegulatorySubtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.batteryRegulatorySubtype.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.batteryRegulatorySubtype.FormattingEnabled = true;
            this.batteryRegulatorySubtype.Location = new System.Drawing.Point(109, 82);
            this.batteryRegulatorySubtype.Name = "batteryRegulatorySubtype";
            this.batteryRegulatorySubtype.PromptText = "(Multiple Values)";
            this.batteryRegulatorySubtype.Size = new System.Drawing.Size(171, 21);
            this.batteryRegulatorySubtype.TabIndex = 5;
            // 
            // batteryPacking
            // 
            this.batteryPacking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.batteryPacking.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.batteryPacking.FormattingEnabled = true;
            this.batteryPacking.Location = new System.Drawing.Point(109, 55);
            this.batteryPacking.Name = "batteryPacking";
            this.batteryPacking.PromptText = "(Multiple Values)";
            this.batteryPacking.Size = new System.Drawing.Size(171, 21);
            this.batteryPacking.TabIndex = 3;
            // 
            // batteryMaterial
            // 
            this.batteryMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.batteryMaterial.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.batteryMaterial.FormattingEnabled = true;
            this.batteryMaterial.Location = new System.Drawing.Point(109, 28);
            this.batteryMaterial.Name = "batteryMaterial";
            this.batteryMaterial.PromptText = "(Multiple Values)";
            this.batteryMaterial.Size = new System.Drawing.Size(171, 21);
            this.batteryMaterial.TabIndex = 1;
            // 
            // batteryMaterialLabel
            // 
            this.batteryMaterialLabel.AutoSize = true;
            this.batteryMaterialLabel.Location = new System.Drawing.Point(59, 31);
            this.batteryMaterialLabel.Name = "batteryMaterialLabel";
            this.batteryMaterialLabel.Size = new System.Drawing.Size(49, 13);
            this.batteryMaterialLabel.TabIndex = 0;
            this.batteryMaterialLabel.Text = "Material:";
            this.batteryMaterialLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Regulatory Subtype:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // batteryPackingLabel
            // 
            this.batteryPackingLabel.AutoSize = true;
            this.batteryPackingLabel.Location = new System.Drawing.Point(61, 58);
            this.batteryPackingLabel.Name = "batteryPackingLabel";
            this.batteryPackingLabel.Size = new System.Drawing.Size(47, 13);
            this.batteryPackingLabel.TabIndex = 2;
            this.batteryPackingLabel.Text = "Packing:";
            this.batteryPackingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // containerTypeLabel
            // 
            this.containerTypeLabel.AutoSize = true;
            this.containerTypeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.containerTypeLabel.Location = new System.Drawing.Point(32, 169);
            this.containerTypeLabel.Name = "containerTypeLabel";
            this.containerTypeLabel.Size = new System.Drawing.Size(85, 13);
            this.containerTypeLabel.TabIndex = 13;
            this.containerTypeLabel.Text = "Container Type:";
            this.containerTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // containerType
            // 
            this.containerType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.containerType.Location = new System.Drawing.Point(118, 166);
            this.containerType.Name = "containerType";
            this.containerType.Size = new System.Drawing.Size(171, 21);
            this.containerType.TabIndex = 14;
            // 
            // numberOfContainersLabel
            // 
            this.numberOfContainersLabel.AutoSize = true;
            this.numberOfContainersLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.numberOfContainersLabel.Location = new System.Drawing.Point(30, 196);
            this.numberOfContainersLabel.Name = "numberOfContainersLabel";
            this.numberOfContainersLabel.Size = new System.Drawing.Size(87, 13);
            this.numberOfContainersLabel.TabIndex = 15;
            this.numberOfContainersLabel.Text = "# of Containers:";
            this.numberOfContainersLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numberOfContainers
            // 
            this.numberOfContainers.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.numberOfContainers.Location = new System.Drawing.Point(118, 193);
            this.numberOfContainers.Name = "numberOfContainers";
            this.numberOfContainers.Size = new System.Drawing.Size(171, 21);
            this.numberOfContainers.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.signatoryPlace);
            this.groupBox1.Controls.Add(this.signatoryContactNameLabel);
            this.groupBox1.Controls.Add(this.signatoryContactName);
            this.groupBox1.Controls.Add(this.signatoryPlaceLabel);
            this.groupBox1.Controls.Add(this.signatoryTitleLabel);
            this.groupBox1.Controls.Add(this.signatoryTitle);
            this.groupBox1.Location = new System.Drawing.Point(9, 222);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 116);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dangerous Goods Signatory";
            // 
            // signatoryPlace
            // 
            this.signatoryPlace.Location = new System.Drawing.Point(109, 82);
            this.signatoryPlace.Name = "signatoryPlace";
            this.signatoryPlace.Size = new System.Drawing.Size(171, 21);
            this.signatoryPlace.TabIndex = 5;
            // 
            // signatoryContactNameLabel
            // 
            this.signatoryContactNameLabel.AutoSize = true;
            this.signatoryContactNameLabel.Location = new System.Drawing.Point(21, 31);
            this.signatoryContactNameLabel.Name = "signatoryContactNameLabel";
            this.signatoryContactNameLabel.Size = new System.Drawing.Size(87, 13);
            this.signatoryContactNameLabel.TabIndex = 0;
            this.signatoryContactNameLabel.Text = "Signatory Name:";
            this.signatoryContactNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // signatoryContactName
            // 
            this.signatoryContactName.Location = new System.Drawing.Point(109, 28);
            this.signatoryContactName.Name = "signatoryContactName";
            this.signatoryContactName.Size = new System.Drawing.Size(171, 21);
            this.signatoryContactName.TabIndex = 1;
            // 
            // signatoryPlaceLabel
            // 
            this.signatoryPlaceLabel.AutoSize = true;
            this.signatoryPlaceLabel.Location = new System.Drawing.Point(23, 85);
            this.signatoryPlaceLabel.Name = "signatoryPlaceLabel";
            this.signatoryPlaceLabel.Size = new System.Drawing.Size(85, 13);
            this.signatoryPlaceLabel.TabIndex = 4;
            this.signatoryPlaceLabel.Text = "Signatory Place:";
            this.signatoryPlaceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // signatoryTitleLabel
            // 
            this.signatoryTitleLabel.AutoSize = true;
            this.signatoryTitleLabel.Location = new System.Drawing.Point(28, 58);
            this.signatoryTitleLabel.Name = "signatoryTitleLabel";
            this.signatoryTitleLabel.Size = new System.Drawing.Size(80, 13);
            this.signatoryTitleLabel.TabIndex = 2;
            this.signatoryTitleLabel.Text = "Signatory Title:";
            this.signatoryTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // signatoryTitle
            // 
            this.signatoryTitle.Location = new System.Drawing.Point(109, 55);
            this.signatoryTitle.Name = "signatoryTitle";
            this.signatoryTitle.Size = new System.Drawing.Size(171, 21);
            this.signatoryTitle.TabIndex = 3;
            // 
            // aircraftLabel
            // 
            this.aircraftLabel.AutoSize = true;
            this.aircraftLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.aircraftLabel.Location = new System.Drawing.Point(70, 144);
            this.aircraftLabel.Name = "aircraftLabel";
            this.aircraftLabel.Size = new System.Drawing.Size(47, 13);
            this.aircraftLabel.TabIndex = 11;
            this.aircraftLabel.Text = "Aircraft:";
            this.aircraftLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dangerousGoodsCargoAircraftOnly
            // 
            this.dangerousGoodsCargoAircraftOnly.AutoSize = true;
            this.dangerousGoodsCargoAircraftOnly.Location = new System.Drawing.Point(118, 143);
            this.dangerousGoodsCargoAircraftOnly.Name = "dangerousGoodsCargoAircraftOnly";
            this.dangerousGoodsCargoAircraftOnly.Size = new System.Drawing.Size(116, 17);
            this.dangerousGoodsCargoAircraftOnly.TabIndex = 12;
            this.dangerousGoodsCargoAircraftOnly.Text = "Cargo aircraft only";
            this.dangerousGoodsCargoAircraftOnly.UseVisualStyleBackColor = true;
            // 
            // dangerousGoodsPackagingUnits
            // 
            this.dangerousGoodsPackagingUnits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsPackagingUnits.FormattingEnabled = true;
            this.dangerousGoodsPackagingUnits.Location = new System.Drawing.Point(213, 111);
            this.dangerousGoodsPackagingUnits.Name = "dangerousGoodsPackagingUnits";
            this.dangerousGoodsPackagingUnits.PromptText = "(Multiple Values)";
            this.dangerousGoodsPackagingUnits.Size = new System.Drawing.Size(76, 21);
            this.dangerousGoodsPackagingUnits.TabIndex = 10;
            // 
            // dangerousGoodsPackagingCounts
            // 
            this.dangerousGoodsPackagingCounts.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dangerousGoodsPackagingCounts.Location = new System.Drawing.Point(118, 111);
            this.dangerousGoodsPackagingCounts.Name = "dangerousGoodsPackagingCounts";
            this.dangerousGoodsPackagingCounts.Size = new System.Drawing.Size(89, 21);
            this.dangerousGoodsPackagingCounts.TabIndex = 9;
            // 
            // packagingLabel
            // 
            this.packagingLabel.AutoSize = true;
            this.packagingLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.packagingLabel.Location = new System.Drawing.Point(58, 114);
            this.packagingLabel.Name = "packagingLabel";
            this.packagingLabel.Size = new System.Drawing.Size(59, 13);
            this.packagingLabel.TabIndex = 8;
            this.packagingLabel.Text = "Packaging:";
            this.packagingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dangerousGoodsAccessibility
            // 
            this.dangerousGoodsAccessibility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsAccessibility.FormattingEnabled = true;
            this.dangerousGoodsAccessibility.Location = new System.Drawing.Point(118, 29);
            this.dangerousGoodsAccessibility.Name = "dangerousGoodsAccessibility";
            this.dangerousGoodsAccessibility.PromptText = "(Multiple Values)";
            this.dangerousGoodsAccessibility.Size = new System.Drawing.Size(171, 21);
            this.dangerousGoodsAccessibility.TabIndex = 3;
            // 
            // hazardousMaterialGroupBox
            // 
            this.hazardousMaterialGroupBox.Controls.Add(this.packingInstructions);
            this.hazardousMaterialGroupBox.Controls.Add(this.packingInstructionsLabel);
            this.hazardousMaterialGroupBox.Controls.Add(this.dangerousGoodsAuthorization);
            this.hazardousMaterialGroupBox.Controls.Add(this.dangerousGoodsAuthorizationLabel);
            this.hazardousMaterialGroupBox.Controls.Add(this.packingDetailsAircraftOnlyLabel);
            this.hazardousMaterialGroupBox.Controls.Add(this.packingDetailsAircraftOnly);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialTechnicalName);
            this.hazardousMaterialGroupBox.Controls.Add(this.hazardousMaterialTechnicalNameLabel);
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
            this.hazardousMaterialGroupBox.Location = new System.Drawing.Point(9, 344);
            this.hazardousMaterialGroupBox.Name = "hazardousMaterialGroupBox";
            this.hazardousMaterialGroupBox.Size = new System.Drawing.Size(294, 238);
            this.hazardousMaterialGroupBox.TabIndex = 18;
            this.hazardousMaterialGroupBox.TabStop = false;
            this.hazardousMaterialGroupBox.Text = "Hazardous Material Details";
            // 
            // packingInstructions
            // 
            this.packingInstructions.Location = new System.Drawing.Point(109, 208);
            this.packingInstructions.Name = "packingInstructions";
            this.packingInstructions.Size = new System.Drawing.Size(171, 21);
            this.packingInstructions.TabIndex = 15;
            // 
            // packingInstructionsLabel
            // 
            this.packingInstructionsLabel.AutoSize = true;
            this.packingInstructionsLabel.Location = new System.Drawing.Point(1, 211);
            this.packingInstructionsLabel.Name = "packingInstructionsLabel";
            this.packingInstructionsLabel.Size = new System.Drawing.Size(107, 13);
            this.packingInstructionsLabel.TabIndex = 14;
            this.packingInstructionsLabel.Text = "Packing Instructions:";
            this.packingInstructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // DangerousGoodsAuthorization
            // 
            this.dangerousGoodsAuthorization.Location = new System.Drawing.Point(109, 215);
            this.dangerousGoodsAuthorization.Name = "dangerousGoodAuthorization";
            this.dangerousGoodsAuthorization.Size = new System.Drawing.Size(171, 21);
            this.dangerousGoodsAuthorization.TabIndex = 15;
            // 
            // packingInstructionsLabel
            // 
            this.dangerousGoodsAuthorizationLabel.AutoSize = true;
            this.dangerousGoodsAuthorizationLabel.Location = new System.Drawing.Point(1, 218);
            this.dangerousGoodsAuthorizationLabel.Name = "dangerousGoodAuthorizationLabel";
            this.dangerousGoodsAuthorizationLabel.Size = new System.Drawing.Size(107, 13);
            this.dangerousGoodsAuthorizationLabel.TabIndex = 14;
            this.dangerousGoodsAuthorizationLabel.Text = "Authorization:";
            this.dangerousGoodsAuthorizationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // packingDetailsAircraftOnlyLabel
            // 
            this.packingDetailsAircraftOnlyLabel.AutoSize = true;
            this.packingDetailsAircraftOnlyLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.packingDetailsAircraftOnlyLabel.Location = new System.Drawing.Point(61, 186);
            this.packingDetailsAircraftOnlyLabel.Name = "packingDetailsAircraftOnlyLabel";
            this.packingDetailsAircraftOnlyLabel.Size = new System.Drawing.Size(47, 13);
            this.packingDetailsAircraftOnlyLabel.TabIndex = 12;
            this.packingDetailsAircraftOnlyLabel.Text = "Aircraft:";
            this.packingDetailsAircraftOnlyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // packingDetailsAircraftOnly
            // 
            this.packingDetailsAircraftOnly.AutoSize = true;
            this.packingDetailsAircraftOnly.Location = new System.Drawing.Point(109, 185);
            this.packingDetailsAircraftOnly.Name = "packingDetailsAircraftOnly";
            this.packingDetailsAircraftOnly.Size = new System.Drawing.Size(116, 17);
            this.packingDetailsAircraftOnly.TabIndex = 13;
            this.packingDetailsAircraftOnly.Text = "Cargo aircraft only";
            this.packingDetailsAircraftOnly.UseVisualStyleBackColor = true;
            // 
            // hazardousMaterialTechnicalName
            // 
            this.hazardousMaterialTechnicalName.Location = new System.Drawing.Point(109, 105);
            this.hazardousMaterialTechnicalName.Name = "hazardousMaterialTechnicalName";
            this.hazardousMaterialTechnicalName.Size = new System.Drawing.Size(171, 21);
            this.hazardousMaterialTechnicalName.TabIndex = 7;
            // 
            // hazardousMaterialTechnicalNameLabel
            // 
            this.hazardousMaterialTechnicalNameLabel.AutoSize = true;
            this.hazardousMaterialTechnicalNameLabel.Location = new System.Drawing.Point(23, 108);
            this.hazardousMaterialTechnicalNameLabel.Name = "hazardousMaterialTechnicalNameLabel";
            this.hazardousMaterialTechnicalNameLabel.Size = new System.Drawing.Size(85, 13);
            this.hazardousMaterialTechnicalNameLabel.TabIndex = 6;
            this.hazardousMaterialTechnicalNameLabel.Text = "Technical Name:";
            this.hazardousMaterialTechnicalNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardousMaterialQuantityValue
            // 
            this.hazardousMaterialQuantityValue.Location = new System.Drawing.Point(109, 158);
            this.hazardousMaterialQuantityValue.Name = "hazardousMaterialQuantityValue";
            this.hazardousMaterialQuantityValue.Size = new System.Drawing.Size(171, 21);
            this.hazardousMaterialQuantityValue.TabIndex = 11;
            // 
            // quantityLabel
            // 
            this.quantityLabel.AutoSize = true;
            this.quantityLabel.Location = new System.Drawing.Point(55, 161);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(53, 13);
            this.quantityLabel.TabIndex = 10;
            this.quantityLabel.Text = "Quantity:";
            this.quantityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardousMaterialPackingGroup
            // 
            this.hazardousMaterialPackingGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.hazardousMaterialPackingGroup.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.hazardousMaterialPackingGroup.FormattingEnabled = true;
            this.hazardousMaterialPackingGroup.Location = new System.Drawing.Point(109, 131);
            this.hazardousMaterialPackingGroup.Name = "hazardousMaterialPackingGroup";
            this.hazardousMaterialPackingGroup.PromptText = "(Multiple Values)";
            this.hazardousMaterialPackingGroup.Size = new System.Drawing.Size(171, 21);
            this.hazardousMaterialPackingGroup.TabIndex = 9;
            // 
            // packingGroupLabel
            // 
            this.packingGroupLabel.AutoSize = true;
            this.packingGroupLabel.Location = new System.Drawing.Point(29, 134);
            this.packingGroupLabel.Name = "packingGroupLabel";
            this.packingGroupLabel.Size = new System.Drawing.Size(79, 13);
            this.packingGroupLabel.TabIndex = 8;
            this.packingGroupLabel.Text = "Packing Group:";
            this.packingGroupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardousMaterialProperName
            // 
            this.hazardousMaterialProperName.Location = new System.Drawing.Point(109, 79);
            this.hazardousMaterialProperName.Name = "hazardousMaterialProperName";
            this.hazardousMaterialProperName.Size = new System.Drawing.Size(171, 21);
            this.hazardousMaterialProperName.TabIndex = 5;
            // 
            // hazardousMaterialProperNameLabel
            // 
            this.hazardousMaterialProperNameLabel.AutoSize = true;
            this.hazardousMaterialProperNameLabel.Location = new System.Drawing.Point(35, 82);
            this.hazardousMaterialProperNameLabel.Name = "hazardousMaterialProperNameLabel";
            this.hazardousMaterialProperNameLabel.Size = new System.Drawing.Size(73, 13);
            this.hazardousMaterialProperNameLabel.TabIndex = 4;
            this.hazardousMaterialProperNameLabel.Text = "Proper Name:";
            this.hazardousMaterialProperNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardousMaterialId
            // 
            this.hazardousMaterialId.Location = new System.Drawing.Point(109, 28);
            this.hazardousMaterialId.Name = "hazardousMaterialId";
            this.hazardousMaterialId.Size = new System.Drawing.Size(171, 21);
            this.hazardousMaterialId.TabIndex = 1;
            // 
            // hazardClass
            // 
            this.hazardClass.Location = new System.Drawing.Point(109, 54);
            this.hazardClass.Name = "hazardClass";
            this.hazardClass.Size = new System.Drawing.Size(171, 21);
            this.hazardClass.TabIndex = 3;
            // 
            // hazardousMaterialCodeLabel
            // 
            this.hazardousMaterialCodeLabel.AutoSize = true;
            this.hazardousMaterialCodeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.hazardousMaterialCodeLabel.Location = new System.Drawing.Point(45, 31);
            this.hazardousMaterialCodeLabel.Name = "hazardousMaterialCodeLabel";
            this.hazardousMaterialCodeLabel.Size = new System.Drawing.Size(63, 13);
            this.hazardousMaterialCodeLabel.TabIndex = 0;
            this.hazardousMaterialCodeLabel.Text = "Material ID:";
            // 
            // hazardClassLabel
            // 
            this.hazardClassLabel.AutoSize = true;
            this.hazardClassLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.hazardClassLabel.Location = new System.Drawing.Point(35, 57);
            this.hazardClassLabel.Name = "hazardClassLabel";
            this.hazardClassLabel.Size = new System.Drawing.Size(73, 13);
            this.hazardClassLabel.TabIndex = 2;
            this.hazardClassLabel.Text = "Hazard Class:";
            // 
            // offerorLabel
            // 
            this.offerorLabel.AutoSize = true;
            this.offerorLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.offerorLabel.Location = new System.Drawing.Point(70, 87);
            this.offerorLabel.Name = "offerorLabel";
            this.offerorLabel.Size = new System.Drawing.Size(47, 13);
            this.offerorLabel.TabIndex = 6;
            this.offerorLabel.Text = "Offeror:";
            this.offerorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // emergencyContactPhoneLabel
            // 
            this.emergencyContactPhoneLabel.AutoSize = true;
            this.emergencyContactPhoneLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.emergencyContactPhoneLabel.Location = new System.Drawing.Point(20, 60);
            this.emergencyContactPhoneLabel.Name = "emergencyContactPhoneLabel";
            this.emergencyContactPhoneLabel.Size = new System.Drawing.Size(97, 13);
            this.emergencyContactPhoneLabel.TabIndex = 4;
            this.emergencyContactPhoneLabel.Text = "Emergency Phone:";
            this.emergencyContactPhoneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // accessibilityLabel
            // 
            this.accessibilityLabel.AutoSize = true;
            this.accessibilityLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.accessibilityLabel.Location = new System.Drawing.Point(22, 32);
            this.accessibilityLabel.Name = "accessibilityLabel";
            this.accessibilityLabel.Size = new System.Drawing.Size(95, 13);
            this.accessibilityLabel.TabIndex = 2;
            this.accessibilityLabel.Text = "Accessibility Type:";
            this.accessibilityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dangerousGoodsTypeLabel
            // 
            this.dangerousGoodsTypeLabel.AutoSize = true;
            this.dangerousGoodsTypeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dangerousGoodsTypeLabel.Location = new System.Drawing.Point(41, 4);
            this.dangerousGoodsTypeLabel.Name = "dangerousGoodsTypeLabel";
            this.dangerousGoodsTypeLabel.Size = new System.Drawing.Size(76, 13);
            this.dangerousGoodsTypeLabel.TabIndex = 0;
            this.dangerousGoodsTypeLabel.Text = "Material Type:";
            this.dangerousGoodsTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // offeror
            // 
            this.offeror.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.offeror.Location = new System.Drawing.Point(118, 84);
            this.offeror.Name = "offeror";
            this.offeror.Size = new System.Drawing.Size(171, 21);
            this.offeror.TabIndex = 7;
            // 
            // dangerousGoodsMaterialType
            // 
            this.dangerousGoodsMaterialType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsMaterialType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dangerousGoodsMaterialType.FormattingEnabled = true;
            this.dangerousGoodsMaterialType.Location = new System.Drawing.Point(118, 1);
            this.dangerousGoodsMaterialType.Name = "dangerousGoodsMaterialType";
            this.dangerousGoodsMaterialType.PromptText = "(Multiple Values)";
            this.dangerousGoodsMaterialType.Size = new System.Drawing.Size(171, 21);
            this.dangerousGoodsMaterialType.TabIndex = 1;
            this.dangerousGoodsMaterialType.SelectedIndexChanged += new System.EventHandler(this.OnMaterialTypeChanged);
            // 
            // emergencyContactPhone
            // 
            this.emergencyContactPhone.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.emergencyContactPhone.Location = new System.Drawing.Point(118, 57);
            this.emergencyContactPhone.Name = "emergencyContactPhone";
            this.emergencyContactPhone.Size = new System.Drawing.Size(171, 21);
            this.emergencyContactPhone.TabIndex = 5;
            // 
            // FedExDangerousGoodsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.panelDangerousGoodsDetails);
            this.Controls.Add(this.dangerousGoodsEnabled);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FedExDangerousGoodsControl";
            this.Size = new System.Drawing.Size(309, 728);
            this.panelDangerousGoodsDetails.ResumeLayout(false);
            this.panelDangerousGoodsDetails.PerformLayout();
            this.batteryDetailsGroup.ResumeLayout(false);
            this.batteryDetailsGroup.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private UI.Controls.MultiValueTextBox hazardousMaterialTechnicalName;
        private System.Windows.Forms.Label hazardousMaterialTechnicalNameLabel;
        private UI.Controls.MultiValueTextBox signatoryPlace;
        private System.Windows.Forms.Label signatoryPlaceLabel;
        private UI.Controls.MultiValueTextBox signatoryTitle;
        private System.Windows.Forms.Label signatoryTitleLabel;
        private UI.Controls.MultiValueTextBox signatoryContactName;
        private System.Windows.Forms.Label signatoryContactNameLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label containerTypeLabel;
        private UI.Controls.MultiValueTextBox containerType;
        private System.Windows.Forms.Label numberOfContainersLabel;
        private UI.Controls.MultiValueTextBox numberOfContainers;
        private System.Windows.Forms.Label packingDetailsAircraftOnlyLabel;
        private System.Windows.Forms.CheckBox packingDetailsAircraftOnly;
        private UI.Controls.MultiValueTextBox packingInstructions;
        private System.Windows.Forms.Label packingInstructionsLabel;
        private UI.Controls.MultiValueTextBox dangerousGoodsAuthorization;
        private System.Windows.Forms.Label dangerousGoodsAuthorizationLabel;
        private System.Windows.Forms.GroupBox batteryDetailsGroup;
        private UI.Controls.MultiValueComboBox batteryMaterial;
        private System.Windows.Forms.Label batteryMaterialLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label batteryPackingLabel;
        private UI.Controls.MultiValueComboBox batteryRegulatorySubtype;
        private UI.Controls.MultiValueComboBox batteryPacking;
    }
}
