namespace ShipWorks.Shipping.UI.Carriers.Dhl
{
    partial class DhlExpressProfileControl
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
            this.groupLabels = new System.Windows.Forms.GroupBox();
            this.labelThermalNote = new System.Windows.Forms.Label();
            this.requestedLabelFormat = new ShipWorks.Shipping.Editing.RequestedLabelFormatProfileControl();
            this.requestedLabelFormatState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge11 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupOptions = new System.Windows.Forms.GroupBox();
            this.nonMachinable = new System.Windows.Forms.CheckBox();
            this.nonMachinableState = new System.Windows.Forms.CheckBox();
            this.labelDuty = new System.Windows.Forms.Label();
            this.labelNonMachinable = new System.Windows.Forms.Label();
            this.labelSaturday = new System.Windows.Forms.Label();
            this.dutyDeliveryPaid = new System.Windows.Forms.CheckBox();
            this.dutyDeliveryPaidState = new System.Windows.Forms.CheckBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.saturdayState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.labelAccount = new System.Windows.Forms.Label();
            this.accountState = new System.Windows.Forms.CheckBox();
            this.dhlExpressAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.tabPagePackages = new System.Windows.Forms.TabPage();
            this.panelPackageControls = new System.Windows.Forms.Panel();
            this.groupBoxPackages = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.packagesCount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackages = new System.Windows.Forms.Label();
            this.packagesState = new System.Windows.Forms.CheckBox();
            this.contents = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelContents = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge4 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.nonDelivery = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelNonDelivery = new System.Windows.Forms.Label();
            this.nonDeliveryState = new System.Windows.Forms.CheckBox();
            this.contentsState = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.groupOptions.SuspendLayout();
            this.groupShipment.SuspendLayout();
            this.groupBoxFrom.SuspendLayout();
            this.tabPagePackages.SuspendLayout();
            this.groupBoxPackages.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.tabPageSettings.Controls.Add(this.groupBox1);
            this.tabPageSettings.Controls.Add(this.groupLabels);
            this.tabPageSettings.Controls.Add(this.groupOptions);
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
            this.groupLabels.Location = new System.Drawing.Point(6, 316);
            this.groupLabels.Name = "groupLabels";
            this.groupLabels.Size = new System.Drawing.Size(405, 71);
            this.groupLabels.TabIndex = 76;
            this.groupLabels.TabStop = false;
            this.groupLabels.Text = "Labels";
            // 
            // labelThermalNote
            // 
            this.labelThermalNote.AutoSize = true;
            this.labelThermalNote.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelThermalNote.Location = new System.Drawing.Point(35, 48);
            this.labelThermalNote.Name = "labelThermalNote";
            this.labelThermalNote.Size = new System.Drawing.Size(258, 13);
            this.labelThermalNote.TabIndex = 103;
            this.labelThermalNote.Text = "Note: DHL Express only supports ZPL thermal labels.";
            this.labelThermalNote.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.kryptonBorderEdge11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge11.AutoSize = false;
            this.kryptonBorderEdge11.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge11.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge11.Name = "kryptonBorderEdge11";
            this.kryptonBorderEdge11.Size = new System.Drawing.Size(1, 40);
            this.kryptonBorderEdge11.Text = "kryptonBorderEdge11";
            // 
            // groupOptions
            // 
            this.groupOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupOptions.Controls.Add(this.nonMachinable);
            this.groupOptions.Controls.Add(this.nonMachinableState);
            this.groupOptions.Controls.Add(this.labelDuty);
            this.groupOptions.Controls.Add(this.labelNonMachinable);
            this.groupOptions.Controls.Add(this.labelSaturday);
            this.groupOptions.Controls.Add(this.dutyDeliveryPaid);
            this.groupOptions.Controls.Add(this.dutyDeliveryPaidState);
            this.groupOptions.Controls.Add(this.saturdayDelivery);
            this.groupOptions.Controls.Add(this.saturdayState);
            this.groupOptions.Controls.Add(this.kryptonBorderEdge2);
            this.groupOptions.Location = new System.Drawing.Point(6, 127);
            this.groupOptions.Name = "groupOptions";
            this.groupOptions.Size = new System.Drawing.Size(405, 96);
            this.groupOptions.TabIndex = 75;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            // 
            // nonMachinable
            // 
            this.nonMachinable.AutoSize = true;
            this.nonMachinable.Location = new System.Drawing.Point(128, 65);
            this.nonMachinable.Name = "nonMachinable";
            this.nonMachinable.Size = new System.Drawing.Size(102, 17);
            this.nonMachinable.TabIndex = 86;
            this.nonMachinable.Text = "Non-Machinable";
            this.nonMachinable.UseVisualStyleBackColor = true;
            // 
            // nonMachinableState
            // 
            this.nonMachinableState.AutoSize = true;
            this.nonMachinableState.Checked = true;
            this.nonMachinableState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nonMachinableState.Location = new System.Drawing.Point(9, 66);
            this.nonMachinableState.Name = "nonMachinableState";
            this.nonMachinableState.Size = new System.Drawing.Size(15, 14);
            this.nonMachinableState.TabIndex = 75;
            this.nonMachinableState.Tag = "";
            this.nonMachinableState.UseVisualStyleBackColor = true;
            // 
            // labelDuty
            // 
            this.labelDuty.AutoSize = true;
            this.labelDuty.BackColor = System.Drawing.Color.Transparent;
            this.labelDuty.Location = new System.Drawing.Point(88, 43);
            this.labelDuty.Name = "labelDuty";
            this.labelDuty.Size = new System.Drawing.Size(34, 13);
            this.labelDuty.TabIndex = 84;
            this.labelDuty.Text = "Duty:";
            // 
            // labelNonMachinable
            // 
            this.labelNonMachinable.AutoSize = true;
            this.labelNonMachinable.BackColor = System.Drawing.Color.Transparent;
            this.labelNonMachinable.Location = new System.Drawing.Point(35, 66);
            this.labelNonMachinable.Name = "labelNonMachinable";
            this.labelNonMachinable.Size = new System.Drawing.Size(87, 13);
            this.labelNonMachinable.TabIndex = 77;
            this.labelNonMachinable.Text = "Non-Machinable:";
            // 
            // labelSaturday
            // 
            this.labelSaturday.AutoSize = true;
            this.labelSaturday.BackColor = System.Drawing.Color.Transparent;
            this.labelSaturday.Location = new System.Drawing.Point(67, 20);
            this.labelSaturday.Name = "labelSaturday";
            this.labelSaturday.Size = new System.Drawing.Size(55, 13);
            this.labelSaturday.TabIndex = 79;
            this.labelSaturday.Text = "Saturday:";
            // 
            // dutyDeliveryPaid
            // 
            this.dutyDeliveryPaid.AutoSize = true;
            this.dutyDeliveryPaid.Location = new System.Drawing.Point(128, 42);
            this.dutyDeliveryPaid.Name = "dutyDeliveryPaid";
            this.dutyDeliveryPaid.Size = new System.Drawing.Size(114, 17);
            this.dutyDeliveryPaid.TabIndex = 83;
            this.dutyDeliveryPaid.Text = "Delivery Duty Paid";
            this.dutyDeliveryPaid.UseVisualStyleBackColor = true;
            // 
            // dutyDeliveryPaidState
            // 
            this.dutyDeliveryPaidState.AutoSize = true;
            this.dutyDeliveryPaidState.Checked = true;
            this.dutyDeliveryPaidState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dutyDeliveryPaidState.Location = new System.Drawing.Point(9, 43);
            this.dutyDeliveryPaidState.Name = "dutyDeliveryPaidState";
            this.dutyDeliveryPaidState.Size = new System.Drawing.Size(15, 14);
            this.dutyDeliveryPaidState.TabIndex = 82;
            this.dutyDeliveryPaidState.Tag = "";
            this.dutyDeliveryPaidState.UseVisualStyleBackColor = true;
            // 
            // saturdayDelivery
            // 
            this.saturdayDelivery.AutoSize = true;
            this.saturdayDelivery.Location = new System.Drawing.Point(128, 19);
            this.saturdayDelivery.Name = "saturdayDelivery";
            this.saturdayDelivery.Size = new System.Drawing.Size(112, 17);
            this.saturdayDelivery.TabIndex = 78;
            this.saturdayDelivery.Text = "Saturday Delivery";
            this.saturdayDelivery.UseVisualStyleBackColor = true;
            // 
            // saturdayState
            // 
            this.saturdayState.AutoSize = true;
            this.saturdayState.Checked = true;
            this.saturdayState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saturdayState.Location = new System.Drawing.Point(9, 20);
            this.saturdayState.Name = "saturdayState";
            this.saturdayState.Size = new System.Drawing.Size(15, 14);
            this.saturdayState.TabIndex = 77;
            this.saturdayState.Tag = "";
            this.saturdayState.UseVisualStyleBackColor = true;
            // 
            // kryptonBorderEdge2
            // 
            this.kryptonBorderEdge2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge2.AutoSize = false;
            this.kryptonBorderEdge2.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge2.Location = new System.Drawing.Point(29, 19);
            this.kryptonBorderEdge2.Name = "kryptonBorderEdge2";
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 163);
            this.kryptonBorderEdge2.Text = "kryptonBorderEdge1";
            // 
            // groupShipment
            // 
            this.groupShipment.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupShipment.Controls.Add(this.kryptonBorderEdge);
            this.groupShipment.Controls.Add(this.labelService);
            this.groupShipment.Controls.Add(this.service);
            this.groupShipment.Controls.Add(this.serviceState);
            this.groupShipment.Location = new System.Drawing.Point(6, 66);
            this.groupShipment.Name = "groupShipment";
            this.groupShipment.Size = new System.Drawing.Size(405, 54);
            this.groupShipment.TabIndex = 5;
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
            this.kryptonBorderEdge.Size = new System.Drawing.Size(1, 26);
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.BackColor = System.Drawing.Color.Transparent;
            this.labelService.Location = new System.Drawing.Point(76, 24);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(46, 13);
            this.labelService.TabIndex = 52;
            this.labelService.Text = "Service:";
            // 
            // service
            // 
            this.service.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.service.FormattingEnabled = true;
            this.service.Location = new System.Drawing.Point(128, 21);
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
            this.groupBoxFrom.Controls.Add(this.accountState);
            this.groupBoxFrom.Controls.Add(this.dhlExpressAccount);
            this.groupBoxFrom.Controls.Add(this.kryptonBorderEdge1);
            this.groupBoxFrom.Location = new System.Drawing.Point(6, 6);
            this.groupBoxFrom.Name = "groupBoxFrom";
            this.groupBoxFrom.Size = new System.Drawing.Size(405, 53);
            this.groupBoxFrom.TabIndex = 3;
            this.groupBoxFrom.TabStop = false;
            this.groupBoxFrom.Text = "From";
            // 
            // labelAccount
            // 
            this.labelAccount.AutoSize = true;
            this.labelAccount.Location = new System.Drawing.Point(72, 23);
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
            // dhlExpressAccount
            // 
            this.dhlExpressAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dhlExpressAccount.FormattingEnabled = true;
            this.dhlExpressAccount.Location = new System.Drawing.Point(128, 20);
            this.dhlExpressAccount.Name = "dhlExpressAccount";
            this.dhlExpressAccount.PromptText = "(Multiple Values)";
            this.dhlExpressAccount.Size = new System.Drawing.Size(206, 21);
            this.dhlExpressAccount.TabIndex = 1;
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(29, 17);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(1, 25);
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
            this.groupBoxPackages.TabIndex = 1;
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
            // contents
            // 
            this.contents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.contents.FormattingEnabled = true;
            this.contents.Location = new System.Drawing.Point(128, 21);
            this.contents.Name = "contents";
            this.contents.PromptText = "(Multiple Values)";
            this.contents.Size = new System.Drawing.Size(206, 21);
            this.contents.TabIndex = 88;
            // 
            // labelContents
            // 
            this.labelContents.AutoSize = true;
            this.labelContents.BackColor = System.Drawing.Color.Transparent;
            this.labelContents.Location = new System.Drawing.Point(67, 24);
            this.labelContents.Name = "labelContents";
            this.labelContents.Size = new System.Drawing.Size(55, 13);
            this.labelContents.TabIndex = 89;
            this.labelContents.Text = "Contents:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.contentsState);
            this.groupBox1.Controls.Add(this.nonDeliveryState);
            this.groupBox1.Controls.Add(this.labelNonDelivery);
            this.groupBox1.Controls.Add(this.nonDelivery);
            this.groupBox1.Controls.Add(this.labelContents);
            this.groupBox1.Controls.Add(this.kryptonBorderEdge4);
            this.groupBox1.Controls.Add(this.contents);
            this.groupBox1.Location = new System.Drawing.Point(6, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(405, 81);
            this.groupBox1.TabIndex = 77;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Customs";
            // 
            // kryptonBorderEdge4
            // 
            this.kryptonBorderEdge4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge4.AutoSize = false;
            this.kryptonBorderEdge4.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge4.Location = new System.Drawing.Point(29, 20);
            this.kryptonBorderEdge4.Name = "kryptonBorderEdge11";
            this.kryptonBorderEdge4.Size = new System.Drawing.Size(1, 50);
            this.kryptonBorderEdge4.Text = "kryptonBorderEdge11";
            // 
            // nonDelivery
            // 
            this.nonDelivery.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.nonDelivery.FormattingEnabled = true;
            this.nonDelivery.Location = new System.Drawing.Point(128, 48);
            this.nonDelivery.Name = "nonDelivery";
            this.nonDelivery.PromptText = "(Multiple Values)";
            this.nonDelivery.Size = new System.Drawing.Size(206, 21);
            this.nonDelivery.TabIndex = 105;
            // 
            // labelNonDelivery
            // 
            this.labelNonDelivery.AutoSize = true;
            this.labelNonDelivery.BackColor = System.Drawing.Color.Transparent;
            this.labelNonDelivery.Location = new System.Drawing.Point(50, 51);
            this.labelNonDelivery.Name = "labelNonDelivery";
            this.labelNonDelivery.Size = new System.Drawing.Size(72, 13);
            this.labelNonDelivery.TabIndex = 106;
            this.labelNonDelivery.Text = "Non Delivery:";
            // 
            // nonDeliveryState
            // 
            this.nonDeliveryState.AutoSize = true;
            this.nonDeliveryState.Checked = true;
            this.nonDeliveryState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nonDeliveryState.Location = new System.Drawing.Point(9, 51);
            this.nonDeliveryState.Name = "nonDeliveryState";
            this.nonDeliveryState.Size = new System.Drawing.Size(15, 14);
            this.nonDeliveryState.TabIndex = 107;
            this.nonDeliveryState.Tag = "";
            this.nonDeliveryState.UseVisualStyleBackColor = true;
            // 
            // contentsState
            // 
            this.contentsState.AutoSize = true;
            this.contentsState.Checked = true;
            this.contentsState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.contentsState.Location = new System.Drawing.Point(9, 24);
            this.contentsState.Name = "contentsState";
            this.contentsState.Size = new System.Drawing.Size(15, 14);
            this.contentsState.TabIndex = 78;
            this.contentsState.Tag = "";
            this.contentsState.UseVisualStyleBackColor = true;
            // 
            // DhlExpressProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Name = "DhlExpressProfileControl";
            this.Size = new System.Drawing.Size(425, 1127);
            this.tabControl.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.groupLabels.ResumeLayout(false);
            this.groupLabels.PerformLayout();
            this.groupOptions.ResumeLayout(false);
            this.groupOptions.PerformLayout();
            this.groupShipment.ResumeLayout(false);
            this.groupShipment.PerformLayout();
            this.groupBoxFrom.ResumeLayout(false);
            this.groupBoxFrom.PerformLayout();
            this.tabPagePackages.ResumeLayout(false);
            this.groupBoxPackages.ResumeLayout(false);
            this.groupBoxPackages.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSettings;
        private System.Windows.Forms.GroupBox groupShipment;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private System.Windows.Forms.Label labelService;
        private ShipWorks.UI.Controls.MultiValueComboBox service;
        private System.Windows.Forms.CheckBox serviceState;
        private System.Windows.Forms.GroupBox groupBoxFrom;
        private System.Windows.Forms.Label labelAccount;
        private ShipWorks.UI.Controls.MultiValueComboBox dhlExpressAccount;
        private System.Windows.Forms.CheckBox accountState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
        private System.Windows.Forms.GroupBox groupOptions;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge2;
        private System.Windows.Forms.CheckBox saturdayDelivery;
        private System.Windows.Forms.Label labelSaturday;
        private System.Windows.Forms.CheckBox saturdayState;
        private System.Windows.Forms.Label labelDuty;
        private System.Windows.Forms.CheckBox dutyDeliveryPaid;
        private System.Windows.Forms.CheckBox dutyDeliveryPaidState;
        private System.Windows.Forms.TabPage tabPagePackages;
        protected System.Windows.Forms.GroupBox groupBoxPackages;
        private ShipWorks.UI.Controls.MultiValueComboBox packagesCount;
        private System.Windows.Forms.Label labelPackages;
        protected System.Windows.Forms.CheckBox packagesState;
        private System.Windows.Forms.Panel panelPackageControls;
        private System.Windows.Forms.CheckBox nonMachinableState;
        private System.Windows.Forms.Label labelNonMachinable;
        protected System.Windows.Forms.GroupBox groupLabels;
        protected Editing.RequestedLabelFormatProfileControl requestedLabelFormat;
        protected System.Windows.Forms.CheckBox requestedLabelFormatState;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge11;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge3;
        private System.Windows.Forms.Label labelThermalNote;
        private System.Windows.Forms.CheckBox nonMachinable;
        protected System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox contentsState;
        private System.Windows.Forms.CheckBox nonDeliveryState;
        private System.Windows.Forms.Label labelNonDelivery;
        private ShipWorks.UI.Controls.MultiValueComboBox nonDelivery;
        private System.Windows.Forms.Label labelContents;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge4;
        private ShipWorks.UI.Controls.MultiValueComboBox contents;
    }
}
