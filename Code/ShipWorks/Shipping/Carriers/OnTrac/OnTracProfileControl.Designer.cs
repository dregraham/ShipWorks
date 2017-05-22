namespace ShipWorks.Shipping.Carriers.OnTrac
{
    partial class OnTracProfileControl
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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupOptions = new System.Windows.Forms.GroupBox();
            this.instructionsState = new System.Windows.Forms.CheckBox();
            this.instructions = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelInstructions = new System.Windows.Forms.Label();
            this.reference2State = new System.Windows.Forms.CheckBox();
            this.referenceNumber2 = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReference2 = new System.Windows.Forms.Label();
            this.kryptonBorderEdge4 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.referenceState = new System.Windows.Forms.CheckBox();
            this.referenceNumber = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReference = new System.Windows.Forms.Label();
            this.groupInsurance = new System.Windows.Forms.GroupBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
            this.insuranceState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge10 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupTo = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge6 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelResidential = new System.Windows.Forms.Label();
            this.residentialDetermination = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.residentialState = new System.Windows.Forms.CheckBox();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.signatureRequired = new System.Windows.Forms.CheckBox();
            this.labelSignature = new System.Windows.Forms.Label();
            this.signatureState = new System.Windows.Forms.CheckBox();
            this.dimensionsState = new System.Windows.Forms.CheckBox();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.weightState = new System.Windows.Forms.CheckBox();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.packaging = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.packagingState = new System.Windows.Forms.CheckBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.labelSaturday = new System.Windows.Forms.Label();
            this.saturdayState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.labelAccount = new System.Windows.Forms.Label();
            this.onTracAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.accountState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge11 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.tabControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupOptions.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupTo.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.groupBoxFrom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.groupLabels.SuspendLayout();
            this.SuspendLayout();
            //
            // tabControl
            //
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(425, 749);
            this.tabControl.TabIndex = 0;
            //
            // tabPageSettings
            //
            this.tabPageSettings.AutoScroll = true;
            this.tabPageSettings.Controls.Add(this.groupLabels);
            this.tabPageSettings.Controls.Add(this.groupOptions);
            this.tabPageSettings.Controls.Add(this.groupInsurance);
            this.tabPageSettings.Controls.Add(this.groupTo);
            this.tabPageSettings.Controls.Add(this.groupShipment);
            this.tabPageSettings.Controls.Add(this.groupBoxFrom);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(417, 723);
            this.tabPageSettings.TabIndex = 0;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            //
            // groupOptions
            //
            this.groupOptions.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupOptions.Controls.Add(this.instructionsState);
            this.groupOptions.Controls.Add(this.instructions);
            this.groupOptions.Controls.Add(this.labelInstructions);
            this.groupOptions.Controls.Add(this.reference2State);
            this.groupOptions.Controls.Add(this.referenceNumber2);
            this.groupOptions.Controls.Add(this.labelReference2);
            this.groupOptions.Controls.Add(this.kryptonBorderEdge4);
            this.groupOptions.Controls.Add(this.referenceState);
            this.groupOptions.Controls.Add(this.referenceNumber);
            this.groupOptions.Controls.Add(this.labelReference);
            this.groupOptions.Location = new System.Drawing.Point(3, 532);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new System.Drawing.Size(405, 111);
            this.groupOptions.TabIndex = 12;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            //
            // instructionsState
            //
            this.instructionsState.AutoSize = true;
            this.instructionsState.Checked = true;
            this.instructionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.instructionsState.Location = new System.Drawing.Point(9, 80);
            this.instructionsState.Name = "instructionsState";
            this.instructionsState.Size = new System.Drawing.Size(15, 14);
            this.instructionsState.TabIndex = 80;
            this.instructionsState.Tag = "";
            this.instructionsState.UseVisualStyleBackColor = true;
            //
            // instructions
            //
            this.instructions.Location = new System.Drawing.Point(139, 76);
            this.instructions.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.instructions, ShipWorks.Data.Utility.EntityFieldLengthSource.OnTracInstructions);
            this.instructions.Name = "instructions";
            this.instructions.Size = new System.Drawing.Size(222, 21);
            this.instructions.TabIndex = 79;
            this.instructions.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            //
            // labelInstructions
            //
            this.labelInstructions.AutoSize = true;
            this.labelInstructions.BackColor = System.Drawing.Color.Transparent;
            this.labelInstructions.Location = new System.Drawing.Point(64, 80);
            this.labelInstructions.Name = "labelInstructions";
            this.labelInstructions.Size = new System.Drawing.Size(68, 13);
            this.labelInstructions.TabIndex = 78;
            this.labelInstructions.Text = "Instructions:";
            //
            // reference2State
            //
            this.reference2State.AutoSize = true;
            this.reference2State.Checked = true;
            this.reference2State.CheckState = System.Windows.Forms.CheckState.Checked;
            this.reference2State.Location = new System.Drawing.Point(9, 54);
            this.reference2State.Name = "reference2State";
            this.reference2State.Size = new System.Drawing.Size(15, 14);
            this.reference2State.TabIndex = 77;
            this.reference2State.Tag = "";
            this.reference2State.UseVisualStyleBackColor = true;
            //
            // referenceNumber2
            //
            this.referenceNumber2.Location = new System.Drawing.Point(139, 49);
            this.referenceNumber2.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceNumber2, ShipWorks.Data.Utility.EntityFieldLengthSource.OnTracReference2);
            this.referenceNumber2.Name = "referenceNumber2";
            this.referenceNumber2.Size = new System.Drawing.Size(222, 21);
            this.referenceNumber2.TabIndex = 76;
            this.referenceNumber2.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
            //
            // labelReference2
            //
            this.labelReference2.AutoSize = true;
            this.labelReference2.BackColor = System.Drawing.Color.Transparent;
            this.labelReference2.Location = new System.Drawing.Point(61, 54);
            this.labelReference2.Name = "labelReference2";
            this.labelReference2.Size = new System.Drawing.Size(70, 13);
            this.labelReference2.TabIndex = 75;
            this.labelReference2.Text = "Reference 2:";
            //
            // kryptonBorderEdge4
            //
            this.kryptonBorderEdge4.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge4.AutoSize = false;
            this.kryptonBorderEdge4.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge4.Location = new System.Drawing.Point(29, 19);
            this.kryptonBorderEdge4.Name = "kryptonBorderEdge4";
            this.kryptonBorderEdge4.Size = new System.Drawing.Size(1, 80);
            this.kryptonBorderEdge4.Text = "kryptonBorderEdge1";
            //
            // referenceState
            //
            this.referenceState.AutoSize = true;
            this.referenceState.Checked = true;
            this.referenceState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.referenceState.Location = new System.Drawing.Point(9, 26);
            this.referenceState.Name = "referenceState";
            this.referenceState.Size = new System.Drawing.Size(15, 14);
            this.referenceState.TabIndex = 2;
            this.referenceState.Tag = "";
            this.referenceState.UseVisualStyleBackColor = true;
            //
            // referenceNumber
            //
            this.referenceNumber.Location = new System.Drawing.Point(139, 23);
            this.referenceNumber.MaxLength = 32767;
            this.fieldLengthProvider.SetMaxLengthSource(this.referenceNumber, ShipWorks.Data.Utility.EntityFieldLengthSource.OnTracReference1);
            this.referenceNumber.Name = "referenceNumber";
            this.referenceNumber.Size = new System.Drawing.Size(222, 21);
            this.referenceNumber.TabIndex = 3;
            this.referenceNumber.TokenSuggestionFactory = commonTokenSuggestionsFactory3;
            //
            // labelReference
            //
            this.labelReference.AutoSize = true;
            this.labelReference.BackColor = System.Drawing.Color.Transparent;
            this.labelReference.Location = new System.Drawing.Point(70, 26);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(61, 13);
            this.labelReference.TabIndex = 70;
            this.labelReference.Text = "Reference:";
            //
            // groupInsurance
            //
            this.groupInsurance.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupInsurance.Controls.Add(this.insuranceControl);
            this.groupInsurance.Controls.Add(this.insuranceState);
            this.groupInsurance.Controls.Add(this.kryptonBorderEdge10);
            this.groupInsurance.Location = new System.Drawing.Point(3, 444);
            this.groupInsurance.Name = "groupInsurance";
            this.groupInsurance.Size = new System.Drawing.Size(405, 82);
            this.groupInsurance.TabIndex = 11;
            this.groupInsurance.TabStop = false;
            this.groupInsurance.Text = "Insurance";
            //
            // insuranceControl
            //
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(45, 21);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(316, 52);
            this.insuranceControl.TabIndex = 97;
            //
            // insuranceState
            //
            this.insuranceState.AutoSize = true;
            this.insuranceState.Location = new System.Drawing.Point(9, 25);
            this.insuranceState.Name = "insuranceState";
            this.insuranceState.Size = new System.Drawing.Size(15, 14);
            this.insuranceState.TabIndex = 0;
            this.insuranceState.UseVisualStyleBackColor = true;
            //
            // kryptonBorderEdge10
            //
            this.kryptonBorderEdge10.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge10.AutoSize = false;
            this.kryptonBorderEdge10.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge10.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge10.Name = "kryptonBorderEdge10";
            this.kryptonBorderEdge10.Size = new System.Drawing.Size(1, 52);
            this.kryptonBorderEdge10.Text = "kryptonBorderEdge1";
            //
            // groupTo
            //
            this.groupTo.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupTo.Controls.Add(this.kryptonBorderEdge6);
            this.groupTo.Controls.Add(this.labelResidential);
            this.groupTo.Controls.Add(this.residentialDetermination);
            this.groupTo.Controls.Add(this.residentialState);
            this.groupTo.Location = new System.Drawing.Point(3, 67);
            this.groupTo.Name = "groupTo";
            this.groupTo.Size = new System.Drawing.Size(405, 52);
            this.groupTo.TabIndex = 4;
            this.groupTo.TabStop = false;
            this.groupTo.Text = "To";
            //
            // kryptonBorderEdge6
            //
            this.kryptonBorderEdge6.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge6.AutoSize = false;
            this.kryptonBorderEdge6.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge6.Location = new System.Drawing.Point(30, 14);
            this.kryptonBorderEdge6.Name = "kryptonBorderEdge6";
            this.kryptonBorderEdge6.Size = new System.Drawing.Size(1, 29);
            this.kryptonBorderEdge6.Text = "kryptonBorderEdge6";
            //
            // labelResidential
            //
            this.labelResidential.AutoSize = true;
            this.labelResidential.Location = new System.Drawing.Point(74, 23);
            this.labelResidential.Name = "labelResidential";
            this.labelResidential.Size = new System.Drawing.Size(63, 13);
            this.labelResidential.TabIndex = 15;
            this.labelResidential.Text = "Residential:";
            //
            // residentialDetermination
            //
            this.residentialDetermination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.residentialDetermination.FormattingEnabled = true;
            this.residentialDetermination.Location = new System.Drawing.Point(142, 20);
            this.residentialDetermination.Name = "residentialDetermination";
            this.residentialDetermination.PromptText = "(Multiple Values)";
            this.residentialDetermination.Size = new System.Drawing.Size(206, 21);
            this.residentialDetermination.TabIndex = 1;
            //
            // residentialState
            //
            this.residentialState.AutoSize = true;
            this.residentialState.Checked = true;
            this.residentialState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.residentialState.Location = new System.Drawing.Point(9, 23);
            this.residentialState.Name = "residentialState";
            this.residentialState.Size = new System.Drawing.Size(15, 14);
            this.residentialState.TabIndex = 0;
            this.residentialState.UseVisualStyleBackColor = true;
            //
            // groupShipment
            //
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.signatureRequired);
            this.groupShipment.Controls.Add(this.labelSignature);
            this.groupShipment.Controls.Add(this.signatureState);
            this.groupShipment.Controls.Add(this.dimensionsState);
            this.groupShipment.Controls.Add(this.dimensionsControl);
            this.groupShipment.Controls.Add(this.labelDimensions);
            this.groupShipment.Controls.Add(this.weightState);
            this.groupShipment.Controls.Add(this.weight);
            this.groupShipment.Controls.Add(this.labelWeight);
            this.groupShipment.Controls.Add(this.labelPackaging);
            this.groupShipment.Controls.Add(this.packaging);
            this.groupShipment.Controls.Add(this.packagingState);
            this.groupShipment.Controls.Add(this.saturdayDelivery);
            this.groupShipment.Controls.Add(this.labelSaturday);
            this.groupShipment.Controls.Add(this.saturdayState);
            this.groupShipment.Controls.Add(this.kryptonBorderEdge);
            this.groupShipment.Controls.Add(this.labelService);
            this.groupShipment.Controls.Add(this.service);
            this.groupShipment.Controls.Add(this.serviceState);
            this.groupShipment.Location = new System.Drawing.Point(3, 125);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(405, 250);
            this.groupShipment.TabIndex = 5;
            this.groupShipment.TabStop = false;
            this.groupShipment.Text = "Shipment";
            //
            // signatureRequired
            //
            this.signatureRequired.AutoSize = true;
            this.signatureRequired.Location = new System.Drawing.Point(142, 79);
            this.signatureRequired.Name = "signatureRequired";
            this.signatureRequired.Size = new System.Drawing.Size(118, 17);
            this.signatureRequired.TabIndex = 75;
            this.signatureRequired.Text = "Signature Required";
            this.signatureRequired.UseVisualStyleBackColor = true;
            //
            // labelSignature
            //
            this.labelSignature.AutoSize = true;
            this.labelSignature.BackColor = System.Drawing.Color.Transparent;
            this.labelSignature.Location = new System.Drawing.Point(79, 80);
            this.labelSignature.Name = "labelSignature";
            this.labelSignature.Size = new System.Drawing.Size(57, 13);
            this.labelSignature.TabIndex = 76;
            this.labelSignature.Text = "Signature:";
            //
            // signatureState
            //
            this.signatureState.AutoSize = true;
            this.signatureState.Checked = true;
            this.signatureState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.signatureState.Location = new System.Drawing.Point(9, 80);
            this.signatureState.Name = "signatureState";
            this.signatureState.Size = new System.Drawing.Size(15, 14);
            this.signatureState.TabIndex = 74;
            this.signatureState.Tag = "";
            this.signatureState.UseVisualStyleBackColor = true;
            //
            // dimensionsState
            //
            this.dimensionsState.AutoSize = true;
            this.dimensionsState.Checked = true;
            this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsState.Location = new System.Drawing.Point(9, 164);
            this.dimensionsState.Name = "dimensionsState";
            this.dimensionsState.Size = new System.Drawing.Size(15, 14);
            this.dimensionsState.TabIndex = 73;
            this.dimensionsState.Tag = "";
            this.dimensionsState.UseVisualStyleBackColor = true;
            //
            // dimensionsControl
            //
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dimensionsControl.Location = new System.Drawing.Point(142, 161);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 72;
            //
            // labelDimensions
            //
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(72, 164);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 71;
            this.labelDimensions.Text = "Dimensions:";
            //
            // weightState
            //
            this.weightState.AutoSize = true;
            this.weightState.Checked = true;
            this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.weightState.Location = new System.Drawing.Point(9, 136);
            this.weightState.Name = "weightState";
            this.weightState.Size = new System.Drawing.Size(15, 14);
            this.weightState.TabIndex = 70;
            this.weightState.Tag = "";
            this.weightState.UseVisualStyleBackColor = true;
            //
            // weight
            //
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(142, 132);
            this.weight.Name = "weight";
            this.weight.RangeMax = 400D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(269, 21);
            this.weight.TabIndex = 68;
            this.weight.Weight = 0D;
            //
            // labelWeight
            //
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(91, 136);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 67;
            this.labelWeight.Text = "Weight:";
            //
            // labelPackaging
            //
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(77, 108);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 60;
            this.labelPackaging.Text = "Packaging:";
            //
            // packaging
            //
            this.packaging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packaging.FormattingEnabled = true;
            this.packaging.Location = new System.Drawing.Point(142, 105);
            this.packaging.Name = "packaging";
            this.packaging.PromptText = "(Multiple Values)";
            this.packaging.Size = new System.Drawing.Size(144, 21);
            this.packaging.TabIndex = 59;
            //
            // packagingState
            //
            this.packagingState.AutoSize = true;
            this.packagingState.Checked = true;
            this.packagingState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packagingState.Location = new System.Drawing.Point(9, 108);
            this.packagingState.Name = "packagingState";
            this.packagingState.Size = new System.Drawing.Size(15, 14);
            this.packagingState.TabIndex = 58;
            this.packagingState.Tag = "";
            this.packagingState.UseVisualStyleBackColor = true;
            //
            // saturdayDelivery
            //
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.Location = new System.Drawing.Point(142, 51);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 3;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = true;
            //
            // labelSaturday
            //
            this.labelSaturday.AutoSize = true;
            this.labelSaturday.BackColor = System.Drawing.Color.Transparent;
            this.labelSaturday.Location = new System.Drawing.Point(81, 52);
            this.labelSaturday.Name = "labelSaturday";
            this.labelSaturday.Size = new System.Drawing.Size(55, 13);
            this.labelSaturday.TabIndex = 57;
            this.labelSaturday.Text = "Saturday:";
            //
            // saturdayState
            //
            this.saturdayState.AutoSize = true;
            this.saturdayState.Checked = true;
            this.saturdayState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saturdayState.Location = new System.Drawing.Point(9, 52);
            this.saturdayState.Name = "saturdayState";
            this.saturdayState.Size = new System.Drawing.Size(15, 14);
            this.saturdayState.TabIndex = 2;
            this.saturdayState.Tag = "";
            this.saturdayState.UseVisualStyleBackColor = true;
            //
            // kryptonBorderEdge
            //
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 222);
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            //
            // labelService
            //
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(90, 24);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 52;
            this.labelService.Text = "Service:";
            //
            // service
            //
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(142, 21);
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
            this.serviceState.Location = new System.Drawing.Point(9, 24);
            this.serviceState.Name = "serviceState";
            this.serviceState.Size = new System.Drawing.Size(15, 14);
            this.serviceState.TabIndex = 0;
            this.serviceState.Tag = "";
            this.serviceState.UseVisualStyleBackColor = true;
            //
            // groupBoxFrom
            //
            this.groupBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFrom.Controls.Add(this.labelAccount);
            this.groupBoxFrom.Controls.Add(this.onTracAccount);
            this.groupBoxFrom.Controls.Add(this.accountState);
            this.groupBoxFrom.Controls.Add(this.kryptonBorderEdge1);
            this.groupBoxFrom.Location = new System.Drawing.Point(3, 6);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(405, 55);
            this.groupBoxFrom.TabIndex = 3;
            this.groupBoxFrom.TabStop = false;
            this.groupBoxFrom.Text = "From";
            //
            // labelAccount
            //
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(84, 23);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 16;
            this.labelAccount.Text = "Account:";
            //
            // onTracAccount
            //
            this.onTracAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.onTracAccount.FormattingEnabled = true;
            this.onTracAccount.Location = new System.Drawing.Point(142, 20);
            this.onTracAccount.Name = "onTracAccount";
            this.onTracAccount.PromptText = "(Multiple Values)";
            this.onTracAccount.Size = new System.Drawing.Size(206, 21);
            this.onTracAccount.TabIndex = 1;
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
            // kryptonBorderEdge1
            //
            this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(29, 17);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 27);
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            //
            // groupLabels
            //
            this.groupLabels.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabels.Controls.Add(this.requestedLabelFormat);
            this.groupLabels.Controls.Add(this.requestedLabelFormatState);
            this.groupLabels.Controls.Add(this.kryptonBorderEdge11);
            this.groupLabels.Location = new System.Drawing.Point(3, 380);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Size = new System.Drawing.Size(405, 58);
            this.groupLabels.TabIndex = 14;
            this.groupLabels.TabStop = false;
            this.groupLabels.Text = "Labels";
            //
            // requestedLabelFormat
            //
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(35, 22);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(267, 21);
            this.requestedLabelFormat.State = false;
            this.requestedLabelFormat.TabIndex = 101;
            //
            // requestedLabelFormatState
            //
            this.requestedLabelFormatState.AutoSize = true;
            this.requestedLabelFormatState.Location = new System.Drawing.Point(9, 25);
            this.requestedLabelFormatState.Name = "requestedLabelFormatState";
            this.requestedLabelFormatState.Size = new System.Drawing.Size(15, 14);
            this.requestedLabelFormatState.TabIndex = 0;
            this.requestedLabelFormatState.UseVisualStyleBackColor = true;
            //
            // kryptonBorderEdge11
            //
            this.kryptonBorderEdge11.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge11.AutoSize = false;
            this.kryptonBorderEdge11.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge11.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge11.Name = "kryptonBorderEdge11";
            this.kryptonBorderEdge11.Size = new System.Drawing.Size(1, 28);
            this.kryptonBorderEdge11.Text = "kryptonBorderEdge11";
            //
            // OnTracProfileControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "OnTracProfileControl";
            this.Size = new System.Drawing.Size(425, 749);
            this.tabControl.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupTo.ResumeLayout(false);
            this.groupTo.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupTo;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge6;
        private System.Windows.Forms.Label labelResidential;
        private UI.Controls.MultiValueComboBox residentialDetermination;
        private System.Windows.Forms.CheckBox residentialState;
        private System.Windows.Forms.GroupBox groupShipment;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private System.Windows.Forms.Label labelSaturday;
        private System.Windows.Forms.CheckBox saturdayState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label labelService;
        private UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.CheckBox serviceState;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private System.Windows.Forms.Label labelAccount;
        private UI.Controls.MultiValueComboBox onTracAccount;
        private System.Windows.Forms.CheckBox accountState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private System.Windows.Forms.GroupBox groupInsurance;
        private Insurance.InsuranceProfileControl insuranceControl;
        private System.Windows.Forms.CheckBox insuranceState;
        private System.Windows.Forms.GroupBox groupOptions;
        private System.Windows.Forms.CheckBox reference2State;
        private Templates.Tokens.TemplateTokenTextBox referenceNumber2;
        private System.Windows.Forms.Label labelReference2;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge4;
        private System.Windows.Forms.CheckBox referenceState;
        private Templates.Tokens.TemplateTokenTextBox referenceNumber;
        private System.Windows.Forms.Label labelReference;
        private System.Windows.Forms.Label labelPackaging;
        private UI.Controls.MultiValueComboBox packaging;
        private System.Windows.Forms.CheckBox packagingState;
        private System.Windows.Forms.CheckBox weightState;
        private UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.CheckBox dimensionsState;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label labelDimensions;
        private System.Windows.Forms.CheckBox signatureRequired;
        private System.Windows.Forms.Label labelSignature;
        private System.Windows.Forms.CheckBox signatureState;
        private Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
        private System.Windows.Forms.CheckBox instructionsState;
        private Templates.Tokens.TemplateTokenTextBox instructions;
        private System.Windows.Forms.Label labelInstructions;
        protected System.Windows.Forms.GroupBox groupLabels;
        protected Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge11;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge10;
    }
}
