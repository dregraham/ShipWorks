namespace ShipWorks.Shipping.Carriers.EquaShip
{
    partial class EquaShipProfileControl
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
            this.tabPage = new System.Windows.Forms.TabPage();
            this.referenceGroup = new System.Windows.Forms.GroupBox();
            this.customerShippingNotes = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelShippingNotes = new System.Windows.Forms.Label();
            this.stateShippingNotes = new System.Windows.Forms.CheckBox();
            this.customerDescription = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.stateDescription = new System.Windows.Forms.CheckBox();
            this.customerReference = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReference = new System.Windows.Forms.Label();
            this.stateReference = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupInsurance = new System.Windows.Forms.GroupBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
            this.insuranceState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge4 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelAccount = new System.Windows.Forms.Label();
            this.labelSender = new System.Windows.Forms.Label();
            this.accountCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.accountState = new System.Windows.Forms.CheckBox();
            this.originCombo = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.senderState = new System.Windows.Forms.CheckBox();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.dimensionsState = new System.Windows.Forms.CheckBox();
            this.packagingState = new System.Windows.Forms.CheckBox();
            this.weightState = new System.Windows.Forms.CheckBox();
            this.dimensionsControl = new ShipWorks.Shipping.Editing.DimensionsControl();
            this.labelDimensions = new System.Windows.Forms.Label();
            this.packagingType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackaging = new System.Windows.Forms.Label();
            this.weight = new ShipWorks.UI.Controls.WeightControl();
            this.labelWeight = new System.Windows.Forms.Label();
            this.labelService = new System.Windows.Forms.Label();
            this.labelConfirmation = new System.Windows.Forms.Label();
            this.confirmation = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.confirmationState = new System.Windows.Forms.CheckBox();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPage.SuspendLayout();
            this.referenceGroup.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupBoxFrom.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(387, 522);
            this.tabControl.TabIndex = 4;
            // 
            // tabPage
            // 
            this.tabPage.AutoScroll = true;
            this.tabPage.Controls.Add(this.referenceGroup);
            this.tabPage.Controls.Add(this.groupInsurance);
            this.tabPage.Controls.Add(this.groupBoxFrom);
            this.tabPage.Controls.Add(this.groupShipment);
            this.tabPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage.Name = "tabPage";
            this.tabPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage.Size = new System.Drawing.Size(379, 496);
            this.tabPage.TabIndex = 0;
            this.tabPage.Text = "Settings";
            this.tabPage.UseVisualStyleBackColor = true;
            // 
            // referenceGroup
            // 
            this.referenceGroup.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.referenceGroup.Controls.Add(this.customerShippingNotes);
            this.referenceGroup.Controls.Add(this.labelShippingNotes);
            this.referenceGroup.Controls.Add(this.stateShippingNotes);
            this.referenceGroup.Controls.Add(this.customerDescription);
            this.referenceGroup.Controls.Add(this.labelDescription);
            this.referenceGroup.Controls.Add(this.stateDescription);
            this.referenceGroup.Controls.Add(this.customerReference);
            this.referenceGroup.Controls.Add(this.labelReference);
            this.referenceGroup.Controls.Add(this.stateReference);
            this.referenceGroup.Controls.Add(this.kryptonBorderEdge2);
            this.referenceGroup.Location = new System.Drawing.Point(8, 371);
            this.referenceGroup.Name = "referenceGroup";
            this.referenceGroup.Size = new System.Drawing.Size(365, 116);
            this.referenceGroup.TabIndex = 100;
            this.referenceGroup.TabStop = false;
            this.referenceGroup.Text = "Reference";
            // 
            // customerShippingNotes
            // 
            this.customerShippingNotes.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerShippingNotes.Location = new System.Drawing.Point(134, 79);
            this.customerShippingNotes.MaxLength = 32767;
            this.customerShippingNotes.Name = "customerShippingNotes";
            this.customerShippingNotes.Size = new System.Drawing.Size(210, 21);
            this.customerShippingNotes.TabIndex = 104;
            // 
            // labelShippingNotes
            // 
            this.labelShippingNotes.AutoSize = true;
            this.labelShippingNotes.BackColor = System.Drawing.Color.Transparent;
            this.labelShippingNotes.Location = new System.Drawing.Point(45, 83);
            this.labelShippingNotes.Name = "labelShippingNotes";
            this.labelShippingNotes.Size = new System.Drawing.Size(82, 13);
            this.labelShippingNotes.TabIndex = 103;
            this.labelShippingNotes.Text = "Shipping Notes:";
            // 
            // stateShippingNotes
            // 
            this.stateShippingNotes.AutoSize = true;
            this.stateShippingNotes.Location = new System.Drawing.Point(9, 83);
            this.stateShippingNotes.Name = "stateShippingNotes";
            this.stateShippingNotes.Size = new System.Drawing.Size(15, 14);
            this.stateShippingNotes.TabIndex = 102;
            this.stateShippingNotes.UseVisualStyleBackColor = true;
            // 
            // customerDescription
            // 
            this.customerDescription.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerDescription.Location = new System.Drawing.Point(134, 52);
            this.customerDescription.MaxLength = 32767;
            this.customerDescription.Name = "customerDescription";
            this.customerDescription.Size = new System.Drawing.Size(210, 21);
            this.customerDescription.TabIndex = 101;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.BackColor = System.Drawing.Color.Transparent;
            this.labelDescription.Location = new System.Drawing.Point(63, 57);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(64, 13);
            this.labelDescription.TabIndex = 100;
            this.labelDescription.Text = "Description:";
            // 
            // stateDescription
            // 
            this.stateDescription.AutoSize = true;
            this.stateDescription.Location = new System.Drawing.Point(9, 56);
            this.stateDescription.Name = "stateDescription";
            this.stateDescription.Size = new System.Drawing.Size(15, 14);
            this.stateDescription.TabIndex = 99;
            this.stateDescription.UseVisualStyleBackColor = true;
            // 
            // customerReference
            // 
            this.customerReference.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.customerReference.Location = new System.Drawing.Point(134, 25);
            this.customerReference.MaxLength = 32767;
            this.customerReference.Name = "customerReference";
            this.customerReference.Size = new System.Drawing.Size(210, 21);
            this.customerReference.TabIndex = 98;
            // 
            // labelReference
            // 
            this.labelReference.AutoSize = true;
            this.labelReference.BackColor = System.Drawing.Color.Transparent;
            this.labelReference.Location = new System.Drawing.Point(55, 29);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(72, 13);
            this.labelReference.TabIndex = 97;
            this.labelReference.Text = "Reference #:";
            // 
            // stateReference
            // 
            this.stateReference.AutoSize = true;
            this.stateReference.Location = new System.Drawing.Point(9, 28);
            this.stateReference.Name = "stateReference";
            this.stateReference.Size = new System.Drawing.Size(15, 14);
            this.stateReference.TabIndex = 0;
            this.stateReference.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge2
            // 
            this.kryptonBorderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge2.AutoSize = false;
            this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 86);
            this.kryptonBorderEdge2.TabIndex = 96;
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge1";
            // 
            // groupInsurance
            // 
            this.groupInsurance.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupInsurance.Controls.Add(this.insuranceControl);
            this.groupInsurance.Controls.Add(this.insuranceState);
            this.groupInsurance.Controls.Add(this.kryptonBorderEdge4);
            this.groupInsurance.Location = new System.Drawing.Point(8, 283);
            this.groupInsurance.Name = "groupInsurance";
            this.groupInsurance.Size = new System.Drawing.Size(365, 82);
            this.groupInsurance.TabIndex = 7;
            this.groupInsurance.TabStop = false;
            this.groupInsurance.Text = "Insurance";
            // 
            // insuranceControl
            // 
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.insuranceControl.Location = new System.Drawing.Point(43, 22);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(316, 52);
            this.insuranceControl.TabIndex = 98;
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
            // kryptonBorderEdge4
            // 
            this.kryptonBorderEdge4.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge4.AutoSize = false;
            this.kryptonBorderEdge4.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge4.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge4.Name = "kryptonBorderEdge4";
            this.kryptonBorderEdge4.Size = new System.Drawing.Size(1, 52);
            this.kryptonBorderEdge4.TabIndex = 96;
            this.kryptonBorderEdge4.Text = "kryptonBorderEdge1";
            // 
            // groupBoxFrom
            // 
            this.groupBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFrom.Controls.Add(this.kryptonBorderEdge1);
            this.groupBoxFrom.Controls.Add(this.labelAccount);
            this.groupBoxFrom.Controls.Add(this.labelSender);
            this.groupBoxFrom.Controls.Add(this.accountCombo);
            this.groupBoxFrom.Controls.Add(this.accountState);
            this.groupBoxFrom.Controls.Add(this.originCombo);
            this.groupBoxFrom.Controls.Add(this.senderState);
            this.groupBoxFrom.Location = new System.Drawing.Point(8, 6);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(365, 87);
            this.groupBoxFrom.TabIndex = 1;
            this.groupBoxFrom.TabStop = false;
            this.groupBoxFrom.Text = "From";
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(29, 17);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 59);
            this.kryptonBorderEdge1.TabIndex = 13;
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(54, 22);
            this.labelAccount.Name = "labelAccount";
            this.labelAccount.Size = new System.Drawing.Size(50, 13);
            this.labelAccount.TabIndex = 12;
            this.labelAccount.Text = "Account:";
            // 
            // labelSender
            // 
            this.labelSender.AutoSize = true;
            this.labelSender.Location = new System.Drawing.Point(65, 50);
            this.labelSender.Name = "labelSender";
            this.labelSender.Size = new System.Drawing.Size(39, 13);
            this.labelSender.TabIndex = 12;
            this.labelSender.Text = "Origin:";
            // 
            // accountCombo
            // 
            this.accountCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.accountCombo.FormattingEnabled = true;
            this.accountCombo.Location = new System.Drawing.Point(110, 18);
            this.accountCombo.Name = "accountCombo";
            this.accountCombo.PromptText = "(Multiple Values)";
            this.accountCombo.Size = new System.Drawing.Size(206, 21);
            this.accountCombo.TabIndex = 11;
            // 
            // accountState
            // 
            this.accountState.AutoSize = true;
            this.accountState.Checked = true;
            this.accountState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.accountState.Location = new System.Drawing.Point(9, 21);
            this.accountState.Name = "accountState";
            this.accountState.Size = new System.Drawing.Size(15, 14);
            this.accountState.TabIndex = 2;
            this.accountState.Tag = "";
            this.accountState.UseVisualStyleBackColor = true;
            // 
            // originCombo
            // 
            this.originCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.originCombo.FormattingEnabled = true;
            this.originCombo.Location = new System.Drawing.Point(110, 47);
            this.originCombo.Name = "originCombo";
            this.originCombo.PromptText = "(Multiple Values)";
            this.originCombo.Size = new System.Drawing.Size(206, 21);
            this.originCombo.TabIndex = 11;
            // 
            // senderState
            // 
            this.senderState.AutoSize = true;
            this.senderState.Checked = true;
            this.senderState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.senderState.Location = new System.Drawing.Point(9, 50);
            this.senderState.Name = "senderState";
            this.senderState.Size = new System.Drawing.Size(15, 14);
            this.senderState.TabIndex = 2;
            this.senderState.Tag = "";
            this.senderState.UseVisualStyleBackColor = true;
            // 
            // groupShipment
            // 
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.kryptonBorderEdge);
            this.groupShipment.Controls.Add(this.dimensionsState);
            this.groupShipment.Controls.Add(this.packagingState);
            this.groupShipment.Controls.Add(this.weightState);
            this.groupShipment.Controls.Add(this.dimensionsControl);
            this.groupShipment.Controls.Add(this.labelDimensions);
            this.groupShipment.Controls.Add(this.packagingType);
            this.groupShipment.Controls.Add(this.labelPackaging);
            this.groupShipment.Controls.Add(this.weight);
            this.groupShipment.Controls.Add(this.labelWeight);
            this.groupShipment.Controls.Add(this.labelService);
            this.groupShipment.Controls.Add(this.labelConfirmation);
            this.groupShipment.Controls.Add(this.confirmation);
            this.groupShipment.Controls.Add(this.service);
            this.groupShipment.Controls.Add(this.confirmationState);
            this.groupShipment.Controls.Add(this.serviceState);
            this.groupShipment.Location = new System.Drawing.Point(8, 99);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(365, 179);
            this.groupShipment.TabIndex = 0;
            this.groupShipment.TabStop = false;
            this.groupShipment.Text = "Shipment";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 146);
            this.kryptonBorderEdge.TabIndex = 5;
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // dimensionsState
            // 
            this.dimensionsState.AutoSize = true;
            this.dimensionsState.Checked = true;
            this.dimensionsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dimensionsState.Location = new System.Drawing.Point(9, 101);
            this.dimensionsState.Name = "dimensionsState";
            this.dimensionsState.Size = new System.Drawing.Size(15, 14);
            this.dimensionsState.TabIndex = 66;
            this.dimensionsState.Tag = "";
            this.dimensionsState.UseVisualStyleBackColor = true;
            // 
            // packagingState
            // 
            this.packagingState.AutoSize = true;
            this.packagingState.Checked = true;
            this.packagingState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packagingState.Location = new System.Drawing.Point(9, 72);
            this.packagingState.Name = "packagingState";
            this.packagingState.Size = new System.Drawing.Size(15, 14);
            this.packagingState.TabIndex = 64;
            this.packagingState.Tag = "";
            this.packagingState.UseVisualStyleBackColor = true;
            // 
            // weightState
            // 
            this.weightState.AutoSize = true;
            this.weightState.Checked = true;
            this.weightState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.weightState.Location = new System.Drawing.Point(9, 45);
            this.weightState.Name = "weightState";
            this.weightState.Size = new System.Drawing.Size(15, 14);
            this.weightState.TabIndex = 63;
            this.weightState.Tag = "";
            this.weightState.UseVisualStyleBackColor = true;
            // 
            // dimensionsControl
            // 
            this.dimensionsControl.BackColor = System.Drawing.Color.Transparent;
            this.dimensionsControl.Cleared = false;
            this.dimensionsControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.dimensionsControl.Location = new System.Drawing.Point(107, 93);
            this.dimensionsControl.Name = "dimensionsControl";
            this.dimensionsControl.Size = new System.Drawing.Size(210, 74);
            this.dimensionsControl.TabIndex = 62;
            // 
            // labelDimensions
            // 
            this.labelDimensions.AutoSize = true;
            this.labelDimensions.BackColor = System.Drawing.Color.Transparent;
            this.labelDimensions.Location = new System.Drawing.Point(40, 99);
            this.labelDimensions.Name = "labelDimensions";
            this.labelDimensions.Size = new System.Drawing.Size(64, 13);
            this.labelDimensions.TabIndex = 59;
            this.labelDimensions.Text = "Dimensions:";
            // 
            // packagingType
            // 
            this.packagingType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagingType.FormattingEnabled = true;
            this.packagingType.Location = new System.Drawing.Point(110, 67);
            this.packagingType.Name = "packagingType";
            this.packagingType.PromptText = "(Multiple Values)";
            this.packagingType.Size = new System.Drawing.Size(144, 21);
            this.packagingType.TabIndex = 58;
            // 
            // labelPackaging
            // 
            this.labelPackaging.AutoSize = true;
            this.labelPackaging.BackColor = System.Drawing.Color.Transparent;
            this.labelPackaging.Location = new System.Drawing.Point(45, 70);
            this.labelPackaging.Name = "labelPackaging";
            this.labelPackaging.Size = new System.Drawing.Size(59, 13);
            this.labelPackaging.TabIndex = 57;
            this.labelPackaging.Text = "Packaging:";
            // 
            // weight
            // 
            this.weight.BackColor = System.Drawing.Color.Transparent;
            this.weight.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.weight.Location = new System.Drawing.Point(110, 41);
            this.weight.Name = "weight";
            this.weight.RangeMax = 300D;
            this.weight.RangeMin = 0D;
            this.weight.Size = new System.Drawing.Size(218, 21);
            this.weight.TabIndex = 56;
            this.weight.Weight = 0D;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.BackColor = System.Drawing.Color.Transparent;
            this.labelWeight.Location = new System.Drawing.Point(59, 44);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(45, 13);
            this.labelWeight.TabIndex = 55;
            this.labelWeight.Text = "Weight:";
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(56, 16);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 52;
            this.labelService.Text = "Service:";
            // 
            // labelConfirmation
            // 
            this.labelConfirmation.AutoSize = true;
            this.labelConfirmation.BackColor = System.Drawing.Color.Transparent;
            this.labelConfirmation.Location = new System.Drawing.Point(303, 29);
            this.labelConfirmation.Name = "labelConfirmation";
            this.labelConfirmation.Size = new System.Drawing.Size(72, 13);
            this.labelConfirmation.TabIndex = 51;
            this.labelConfirmation.Text = "Confirmation:";
            this.labelConfirmation.Visible = false;
            // 
            // confirmation
            // 
            this.confirmation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.confirmation.FormattingEnabled = true;
            this.confirmation.Location = new System.Drawing.Point(305, 45);
            this.confirmation.Name = "confirmation";
            this.confirmation.PromptText = "(Multiple Values)";
            this.confirmation.Size = new System.Drawing.Size(144, 21);
            this.confirmation.TabIndex = 50;
            this.confirmation.Visible = false;
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(110, 13);
            this.service.Name = "service";
            this.service.PromptText = "(Multiple Values)";
            this.service.Size = new System.Drawing.Size(144, 21);
            this.service.TabIndex = 49;
            // 
            // confirmationState
            // 
            this.confirmationState.AutoSize = true;
            this.confirmationState.Checked = true;
            this.confirmationState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.confirmationState.Location = new System.Drawing.Point(306, 15);
            this.confirmationState.Name = "confirmationState";
            this.confirmationState.Size = new System.Drawing.Size(15, 14);
            this.confirmationState.TabIndex = 3;
            this.confirmationState.Tag = "";
            this.confirmationState.UseVisualStyleBackColor = true;
            this.confirmationState.Visible = false;
            // 
            // serviceState
            // 
            this.serviceState.AutoSize = true;
            this.serviceState.Checked = true;
            this.serviceState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.serviceState.Location = new System.Drawing.Point(9, 20);
            this.serviceState.Name = "serviceState";
            this.serviceState.Size = new System.Drawing.Size(15, 14);
            this.serviceState.TabIndex = 2;
            this.serviceState.Tag = "";
            this.serviceState.UseVisualStyleBackColor = true;
            // 
            // EquaShipProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "EquaShipProfileControl";
            this.Size = new System.Drawing.Size(387, 522);
            this.tabControl.ResumeLayout(false);
            this.tabPage.ResumeLayout(false);
            this.referenceGroup.ResumeLayout(false);
            this.referenceGroup.PerformLayout();
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        protected System.Windows.Forms.TabPage tabPage;
        protected System.Windows.Forms.GroupBox groupInsurance;
        private System.Windows.Forms.CheckBox insuranceState;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge4;
        protected System.Windows.Forms.GroupBox groupBoxFrom;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        protected System.Windows.Forms.Label labelSender;
        protected UI.Controls.MultiValueComboBox originCombo;
        protected System.Windows.Forms.CheckBox senderState;
        protected System.Windows.Forms.GroupBox groupShipment;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.CheckBox dimensionsState;
        private System.Windows.Forms.CheckBox packagingState;
        private System.Windows.Forms.CheckBox weightState;
        private Editing.DimensionsControl dimensionsControl;
        private System.Windows.Forms.Label labelDimensions;
        private UI.Controls.MultiValueComboBox packagingType;
        private System.Windows.Forms.Label labelPackaging;
        private UI.Controls.WeightControl weight;
        private System.Windows.Forms.Label labelWeight;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.Label labelConfirmation;
        private UI.Controls.MultiValueComboBox confirmation;
        private UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.CheckBox confirmationState;
        private System.Windows.Forms.CheckBox serviceState;
        protected System.Windows.Forms.Label labelAccount;
        protected UI.Controls.MultiValueComboBox accountCombo;
        protected System.Windows.Forms.CheckBox accountState;
        protected System.Windows.Forms.GroupBox referenceGroup;
        private Templates.Tokens.TemplateTokenTextBox customerReference;
        private System.Windows.Forms.Label labelReference;
        private System.Windows.Forms.CheckBox stateReference;
        protected ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private Templates.Tokens.TemplateTokenTextBox customerShippingNotes;
        private System.Windows.Forms.Label labelShippingNotes;
        private System.Windows.Forms.CheckBox stateShippingNotes;
        private Templates.Tokens.TemplateTokenTextBox customerDescription;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.CheckBox stateDescription;
        private Insurance.InsuranceProfileControl insuranceControl;
    }
}
