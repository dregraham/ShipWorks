using System.Windows.Forms;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    partial class DhlEcommerceProfileControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupInsurance = new System.Windows.Forms.GroupBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
            this.insuranceState = new System.Windows.Forms.CheckBox();
            this.dividerInsurance = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupCustoms = new System.Windows.Forms.GroupBox();
            this.contentsState = new System.Windows.Forms.CheckBox();
            this.nonDeliveryState = new System.Windows.Forms.CheckBox();
            this.labelNonDelivery = new System.Windows.Forms.Label();
            this.nonDelivery = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelContents = new System.Windows.Forms.Label();
            this.dividerCustoms = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.contents = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.customsRecipientTinState = new System.Windows.Forms.CheckBox();
            this.labelCustomsRecipientTin = new System.Windows.Forms.Label();
            this.customsRecipientTin = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.taxIdTypeState = new System.Windows.Forms.CheckBox();
            this.labelTaxIdType = new System.Windows.Forms.Label();
            this.taxIdType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.customsTinIssuingAuthorityState = new System.Windows.Forms.CheckBox();
            this.labelCustomsTinIssuingAuthority = new System.Windows.Forms.Label();
            this.customsTinIssuingAuthority = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.labelThermalNote = new System.Windows.Forms.Label();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.dividerLabels = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupOptions = new System.Windows.Forms.GroupBox();
            this.labelReference1 = new System.Windows.Forms.Label();
            this.reference1State = new System.Windows.Forms.CheckBox();
            this.reference1 = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.resDeliveryState = new System.Windows.Forms.CheckBox();
            this.resDelivery = new System.Windows.Forms.CheckBox();
            this.labelResDelivery = new System.Windows.Forms.Label();
            this.nonMachinable = new System.Windows.Forms.CheckBox();
            this.nonMachinableState = new System.Windows.Forms.CheckBox();
            this.labelDuty = new System.Windows.Forms.Label();
            this.labelNonMachinable = new System.Windows.Forms.Label();
            this.labelSaturday = new System.Windows.Forms.Label();
            this.dutyDeliveryPaid = new System.Windows.Forms.CheckBox();
            this.dutyDeliveryPaidState = new System.Windows.Forms.CheckBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.saturdayState = new System.Windows.Forms.CheckBox();
            this.dividerOptions = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.dimensions = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.dimensionsState = new System.Windows.Forms.CheckBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.weightState = new System.Windows.Forms.CheckBox();
            this.labelPackageType = new System.Windows.Forms.Label();
            this.packageTypeState = new System.Windows.Forms.CheckBox();
            this.packageType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.dividerShipment = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.groupFrom = new System.Windows.Forms.GroupBox();
            this.labelAccount = new System.Windows.Forms.Label();
            this.accountState = new System.Windows.Forms.CheckBox();
            this.dhlEcommerceAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.dividerFrom = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.tabControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupCustoms.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.groupOptions.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.groupFrom.SuspendLayout();
            this.SuspendLayout();
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
            // tabPageSettings
            // 
            this.tabPageSettings.AutoScroll = true;
            this.tabPageSettings.Controls.Add(this.groupInsurance);
            this.tabPageSettings.Controls.Add(this.groupCustoms);
            this.tabPageSettings.Controls.Add(this.groupLabels);
            this.tabPageSettings.Controls.Add(this.groupOptions);
            this.tabPageSettings.Controls.Add(this.groupShipment);
            this.tabPageSettings.Controls.Add(this.groupFrom);
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
            this.groupInsurance.Controls.Add(this.dividerInsurance);
            this.groupInsurance.Location = new System.Drawing.Point(6, 334);
            this.groupInsurance.Name = "groupInsurance";
            this.groupInsurance.Size = new System.Drawing.Size(405, 76);
            this.groupInsurance.TabIndex = 3;
            this.groupInsurance.TabStop = false;
            this.groupInsurance.Text = "Insurance";
            // 
            // insuranceControl
            // 
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(50, 19);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(316, 52);
            this.insuranceControl.TabIndex = 1;
            // 
            // insuranceState
            // 
            this.insuranceState.AutoSize = true;
            this.insuranceState.Location = new System.Drawing.Point(9, 23);
            this.insuranceState.Name = "insuranceState";
            this.insuranceState.Size = new System.Drawing.Size(15, 14);
            this.insuranceState.TabIndex = 0;
            this.insuranceState.UseVisualStyleBackColor = true;
            // 
            // dividerInsurance
            // 
            this.dividerInsurance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dividerInsurance.AutoSize = false;
            this.dividerInsurance.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.dividerInsurance.Location = new System.Drawing.Point(29, 17);
            this.dividerInsurance.Name = "kryptonBorderEdge10";
            this.dividerInsurance.Size = new System.Drawing.Size(1, 51);
            this.dividerInsurance.Text = "kryptonBorderEdge1";
            // 
            // groupCustoms
            // 
            this.groupCustoms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupCustoms.Controls.Add(this.contentsState);
            this.groupCustoms.Controls.Add(this.nonDeliveryState);
            this.groupCustoms.Controls.Add(this.labelNonDelivery);
            this.groupCustoms.Controls.Add(this.nonDelivery);
            this.groupCustoms.Controls.Add(this.labelContents);
            this.groupCustoms.Controls.Add(this.dividerCustoms);
            this.groupCustoms.Controls.Add(this.contents);
            this.groupCustoms.Controls.Add(this.customsRecipientTinState);
            this.groupCustoms.Controls.Add(this.labelCustomsRecipientTin);
            this.groupCustoms.Controls.Add(this.customsRecipientTin);
            this.groupCustoms.Controls.Add(this.taxIdTypeState);
            this.groupCustoms.Controls.Add(this.labelTaxIdType);
            this.groupCustoms.Controls.Add(this.taxIdType);
            this.groupCustoms.Controls.Add(this.customsTinIssuingAuthorityState);
            this.groupCustoms.Controls.Add(this.labelCustomsTinIssuingAuthority);
            this.groupCustoms.Controls.Add(this.customsTinIssuingAuthority);
            this.groupCustoms.Location = new System.Drawing.Point(6, 587);
            this.groupCustoms.Name = "groupCustoms";
            this.groupCustoms.Size = new System.Drawing.Size(405, 165);
            this.groupCustoms.TabIndex = 5;
            this.groupCustoms.TabStop = false;
            this.groupCustoms.Text = "Customs";
            // 
            // contentsState
            // 
            this.contentsState.AutoSize = true;
            this.contentsState.Checked = true;
            this.contentsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.contentsState.Location = new System.Drawing.Point(9, 23);
            this.contentsState.Name = "contentsState";
            this.contentsState.Size = new System.Drawing.Size(15, 14);
            this.contentsState.TabIndex = 0;
            this.contentsState.Tag = "";
            this.contentsState.UseVisualStyleBackColor = true;
            // 
            // nonDeliveryState
            // 
            this.nonDeliveryState.AutoSize = true;
            this.nonDeliveryState.Checked = true;
            this.nonDeliveryState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nonDeliveryState.Location = new System.Drawing.Point(9, 51);
            this.nonDeliveryState.Name = "nonDeliveryState";
            this.nonDeliveryState.Size = new System.Drawing.Size(15, 14);
            this.nonDeliveryState.TabIndex = 2;
            this.nonDeliveryState.Tag = "";
            this.nonDeliveryState.UseVisualStyleBackColor = true;
            // 
            // labelNonDelivery
            // 
            this.labelNonDelivery.AutoSize = true;
            this.labelNonDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelNonDelivery.Location = new System.Drawing.Point(68, 51);
            this.labelNonDelivery.Name = "labelNonDelivery";
            this.labelNonDelivery.Size = new System.Drawing.Size(72, 13);
            this.labelNonDelivery.TabIndex = 106;
            this.labelNonDelivery.Text = "Non Delivery:";
            // 
            // nonDelivery
            // 
            this.nonDelivery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nonDelivery.FormattingEnabled = true;
            this.nonDelivery.Location = new System.Drawing.Point(146, 48);
            this.nonDelivery.Name = "nonDelivery";
            this.nonDelivery.PromptText = "(Multiple Values)";
            this.nonDelivery.Size = new System.Drawing.Size(206, 21);
            this.nonDelivery.TabIndex = 3;
            // 
            // labelContents
            // 
            this.labelContents.AutoSize = true;
            this.labelContents.BackColor = System.Drawing.Color.Transparent;
            this.labelContents.Location = new System.Drawing.Point(85, 23);
            this.labelContents.Name = "labelContents";
            this.labelContents.Size = new System.Drawing.Size(55, 13);
            this.labelContents.TabIndex = 89;
            this.labelContents.Text = "Contents:";
            // 
            // dividerCustoms
            // 
            this.dividerCustoms.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dividerCustoms.AutoSize = false;
            this.dividerCustoms.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.dividerCustoms.Location = new System.Drawing.Point(29, 17);
            this.dividerCustoms.Name = "kryptonBorderEdge11";
            this.dividerCustoms.Size = new System.Drawing.Size(1, 140);
            this.dividerCustoms.Text = "kryptonBorderEdge11";
            // 
            // contents
            // 
            this.contents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contents.FormattingEnabled = true;
            this.contents.Location = new System.Drawing.Point(146, 20);
            this.contents.Name = "contents";
            this.contents.PromptText = "(Multiple Values)";
            this.contents.Size = new System.Drawing.Size(206, 21);
            this.contents.TabIndex = 1;
            // 
            // customsRecipientTinState
            // 
            this.customsRecipientTinState.AutoSize = true;
            this.customsRecipientTinState.Checked = true;
            this.customsRecipientTinState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.customsRecipientTinState.Location = new System.Drawing.Point(9, 79);
            this.customsRecipientTinState.Name = "customsRecipientTinState";
            this.customsRecipientTinState.Size = new System.Drawing.Size(15, 14);
            this.customsRecipientTinState.TabIndex = 4;
            this.customsRecipientTinState.Tag = "";
            this.customsRecipientTinState.UseVisualStyleBackColor = true;
            // 
            // labelCustomsRecipientTin
            // 
            this.labelCustomsRecipientTin.AutoSize = true;
            this.labelCustomsRecipientTin.BackColor = System.Drawing.Color.Transparent;
            this.labelCustomsRecipientTin.Location = new System.Drawing.Point(50, 79);
            this.labelCustomsRecipientTin.Name = "labelCustomsRecipientTin";
            this.labelCustomsRecipientTin.Size = new System.Drawing.Size(90, 13);
            this.labelCustomsRecipientTin.TabIndex = 110;
            this.labelCustomsRecipientTin.Text = "Recipient Tax ID:";
            // 
            // customsRecipientTin
            // 
            this.customsRecipientTin.Location = new System.Drawing.Point(146, 76);
            this.customsRecipientTin.Name = "customsRecipientTin";
            this.customsRecipientTin.Size = new System.Drawing.Size(206, 21);
            this.customsRecipientTin.TabIndex = 5;
            // 
            // taxIdTypeState
            // 
            this.taxIdTypeState.AutoSize = true;
            this.taxIdTypeState.Checked = true;
            this.taxIdTypeState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.taxIdTypeState.Location = new System.Drawing.Point(9, 107);
            this.taxIdTypeState.Name = "taxIdTypeState";
            this.taxIdTypeState.Size = new System.Drawing.Size(15, 14);
            this.taxIdTypeState.TabIndex = 6;
            this.taxIdTypeState.Tag = "";
            this.taxIdTypeState.UseVisualStyleBackColor = true;
            // 
            // labelTaxIdType
            // 
            this.labelTaxIdType.AutoSize = true;
            this.labelTaxIdType.BackColor = System.Drawing.Color.Transparent;
            this.labelTaxIdType.Location = new System.Drawing.Point(85, 107);
            this.labelTaxIdType.Name = "labelTaxIdType";
            this.labelTaxIdType.Size = new System.Drawing.Size(55, 13);
            this.labelTaxIdType.TabIndex = 111;
            this.labelTaxIdType.Text = "TIN Type:";
            // 
            // taxIdType
            // 
            this.taxIdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.taxIdType.FormattingEnabled = true;
            this.taxIdType.Location = new System.Drawing.Point(146, 104);
            this.taxIdType.Name = "taxIdType";
            this.taxIdType.PromptText = "(Multiple Values)";
            this.taxIdType.Size = new System.Drawing.Size(206, 21);
            this.taxIdType.TabIndex = 7;
            // 
            // customsTinIssuingAuthorityState
            // 
            this.customsTinIssuingAuthorityState.AutoSize = true;
            this.customsTinIssuingAuthorityState.Checked = true;
            this.customsTinIssuingAuthorityState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.customsTinIssuingAuthorityState.Location = new System.Drawing.Point(9, 135);
            this.customsTinIssuingAuthorityState.Name = "customsTinIssuingAuthorityState";
            this.customsTinIssuingAuthorityState.Size = new System.Drawing.Size(15, 14);
            this.customsTinIssuingAuthorityState.TabIndex = 8;
            this.customsTinIssuingAuthorityState.Tag = "";
            this.customsTinIssuingAuthorityState.UseVisualStyleBackColor = true;
            // 
            // labelCustomsTinIssuingAuthority
            // 
            this.labelCustomsTinIssuingAuthority.AutoSize = true;
            this.labelCustomsTinIssuingAuthority.BackColor = System.Drawing.Color.Transparent;
            this.labelCustomsTinIssuingAuthority.Location = new System.Drawing.Point(47, 135);
            this.labelCustomsTinIssuingAuthority.Name = "labelCustomsTinIssuingAuthority";
            this.labelCustomsTinIssuingAuthority.Size = new System.Drawing.Size(93, 13);
            this.labelCustomsTinIssuingAuthority.TabIndex = 115;
            this.labelCustomsTinIssuingAuthority.Text = "Issuing Authority:";
            // 
            // customsTinIssuingAuthority
            // 
            this.customsTinIssuingAuthority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customsTinIssuingAuthority.FormattingEnabled = true;
            this.customsTinIssuingAuthority.Location = new System.Drawing.Point(146, 132);
            this.customsTinIssuingAuthority.Name = "customsTinIssuingAuthority";
            this.customsTinIssuingAuthority.PromptText = "(Multiple Values)";
            this.customsTinIssuingAuthority.Size = new System.Drawing.Size(206, 21);
            this.customsTinIssuingAuthority.TabIndex = 9;
            // 
            // groupLabels
            // 
            this.groupLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabels.Controls.Add(this.labelThermalNote);
            this.groupLabels.Controls.Add(this.requestedLabelFormat);
            this.groupLabels.Controls.Add(this.requestedLabelFormatState);
            this.groupLabels.Controls.Add(this.dividerLabels);
            this.groupLabels.Location = new System.Drawing.Point(6, 255);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Size = new System.Drawing.Size(405, 73);
            this.groupLabels.TabIndex = 2;
            this.groupLabels.TabStop = false;
            this.groupLabels.Text = "Labels";
            // 
            // labelThermalNote
            // 
            this.labelThermalNote.AutoSize = true;
            this.labelThermalNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelThermalNote.Location = new System.Drawing.Point(50, 48);
            this.labelThermalNote.Name = "labelThermalNote";
            this.labelThermalNote.Size = new System.Drawing.Size(305, 13);
            this.labelThermalNote.TabIndex = 103;
            this.labelThermalNote.Text = "Note: DHL eCommerce only supports Standard and ZPL labels.";
            this.labelThermalNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(50, 20);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(267, 21);
            this.requestedLabelFormat.State = false;
            this.requestedLabelFormat.TabIndex = 1;
            // 
            // requestedLabelFormatState
            // 
            this.requestedLabelFormatState.AutoSize = true;
            this.requestedLabelFormatState.Location = new System.Drawing.Point(9, 23);
            this.requestedLabelFormatState.Name = "requestedLabelFormatState";
            this.requestedLabelFormatState.Size = new System.Drawing.Size(15, 14);
            this.requestedLabelFormatState.TabIndex = 0;
            this.requestedLabelFormatState.UseVisualStyleBackColor = true;
            // 
            // dividerLabels
            // 
            this.dividerLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dividerLabels.AutoSize = false;
            this.dividerLabels.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.dividerLabels.Location = new System.Drawing.Point(29, 17);
            this.dividerLabels.Name = "kryptonBorderEdge11";
            this.dividerLabels.Size = new System.Drawing.Size(1, 48);
            this.dividerLabels.Text = "kryptonBorderEdge11";
            // 
            // groupOptions
            // 
            this.groupOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupOptions.Controls.Add(this.labelReference1);
            this.groupOptions.Controls.Add(this.reference1State);
            this.groupOptions.Controls.Add(this.reference1);
            this.groupOptions.Controls.Add(this.resDeliveryState);
            this.groupOptions.Controls.Add(this.resDelivery);
            this.groupOptions.Controls.Add(this.labelResDelivery);
            this.groupOptions.Controls.Add(this.nonMachinable);
            this.groupOptions.Controls.Add(this.nonMachinableState);
            this.groupOptions.Controls.Add(this.labelDuty);
            this.groupOptions.Controls.Add(this.labelNonMachinable);
            this.groupOptions.Controls.Add(this.labelSaturday);
            this.groupOptions.Controls.Add(this.dutyDeliveryPaid);
            this.groupOptions.Controls.Add(this.dutyDeliveryPaidState);
            this.groupOptions.Controls.Add(this.saturdayDelivery);
            this.groupOptions.Controls.Add(this.saturdayState);
            this.groupOptions.Controls.Add(this.dividerOptions);
            this.groupOptions.Location = new System.Drawing.Point(6, 416);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new System.Drawing.Size(405, 165);
            this.groupOptions.TabIndex = 4;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            // 
            // labelReference1
            // 
            this.labelReference1.AutoSize = true;
            this.labelReference1.BackColor = System.Drawing.Color.Transparent;
            this.labelReference1.Location = new System.Drawing.Point(70, 135);
            this.labelReference1.Name = "labelReference1";
            this.labelReference1.Size = new System.Drawing.Size(70, 13);
            this.labelReference1.TabIndex = 93;
            this.labelReference1.Text = "Reference 1:";
            // 
            // reference1State
            // 
            this.reference1State.AutoSize = true;
            this.reference1State.Checked = true;
            this.reference1State.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reference1State.Location = new System.Drawing.Point(9, 135);
            this.reference1State.Name = "reference1State";
            this.reference1State.Size = new System.Drawing.Size(15, 14);
            this.reference1State.TabIndex = 92;
            this.reference1State.Tag = "";
            this.reference1State.UseVisualStyleBackColor = true;
            // 
            // reference1
            // 
            this.reference1.Location = new System.Drawing.Point(146, 132);
            this.reference1.Name = "reference1";
            this.reference1.Size = new System.Drawing.Size(206, 21);
            this.reference1.TabIndex = 90;
            // 
            // resDeliveryState
            // 
            this.resDeliveryState.AutoSize = true;
            this.resDeliveryState.Checked = true;
            this.resDeliveryState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resDeliveryState.Location = new System.Drawing.Point(9, 107);
            this.resDeliveryState.Name = "resDeliveryState";
            this.resDeliveryState.Size = new System.Drawing.Size(15, 14);
            this.resDeliveryState.TabIndex = 88;
            this.resDeliveryState.Tag = "";
            this.resDeliveryState.UseVisualStyleBackColor = true;
            // 
            // resDelivery
            // 
            this.resDelivery.AutoSize = true;
            this.resDelivery.Location = new System.Drawing.Point(146, 107);
            this.resDelivery.Name = "resDelivery";
            this.resDelivery.Size = new System.Drawing.Size(120, 17);
            this.resDelivery.TabIndex = 86;
            this.resDelivery.Text = "Residential Delivery";
            this.resDelivery.UseVisualStyleBackColor = true;
            // 
            // labelResDelivery
            // 
            this.labelResDelivery.AutoSize = true;
            this.labelResDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelResDelivery.Location = new System.Drawing.Point(35, 107);
            this.labelResDelivery.Name = "labelResDelivery";
            this.labelResDelivery.Size = new System.Drawing.Size(105, 13);
            this.labelResDelivery.TabIndex = 87;
            this.labelResDelivery.Text = "Residential Delivery:";
            // 
            // nonMachinable
            // 
            this.nonMachinable.AutoSize = true;
            this.nonMachinable.Location = new System.Drawing.Point(146, 79);
            this.nonMachinable.Name = "nonMachinable";
            this.nonMachinable.Size = new System.Drawing.Size(102, 17);
            this.nonMachinable.TabIndex = 5;
            this.nonMachinable.Text = "Non-Machinable";
            this.nonMachinable.UseVisualStyleBackColor = true;
            // 
            // nonMachinableState
            // 
            this.nonMachinableState.AutoSize = true;
            this.nonMachinableState.Checked = true;
            this.nonMachinableState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nonMachinableState.Location = new System.Drawing.Point(9, 79);
            this.nonMachinableState.Name = "nonMachinableState";
            this.nonMachinableState.Size = new System.Drawing.Size(15, 14);
            this.nonMachinableState.TabIndex = 4;
            this.nonMachinableState.Tag = "";
            this.nonMachinableState.UseVisualStyleBackColor = true;
            // 
            // labelDuty
            // 
            this.labelDuty.AutoSize = true;
            this.labelDuty.BackColor = System.Drawing.Color.Transparent;
            this.labelDuty.Location = new System.Drawing.Point(106, 51);
            this.labelDuty.Name = "labelDuty";
            this.labelDuty.Size = new System.Drawing.Size(34, 13);
            this.labelDuty.TabIndex = 84;
            this.labelDuty.Text = "Duty:";
            // 
            // labelNonMachinable
            // 
            this.labelNonMachinable.AutoSize = true;
            this.labelNonMachinable.BackColor = System.Drawing.Color.Transparent;
            this.labelNonMachinable.Location = new System.Drawing.Point(53, 79);
            this.labelNonMachinable.Name = "labelNonMachinable";
            this.labelNonMachinable.Size = new System.Drawing.Size(87, 13);
            this.labelNonMachinable.TabIndex = 77;
            this.labelNonMachinable.Text = "Non-Machinable:";
            // 
            // labelSaturday
            // 
            this.labelSaturday.AutoSize = true;
            this.labelSaturday.BackColor = System.Drawing.Color.Transparent;
            this.labelSaturday.Location = new System.Drawing.Point(85, 23);
            this.labelSaturday.Name = "labelSaturday";
            this.labelSaturday.Size = new System.Drawing.Size(55, 13);
            this.labelSaturday.TabIndex = 79;
            this.labelSaturday.Text = "Saturday:";
            // 
            // dutyDeliveryPaid
            // 
            this.dutyDeliveryPaid.AutoSize = true;
            this.dutyDeliveryPaid.Location = new System.Drawing.Point(146, 51);
            this.dutyDeliveryPaid.Name = "dutyDeliveryPaid";
            this.dutyDeliveryPaid.Size = new System.Drawing.Size(114, 17);
            this.dutyDeliveryPaid.TabIndex = 3;
            this.dutyDeliveryPaid.Text = "Delivery Duty Paid";
            this.dutyDeliveryPaid.UseVisualStyleBackColor = true;
            // 
            // dutyDeliveryPaidState
            // 
            this.dutyDeliveryPaidState.AutoSize = true;
            this.dutyDeliveryPaidState.Checked = true;
            this.dutyDeliveryPaidState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dutyDeliveryPaidState.Location = new System.Drawing.Point(9, 51);
            this.dutyDeliveryPaidState.Name = "dutyDeliveryPaidState";
            this.dutyDeliveryPaidState.Size = new System.Drawing.Size(15, 14);
            this.dutyDeliveryPaidState.TabIndex = 2;
            this.dutyDeliveryPaidState.Tag = "";
            this.dutyDeliveryPaidState.UseVisualStyleBackColor = true;
            // 
            // saturdayDelivery
            // 
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.Location = new System.Drawing.Point(146, 23);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 1;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = true;
            // 
            // saturdayState
            // 
            this.saturdayState.AutoSize = true;
            this.saturdayState.Checked = true;
            this.saturdayState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saturdayState.Location = new System.Drawing.Point(9, 23);
            this.saturdayState.Name = "saturdayState";
            this.saturdayState.Size = new System.Drawing.Size(15, 14);
            this.saturdayState.TabIndex = 0;
            this.saturdayState.Tag = "";
            this.saturdayState.UseVisualStyleBackColor = true;
            // 
            // dividerOptions
            // 
            this.dividerOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dividerOptions.AutoSize = false;
            this.dividerOptions.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.dividerOptions.Location = new System.Drawing.Point(29, 17);
            this.dividerOptions.Name = "kryptonBorderEdge2";
            this.dividerOptions.Size = new System.Drawing.Size(1, 140);
            this.dividerOptions.Text = "kryptonBorderEdge1";
            // 
            // groupShipment
            // 
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.dimensions);
            this.groupShipment.Controls.Add(this.labelDimensions);
            this.groupShipment.Controls.Add(this.dimensionsState);
            this.groupShipment.Controls.Add(this.weight);
            this.groupShipment.Controls.Add(this.labelWeight);
            this.groupShipment.Controls.Add(this.weightState);
            this.groupShipment.Controls.Add(this.labelPackageType);
            this.groupShipment.Controls.Add(this.packageTypeState);
            this.groupShipment.Controls.Add(this.packageType);
            this.groupShipment.Controls.Add(this.dividerShipment);
            this.groupShipment.Controls.Add(this.labelService);
            this.groupShipment.Controls.Add(this.service);
            this.groupShipment.Controls.Add(this.serviceState);
            this.groupShipment.Location = new System.Drawing.Point(6, 65);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(405, 184);
            this.groupShipment.TabIndex = 1;
            this.groupShipment.TabStop = false;
            this.groupShipment.Text = "Shipment";
            // 
            // dimensions
            // 
            this.dimensions.Cleared = false;
            this.dimensions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensions.Location = new System.Drawing.Point(116, 101);
            this.dimensions.Name = "dimensions";
            this.dimensions.Size = new System.Drawing.Size(210, 77);
            this.dimensions.TabIndex = 61;
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(49, 107);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 60;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // dimensionsState
            // 
            this.dimensionsState.AutoSize = true;
            this.dimensionsState.Checked = true;
            this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsState.Location = new System.Drawing.Point(9, 107);
            this.dimensionsState.Name = "dimensionsState";
            this.dimensionsState.Size = new System.Drawing.Size(15, 14);
            this.dimensionsState.TabIndex = 59;
            this.dimensionsState.Tag = "";
            this.dimensionsState.UseVisualStyleBackColor = true;
            // 
            // weight
            // 
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.ConfigureTelemetryEntityCounts = null;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(119, 76);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(282, 24);
            this.weight.TabIndex = 58;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(68, 79);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 57;
            this.labelWeight.Text = "Weight:";
            // 
            // weightState
            // 
            this.weightState.AutoSize = true;
            this.weightState.Checked = true;
            this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.weightState.Location = new System.Drawing.Point(9, 79);
            this.weightState.Name = "weightState";
            this.weightState.Size = new System.Drawing.Size(15, 14);
            this.weightState.TabIndex = 56;
            this.weightState.Tag = "";
            this.weightState.UseVisualStyleBackColor = true;
            // 
            // labelPackageType
            // 
            this.labelPackageType.AutoSize = true;
            this.labelPackageType.BackColor = System.Drawing.Color.Transparent;
            this.labelPackageType.Location = new System.Drawing.Point(35, 51);
            this.labelPackageType.Name = "labelPackageType";
            this.labelPackageType.Size = new System.Drawing.Size(78, 13);
            this.labelPackageType.TabIndex = 55;
            this.labelPackageType.Text = "Package Type:";
            // 
            // packageTypeState
            // 
            this.packageTypeState.AutoSize = true;
            this.packageTypeState.Checked = true;
            this.packageTypeState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packageTypeState.Location = new System.Drawing.Point(9, 51);
            this.packageTypeState.Name = "packageTypeState";
            this.packageTypeState.Size = new System.Drawing.Size(15, 14);
            this.packageTypeState.TabIndex = 54;
            this.packageTypeState.Tag = "";
            this.packageTypeState.UseVisualStyleBackColor = true;
            // 
            // packageType
            // 
            this.packageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packageType.FormattingEnabled = true;
            this.packageType.Location = new System.Drawing.Point(119, 48);
            this.packageType.Name = "packageType";
            this.packageType.PromptText = "(Multiple Values)";
            this.packageType.Size = new System.Drawing.Size(206, 21);
            this.packageType.TabIndex = 53;
            // 
            // dividerShipment
            // 
            this.dividerShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dividerShipment.AutoSize = false;
            this.dividerShipment.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.dividerShipment.Location = new System.Drawing.Point(29, 17);
            this.dividerShipment.Name = "kryptonBorderEdge";
            this.dividerShipment.Size = new System.Drawing.Size(1, 159);
            this.dividerShipment.Text = "kryptonBorderEdge1";
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(67, 23);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 52;
            this.labelService.Text = "Service:";
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(119, 20);
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
            this.serviceState.Location = new System.Drawing.Point(9, 23);
            this.serviceState.Name = "serviceState";
            this.serviceState.Size = new System.Drawing.Size(15, 14);
            this.serviceState.TabIndex = 0;
            this.serviceState.Tag = "";
            this.serviceState.UseVisualStyleBackColor = true;
            // 
            // groupFrom
            // 
            this.groupFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupFrom.Controls.Add(this.labelAccount);
            this.groupFrom.Controls.Add(this.accountState);
            this.groupFrom.Controls.Add(this.dhlEcommerceAccount);
            this.groupFrom.Controls.Add(this.dividerFrom);
            this.groupFrom.Location = new System.Drawing.Point(6, 6);
            this.groupFrom.Name = "groupFrom";
            this.groupFrom.Size = new System.Drawing.Size(405, 53);
            this.groupFrom.TabIndex = 0;
            this.groupFrom.TabStop = false;
            this.groupFrom.Text = "From";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(63, 23);
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
            this.accountState.Location = new System.Drawing.Point(9, 23);
            this.accountState.Name = "accountState";
            this.accountState.Size = new System.Drawing.Size(15, 14);
            this.accountState.TabIndex = 0;
            this.accountState.Tag = "";
            this.accountState.UseVisualStyleBackColor = true;
            // 
            // dhlEcommerceAccount
            // 
            this.dhlEcommerceAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dhlEcommerceAccount.FormattingEnabled = true;
            this.dhlEcommerceAccount.Location = new System.Drawing.Point(119, 20);
            this.dhlEcommerceAccount.Name = "dhlEcommerceAccount";
            this.dhlEcommerceAccount.PromptText = "(Multiple Values)";
            this.dhlEcommerceAccount.Size = new System.Drawing.Size(206, 21);
            this.dhlEcommerceAccount.TabIndex = 1;
            // 
            // dividerFrom
            // 
            this.dividerFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dividerFrom.AutoSize = false;
            this.dividerFrom.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.dividerFrom.Location = new System.Drawing.Point(29, 17);
            this.dividerFrom.Name = "kryptonBorderEdge1";
            this.dividerFrom.Size = new System.Drawing.Size(1, 28);
            this.dividerFrom.Text = "kryptonBorderEdge1";
            // 
            // DhlEcommerceProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "DhlEcommerceProfileControl";
            this.Size = new System.Drawing.Size(425, 1127);
            this.tabControl.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupCustoms.ResumeLayout(false);
            this.groupCustoms.PerformLayout();
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.groupFrom.ResumeLayout(false);
            this.groupFrom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupShipment;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge dividerShipment;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.CheckBox serviceState;
        private System.Windows.Forms.GroupBox groupFrom;
        private System.Windows.Forms.Label labelAccount;
        private ShipWorks.UI.Controls.MultiValueComboBox dhlEcommerceAccount;
        private System.Windows.Forms.CheckBox accountState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge dividerFrom;
        private System.Windows.Forms.GroupBox groupOptions;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge dividerOptions;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private System.Windows.Forms.Label labelSaturday;
        private System.Windows.Forms.CheckBox saturdayState;
        private System.Windows.Forms.Label labelDuty;
        private System.Windows.Forms.CheckBox dutyDeliveryPaid;
        private System.Windows.Forms.CheckBox dutyDeliveryPaidState;
        private System.Windows.Forms.CheckBox nonMachinableState;
        private System.Windows.Forms.Label labelNonMachinable;
        protected System.Windows.Forms.GroupBox groupLabels;
        protected Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge dividerLabels;
        private System.Windows.Forms.Label labelThermalNote;
        private System.Windows.Forms.CheckBox nonMachinable;
        protected System.Windows.Forms.GroupBox groupCustoms;
        private System.Windows.Forms.CheckBox contentsState;
        private System.Windows.Forms.CheckBox nonDeliveryState;
        private System.Windows.Forms.Label labelNonDelivery;
        private ShipWorks.UI.Controls.MultiValueComboBox nonDelivery;
        private System.Windows.Forms.Label labelContents;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge dividerCustoms;
        private ShipWorks.UI.Controls.MultiValueComboBox contents;
        private System.Windows.Forms.GroupBox groupInsurance;
        private ShipWorks.Shipping.Insurance.InsuranceProfileControl insuranceControl;
        private System.Windows.Forms.CheckBox insuranceState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge dividerInsurance;
        private System.Windows.Forms.CheckBox resDeliveryState;
        private System.Windows.Forms.CheckBox resDelivery;
        private System.Windows.Forms.Label labelResDelivery;
        private System.Windows.Forms.CheckBox customsRecipientTinState;
        private ShipWorks.UI.Controls.MultiValueTextBox customsRecipientTin;
        private System.Windows.Forms.Label labelCustomsRecipientTin;
        private System.Windows.Forms.Label labelTaxIdType;
        private ShipWorks.UI.Controls.MultiValueComboBox taxIdType;
        private System.Windows.Forms.CheckBox taxIdTypeState;
        private MultiValueComboBox customsTinIssuingAuthority;
        private Label labelCustomsTinIssuingAuthority;
        private CheckBox customsTinIssuingAuthorityState;
        private MultiValueTextBox reference1;
        private Label labelReference1;
        private CheckBox reference1State;
        private Label labelPackageType;
        private CheckBox packageTypeState;
        private MultiValueComboBox packageType;
        private WeightControl weight;
        private Label labelWeight;
        private CheckBox weightState;
        private Editing.DimensionsControl dimensions;
        private Label labelDimensions;
        private CheckBox dimensionsState;
    }
}