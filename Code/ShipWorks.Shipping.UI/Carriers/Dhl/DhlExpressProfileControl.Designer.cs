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
            this.nonMachinableState = new System.Windows.Forms.CheckBox();
            this.labelDuty = new System.Windows.Forms.Label();
            this.labelNonMachinable = new System.Windows.Forms.Label();
            this.dutyDeliveryPaid = new System.Windows.Forms.CheckBox();
            this.dutyDeliveryPaidState = new System.Windows.Forms.CheckBox();
            this.saturdayDelivery = new System.Windows.Forms.CheckBox();
            this.labelSaturday = new System.Windows.Forms.Label();
            this.saturdayState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge2 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.groupShipment = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.labelService = new System.Windows.Forms.Label();
            this.service = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.serviceState = new System.Windows.Forms.CheckBox();
            this.groupBoxFrom = new System.Windows.Forms.GroupBox();
            this.labelAccount = new System.Windows.Forms.Label();
            this.dhlExpressAccount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.accountState = new System.Windows.Forms.CheckBox();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.tabPagePackages = new System.Windows.Forms.TabPage();
            this.panelPackageControls = new System.Windows.Forms.Panel();
            this.groupBoxPackages = new System.Windows.Forms.GroupBox();
            this.kryptonBorderEdge3 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.packagesCount = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelPackages = new System.Windows.Forms.Label();
            this.packagesState = new System.Windows.Forms.CheckBox();
            this.nonMachinable = new System.Windows.Forms.CheckBox();
            this.tabControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.groupLabels.SuspendLayout();
            this.groupOptions.SuspendLayout();
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
            this.groupLabels.Location = new System.Drawing.Point(6, 225);
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
            this.labelThermalNote.Text = "Note: DHL Express only supports EPL thermal labels.";
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
            this.groupOptions.Size = new System.Drawing.Size(405, 91);
            this.groupOptions.TabIndex = 75;
            this.groupOptions.TabStop = false;
            this.groupOptions.Text = "Options";
            // 
            // nonMachinableState
            // 
            this.nonMachinableState.AutoSize = true;
            this.nonMachinableState.Checked = true;
            this.nonMachinableState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nonMachinableState.Location = new System.Drawing.Point(6, 66);
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
            // dutyDeliveryPaid
            // 
            this.dutyDeliveryPaid.AutoSize = true;
            this.dutyDeliveryPaid.Location = new System.Drawing.Point(128, 42);
            this.dutyDeliveryPaid.Name = "dutyDeliveryPaid";
            this.dutyDeliveryPaid.Size = new System.Drawing.Size(114, 17);
            this.dutyDeliveryPaid.TabIndex = 83;
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
            // saturdayState
            // 
            this.saturdayState.AutoSize = true;
            this.saturdayState.Checked = true;
            this.saturdayState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saturdayState.Location = new System.Drawing.Point(6, 20);
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
            this.kryptonBorderEdge2.Size = new System.Drawing.Size(1, 158);
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
    }
}
