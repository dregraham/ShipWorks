using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.UPS
{
    partial class UpsServiceControl
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory3 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory4 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.panelTop = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.upsAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.infotipShipDate = new ShipWorks.UI.Controls.InfoTip();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.packageControl = new ShipWorks.Shipping.Carriers.UPS.UpsPackageControl();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.insuranceUps = new System.Windows.Forms.RadioButton();
            this.sectionCod = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.codPaymentType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.codEnabled = new System.Windows.Forms.CheckBox();
            this.labelCodPayment = new System.Windows.Forms.Label();
            this.labelCodAmount = new System.Windows.Forms.Label();
            this.codAmount = new ShipWorks.UI.Controls.MoneyTextBox();
            this.sectionOptions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.confirmationAndReferenceFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.confirmationPanel = new System.Windows.Forms.Panel();
            this.confirmation = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelConfirmation = new System.Windows.Forms.Label();
            this.endorsementPanel = new System.Windows.Forms.Panel();
            this.uspsEndorsement = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelUspsEndorsement = new System.Windows.Forms.Label();
            this.referencePanel = new System.Windows.Forms.Panel();
            this.referenceNumber = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReference = new System.Windows.Forms.Label();
            this.reference2Panel = new System.Windows.Forms.Panel();
            this.reference2Number = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.shipperReleasePanel = new System.Windows.Forms.Panel();
            this.shipperRelease = new System.Windows.Forms.CheckBox();
            this.shipperReleaseLabel = new System.Windows.Forms.Label();
            this.carbonNeutralPanel = new System.Windows.Forms.Panel();
            this.carbonNeutral = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.sectionBilling = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.panelPayorTransport = new System.Windows.Forms.Panel();
            this.payorTransport = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPayor = new System.Windows.Forms.Label();
            this.panelPayorDuties = new System.Windows.Forms.Panel();
            this.panelDutiesAccount = new System.Windows.Forms.Panel();
            this.labelDutiesAccount = new System.Windows.Forms.Label();
            this.dutiesPostalCode = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.dutiesAccount = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelDutiesPostalCode = new System.Windows.Forms.Label();
            this.dutiesCountryCode = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelDutiesCountryCode = new System.Windows.Forms.Label();
            this.payorDuties = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPayorDuties = new System.Windows.Forms.Label();
            this.panelTransportAccount = new System.Windows.Forms.Panel();
            this.labelPayorAccount = new System.Windows.Forms.Label();
            this.payorPostalCode = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.payorAccount = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelPayorPostalCode = new System.Windows.Forms.Label();
            this.payorCountry = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPayorCountry = new System.Windows.Forms.Label();
            this.sectionEmail = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.emailSubject = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelEmailSubject = new System.Windows.Forms.Label();
            this.emailFrom = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelEmailFrom = new System.Windows.Forms.Label();
            this.emailNotifyMessage = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.labelPersonalMessage = new System.Windows.Forms.Label();
            this.kryptonBorderEdgeEmail2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.emailNotifyOtherShip = new System.Windows.Forms.CheckBox();
            this.emailNotifyOtherDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyOtherException = new System.Windows.Forms.CheckBox();
            this.emailNotifyRecipientShip = new System.Windows.Forms.CheckBox();
            this.emailNotifyRecipientDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifyRecipientException = new System.Windows.Forms.CheckBox();
            this.emailNotifyOtherAddress = new ShipWorks.UI.Controls.MultiValueTextBox();
            this.kryptonBorderEdgeEmail = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelEmailOther = new System.Windows.Forms.Label();
            this.labelEmailRecipient = new System.Windows.Forms.Label();
            this.labelEmailSender = new System.Windows.Forms.Label();
            this.emailNotifySenderShip = new System.Windows.Forms.CheckBox();
            this.emailNotifySenderDelivery = new System.Windows.Forms.CheckBox();
            this.emailNotifySenderException = new System.Windows.Forms.CheckBox();
            this.labelEmailDelivery = new System.Windows.Forms.Label();
            this.labelEmailException = new System.Windows.Forms.Label();
            this.labelEmailShip = new System.Windows.Forms.Label();
            this.labelEmailAddress = new System.Windows.Forms.Label();
            this.labelEmailInfo = new System.Windows.Forms.Label();
            this.picturEmailInfo = new System.Windows.Forms.PictureBox();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.sectionSurePost = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.costCenter = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.packageID = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelPackageID = new System.Windows.Forms.Label();
            this.irregularIndicator = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelIrregularIndicator = new System.Windows.Forms.Label();
            this.labelCostCenter = new System.Windows.Forms.Label();
            this.surePostClassification = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelClassification = new System.Windows.Forms.Label();
            this.otherPackageDetails = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.packageDetailsControl = new ShipWorks.Shipping.Carriers.UPS.UpsPackageDetailControl();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCod.ContentPanel)).BeginInit();
            this.sectionCod.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).BeginInit();
            this.sectionOptions.ContentPanel.SuspendLayout();
            this.confirmationAndReferenceFlowPanel.SuspendLayout();
            this.confirmationPanel.SuspendLayout();
            this.endorsementPanel.SuspendLayout();
            this.referencePanel.SuspendLayout();
            this.reference2Panel.SuspendLayout();
            this.shipperReleasePanel.SuspendLayout();
            this.carbonNeutralPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling.ContentPanel)).BeginInit();
            this.sectionBilling.ContentPanel.SuspendLayout();
            this.panelPayorTransport.SuspendLayout();
            this.panelPayorDuties.SuspendLayout();
            this.panelDutiesAccount.SuspendLayout();
            this.panelTransportAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail.ContentPanel)).BeginInit();
            this.sectionEmail.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturEmailInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSurePost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSurePost.ContentPanel)).BeginInit();
            this.sectionSurePost.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.otherPackageDetails)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherPackageDetails.ContentPanel)).BeginInit();
            this.otherPackageDetails.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionRecipient
            // 
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(366, 24);
            this.sectionRecipient.TabIndex = 1;
            // 
            // personControl
            // 
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(356, 330);
            // 
            // residentialDetermination
            // 
            this.residentialDetermination.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.residentialDetermination.Size = new System.Drawing.Size(267, 21);
            this.residentialDetermination.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // sectionReturns
            // 
            this.sectionReturns.Location = new System.Drawing.Point(3, 1583);
            this.sectionReturns.Size = new System.Drawing.Size(366, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(366, 24);
            // 
            // sectionShipment
            // 
            // 
            // sectionShipment.ContentPanel
            // 
            this.sectionShipment.ContentPanel.Controls.Add(this.saturdayDelivery);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.infotipShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.packageControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(366, 365);
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
            this.sectionFrom.ExpandedHeight = 487;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(366, 24);
            this.sectionFrom.TabIndex = 0;
            // 
            // originControl
            // 
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields)(((((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company) 
            | ShipWorks.Data.Controls.PersonFields.Street) 
            | ShipWorks.Data.Controls.PersonFields.City) 
            | ShipWorks.Data.Controls.PersonFields.State) 
            | ShipWorks.Data.Controls.PersonFields.Postal) 
            | ShipWorks.Data.Controls.PersonFields.Country) 
            | ShipWorks.Data.Controls.PersonFields.Residential) 
            | ShipWorks.Data.Controls.PersonFields.Email) 
            | ShipWorks.Data.Controls.PersonFields.Phone) 
            | ShipWorks.Data.Controls.PersonFields.Website)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.originControl.Location = new System.Drawing.Point(3, 54);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(356, 403);
            this.originControl.TabIndex = 8;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // panelTop
            // 
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.upsAccount);
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(370, 50);
            this.panelTop.TabIndex = 9;
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
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "UPS Account";
            // 
            // upsAccount
            // 
            this.upsAccount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.upsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.upsAccount.FormattingEnabled = true;
            this.upsAccount.Location = new System.Drawing.Point(79, 25);
            this.upsAccount.Name = "upsAccount";
            this.upsAccount.PromptText = "(Multiple Values)";
            this.upsAccount.Size = new System.Drawing.Size(264, 21);
            this.upsAccount.TabIndex = 3;
            this.upsAccount.SelectedIndexChanged += new System.EventHandler(this.OnOriginChanged);
            // 
            // saturdayDelivery
            // 
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.BackColor = System.Drawing.Color.White;
            this.saturdayDelivery.Location = new System.Drawing.Point(251, 38);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 2;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = false;
            this.saturdayDelivery.CheckedChanged += new System.EventHandler(this.OnSaturdayDeliveryChanged);
            // 
            // infotipShipDate
            // 
            this.infotipShipDate.Caption = "UPS does not provide a way for ShipWorks to specify a future ship date.";
            this.infotipShipDate.Location = new System.Drawing.Point(252, 40);
            this.infotipShipDate.Name = "infotipShipDate";
            this.infotipShipDate.Size = new System.Drawing.Size(12, 12);
            this.infotipShipDate.TabIndex = 67;
            this.infotipShipDate.Title = "Ship Date";
            // 
            // labelShipDate
            // 
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(39, 39);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 64;
            this.labelShipDate.Text = "Ship date:";
            // 
            // shipDate
            // 
            this.shipDate.Enabled = false;
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(101, 35);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(144, 21);
            this.shipDate.TabIndex = 1;
            this.shipDate.ValueChanged += new System.EventHandler(this.OnChangeShipDate);
            // 
            // packageControl
            // 
            this.packageControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.packageControl.BackColor = System.Drawing.Color.White;
            this.packageControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.packageControl.Location = new System.Drawing.Point(3, 61);
            this.packageControl.Name = "packageControl";
            this.packageControl.Size = new System.Drawing.Size(2818, 275);
            this.packageControl.TabIndex = 3;
            this.packageControl.RateCriteriaChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.packageControl.PackageCountChanged += new System.EventHandler(this.OnPackageCountChanged);
            this.packageControl.ShipSenseFieldChanged += OnShipSenseFieldChanged;
            this.packageControl.SizeChanged += new System.EventHandler(this.OnPackageControlSizeChanged);
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(101, 8);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(210, 21);
            this.service.TabIndex = 0;
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(49, 11);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 58;
            this.labelService.Text = "Service:";
            // 
            // insuranceUps
            // 
            this.insuranceUps.AutoSize = true;
            this.insuranceUps.BackColor = System.Drawing.Color.White;
            this.insuranceUps.Location = new System.Drawing.Point(8, 68);
            this.insuranceUps.Name = "insuranceUps";
            this.insuranceUps.Size = new System.Drawing.Size(95, 17);
            this.insuranceUps.TabIndex = 13;
            this.insuranceUps.TabStop = true;
            this.insuranceUps.Text = "UPS Insurance";
            this.insuranceUps.UseVisualStyleBackColor = false;
            // 
            // sectionCod
            // 
            this.sectionCod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionCod.Collapsed = true;
            // 
            // sectionCod.ContentPanel
            // 
            this.sectionCod.ContentPanel.Controls.Add(this.codPaymentType);
            this.sectionCod.ContentPanel.Controls.Add(this.codEnabled);
            this.sectionCod.ContentPanel.Controls.Add(this.labelCodPayment);
            this.sectionCod.ContentPanel.Controls.Add(this.labelCodAmount);
            this.sectionCod.ContentPanel.Controls.Add(this.codAmount);
            this.sectionCod.ExpandedHeight = 117;
            this.sectionCod.ExtraText = "";
            this.sectionCod.Location = new System.Drawing.Point(3, 1406);
            this.sectionCod.Name = "sectionCod";
            this.sectionCod.SectionName = "C.O.D.";
            this.sectionCod.SettingsKey = "{640124b8-f610-4488-b282-7e2c36618b81}";
            this.sectionCod.Size = new System.Drawing.Size(366, 24);
            this.sectionCod.TabIndex = 9;
            // 
            // codPaymentType
            // 
            this.codPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codPaymentType.FormattingEnabled = true;
            this.codPaymentType.Location = new System.Drawing.Point(92, 60);
            this.codPaymentType.Name = "codPaymentType";
            this.codPaymentType.PromptText = "(Multiple Values)";
            this.codPaymentType.Size = new System.Drawing.Size(210, 21);
            this.codPaymentType.TabIndex = 2;
            // 
            // codEnabled
            // 
            this.codEnabled.AutoSize = true;
            this.codEnabled.BackColor = System.Drawing.Color.White;
            this.codEnabled.Location = new System.Drawing.Point(13, 11);
            this.codEnabled.Name = "codEnabled";
            this.codEnabled.Size = new System.Drawing.Size(160, 17);
            this.codEnabled.TabIndex = 0;
            this.codEnabled.Text = "C.O.D. (Collect on Delivery)";
            this.codEnabled.UseVisualStyleBackColor = false;
            this.codEnabled.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // labelCodPayment
            // 
            this.labelCodPayment.AutoSize = true;
            this.labelCodPayment.BackColor = System.Drawing.Color.White;
            this.labelCodPayment.Location = new System.Drawing.Point(36, 63);
            this.labelCodPayment.Name = "labelCodPayment";
            this.labelCodPayment.Size = new System.Drawing.Size(53, 13);
            this.labelCodPayment.TabIndex = 65;
            this.labelCodPayment.Text = "Payment:";
            // 
            // labelCodAmount
            // 
            this.labelCodAmount.AutoSize = true;
            this.labelCodAmount.BackColor = System.Drawing.Color.White;
            this.labelCodAmount.Location = new System.Drawing.Point(41, 36);
            this.labelCodAmount.Name = "labelCodAmount";
            this.labelCodAmount.Size = new System.Drawing.Size(48, 13);
            this.labelCodAmount.TabIndex = 63;
            this.labelCodAmount.Text = "Amount:";
            // 
            // codAmount
            // 
            this.codAmount.Amount = new decimal(new int[] {
            0,
            0,
            0,
            65536});
            this.codAmount.Location = new System.Drawing.Point(92, 33);
            this.codAmount.Name = "codAmount";
            this.codAmount.Size = new System.Drawing.Size(94, 21);
            this.codAmount.TabIndex = 1;
            this.codAmount.Text = "$0.00";
            // 
            // sectionOptions
            // 
            this.sectionOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionOptions.Collapsed = true;
            // 
            // sectionOptions.ContentPanel
            // 
            this.sectionOptions.ContentPanel.Controls.Add(this.confirmationAndReferenceFlowPanel);
            this.sectionOptions.ExpandedHeight = 275;
            this.sectionOptions.ExtraText = "";
            this.sectionOptions.Location = new System.Drawing.Point(3, 462);
            this.sectionOptions.Name = "sectionOptions";
            this.sectionOptions.SectionName = "Options & Reference";
            this.sectionOptions.SettingsKey = "{e3c1d665-00e2-42af-a748-99bc1fc7a387}";
            this.sectionOptions.Size = new System.Drawing.Size(366, 24);
            this.sectionOptions.TabIndex = 6;
            // 
            // confirmationAndReferenceFlowPanel
            // 
            this.confirmationAndReferenceFlowPanel.AutoSize = true;
            this.confirmationAndReferenceFlowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.confirmationAndReferenceFlowPanel.BackColor = System.Drawing.Color.Transparent;
            this.confirmationAndReferenceFlowPanel.Controls.Add(this.confirmationPanel);
            this.confirmationAndReferenceFlowPanel.Controls.Add(this.endorsementPanel);
            this.confirmationAndReferenceFlowPanel.Controls.Add(this.referencePanel);
            this.confirmationAndReferenceFlowPanel.Controls.Add(this.reference2Panel);
            this.confirmationAndReferenceFlowPanel.Controls.Add(this.shipperReleasePanel);
            this.confirmationAndReferenceFlowPanel.Controls.Add(this.carbonNeutralPanel);
            this.confirmationAndReferenceFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.confirmationAndReferenceFlowPanel.Location = new System.Drawing.Point(3, 3);
            this.confirmationAndReferenceFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.confirmationAndReferenceFlowPanel.Name = "confirmationAndReferenceFlowPanel";
            this.confirmationAndReferenceFlowPanel.Size = new System.Drawing.Size(380, 180);
            this.confirmationAndReferenceFlowPanel.TabIndex = 11;
            // 
            // confirmationPanel
            // 
            this.confirmationPanel.BackColor = System.Drawing.Color.White;
            this.confirmationPanel.Controls.Add(this.confirmation);
            this.confirmationPanel.Controls.Add(this.labelConfirmation);
            this.confirmationPanel.Location = new System.Drawing.Point(0, 0);
            this.confirmationPanel.Margin = new System.Windows.Forms.Padding(0);
            this.confirmationPanel.Name = "confirmationPanel";
            this.confirmationPanel.Size = new System.Drawing.Size(380, 30);
            this.confirmationPanel.TabIndex = 80;
            // 
            // confirmation
            // 
            this.confirmation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.confirmation.FormattingEnabled = true;
            this.confirmation.Location = new System.Drawing.Point(121, 5);
            this.confirmation.Name = "confirmation";
            this.confirmation.PromptText = "(Multiple Values)";
            this.confirmation.Size = new System.Drawing.Size(175, 21);
            this.confirmation.TabIndex = 73;
            this.confirmation.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // labelConfirmation
            // 
            this.labelConfirmation.AutoSize = true;
            this.labelConfirmation.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmation.Location = new System.Drawing.Point(43, 8);
            this.labelConfirmation.Name = "labelConfirmation";
            this.labelConfirmation.Size = new System.Drawing.Size(72, 13);
            this.labelConfirmation.TabIndex = 74;
            this.labelConfirmation.Text = "Confirmation:";
            // 
            // endorsementPanel
            // 
            this.endorsementPanel.BackColor = System.Drawing.Color.Transparent;
            this.endorsementPanel.Controls.Add(this.uspsEndorsement);
            this.endorsementPanel.Controls.Add(this.labelUspsEndorsement);
            this.endorsementPanel.Location = new System.Drawing.Point(0, 30);
            this.endorsementPanel.Margin = new System.Windows.Forms.Padding(0);
            this.endorsementPanel.Name = "endorsementPanel";
            this.endorsementPanel.Size = new System.Drawing.Size(380, 30);
            this.endorsementPanel.TabIndex = 11;
            // 
            // uspsEndorsement
            // 
            this.uspsEndorsement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uspsEndorsement.FormattingEnabled = true;
            this.uspsEndorsement.Location = new System.Drawing.Point(121, 5);
            this.uspsEndorsement.Name = "uspsEndorsement";
            this.uspsEndorsement.PromptText = "(Multiple Values)";
            this.uspsEndorsement.Size = new System.Drawing.Size(175, 21);
            this.uspsEndorsement.TabIndex = 9;
            // 
            // labelUspsEndorsement
            // 
            this.labelUspsEndorsement.AutoSize = true;
            this.labelUspsEndorsement.BackColor = System.Drawing.Color.Transparent;
            this.labelUspsEndorsement.Location = new System.Drawing.Point(41, 8);
            this.labelUspsEndorsement.Name = "labelUspsEndorsement";
            this.labelUspsEndorsement.Size = new System.Drawing.Size(74, 13);
            this.labelUspsEndorsement.TabIndex = 8;
            this.labelUspsEndorsement.Text = "Endorsement:";
            // 
            // referencePanel
            // 
            this.referencePanel.BackColor = System.Drawing.Color.White;
            this.referencePanel.Controls.Add(this.referenceNumber);
            this.referencePanel.Controls.Add(this.labelReference);
            this.referencePanel.Location = new System.Drawing.Point(0, 60);
            this.referencePanel.Margin = new System.Windows.Forms.Padding(0);
            this.referencePanel.Name = "referencePanel";
            this.referencePanel.Size = new System.Drawing.Size(380, 30);
            this.referencePanel.TabIndex = 81;
            // 
            // referenceNumber
            // 
            this.referenceNumber.Location = new System.Drawing.Point(121, 5);
            this.referenceNumber.MaxLength = 32767;
            this.referenceNumber.Name = "referenceNumber";
            this.referenceNumber.Size = new System.Drawing.Size(210, 21);
            this.referenceNumber.TabIndex = 79;
            this.referenceNumber.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            // 
            // labelReference
            // 
            this.labelReference.AutoSize = true;
            this.labelReference.BackColor = System.Drawing.Color.Transparent;
            this.labelReference.Location = new System.Drawing.Point(43, 7);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(72, 13);
            this.labelReference.TabIndex = 80;
            this.labelReference.Text = "Reference #:";
            // 
            // reference2Panel
            // 
            this.reference2Panel.BackColor = System.Drawing.Color.White;
            this.reference2Panel.Controls.Add(this.reference2Number);
            this.reference2Panel.Controls.Add(this.label3);
            this.reference2Panel.Location = new System.Drawing.Point(0, 90);
            this.reference2Panel.Margin = new System.Windows.Forms.Padding(0);
            this.reference2Panel.Name = "reference2Panel";
            this.reference2Panel.Size = new System.Drawing.Size(380, 30);
            this.reference2Panel.TabIndex = 77;
            // 
            // reference2Number
            // 
            this.reference2Number.Location = new System.Drawing.Point(121, 5);
            this.reference2Number.MaxLength = 32767;
            this.reference2Number.Name = "reference2Number";
            this.reference2Number.Size = new System.Drawing.Size(210, 21);
            this.reference2Number.TabIndex = 77;
            this.reference2Number.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(37, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 78;
            this.label3.Text = "Reference 2#:";
            // 
            // shipperReleasePanel
            // 
            this.shipperReleasePanel.BackColor = System.Drawing.Color.Transparent;
            this.shipperReleasePanel.Controls.Add(this.shipperRelease);
            this.shipperReleasePanel.Controls.Add(this.shipperReleaseLabel);
            this.shipperReleasePanel.Location = new System.Drawing.Point(0, 120);
            this.shipperReleasePanel.Margin = new System.Windows.Forms.Padding(0);
            this.shipperReleasePanel.Name = "shipperReleasePanel";
            this.shipperReleasePanel.Size = new System.Drawing.Size(380, 30);
            this.shipperReleasePanel.TabIndex = 82;
            // 
            // shipperRelease
            // 
            this.shipperRelease.AutoSize = true;
            this.shipperRelease.BackColor = System.Drawing.Color.White;
            this.shipperRelease.Location = new System.Drawing.Point(121, 8);
            this.shipperRelease.Name = "shipperRelease";
            this.shipperRelease.Size = new System.Drawing.Size(15, 14);
            this.shipperRelease.TabIndex = 9;
            this.shipperRelease.UseVisualStyleBackColor = false;
            this.shipperRelease.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // shipperReleaseLabel
            // 
            this.shipperReleaseLabel.AutoSize = true;
            this.shipperReleaseLabel.BackColor = System.Drawing.Color.Transparent;
            this.shipperReleaseLabel.Location = new System.Drawing.Point(27, 8);
            this.shipperReleaseLabel.Name = "shipperReleaseLabel";
            this.shipperReleaseLabel.Size = new System.Drawing.Size(88, 13);
            this.shipperReleaseLabel.TabIndex = 8;
            this.shipperReleaseLabel.Text = "Shipper Release:";
            // 
            // carbonNeutralPanel
            // 
            this.carbonNeutralPanel.BackColor = System.Drawing.Color.Transparent;
            this.carbonNeutralPanel.Controls.Add(this.carbonNeutral);
            this.carbonNeutralPanel.Controls.Add(this.label5);
            this.carbonNeutralPanel.Location = new System.Drawing.Point(0, 150);
            this.carbonNeutralPanel.Margin = new System.Windows.Forms.Padding(0);
            this.carbonNeutralPanel.Name = "carbonNeutralPanel";
            this.carbonNeutralPanel.Size = new System.Drawing.Size(380, 30);
            this.carbonNeutralPanel.TabIndex = 83;
            // 
            // carbonNeutral
            // 
            this.carbonNeutral.AutoSize = true;
            this.carbonNeutral.BackColor = System.Drawing.Color.White;
            this.carbonNeutral.Location = new System.Drawing.Point(121, 8);
            this.carbonNeutral.Name = "carbonNeutral";
            this.carbonNeutral.Size = new System.Drawing.Size(15, 14);
            this.carbonNeutral.TabIndex = 9;
            this.carbonNeutral.UseVisualStyleBackColor = false;
            this.carbonNeutral.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(9, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "UPS Carbon Neutral:";
            // 
            // sectionBilling
            // 
            this.sectionBilling.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionBilling.ContentPanel
            // 
            this.sectionBilling.ContentPanel.Controls.Add(this.panelPayorTransport);
            this.sectionBilling.ContentPanel.Controls.Add(this.panelPayorDuties);
            this.sectionBilling.ContentPanel.Controls.Add(this.panelTransportAccount);
            this.sectionBilling.ExtraText = "";
            this.sectionBilling.Location = new System.Drawing.Point(3, 491);
            this.sectionBilling.Name = "sectionBilling";
            this.sectionBilling.SectionName = "Billing";
            this.sectionBilling.SettingsKey = "{7ec15092-b2c0-4faf-9ce3-27d6bb912fba}";
            this.sectionBilling.Size = new System.Drawing.Size(366, 588);
            this.sectionBilling.TabIndex = 7;
            // 
            // panelPayorTransport
            // 
            this.panelPayorTransport.BackColor = System.Drawing.Color.White;
            this.panelPayorTransport.Controls.Add(this.payorTransport);
            this.panelPayorTransport.Controls.Add(this.labelPayor);
            this.panelPayorTransport.Location = new System.Drawing.Point(1, 1);
            this.panelPayorTransport.Name = "panelPayorTransport";
            this.panelPayorTransport.Size = new System.Drawing.Size(475, 33);
            this.panelPayorTransport.TabIndex = 76;
            // 
            // payorTransport
            // 
            this.payorTransport.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.payorTransport.FormattingEnabled = true;
            this.payorTransport.Location = new System.Drawing.Point(106, 6);
            this.payorTransport.Name = "payorTransport";
            this.payorTransport.PromptText = "(Multiple Values)";
            this.payorTransport.Size = new System.Drawing.Size(173, 21);
            this.payorTransport.TabIndex = 75;
            this.payorTransport.SelectedIndexChanged += new System.EventHandler(this.OnChangePayorType);
            // 
            // labelPayor
            // 
            this.labelPayor.AutoSize = true;
            this.labelPayor.BackColor = System.Drawing.Color.White;
            this.labelPayor.Location = new System.Drawing.Point(18, 9);
            this.labelPayor.Name = "labelPayor";
            this.labelPayor.Size = new System.Drawing.Size(82, 13);
            this.labelPayor.TabIndex = 76;
            this.labelPayor.Text = "Bill shipment to:";
            // 
            // panelPayorDuties
            // 
            this.panelPayorDuties.BackColor = System.Drawing.Color.White;
            this.panelPayorDuties.Controls.Add(this.panelDutiesAccount);
            this.panelPayorDuties.Controls.Add(this.payorDuties);
            this.panelPayorDuties.Controls.Add(this.labelPayorDuties);
            this.panelPayorDuties.Location = new System.Drawing.Point(1, 119);
            this.panelPayorDuties.Name = "panelPayorDuties";
            this.panelPayorDuties.Size = new System.Drawing.Size(475, 118);
            this.panelPayorDuties.TabIndex = 75;
            // 
            // panelDutiesAccount
            // 
            this.panelDutiesAccount.BackColor = System.Drawing.Color.White;
            this.panelDutiesAccount.Controls.Add(this.labelDutiesAccount);
            this.panelDutiesAccount.Controls.Add(this.dutiesPostalCode);
            this.panelDutiesAccount.Controls.Add(this.dutiesAccount);
            this.panelDutiesAccount.Controls.Add(this.labelDutiesPostalCode);
            this.panelDutiesAccount.Controls.Add(this.dutiesCountryCode);
            this.panelDutiesAccount.Controls.Add(this.labelDutiesCountryCode);
            this.panelDutiesAccount.Location = new System.Drawing.Point(8, 32);
            this.panelDutiesAccount.Name = "panelDutiesAccount";
            this.panelDutiesAccount.Size = new System.Drawing.Size(455, 83);
            this.panelDutiesAccount.TabIndex = 85;
            // 
            // labelDutiesAccount
            // 
            this.labelDutiesAccount.AutoSize = true;
            this.labelDutiesAccount.BackColor = System.Drawing.Color.White;
            this.labelDutiesAccount.Location = new System.Drawing.Point(31, 7);
            this.labelDutiesAccount.Name = "labelDutiesAccount";
            this.labelDutiesAccount.Size = new System.Drawing.Size(61, 13);
            this.labelDutiesAccount.TabIndex = 83;
            this.labelDutiesAccount.Text = "Account #:";
            // 
            // dutiesPostalCode
            // 
            this.dutiesPostalCode.Location = new System.Drawing.Point(99, 31);
            this.dutiesPostalCode.Name = "dutiesPostalCode";
            this.dutiesPostalCode.Size = new System.Drawing.Size(173, 21);
            this.dutiesPostalCode.TabIndex = 87;
            // 
            // dutiesAccount
            // 
            this.dutiesAccount.Location = new System.Drawing.Point(99, 4);
            this.dutiesAccount.Name = "dutiesAccount";
            this.dutiesAccount.Size = new System.Drawing.Size(173, 21);
            this.dutiesAccount.TabIndex = 84;
            // 
            // labelDutiesPostalCode
            // 
            this.labelDutiesPostalCode.AutoSize = true;
            this.labelDutiesPostalCode.BackColor = System.Drawing.Color.White;
            this.labelDutiesPostalCode.Location = new System.Drawing.Point(24, 34);
            this.labelDutiesPostalCode.Name = "labelDutiesPostalCode";
            this.labelDutiesPostalCode.Size = new System.Drawing.Size(68, 13);
            this.labelDutiesPostalCode.TabIndex = 86;
            this.labelDutiesPostalCode.Text = "Postal Code:";
            // 
            // dutiesCountryCode
            // 
            this.dutiesCountryCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dutiesCountryCode.FormattingEnabled = true;
            this.dutiesCountryCode.Location = new System.Drawing.Point(99, 58);
            this.dutiesCountryCode.Name = "dutiesCountryCode";
            this.dutiesCountryCode.PromptText = "(Multiple Values)";
            this.dutiesCountryCode.Size = new System.Drawing.Size(173, 21);
            this.dutiesCountryCode.TabIndex = 88;
            // 
            // labelDutiesCountryCode
            // 
            this.labelDutiesCountryCode.AutoSize = true;
            this.labelDutiesCountryCode.BackColor = System.Drawing.Color.Transparent;
            this.labelDutiesCountryCode.Location = new System.Drawing.Point(42, 61);
            this.labelDutiesCountryCode.Name = "labelDutiesCountryCode";
            this.labelDutiesCountryCode.Size = new System.Drawing.Size(50, 13);
            this.labelDutiesCountryCode.TabIndex = 85;
            this.labelDutiesCountryCode.Text = "Country:";
            // 
            // payorDuties
            // 
            this.payorDuties.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.payorDuties.FormattingEnabled = true;
            this.payorDuties.Location = new System.Drawing.Point(107, 7);
            this.payorDuties.Name = "payorDuties";
            this.payorDuties.PromptText = "(Multiple Values)";
            this.payorDuties.Size = new System.Drawing.Size(173, 21);
            this.payorDuties.TabIndex = 83;
            this.payorDuties.SelectedIndexChanged += new System.EventHandler(this.OnPayorDutiesFeesChanged);
            // 
            // labelPayorDuties
            // 
            this.labelPayorDuties.AutoSize = true;
            this.labelPayorDuties.BackColor = System.Drawing.Color.White;
            this.labelPayorDuties.Location = new System.Drawing.Point(7, 10);
            this.labelPayorDuties.Name = "labelPayorDuties";
            this.labelPayorDuties.Size = new System.Drawing.Size(93, 13);
            this.labelPayorDuties.TabIndex = 84;
            this.labelPayorDuties.Text = "Bill duties/fees to:";
            // 
            // panelTransportAccount
            // 
            this.panelTransportAccount.BackColor = System.Drawing.Color.White;
            this.panelTransportAccount.Controls.Add(this.labelPayorAccount);
            this.panelTransportAccount.Controls.Add(this.payorPostalCode);
            this.panelTransportAccount.Controls.Add(this.payorAccount);
            this.panelTransportAccount.Controls.Add(this.labelPayorPostalCode);
            this.panelTransportAccount.Controls.Add(this.payorCountry);
            this.panelTransportAccount.Controls.Add(this.labelPayorCountry);
            this.panelTransportAccount.Location = new System.Drawing.Point(1, 34);
            this.panelTransportAccount.Name = "panelTransportAccount";
            this.panelTransportAccount.Size = new System.Drawing.Size(475, 83);
            this.panelTransportAccount.TabIndex = 1;
            // 
            // labelPayorAccount
            // 
            this.labelPayorAccount.AutoSize = true;
            this.labelPayorAccount.BackColor = System.Drawing.Color.White;
            this.labelPayorAccount.Location = new System.Drawing.Point(39, 5);
            this.labelPayorAccount.Name = "labelPayorAccount";
            this.labelPayorAccount.Size = new System.Drawing.Size(61, 13);
            this.labelPayorAccount.TabIndex = 76;
            this.labelPayorAccount.Text = "Account #:";
            // 
            // payorPostalCode
            // 
            this.payorPostalCode.Location = new System.Drawing.Point(107, 29);
            this.payorPostalCode.Name = "payorPostalCode";
            this.payorPostalCode.Size = new System.Drawing.Size(173, 21);
            this.payorPostalCode.TabIndex = 81;
            // 
            // payorAccount
            // 
            this.payorAccount.Location = new System.Drawing.Point(107, 2);
            this.payorAccount.Name = "payorAccount";
            this.payorAccount.Size = new System.Drawing.Size(173, 21);
            this.payorAccount.TabIndex = 77;
            // 
            // labelPayorPostalCode
            // 
            this.labelPayorPostalCode.AutoSize = true;
            this.labelPayorPostalCode.BackColor = System.Drawing.Color.White;
            this.labelPayorPostalCode.Location = new System.Drawing.Point(32, 32);
            this.labelPayorPostalCode.Name = "labelPayorPostalCode";
            this.labelPayorPostalCode.Size = new System.Drawing.Size(68, 13);
            this.labelPayorPostalCode.TabIndex = 80;
            this.labelPayorPostalCode.Text = "Postal Code:";
            // 
            // payorCountry
            // 
            this.payorCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.payorCountry.FormattingEnabled = true;
            this.payorCountry.Location = new System.Drawing.Point(107, 56);
            this.payorCountry.Name = "payorCountry";
            this.payorCountry.PromptText = "(Multiple Values)";
            this.payorCountry.Size = new System.Drawing.Size(173, 21);
            this.payorCountry.TabIndex = 82;
            // 
            // labelPayorCountry
            // 
            this.labelPayorCountry.AutoSize = true;
            this.labelPayorCountry.BackColor = System.Drawing.Color.Transparent;
            this.labelPayorCountry.Location = new System.Drawing.Point(50, 59);
            this.labelPayorCountry.Name = "labelPayorCountry";
            this.labelPayorCountry.Size = new System.Drawing.Size(50, 13);
            this.labelPayorCountry.TabIndex = 78;
            this.labelPayorCountry.Text = "Country:";
            // 
            // sectionEmail
            // 
            this.sectionEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionEmail.ContentPanel
            // 
            this.sectionEmail.ContentPanel.Controls.Add(this.emailSubject);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailSubject);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailFrom);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailFrom);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyMessage);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelPersonalMessage);
            this.sectionEmail.ContentPanel.Controls.Add(this.kryptonBorderEdgeEmail2);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherException);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyRecipientShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyRecipientDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyRecipientException);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifyOtherAddress);
            this.sectionEmail.ContentPanel.Controls.Add(this.kryptonBorderEdgeEmail);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailOther);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailRecipient);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailSender);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifySenderShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifySenderDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.emailNotifySenderException);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailDelivery);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailException);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailShip);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailAddress);
            this.sectionEmail.ContentPanel.Controls.Add(this.labelEmailInfo);
            this.sectionEmail.ContentPanel.Controls.Add(this.picturEmailInfo);
            this.sectionEmail.ExtraText = "";
            this.sectionEmail.Location = new System.Drawing.Point(3, 1084);
            this.sectionEmail.Name = "sectionEmail";
            this.sectionEmail.SectionName = "Quantum View Notify";
            this.sectionEmail.SettingsKey = "{a71bde8e-f7e1-49ce-8e13-635a44b963b2}";
            this.sectionEmail.Size = new System.Drawing.Size(366, 317);
            this.sectionEmail.TabIndex = 8;
            // 
            // emailSubject
            // 
            this.emailSubject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.emailSubject.FormattingEnabled = true;
            this.emailSubject.Location = new System.Drawing.Point(74, 174);
            this.emailSubject.Name = "emailSubject";
            this.emailSubject.PromptText = "(Multiple Values)";
            this.emailSubject.Size = new System.Drawing.Size(164, 21);
            this.emailSubject.TabIndex = 82;
            // 
            // labelEmailSubject
            // 
            this.labelEmailSubject.AutoSize = true;
            this.labelEmailSubject.BackColor = System.Drawing.Color.White;
            this.labelEmailSubject.Location = new System.Drawing.Point(24, 177);
            this.labelEmailSubject.Name = "labelEmailSubject";
            this.labelEmailSubject.Size = new System.Drawing.Size(47, 13);
            this.labelEmailSubject.TabIndex = 81;
            this.labelEmailSubject.Text = "Subject:";
            // 
            // emailFrom
            // 
            this.emailFrom.Location = new System.Drawing.Point(74, 146);
            this.fieldLengthProvider.SetMaxLengthSource(this.emailFrom, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsQvnFrom);
            this.emailFrom.Name = "emailFrom";
            this.emailFrom.Size = new System.Drawing.Size(165, 21);
            this.emailFrom.TabIndex = 80;
            // 
            // labelEmailFrom
            // 
            this.labelEmailFrom.AutoSize = true;
            this.labelEmailFrom.BackColor = System.Drawing.Color.White;
            this.labelEmailFrom.Location = new System.Drawing.Point(8, 149);
            this.labelEmailFrom.Name = "labelEmailFrom";
            this.labelEmailFrom.Size = new System.Drawing.Size(64, 13);
            this.labelEmailFrom.TabIndex = 79;
            this.labelEmailFrom.Text = "From name:";
            // 
            // emailNotifyMessage
            // 
            this.emailNotifyMessage.Location = new System.Drawing.Point(20, 215);
            this.emailNotifyMessage.MaxLength = 120;
            this.fieldLengthProvider.SetMaxLengthSource(this.emailNotifyMessage, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsQvnMessage);
            this.emailNotifyMessage.Multiline = true;
            this.emailNotifyMessage.Name = "emailNotifyMessage";
            this.emailNotifyMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.emailNotifyMessage.Size = new System.Drawing.Size(319, 61);
            this.emailNotifyMessage.TabIndex = 78;
            // 
            // labelPersonalMessage
            // 
            this.labelPersonalMessage.AutoSize = true;
            this.labelPersonalMessage.BackColor = System.Drawing.Color.White;
            this.labelPersonalMessage.Location = new System.Drawing.Point(9, 199);
            this.labelPersonalMessage.Name = "labelPersonalMessage";
            this.labelPersonalMessage.Size = new System.Drawing.Size(144, 13);
            this.labelPersonalMessage.TabIndex = 77;
            this.labelPersonalMessage.Text = "Personal Message (Optional)";
            // 
            // kryptonBorderEdgeEmail2
            // 
            this.kryptonBorderEdgeEmail2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdgeEmail2.AutoSize = false;
            this.kryptonBorderEdgeEmail2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdgeEmail2.Location = new System.Drawing.Point(12, 138);
            this.kryptonBorderEdgeEmail2.Name = "kryptonBorderEdgeEmail2";
            this.kryptonBorderEdgeEmail2.Size = new System.Drawing.Size(206, 1);
            this.kryptonBorderEdgeEmail2.TabIndex = 76;
            this.kryptonBorderEdgeEmail2.Text = "kryptonBorderEdge1";
            // 
            // emailNotifyOtherShip
            // 
            this.emailNotifyOtherShip.AutoSize = true;
            this.emailNotifyOtherShip.BackColor = System.Drawing.Color.White;
            this.emailNotifyOtherShip.Location = new System.Drawing.Point(224, 111);
            this.emailNotifyOtherShip.Name = "emailNotifyOtherShip";
            this.emailNotifyOtherShip.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyOtherShip.TabIndex = 75;
            this.emailNotifyOtherShip.UseVisualStyleBackColor = false;
            // 
            // emailNotifyOtherDelivery
            // 
            this.emailNotifyOtherDelivery.AutoSize = true;
            this.emailNotifyOtherDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyOtherDelivery.Location = new System.Drawing.Point(324, 111);
            this.emailNotifyOtherDelivery.Name = "emailNotifyOtherDelivery";
            this.emailNotifyOtherDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyOtherDelivery.TabIndex = 74;
            this.emailNotifyOtherDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyOtherException
            // 
            this.emailNotifyOtherException.AutoSize = true;
            this.emailNotifyOtherException.BackColor = System.Drawing.Color.White;
            this.emailNotifyOtherException.Location = new System.Drawing.Point(267, 111);
            this.emailNotifyOtherException.Name = "emailNotifyOtherException";
            this.emailNotifyOtherException.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyOtherException.TabIndex = 73;
            this.emailNotifyOtherException.UseVisualStyleBackColor = false;
            // 
            // emailNotifyRecipientShip
            // 
            this.emailNotifyRecipientShip.AutoSize = true;
            this.emailNotifyRecipientShip.BackColor = System.Drawing.Color.White;
            this.emailNotifyRecipientShip.Location = new System.Drawing.Point(224, 89);
            this.emailNotifyRecipientShip.Name = "emailNotifyRecipientShip";
            this.emailNotifyRecipientShip.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyRecipientShip.TabIndex = 72;
            this.emailNotifyRecipientShip.UseVisualStyleBackColor = false;
            // 
            // emailNotifyRecipientDelivery
            // 
            this.emailNotifyRecipientDelivery.AutoSize = true;
            this.emailNotifyRecipientDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifyRecipientDelivery.Location = new System.Drawing.Point(324, 89);
            this.emailNotifyRecipientDelivery.Name = "emailNotifyRecipientDelivery";
            this.emailNotifyRecipientDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyRecipientDelivery.TabIndex = 71;
            this.emailNotifyRecipientDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifyRecipientException
            // 
            this.emailNotifyRecipientException.AutoSize = true;
            this.emailNotifyRecipientException.BackColor = System.Drawing.Color.White;
            this.emailNotifyRecipientException.Location = new System.Drawing.Point(267, 89);
            this.emailNotifyRecipientException.Name = "emailNotifyRecipientException";
            this.emailNotifyRecipientException.Size = new System.Drawing.Size(15, 14);
            this.emailNotifyRecipientException.TabIndex = 70;
            this.emailNotifyRecipientException.UseVisualStyleBackColor = false;
            // 
            // emailNotifyOtherAddress
            // 
            this.emailNotifyOtherAddress.Location = new System.Drawing.Point(52, 108);
            this.fieldLengthProvider.SetMaxLengthSource(this.emailNotifyOtherAddress, ShipWorks.Data.Utility.EntityFieldLengthSource.UpsQvnOtherAddress);
            this.emailNotifyOtherAddress.Name = "emailNotifyOtherAddress";
            this.emailNotifyOtherAddress.Size = new System.Drawing.Size(154, 21);
            this.emailNotifyOtherAddress.TabIndex = 69;
            // 
            // kryptonBorderEdgeEmail
            // 
            this.kryptonBorderEdgeEmail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdgeEmail.AutoSize = false;
            this.kryptonBorderEdgeEmail.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdgeEmail.Location = new System.Drawing.Point(11, 61);
            this.kryptonBorderEdgeEmail.Name = "kryptonBorderEdgeEmail";
            this.kryptonBorderEdgeEmail.Size = new System.Drawing.Size(206, 1);
            this.kryptonBorderEdgeEmail.TabIndex = 68;
            this.kryptonBorderEdgeEmail.Text = "kryptonBorderEdge1";
            // 
            // labelEmailOther
            // 
            this.labelEmailOther.AutoSize = true;
            this.labelEmailOther.BackColor = System.Drawing.Color.White;
            this.labelEmailOther.Location = new System.Drawing.Point(17, 111);
            this.labelEmailOther.Name = "labelEmailOther";
            this.labelEmailOther.Size = new System.Drawing.Size(35, 13);
            this.labelEmailOther.TabIndex = 67;
            this.labelEmailOther.Text = "Other";
            // 
            // labelEmailRecipient
            // 
            this.labelEmailRecipient.AutoSize = true;
            this.labelEmailRecipient.BackColor = System.Drawing.Color.White;
            this.labelEmailRecipient.Location = new System.Drawing.Point(17, 89);
            this.labelEmailRecipient.Name = "labelEmailRecipient";
            this.labelEmailRecipient.Size = new System.Drawing.Size(51, 13);
            this.labelEmailRecipient.TabIndex = 66;
            this.labelEmailRecipient.Text = "Recipient";
            // 
            // labelEmailSender
            // 
            this.labelEmailSender.AutoSize = true;
            this.labelEmailSender.BackColor = System.Drawing.Color.White;
            this.labelEmailSender.Location = new System.Drawing.Point(17, 69);
            this.labelEmailSender.Name = "labelEmailSender";
            this.labelEmailSender.Size = new System.Drawing.Size(41, 13);
            this.labelEmailSender.TabIndex = 65;
            this.labelEmailSender.Text = "Sender";
            // 
            // emailNotifySenderShip
            // 
            this.emailNotifySenderShip.AutoSize = true;
            this.emailNotifySenderShip.BackColor = System.Drawing.Color.White;
            this.emailNotifySenderShip.Location = new System.Drawing.Point(224, 68);
            this.emailNotifySenderShip.Name = "emailNotifySenderShip";
            this.emailNotifySenderShip.Size = new System.Drawing.Size(15, 14);
            this.emailNotifySenderShip.TabIndex = 64;
            this.emailNotifySenderShip.UseVisualStyleBackColor = false;
            // 
            // emailNotifySenderDelivery
            // 
            this.emailNotifySenderDelivery.AutoSize = true;
            this.emailNotifySenderDelivery.BackColor = System.Drawing.Color.White;
            this.emailNotifySenderDelivery.Location = new System.Drawing.Point(324, 68);
            this.emailNotifySenderDelivery.Name = "emailNotifySenderDelivery";
            this.emailNotifySenderDelivery.Size = new System.Drawing.Size(15, 14);
            this.emailNotifySenderDelivery.TabIndex = 63;
            this.emailNotifySenderDelivery.UseVisualStyleBackColor = false;
            // 
            // emailNotifySenderException
            // 
            this.emailNotifySenderException.AutoSize = true;
            this.emailNotifySenderException.BackColor = System.Drawing.Color.White;
            this.emailNotifySenderException.Location = new System.Drawing.Point(267, 68);
            this.emailNotifySenderException.Name = "emailNotifySenderException";
            this.emailNotifySenderException.Size = new System.Drawing.Size(15, 14);
            this.emailNotifySenderException.TabIndex = 62;
            this.emailNotifySenderException.UseVisualStyleBackColor = false;
            // 
            // labelEmailDelivery
            // 
            this.labelEmailDelivery.AutoSize = true;
            this.labelEmailDelivery.BackColor = System.Drawing.Color.White;
            this.labelEmailDelivery.Location = new System.Drawing.Point(305, 44);
            this.labelEmailDelivery.Name = "labelEmailDelivery";
            this.labelEmailDelivery.Size = new System.Drawing.Size(46, 13);
            this.labelEmailDelivery.TabIndex = 61;
            this.labelEmailDelivery.Text = "Delivery";
            // 
            // labelEmailException
            // 
            this.labelEmailException.AutoSize = true;
            this.labelEmailException.BackColor = System.Drawing.Color.White;
            this.labelEmailException.Location = new System.Drawing.Point(249, 44);
            this.labelEmailException.Name = "labelEmailException";
            this.labelEmailException.Size = new System.Drawing.Size(54, 13);
            this.labelEmailException.TabIndex = 60;
            this.labelEmailException.Text = "Exception";
            // 
            // labelEmailShip
            // 
            this.labelEmailShip.AutoSize = true;
            this.labelEmailShip.BackColor = System.Drawing.Color.White;
            this.labelEmailShip.Location = new System.Drawing.Point(218, 44);
            this.labelEmailShip.Name = "labelEmailShip";
            this.labelEmailShip.Size = new System.Drawing.Size(27, 13);
            this.labelEmailShip.TabIndex = 59;
            this.labelEmailShip.Text = "Ship";
            // 
            // labelEmailAddress
            // 
            this.labelEmailAddress.AutoSize = true;
            this.labelEmailAddress.BackColor = System.Drawing.Color.White;
            this.labelEmailAddress.Location = new System.Drawing.Point(9, 44);
            this.labelEmailAddress.Name = "labelEmailAddress";
            this.labelEmailAddress.Size = new System.Drawing.Size(73, 13);
            this.labelEmailAddress.TabIndex = 58;
            this.labelEmailAddress.Text = "Email Address";
            // 
            // labelEmailInfo
            // 
            this.labelEmailInfo.BackColor = System.Drawing.Color.White;
            this.labelEmailInfo.Location = new System.Drawing.Point(26, 9);
            this.labelEmailInfo.Name = "labelEmailInfo";
            this.labelEmailInfo.Size = new System.Drawing.Size(332, 29);
            this.labelEmailInfo.TabIndex = 57;
            this.labelEmailInfo.Text = "These settings are for email sent from UPS.  This is separate from and in additio" +
    "n to any email configured to be sent by ShipWorks.";
            // 
            // picturEmailInfo
            // 
            this.picturEmailInfo.BackColor = System.Drawing.Color.White;
            this.picturEmailInfo.Image = global::ShipWorks.Properties.Resources.information16;
            this.picturEmailInfo.Location = new System.Drawing.Point(8, 10);
            this.picturEmailInfo.Name = "picturEmailInfo";
            this.picturEmailInfo.Size = new System.Drawing.Size(16, 16);
            this.picturEmailInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picturEmailInfo.TabIndex = 56;
            this.picturEmailInfo.TabStop = false;
            // 
            // sectionSurePost
            // 
            this.sectionSurePost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // sectionSurePost.ContentPanel
            // 
            this.sectionSurePost.ContentPanel.Controls.Add(this.costCenter);
            this.sectionSurePost.ContentPanel.Controls.Add(this.packageID);
            this.sectionSurePost.ContentPanel.Controls.Add(this.labelPackageID);
            this.sectionSurePost.ContentPanel.Controls.Add(this.irregularIndicator);
            this.sectionSurePost.ContentPanel.Controls.Add(this.labelIrregularIndicator);
            this.sectionSurePost.ContentPanel.Controls.Add(this.labelCostCenter);
            this.sectionSurePost.ContentPanel.Controls.Add(this.surePostClassification);
            this.sectionSurePost.ContentPanel.Controls.Add(this.labelClassification);
            this.sectionSurePost.ExtraText = "";
            this.sectionSurePost.Location = new System.Drawing.Point(3, 1435);
            this.sectionSurePost.Name = "sectionSurePost";
            this.sectionSurePost.SectionName = "SurePost & Mail Innovations";
            this.sectionSurePost.SettingsKey = "{b36e5a50-de19-4dcf-b42f-81399e8f1137}";
            this.sectionSurePost.Size = new System.Drawing.Size(366, 143);
            this.sectionSurePost.TabIndex = 10;
            this.sectionSurePost.Visible = false;
            // 
            // costCenter
            // 
            this.costCenter.Location = new System.Drawing.Point(134, 33);
            this.costCenter.MaxLength = 32767;
            this.costCenter.Name = "costCenter";
            this.costCenter.Size = new System.Drawing.Size(210, 21);
            this.costCenter.TabIndex = 89;
            this.costCenter.TokenSuggestionFactory = commonTokenSuggestionsFactory3;
            // 
            // packageID
            // 
            this.packageID.Location = new System.Drawing.Point(134, 60);
            this.packageID.MaxLength = 32767;
            this.packageID.Name = "packageID";
            this.packageID.Size = new System.Drawing.Size(210, 21);
            this.packageID.TabIndex = 88;
            this.packageID.TokenSuggestionFactory = commonTokenSuggestionsFactory4;
            // 
            // labelPackageID
            // 
            this.labelPackageID.AutoSize = true;
            this.labelPackageID.BackColor = System.Drawing.Color.Transparent;
            this.labelPackageID.Location = new System.Drawing.Point(67, 64);
            this.labelPackageID.Name = "labelPackageID";
            this.labelPackageID.Size = new System.Drawing.Size(65, 13);
            this.labelPackageID.TabIndex = 87;
            this.labelPackageID.Text = "Package ID:";
            // 
            // irregularIndicator
            // 
            this.irregularIndicator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.irregularIndicator.FormattingEnabled = true;
            this.irregularIndicator.Location = new System.Drawing.Point(134, 87);
            this.irregularIndicator.Name = "irregularIndicator";
            this.irregularIndicator.PromptText = "(Multiple Values)";
            this.irregularIndicator.Size = new System.Drawing.Size(210, 21);
            this.irregularIndicator.TabIndex = 85;
            // 
            // labelIrregularIndicator
            // 
            this.labelIrregularIndicator.AutoSize = true;
            this.labelIrregularIndicator.BackColor = System.Drawing.Color.Transparent;
            this.labelIrregularIndicator.Location = new System.Drawing.Point(33, 91);
            this.labelIrregularIndicator.Name = "labelIrregularIndicator";
            this.labelIrregularIndicator.Size = new System.Drawing.Size(99, 13);
            this.labelIrregularIndicator.TabIndex = 84;
            this.labelIrregularIndicator.Text = "Irregular Indicator:";
            // 
            // labelCostCenter
            // 
            this.labelCostCenter.AutoSize = true;
            this.labelCostCenter.BackColor = System.Drawing.Color.Transparent;
            this.labelCostCenter.Location = new System.Drawing.Point(63, 37);
            this.labelCostCenter.Name = "labelCostCenter";
            this.labelCostCenter.Size = new System.Drawing.Size(69, 13);
            this.labelCostCenter.TabIndex = 83;
            this.labelCostCenter.Text = "Cost Center:";
            // 
            // surePostClassification
            // 
            this.surePostClassification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.surePostClassification.FormattingEnabled = true;
            this.surePostClassification.Location = new System.Drawing.Point(134, 7);
            this.surePostClassification.Name = "surePostClassification";
            this.surePostClassification.PromptText = "(Multiple Values)";
            this.surePostClassification.Size = new System.Drawing.Size(210, 21);
            this.surePostClassification.TabIndex = 6;
            // 
            // labelClassification
            // 
            this.labelClassification.AutoSize = true;
            this.labelClassification.BackColor = System.Drawing.Color.Transparent;
            this.labelClassification.Location = new System.Drawing.Point(59, 10);
            this.labelClassification.Name = "labelClassification";
            this.labelClassification.Size = new System.Drawing.Size(73, 13);
            this.labelClassification.TabIndex = 4;
            this.labelClassification.Text = "Classification:";
            // 
            // otherPackageDetails
            // 
            this.otherPackageDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.otherPackageDetails.Collapsed = true;
            // 
            // otherPackageDetails.ContentPanel
            // 
            this.otherPackageDetails.ContentPanel.Controls.Add(this.packageDetailsControl);
            this.otherPackageDetails.ExpandedHeight = 231;
            this.otherPackageDetails.ExtraText = "";
            this.otherPackageDetails.Location = new System.Drawing.Point(3, 433);
            this.otherPackageDetails.Name = "otherPackageDetails";
            this.otherPackageDetails.SectionName = "Other Package Details";
            this.otherPackageDetails.SettingsKey = "d02b34e0-adb3-44fc-880d-da164c96a155";
            this.otherPackageDetails.Size = new System.Drawing.Size(366, 24);
            this.otherPackageDetails.TabIndex = 0;
            // 
            // packageDetailsControl
            // 
            this.packageDetailsControl.BackColor = System.Drawing.Color.White;
            this.packageDetailsControl.Location = new System.Drawing.Point(1, 3);
            this.packageDetailsControl.Name = "packageDetailsControl";
            this.packageDetailsControl.Size = new System.Drawing.Size(431, 181);
            this.packageDetailsControl.TabIndex = 0;
            this.packageDetailsControl.PackageDetailsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.packageDetailsControl.SizeChanged += new System.EventHandler(this.OnPackageDetailsControlSizeChanged);
            // 
            // UpsServiceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.otherPackageDetails);
            this.Controls.Add(this.sectionOptions);
            this.Controls.Add(this.sectionBilling);
            this.Controls.Add(this.sectionEmail);
            this.Controls.Add(this.sectionCod);
            this.Controls.Add(this.sectionSurePost);
            this.Name = "UpsServiceControl";
            this.Size = new System.Drawing.Size(372, 1299);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionSurePost, 0);
            this.Controls.SetChildIndex(this.sectionCod, 0);
            this.Controls.SetChildIndex(this.sectionEmail, 0);
            this.Controls.SetChildIndex(this.sectionBilling, 0);
            this.Controls.SetChildIndex(this.sectionOptions, 0);
            this.Controls.SetChildIndex(this.otherPackageDetails, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionFrom)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCod.ContentPanel)).EndInit();
            this.sectionCod.ContentPanel.ResumeLayout(false);
            this.sectionCod.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionCod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions.ContentPanel)).EndInit();
            this.sectionOptions.ContentPanel.ResumeLayout(false);
            this.sectionOptions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionOptions)).EndInit();
            this.confirmationAndReferenceFlowPanel.ResumeLayout(false);
            this.confirmationPanel.ResumeLayout(false);
            this.confirmationPanel.PerformLayout();
            this.endorsementPanel.ResumeLayout(false);
            this.endorsementPanel.PerformLayout();
            this.referencePanel.ResumeLayout(false);
            this.referencePanel.PerformLayout();
            this.reference2Panel.ResumeLayout(false);
            this.reference2Panel.PerformLayout();
            this.shipperReleasePanel.ResumeLayout(false);
            this.shipperReleasePanel.PerformLayout();
            this.carbonNeutralPanel.ResumeLayout(false);
            this.carbonNeutralPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling.ContentPanel)).EndInit();
            this.sectionBilling.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sectionBilling)).EndInit();
            this.panelPayorTransport.ResumeLayout(false);
            this.panelPayorTransport.PerformLayout();
            this.panelPayorDuties.ResumeLayout(false);
            this.panelPayorDuties.PerformLayout();
            this.panelDutiesAccount.ResumeLayout(false);
            this.panelDutiesAccount.PerformLayout();
            this.panelTransportAccount.ResumeLayout(false);
            this.panelTransportAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail.ContentPanel)).EndInit();
            this.sectionEmail.ContentPanel.ResumeLayout(false);
            this.sectionEmail.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picturEmailInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fieldLengthProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSurePost.ContentPanel)).EndInit();
            this.sectionSurePost.ContentPanel.ResumeLayout(false);
            this.sectionSurePost.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sectionSurePost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.otherPackageDetails.ContentPanel)).EndInit();
            this.otherPackageDetails.ContentPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.otherPackageDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionFrom;
        private ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl originControl;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ShipWorks.UI.Controls.MultiValueComboBox upsAccount;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelService;
        private UpsPackageControl packageControl;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private System.Windows.Forms.Label labelShipDate;
        private ShipWorks.UI.Controls.MultiValueDateTimePicker shipDate;
        private System.Windows.Forms.RadioButton insuranceUps;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionCod;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionOptions;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionBilling;
        private ShipWorks.UI.Controls.CollapsibleGroupControl sectionEmail;
        private System.Windows.Forms.Panel panelTransportAccount;
        private System.Windows.Forms.Label labelPayorAccount;
        private ShipWorks.UI.Controls.MultiValueTextBox payorPostalCode;
        private ShipWorks.UI.Controls.MultiValueTextBox payorAccount;
        private System.Windows.Forms.Label labelPayorPostalCode;
        private ShipWorks.UI.Controls.MultiValueComboBox payorCountry;
        private System.Windows.Forms.Label labelPayorCountry;
        private System.Windows.Forms.Label labelEmailSubject;
        private ShipWorks.UI.Controls.MultiValueTextBox emailFrom;
        private System.Windows.Forms.Label labelEmailFrom;
        private ShipWorks.UI.Controls.MultiValueTextBox emailNotifyMessage;
        private System.Windows.Forms.Label labelPersonalMessage;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdgeEmail2;
        private System.Windows.Forms.CheckBox emailNotifyOtherShip;
        private System.Windows.Forms.CheckBox emailNotifyOtherDelivery;
        private System.Windows.Forms.CheckBox emailNotifyOtherException;
        private System.Windows.Forms.CheckBox emailNotifyRecipientShip;
        private System.Windows.Forms.CheckBox emailNotifyRecipientDelivery;
        private System.Windows.Forms.CheckBox emailNotifyRecipientException;
        private ShipWorks.UI.Controls.MultiValueTextBox emailNotifyOtherAddress;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdgeEmail;
        private System.Windows.Forms.Label labelEmailOther;
        private System.Windows.Forms.Label labelEmailRecipient;
        private System.Windows.Forms.Label labelEmailSender;
        private System.Windows.Forms.CheckBox emailNotifySenderShip;
        private System.Windows.Forms.CheckBox emailNotifySenderDelivery;
        private System.Windows.Forms.CheckBox emailNotifySenderException;
        private System.Windows.Forms.Label labelEmailDelivery;
        private System.Windows.Forms.Label labelEmailException;
        private System.Windows.Forms.Label labelEmailShip;
        private System.Windows.Forms.Label labelEmailAddress;
        private System.Windows.Forms.Label labelEmailInfo;
        private System.Windows.Forms.PictureBox picturEmailInfo;
        private ShipWorks.UI.Controls.MultiValueComboBox emailSubject;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.InfoTip infotipShipDate;
        private UI.Controls.MultiValueComboBox codPaymentType;
        private System.Windows.Forms.CheckBox codEnabled;
        private System.Windows.Forms.Label labelCodPayment;
        private System.Windows.Forms.Label labelCodAmount;
        private UI.Controls.MoneyTextBox codAmount;
        private System.Windows.Forms.Panel reference2Panel;
        private Templates.Tokens.TemplateTokenTextBox reference2Number;
        private System.Windows.Forms.Label label3;
        private UI.Controls.CollapsibleGroupControl sectionSurePost;
        private UI.Controls.MultiValueComboBox surePostClassification;
        private System.Windows.Forms.Label labelClassification;
        private System.Windows.Forms.FlowLayoutPanel confirmationAndReferenceFlowPanel;
        private System.Windows.Forms.Panel confirmationPanel;
        private UI.Controls.MultiValueComboBox confirmation;
        private System.Windows.Forms.Label labelConfirmation;
        private System.Windows.Forms.Panel referencePanel;
        private Templates.Tokens.TemplateTokenTextBox referenceNumber;
        private System.Windows.Forms.Label labelReference;
        private System.Windows.Forms.Panel endorsementPanel;
        private UI.Controls.MultiValueComboBox uspsEndorsement;
        private System.Windows.Forms.Label labelUspsEndorsement;
        private System.Windows.Forms.Panel shipperReleasePanel;
        private System.Windows.Forms.Label shipperReleaseLabel;
        private System.Windows.Forms.Panel carbonNeutralPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox shipperRelease;
        private System.Windows.Forms.CheckBox carbonNeutral;
        private ShipWorks.UI.Controls.CollapsibleGroupControl otherPackageDetails;
        private UpsPackageDetailControl packageDetailsControl;
        private System.Windows.Forms.Label labelCostCenter;
        private MultiValueComboBox irregularIndicator;
        private System.Windows.Forms.Label labelIrregularIndicator;
        private System.Windows.Forms.Panel panelPayorDuties;
        private MultiValueComboBox payorDuties;
        private System.Windows.Forms.Label labelPayorDuties;
        private System.Windows.Forms.Panel panelPayorTransport;
        private MultiValueComboBox payorTransport;
        private System.Windows.Forms.Label labelPayor;
        private System.Windows.Forms.Panel panelDutiesAccount;
        private System.Windows.Forms.Label labelDutiesAccount;
        private MultiValueTextBox dutiesPostalCode;
        private MultiValueTextBox dutiesAccount;
        private System.Windows.Forms.Label labelDutiesPostalCode;
        private MultiValueComboBox dutiesCountryCode;
        private System.Windows.Forms.Label labelDutiesCountryCode;
        private Templates.Tokens.TemplateTokenTextBox costCenter;
        private Templates.Tokens.TemplateTokenTextBox packageID;
        private System.Windows.Forms.Label labelPackageID;
    }
}
