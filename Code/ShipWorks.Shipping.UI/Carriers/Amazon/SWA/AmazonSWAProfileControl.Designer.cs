﻿namespace ShipWorks.Shipping.UI.Carriers.Amazon.SWA
{
    partial class AmazonSWAProfileControl
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
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupInsurance = new System.Windows.Forms.GroupBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
            this.insuranceState = new System.Windows.Forms.CheckBox();
            this.insuranceBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.labelThermalNote = new System.Windows.Forms.Label();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.labelsBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.dimensionsState = new System.Windows.Forms.CheckBox();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.weightState = new System.Windows.Forms.CheckBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.shipmentBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.labelAccount = new System.Windows.Forms.Label();
            this.accountState = new System.Windows.Forms.CheckBox();
            this.AmazonSWAAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.fromBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.groupBoxFrom.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.AutoScroll = true;
            this.tabPageSettings.Controls.Add(this.groupInsurance);
            this.tabPageSettings.Controls.Add(this.groupLabels);
            this.tabPageSettings.Controls.Add(this.groupShipment);
            this.tabPageSettings.Controls.Add(this.groupBoxFrom);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(417, 1101);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // groupInsurance
            // 
            this.groupInsurance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupInsurance.Controls.Add(this.insuranceControl);
            this.groupInsurance.Controls.Add(this.insuranceState);
            this.groupInsurance.Controls.Add(this.insuranceBorderEdge);
            this.groupInsurance.Location = new System.Drawing.Point(6, 305);
            this.groupInsurance.Name = "groupInsurance";
            this.groupInsurance.Size = new System.Drawing.Size(405, 70);
            this.groupInsurance.TabIndex = 3;
            this.groupInsurance.TabStop = false;
            this.groupInsurance.Text = "Insurance";
            // 
            // insuranceControl
            // 
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(49, 14);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(307, 50);
            this.insuranceControl.TabIndex = 1;
            // 
            // insuranceState
            // 
            this.insuranceState.AutoSize = true;
            this.insuranceState.Location = new System.Drawing.Point(9, 18);
            this.insuranceState.Name = "insuranceState";
            this.insuranceState.Size = new System.Drawing.Size(15, 14);
            this.insuranceState.TabIndex = 0;
            this.insuranceState.UseVisualStyleBackColor = true;
            // 
            // insuranceBorderEdge
            // 
            this.insuranceBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.insuranceBorderEdge.AutoSize = false;
            this.insuranceBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.insuranceBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.insuranceBorderEdge.Name = "insuranceBorderEdge";
            this.insuranceBorderEdge.Size = new System.Drawing.Size(1, 40);
            this.insuranceBorderEdge.Text = "insuranceBorderEdge";
            // 
            // groupLabels
            // 
            this.groupLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabels.Controls.Add(this.labelThermalNote);
            this.groupLabels.Controls.Add(this.requestedLabelFormat);
            this.groupLabels.Controls.Add(this.requestedLabelFormatState);
            this.groupLabels.Controls.Add(this.labelsBorderEdge);
            this.groupLabels.Location = new System.Drawing.Point(6, 231);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Size = new System.Drawing.Size(405, 67);
            this.groupLabels.TabIndex = 2;
            this.groupLabels.TabStop = false;
            this.groupLabels.Text = "Labels";
            // 
            // labelThermalNote
            // 
            this.labelThermalNote.AutoSize = true;
            this.labelThermalNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelThermalNote.Location = new System.Drawing.Point(35, 44);
            this.labelThermalNote.Name = "labelThermalNote";
            this.labelThermalNote.Size = new System.Drawing.Size(236, 13);
            this.labelThermalNote.TabIndex = 103;
            this.labelThermalNote.Text = "Note: Amazon only supports ZPL thermal labels.";
            this.labelThermalNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(35, 18);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(267, 21);
            this.requestedLabelFormat.State = false;
            this.requestedLabelFormat.TabIndex = 1;
            // 
            // requestedLabelFormatState
            // 
            this.requestedLabelFormatState.AutoSize = true;
            this.requestedLabelFormatState.Location = new System.Drawing.Point(9, 21);
            this.requestedLabelFormatState.Name = "requestedLabelFormatState";
            this.requestedLabelFormatState.Size = new System.Drawing.Size(15, 14);
            this.requestedLabelFormatState.TabIndex = 0;
            this.requestedLabelFormatState.UseVisualStyleBackColor = true;
            // 
            // labelsBorderEdge
            // 
            this.labelsBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelsBorderEdge.AutoSize = false;
            this.labelsBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.labelsBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.labelsBorderEdge.Name = "labelsBorderEdge";
            this.labelsBorderEdge.Size = new System.Drawing.Size(1, 37);
            this.labelsBorderEdge.Text = "labelsBorderEdge";
            // 
            // groupShipment
            // 
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.dimensionsState);
            this.groupShipment.Controls.Add(this.dimensionsControl);
            this.groupShipment.Controls.Add(this.labelDimensions);
            this.groupShipment.Controls.Add(this.weightState);
            this.groupShipment.Controls.Add(this.weight);
            this.groupShipment.Controls.Add(this.labelWeight);
            this.groupShipment.Controls.Add(this.shipmentBorderEdge);
            this.groupShipment.Controls.Add(this.labelService);
            this.groupShipment.Controls.Add(this.service);
            this.groupShipment.Controls.Add(this.serviceState);
            this.groupShipment.Location = new System.Drawing.Point(6, 64);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(405, 160);
            this.groupShipment.TabIndex = 1;
            this.groupShipment.TabStop = false;
            this.groupShipment.Text = "Shipment";
            // 
            // dimensionsState
            // 
            this.dimensionsState.AutoSize = true;
            this.dimensionsState.Checked = true;
            this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsState.Location = new System.Drawing.Point(9, 86);
            this.dimensionsState.Name = "dimensionsState";
            this.dimensionsState.Size = new System.Drawing.Size(15, 14);
            this.dimensionsState.TabIndex = 4;
            this.dimensionsState.Tag = "";
            this.dimensionsState.UseVisualStyleBackColor = true;
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(125, 77);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 5;
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(58, 86);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 77;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // weightState
            // 
            this.weightState.AutoSize = true;
            this.weightState.Checked = true;
            this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.weightState.Location = new System.Drawing.Point(9, 53);
            this.weightState.Name = "weightState";
            this.weightState.Size = new System.Drawing.Size(15, 14);
            this.weightState.TabIndex = 2;
            this.weightState.Tag = "";
            this.weightState.UseVisualStyleBackColor = true;
            // 
            // weight
            // 
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.ConfigureTelemetryEntityCounts = null;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(128, 49);
            this.weight.Name = "weight";
            this.weight.RangeMax = 400D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(269, 24);
            this.weight.TabIndex = 3;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(77, 53);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 74;
            this.labelWeight.Text = "Weight:";
            // 
            // shipmentBorderEdge
            // 
            this.shipmentBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.shipmentBorderEdge.AutoSize = false;
            this.shipmentBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.shipmentBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.shipmentBorderEdge.Name = "shipmentBorderEdge";
            this.shipmentBorderEdge.Size = new System.Drawing.Size(1, 130);
            this.shipmentBorderEdge.Text = "shipmentBorderEdge";
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(76, 21);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 52;
            this.labelService.Text = "Service:";
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(128, 18);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(206, 21);
            this.service.TabIndex = 1;
            // 
            // serviceState
            // 
            this.serviceState.AutoSize = true;
            this.serviceState.Checked = true;
            this.serviceState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.serviceState.Location = new System.Drawing.Point(9, 21);
            this.serviceState.Name = "serviceState";
            this.serviceState.Size = new System.Drawing.Size(15, 14);
            this.serviceState.TabIndex = 0;
            this.serviceState.Tag = "";
            this.serviceState.UseVisualStyleBackColor = true;
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFrom.Controls.Add(this.labelAccount);
            this.groupBoxFrom.Controls.Add(this.accountState);
            this.groupBoxFrom.Controls.Add(this.AmazonSWAAccount);
            this.groupBoxFrom.Controls.Add(this.fromBorderEdge);
            this.groupBoxFrom.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(405, 51);
            this.groupBoxFrom.TabIndex = 0;
            this.groupBoxFrom.TabStop = false;
            this.groupBoxFrom.Text = "From";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(72, 21);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 16;
            this.labelAccount.Text = "Account:";
            // 
            // accountState
            // 
            this.accountState.AutoSize = true;
            this.accountState.Checked = true;
            this.accountState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.accountState.Location = new System.Drawing.Point(9, 21);
            this.accountState.Name = "accountState";
            this.accountState.Size = new System.Drawing.Size(15, 14);
            this.accountState.TabIndex = 0;
            this.accountState.Tag = "";
            this.accountState.UseVisualStyleBackColor = true;
            // 
            // AmazonSWAAccount
            // 
            this.AmazonSWAAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AmazonSWAAccount.FormattingEnabled = true;
            this.AmazonSWAAccount.Location = new System.Drawing.Point(128, 18);
            this.AmazonSWAAccount.Name = "AmazonSWAAccount";
            this.AmazonSWAAccount.PromptText = "(Multiple Values)";
            this.AmazonSWAAccount.Size = new System.Drawing.Size(206, 21);
            this.AmazonSWAAccount.TabIndex = 1;
            // 
            // fromBorderEdge
            // 
            this.fromBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.fromBorderEdge.AutoSize = false;
            this.fromBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.fromBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.fromBorderEdge.Name = "fromBorderEdge";
            this.fromBorderEdge.Size = new System.Drawing.Size(1, 21);
            this.fromBorderEdge.Text = "fromBorderEdge";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(425, 1127);
            this.tabControl.TabIndex = 0;
            // 
            // AmazonSWAProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "AmazonSWAProfileControl";
            this.Size = new System.Drawing.Size(425, 1127);
            this.tabPageSettings.ResumeLayout(false);
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupInsurance;
        private ShipWorks.Shipping.Insurance.InsuranceProfileControl insuranceControl;
        private System.Windows.Forms.CheckBox insuranceState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge insuranceBorderEdge;
        protected System.Windows.Forms.GroupBox groupLabels;
        private System.Windows.Forms.Label labelThermalNote;
        protected Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge labelsBorderEdge;
        private System.Windows.Forms.GroupBox groupShipment;
        private System.Windows.Forms.CheckBox dimensionsState;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label labelDimensions;
        private System.Windows.Forms.CheckBox weightState;
        private ShipWorks.UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge shipmentBorderEdge;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.CheckBox serviceState;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private System.Windows.Forms.Label labelAccount;
        private System.Windows.Forms.CheckBox accountState;
        private ShipWorks.UI.Controls.MultiValueComboBox AmazonSWAAccount;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge fromBorderEdge;
        private System.Windows.Forms.TabControl tabControl;
    }
}
