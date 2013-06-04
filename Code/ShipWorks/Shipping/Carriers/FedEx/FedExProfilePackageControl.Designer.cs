﻿using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExProfilePackageControl
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.packagingCount = new ShipWorks.UI.Controls.NumericTextBox();
            this.quantity = new ShipWorks.Shipping.Carriers.FedEx.FedExProfilePackageHazardousQuantityControl();
            this.quantityState = new System.Windows.Forms.CheckBox();
            this.packingGroupState = new System.Windows.Forms.CheckBox();
            this.properNameState = new System.Windows.Forms.CheckBox();
            this.labelQuantity = new System.Windows.Forms.Label();
            this.packingGroup = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackingGroup = new System.Windows.Forms.Label();
            this.properName = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelProperName = new System.Windows.Forms.Label();
            this.aircraftState = new System.Windows.Forms.CheckBox();
            this.aircarft = new System.Windows.Forms.CheckBox();
            this.labelAircraft = new System.Windows.Forms.Label();
            this.packagingCountState = new System.Windows.Forms.CheckBox();
            this.labelpackagingCount = new System.Windows.Forms.Label();
            this.hazardClassState = new System.Windows.Forms.CheckBox();
            this.hazardousMaterialIdState = new System.Windows.Forms.CheckBox();
            this.offerorState = new System.Windows.Forms.CheckBox();
            this.emergencyContactPhoneState = new System.Windows.Forms.CheckBox();
            this.dangerousGoodsAccessibilityState = new System.Windows.Forms.CheckBox();
            this.dangerousGoodsMaterialTypeState = new System.Windows.Forms.CheckBox();
            this.dangerousGoodsEnabledState = new System.Windows.Forms.CheckBox();
            this.hazardousMaterialId = new System.Windows.Forms.TextBox();
            this.hazardClass = new System.Windows.Forms.TextBox();
            this.labelDangerousGoods = new System.Windows.Forms.Label();
            this.labelHazardClass = new System.Windows.Forms.Label();
            this.dangerousGoodsAccessibility = new System.Windows.Forms.ComboBox();
            this.labelHazardousMaterialCode = new System.Windows.Forms.Label();
            this.containsAlcohol = new System.Windows.Forms.CheckBox();
            this.labelAlcohol = new System.Windows.Forms.Label();
            this.dangerousGoodsEnabled = new System.Windows.Forms.CheckBox();
            this.labelOfferor = new System.Windows.Forms.Label();
            this.alcoholState = new System.Windows.Forms.CheckBox();
            this.labelEmergencyContactPhone = new System.Windows.Forms.Label();
            this.priorityAlertControl = new ShipWorks.Shipping.Carriers.FedEx.FedExProfilePackagePriorityAlertsControl();
            this.labelAccessibility = new System.Windows.Forms.Label();
            this.priorityAlertState = new System.Windows.Forms.CheckBox();
            this.labelDangerousGoodsMaterialType = new System.Windows.Forms.Label();
            this.dryIceState = new System.Windows.Forms.CheckBox();
            this.offeror = new System.Windows.Forms.TextBox();
            this.dryIceWeight = new ShipWorks.UI.Controls.WeightControl();
            this.dangerousGoodsMaterialType = new System.Windows.Forms.ComboBox();
            this.labelDryIceWeight = new System.Windows.Forms.Label();
            this.emergencyContactPhone = new System.Windows.Forms.TextBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.dimensionsState = new System.Windows.Forms.CheckBox();
            this.weightState = new System.Windows.Forms.CheckBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.packagingCount);
            this.groupBox.Controls.Add(this.quantity);
            this.groupBox.Controls.Add(this.quantityState);
            this.groupBox.Controls.Add(this.packingGroupState);
            this.groupBox.Controls.Add(this.properNameState);
            this.groupBox.Controls.Add(this.labelQuantity);
            this.groupBox.Controls.Add(this.packingGroup);
            this.groupBox.Controls.Add(this.labelPackingGroup);
            this.groupBox.Controls.Add(this.properName);
            this.groupBox.Controls.Add(this.labelProperName);
            this.groupBox.Controls.Add(this.aircraftState);
            this.groupBox.Controls.Add(this.aircarft);
            this.groupBox.Controls.Add(this.labelAircraft);
            this.groupBox.Controls.Add(this.packagingCountState);
            this.groupBox.Controls.Add(this.labelpackagingCount);
            this.groupBox.Controls.Add(this.hazardClassState);
            this.groupBox.Controls.Add(this.hazardousMaterialIdState);
            this.groupBox.Controls.Add(this.offerorState);
            this.groupBox.Controls.Add(this.emergencyContactPhoneState);
            this.groupBox.Controls.Add(this.dangerousGoodsAccessibilityState);
            this.groupBox.Controls.Add(this.dangerousGoodsMaterialTypeState);
            this.groupBox.Controls.Add(this.dangerousGoodsEnabledState);
            this.groupBox.Controls.Add(this.hazardousMaterialId);
            this.groupBox.Controls.Add(this.hazardClass);
            this.groupBox.Controls.Add(this.labelDangerousGoods);
            this.groupBox.Controls.Add(this.labelHazardClass);
            this.groupBox.Controls.Add(this.dangerousGoodsAccessibility);
            this.groupBox.Controls.Add(this.labelHazardousMaterialCode);
            this.groupBox.Controls.Add(this.containsAlcohol);
            this.groupBox.Controls.Add(this.labelAlcohol);
            this.groupBox.Controls.Add(this.dangerousGoodsEnabled);
            this.groupBox.Controls.Add(this.labelOfferor);
            this.groupBox.Controls.Add(this.alcoholState);
            this.groupBox.Controls.Add(this.labelEmergencyContactPhone);
            this.groupBox.Controls.Add(this.priorityAlertControl);
            this.groupBox.Controls.Add(this.labelAccessibility);
            this.groupBox.Controls.Add(this.priorityAlertState);
            this.groupBox.Controls.Add(this.labelDangerousGoodsMaterialType);
            this.groupBox.Controls.Add(this.dryIceState);
            this.groupBox.Controls.Add(this.offeror);
            this.groupBox.Controls.Add(this.dryIceWeight);
            this.groupBox.Controls.Add(this.dangerousGoodsMaterialType);
            this.groupBox.Controls.Add(this.labelDryIceWeight);
            this.groupBox.Controls.Add(this.emergencyContactPhone);
            this.groupBox.Controls.Add(this.kryptonBorderEdge);
            this.groupBox.Controls.Add(this.dimensionsState);
            this.groupBox.Controls.Add(this.weightState);
            this.groupBox.Controls.Add(this.weight);
            this.groupBox.Controls.Add(this.labelWeight);
            this.groupBox.Controls.Add(this.labelDimensions);
            this.groupBox.Controls.Add(this.dimensionsControl);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(396, 654);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Package {0}";
            // 
            // packagingCount
            // 
            this.packagingCount.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.packagingCount.Location = new System.Drawing.Point(159, 452);
            this.packagingCount.Name = "packagingCount";
            this.packagingCount.Size = new System.Drawing.Size(171, 21);
            this.packagingCount.TabIndex = 122;
            // 
            // quantity
            // 
            this.quantity.BackColor = System.Drawing.Color.Transparent;
            this.quantity.Location = new System.Drawing.Point(159, 613);
            this.quantity.Name = "quantity";
            this.quantity.Size = new System.Drawing.Size(193, 23);
            this.quantity.State = true;
            this.quantity.TabIndex = 121;
            // 
            // quantityState
            // 
            this.quantityState.AutoSize = true;
            this.quantityState.Checked = true;
            this.quantityState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.quantityState.Location = new System.Drawing.Point(8, 616);
            this.quantityState.Name = "quantityState";
            this.quantityState.Size = new System.Drawing.Size(15, 14);
            this.quantityState.TabIndex = 119;
            this.quantityState.Tag = "";
            this.quantityState.UseVisualStyleBackColor = true;
            // 
            // packingGroupState
            // 
            this.packingGroupState.AutoSize = true;
            this.packingGroupState.Checked = true;
            this.packingGroupState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packingGroupState.Location = new System.Drawing.Point(8, 589);
            this.packingGroupState.Name = "packingGroupState";
            this.packingGroupState.Size = new System.Drawing.Size(15, 14);
            this.packingGroupState.TabIndex = 118;
            this.packingGroupState.Tag = "";
            this.packingGroupState.UseVisualStyleBackColor = true;
            // 
            // properNameState
            // 
            this.properNameState.AutoSize = true;
            this.properNameState.Checked = true;
            this.properNameState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.properNameState.Location = new System.Drawing.Point(8, 561);
            this.properNameState.Name = "properNameState";
            this.properNameState.Size = new System.Drawing.Size(15, 14);
            this.properNameState.TabIndex = 117;
            this.properNameState.Tag = "";
            this.properNameState.UseVisualStyleBackColor = true;
            // 
            // labelQuantity
            // 
            this.labelQuantity.AutoSize = true;
            this.labelQuantity.Location = new System.Drawing.Point(100, 616);
            this.labelQuantity.Name = "labelQuantity";
            this.labelQuantity.Size = new System.Drawing.Size(53, 13);
            this.labelQuantity.TabIndex = 114;
            this.labelQuantity.Text = "Quantity:";
            this.labelQuantity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // packingGroup
            // 
            this.packingGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packingGroup.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.packingGroup.FormattingEnabled = true;
            this.packingGroup.Location = new System.Drawing.Point(159, 586);
            this.packingGroup.Name = "packingGroup";
            this.packingGroup.PromptText = "(Multiple Values)";
            this.packingGroup.Size = new System.Drawing.Size(171, 21);
            this.packingGroup.TabIndex = 113;
            // 
            // labelPackingGroup
            // 
            this.labelPackingGroup.AutoSize = true;
            this.labelPackingGroup.Location = new System.Drawing.Point(74, 589);
            this.labelPackingGroup.Name = "labelPackingGroup";
            this.labelPackingGroup.Size = new System.Drawing.Size(79, 13);
            this.labelPackingGroup.TabIndex = 112;
            this.labelPackingGroup.Text = "Packing Group:";
            this.labelPackingGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // properName
            // 
            this.properName.Location = new System.Drawing.Point(159, 558);
            this.properName.Name = "properName";
            this.properName.Size = new System.Drawing.Size(171, 21);
            this.properName.TabIndex = 111;
            // 
            // labelProperName
            // 
            this.labelProperName.AutoSize = true;
            this.labelProperName.Location = new System.Drawing.Point(80, 561);
            this.labelProperName.Name = "labelProperName";
            this.labelProperName.Size = new System.Drawing.Size(73, 13);
            this.labelProperName.TabIndex = 110;
            this.labelProperName.Text = "Proper Name:";
            this.labelProperName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // aircraftState
            // 
            this.aircraftState.AutoSize = true;
            this.aircraftState.Checked = true;
            this.aircraftState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.aircraftState.Location = new System.Drawing.Point(8, 480);
            this.aircraftState.Name = "aircraftState";
            this.aircraftState.Size = new System.Drawing.Size(15, 14);
            this.aircraftState.TabIndex = 109;
            this.aircraftState.Tag = "";
            this.aircraftState.UseVisualStyleBackColor = true;
            // 
            // aircarft
            // 
            this.aircarft.AutoSize = true;
            this.aircarft.Location = new System.Drawing.Point(159, 480);
            this.aircarft.Name = "aircarft";
            this.aircarft.Size = new System.Drawing.Size(116, 17);
            this.aircarft.TabIndex = 108;
            this.aircarft.Text = "Cargo aircraft only";
            this.aircarft.UseVisualStyleBackColor = true;
            // 
            // labelAircraft
            // 
            this.labelAircraft.AutoSize = true;
            this.labelAircraft.BackColor = System.Drawing.Color.Transparent;
            this.labelAircraft.Location = new System.Drawing.Point(106, 481);
            this.labelAircraft.Name = "labelAircraft";
            this.labelAircraft.Size = new System.Drawing.Size(47, 13);
            this.labelAircraft.TabIndex = 107;
            this.labelAircraft.Text = "Aircraft:";
            // 
            // packagingCountState
            // 
            this.packagingCountState.AutoSize = true;
            this.packagingCountState.Checked = true;
            this.packagingCountState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packagingCountState.Location = new System.Drawing.Point(8, 455);
            this.packagingCountState.Name = "packagingCountState";
            this.packagingCountState.Size = new System.Drawing.Size(15, 14);
            this.packagingCountState.TabIndex = 106;
            this.packagingCountState.Tag = "";
            this.packagingCountState.UseVisualStyleBackColor = true;
            // 
            // labelpackagingCount
            // 
            this.labelpackagingCount.AutoSize = true;
            this.labelpackagingCount.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelpackagingCount.Location = new System.Drawing.Point(62, 455);
            this.labelpackagingCount.Name = "labelpackagingCount";
            this.labelpackagingCount.Size = new System.Drawing.Size(91, 13);
            this.labelpackagingCount.TabIndex = 104;
            this.labelpackagingCount.Text = "Packaging Count:";
            this.labelpackagingCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hazardClassState
            // 
            this.hazardClassState.AutoSize = true;
            this.hazardClassState.Checked = true;
            this.hazardClassState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hazardClassState.Location = new System.Drawing.Point(8, 534);
            this.hazardClassState.Name = "hazardClassState";
            this.hazardClassState.Size = new System.Drawing.Size(15, 14);
            this.hazardClassState.TabIndex = 102;
            this.hazardClassState.Tag = "";
            this.hazardClassState.UseVisualStyleBackColor = true;
            // 
            // hazardousMaterialIdState
            // 
            this.hazardousMaterialIdState.AutoSize = true;
            this.hazardousMaterialIdState.Checked = true;
            this.hazardousMaterialIdState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hazardousMaterialIdState.Location = new System.Drawing.Point(8, 507);
            this.hazardousMaterialIdState.Name = "hazardousMaterialIdState";
            this.hazardousMaterialIdState.Size = new System.Drawing.Size(15, 14);
            this.hazardousMaterialIdState.TabIndex = 101;
            this.hazardousMaterialIdState.Tag = "";
            this.hazardousMaterialIdState.UseVisualStyleBackColor = true;
            // 
            // offerorState
            // 
            this.offerorState.AutoSize = true;
            this.offerorState.Checked = true;
            this.offerorState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.offerorState.Location = new System.Drawing.Point(8, 428);
            this.offerorState.Name = "offerorState";
            this.offerorState.Size = new System.Drawing.Size(15, 14);
            this.offerorState.TabIndex = 100;
            this.offerorState.Tag = "";
            this.offerorState.UseVisualStyleBackColor = true;
            // 
            // emergencyContactPhoneState
            // 
            this.emergencyContactPhoneState.AutoSize = true;
            this.emergencyContactPhoneState.Checked = true;
            this.emergencyContactPhoneState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.emergencyContactPhoneState.Location = new System.Drawing.Point(8, 401);
            this.emergencyContactPhoneState.Name = "emergencyContactPhoneState";
            this.emergencyContactPhoneState.Size = new System.Drawing.Size(15, 14);
            this.emergencyContactPhoneState.TabIndex = 99;
            this.emergencyContactPhoneState.Tag = "";
            this.emergencyContactPhoneState.UseVisualStyleBackColor = true;
            // 
            // dangerousGoodsAccessibilityState
            // 
            this.dangerousGoodsAccessibilityState.AutoSize = true;
            this.dangerousGoodsAccessibilityState.Checked = true;
            this.dangerousGoodsAccessibilityState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dangerousGoodsAccessibilityState.Location = new System.Drawing.Point(8, 373);
            this.dangerousGoodsAccessibilityState.Name = "dangerousGoodsAccessibilityState";
            this.dangerousGoodsAccessibilityState.Size = new System.Drawing.Size(15, 14);
            this.dangerousGoodsAccessibilityState.TabIndex = 98;
            this.dangerousGoodsAccessibilityState.Tag = "";
            this.dangerousGoodsAccessibilityState.UseVisualStyleBackColor = true;
            // 
            // dangerousGoodsMaterialTypeState
            // 
            this.dangerousGoodsMaterialTypeState.AutoSize = true;
            this.dangerousGoodsMaterialTypeState.Checked = true;
            this.dangerousGoodsMaterialTypeState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dangerousGoodsMaterialTypeState.Location = new System.Drawing.Point(8, 345);
            this.dangerousGoodsMaterialTypeState.Name = "dangerousGoodsMaterialTypeState";
            this.dangerousGoodsMaterialTypeState.Size = new System.Drawing.Size(15, 14);
            this.dangerousGoodsMaterialTypeState.TabIndex = 97;
            this.dangerousGoodsMaterialTypeState.Tag = "";
            this.dangerousGoodsMaterialTypeState.UseVisualStyleBackColor = true;
            // 
            // dangerousGoodsEnabledState
            // 
            this.dangerousGoodsEnabledState.AutoSize = true;
            this.dangerousGoodsEnabledState.Checked = true;
            this.dangerousGoodsEnabledState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dangerousGoodsEnabledState.Location = new System.Drawing.Point(8, 318);
            this.dangerousGoodsEnabledState.Name = "dangerousGoodsEnabledState";
            this.dangerousGoodsEnabledState.Size = new System.Drawing.Size(15, 14);
            this.dangerousGoodsEnabledState.TabIndex = 96;
            this.dangerousGoodsEnabledState.Tag = "";
            this.dangerousGoodsEnabledState.UseVisualStyleBackColor = true;
            // 
            // hazardousMaterialId
            // 
            this.hazardousMaterialId.Location = new System.Drawing.Point(159, 504);
            this.hazardousMaterialId.Name = "hazardousMaterialId";
            this.hazardousMaterialId.Size = new System.Drawing.Size(171, 21);
            this.hazardousMaterialId.TabIndex = 22;
            // 
            // hazardClass
            // 
            this.hazardClass.Location = new System.Drawing.Point(159, 531);
            this.hazardClass.Name = "hazardClass";
            this.hazardClass.Size = new System.Drawing.Size(171, 21);
            this.hazardClass.TabIndex = 23;
            // 
            // labelDangerousGoods
            // 
            this.labelDangerousGoods.AutoSize = true;
            this.labelDangerousGoods.BackColor = System.Drawing.Color.Transparent;
            this.labelDangerousGoods.Location = new System.Drawing.Point(57, 318);
            this.labelDangerousGoods.Name = "labelDangerousGoods";
            this.labelDangerousGoods.Size = new System.Drawing.Size(96, 13);
            this.labelDangerousGoods.TabIndex = 95;
            this.labelDangerousGoods.Text = "Dangerous Goods:";
            // 
            // labelHazardClass
            // 
            this.labelHazardClass.AutoSize = true;
            this.labelHazardClass.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelHazardClass.Location = new System.Drawing.Point(80, 534);
            this.labelHazardClass.Name = "labelHazardClass";
            this.labelHazardClass.Size = new System.Drawing.Size(73, 13);
            this.labelHazardClass.TabIndex = 1;
            this.labelHazardClass.Text = "Hazard Class:";
            // 
            // dangerousGoodsAccessibility
            // 
            this.dangerousGoodsAccessibility.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsAccessibility.FormattingEnabled = true;
            this.dangerousGoodsAccessibility.Location = new System.Drawing.Point(159, 370);
            this.dangerousGoodsAccessibility.Name = "dangerousGoodsAccessibility";
            this.dangerousGoodsAccessibility.Size = new System.Drawing.Size(171, 21);
            this.dangerousGoodsAccessibility.TabIndex = 28;
            this.dangerousGoodsAccessibility.Tag = "97";
            // 
            // labelHazardousMaterialCode
            // 
            this.labelHazardousMaterialCode.AutoSize = true;
            this.labelHazardousMaterialCode.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelHazardousMaterialCode.Location = new System.Drawing.Point(36, 507);
            this.labelHazardousMaterialCode.Name = "labelHazardousMaterialCode";
            this.labelHazardousMaterialCode.Size = new System.Drawing.Size(117, 13);
            this.labelHazardousMaterialCode.TabIndex = 0;
            this.labelHazardousMaterialCode.Text = "Hazardous Material ID:";
            // 
            // containsAlcohol
            // 
            this.containsAlcohol.AutoSize = true;
            this.containsAlcohol.Location = new System.Drawing.Point(159, 288);
            this.containsAlcohol.Name = "containsAlcohol";
            this.containsAlcohol.Size = new System.Drawing.Size(167, 17);
            this.containsAlcohol.TabIndex = 94;
            this.containsAlcohol.Text = "This package contains alcohol";
            this.containsAlcohol.UseVisualStyleBackColor = true;
            // 
            // labelAlcohol
            // 
            this.labelAlcohol.AutoSize = true;
            this.labelAlcohol.BackColor = System.Drawing.Color.Transparent;
            this.labelAlcohol.Location = new System.Drawing.Point(108, 289);
            this.labelAlcohol.Name = "labelAlcohol";
            this.labelAlcohol.Size = new System.Drawing.Size(45, 13);
            this.labelAlcohol.TabIndex = 93;
            this.labelAlcohol.Text = "Alcohol:";
            // 
            // dangerousGoodsEnabled
            // 
            this.dangerousGoodsEnabled.AutoSize = true;
            this.dangerousGoodsEnabled.Location = new System.Drawing.Point(159, 317);
            this.dangerousGoodsEnabled.Name = "dangerousGoodsEnabled";
            this.dangerousGoodsEnabled.Size = new System.Drawing.Size(217, 17);
            this.dangerousGoodsEnabled.TabIndex = 95;
            this.dangerousGoodsEnabled.Text = "This package contains dangerous goods";
            this.dangerousGoodsEnabled.UseVisualStyleBackColor = true;
            // 
            // labelOfferor
            // 
            this.labelOfferor.AutoSize = true;
            this.labelOfferor.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelOfferor.Location = new System.Drawing.Point(106, 428);
            this.labelOfferor.Name = "labelOfferor";
            this.labelOfferor.Size = new System.Drawing.Size(47, 13);
            this.labelOfferor.TabIndex = 22;
            this.labelOfferor.Text = "Offeror:";
            this.labelOfferor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // alcoholState
            // 
            this.alcoholState.AutoSize = true;
            this.alcoholState.Checked = true;
            this.alcoholState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.alcoholState.Location = new System.Drawing.Point(8, 288);
            this.alcoholState.Name = "alcoholState";
            this.alcoholState.Size = new System.Drawing.Size(15, 14);
            this.alcoholState.TabIndex = 92;
            this.alcoholState.Tag = "";
            this.alcoholState.UseVisualStyleBackColor = true;
            // 
            // labelEmergencyContactPhone
            // 
            this.labelEmergencyContactPhone.AutoSize = true;
            this.labelEmergencyContactPhone.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelEmergencyContactPhone.Location = new System.Drawing.Point(56, 401);
            this.labelEmergencyContactPhone.Name = "labelEmergencyContactPhone";
            this.labelEmergencyContactPhone.Size = new System.Drawing.Size(97, 13);
            this.labelEmergencyContactPhone.TabIndex = 23;
            this.labelEmergencyContactPhone.Text = "Emergency Phone:";
            this.labelEmergencyContactPhone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // priorityAlertControl
            // 
            this.priorityAlertControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.priorityAlertControl.Location = new System.Drawing.Point(75, 154);
            this.priorityAlertControl.Name = "priorityAlertControl";
            this.priorityAlertControl.Size = new System.Drawing.Size(309, 138);
            this.priorityAlertControl.State = true;
            this.priorityAlertControl.TabIndex = 91;
            // 
            // labelAccessibility
            // 
            this.labelAccessibility.AutoSize = true;
            this.labelAccessibility.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelAccessibility.Location = new System.Drawing.Point(58, 373);
            this.labelAccessibility.Name = "labelAccessibility";
            this.labelAccessibility.Size = new System.Drawing.Size(95, 13);
            this.labelAccessibility.TabIndex = 24;
            this.labelAccessibility.Text = "Accessibility Type:";
            this.labelAccessibility.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // priorityAlertState
            // 
            this.priorityAlertState.AutoSize = true;
            this.priorityAlertState.Checked = true;
            this.priorityAlertState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.priorityAlertState.Location = new System.Drawing.Point(8, 154);
            this.priorityAlertState.Name = "priorityAlertState";
            this.priorityAlertState.Size = new System.Drawing.Size(15, 14);
            this.priorityAlertState.TabIndex = 90;
            this.priorityAlertState.Tag = "";
            this.priorityAlertState.UseVisualStyleBackColor = true;
            // 
            // labelDangerousGoodsMaterialType
            // 
            this.labelDangerousGoodsMaterialType.AutoSize = true;
            this.labelDangerousGoodsMaterialType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelDangerousGoodsMaterialType.Location = new System.Drawing.Point(77, 345);
            this.labelDangerousGoodsMaterialType.Name = "labelDangerousGoodsMaterialType";
            this.labelDangerousGoodsMaterialType.Size = new System.Drawing.Size(76, 13);
            this.labelDangerousGoodsMaterialType.TabIndex = 25;
            this.labelDangerousGoodsMaterialType.Text = "Material Type:";
            this.labelDangerousGoodsMaterialType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dryIceState
            // 
            this.dryIceState.AutoSize = true;
            this.dryIceState.Checked = true;
            this.dryIceState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dryIceState.Location = new System.Drawing.Point(8, 130);
            this.dryIceState.Name = "dryIceState";
            this.dryIceState.Size = new System.Drawing.Size(15, 14);
            this.dryIceState.TabIndex = 88;
            this.dryIceState.Tag = "";
            this.dryIceState.UseVisualStyleBackColor = true;
            // 
            // offeror
            // 
            this.offeror.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.offeror.Location = new System.Drawing.Point(159, 425);
            this.offeror.Name = "offeror";
            this.offeror.Size = new System.Drawing.Size(171, 21);
            this.offeror.TabIndex = 21;
            // 
            // dryIceWeight
            // 
            this.dryIceWeight.BackColor = System.Drawing.Color.Transparent;
            this.dryIceWeight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dryIceWeight.Location = new System.Drawing.Point(159, 127);
            this.dryIceWeight.Name = "dryIceWeight";
            this.dryIceWeight.RangeMax = 2000D;
            this.dryIceWeight.RangeMin = 0D;
            this.dryIceWeight.Size = new System.Drawing.Size(218, 21);
            this.dryIceWeight.TabIndex = 87;
            this.dryIceWeight.Weight = 0D;
            // 
            // dangerousGoodsMaterialType
            // 
            this.dangerousGoodsMaterialType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dangerousGoodsMaterialType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.dangerousGoodsMaterialType.FormattingEnabled = true;
            this.dangerousGoodsMaterialType.Location = new System.Drawing.Point(159, 342);
            this.dangerousGoodsMaterialType.Name = "dangerousGoodsMaterialType";
            this.dangerousGoodsMaterialType.Size = new System.Drawing.Size(171, 21);
            this.dangerousGoodsMaterialType.TabIndex = 20;
            this.dangerousGoodsMaterialType.Tag = "96";
            // 
            // labelDryIceWeight
            // 
            this.labelDryIceWeight.AutoSize = true;
            this.labelDryIceWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelDryIceWeight.Location = new System.Drawing.Point(70, 131);
            this.labelDryIceWeight.Name = "labelDryIceWeight";
            this.labelDryIceWeight.Size = new System.Drawing.Size(83, 13);
            this.labelDryIceWeight.TabIndex = 86;
            this.labelDryIceWeight.Text = "Dry Ice Weight:";
            // 
            // emergencyContactPhone
            // 
            this.emergencyContactPhone.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.emergencyContactPhone.Location = new System.Drawing.Point(159, 398);
            this.emergencyContactPhone.Name = "emergencyContactPhone";
            this.emergencyContactPhone.Size = new System.Drawing.Size(171, 21);
            this.emergencyContactPhone.TabIndex = 19;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(31, 20);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 617);
            this.kryptonBorderEdge.TabIndex = 83;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // dimensionsState
            // 
            this.dimensionsState.AutoSize = true;
            this.dimensionsState.Checked = true;
            this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsState.Location = new System.Drawing.Point(8, 55);
            this.dimensionsState.Name = "dimensionsState";
            this.dimensionsState.Size = new System.Drawing.Size(15, 14);
            this.dimensionsState.TabIndex = 2;
            this.dimensionsState.Tag = "";
            this.dimensionsState.UseVisualStyleBackColor = true;
            // 
            // weightState
            // 
            this.weightState.AutoSize = true;
            this.weightState.Checked = true;
            this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.weightState.Location = new System.Drawing.Point(8, 26);
            this.weightState.Name = "weightState";
            this.weightState.Size = new System.Drawing.Size(15, 14);
            this.weightState.TabIndex = 0;
            this.weightState.Tag = "";
            this.weightState.UseVisualStyleBackColor = true;
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(159, 23);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 21);
            this.weight.TabIndex = 1;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(108, 26);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 75;
            this.labelWeight.Text = "Weight:";
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(89, 55);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 77;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(156, 49);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 3;
            // 
            // FedExProfilePackageControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FedExProfilePackageControl";
            this.Size = new System.Drawing.Size(396, 654);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelDimensions;
        private ShipWorks.Shipping.Editing.DimensionsControl dimensionsControl;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.CheckBox dimensionsState;
        private System.Windows.Forms.CheckBox weightState;
        private UI.Controls.WeightControl dryIceWeight;
        private System.Windows.Forms.Label labelDryIceWeight;
        private System.Windows.Forms.CheckBox dryIceState;
        private System.Windows.Forms.CheckBox priorityAlertState;
        private FedExProfilePackagePriorityAlertsControl priorityAlertControl;
        private System.Windows.Forms.CheckBox alcoholState;
        private System.Windows.Forms.CheckBox containsAlcohol;
        private System.Windows.Forms.Label labelAlcohol;
        private System.Windows.Forms.CheckBox hazardClassState;
        private System.Windows.Forms.CheckBox hazardousMaterialIdState;
        private System.Windows.Forms.CheckBox offerorState;
        private System.Windows.Forms.CheckBox emergencyContactPhoneState;
        private System.Windows.Forms.CheckBox dangerousGoodsAccessibilityState;
        private System.Windows.Forms.CheckBox dangerousGoodsMaterialTypeState;
        private System.Windows.Forms.CheckBox dangerousGoodsEnabledState;
        private System.Windows.Forms.TextBox hazardousMaterialId;
        private System.Windows.Forms.TextBox hazardClass;
        private System.Windows.Forms.Label labelDangerousGoods;
        private System.Windows.Forms.Label labelHazardClass;
        private System.Windows.Forms.ComboBox dangerousGoodsAccessibility;
        private System.Windows.Forms.Label labelHazardousMaterialCode;
        private System.Windows.Forms.CheckBox dangerousGoodsEnabled;
        private System.Windows.Forms.Label labelOfferor;
        private System.Windows.Forms.Label labelEmergencyContactPhone;
        private System.Windows.Forms.Label labelAccessibility;
        private System.Windows.Forms.Label labelDangerousGoodsMaterialType;
        private System.Windows.Forms.TextBox offeror;
        private System.Windows.Forms.ComboBox dangerousGoodsMaterialType;
        private System.Windows.Forms.TextBox emergencyContactPhone;
        private System.Windows.Forms.Label labelpackagingCount;
        private System.Windows.Forms.CheckBox aircraftState;
        private System.Windows.Forms.CheckBox aircarft;
        private System.Windows.Forms.Label labelAircraft;
        private System.Windows.Forms.CheckBox packagingCountState;
        private System.Windows.Forms.Label labelQuantity;
        private UI.Controls.MultiValueComboBox packingGroup;
        private System.Windows.Forms.Label labelPackingGroup;
        private UI.Controls.MultiValueTextBox properName;
        private System.Windows.Forms.Label labelProperName;
        private System.Windows.Forms.CheckBox quantityState;
        private System.Windows.Forms.CheckBox packingGroupState;
        private System.Windows.Forms.CheckBox properNameState;
        private FedExProfilePackageHazardousQuantityControl quantity;
        private NumericTextBox packagingCount;
    }
}
