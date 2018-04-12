using ShipWorks.Templates.Tokens;
namespace ShipWorks.Shipping.Carriers.iParcel
{
    partial class iParcelProfileControl
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
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory1 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory commonTokenSuggestionsFactory2 = new ShipWorks.Templates.Tokens.CommonTokenSuggestionsFactory();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.labelThermalNote = new System.Windows.Forms.Label();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge11 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupOptions = new System.Windows.Forms.GroupBox();
            this.skuAndQuantityState = new System.Windows.Forms.CheckBox();
            this.skuAndQuantity = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelDuty = new System.Windows.Forms.Label();
            this.labelSkuAndQuantity = new System.Windows.Forms.Label();
            this.dutyDeliveryPaid = new System.Windows.Forms.CheckBox();
            this.dutyDeliveryPaidState = new System.Windows.Forms.CheckBox();
            this.emailTrack = new System.Windows.Forms.CheckBox();
            this.labelEmailTrack = new System.Windows.Forms.Label();
            this.emailTrackState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupReference = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge4 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.referenceState = new System.Windows.Forms.CheckBox();
            this.referenceNumber = new ShipWorks.Templates.Tokens.TemplateTokenTextBox();
            this.labelReference = new System.Windows.Forms.Label();
            this.groupInsurance = new System.Windows.Forms.GroupBox();
            this.insuranceControl = new ShipWorks.Shipping.Insurance.InsuranceProfileControl();
            this.insuranceState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge10 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.labelAccount = new System.Windows.Forms.Label();
            this.iParcelAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.accountState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.tabPagePackages = new System.Windows.Forms.TabPage();
            this.panelPackageControls = new System.Windows.Forms.Panel();
            this.groupBoxPackages = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.packagesCount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackages = new System.Windows.Forms.Label();
            this.packagesState = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.groupOptions.SuspendLayout();
            this.groupReference.SuspendLayout();
            this.groupInsurance.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.groupBoxFrom.SuspendLayout();
            this.tabPagePackages.SuspendLayout();
            this.groupBoxPackages.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSettings);
            this.tabControl.Controls.Add(this.tabPagePackages);
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
            this.tabPageSettings.Controls.Add(this.groupLabels);
            this.tabPageSettings.Controls.Add(this.groupOptions);
            this.tabPageSettings.Controls.Add(this.groupReference);
            this.tabPageSettings.Controls.Add(this.groupInsurance);
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
            // groupLabels
            // 
            this.groupLabels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupLabels.Controls.Add(this.labelThermalNote);
            this.groupLabels.Controls.Add(this.requestedLabelFormat);
            this.groupLabels.Controls.Add(this.requestedLabelFormatState);
            this.groupLabels.Controls.Add(this.kryptonBorderEdge11);
            this.groupLabels.Location = new System.Drawing.Point(6, 129);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Size = new System.Drawing.Size(405, 83);
            this.groupLabels.TabIndex = 2;
            this.groupLabels.TabStop = false;
            this.groupLabels.Text = "Labels";
            // 
            // labelThermalNote
            // 
            this.labelThermalNote.AutoSize = true;
            this.labelThermalNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelThermalNote.Location = new System.Drawing.Point(35, 48);
            this.labelThermalNote.Name = "labelThermalNote";
            this.labelThermalNote.Size = new System.Drawing.Size(233, 13);
            this.labelThermalNote.TabIndex = 103;
            this.labelThermalNote.Text = "Note: i-parcel only supports EPL thermal labels.";
            this.labelThermalNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // requestedLabelFormat
            // 
            this.requestedLabelFormat.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.requestedLabelFormat.Location = new System.Drawing.Point(35, 22);
            this.requestedLabelFormat.Name = "requestedLabelFormat";
            this.requestedLabelFormat.Size = new System.Drawing.Size(267, 21);
            this.requestedLabelFormat.State = false;
            this.requestedLabelFormat.TabIndex = 1;
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
            this.kryptonBorderEdge11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge11.AutoSize = false;
            this.kryptonBorderEdge11.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge11.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge11.Name = "kryptonBorderEdge11";
            this.kryptonBorderEdge11.Size = new System.Drawing.Size(1, 52);
            this.kryptonBorderEdge11.Text = "kryptonBorderEdge11";
            // 
            // groupOptions
            // 
            this.groupOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupOptions.Controls.Add(this.skuAndQuantityState);
            this.groupOptions.Controls.Add(this.skuAndQuantity);
            this.groupOptions.Controls.Add(this.labelDuty);
            this.groupOptions.Controls.Add(this.labelSkuAndQuantity);
            this.groupOptions.Controls.Add(this.dutyDeliveryPaid);
            this.groupOptions.Controls.Add(this.dutyDeliveryPaidState);
            this.groupOptions.Controls.Add(this.emailTrack);
            this.groupOptions.Controls.Add(this.labelEmailTrack);
            this.groupOptions.Controls.Add(this.emailTrackState);
            this.groupOptions.Controls.Add(this.kryptonBorderEdge2);
            this.groupOptions.Location = new System.Drawing.Point(6, 371);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new System.Drawing.Size(405, 101);
            this.groupOptions.TabIndex = 5;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            // 
            // skuAndQuantityState
            // 
            this.skuAndQuantityState.AutoSize = true;
            this.skuAndQuantityState.Checked = true;
            this.skuAndQuantityState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.skuAndQuantityState.Location = new System.Drawing.Point(6, 68);
            this.skuAndQuantityState.Name = "skuAndQuantityState";
            this.skuAndQuantityState.Size = new System.Drawing.Size(15, 14);
            this.skuAndQuantityState.TabIndex = 4;
            this.skuAndQuantityState.Tag = "";
            this.skuAndQuantityState.UseVisualStyleBackColor = true;
            // 
            // skuAndQuantity
            // 
            this.skuAndQuantity.Location = new System.Drawing.Point(139, 65);
            this.skuAndQuantity.MaxLength = 32767;
            this.skuAndQuantity.Name = "skuAndQuantity";
            this.skuAndQuantity.Size = new System.Drawing.Size(222, 21);
            this.skuAndQuantity.TabIndex = 5;
            this.skuAndQuantity.TokenSuggestionFactory = commonTokenSuggestionsFactory1;
            this.skuAndQuantity.TokenUsage = ShipWorks.Templates.Tokens.TokenUsage.ShippingReference;
            // 
            // labelDuty
            // 
            this.labelDuty.AutoSize = true;
            this.labelDuty.BackColor = System.Drawing.Color.Transparent;
            this.labelDuty.Location = new System.Drawing.Point(97, 43);
            this.labelDuty.Name = "labelDuty";
            this.labelDuty.Size = new System.Drawing.Size(34, 13);
            this.labelDuty.TabIndex = 84;
            this.labelDuty.Text = "Duty:";
            // 
            // labelSkuAndQuantity
            // 
            this.labelSkuAndQuantity.AutoSize = true;
            this.labelSkuAndQuantity.BackColor = System.Drawing.Color.Transparent;
            this.labelSkuAndQuantity.Location = new System.Drawing.Point(32, 68);
            this.labelSkuAndQuantity.Name = "labelSkuAndQuantity";
            this.labelSkuAndQuantity.Size = new System.Drawing.Size(99, 13);
            this.labelSkuAndQuantity.TabIndex = 77;
            this.labelSkuAndQuantity.Text = "Item and Quantity:";
            // 
            // dutyDeliveryPaid
            // 
            this.dutyDeliveryPaid.AutoSize = true;
            this.dutyDeliveryPaid.Location = new System.Drawing.Point(139, 42);
            this.dutyDeliveryPaid.Name = "dutyDeliveryPaid";
            this.dutyDeliveryPaid.Size = new System.Drawing.Size(114, 17);
            this.dutyDeliveryPaid.TabIndex = 3;
            this.dutyDeliveryPaid.Text = "Duty Delivery Paid";
            this.dutyDeliveryPaid.UseVisualStyleBackColor = true;
            // 
            // dutyDeliveryPaidState
            // 
            this.dutyDeliveryPaidState.AutoSize = true;
            this.dutyDeliveryPaidState.Checked = true;
            this.dutyDeliveryPaidState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dutyDeliveryPaidState.Location = new System.Drawing.Point(6, 43);
            this.dutyDeliveryPaidState.Name = "dutyDeliveryPaidState";
            this.dutyDeliveryPaidState.Size = new System.Drawing.Size(15, 14);
            this.dutyDeliveryPaidState.TabIndex = 2;
            this.dutyDeliveryPaidState.Tag = "";
            this.dutyDeliveryPaidState.UseVisualStyleBackColor = true;
            // 
            // emailTrack
            // 
            this.emailTrack.AutoSize = true;
            this.emailTrack.Location = new System.Drawing.Point(139, 19);
            this.emailTrack.Name = "emailTrack";
            this.emailTrack.Size = new System.Drawing.Size(94, 17);
            this.emailTrack.TabIndex = 1;
            this.emailTrack.Text = "Track By Email";
            this.emailTrack.UseVisualStyleBackColor = true;
            // 
            // labelEmailTrack
            // 
            this.labelEmailTrack.AutoSize = true;
            this.labelEmailTrack.BackColor = System.Drawing.Color.Transparent;
            this.labelEmailTrack.Location = new System.Drawing.Point(39, 20);
            this.labelEmailTrack.Name = "labelEmailTrack";
            this.labelEmailTrack.Size = new System.Drawing.Size(92, 13);
            this.labelEmailTrack.TabIndex = 79;
            this.labelEmailTrack.Text = "Email Notification:";
            // 
            // emailTrackState
            // 
            this.emailTrackState.AutoSize = true;
            this.emailTrackState.Checked = true;
            this.emailTrackState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.emailTrackState.Location = new System.Drawing.Point(6, 20);
            this.emailTrackState.Name = "emailTrackState";
            this.emailTrackState.Size = new System.Drawing.Size(15, 14);
            this.emailTrackState.TabIndex = 0;
            this.emailTrackState.Tag = "";
            this.emailTrackState.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge2
            // 
            this.kryptonBorderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge2.AutoSize = false;
            this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(29, 19);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 70);
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge1";
            // 
            // groupReference
            // 
            this.groupReference.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupReference.Controls.Add(this.kryptonBorderEdge4);
            this.groupReference.Controls.Add(this.referenceState);
            this.groupReference.Controls.Add(this.referenceNumber);
            this.groupReference.Controls.Add(this.labelReference);
            this.groupReference.Location = new System.Drawing.Point(6, 305);
            this.groupReference.Name = "groupReference";
            this.groupReference.Size = new System.Drawing.Size(405, 60);
            this.groupReference.TabIndex = 4;
            this.groupReference.TabStop = false;
            this.groupReference.Text = "Reference";
            // 
            // kryptonBorderEdge4
            // 
            this.kryptonBorderEdge4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge4.AutoSize = false;
            this.kryptonBorderEdge4.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge4.Location = new System.Drawing.Point(29, 19);
            this.kryptonBorderEdge4.Name = "kryptonBorderEdge4";
            this.kryptonBorderEdge4.Size = new System.Drawing.Size(1, 29);
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
            this.referenceState.TabIndex = 0;
            this.referenceState.Tag = "";
            this.referenceState.UseVisualStyleBackColor = true;
            // 
            // referenceNumber
            // 
            this.referenceNumber.Location = new System.Drawing.Point(139, 23);
            this.referenceNumber.MaxLength = 32767;
            this.referenceNumber.Name = "referenceNumber";
            this.referenceNumber.Size = new System.Drawing.Size(222, 21);
            this.referenceNumber.TabIndex = 1;
            this.referenceNumber.TokenSuggestionFactory = commonTokenSuggestionsFactory2;
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
            this.groupInsurance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupInsurance.Controls.Add(this.insuranceControl);
            this.groupInsurance.Controls.Add(this.insuranceState);
            this.groupInsurance.Controls.Add(this.kryptonBorderEdge10);
            this.groupInsurance.Location = new System.Drawing.Point(6, 217);
            this.groupInsurance.Name = "groupInsurance";
            this.groupInsurance.Size = new System.Drawing.Size(405, 82);
            this.groupInsurance.TabIndex = 3;
            this.groupInsurance.TabStop = false;
            this.groupInsurance.Text = "Insurance";
            // 
            // insuranceControl
            // 
            this.insuranceControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.insuranceControl.Location = new System.Drawing.Point(48, 21);
            this.insuranceControl.Name = "insuranceControl";
            this.insuranceControl.Size = new System.Drawing.Size(316, 52);
            this.insuranceControl.TabIndex = 1;
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
            this.kryptonBorderEdge10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge10.AutoSize = false;
            this.kryptonBorderEdge10.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge10.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge10.Name = "kryptonBorderEdge10";
            this.kryptonBorderEdge10.Size = new System.Drawing.Size(1, 52);
            this.kryptonBorderEdge10.Text = "kryptonBorderEdge1";
            // 
            // groupShipment
            // 
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.kryptonBorderEdge);
            this.groupShipment.Controls.Add(this.labelService);
            this.groupShipment.Controls.Add(this.service);
            this.groupShipment.Controls.Add(this.serviceState);
            this.groupShipment.Location = new System.Drawing.Point(6, 67);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(405, 57);
            this.groupShipment.TabIndex = 1;
            this.groupShipment.TabStop = false;
            this.groupShipment.Text = "Shipment";
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(29, 18);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 29);
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
            this.groupBoxFrom.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxFrom.Controls.Add(this.labelAccount);
            this.groupBoxFrom.Controls.Add(this.iParcelAccount);
            this.groupBoxFrom.Controls.Add(this.accountState);
            this.groupBoxFrom.Controls.Add(this.kryptonBorderEdge1);
            this.groupBoxFrom.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(405, 55);
            this.groupBoxFrom.TabIndex = 0;
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
            // iParcelAccount
            // 
            this.iParcelAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iParcelAccount.FormattingEnabled = true;
            this.iParcelAccount.Location = new System.Drawing.Point(142, 20);
            this.iParcelAccount.Name = "iParcelAccount";
            this.iParcelAccount.PromptText = "(Multiple Values)";
            this.iParcelAccount.Size = new System.Drawing.Size(206, 21);
            this.iParcelAccount.TabIndex = 1;
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
            this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(29, 17);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 27);
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // tabPagePackages
            // 
            this.tabPagePackages.AutoScroll = true;
            this.tabPagePackages.Controls.Add(this.panelPackageControls);
            this.tabPagePackages.Controls.Add(this.groupBoxPackages);
            this.tabPagePackages.Location = new System.Drawing.Point(4, 22);
            this.tabPagePackages.Name = "tabPagePackages";
            this.tabPagePackages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePackages.Size = new System.Drawing.Size(417, 1101);
            this.tabPagePackages.TabIndex = 1;
            this.tabPagePackages.Text = "Packages";
            this.tabPagePackages.UseVisualStyleBackColor = true;
            // 
            // panelPackageControls
            // 
            this.panelPackageControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPackageControls.Location = new System.Drawing.Point(6, 64);
            this.panelPackageControls.Name = "panelPackageControls";
            this.panelPackageControls.Size = new System.Drawing.Size(405, 318);
            this.panelPackageControls.TabIndex = 1;
            // 
            // groupBoxPackages
            // 
            this.groupBoxPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPackages.Controls.Add(this.kryptonBorderEdge3);
            this.groupBoxPackages.Controls.Add(this.packagesCount);
            this.groupBoxPackages.Controls.Add(this.labelPackages);
            this.groupBoxPackages.Controls.Add(this.packagesState);
            this.groupBoxPackages.Location = new System.Drawing.Point(6, 6);
            this.groupBoxPackages.Name = "groupBoxPackages";
            this.groupBoxPackages.Size = new System.Drawing.Size(405, 52);
            this.groupBoxPackages.TabIndex = 0;
            this.groupBoxPackages.TabStop = false;
            this.groupBoxPackages.Text = "From";
            // 
            // kryptonBorderEdge3
            // 
            this.kryptonBorderEdge3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge3.AutoSize = false;
            this.kryptonBorderEdge3.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge3.Location = new System.Drawing.Point(29, 17);
            this.kryptonBorderEdge3.Name = "kryptonBorderEdge3";
            this.kryptonBorderEdge3.Size = new System.Drawing.Size(1, 24);
            this.kryptonBorderEdge3.Text = "kryptonBorderEdge3";
            // 
            // packagesCount
            // 
            this.packagesCount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.packagesCount.FormattingEnabled = true;
            this.packagesCount.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.packagesCount.Location = new System.Drawing.Point(108, 17);
            this.packagesCount.Name = "packagesCount";
            this.packagesCount.PromptText = "(Multiple Values)";
            this.packagesCount.Size = new System.Drawing.Size(72, 21);
            this.packagesCount.TabIndex = 1;
            // 
            // labelPackages
            // 
            this.labelPackages.AutoSize = true;
            this.labelPackages.BackColor = System.Drawing.Color.Transparent;
            this.labelPackages.Location = new System.Drawing.Point(46, 21);
            this.labelPackages.Name = "labelPackages";
            this.labelPackages.Size = new System.Drawing.Size(56, 13);
            this.labelPackages.TabIndex = 71;
            this.labelPackages.Text = "Packages:";
            // 
            // packagesState
            // 
            this.packagesState.AutoSize = true;
            this.packagesState.Checked = true;
            this.packagesState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.packagesState.Location = new System.Drawing.Point(9, 20);
            this.packagesState.Name = "packagesState";
            this.packagesState.Size = new System.Drawing.Size(15, 14);
            this.packagesState.TabIndex = 0;
            this.packagesState.Tag = "";
            this.packagesState.UseVisualStyleBackColor = true;
            // 
            // iParcelProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "iParcelProfileControl";
            this.Size = new System.Drawing.Size(425, 1127);
            this.tabControl.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            this.groupReference.ResumeLayout(false);
            this.groupReference.PerformLayout();
            this.groupInsurance.ResumeLayout(false);
            this.groupInsurance.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.tabPagePackages.ResumeLayout(false);
            this.groupBoxPackages.ResumeLayout(false);
            this.groupBoxPackages.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupReference;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge4;
        private System.Windows.Forms.CheckBox referenceState;
        private Templates.Tokens.TemplateTokenTextBox referenceNumber;
        private System.Windows.Forms.Label labelReference;
        private System.Windows.Forms.GroupBox groupInsurance;
        private Insurance.InsuranceProfileControl insuranceControl;
        private System.Windows.Forms.CheckBox insuranceState;
        private System.Windows.Forms.GroupBox groupShipment;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label labelService;
        private UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.CheckBox serviceState;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private System.Windows.Forms.Label labelAccount;
        private UI.Controls.MultiValueComboBox iParcelAccount;
        private System.Windows.Forms.CheckBox accountState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private System.Windows.Forms.GroupBox groupOptions;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private System.Windows.Forms.CheckBox emailTrack;
        private System.Windows.Forms.Label labelEmailTrack;
        private System.Windows.Forms.CheckBox emailTrackState;
        private System.Windows.Forms.Label labelDuty;
        private System.Windows.Forms.CheckBox dutyDeliveryPaid;
        private System.Windows.Forms.CheckBox dutyDeliveryPaidState;
        private System.Windows.Forms.TabPage tabPagePackages;
        protected System.Windows.Forms.GroupBox groupBoxPackages;
        private UI.Controls.MultiValueComboBox packagesCount;
        private System.Windows.Forms.Label labelPackages;
        protected System.Windows.Forms.CheckBox packagesState;
        private System.Windows.Forms.Panel panelPackageControls;
        private System.Windows.Forms.CheckBox skuAndQuantityState;
        private Templates.Tokens.TemplateTokenTextBox skuAndQuantity;
        private System.Windows.Forms.Label labelSkuAndQuantity;
        protected System.Windows.Forms.GroupBox groupLabels;
        protected Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge11;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge10;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        private System.Windows.Forms.Label labelThermalNote;

    }
}
