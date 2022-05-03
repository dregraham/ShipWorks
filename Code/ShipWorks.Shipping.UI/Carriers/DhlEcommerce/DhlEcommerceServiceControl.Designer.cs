using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.UI.Carriers.DhlEcommerce
{
    partial class DhlEcommerceServiceControl
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
            this.cutoffDateDisplay = new ShipWorks.Shipping.Editing.ShippingDateCutoffDisplayControl();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.accountSectionLabel = new System.Windows.Forms.Label();
            this.dhlEcommerceAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.accountLabel = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackageType = new System.Windows.Forms.Label();
            this.packageType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.sectionOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.labelSaturdayDelivery = new System.Windows.Forms.Label();
            this.labelDeliveryDutyPaid = new System.Windows.Forms.Label();
            this.deliveryDutyPaid = new System.Windows.Forms.CheckBox();
            this.labelNonMachinable = new System.Windows.Forms.Label();
            this.nonMachinable = new System.Windows.Forms.CheckBox();
            this.labelResidentialDelivery = new System.Windows.Forms.Label();
            this.residentialDelivery = new System.Windows.Forms.CheckBox();
            this.labelReference1 = new System.Windows.Forms.Label();
            this.reference1 = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.dimensionsLabel = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).BeginInit();
            this.sectionLabelOptions.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).BeginInit();
            this.sectionOptions.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Collapsed = false;
            // 
            // sectionRecipient.ContentPanel
            // 
            this.sectionRecipient.ContentPanel.Controls.Add(this.residentialDetermination);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelAddress);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelResidentialCommercial);
            this.sectionRecipient.ContentPanel.Controls.Add(this.personControl);
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(377, 414);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(367, 333);
            // 
            // labelResidentialCommercial
            // 
            this.labelResidentialCommercial.Location = new System.Drawing.Point(10, 338);
            // 
            // labelAddress
            // 
            this.labelAddress.Location = new System.Drawing.Point(23, 361);
            // 
            // residentialDetermination
            // 
            this.residentialDetermination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.residentialDetermination.Location = new System.Drawing.Point(79, 358);
            this.residentialDetermination.Size = new System.Drawing.Size(277, 21);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 811);
            this.sectionReturns.Size = new System.Drawing.Size(377, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsLabel);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelWeight);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelPackageType);
            this.sectionShipment.ContentPanel.Controls.Add(this.packageType);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.cutoffDateDisplay);
            this.sectionShipment.Location = new System.Drawing.Point(3, 528);
            this.sectionShipment.Size = new System.Drawing.Size(377, 278);
            this.sectionShipment.TabIndex = 2;
            // 
            // sectionLabelOptions
            // 
            this.sectionLabelOptions.Collapsed = false;
            // 
            // sectionLabelOptions.ContentPanel
            // 
            this.sectionLabelOptions.ContentPanel.Controls.Add(this.labelFormat);
            this.sectionLabelOptions.Location = new System.Drawing.Point(3, 453);
            this.sectionLabelOptions.Size = new System.Drawing.Size(377, 70);
            // 
            // cutoffDateDisplay
            // 
            this.cutoffDateDisplay.AutoSize = true;
            this.cutoffDateDisplay.BackColor = System.Drawing.Color.White;
            this.cutoffDateDisplay.Caption = "Cutoff time is 3:00 PM";
            this.cutoffDateDisplay.Location = new System.Drawing.Point(239, 71);
            this.cutoffDateDisplay.Name = "cutoffDateDisplay";
            this.cutoffDateDisplay.ShipmentType = ShipWorks.Shipping.ShipmentTypeCode.DhlEcommerce;
            this.cutoffDateDisplay.Size = new System.Drawing.Size(113, 15);
            this.cutoffDateDisplay.TabIndex = 6;
            this.cutoffDateDisplay.Title = "Shipment cutoff time";
            // 
            // sectionFrom
            // 
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            // 
            // sectionFrom.ContentPanel
            // 
            this.sectionFrom.ContentPanel.Controls.Add(this.originControl);
            this.sectionFrom.ContentPanel.Controls.Add(this.accountSectionLabel);
            this.sectionFrom.ContentPanel.Controls.Add(this.dhlEcommerceAccount);
            this.sectionFrom.ContentPanel.Controls.Add(this.accountLabel);
            this.sectionFrom.ExpandedHeight = 438;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(377, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(1, 60);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(369, 355);
            this.originControl.TabIndex = 9;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // accountSectionLabel
            // 
            this.accountSectionLabel.AutoSize = true;
            this.accountSectionLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountSectionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountSectionLabel.Location = new System.Drawing.Point(3, 10);
            this.accountSectionLabel.Name = "accountSectionLabel";
            this.accountSectionLabel.Size = new System.Drawing.Size(149, 13);
            this.accountSectionLabel.TabIndex = 5;
            this.accountSectionLabel.Text = "DHL eCommerce Account";
            // 
            // dhlEcommerceAccount
            // 
            this.dhlEcommerceAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dhlEcommerceAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dhlEcommerceAccount.FormattingEnabled = true;
            this.dhlEcommerceAccount.Location = new System.Drawing.Point(80, 32);
            this.dhlEcommerceAccount.Name = "dhlEcommerceAccount";
            this.dhlEcommerceAccount.PromptText = "(Multiple Values)";
            this.dhlEcommerceAccount.Size = new System.Drawing.Size(276, 21);
            this.dhlEcommerceAccount.TabIndex = 4;
            this.dhlEcommerceAccount.SelectedIndexChanged += new System.EventHandler(this.OnChangeAccount);
            // 
            // accountLabel
            // 
            this.accountLabel.AutoSize = true;
            this.accountLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountLabel.Location = new System.Drawing.Point(23, 35);
            this.accountLabel.Name = "accountLabel";
            this.accountLabel.Size = new System.Drawing.Size(50, 13);
            this.accountLabel.TabIndex = 0;
            this.accountLabel.Text = "Account:";
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(38, 15);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 68;
            this.labelService.Text = "Service:";
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(90, 12);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(175, 21);
            this.service.TabIndex = 65;
            // 
            // labelPackageType
            // 
            this.labelPackageType.AutoSize = true;
            this.labelPackageType.BackColor = System.Drawing.Color.Transparent;
            this.labelPackageType.Location = new System.Drawing.Point(6, 42);
            this.labelPackageType.Name = "labelPackageType";
            this.labelPackageType.Size = new System.Drawing.Size(78, 13);
            this.labelPackageType.TabIndex = 68;
            this.labelPackageType.Text = "Package Type:";
            // 
            // packageType
            // 
            this.packageType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packageType.FormattingEnabled = true;
            this.packageType.Location = new System.Drawing.Point(90, 39);
            this.packageType.Name = "packageType";
            this.packageType.PromptText = "(Multiple Values)";
            this.packageType.Size = new System.Drawing.Size(175, 21);
            this.packageType.TabIndex = 65;
            this.packageType.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // labelShipDate
            // 
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(28, 71);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 69;
            this.labelShipDate.Text = "Ship date:";
            // 
            // shipDate
            // 
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(90, 67);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(144, 21);
            this.shipDate.TabIndex = 66;
            // 
            // sectionOptions
            // 
            this.sectionOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionOptions.ContentPanel
            // 
            this.sectionOptions.ContentPanel.Controls.Add(this.labelSaturdayDelivery);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelDeliveryDutyPaid);
            this.sectionOptions.ContentPanel.Controls.Add(this.deliveryDutyPaid);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelNonMachinable);
            this.sectionOptions.ContentPanel.Controls.Add(this.nonMachinable);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelResidentialDelivery);
            this.sectionOptions.ContentPanel.Controls.Add(this.residentialDelivery);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelReference1);
            this.sectionOptions.ContentPanel.Controls.Add(this.reference1);
            this.sectionOptions.ContentPanel.Controls.Add(this.saturdayDelivery);
            this.sectionOptions.ExtraText = "";
            this.sectionOptions.Location = new System.Drawing.Point(3, 840);
            this.sectionOptions.Name = "sectionOptions";
            this.sectionOptions.SectionName = "Options";
            this.sectionOptions.SettingsKey = "{2740f860-1d14-453e-a511-8f62ad1e7dcc}";
            this.sectionOptions.Size = new System.Drawing.Size(377, 165);
            this.sectionOptions.TabIndex = 3;
            // 
            // labelSaturdayDelivery
            // 
            this.labelSaturdayDelivery.AutoSize = true;
            this.labelSaturdayDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelSaturdayDelivery.Location = new System.Drawing.Point(14, 15);
            this.labelSaturdayDelivery.Name = "labelSaturdayDelivery";
            this.labelSaturdayDelivery.Size = new System.Drawing.Size(97, 13);
            this.labelSaturdayDelivery.TabIndex = 7;
            this.labelSaturdayDelivery.Text = "Saturday Delivery:";
            // 
            // labelDeliveryDutyPaid
            // 
            this.labelDeliveryDutyPaid.AutoSize = true;
            this.labelDeliveryDutyPaid.BackColor = System.Drawing.Color.Transparent;
            this.labelDeliveryDutyPaid.Location = new System.Drawing.Point(12, 38);
            this.labelDeliveryDutyPaid.Name = "labelDeliveryDutyPaid";
            this.labelDeliveryDutyPaid.Size = new System.Drawing.Size(99, 13);
            this.labelDeliveryDutyPaid.TabIndex = 7;
            this.labelDeliveryDutyPaid.Text = "Delivery Duty Paid:";
            // 
            // deliveryDutyPaid
            // 
            this.deliveryDutyPaid.AutoSize = true;
            this.deliveryDutyPaid.BackColor = System.Drawing.Color.White;
            this.deliveryDutyPaid.Location = new System.Drawing.Point(117, 37);
            this.deliveryDutyPaid.Name = "deliveryDutyPaid";
            this.deliveryDutyPaid.Size = new System.Drawing.Size(114, 17);
            this.deliveryDutyPaid.TabIndex = 8;
            this.deliveryDutyPaid.Text = "Delivery Duty Paid";
            this.deliveryDutyPaid.UseVisualStyleBackColor = false;
            // 
            // labelNonMachinable
            // 
            this.labelNonMachinable.AutoSize = true;
            this.labelNonMachinable.BackColor = System.Drawing.Color.Transparent;
            this.labelNonMachinable.Location = new System.Drawing.Point(24, 61);
            this.labelNonMachinable.Name = "labelNonMachinable";
            this.labelNonMachinable.Size = new System.Drawing.Size(87, 13);
            this.labelNonMachinable.TabIndex = 7;
            this.labelNonMachinable.Text = "Non-Machinable:";
            // 
            // nonMachinable
            // 
            this.nonMachinable.AutoSize = true;
            this.nonMachinable.BackColor = System.Drawing.Color.White;
            this.nonMachinable.Location = new System.Drawing.Point(117, 60);
            this.nonMachinable.Name = "nonMachinable";
            this.nonMachinable.Size = new System.Drawing.Size(102, 17);
            this.nonMachinable.TabIndex = 8;
            this.nonMachinable.Text = "Non-Machinable";
            this.nonMachinable.UseVisualStyleBackColor = false;
            // 
            // labelResidentialDelivery
            // 
            this.labelResidentialDelivery.AutoSize = true;
            this.labelResidentialDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelResidentialDelivery.Location = new System.Drawing.Point(6, 84);
            this.labelResidentialDelivery.Name = "labelResidentialDelivery";
            this.labelResidentialDelivery.Size = new System.Drawing.Size(105, 13);
            this.labelResidentialDelivery.TabIndex = 7;
            this.labelResidentialDelivery.Text = "Residential Delivery:";
            // 
            // residentialDelivery
            // 
            this.residentialDelivery.AutoSize = true;
            this.residentialDelivery.BackColor = System.Drawing.Color.White;
            this.residentialDelivery.Location = new System.Drawing.Point(117, 83);
            this.residentialDelivery.Name = "residentialDelivery";
            this.residentialDelivery.Size = new System.Drawing.Size(120, 17);
            this.residentialDelivery.TabIndex = 8;
            this.residentialDelivery.Text = "Residential Delivery";
            this.residentialDelivery.UseVisualStyleBackColor = false;
            // 
            // labelReference1
            // 
            this.labelReference1.AutoSize = true;
            this.labelReference1.BackColor = System.Drawing.Color.Transparent;
            this.labelReference1.Location = new System.Drawing.Point(41, 109);
            this.labelReference1.Name = "labelReference1";
            this.labelReference1.Size = new System.Drawing.Size(70, 13);
            this.labelReference1.TabIndex = 7;
            this.labelReference1.Text = "Reference 1:";
            // 
            // reference1
            // 
            this.reference1.BackColor = System.Drawing.Color.White;
            this.reference1.Location = new System.Drawing.Point(117, 106);
            this.reference1.Name = "reference1";
            this.reference1.Size = new System.Drawing.Size(165, 21);
            this.reference1.TabIndex = 8;
            this.reference1.Text = "(Multiple Values)";
            // 
            // saturdayDelivery
            // 
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.BackColor = System.Drawing.Color.White;
            this.saturdayDelivery.Location = new System.Drawing.Point(117, 14);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 8;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = false;
            // 
            // insuranceControl
            // 
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(3, 198);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(404, 50);
            this.insuranceControl.TabIndex = 74;
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dimensionsControl.Location = new System.Drawing.Point(87, 119);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 73;
            this.dimensionsControl.DimensionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // dimensionsLabel
            // 
            this.dimensionsLabel.AutoSize = true;
            this.dimensionsLabel.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsLabel.Location = new System.Drawing.Point(20, 125);
            this.dimensionsLabel.Name = "dimensionsLabel";
            this.dimensionsLabel.Size = new System.Drawing.Size(64, 13);
            this.dimensionsLabel.TabIndex = 72;
            this.dimensionsLabel.Text = "Dimensions:";
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(39, 97);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 70;
            this.labelWeight.Text = "Weight:";
            // 
            // weight
            // 
            this.weight.AutoSize = true;
            this.weight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.ConfigureTelemetryEntityCounts = null;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weight.Location = new System.Drawing.Point(90, 94);
            this.weight.Name = "weight";
            this.weight.RangeMax = 9999D;
            this.weight.RangeMin = 0D;
            this.weight.ShowShortcutInfo = true;
            this.weight.Size = new System.Drawing.Size(269, 24);
            this.weight.TabIndex = 71;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler<ShipWorks.UI.Controls.WeightChangedEventArgs>(this.OnRateCriteriaChanged);
            // 
            // DhlEcommerceServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.sectionOptions);
            this.Name = "DhlEcommerceServiceControl";
            this.Size = new System.Drawing.Size(383, 651);
            this.Controls.SetChildIndex(this.sectionOptions, 0);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionLabelOptions, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            this.sectionLabelOptions.ContentPanel.ResumeLayout(false);
            this.sectionLabelOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            this.sectionFrom.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).EndInit();
            this.sectionOptions.ContentPanel.ResumeLayout(false);
            this.sectionOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private System.Windows.Forms.Label accountLabel;
        private ShipWorks.UI.Controls.MultiValueComboBox dhlEcommerceAccount;
        private System.Windows.Forms.Label accountSectionLabel;
        private ShipmentOriginControl originControl;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelPackageType;
        private ShipWorks.UI.Controls.MultiValueComboBox packageType;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionOptions;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private System.Windows.Forms.Label labelSaturdayDelivery;
        private System.Windows.Forms.CheckBox deliveryDutyPaid;
        private System.Windows.Forms.Label labelDeliveryDutyPaid;
        private System.Windows.Forms.CheckBox nonMachinable;
        private System.Windows.Forms.Label labelNonMachinable;
        private System.Windows.Forms.CheckBox residentialDelivery;
        private System.Windows.Forms.Label labelResidentialDelivery;
        private ShipWorks.UI.Controls.MultiValueTextBox reference1;
        private System.Windows.Forms.Label labelReference1;
        private System.Windows.Forms.Label labelShipDate;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker shipDate;
        protected ShipWorks.Shipping.Insurance.InsuranceSelectionControl insuranceControl;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label labelWeight;
        private ShipWorks.UI.Controls.WeightControl weight;
        private Editing.ShippingDateCutoffDisplayControl cutoffDateDisplay;
        private System.Windows.Forms.Label dimensionsLabel;
    }
}
