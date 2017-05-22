namespace ShipWorks.Shipping.Carriers.OnTrac
{
    partial class OnTracServiceControl
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
            this.sectionFrom = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.originControl = new ShipWorks.Shipping.Settings.Origin.ShipmentOriginControl();
            this.accountSectionLabel = new System.Windows.Forms.Label();
            this.onTracAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.accountLabel = new System.Windows.Forms.Label();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.referenceNumber = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.referenceNumber2 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.instructions = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.sectionReferenceInstructions = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.labelReference2 = new System.Windows.Forms.Label();
            this.labelReference = new System.Windows.Forms.Label();
            this.sectionCod = new ShipWorks.UI.Controls.CollapsibleGroupControl();
            this.codPaymentType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelCodPayment = new System.Windows.Forms.Label();
            this.codEnabled = new System.Windows.Forms.CheckBox();
            this.labelCodAmount = new System.Windows.Forms.Label();
            this.codAmount = new ShipWorks.UI.Controls.MoneyTextBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelShipDate = new System.Windows.Forms.Label();
            this.shipDate = new ShipWorks.UI.Controls.MultiValueDateTimePicker();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceSelectionControl();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.label3 = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.packagingType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.signatureRequired = new System.Windows.Forms.CheckBox();
            this.labelSignatureRequired = new System.Windows.Forms.Label();
            this.labelSaturday = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient.ContentPanel)).BeginInit();
            this.sectionRecipient.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns.ContentPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment.ContentPanel)).BeginInit();
            this.sectionShipment.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom.ContentPanel)).BeginInit();
            this.sectionFrom.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReferenceInstructions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReferenceInstructions.ContentPanel)).BeginInit();
            this.sectionReferenceInstructions.ContentPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionCod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionCod.ContentPanel)).BeginInit();
            this.sectionCod.ContentPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // sectionRecipient
            //
            this.sectionRecipient.Location = new System.Drawing.Point(3, 34);
            this.sectionRecipient.Size = new System.Drawing.Size(399, 24);
            //
            // personControl
            //
            this.personControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.personControl.Size = new System.Drawing.Size(389, 333);
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
            this.residentialDetermination.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.residentialDetermination.Location = new System.Drawing.Point(79, 358);
            this.residentialDetermination.Size = new System.Drawing.Size(300, 21);
            this.residentialDetermination.TextChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // sectionReturns
            //
            this.sectionReturns.Location = new System.Drawing.Point(3, 458);
            this.sectionReturns.Size = new System.Drawing.Size(399, 24);

            this.sectionLabelOptions.Size = new System.Drawing.Size(399, 24);
            //
            // sectionShipment
            //
            //
            // sectionShipment.ContentPanel
            //
            this.sectionShipment.ContentPanel.Controls.Add(this.labelSaturday);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelSignatureRequired);
            this.sectionShipment.ContentPanel.Controls.Add(this.signatureRequired);
            this.sectionShipment.ContentPanel.Controls.Add(this.packagingType);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelPackaging);
            this.sectionShipment.ContentPanel.Controls.Add(this.insuranceControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.dimensionsControl);
            this.sectionShipment.ContentPanel.Controls.Add(this.label3);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelWeight);
            this.sectionShipment.ContentPanel.Controls.Add(this.weight);
            this.sectionShipment.ContentPanel.Controls.Add(this.saturdayDelivery);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelService);
            this.sectionShipment.ContentPanel.Controls.Add(this.service);
            this.sectionShipment.ContentPanel.Controls.Add(this.labelShipDate);
            this.sectionShipment.ContentPanel.Controls.Add(this.shipDate);
            this.sectionShipment.Location = new System.Drawing.Point(3, 63);
            this.sectionShipment.Size = new System.Drawing.Size(399, 332);
            //
            // sectionFrom
            //
            this.sectionFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionFrom.Collapsed = true;
            //
            // sectionFrom.ContentPanel
            //
            this.sectionFrom.ContentPanel.Controls.Add(this.originControl);
            this.sectionFrom.ContentPanel.Controls.Add(this.accountSectionLabel);
            this.sectionFrom.ContentPanel.Controls.Add(this.onTracAccount);
            this.sectionFrom.ContentPanel.Controls.Add(this.accountLabel);
            this.sectionFrom.ExpandedHeight = 438;
            this.sectionFrom.ExtraText = "";
            this.sectionFrom.Location = new System.Drawing.Point(3, 5);
            this.sectionFrom.Name = "sectionFrom";
            this.sectionFrom.SectionName = "From";
            this.sectionFrom.SettingsKey = "6306b47c-8029-44bc-8b97-9b9eb001a61a";
            this.sectionFrom.Size = new System.Drawing.Size(399, 24);
            this.sectionFrom.TabIndex = 5;
            //
            // originControl
            //
            this.originControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.originControl.AvailableFields = ((ShipWorks.Data.Controls.PersonFields) (((((((((ShipWorks.Data.Controls.PersonFields.Name | ShipWorks.Data.Controls.PersonFields.Company)
            | ShipWorks.Data.Controls.PersonFields.Street)
            | ShipWorks.Data.Controls.PersonFields.City)
            | ShipWorks.Data.Controls.PersonFields.State)
            | ShipWorks.Data.Controls.PersonFields.Postal)
            | ShipWorks.Data.Controls.PersonFields.Residential)
            | ShipWorks.Data.Controls.PersonFields.Email)
            | ShipWorks.Data.Controls.PersonFields.Phone)));
            this.originControl.BackColor = System.Drawing.Color.Transparent;
            this.originControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.originControl.Location = new System.Drawing.Point(1, 60);
            this.originControl.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.originControl.Name = "originControl";
            this.originControl.Size = new System.Drawing.Size(391, 355);
            this.originControl.TabIndex = 9;
            this.originControl.OriginChanged += new System.EventHandler(this.OnOriginChanged);
            //
            // accountSectionLabel
            //
            this.accountSectionLabel.AutoSize = true;
            this.accountSectionLabel.BackColor = System.Drawing.Color.Transparent;
            this.accountSectionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.accountSectionLabel.Location = new System.Drawing.Point(3, 10);
            this.accountSectionLabel.Name = "accountSectionLabel";
            this.accountSectionLabel.Size = new System.Drawing.Size(96, 13);
            this.accountSectionLabel.TabIndex = 5;
            this.accountSectionLabel.Text = "OnTrac Account";
            //
            // onTracAccount
            //
            this.onTracAccount.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.onTracAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.onTracAccount.FormattingEnabled = true;
            this.onTracAccount.Location = new System.Drawing.Point(80, 32);
            this.onTracAccount.Name = "onTracAccount";
            this.onTracAccount.PromptText = "(Multiple Values)";
            this.onTracAccount.Size = new System.Drawing.Size(298, 21);
            this.onTracAccount.TabIndex = 4;
            this.onTracAccount.SelectedIndexChanged += new System.EventHandler(this.OnChangeAccount);
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
            // referenceNumber
            //
            this.referenceNumber.Location = new System.Drawing.Point(101, 8);
            this.referenceNumber.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceNumber, ShipWorks.Data.Utility.EntityFieldLengthSource.OnTracReference1);
            this.referenceNumber.Name = "referenceNumber";
            this.referenceNumber.Size = new System.Drawing.Size(210, 21);
            this.referenceNumber.TabIndex = 78;
            this.referenceNumber.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            //
            // referenceNumber2
            //
            this.referenceNumber2.Location = new System.Drawing.Point(101, 34);
            this.referenceNumber2.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceNumber2, ShipWorks.Data.Utility.EntityFieldLengthSource.OnTracReference2);
            this.referenceNumber2.Name = "referenceNumber2";
            this.referenceNumber2.Size = new System.Drawing.Size(210, 21);
            this.referenceNumber2.TabIndex = 80;
            this.referenceNumber2.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            //
            // instructions
            //
            this.instructions.Location = new System.Drawing.Point(101, 60);
            this.instructions.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.instructions, ShipWorks.Data.Utility.EntityFieldLengthSource.OnTracInstructions);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(210, 21);
            this.instructions.TabIndex = 82;
            this.instructions.TokenSuggestionFactory = commonTokenSuggestionsFactory3;
            //
            // sectionReferenceInstructions
            //
            this.sectionReferenceInstructions.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionReferenceInstructions.Collapsed = true;
            //
            // sectionReferenceInstructions.ContentPanel
            //
            this.sectionReferenceInstructions.ContentPanel.Controls.Add(this.instructions);
            this.sectionReferenceInstructions.ContentPanel.Controls.Add(this.instructionsLabel);
            this.sectionReferenceInstructions.ContentPanel.Controls.Add(this.referenceNumber2);
            this.sectionReferenceInstructions.ContentPanel.Controls.Add(this.labelReference2);
            this.sectionReferenceInstructions.ContentPanel.Controls.Add(this.referenceNumber);
            this.sectionReferenceInstructions.ContentPanel.Controls.Add(this.labelReference);
            this.sectionReferenceInstructions.ExpandedHeight = 115;
            this.sectionReferenceInstructions.ExtraText = "";
            this.sectionReferenceInstructions.Location = new System.Drawing.Point(3, 429);
            this.sectionReferenceInstructions.Name = "sectionReferenceInstructions";
            this.sectionReferenceInstructions.SectionName = "Reference & Instructions";
            this.sectionReferenceInstructions.SettingsKey = "{7A064A2F-0914-4A65-A993-DC3E8E70E054}";
            this.sectionReferenceInstructions.Size = new System.Drawing.Size(399, 24);
            this.sectionReferenceInstructions.TabIndex = 7;
            //
            // instructionsLabel
            //
            this.instructionsLabel.AutoSize = true;
            this.instructionsLabel.BackColor = System.Drawing.Color.Transparent;
            this.instructionsLabel.Location = new System.Drawing.Point(27, 64);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(68, 13);
            this.instructionsLabel.TabIndex = 83;
            this.instructionsLabel.Text = "Instructions:";
            //
            // labelReference2
            //
            this.labelReference2.AutoSize = true;
            this.labelReference2.BackColor = System.Drawing.Color.Transparent;
            this.labelReference2.Location = new System.Drawing.Point(25, 39);
            this.labelReference2.Name = "labelReference2";
            this.labelReference2.Size = new System.Drawing.Size(70, 13);
            this.labelReference2.TabIndex = 81;
            this.labelReference2.Text = "Reference 2:";
            //
            // labelReference
            //
            this.labelReference.AutoSize = true;
            this.labelReference.BackColor = System.Drawing.Color.Transparent;
            this.labelReference.Location = new System.Drawing.Point(34, 12);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(61, 13);
            this.labelReference.TabIndex = 79;
            this.labelReference.Text = "Reference:";
            //
            // sectionCod
            //
            this.sectionCod.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionCod.Collapsed = true;
            //
            // sectionCod.ContentPanel
            //
            this.sectionCod.ContentPanel.Controls.Add(this.codPaymentType);
            this.sectionCod.ContentPanel.Controls.Add(this.labelCodPayment);
            this.sectionCod.ContentPanel.Controls.Add(this.codEnabled);
            this.sectionCod.ContentPanel.Controls.Add(this.labelCodAmount);
            this.sectionCod.ContentPanel.Controls.Add(this.codAmount);
            this.sectionCod.ExpandedHeight = 117;
            this.sectionCod.ExtraText = "";
            this.sectionCod.Location = new System.Drawing.Point(3, 400);
            this.sectionCod.Name = "sectionCod";
            this.sectionCod.SectionName = "C.O.D.";
            this.sectionCod.SettingsKey = "{640124b8-f610-4488-b282-7e2c36618b81}";
            this.sectionCod.Size = new System.Drawing.Size(399, 24);
            this.sectionCod.TabIndex = 10;
            //
            // codPaymentType
            //
            this.codPaymentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codPaymentType.FormattingEnabled = true;
            this.codPaymentType.Location = new System.Drawing.Point(92, 60);
            this.codPaymentType.Name = "codPaymentType";
            this.codPaymentType.PromptText = "(Multiple Values)";
            this.codPaymentType.Size = new System.Drawing.Size(210, 21);
            this.codPaymentType.TabIndex = 67;
            //
            // labelCodPayment
            //
            this.labelCodPayment.AutoSize = true;
            this.labelCodPayment.BackColor = System.Drawing.Color.White;
            this.labelCodPayment.Location = new System.Drawing.Point(36, 63);
            this.labelCodPayment.Name = "labelCodPayment";
            this.labelCodPayment.Size = new System.Drawing.Size(53, 13);
            this.labelCodPayment.TabIndex = 68;
            this.labelCodPayment.Text = "Payment:";
            //
            // codEnabled
            //
            this.codEnabled.AutoSize = true;
            this.codEnabled.BackColor = System.Drawing.Color.White;
            this.codEnabled.Location = new System.Drawing.Point(13, 11);
            this.codEnabled.Name = "codEnabled";
            this.codEnabled.Size = new System.Drawing.Size(160, 17);
            this.codEnabled.TabIndex = 64;
            this.codEnabled.Text = "C.O.D. (Collect on Delivery)";
            this.codEnabled.UseVisualStyleBackColor = false;
            //
            // labelCodAmount
            //
            this.labelCodAmount.AutoSize = true;
            this.labelCodAmount.BackColor = System.Drawing.Color.White;
            this.labelCodAmount.Location = new System.Drawing.Point(41, 36);
            this.labelCodAmount.Name = "labelCodAmount";
            this.labelCodAmount.Size = new System.Drawing.Size(48, 13);
            this.labelCodAmount.TabIndex = 66;
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
            this.codAmount.TabIndex = 65;
            this.codAmount.Text = "$0.00";
            this.codAmount.TextChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // saturdayDelivery
            //
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.BackColor = System.Drawing.Color.White;
            this.saturdayDelivery.Location = new System.Drawing.Point(80, 69);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 67;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = false;
            this.saturdayDelivery.CheckedChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // labelService
            //
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(28, 15);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 68;
            this.labelService.Text = "Service:";
            //
            // service
            //
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(80, 12);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(175, 21);
            this.service.TabIndex = 65;
            this.service.SelectedIndexChanged += new System.EventHandler(this.OnServiceChanged);
            //
            // labelShipDate
            //
            this.labelShipDate.AutoSize = true;
            this.labelShipDate.BackColor = System.Drawing.Color.Transparent;
            this.labelShipDate.Location = new System.Drawing.Point(18, 43);
            this.labelShipDate.Name = "labelShipDate";
            this.labelShipDate.Size = new System.Drawing.Size(56, 13);
            this.labelShipDate.TabIndex = 69;
            this.labelShipDate.Text = "Ship date:";
            //
            // shipDate
            //
            this.shipDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.shipDate.Location = new System.Drawing.Point(80, 39);
            this.shipDate.Name = "shipDate";
            this.shipDate.Size = new System.Drawing.Size(144, 21);
            this.shipDate.TabIndex = 66;
            //
            // insuranceControl
            //
            this.insuranceControl.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.insuranceControl.BackColor = System.Drawing.Color.Transparent;
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(11, 251);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(493, 50);
            this.insuranceControl.TabIndex = 74;
            this.insuranceControl.InsuranceOptionsChanged += OnRateCriteriaChanged;
            //
            // dimensionsControl
            //
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dimensionsControl.Location = new System.Drawing.Point(77, 172);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 73;
            this.dimensionsControl.DimensionsChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.dimensionsControl.DimensionsChanged += OnShipSenseFieldChanged;
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(10, 178);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 72;
            this.label3.Text = "Dimensions:";
            //
            // labelWeight
            //
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(29, 121);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 70;
            this.labelWeight.Text = "Weight:";
            //
            // weight
            //
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(80, 118);
            this.weight.Name = "weight";
            this.weight.RangeMax = 9999D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(269, 21);
            this.weight.TabIndex = 71;
            this.weight.Weight = 0D;
            this.weight.WeightChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            this.weight.WeightChanged += OnShipSenseFieldChanged;
            this.weight.ShowShortcutInfo = true;
            //
            // packagingType
            //
            this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagingType.FormattingEnabled = true;
            this.packagingType.Location = new System.Drawing.Point(80, 147);
            this.packagingType.Name = "packagingType";
            this.packagingType.PromptText = "(Multiple Values)";
            this.packagingType.Size = new System.Drawing.Size(175, 21);
            this.packagingType.TabIndex = 75;
            this.packagingType.SelectedIndexChanged += new System.EventHandler(this.OnRateCriteriaChanged);
            //
            // labelPackaging
            //
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(15, 150);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 76;
            this.labelPackaging.Text = "Packaging:";
            //
            // signatureRequired
            //
            this.signatureRequired.AutoSize = true;
            this.signatureRequired.BackColor = System.Drawing.Color.White;
            this.signatureRequired.Cursor = System.Windows.Forms.Cursors.Default;
            this.signatureRequired.Location = new System.Drawing.Point(80, 93);
            this.signatureRequired.Name = "signatureRequired";
            this.signatureRequired.Size = new System.Drawing.Size(118, 17);
            this.signatureRequired.TabIndex = 77;
            this.signatureRequired.Text = "Signature Required";
            this.signatureRequired.UseVisualStyleBackColor = false;
            //
            // labelSignatureRequired
            //
            this.labelSignatureRequired.AutoSize = true;
            this.labelSignatureRequired.BackColor = System.Drawing.Color.Transparent;
            this.labelSignatureRequired.Location = new System.Drawing.Point(17, 93);
            this.labelSignatureRequired.Name = "labelSignatureRequired";
            this.labelSignatureRequired.Size = new System.Drawing.Size(57, 13);
            this.labelSignatureRequired.TabIndex = 78;
            this.labelSignatureRequired.Text = "Signature:";
            //
            // labelSaturday
            //
            this.labelSaturday.AutoSize = true;
            this.labelSaturday.BackColor = System.Drawing.Color.Transparent;
            this.labelSaturday.Location = new System.Drawing.Point(18, 70);
            this.labelSaturday.Name = "labelSaturday";
            this.labelSaturday.Size = new System.Drawing.Size(55, 13);
            this.labelSaturday.TabIndex = 79;
            this.labelSaturday.Text = "Saturday:";
            //
            // OnTracServiceControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.sectionFrom);
            this.Controls.Add(this.sectionCod);
            this.Controls.Add(this.sectionReferenceInstructions);
            this.Name = "OnTracServiceControl";
            this.Size = new System.Drawing.Size(405, 651);
            this.Controls.SetChildIndex(this.sectionReturns, 0);
            this.Controls.SetChildIndex(this.sectionReferenceInstructions, 0);
            this.Controls.SetChildIndex(this.sectionCod, 0);
            this.Controls.SetChildIndex(this.sectionShipment, 0);
            this.Controls.SetChildIndex(this.sectionRecipient, 0);
            this.Controls.SetChildIndex(this.sectionFrom, 0);
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient.ContentPanel)).EndInit();
            this.sectionRecipient.ContentPanel.ResumeLayout(false);
            this.sectionRecipient.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionRecipient)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReturns)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions.ContentPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionLabelOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment.ContentPanel)).EndInit();
            this.sectionShipment.ContentPanel.ResumeLayout(false);
            this.sectionShipment.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionShipment)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom.ContentPanel)).EndInit();
            this.sectionFrom.ContentPanel.ResumeLayout(false);
            this.sectionFrom.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReferenceInstructions.ContentPanel)).EndInit();
            this.sectionReferenceInstructions.ContentPanel.ResumeLayout(false);
            this.sectionReferenceInstructions.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionReferenceInstructions)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.sectionCod.ContentPanel)).EndInit();
            this.sectionCod.ContentPanel.ResumeLayout(false);
            this.sectionCod.ContentPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.sectionCod)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Controls.CollapsibleGroupControl sectionFrom;
        private System.Windows.Forms.Label accountLabel;
        private UI.Controls.MultiValueComboBox onTracAccount;
        private System.Windows.Forms.Label accountSectionLabel;
        private Settings.Origin.ShipmentOriginControl originControl;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private UI.Controls.CollapsibleGroupControl sectionReferenceInstructions;
        private Templates.Tokens.TemplateTokenTextBox referenceNumber;
        private System.Windows.Forms.Label labelReference;
        private Templates.Tokens.TemplateTokenTextBox referenceNumber2;
        private System.Windows.Forms.Label labelReference2;
        private UI.Controls.CollapsibleGroupControl sectionCod;
        private System.Windows.Forms.CheckBox codEnabled;
        private System.Windows.Forms.Label labelCodAmount;
        private UI.Controls.MoneyTextBox codAmount;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private System.Windows.Forms.Label labelService;
        private UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.Label labelShipDate;
        private UI.Controls.MultiValueDateTimePicker shipDate;
        protected Insurance.InsuranceSelectionControl insuranceControl;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelWeight;
        private UI.Controls.WeightControl weight;
        private UI.Controls.MultiValueComboBox codPaymentType;
        private System.Windows.Forms.Label labelCodPayment;
        private Templates.Tokens.TemplateTokenTextBox instructions;
        private System.Windows.Forms.Label instructionsLabel;
        private UI.Controls.MultiValueComboBox packagingType;
        private System.Windows.Forms.Label labelPackaging;
        private System.Windows.Forms.Label labelSignatureRequired;
        private System.Windows.Forms.CheckBox signatureRequired;
        private System.Windows.Forms.Label labelSaturday;



    }
}
