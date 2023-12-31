﻿using System;
using System.Windows.Forms;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    partial class FedExServiceControl
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory3 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory4 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory5 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.cutoffDateDisplay = new ShipWorks.Shipping.Editing.ShippingDateCutoffDisplayControl();
            this.sectionHoldAtLocation = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.fedExHoldAtLocationControl = new ShipWorks.Shipping.Carriers.FedEx.FedExHoldAtLocationControl();
            this.sectionBilling = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.panelTransportAccount = new System.Windows.Forms.Panel();
            this.payorTransportName = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelTransportPayorName = new System.Windows.Forms.Label();
            this.transportAccount = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelTransportAccount = new System.Windows.Forms.Label();
            this.payorCountry = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPayorCountry = new System.Windows.Forms.Label();
            this.payorPostalCode = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelPayorPostalCode = new System.Windows.Forms.Label();
            this.panelPayorTransport = new System.Windows.Forms.Panel();
            this.payorTransport = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPayorTransport = new System.Windows.Forms.Label();
            this.panelDeliveredDutiesPaid = new System.Windows.Forms.Panel();
            this.deliveredDutyPaid = new System.Windows.Forms.CheckBox();
            this.labelDeliveredDutyPaid = new System.Windows.Forms.Label();
            this.panelPayorDuties = new System.Windows.Forms.Panel();
            this.dutiesAccount = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelDutiesAccount = new System.Windows.Forms.Label();
            this.payorDuties = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPayorDuties = new System.Windows.Forms.Label();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fedexAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelFromResidentialCommercial = new System.Windows.Forms.Label();
            this.labelFromAddressType = new System.Windows.Forms.Label();
            this.fromAddressType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.nonStandardPackaging = new System.Windows.Forms.CheckBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.packageControl = new ShipWorks.Shipping.Carriers.FedEx.FedExPackageControl();
            this.packagingType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.sectionHomeDelivery = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.homePremiumPhone = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelHomePremiumPhone = new System.Windows.Forms.Label();
            this.homeInstructions = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.homePremiumDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.homePremiumService = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPremium = new System.Windows.Forms.Label();
            this.labelHomeInstructions = new System.Windows.Forms.Label();
            this.sectionFreight = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.fedExFreightContainerControl = new ShipWorks.Shipping.Carriers.FedEx.FedExFreightContainerControl();
            this.sectionOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.referencePO = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelPO = new System.Windows.Forms.Label();
            this.referenceInvoice = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelInvoice = new System.Windows.Forms.Label();
            this.referenceCustomer = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReference = new System.Windows.Forms.Label();
            this.signature = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelSignature = new System.Windows.Forms.Label();
            this.referenceShipmentIntegrity = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelShipmentIntegrity = new System.Windows.Forms.Label();
            this.sectionEmail = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.emailNotifyBrokerShip = new System.Windows.Forms.CheckBox();
            this.emailNotifyBrokerDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyBrokerEstimatedDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyBrokerException = new System.Windows.Forms.CheckBox();
            this.labelEmailBroker = new System.Windows.Forms.Label();
            this.emailNotifyMessage = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelPersonalMessage = new System.Windows.Forms.Label();
            this.kryptonBorderEdgeEmail2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.emailNotifyOtherShip = new System.Windows.Forms.CheckBox();
            this.emailNotifyOtherDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyOtherEstimatedDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyOtherException = new System.Windows.Forms.CheckBox();
            this.emailNotifyRecipientShip = new System.Windows.Forms.CheckBox();
            this.emailNotifyRecipientDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyRecipientEstimatedDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyRecipientException = new System.Windows.Forms.CheckBox();
            this.emailNotifyOtherAddress = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.kryptonBorderEdgeEmail = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelEmailOther = new System.Windows.Forms.Label();
            this.labelEmailRecipient = new System.Windows.Forms.Label();
            this.labelEmailSender = new System.Windows.Forms.Label();
            this.emailNotifySenderShip = new System.Windows.Forms.CheckBox();
            this.emailNotifySenderDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifySenderEstimatedDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifySenderException = new System.Windows.Forms.CheckBox();
            this.labelEmailDelivery = new System.Windows.Forms.Label();
            this.labelEmailEstimatedDelivery = new System.Windows.Forms.Label();
            this.labelEmailException = new System.Windows.Forms.Label();
            this.labelEmailShip = new System.Windows.Forms.Label();
            this.labelEmailAddress = new System.Windows.Forms.Label();
            this.labelEmailInfo = new System.Windows.Forms.Label();
            this.picturEmailInfo = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sectionCOD = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.codAddFreight = new System.Windows.Forms.CheckBox();
            this.codPaymentType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelCodPayment = new System.Windows.Forms.Label();
            this.codAmount = new ShipWorks.UI.Controls.MoneyTextBox();
            this.labelCodAmount = new System.Windows.Forms.Label();
            this.codEnabled = new System.Windows.Forms.CheckBox();
            this.taxInfoLabel = new System.Windows.Forms.Label();
            this.codTaxId = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.CodTINLabel = new System.Windows.Forms.Label();
            this.codOrigin = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.panelTrademarkInfo = new System.Windows.Forms.Panel();
            this.linkTrademarkInfo = new ShipWorks.UI.Controls.LinkControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.smartManifestID = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.sectionSmartPost = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.smartEndorsement = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.smartIndicia = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelSmartAncillary = new System.Windows.Forms.Label();
            this.labelSmartEndicia = new System.Windows.Forms.Label();
            this.infotipSmartPostConfirmation = new ShipWorks.UI.Controls.InfoTip();
            this.smartHubID = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelSmartHubID = new System.Windows.Forms.Label();
            this.smartConfirmation = new System.Windows.Forms.CheckBox();
            this.labelSmartConfirmation = new System.Windows.Forms.Label();
            this.labelSmartManifestID = new System.Windows.Forms.Label();
            this.sectionPackageDetails = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.otherPackageHolder = new System.Windows.Forms.Panel();
            this.packageDetailsControl = new ShipWorks.Shipping.Carriers.FedEx.FedExPackageDetailControl();
            this.sectionServiceOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.thirdPartyConsignee = new System.Windows.Forms.CheckBox();
            this.consigneeLabel = new System.Windows.Forms.Label();
            this.returnsClearance = new System.Windows.Forms.CheckBox();
            this.returnsClearanceLabel = new System.Windows.Forms.Label();
            this.labelDropoffType = new System.Windows.Forms.Label();
            this.dropoffType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.sectionFimsOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.fimsOptionsControl = new ShipWorks.Shipping.Carriers.FedEx.FimsOptionsControl();
            this.labelReturnsClearance = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionHoldAtLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionHoldAtLocation.ContentPanel)).BeginInit();
            this.sectionHoldAtLocation.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling.ContentPanel)).BeginInit();
            this.sectionBilling.ContentPanel.SuspendLayout();
            this.panelTransportAccount.SuspendLayout();
            this.panelPayorTransport.SuspendLayout();
            this.panelDeliveredDutiesPaid.SuspendLayout();
            this.panelPayorDuties.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionHomeDelivery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionHomeDelivery.ContentPanel)).BeginInit();
            this.sectionHomeDelivery.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFreight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFreight.ContentPanel)).BeginInit();
            this.sectionFreight.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).BeginInit();
            this.sectionOptions.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail.ContentPanel)).BeginInit();
            this.sectionEmail.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturEmailInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCOD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCOD.ContentPanel)).BeginInit();
            this.sectionCOD.ContentPanel.SuspendLayout();
            this.panelTrademarkInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSmartPost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSmartPost.ContentPanel)).BeginInit();
            this.sectionSmartPost.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionPackageDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionPackageDetails.ContentPanel)).BeginInit();
            this.sectionPackageDetails.ContentPanel.SuspendLayout();
            this.otherPackageHolder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionServiceOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionServiceOptions.ContentPanel)).BeginInit();
            this.sectionServiceOptions.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFimsOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFimsOptions.ContentPanel)).BeginInit();
            this.sectionFimsOptions.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            // 
            // sectionRecipient.ContentPanel
            // 
            this.sectionRecipient.ContentPanel.Controls.Add(this.residentialDetermination);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelAddress);
            this.sectionRecipient.ContentPanel.Controls.Add(this.labelResidentialCommercial);
            this.sectionRecipient.ContentPanel.Controls.Add(this.personControl);
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(487, 24);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(477, 330);
            // 
            // residentialDetermination
            // 
            this.residentialDetermination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.residentialDetermination.Size = new System.Drawing.Size(388, 21);
            this.residentialDetermination.SelectedIndexChanged += new System.EventHandler(this.OnResidentialDeterminationChanged);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 642);
            this.sectionReturns.SectionName = "FedEx® Returns";
            this.sectionReturns.Size = new System.Drawing.Size(487, 24);
            this.sectionReturns.TabIndex = 12;
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.nonStandardPackaging);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.saturdayDelivery);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.cutoffDateDisplay);
            this.sectionShipment.ContentPanel.Controls.Add(this.packageControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.packagingType);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelPackaging);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(487, 371);
            this.sectionShipment.TabIndex = 2;
            // 
            // sectionLabelOptions
            // 
            // 
            // sectionLabelOptions.ContentPanel
            // 
            this.sectionLabelOptions.ContentPanel.Controls.Add(this.labelFormat);
            this.sectionLabelOptions.Location = new System.Drawing.Point(3, 700);
            this.sectionLabelOptions.Size = new System.Drawing.Size(487, 24);
            this.sectionLabelOptions.TabIndex = 14;
            // 
            // cutoffDateDisplay
            // 
            this.cutoffDateDisplay.AutoSize = true;
            this.cutoffDateDisplay.BackColor = System.Drawing.Color.White;
            this.cutoffDateDisplay.Caption = "Shipments processed after 5:00 PM today will have a ship date of the next valid s" +
    "hipping day.\r\nTo update this setting, go to Manage > Shipping Settings > FedEx >" +
    " Settings.";
            this.cutoffDateDisplay.Location = new System.Drawing.Point(247, 39);
            this.cutoffDateDisplay.Name = "cutoffDateDisplay";
            this.cutoffDateDisplay.ShipmentType = ShipWorks.Shipping.ShipmentTypeCode.FedEx;
            this.cutoffDateDisplay.Size = new System.Drawing.Size(113, 15);
            this.cutoffDateDisplay.TabIndex = 6;
            this.cutoffDateDisplay.Title = "Shipment cutoff time";
            // 
            // sectionHoldAtLocation
            // 
            this.sectionHoldAtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionHoldAtLocation.Collapsed = true;
            // 
            // sectionHoldAtLocation.ContentPanel
            // 
            this.sectionHoldAtLocation.ContentPanel.Controls.Add(this.fedExHoldAtLocationControl);
            this.sectionHoldAtLocation.ExpandedHeight = 176;
            this.sectionHoldAtLocation.ExtraText = "";
            this.sectionHoldAtLocation.Location = new System.Drawing.Point(3, 526);
            this.sectionHoldAtLocation.Name = "sectionHoldAtLocation";
            this.sectionHoldAtLocation.SectionName = "Hold at Location";
            this.sectionHoldAtLocation.SettingsKey = "{9b529fd1-0bfb-4d24-8aa8-a856c930e196}";
            this.sectionHoldAtLocation.Size = new System.Drawing.Size(487, 24);
            this.sectionHoldAtLocation.TabIndex = 10;
            // 
            // fedExHoldAtLocationControl
            // 
            this.fedExHoldAtLocationControl.BackColor = System.Drawing.Color.White;
            this.fedExHoldAtLocationControl.Location = new System.Drawing.Point(1, 4);
            this.fedExHoldAtLocationControl.Name = "fedExHoldAtLocationControl";
            this.fedExHoldAtLocationControl.Size = new System.Drawing.Size(392, 150);
            this.fedExHoldAtLocationControl.TabIndex = 0;
            // 
            // sectionBilling
            // 
            this.sectionBilling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionBilling.Collapsed = true;
            // 
            // sectionBilling.ContentPanel
            // 
            this.sectionBilling.ContentPanel.Controls.Add(this.panelTransportAccount);
            this.sectionBilling.ContentPanel.Controls.Add(this.panelPayorTransport);
            this.sectionBilling.ContentPanel.Controls.Add(this.panelDeliveredDutiesPaid);
            this.sectionBilling.ExpandedHeight = 201;
            this.sectionBilling.ExtraText = "";
            this.sectionBilling.Location = new System.Drawing.Point(3, 555);
            this.sectionBilling.Name = "sectionBilling";
            this.sectionBilling.SectionName = "Billing";
            this.sectionBilling.SettingsKey = "84780845-669f-4fb1-9ab1-cd2accdff93c";
            this.sectionBilling.Size = new System.Drawing.Size(487, 24);
            this.sectionBilling.TabIndex = 6;
            // 
            // panelTransportAccount
            // 
            this.panelTransportAccount.BackColor = System.Drawing.Color.White;
            this.panelTransportAccount.Controls.Add(this.payorTransportName);
            this.panelTransportAccount.Controls.Add(this.labelTransportPayorName);
            this.panelTransportAccount.Controls.Add(this.transportAccount);
            this.panelTransportAccount.Controls.Add(this.labelTransportAccount);
            this.panelTransportAccount.Controls.Add(this.payorCountry);
            this.panelTransportAccount.Controls.Add(this.labelPayorCountry);
            this.panelTransportAccount.Controls.Add(this.payorPostalCode);
            this.panelTransportAccount.Controls.Add(this.labelPayorPostalCode);
            this.panelTransportAccount.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTransportAccount.Location = new System.Drawing.Point(0, 29);
            this.panelTransportAccount.Name = "panelTransportAccount";
            this.panelTransportAccount.Size = new System.Drawing.Size(483, 115);
            this.panelTransportAccount.TabIndex = 5;
            // 
            // payorTransportName
            // 
            this.payorTransportName.Location = new System.Drawing.Point(123, 35);
            this.payorTransportName.Name = "payorTransportName";
            this.payorTransportName.Size = new System.Drawing.Size(173, 21);
            this.payorTransportName.TabIndex = 5;
            // 
            // labelTransportPayorName
            // 
            this.labelTransportPayorName.Location = new System.Drawing.Point(71, 33);
            this.labelTransportPayorName.Name = "labelTransportPayorName";
            this.labelTransportPayorName.Size = new System.Drawing.Size(46, 23);
            this.labelTransportPayorName.TabIndex = 4;
            this.labelTransportPayorName.Text = "Name:";
            this.labelTransportPayorName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // transportAccount
            // 
            this.transportAccount.Location = new System.Drawing.Point(123, 6);
            this.fieldLengthProvider.SetMaxLengthSource(this.transportAccount, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExAccountNumber);
            this.transportAccount.Name = "transportAccount";
            this.transportAccount.Size = new System.Drawing.Size(173, 21);
            this.transportAccount.TabIndex = 3;
            // 
            // labelTransportAccount
            // 
            this.labelTransportAccount.AutoSize = true;
            this.labelTransportAccount.BackColor = System.Drawing.Color.White;
            this.labelTransportAccount.Location = new System.Drawing.Point(58, 9);
            this.labelTransportAccount.Name = "labelTransportAccount";
            this.labelTransportAccount.Size = new System.Drawing.Size(61, 13);
            this.labelTransportAccount.TabIndex = 2;
            this.labelTransportAccount.Text = "Account #:";
            // 
            // payorCountry
            // 
            this.payorCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.payorCountry.FormattingEnabled = true;
            this.payorCountry.Location = new System.Drawing.Point(123, 64);
            this.payorCountry.MaxDropDownItems = 20;
            this.payorCountry.Name = "payorCountry";
            this.payorCountry.PromptText = "(Multiple Values)";
            this.payorCountry.Size = new System.Drawing.Size(173, 21);
            this.payorCountry.TabIndex = 6;
            // 
            // labelPayorCountry
            // 
            this.labelPayorCountry.AutoSize = true;
            this.labelPayorCountry.Location = new System.Drawing.Point(36, 67);
            this.labelPayorCountry.Name = "labelPayorCountry";
            this.labelPayorCountry.Size = new System.Drawing.Size(81, 13);
            this.labelPayorCountry.TabIndex = 7;
            this.labelPayorCountry.Text = "Payor Country:";
            // 
            // payorPostalCode
            // 
            this.payorPostalCode.Location = new System.Drawing.Point(123, 93);
            this.fieldLengthProvider.SetMaxLengthSource(this.payorPostalCode, ShipWorks.Data.Utility.EntityFieldLengthSource.PersonPostal);
            this.payorPostalCode.Name = "payorPostalCode";
            this.payorPostalCode.Size = new System.Drawing.Size(173, 21);
            this.payorPostalCode.TabIndex = 7;
            // 
            // labelPayorPostalCode
            // 
            this.labelPayorPostalCode.AutoSize = true;
            this.labelPayorPostalCode.Location = new System.Drawing.Point(18, 96);
            this.labelPayorPostalCode.Name = "labelPayorPostalCode";
            this.labelPayorPostalCode.Size = new System.Drawing.Size(99, 13);
            this.labelPayorPostalCode.TabIndex = 8;
            this.labelPayorPostalCode.Text = "Payor Postal Code:";
            // 
            // panelPayorTransport
            // 
            this.panelPayorTransport.BackColor = System.Drawing.Color.White;
            this.panelPayorTransport.Controls.Add(this.payorTransport);
            this.panelPayorTransport.Controls.Add(this.labelPayorTransport);
            this.panelPayorTransport.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPayorTransport.Location = new System.Drawing.Point(0, 0);
            this.panelPayorTransport.Name = "panelPayorTransport";
            this.panelPayorTransport.Size = new System.Drawing.Size(483, 29);
            this.panelPayorTransport.TabIndex = 4;
            // 
            // payorTransport
            // 
            this.payorTransport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.payorTransport.FormattingEnabled = true;
            this.payorTransport.Location = new System.Drawing.Point(123, 8);
            this.payorTransport.Name = "payorTransport";
            this.payorTransport.PromptText = "(Multiple Values)";
            this.payorTransport.Size = new System.Drawing.Size(173, 21);
            this.payorTransport.TabIndex = 3;
            this.payorTransport.SelectedIndexChanged += new System.EventHandler(this.OnChangePayorTransport);
            // 
            // labelPayorTransport
            // 
            this.labelPayorTransport.AutoSize = true;
            this.labelPayorTransport.BackColor = System.Drawing.Color.White;
            this.labelPayorTransport.Location = new System.Drawing.Point(9, 11);
            this.labelPayorTransport.Name = "labelPayorTransport";
            this.labelPayorTransport.Size = new System.Drawing.Size(108, 13);
            this.labelPayorTransport.TabIndex = 0;
            this.labelPayorTransport.Text = "Bill transportation to:";
            // 
            // panelDeliveredDutiesPaid
            // 
            this.panelDeliveredDutiesPaid.AutoSize = true;
            this.panelDeliveredDutiesPaid.BackColor = System.Drawing.Color.White;
            this.panelDeliveredDutiesPaid.Controls.Add(this.deliveredDutyPaid);
            this.panelDeliveredDutiesPaid.Controls.Add(this.labelDeliveredDutyPaid);
            this.panelDeliveredDutiesPaid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDeliveredDutiesPaid.Location = new System.Drawing.Point(0, -27);
            this.panelDeliveredDutiesPaid.Name = "panelDeliveredDutiesPaid";
            this.panelDeliveredDutiesPaid.Size = new System.Drawing.Size(483, 27);
            this.panelDeliveredDutiesPaid.TabIndex = 6;
            // 
            // deliveredDutyPaid
            // 
            this.deliveredDutyPaid.AutoSize = true;
            this.deliveredDutyPaid.Location = new System.Drawing.Point(125, 7);
            this.deliveredDutyPaid.Name = "deliveredDutyPaid";
            this.deliveredDutyPaid.Size = new System.Drawing.Size(127, 17);
            this.deliveredDutyPaid.TabIndex = 24;
            this.deliveredDutyPaid.Text = "Delivered Duties Paid";
            this.deliveredDutyPaid.UseVisualStyleBackColor = true;
            // 
            // labelDeliveredDutyPaid
            // 
            this.labelDeliveredDutyPaid.AutoSize = true;
            this.labelDeliveredDutyPaid.BackColor = System.Drawing.Color.Transparent;
            this.labelDeliveredDutyPaid.Location = new System.Drawing.Point(87, 7);
            this.labelDeliveredDutyPaid.Name = "labelDeliveredDutyPaid";
            this.labelDeliveredDutyPaid.Size = new System.Drawing.Size(31, 13);
            this.labelDeliveredDutyPaid.TabIndex = 25;
            this.labelDeliveredDutyPaid.Text = "DDP:";
            // 
            // panelPayorDuties
            // 
            this.panelPayorDuties.BackColor = System.Drawing.Color.White;
            this.panelPayorDuties.Controls.Add(this.dutiesAccount);
            this.panelPayorDuties.Controls.Add(this.labelDutiesAccount);
            this.panelPayorDuties.Controls.Add(this.payorDuties);
            this.panelPayorDuties.Controls.Add(this.labelPayorDuties);
            this.panelPayorDuties.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPayorDuties.Location = new System.Drawing.Point(0, 90);
            this.panelPayorDuties.Name = "panelPayorDuties";
            this.panelPayorDuties.Size = new System.Drawing.Size(483, 65);
            this.panelPayorDuties.TabIndex = 6;
            this.panelPayorDuties.Visible = false;
            // 
            // dutiesAccount
            // 
            this.dutiesAccount.Location = new System.Drawing.Point(123, 36);
            this.fieldLengthProvider.SetMaxLengthSource(this.dutiesAccount, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExAccountNumber);
            this.dutiesAccount.Name = "dutiesAccount";
            this.dutiesAccount.Size = new System.Drawing.Size(173, 20);
            this.dutiesAccount.TabIndex = 6;
            // 
            // labelDutiesAccount
            // 
            this.labelDutiesAccount.AutoSize = true;
            this.labelDutiesAccount.BackColor = System.Drawing.Color.White;
            this.labelDutiesAccount.Location = new System.Drawing.Point(56, 39);
            this.labelDutiesAccount.Name = "labelDutiesAccount";
            this.labelDutiesAccount.Size = new System.Drawing.Size(60, 13);
            this.labelDutiesAccount.TabIndex = 5;
            this.labelDutiesAccount.Text = "Account #:";
            // 
            // payorDuties
            // 
            this.payorDuties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.payorDuties.FormattingEnabled = true;
            this.payorDuties.Location = new System.Drawing.Point(123, 9);
            this.payorDuties.Name = "payorDuties";
            this.payorDuties.PromptText = "(Multiple Values)";
            this.payorDuties.Size = new System.Drawing.Size(173, 21);
            this.payorDuties.TabIndex = 4;
            this.payorDuties.SelectedIndexChanged += new System.EventHandler(this.OnChangePayorDuties);
            // 
            // labelPayorDuties
            // 
            this.labelPayorDuties.AutoSize = true;
            this.labelPayorDuties.BackColor = System.Drawing.Color.White;
            this.labelPayorDuties.Location = new System.Drawing.Point(24, 12);
            this.labelPayorDuties.Name = "labelPayorDuties";
            this.labelPayorDuties.Size = new System.Drawing.Size(91, 13);
            this.labelPayorDuties.TabIndex = 1;
            this.labelPayorDuties.Text = "Bill duties/fees to:";
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
            this.sectionFrom.ContentPanel.Controls.Add(this.panelTop);
            this.sectionFrom.ContentPanel.Controls.Add(this.labelFromResidentialCommercial);
            this.sectionFrom.ContentPanel.Controls.Add(this.labelFromAddressType);
            this.sectionFrom.ContentPanel.Controls.Add(this.fromAddressType);
            this.sectionFrom.ExpandedHeight = 563;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(487, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(0, 52);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(482, 427);
            this.originControl.TabIndex = 1;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.fedexAccount);
            this.panelTop.Location = new System.Drawing.Point(3, 1);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(479, 50);
            this.panelTop.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Account:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "FedEx Account";
            // 
            // fedexAccount
            // 
            this.fedexAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fedexAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fedexAccount.FormattingEnabled = true;
            this.fedexAccount.Location = new System.Drawing.Point(79, 25);
            this.fedexAccount.Name = "fedexAccount";
            this.fedexAccount.PromptText = "(Multiple Values)";
            this.fedexAccount.Size = new System.Drawing.Size(386, 21);
            this.fedexAccount.TabIndex = 0;
            this.fedexAccount.SelectedIndexChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // labelFromResidentialCommercial
            // 
            this.labelFromResidentialCommercial.AutoSize = true;
            this.labelFromResidentialCommercial.BackColor = System.Drawing.Color.Transparent;
            this.labelFromResidentialCommercial.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFromResidentialCommercial.Location = new System.Drawing.Point(6, 483);
            this.labelFromResidentialCommercial.Name = "labelFromResidentialCommercial";
            this.labelFromResidentialCommercial.Size = new System.Drawing.Size(149, 13);
            this.labelFromResidentialCommercial.TabIndex = 2;
            this.labelFromResidentialCommercial.Text = "Residential \\ Commercial";
            // 
            // labelFromAddressType
            // 
            this.labelFromAddressType.AutoSize = true;
            this.labelFromAddressType.BackColor = System.Drawing.Color.Transparent;
            this.labelFromAddressType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.labelFromAddressType.Location = new System.Drawing.Point(26, 506);
            this.labelFromAddressType.Name = "labelFromAddressType";
            this.labelFromAddressType.Size = new System.Drawing.Size(50, 13);
            this.labelFromAddressType.TabIndex = 3;
            this.labelFromAddressType.Text = "Address:";
            // 
            // fromAddressType
            // 
            this.fromAddressType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fromAddressType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fromAddressType.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.fromAddressType.FormattingEnabled = true;
            this.fromAddressType.Location = new System.Drawing.Point(82, 503);
            this.fromAddressType.Name = "fromAddressType";
            this.fromAddressType.PromptText = "(Multiple Values)";
            this.fromAddressType.Size = new System.Drawing.Size(387, 21);
            this.fromAddressType.TabIndex = 2;
            this.fromAddressType.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // nonStandardPackaging
            // 
            this.nonStandardPackaging.AutoSize = true;
            this.nonStandardPackaging.BackColor = System.Drawing.Color.White;
            this.nonStandardPackaging.Location = new System.Drawing.Point(245, 63);
            this.nonStandardPackaging.Name = "nonStandardPackaging";
            this.nonStandardPackaging.Size = new System.Drawing.Size(92, 17);
            this.nonStandardPackaging.TabIndex = 7;
            this.nonStandardPackaging.Text = "Non-standard";
            this.nonStandardPackaging.UseVisualStyleBackColor = false;
            this.nonStandardPackaging.CheckedChanged += new System.EventHandler(this.OnNonStandardPackagingChanged);
            this.nonStandardPackaging.CheckStateChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // saturdayDelivery
            // 
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.BackColor = System.Drawing.Color.White;
            this.saturdayDelivery.Location = new System.Drawing.Point(245, 38);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 4;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = false;
            this.saturdayDelivery.CheckStateChanged += new System.EventHandler(this.OnSaturdayDeliveryChanged);
            // 
            // packageControl
            // 
            this.packageControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageControl.BackColor = System.Drawing.Color.White;
            this.packageControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packageControl.Location = new System.Drawing.Point(-26, 86);
            this.packageControl.Name = "packageControl";
            this.packageControl.PackageCountChanged = null;
            this.packageControl.Size = new System.Drawing.Size(1842, 246);
            this.packageControl.TabIndex = 8;
            this.packageControl.RateCriteriaChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.packageControl.SizeChanged += new System.EventHandler(this.OnPackageControlSizeChanged);
            // 
            // packagingType
            // 
            this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagingType.FormattingEnabled = true;
            this.packagingType.Location = new System.Drawing.Point(95, 61);
            this.packagingType.Name = "packagingType";
            this.packagingType.PromptText = "(Multiple Values)";
            this.packagingType.Size = new System.Drawing.Size(147, 21);
            this.packagingType.TabIndex = 6;
            this.packagingType.SelectedIndexChanged += new System.EventHandler(this.OnChangePackaging);
            // 
            // labelPackaging
            // 
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(30, 64);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 5;
            this.labelPackaging.Text = "Packaging:";
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.DropDownWidth = 360;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(95, 8);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(250, 21);
            this.service.TabIndex = 1;
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(43, 11);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 0;
            this.labelService.Text = "Service:";
            // 
            // labelShipDate
            // 
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(33, 39);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 2;
            this.labelShipDate.Text = "Ship date:";
            // 
            // shipDate
            // 
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(95, 35);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(147, 21);
            this.shipDate.TabIndex = 3;
            this.shipDate.ValueChanged += new System.EventHandler(this.OnChangeShipDate);
            // 
            // sectionHomeDelivery
            // 
            this.sectionHomeDelivery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionHomeDelivery.Collapsed = true;
            // 
            // sectionHomeDelivery.ContentPanel
            // 
            this.sectionHomeDelivery.ContentPanel.Controls.Add(this.homePremiumPhone);
            this.sectionHomeDelivery.ContentPanel.Controls.Add(this.labelHomePremiumPhone);
            this.sectionHomeDelivery.ContentPanel.Controls.Add(this.homeInstructions);
            this.sectionHomeDelivery.ContentPanel.Controls.Add(this.homePremiumDate);
            this.sectionHomeDelivery.ContentPanel.Controls.Add(this.homePremiumService);
            this.sectionHomeDelivery.ContentPanel.Controls.Add(this.labelPremium);
            this.sectionHomeDelivery.ContentPanel.Controls.Add(this.labelHomeInstructions);
            this.sectionHomeDelivery.ExpandedHeight = 144;
            this.sectionHomeDelivery.ExtraText = "";
            this.sectionHomeDelivery.Location = new System.Drawing.Point(3, 584);
            this.sectionHomeDelivery.Name = "sectionHomeDelivery";
            this.sectionHomeDelivery.SectionName = "Home Delivery";
            this.sectionHomeDelivery.SettingsKey = "{93c6c394-09fb-4126-9c2e-a5129d9b5ec6}";
            this.sectionHomeDelivery.Size = new System.Drawing.Size(487, 24);
            this.sectionHomeDelivery.TabIndex = 8;
            // 
            // homePremiumPhone
            // 
            this.homePremiumPhone.Location = new System.Drawing.Point(109, 36);
            this.fieldLengthProvider.SetMaxLengthSource(this.homePremiumPhone, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExHomeDeliveryPhone);
            this.homePremiumPhone.Name = "homePremiumPhone";
            this.homePremiumPhone.Size = new System.Drawing.Size(129, 21);
            this.homePremiumPhone.TabIndex = 2;
            // 
            // labelHomePremiumPhone
            // 
            this.labelHomePremiumPhone.AutoSize = true;
            this.labelHomePremiumPhone.BackColor = System.Drawing.Color.White;
            this.labelHomePremiumPhone.Location = new System.Drawing.Point(62, 39);
            this.labelHomePremiumPhone.Name = "labelHomePremiumPhone";
            this.labelHomePremiumPhone.Size = new System.Drawing.Size(41, 13);
            this.labelHomePremiumPhone.TabIndex = 65;
            this.labelHomePremiumPhone.Text = "Phone:";
            // 
            // homeInstructions
            // 
            this.homeInstructions.Location = new System.Drawing.Point(109, 62);
            this.fieldLengthProvider.SetMaxLengthSource(this.homeInstructions, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExHomeDeliveryInstructions);
            this.homeInstructions.Multiline = true;
            this.homeInstructions.Name = "homeInstructions";
            this.homeInstructions.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.homeInstructions.Size = new System.Drawing.Size(204, 47);
            this.homeInstructions.TabIndex = 3;
            // 
            // homePremiumDate
            // 
            this.homePremiumDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.homePremiumDate.Location = new System.Drawing.Point(210, 11);
            this.homePremiumDate.Name = "homePremiumDate";
            this.homePremiumDate.Size = new System.Drawing.Size(103, 21);
            this.homePremiumDate.TabIndex = 1;
            // 
            // homePremiumService
            // 
            this.homePremiumService.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.homePremiumService.FormattingEnabled = true;
            this.homePremiumService.Location = new System.Drawing.Point(109, 11);
            this.homePremiumService.Name = "homePremiumService";
            this.homePremiumService.PromptText = "(Multiple Values)";
            this.homePremiumService.Size = new System.Drawing.Size(95, 21);
            this.homePremiumService.TabIndex = 0;
            this.homePremiumService.SelectedIndexChanged += new System.EventHandler(this.OnChangeHomePremiumService);
            // 
            // labelPremium
            // 
            this.labelPremium.AutoSize = true;
            this.labelPremium.BackColor = System.Drawing.Color.White;
            this.labelPremium.Location = new System.Drawing.Point(14, 14);
            this.labelPremium.Name = "labelPremium";
            this.labelPremium.Size = new System.Drawing.Size(89, 13);
            this.labelPremium.TabIndex = 2;
            this.labelPremium.Text = "Premium Service:";
            // 
            // labelHomeInstructions
            // 
            this.labelHomeInstructions.AutoSize = true;
            this.labelHomeInstructions.BackColor = System.Drawing.Color.White;
            this.labelHomeInstructions.Location = new System.Drawing.Point(35, 62);
            this.labelHomeInstructions.Name = "labelHomeInstructions";
            this.labelHomeInstructions.Size = new System.Drawing.Size(68, 13);
            this.labelHomeInstructions.TabIndex = 1;
            this.labelHomeInstructions.Text = "Instructions:";
            // 
            // sectionFreight
            // 
            this.sectionFreight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionFreight.ContentPanel
            // 
            this.sectionFreight.ContentPanel.Controls.Add(this.fedExFreightContainerControl);
            this.sectionFreight.ExtraText = "";
            this.sectionFreight.Location = new System.Drawing.Point(3, 613);
            this.sectionFreight.Name = "sectionFreight";
            this.sectionFreight.SectionName = "Freight";
            this.sectionFreight.SettingsKey = "{A7947F0F-9648-4443-A678-F67E7FCEE38C}";
            this.sectionFreight.Size = new System.Drawing.Size(487, 88);
            this.sectionFreight.TabIndex = 9;
            // 
            // fedExFreightContainerControl
            // 
            this.fedExFreightContainerControl.BackColor = System.Drawing.SystemColors.Window;
            this.fedExFreightContainerControl.Location = new System.Drawing.Point(7, 5);
            this.fedExFreightContainerControl.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.fedExFreightContainerControl.Name = "fedExFreightContainerControl";
            this.fedExFreightContainerControl.Size = new System.Drawing.Size(469, 50);
            this.fedExFreightContainerControl.TabIndex = 7;
            this.fedExFreightContainerControl.Resize += OnFreightContainerControlResize;
            // 
            // sectionOptions
            // 
            this.sectionOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionOptions.Collapsed = true;
            // 
            // sectionOptions.ContentPanel
            // 
            this.sectionOptions.ContentPanel.Controls.Add(this.referencePO);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelPO);
            this.sectionOptions.ContentPanel.Controls.Add(this.referenceInvoice);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelInvoice);
            this.sectionOptions.ContentPanel.Controls.Add(this.referenceCustomer);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelReference);
            this.sectionOptions.ContentPanel.Controls.Add(this.signature);
            this.sectionOptions.ContentPanel.Controls.Add(this.labelSignature);
            this.sectionOptions.ExpandedHeight = 140;
            this.sectionOptions.ExtraText = "";
            this.sectionOptions.Location = new System.Drawing.Point(3, 468);
            this.sectionOptions.Name = "sectionOptions";
            this.sectionOptions.SectionName = "FedEx® Delivery Signature Options & Reference";
            this.sectionOptions.SettingsKey = "{2740f860-1d14-453e-a511-8f62ad1e7dcc}";
            this.sectionOptions.Size = new System.Drawing.Size(487, 24);
            this.sectionOptions.TabIndex = 4;
            // 
            // referencePO
            // 
            this.referencePO.Location = new System.Drawing.Point(211, 89);
            this.referencePO.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referencePO, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExReferencePO);
            this.referencePO.Name = "referencePO";
            this.referencePO.Size = new System.Drawing.Size(210, 21);
            this.referencePO.TabIndex = 7;
            this.referencePO.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // labelPO
            // 
            this.labelPO.AutoSize = true;
            this.labelPO.BackColor = System.Drawing.Color.Transparent;
            this.labelPO.Location = new System.Drawing.Point(161, 91);
            this.labelPO.Name = "labelPO";
            this.labelPO.Size = new System.Drawing.Size(44, 13);
            this.labelPO.TabIndex = 6;
            this.labelPO.Text = "P.O. #:";
            // 
            // referenceInvoice
            // 
            this.referenceInvoice.Location = new System.Drawing.Point(211, 62);
            this.referenceInvoice.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceInvoice, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExReferenceInvoice);
            this.referenceInvoice.Name = "referenceInvoice";
            this.referenceInvoice.Size = new System.Drawing.Size(210, 21);
            this.referenceInvoice.TabIndex = 5;
            this.referenceInvoice.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            // 
            // labelInvoice
            // 
            this.labelInvoice.AutoSize = true;
            this.labelInvoice.BackColor = System.Drawing.Color.Transparent;
            this.labelInvoice.Location = new System.Drawing.Point(148, 64);
            this.labelInvoice.Name = "labelInvoice";
            this.labelInvoice.Size = new System.Drawing.Size(57, 13);
            this.labelInvoice.TabIndex = 4;
            this.labelInvoice.Text = "Invoice #:";
            // 
            // referenceCustomer
            // 
            this.referenceCustomer.Location = new System.Drawing.Point(211, 35);
            this.referenceCustomer.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceCustomer, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExReferenceCustomer);
            this.referenceCustomer.Name = "referenceCustomer";
            this.referenceCustomer.Size = new System.Drawing.Size(210, 21);
            this.referenceCustomer.TabIndex = 3;
            this.referenceCustomer.TokenSuggestionFactory = commonTokenSuggestionsFactory3;
            // 
            // labelReference
            // 
            this.labelReference.AutoSize = true;
            this.labelReference.BackColor = System.Drawing.Color.Transparent;
            this.labelReference.Location = new System.Drawing.Point(133, 37);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(72, 13);
            this.labelReference.TabIndex = 2;
            this.labelReference.Text = "Reference #:";
            // 
            // signature
            // 
            this.signature.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.signature.FormattingEnabled = true;
            this.signature.Location = new System.Drawing.Point(211, 8);
            this.signature.Name = "signature";
            this.signature.PromptText = "(Multiple Values)";
            this.signature.Size = new System.Drawing.Size(175, 21);
            this.signature.TabIndex = 1;
            this.signature.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // labelSignature
            // 
            this.labelSignature.AutoSize = true;
            this.labelSignature.BackColor = System.Drawing.Color.Transparent;
            this.labelSignature.Location = new System.Drawing.Point(23, 11);
            this.labelSignature.Name = "labelSignature";
            this.labelSignature.Size = new System.Drawing.Size(182, 13);
            this.labelSignature.TabIndex = 0;
            this.labelSignature.Text = "FedEx® Delivery Signature Options:";
            // 
            // referenceShipmentIntegrity
            // 
            this.referenceShipmentIntegrity.Location = new System.Drawing.Point(86, 116);
            this.referenceShipmentIntegrity.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceShipmentIntegrity, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExReferenceShipmentIntegrity);
            this.referenceShipmentIntegrity.Name = "referenceShipmentIntegrity";
            this.referenceShipmentIntegrity.Size = new System.Drawing.Size(210, 21);
            this.referenceShipmentIntegrity.TabIndex = 9;
            this.referenceShipmentIntegrity.TokenSuggestionFactory = commonTokenSuggestionsFactory4;
            // 
            // labelShipmentIntegrity
            // 
            this.labelShipmentIntegrity.AutoSize = true;
            this.labelShipmentIntegrity.BackColor = System.Drawing.Color.Transparent;
            this.labelShipmentIntegrity.Location = new System.Drawing.Point(27, 118);
            this.labelShipmentIntegrity.Name = "labelShipmentIntegrity";
            this.labelShipmentIntegrity.Size = new System.Drawing.Size(53, 13);
            this.labelShipmentIntegrity.TabIndex = 8;
            this.labelShipmentIntegrity.Text = "Integrity:";
            // 
            // sectionEmail
            // 
            this.sectionEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionEmail.Collapsed = true;
            // 
            // sectionEmail.ContentPanel
            // 
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyBrokerShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyBrokerDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyBrokerEstimatedDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyBrokerException);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailBroker);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyMessage);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelPersonalMessage);
            this.sectionEmail.ContentPanel.Controls.Add(this.kryptonBorderEdgeEmail2);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherEstimatedDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherException);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyRecipientShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyRecipientDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyRecipientEstimatedDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyRecipientException);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherAddress);
            this.sectionEmail.ContentPanel.Controls.Add(this.kryptonBorderEdgeEmail);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailOther);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailRecipient);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailSender);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifySenderShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifySenderDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifySenderEstimatedDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifySenderException);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailEstimatedDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailException);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailAddress);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailInfo);
            this.sectionEmail.ContentPanel.Controls.Add(this.picturEmailInfo);
            this.sectionEmail.ContentPanel.Controls.Add(this.label4);
            this.sectionEmail.ContentPanel.Controls.Add(this.kryptonBorderEdge1);
            this.sectionEmail.ContentPanel.Controls.Add(this.kryptonBorderEdge2);
            this.sectionEmail.ContentPanel.Controls.Add(this.label5);
            this.sectionEmail.ContentPanel.Controls.Add(this.label6);
            this.sectionEmail.ContentPanel.Controls.Add(this.label7);
            this.sectionEmail.ContentPanel.Controls.Add(this.label8);
            this.sectionEmail.ContentPanel.Controls.Add(this.label9);
            this.sectionEmail.ContentPanel.Controls.Add(this.label10);
            this.sectionEmail.ContentPanel.Controls.Add(this.label11);
            this.sectionEmail.ContentPanel.Controls.Add(this.label12);
            this.sectionEmail.ContentPanel.Controls.Add(this.pictureBox1);
            this.sectionEmail.ExpandedHeight = 277;
            this.sectionEmail.ExtraText = "";
            this.sectionEmail.Location = new System.Drawing.Point(3, 555);
            this.sectionEmail.Name = "sectionEmail";
            this.sectionEmail.SectionName = "Email Notifications";
            this.sectionEmail.SettingsKey = "{2a314180-f0db-4a03-ba4c-dcc418010bca}";
            this.sectionEmail.Size = new System.Drawing.Size(487, 24);
            this.sectionEmail.TabIndex = 7;
            // 
            // emailNotifyBrokerShip
            // 
            this.emailNotifyBrokerShip.AutoSize = true;
            this.emailNotifyBrokerShip.BackColor = System.Drawing.Color.White;
            this.emailNotifyBrokerShip.Location = new System.Drawing.Point(221, 108);
            this.emailNotifyBrokerShip.Name = "emailNotifyBrokerShip";
            this.emailNotifyBrokerShip.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyBrokerShip.TabIndex = 7;
            this.emailNotifyBrokerShip.UseVisualStyleBackColor = false;
            // 
            // emailNotifyBrokerDelivery
            // 
            this.emailNotifyBrokerDelivery.AutoSize = true;
            this.emailNotifyBrokerDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyBrokerDelivery.Location = new System.Drawing.Point(321, 108);
            this.emailNotifyBrokerDelivery.Name = "emailNotifyBrokerDelivery";
            this.emailNotifyBrokerDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyBrokerDelivery.TabIndex = 9;
            this.emailNotifyBrokerDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyBrokerEstimatedDelivery
            // 
            this.emailNotifyBrokerEstimatedDelivery.AutoSize = true;
            this.emailNotifyBrokerEstimatedDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyBrokerEstimatedDelivery.Location = new System.Drawing.Point(395, 108);
            this.emailNotifyBrokerEstimatedDelivery.Name = "emailNotifyBrokerEstimatedDelivery";
            this.emailNotifyBrokerEstimatedDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyBrokerEstimatedDelivery.TabIndex = 10;
            this.emailNotifyBrokerEstimatedDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyBrokerException
            // 
            this.emailNotifyBrokerException.AutoSize = true;
            this.emailNotifyBrokerException.BackColor = System.Drawing.Color.White;
            this.emailNotifyBrokerException.Location = new System.Drawing.Point(264, 108);
            this.emailNotifyBrokerException.Name = "emailNotifyBrokerException";
            this.emailNotifyBrokerException.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyBrokerException.TabIndex = 8;
            this.emailNotifyBrokerException.UseVisualStyleBackColor = false;
            // 
            // labelEmailBroker
            // 
            this.labelEmailBroker.AutoSize = true;
            this.labelEmailBroker.BackColor = System.Drawing.Color.White;
            this.labelEmailBroker.Location = new System.Drawing.Point(14, 108);
            this.labelEmailBroker.Name = "labelEmailBroker";
            this.labelEmailBroker.Size = new System.Drawing.Size(38, 13);
            this.labelEmailBroker.TabIndex = 26;
            this.labelEmailBroker.Text = "Broker";
            // 
            // emailNotifyMessage
            // 
            this.emailNotifyMessage.Location = new System.Drawing.Point(17, 178);
            this.emailNotifyMessage.MaxLength = 120;
            this.fieldLengthProvider.SetMaxLengthSource(this.emailNotifyMessage, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExEmailNotifyMessage);
            this.emailNotifyMessage.Multiline = true;
            this.emailNotifyMessage.Name = "emailNotifyMessage";
            this.emailNotifyMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.emailNotifyMessage.Size = new System.Drawing.Size(319, 61);
            this.emailNotifyMessage.TabIndex = 16;
            // 
            // labelPersonalMessage
            // 
            this.labelPersonalMessage.AutoSize = true;
            this.labelPersonalMessage.BackColor = System.Drawing.Color.White;
            this.labelPersonalMessage.Location = new System.Drawing.Point(6, 162);
            this.labelPersonalMessage.Name = "labelPersonalMessage";
            this.labelPersonalMessage.Size = new System.Drawing.Size(142, 13);
            this.labelPersonalMessage.TabIndex = 22;
            this.labelPersonalMessage.Text = "Personal Message (Optional)";
            // 
            // kryptonBorderEdgeEmail2
            // 
            this.kryptonBorderEdgeEmail2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdgeEmail2.AutoSize = false;
            this.kryptonBorderEdgeEmail2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdgeEmail2.Location = new System.Drawing.Point(9, 158);
            this.kryptonBorderEdgeEmail2.Name = "kryptonBorderEdgeEmail2";
            this.kryptonBorderEdgeEmail2.Size = new System.Drawing.Size(385, 1);
            this.kryptonBorderEdgeEmail2.Text = "kryptonBorderEdge1";
            // 
            // emailNotifyOtherShip
            // 
            this.emailNotifyOtherShip.AutoSize = true;
            this.emailNotifyOtherShip.BackColor = System.Drawing.Color.White;
            this.emailNotifyOtherShip.Location = new System.Drawing.Point(221, 131);
            this.emailNotifyOtherShip.Name = "emailNotifyOtherShip";
            this.emailNotifyOtherShip.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyOtherShip.TabIndex = 12;
            this.emailNotifyOtherShip.UseVisualStyleBackColor = false;
            // 
            // emailNotifyOtherDelivery
            // 
            this.emailNotifyOtherDelivery.AutoSize = true;
            this.emailNotifyOtherDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyOtherDelivery.Location = new System.Drawing.Point(321, 131);
            this.emailNotifyOtherDelivery.Name = "emailNotifyOtherDelivery";
            this.emailNotifyOtherDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyOtherDelivery.TabIndex = 14;
            this.emailNotifyOtherDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyOtherEstimatedDelivery
            // 
            this.emailNotifyOtherEstimatedDelivery.AutoSize = true;
            this.emailNotifyOtherEstimatedDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyOtherEstimatedDelivery.Location = new System.Drawing.Point(395, 131);
            this.emailNotifyOtherEstimatedDelivery.Name = "emailNotifyOtherEstimatedDelivery";
            this.emailNotifyOtherEstimatedDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyOtherEstimatedDelivery.TabIndex = 15;
            this.emailNotifyOtherEstimatedDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyOtherException
            // 
            this.emailNotifyOtherException.AutoSize = true;
            this.emailNotifyOtherException.BackColor = System.Drawing.Color.White;
            this.emailNotifyOtherException.Location = new System.Drawing.Point(264, 131);
            this.emailNotifyOtherException.Name = "emailNotifyOtherException";
            this.emailNotifyOtherException.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyOtherException.TabIndex = 13;
            this.emailNotifyOtherException.UseVisualStyleBackColor = false;
            // 
            // emailNotifyRecipientShip
            // 
            this.emailNotifyRecipientShip.AutoSize = true;
            this.emailNotifyRecipientShip.BackColor = System.Drawing.Color.White;
            this.emailNotifyRecipientShip.Location = new System.Drawing.Point(221, 87);
            this.emailNotifyRecipientShip.Name = "emailNotifyRecipientShip";
            this.emailNotifyRecipientShip.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyRecipientShip.TabIndex = 3;
            this.emailNotifyRecipientShip.UseVisualStyleBackColor = false;
            // 
            // emailNotifyRecipientDelivery
            // 
            this.emailNotifyRecipientDelivery.AutoSize = true;
            this.emailNotifyRecipientDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyRecipientDelivery.Location = new System.Drawing.Point(321, 87);
            this.emailNotifyRecipientDelivery.Name = "emailNotifyRecipientDelivery";
            this.emailNotifyRecipientDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyRecipientDelivery.TabIndex = 5;
            this.emailNotifyRecipientDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyRecipientEstimatedDelivery
            // 
            this.emailNotifyRecipientEstimatedDelivery.AutoSize = true;
            this.emailNotifyRecipientEstimatedDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyRecipientEstimatedDelivery.Location = new System.Drawing.Point(395, 87);
            this.emailNotifyRecipientEstimatedDelivery.Name = "emailNotifyRecipientEstimatedDelivery";
            this.emailNotifyRecipientEstimatedDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyRecipientEstimatedDelivery.TabIndex = 5;
            this.emailNotifyRecipientEstimatedDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyRecipientException
            // 
            this.emailNotifyRecipientException.AutoSize = true;
            this.emailNotifyRecipientException.BackColor = System.Drawing.Color.White;
            this.emailNotifyRecipientException.Location = new System.Drawing.Point(264, 87);
            this.emailNotifyRecipientException.Name = "emailNotifyRecipientException";
            this.emailNotifyRecipientException.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyRecipientException.TabIndex = 4;
            this.emailNotifyRecipientException.UseVisualStyleBackColor = false;
            // 
            // emailNotifyOtherAddress
            // 
            this.emailNotifyOtherAddress.Location = new System.Drawing.Point(49, 128);
            this.fieldLengthProvider.SetMaxLengthSource(this.emailNotifyOtherAddress, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExEmailNotifyOtherAddress);
            this.emailNotifyOtherAddress.Name = "emailNotifyOtherAddress";
            this.emailNotifyOtherAddress.Size = new System.Drawing.Size(154, 20);
            this.emailNotifyOtherAddress.TabIndex = 11;
            // 
            // kryptonBorderEdgeEmail
            // 
            this.kryptonBorderEdgeEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdgeEmail.AutoSize = false;
            this.kryptonBorderEdgeEmail.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdgeEmail.Location = new System.Drawing.Point(8, 59);
            this.kryptonBorderEdgeEmail.Name = "kryptonBorderEdgeEmail";
            this.kryptonBorderEdgeEmail.Size = new System.Drawing.Size(385, 1);
            this.kryptonBorderEdgeEmail.Text = "kryptonBorderEdge1";
            // 
            // labelEmailOther
            // 
            this.labelEmailOther.AutoSize = true;
            this.labelEmailOther.BackColor = System.Drawing.Color.White;
            this.labelEmailOther.Location = new System.Drawing.Point(14, 131);
            this.labelEmailOther.Name = "labelEmailOther";
            this.labelEmailOther.Size = new System.Drawing.Size(33, 13);
            this.labelEmailOther.TabIndex = 12;
            this.labelEmailOther.Text = "Other";
            // 
            // labelEmailRecipient
            // 
            this.labelEmailRecipient.AutoSize = true;
            this.labelEmailRecipient.BackColor = System.Drawing.Color.White;
            this.labelEmailRecipient.Location = new System.Drawing.Point(14, 87);
            this.labelEmailRecipient.Name = "labelEmailRecipient";
            this.labelEmailRecipient.Size = new System.Drawing.Size(52, 13);
            this.labelEmailRecipient.TabIndex = 10;
            this.labelEmailRecipient.Text = "Recipient";
            // 
            // labelEmailSender
            // 
            this.labelEmailSender.AutoSize = true;
            this.labelEmailSender.BackColor = System.Drawing.Color.White;
            this.labelEmailSender.Location = new System.Drawing.Point(14, 67);
            this.labelEmailSender.Name = "labelEmailSender";
            this.labelEmailSender.Size = new System.Drawing.Size(41, 13);
            this.labelEmailSender.TabIndex = 9;
            this.labelEmailSender.Text = "Sender";
            // 
            // emailNotifySenderShip
            // 
            this.emailNotifySenderShip.AutoSize = true;
            this.emailNotifySenderShip.BackColor = System.Drawing.Color.White;
            this.emailNotifySenderShip.Location = new System.Drawing.Point(221, 66);
            this.emailNotifySenderShip.Name = "emailNotifySenderShip";
            this.emailNotifySenderShip.Size = new System.Drawing.Size(15, 14);
            this.emailNotifySenderShip.TabIndex = 0;
            this.emailNotifySenderShip.UseVisualStyleBackColor = false;
            // 
            // emailNotifySenderDelivery
            // 
            this.emailNotifySenderDelivery.AutoSize = true;
            this.emailNotifySenderDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifySenderDelivery.Location = new System.Drawing.Point(321, 66);
            this.emailNotifySenderDelivery.Name = "emailNotifySenderDelivery";
            this.emailNotifySenderDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifySenderDelivery.TabIndex = 2;
            this.emailNotifySenderDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifySenderEstimatedDelivery
            // 
            this.emailNotifySenderEstimatedDelivery.AutoSize = true;
            this.emailNotifySenderEstimatedDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifySenderEstimatedDelivery.Location = new System.Drawing.Point(395, 66);
            this.emailNotifySenderEstimatedDelivery.Name = "emailNotifySenderEstimatedDelivery";
            this.emailNotifySenderEstimatedDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifySenderEstimatedDelivery.TabIndex = 2;
            this.emailNotifySenderEstimatedDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifySenderException
            // 
            this.emailNotifySenderException.AutoSize = true;
            this.emailNotifySenderException.BackColor = System.Drawing.Color.White;
            this.emailNotifySenderException.Location = new System.Drawing.Point(264, 66);
            this.emailNotifySenderException.Name = "emailNotifySenderException";
            this.emailNotifySenderException.Size = new System.Drawing.Size(15, 14);
            this.emailNotifySenderException.TabIndex = 1;
            this.emailNotifySenderException.UseVisualStyleBackColor = false;
            // 
            // labelEmailDelivery
            // 
            this.labelEmailDelivery.AutoSize = true;
            this.labelEmailDelivery.BackColor = System.Drawing.Color.White;
            this.labelEmailDelivery.Location = new System.Drawing.Point(302, 42);
            this.labelEmailDelivery.Name = "labelEmailDelivery";
            this.labelEmailDelivery.Size = new System.Drawing.Size(45, 13);
            this.labelEmailDelivery.TabIndex = 5;
            this.labelEmailDelivery.Text = "Delivery";
            // 
            // labelEmailEstimatedDelivery
            // 
            this.labelEmailEstimatedDelivery.AutoSize = true;
            this.labelEmailEstimatedDelivery.BackColor = System.Drawing.Color.White;
            this.labelEmailEstimatedDelivery.Location = new System.Drawing.Point(356, 42);
            this.labelEmailEstimatedDelivery.Name = "labelEmailEstimatedDelivery";
            this.labelEmailEstimatedDelivery.Size = new System.Drawing.Size(94, 13);
            this.labelEmailEstimatedDelivery.TabIndex = 5;
            this.labelEmailEstimatedDelivery.Text = "Estimated Delivery";
            // 
            // labelEmailException
            // 
            this.labelEmailException.AutoSize = true;
            this.labelEmailException.BackColor = System.Drawing.Color.White;
            this.labelEmailException.Location = new System.Drawing.Point(246, 42);
            this.labelEmailException.Name = "labelEmailException";
            this.labelEmailException.Size = new System.Drawing.Size(54, 13);
            this.labelEmailException.TabIndex = 4;
            this.labelEmailException.Text = "Exception";
            // 
            // labelEmailShip
            // 
            this.labelEmailShip.AutoSize = true;
            this.labelEmailShip.BackColor = System.Drawing.Color.White;
            this.labelEmailShip.Location = new System.Drawing.Point(215, 42);
            this.labelEmailShip.Name = "labelEmailShip";
            this.labelEmailShip.Size = new System.Drawing.Size(28, 13);
            this.labelEmailShip.TabIndex = 3;
            this.labelEmailShip.Text = "Ship";
            // 
            // labelEmailAddress
            // 
            this.labelEmailAddress.AutoSize = true;
            this.labelEmailAddress.BackColor = System.Drawing.Color.White;
            this.labelEmailAddress.Location = new System.Drawing.Point(6, 42);
            this.labelEmailAddress.Name = "labelEmailAddress";
            this.labelEmailAddress.Size = new System.Drawing.Size(73, 13);
            this.labelEmailAddress.TabIndex = 2;
            this.labelEmailAddress.Text = "Email Address";
            // 
            // labelEmailInfo
            // 
            this.labelEmailInfo.BackColor = System.Drawing.Color.White;
            this.labelEmailInfo.Location = new System.Drawing.Point(23, 7);
            this.labelEmailInfo.Name = "labelEmailInfo";
            this.labelEmailInfo.Size = new System.Drawing.Size(332, 29);
            this.labelEmailInfo.TabIndex = 1;
            this.labelEmailInfo.Text = "These settings are for email sent from FedEx.  This is separate from and in addit" +
    "ion to any email configured to be sent by ShipWorks.";
            // 
            // picturEmailInfo
            // 
            this.picturEmailInfo.BackColor = System.Drawing.Color.White;
            this.picturEmailInfo.Image = global::ShipWorks.Properties.Resources.information16;
            this.picturEmailInfo.Location = new System.Drawing.Point(5, 8);
            this.picturEmailInfo.Name = "picturEmailInfo";
            this.picturEmailInfo.Size = new System.Drawing.Size(16, 16);
            this.picturEmailInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picturEmailInfo.TabIndex = 0;
            this.picturEmailInfo.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(6, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "Personal Message (Optional)";
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(9, 158);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(385, 1);
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // kryptonBorderEdge2
            // 
            this.kryptonBorderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge2.AutoSize = false;
            this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(8, 59);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(385, 1);
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(14, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Other";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(14, 87);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Recipient";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(14, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Sender";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(302, 42);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Delivery";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(246, 42);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Exception";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(215, 42);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(28, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Ship";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(6, 42);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(73, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Email Address";
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(23, 7);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(332, 29);
            this.label12.TabIndex = 1;
            this.label12.Text = "These settings are for email sent from FedEx.  This is separate from and in addit" +
    "ion to any email configured to be sent by ShipWorks.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox1.Location = new System.Drawing.Point(5, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // sectionCOD
            // 
            this.sectionCOD.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionCOD.Collapsed = true;
            // 
            // sectionCOD.ContentPanel
            // 
            this.sectionCOD.ContentPanel.Controls.Add(this.codAddFreight);
            this.sectionCOD.ContentPanel.Controls.Add(this.codPaymentType);
            this.sectionCOD.ContentPanel.Controls.Add(this.labelCodPayment);
            this.sectionCOD.ContentPanel.Controls.Add(this.codAmount);
            this.sectionCOD.ContentPanel.Controls.Add(this.labelCodAmount);
            this.sectionCOD.ContentPanel.Controls.Add(this.codEnabled);
            this.sectionCOD.ExpandedHeight = 125;
            this.sectionCOD.ExtraText = "";
            this.sectionCOD.Location = new System.Drawing.Point(3, 613);
            this.sectionCOD.Name = "sectionCOD";
            this.sectionCOD.SectionName = "FedEx® Collect on Delivery (C.O.D.) or FedEx Ground® C.O.D.";
            this.sectionCOD.SettingsKey = "{c15e50fb-864c-415f-a752-ddfcc1c1e315}";
            this.sectionCOD.Size = new System.Drawing.Size(487, 24);
            this.sectionCOD.TabIndex = 11;
            // 
            // codAddFreight
            // 
            this.codAddFreight.AutoSize = true;
            this.codAddFreight.BackColor = System.Drawing.Color.White;
            this.codAddFreight.Location = new System.Drawing.Point(186, 32);
            this.codAddFreight.Name = "codAddFreight";
            this.codAddFreight.Size = new System.Drawing.Size(114, 17);
            this.codAddFreight.TabIndex = 2;
            this.codAddFreight.Text = "Add shipment cost";
            this.codAddFreight.UseVisualStyleBackColor = false;
            // 
            // codPaymentType
            // 
            this.codPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codPaymentType.FormattingEnabled = true;
            this.codPaymentType.Location = new System.Drawing.Point(86, 57);
            this.codPaymentType.Name = "codPaymentType";
            this.codPaymentType.PromptText = "(Multiple Values)";
            this.codPaymentType.Size = new System.Drawing.Size(94, 21);
            this.codPaymentType.TabIndex = 3;
            // 
            // labelCodPayment
            // 
            this.labelCodPayment.AutoSize = true;
            this.labelCodPayment.BackColor = System.Drawing.Color.White;
            this.labelCodPayment.Location = new System.Drawing.Point(30, 60);
            this.labelCodPayment.Name = "labelCodPayment";
            this.labelCodPayment.Size = new System.Drawing.Size(53, 13);
            this.labelCodPayment.TabIndex = 12;
            this.labelCodPayment.Text = "Payment:";
            // 
            // codAmount
            // 
            this.codAmount.Amount = new decimal(new int[] {
            0,
            0,
            0,
            131072});
            this.codAmount.IgnoreSet = false;
            this.codAmount.Location = new System.Drawing.Point(86, 30);
            this.codAmount.Name = "codAmount";
            this.codAmount.Size = new System.Drawing.Size(94, 21);
            this.codAmount.TabIndex = 1;
            this.codAmount.Text = "$0.00";
            // 
            // labelCodAmount
            // 
            this.labelCodAmount.AutoSize = true;
            this.labelCodAmount.BackColor = System.Drawing.Color.White;
            this.labelCodAmount.Location = new System.Drawing.Point(35, 33);
            this.labelCodAmount.Name = "labelCodAmount";
            this.labelCodAmount.Size = new System.Drawing.Size(48, 13);
            this.labelCodAmount.TabIndex = 10;
            this.labelCodAmount.Text = "Amount:";
            // 
            // codEnabled
            // 
            this.codEnabled.AutoSize = true;
            this.codEnabled.BackColor = System.Drawing.Color.White;
            this.codEnabled.Location = new System.Drawing.Point(7, 8);
            this.codEnabled.Name = "codEnabled";
            this.codEnabled.Size = new System.Drawing.Size(160, 17);
            this.codEnabled.TabIndex = 0;
            this.codEnabled.Text = "C.O.D. (Collect on Delivery)";
            this.codEnabled.UseVisualStyleBackColor = false;
            this.codEnabled.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // taxInfoLabel
            // 
            this.taxInfoLabel.AutoSize = true;
            this.taxInfoLabel.BackColor = System.Drawing.Color.White;
            this.taxInfoLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.taxInfoLabel.Location = new System.Drawing.Point(33, 416);
            this.taxInfoLabel.Name = "taxInfoLabel";
            this.taxInfoLabel.Size = new System.Drawing.Size(99, 13);
            this.taxInfoLabel.TabIndex = 15;
            this.taxInfoLabel.Text = "Tax Information";
            // 
            // codTaxId
            // 
            this.codTaxId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codTaxId.Location = new System.Drawing.Point(109, 436);
            this.codTaxId.Name = "codTaxId";
            this.codTaxId.Size = new System.Drawing.Size(276, 20);
            this.codTaxId.TabIndex = 14;
            // 
            // CodTINLabel
            // 
            this.CodTINLabel.AutoSize = true;
            this.CodTINLabel.BackColor = System.Drawing.Color.White;
            this.CodTINLabel.Location = new System.Drawing.Point(56, 439);
            this.CodTINLabel.Name = "CodTINLabel";
            this.CodTINLabel.Size = new System.Drawing.Size(43, 13);
            this.CodTINLabel.TabIndex = 13;
            this.CodTINLabel.Text = "Tax ID:";
            // 
            // codOrigin
            // 
            this.codOrigin.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codOrigin.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.codOrigin.BackColor = System.Drawing.Color.Transparent;
            this.codOrigin.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codOrigin.Location = new System.Drawing.Point(28, 86);
            this.codOrigin.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.codOrigin.Name = "codOrigin";
            this.codOrigin.OriginLabel = "Return To";
            this.codOrigin.Size = new System.Drawing.Size(372, 323);
            this.codOrigin.TabIndex = 4;
            this.codOrigin.OriginChanged += new System.EventHandler(this.OnCodOriginChanged);
            // 
            // panelTrademarkInfo
            // 
            this.panelTrademarkInfo.Controls.Add(this.linkTrademarkInfo);
            this.panelTrademarkInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelTrademarkInfo.Location = new System.Drawing.Point(0, 1115);
            this.panelTrademarkInfo.Name = "panelTrademarkInfo";
            this.panelTrademarkInfo.Size = new System.Drawing.Size(493, 19);
            this.panelTrademarkInfo.TabIndex = 16;
            // 
            // linkTrademarkInfo
            // 
            this.linkTrademarkInfo.AutoSize = true;
            this.linkTrademarkInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkTrademarkInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkTrademarkInfo.ForeColor = System.Drawing.Color.RoyalBlue;
            this.linkTrademarkInfo.Location = new System.Drawing.Point(1, 4);
            this.linkTrademarkInfo.Name = "linkTrademarkInfo";
            this.linkTrademarkInfo.Size = new System.Drawing.Size(433, 13);
            this.linkTrademarkInfo.TabIndex = 0;
            this.linkTrademarkInfo.Text = "FedEx service marks are owned by Federal Express Corporation and used by permissi" +
    "on.\r\n";
            this.linkTrademarkInfo.Click += new System.EventHandler(this.OnLinkTrademarkInfo);
            // 
            // smartManifestID
            // 
            this.smartManifestID.Location = new System.Drawing.Point(87, 119);
            this.smartManifestID.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.smartManifestID, ShipWorks.Data.Utility.EntityFieldLengthSource.FedExSmartPostCustomerManifest);
            this.smartManifestID.Name = "smartManifestID";
            this.smartManifestID.Size = new System.Drawing.Size(222, 21);
            this.smartManifestID.TabIndex = 9;
            this.smartManifestID.TokenSuggestionFactory = commonTokenSuggestionsFactory5;
            // 
            // sectionSmartPost
            // 
            this.sectionSmartPost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionSmartPost.Collapsed = true;
            // 
            // sectionSmartPost.ContentPanel
            // 
            this.sectionSmartPost.ContentPanel.Controls.Add(this.smartEndorsement);
            this.sectionSmartPost.ContentPanel.Controls.Add(this.smartIndicia);
            this.sectionSmartPost.ContentPanel.Controls.Add(this.labelSmartAncillary);
            this.sectionSmartPost.ContentPanel.Controls.Add(this.labelSmartEndicia);
            this.sectionSmartPost.ExpandedHeight = 100;
            this.sectionSmartPost.ExtraText = "";
            this.sectionSmartPost.Location = new System.Drawing.Point(3, 497);
            this.sectionSmartPost.Name = "sectionSmartPost";
            this.sectionSmartPost.SectionName = "FedEx Ground® Economy";
            this.sectionSmartPost.SettingsKey = "{37cbefe5-8feb-4b9c-945f-970382580a52}";
            this.sectionSmartPost.Size = new System.Drawing.Size(487, 24);
            this.sectionSmartPost.TabIndex = 5;
            // 
            // smartEndorsement
            // 
            this.smartEndorsement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.smartEndorsement.FormattingEnabled = true;
            this.smartEndorsement.Location = new System.Drawing.Point(87, 37);
            this.smartEndorsement.Name = "smartEndorsement";
            this.smartEndorsement.PromptText = "(Multiple Values)";
            this.smartEndorsement.Size = new System.Drawing.Size(225, 21);
            this.smartEndorsement.TabIndex = 5;
            this.smartEndorsement.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // smartIndicia
            // 
            this.smartIndicia.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.smartIndicia.FormattingEnabled = true;
            this.smartIndicia.Location = new System.Drawing.Point(87, 8);
            this.smartIndicia.Name = "smartIndicia";
            this.smartIndicia.PromptText = "(Multiple Values)";
            this.smartIndicia.Size = new System.Drawing.Size(275, 21);
            this.smartIndicia.TabIndex = 3;
            this.smartIndicia.SelectedIndexChanged += new System.EventHandler(this.OnChangeSmartPostIndicia);
            // 
            // labelSmartAncillary
            // 
            this.labelSmartAncillary.AutoSize = true;
            this.labelSmartAncillary.BackColor = System.Drawing.Color.Transparent;
            this.labelSmartAncillary.Location = new System.Drawing.Point(30, 40);
            this.labelSmartAncillary.Name = "labelSmartAncillary";
            this.labelSmartAncillary.Size = new System.Drawing.Size(51, 13);
            this.labelSmartAncillary.TabIndex = 4;
            this.labelSmartAncillary.Text = "Ancillary:";
            // 
            // labelSmartEndicia
            // 
            this.labelSmartEndicia.AutoSize = true;
            this.labelSmartEndicia.BackColor = System.Drawing.Color.Transparent;
            this.labelSmartEndicia.Location = new System.Drawing.Point(39, 11);
            this.labelSmartEndicia.Name = "labelSmartEndicia";
            this.labelSmartEndicia.Size = new System.Drawing.Size(42, 13);
            this.labelSmartEndicia.TabIndex = 2;
            this.labelSmartEndicia.Text = "Indicia:";
            // 
            // infotipSmartPostConfirmation
            // 
            this.infotipSmartPostConfirmation.Caption = "Delivery Confirmation is always used when Indicia is Parcel Select.  This option " +
    "only affects the other Indicia types.";
            this.infotipSmartPostConfirmation.Location = new System.Drawing.Point(242, 96);
            this.infotipSmartPostConfirmation.Name = "infotipSmartPostConfirmation";
            this.infotipSmartPostConfirmation.Size = new System.Drawing.Size(12, 12);
            this.infotipSmartPostConfirmation.TabIndex = 99;
            this.infotipSmartPostConfirmation.Title = "FedEx Ground® Economy Delivery Confirmation";
            // 
            // smartHubID
            // 
            this.smartHubID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.smartHubID.FormattingEnabled = true;
            this.smartHubID.Location = new System.Drawing.Point(87, 8);
            this.smartHubID.Name = "smartHubID";
            this.smartHubID.PromptText = "(Multiple Values)";
            this.smartHubID.Size = new System.Drawing.Size(225, 21);
            this.smartHubID.TabIndex = 1;
            this.smartHubID.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // labelSmartHubID
            // 
            this.labelSmartHubID.AutoSize = true;
            this.labelSmartHubID.BackColor = System.Drawing.Color.Transparent;
            this.labelSmartHubID.Location = new System.Drawing.Point(37, 11);
            this.labelSmartHubID.Name = "labelSmartHubID";
            this.labelSmartHubID.Size = new System.Drawing.Size(44, 13);
            this.labelSmartHubID.TabIndex = 0;
            this.labelSmartHubID.Text = "Hub ID:";
            // 
            // smartConfirmation
            // 
            this.smartConfirmation.AutoSize = true;
            this.smartConfirmation.BackColor = System.Drawing.Color.White;
            this.smartConfirmation.Location = new System.Drawing.Point(87, 94);
            this.smartConfirmation.Name = "smartConfirmation";
            this.smartConfirmation.Size = new System.Drawing.Size(157, 17);
            this.smartConfirmation.TabIndex = 7;
            this.smartConfirmation.Text = "USPS Delivery Confirmation";
            this.smartConfirmation.UseVisualStyleBackColor = false;
            this.smartConfirmation.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // labelSmartConfirmation
            // 
            this.labelSmartConfirmation.AutoSize = true;
            this.labelSmartConfirmation.BackColor = System.Drawing.Color.Transparent;
            this.labelSmartConfirmation.Location = new System.Drawing.Point(9, 95);
            this.labelSmartConfirmation.Name = "labelSmartConfirmation";
            this.labelSmartConfirmation.Size = new System.Drawing.Size(72, 13);
            this.labelSmartConfirmation.TabIndex = 6;
            this.labelSmartConfirmation.Text = "Confirmation:";
            // 
            // labelSmartManifestID
            // 
            this.labelSmartManifestID.AutoSize = true;
            this.labelSmartManifestID.BackColor = System.Drawing.Color.Transparent;
            this.labelSmartManifestID.Location = new System.Drawing.Point(15, 122);
            this.labelSmartManifestID.Name = "labelSmartManifestID";
            this.labelSmartManifestID.Size = new System.Drawing.Size(66, 13);
            this.labelSmartManifestID.TabIndex = 8;
            this.labelSmartManifestID.Text = "Manifest ID:";
            // 
            // sectionPackageDetails
            // 
            this.sectionPackageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionPackageDetails.Collapsed = true;
            // 
            // sectionPackageDetails.ContentPanel
            // 
            this.sectionPackageDetails.ContentPanel.Controls.Add(this.otherPackageHolder);
            this.sectionPackageDetails.ExpandedHeight = 774;
            this.sectionPackageDetails.ExtraText = "";
            this.sectionPackageDetails.Location = new System.Drawing.Point(3, 439);
            this.sectionPackageDetails.Name = "sectionPackageDetails";
            this.sectionPackageDetails.SectionName = "Other Package Details";
            this.sectionPackageDetails.SettingsKey = "{ce947713-e7de-463b-b583-9106a542cd24}";
            this.sectionPackageDetails.Size = new System.Drawing.Size(487, 24);
            this.sectionPackageDetails.TabIndex = 3;
            // 
            // otherPackageHolder
            // 
            this.otherPackageHolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otherPackageHolder.BackColor = System.Drawing.Color.Transparent;
            this.otherPackageHolder.Controls.Add(this.packageDetailsControl);
            this.otherPackageHolder.Location = new System.Drawing.Point(0, 0);
            this.otherPackageHolder.Name = "otherPackageHolder";
            this.otherPackageHolder.Size = new System.Drawing.Size(477, 546);
            this.otherPackageHolder.TabIndex = 1;
            // 
            // packageDetailsControl
            // 
            this.packageDetailsControl.AutoSize = true;
            this.packageDetailsControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.packageDetailsControl.BackColor = System.Drawing.Color.Transparent;
            this.packageDetailsControl.Location = new System.Drawing.Point(0, 0);
            this.packageDetailsControl.Name = "packageDetailsControl";
            this.packageDetailsControl.Size = new System.Drawing.Size(345, 148);
            this.packageDetailsControl.TabIndex = 0;
            this.packageDetailsControl.PackageDetailsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.packageDetailsControl.DangerousGoodsChanged += new System.EventHandler(this.OnDangerousGoodsChecked);
            this.packageDetailsControl.Resize += new System.EventHandler(this.OnPackageDetailsResize);
            // 
            // sectionServiceOptions
            // 
            this.sectionServiceOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionServiceOptions.Collapsed = true;
            // 
            // sectionServiceOptions.ContentPanel
            // 
            this.sectionServiceOptions.ContentPanel.Controls.Add(this.thirdPartyConsignee);
            this.sectionServiceOptions.ContentPanel.Controls.Add(this.consigneeLabel);
            this.sectionServiceOptions.ExpandedHeight = 60;
            this.sectionServiceOptions.ExtraText = "";
            this.sectionServiceOptions.Location = new System.Drawing.Point(3, 671);
            this.sectionServiceOptions.Name = "sectionServiceOptions";
            this.sectionServiceOptions.SectionName = "Service Options";
            this.sectionServiceOptions.SettingsKey = "{e4ccd963-eb98-4d6a-880f-1a3e236ce413}";
            this.sectionServiceOptions.Size = new System.Drawing.Size(487, 24);
            this.sectionServiceOptions.TabIndex = 13;
            // 
            // thirdPartyConsignee
            // 
            this.thirdPartyConsignee.AutoSize = true;
            this.thirdPartyConsignee.BackColor = System.Drawing.Color.Transparent;
            this.thirdPartyConsignee.Location = new System.Drawing.Point(106, 5);
            this.thirdPartyConsignee.Name = "thirdPartyConsignee";
            this.thirdPartyConsignee.Size = new System.Drawing.Size(79, 17);
            this.thirdPartyConsignee.TabIndex = 66;
            this.thirdPartyConsignee.Text = "Third Party";
            this.thirdPartyConsignee.UseVisualStyleBackColor = false;
            // 
            // consigneeLabel
            // 
            this.consigneeLabel.AutoSize = true;
            this.consigneeLabel.BackColor = System.Drawing.Color.Transparent;
            this.consigneeLabel.Location = new System.Drawing.Point(41, 5);
            this.consigneeLabel.Name = "consigneeLabel";
            this.consigneeLabel.Size = new System.Drawing.Size(61, 13);
            this.consigneeLabel.TabIndex = 6;
            this.consigneeLabel.Text = "Consignee:";
            // 
            // returnsClearance
            // 
            this.returnsClearance.AutoSize = true;
            this.returnsClearance.BackColor = System.Drawing.Color.Transparent;
            this.returnsClearance.Location = new System.Drawing.Point(106, 33);
            this.returnsClearance.Name = "returnsClearance";
            this.returnsClearance.Size = new System.Drawing.Size(115, 17);
            this.returnsClearance.TabIndex = 65;
            this.returnsClearance.Text = "Returns Clearance";
            this.returnsClearance.UseVisualStyleBackColor = false;
            // 
            // returnsClearanceLabel
            // 
            this.returnsClearanceLabel.AutoSize = true;
            this.returnsClearanceLabel.BackColor = System.Drawing.Color.Transparent;
            this.returnsClearanceLabel.Location = new System.Drawing.Point(2, 33);
            this.returnsClearanceLabel.Name = "returnsClearanceLabel";
            this.returnsClearanceLabel.Size = new System.Drawing.Size(100, 13);
            this.returnsClearanceLabel.TabIndex = 64;
            this.returnsClearanceLabel.Text = "Returns Clearance:";
            // 
            // labelDropoffType
            // 
            this.labelDropoffType.AutoSize = true;
            this.labelDropoffType.BackColor = System.Drawing.Color.Transparent;
            this.labelDropoffType.Location = new System.Drawing.Point(54, 9);
            this.labelDropoffType.Name = "labelDropoffType";
            this.labelDropoffType.Size = new System.Drawing.Size(48, 13);
            this.labelDropoffType.TabIndex = 63;
            this.labelDropoffType.Text = "Dropoff:";
            // 
            // dropoffType
            // 
            this.dropoffType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dropoffType.FormattingEnabled = true;
            this.dropoffType.Location = new System.Drawing.Point(106, 6);
            this.dropoffType.Name = "dropoffType";
            this.dropoffType.PromptText = "(Multiple Values)";
            this.dropoffType.Size = new System.Drawing.Size(175, 21);
            this.dropoffType.TabIndex = 62;
            // 
            // sectionFimsOptions
            // 
            this.sectionFimsOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionFimsOptions.ContentPanel
            // 
            this.sectionFimsOptions.ContentPanel.Controls.Add(this.fimsOptionsControl);
            this.sectionFimsOptions.ExtraText = "";
            this.sectionFimsOptions.Location = new System.Drawing.Point(3, 729);
            this.sectionFimsOptions.Name = "sectionFimsOptions";
            this.sectionFimsOptions.SectionName = "FIMS Options";
            this.sectionFimsOptions.SettingsKey = "{e87cbd40-e049-4cdf-adbd-d2daa1720fcf}";
            this.sectionFimsOptions.Size = new System.Drawing.Size(487, 99);
            this.sectionFimsOptions.TabIndex = 15;
            // 
            // fimsOptionsControl
            // 
            this.fimsOptionsControl.AutoSize = true;
            this.fimsOptionsControl.BackColor = System.Drawing.Color.Transparent;
            this.fimsOptionsControl.Location = new System.Drawing.Point(27, 5);
            this.fimsOptionsControl.Name = "fimsOptionsControl";
            this.fimsOptionsControl.Size = new System.Drawing.Size(279, 59);
            this.fimsOptionsControl.TabIndex = 0;
            // 
            // labelReturnsClearance
            // 
            this.labelReturnsClearance.AutoSize = true;
            this.labelReturnsClearance.BackColor = System.Drawing.Color.Transparent;
            this.labelReturnsClearance.Location = new System.Drawing.Point(2, 33);
            this.labelReturnsClearance.Name = "labelReturnsClearance";
            this.labelReturnsClearance.Size = new System.Drawing.Size(100, 13);
            this.labelReturnsClearance.TabIndex = 64;
            this.labelReturnsClearance.Text = "Returns Clearance:";
            // 
            // FedExServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panelTrademarkInfo);
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.sectionPackageDetails);
            this.Controls.Add(this.sectionOptions);
            this.Controls.Add(this.sectionSmartPost);
            this.Controls.Add(this.sectionHoldAtLocation);
            this.Controls.Add(this.sectionBilling);
            this.Controls.Add(this.sectionFreight);
            this.Controls.Add(this.sectionHomeDelivery);
            this.Controls.Add(this.sectionCOD);
            this.Controls.Add(this.sectionServiceOptions);
            this.Controls.Add(this.sectionFimsOptions);
            this.Name = "FedExServiceControl";
            this.Size = new System.Drawing.Size(493, 1134);
            this.Controls.SetChildIndex(this.sectionFimsOptions, 0);
            this.Controls.SetChildIndex(this.sectionLabelOptions, 0);
            this.Controls.SetChildIndex(this.sectionServiceOptions, 0);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionCOD, 0);
            this.Controls.SetChildIndex(this.sectionHomeDelivery, 0);
            this.Controls.SetChildIndex(this.sectionBilling, 0);
            this.Controls.SetChildIndex(this.sectionFreight, 0);
            this.Controls.SetChildIndex(this.sectionHoldAtLocation, 0);
            this.Controls.SetChildIndex(this.sectionSmartPost, 0);
            this.Controls.SetChildIndex(this.sectionOptions, 0);
            this.Controls.SetChildIndex(this.sectionPackageDetails, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            this.Controls.SetChildIndex(this.panelTrademarkInfo, 0);
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
            ((System.ComponentModel.ISupportInitialize)(this.sectionHoldAtLocation.ContentPanel)).EndInit();
            this.sectionHoldAtLocation.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionHoldAtLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling.ContentPanel)).EndInit();
            this.sectionBilling.ContentPanel.ResumeLayout(false);
            this.sectionBilling.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling)).EndInit();
            this.panelTransportAccount.ResumeLayout(false);
            this.panelTransportAccount.PerformLayout();
            this.panelPayorTransport.ResumeLayout(false);
            this.panelPayorTransport.PerformLayout();
            this.panelDeliveredDutiesPaid.ResumeLayout(false);
            this.panelDeliveredDutiesPaid.PerformLayout();
            this.panelPayorDuties.ResumeLayout(false);
            this.panelPayorDuties.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            this.sectionFrom.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionHomeDelivery.ContentPanel)).EndInit();
            this.sectionHomeDelivery.ContentPanel.ResumeLayout(false);
            this.sectionHomeDelivery.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionHomeDelivery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFreight.ContentPanel)).EndInit();
            this.sectionFreight.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionFreight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).EndInit();
            this.sectionOptions.ContentPanel.ResumeLayout(false);
            this.sectionOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail.ContentPanel)).EndInit();
            this.sectionEmail.ContentPanel.ResumeLayout(false);
            this.sectionEmail.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturEmailInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCOD.ContentPanel)).EndInit();
            this.sectionCOD.ContentPanel.ResumeLayout(false);
            this.sectionCOD.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCOD)).EndInit();
            this.panelTrademarkInfo.ResumeLayout(false);
            this.panelTrademarkInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSmartPost.ContentPanel)).EndInit();
            this.sectionSmartPost.ContentPanel.ResumeLayout(false);
            this.sectionSmartPost.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSmartPost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionPackageDetails.ContentPanel)).EndInit();
            this.sectionPackageDetails.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionPackageDetails)).EndInit();
            this.otherPackageHolder.ResumeLayout(false);
            this.otherPackageHolder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionServiceOptions.ContentPanel)).EndInit();
            this.sectionServiceOptions.ContentPanel.ResumeLayout(false);
            this.sectionServiceOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionServiceOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFimsOptions.ContentPanel)).EndInit();
            this.sectionFimsOptions.ContentPanel.ResumeLayout(false);
            this.sectionFimsOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFimsOptions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionBilling;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ShipWorks.UI.Controls.MultiValueComboBox fedexAccount;
        private ShipWorks.UI.Controls.MultiValueComboBox packagingType;
        private System.Windows.Forms.Label labelPackaging;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.Label labelShipDate;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker shipDate;
        private FedExPackageControl packageControl;
        private System.Windows.Forms.Label labelPayorTransport;
        private ShipWorks.UI.Controls.MultiValueComboBox payorTransport;
        private ShipWorks.UI.Controls.MultiValueComboBox payorCountry;
        private System.Windows.Forms.Label labelPayorCountry;
        private ShipWorks.UI.Controls.MultiValueTextBox payorPostalCode;
        private System.Windows.Forms.Label labelPayorPostalCode;
        private System.Windows.Forms.Label labelTransportAccount;
        private System.Windows.Forms.Label labelPayorDuties;
        private System.Windows.Forms.Panel panelPayorDuties;
        private ShipWorks.UI.Controls.MultiValueTextBox dutiesAccount;
        private System.Windows.Forms.Label labelDutiesAccount;
        private ShipWorks.UI.Controls.MultiValueComboBox payorDuties;
        private System.Windows.Forms.Panel panelTransportAccount;
        private ShipWorks.UI.Controls.MultiValueTextBox transportAccount;
        private System.Windows.Forms.Panel panelPayorTransport;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionHomeDelivery;
        private System.Windows.Forms.Label labelPremium;
        private System.Windows.Forms.Label labelHomeInstructions;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker homePremiumDate;
        private ShipWorks.UI.Controls.MultiValueComboBox homePremiumService;
        private ShipWorks.UI.Controls.MultiValueTextBox homeInstructions;
        private ShipWorks.UI.Controls.MultiValueTextBox homePremiumPhone;
        private System.Windows.Forms.Label labelHomePremiumPhone;
        private System.Windows.Forms.CheckBox nonStandardPackaging;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFreight;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionOptions;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox referenceInvoice;
        private System.Windows.Forms.Label labelInvoice;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox referenceCustomer;
        private System.Windows.Forms.Label labelReference;
        private ShipWorks.UI.Controls.MultiValueComboBox signature;
        private System.Windows.Forms.Label labelSignature;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox referencePO;
        private System.Windows.Forms.Label labelPO;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox referenceShipmentIntegrity;
        private System.Windows.Forms.Label labelShipmentIntegrity;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionEmail;
        private System.Windows.Forms.Label labelEmailInfo;
        private System.Windows.Forms.PictureBox picturEmailInfo;
        private System.Windows.Forms.CheckBox emailNotifySenderShip;
        private System.Windows.Forms.CheckBox emailNotifySenderDelivery;
        private System.Windows.Forms.CheckBox emailNotifySenderEstimatedDelivery;
        private System.Windows.Forms.CheckBox emailNotifySenderException;
        private System.Windows.Forms.Label labelEmailDelivery;
        private System.Windows.Forms.Label labelEmailEstimatedDelivery;
        private System.Windows.Forms.Label labelEmailException;
        private System.Windows.Forms.Label labelEmailShip;
        private System.Windows.Forms.Label labelEmailAddress;
        private System.Windows.Forms.Label labelEmailOther;
        private System.Windows.Forms.Label labelEmailRecipient;
        private System.Windows.Forms.Label labelEmailSender;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdgeEmail;
        private System.Windows.Forms.CheckBox emailNotifyOtherShip;
        private System.Windows.Forms.CheckBox emailNotifyOtherDelivery;
        private System.Windows.Forms.CheckBox emailNotifyOtherEstimatedDelivery;
        private System.Windows.Forms.CheckBox emailNotifyOtherException;
        private System.Windows.Forms.CheckBox emailNotifyRecipientShip;
        private System.Windows.Forms.CheckBox emailNotifyRecipientDelivery;
        private System.Windows.Forms.CheckBox emailNotifyRecipientEstimatedDelivery;
        private System.Windows.Forms.CheckBox emailNotifyRecipientException;
        private ShipWorks.UI.Controls.MultiValueTextBox emailNotifyOtherAddress;
        private ShipWorks.UI.Controls.MultiValueTextBox emailNotifyMessage;
        private System.Windows.Forms.Label labelPersonalMessage;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdgeEmail2;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionCOD;
        private System.Windows.Forms.CheckBox codEnabled;
        private ShipWorks.UI.Controls.MoneyTextBox codAmount;
        private System.Windows.Forms.Label labelCodAmount;
        private System.Windows.Forms.CheckBox codAddFreight;
        private ShipWorks.UI.Controls.MultiValueComboBox codPaymentType;
        private System.Windows.Forms.Label labelCodPayment;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl codOrigin;
        private System.Windows.Forms.Panel panelTrademarkInfo;
        private ShipWorks.UI.Controls.LinkControl linkTrademarkInfo;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionSmartPost;
        private System.Windows.Forms.CheckBox smartConfirmation;
        private ShipWorks.UI.Controls.MultiValueComboBox smartEndorsement;
        private ShipWorks.UI.Controls.MultiValueComboBox smartIndicia;
        private System.Windows.Forms.Label labelSmartAncillary;
        private System.Windows.Forms.Label labelSmartEndicia;
        private System.Windows.Forms.Label labelSmartConfirmation;
        private ShipWorks.Templates.Tokens.TemplateTokenTextBox smartManifestID;
        private System.Windows.Forms.Label labelSmartManifestID;
        private ShipWorks.UI.Controls.MultiValueComboBox smartHubID;
        private System.Windows.Forms.Label labelSmartHubID;
        private UI.Controls.InfoTip infotipSmartPostConfirmation;
        private System.Windows.Forms.Label label4;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox emailNotifyBrokerShip;
        private System.Windows.Forms.CheckBox emailNotifyBrokerDelivery;
        private System.Windows.Forms.CheckBox emailNotifyBrokerEstimatedDelivery;
        private System.Windows.Forms.CheckBox emailNotifyBrokerException;
        private System.Windows.Forms.Label labelEmailBroker;
        private System.Windows.Forms.Label labelFromResidentialCommercial;
        private System.Windows.Forms.Label labelFromAddressType;
        private UI.Controls.MultiValueComboBox fromAddressType;
        private MultiValueTextBox payorTransportName;
        private Label labelTransportPayorName;

        private CollapsibleGroupControl sectionHoldAtLocation;
        private FedExHoldAtLocationControl fedExHoldAtLocationControl;
        private Label CodTINLabel;
        private Label taxInfoLabel;
        private MultiValueTextBox codTaxId;
        private CollapsibleGroupControl sectionPackageDetails;
        private FedExPackageDetailControl packageDetailsControl;
        private Panel otherPackageHolder;
        private CollapsibleGroupControl sectionServiceOptions;
        private Label labelDropoffType;
        private MultiValueComboBox dropoffType;
        private CollapsibleGroupControl sectionFimsOptions;
        private FimsOptionsControl fimsOptionsControl;
        private CheckBox returnsClearance;
        private Label returnsClearanceLabel;
        private CheckBox thirdPartyConsignee;
        private Label consigneeLabel;
        private Label labelReturnsClearance;
        private Editing.ShippingDateCutoffDisplayControl cutoffDateDisplay;
        private FedExFreightContainerControl fedExFreightContainerControl;
        private Panel panelDeliveredDutiesPaid;
        private CheckBox deliveredDutyPaid;
        private Label labelDeliveredDutyPaid;
    }
}