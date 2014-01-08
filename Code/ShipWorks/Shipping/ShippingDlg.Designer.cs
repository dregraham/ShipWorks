﻿namespace ShipWorks.Shipping
{
    partial class ShippingDlg
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelProcessing = new System.Windows.Forms.Label();
            this.close = new System.Windows.Forms.Button();
            this.labelSettings = new System.Windows.Forms.Label();
            this.shippingServices = new System.Windows.Forms.Button();
            this.voidSelected = new System.Windows.Forms.Button();
            this.contextMenuProcess = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuProcessSelected = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProcessAll = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuProfiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuProfilePlaceholder = new System.Windows.Forms.ToolStripMenuItem();
            this.labelPrep = new System.Windows.Forms.Label();
            this.print = new System.Windows.Forms.Button();
            this.getRates = new System.Windows.Forms.Button();
            this.panelEditButtons = new System.Windows.Forms.Panel();
            this.processAll = new System.Windows.Forms.Button();
            this.processSelected = new System.Windows.Forms.Button();
            this.panelSettingsButtons = new System.Windows.Forms.Panel();
            this.labelInternal = new System.Windows.Forms.Label();
            this.unprocess = new System.Windows.Forms.Button();
            this.applyProfile = new ShipWorks.UI.Controls.DropDownButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.shipmentControl = new ShipWorks.Shipping.Editing.ShipmentGridControl();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageService = new System.Windows.Forms.TabPage();
            this.serviceControlArea = new System.Windows.Forms.Panel();
            this.comboShipmentType = new ShipWorks.UI.Controls.MultiValueComboBox();
            this.labelService = new System.Windows.Forms.Label();
            this.tabPageCustoms = new System.Windows.Forms.TabPage();
            this.customsControlArea = new System.Windows.Forms.Panel();
            this.tabPageTracking = new System.Windows.Forms.TabPage();
            this.panelTrackingData = new System.Windows.Forms.Panel();
            this.trackingControl = new ShipWorks.Shipping.Tracking.ShipmentTrackingControl();
            this.track = new System.Windows.Forms.Button();
            this.trackingNumbers = new System.Windows.Forms.TextBox();
            this.labelTrackingNumbers = new System.Windows.Forms.Label();
            this.trackingCost = new System.Windows.Forms.Label();
            this.labelTrackingCost = new System.Windows.Forms.Label();
            this.trackingProcessedDate = new System.Windows.Forms.Label();
            this.labelTrackingProcessed = new System.Windows.Forms.Label();
            this.panelTrackingMessage = new System.Windows.Forms.Panel();
            this.labelTrackingMessage = new System.Windows.Forms.Label();
            this.contextMenuProcess.SuspendLayout();
            this.contextMenuProfiles.SuspendLayout();
            this.panelEditButtons.SuspendLayout();
            this.panelSettingsButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageService.SuspendLayout();
            this.tabPageCustoms.SuspendLayout();
            this.tabPageTracking.SuspendLayout();
            this.panelTrackingData.SuspendLayout();
            this.panelTrackingMessage.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelProcessing
            // 
            this.labelProcessing.AutoSize = true;
            this.labelProcessing.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcessing.Location = new System.Drawing.Point(6, 0);
            this.labelProcessing.Name = "labelProcessing";
            this.labelProcessing.Size = new System.Drawing.Size(68, 13);
            this.labelProcessing.TabIndex = 0;
            this.labelProcessing.Text = "Processing";
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(747, 504);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 2;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSettings.Location = new System.Drawing.Point(6, 6);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(54, 13);
            this.labelSettings.TabIndex = 7;
            this.labelSettings.Text = "Settings";
            // 
            // shippingServices
            // 
            this.shippingServices.Image = global::ShipWorks.Properties.Resources.box_preferences;
            this.shippingServices.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.shippingServices.Location = new System.Drawing.Point(9, 22);
            this.shippingServices.Name = "shippingServices";
            this.shippingServices.Size = new System.Drawing.Size(150, 23);
            this.shippingServices.TabIndex = 8;
            this.shippingServices.Text = "Settings...";
            this.shippingServices.UseVisualStyleBackColor = true;
            this.shippingServices.Click += new System.EventHandler(this.OnShippingSettings);
            // 
            // voidSelected
            // 
            this.voidSelected.Image = global::ShipWorks.Properties.Resources.cancel16;
            this.voidSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.voidSelected.Location = new System.Drawing.Point(9, 178);
            this.voidSelected.Name = "voidSelected";
            this.voidSelected.Size = new System.Drawing.Size(150, 23);
            this.voidSelected.TabIndex = 6;
            this.voidSelected.Text = "Void";
            this.voidSelected.UseVisualStyleBackColor = true;
            this.voidSelected.Click += new System.EventHandler(this.OnVoid);
            // 
            // contextMenuProcess
            // 
            this.contextMenuProcess.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuProcess.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProcessSelected,
            this.menuProcessAll});
            this.contextMenuProcess.Name = "contextMenuProcess";
            this.contextMenuProcess.Size = new System.Drawing.Size(162, 48);
            // 
            // menuProcessSelected
            // 
            this.menuProcessSelected.Name = "menuProcessSelected";
            this.menuProcessSelected.Size = new System.Drawing.Size(161, 22);
            this.menuProcessSelected.Text = "Process Selected";
            this.menuProcessSelected.Click += new System.EventHandler(this.OnProcessSelected);
            // 
            // menuProcessAll
            // 
            this.menuProcessAll.Name = "menuProcessAll";
            this.menuProcessAll.Size = new System.Drawing.Size(161, 22);
            this.menuProcessAll.Text = "Process All";
            this.menuProcessAll.Click += new System.EventHandler(this.OnProcessAll);
            // 
            // contextMenuProfiles
            // 
            this.contextMenuProfiles.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.contextMenuProfiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProfilePlaceholder});
            this.contextMenuProfiles.Name = "contextMenuPrint";
            this.contextMenuProfiles.Size = new System.Drawing.Size(115, 26);
            this.contextMenuProfiles.Opening += new System.ComponentModel.CancelEventHandler(this.OnOpeningProfilesMenu);
            // 
            // menuProfilePlaceholder
            // 
            this.menuProfilePlaceholder.Name = "menuProfilePlaceholder";
            this.menuProfilePlaceholder.Size = new System.Drawing.Size(114, 22);
            this.menuProfilePlaceholder.Text = "Profile1";
            // 
            // labelPrep
            // 
            this.labelPrep.AutoSize = true;
            this.labelPrep.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrep.Location = new System.Drawing.Point(6, 48);
            this.labelPrep.Name = "labelPrep";
            this.labelPrep.Size = new System.Drawing.Size(119, 13);
            this.labelPrep.TabIndex = 2;
            this.labelPrep.Text = "Selected Shipments";
            // 
            // print
            // 
            this.print.Image = global::ShipWorks.Properties.Resources.printer1;
            this.print.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.print.Location = new System.Drawing.Point(9, 149);
            this.print.Name = "print";
            this.print.Size = new System.Drawing.Size(150, 23);
            this.print.TabIndex = 5;
            this.print.Text = "Print";
            this.print.UseVisualStyleBackColor = true;
            this.print.Click += new System.EventHandler(this.OnPrint);
            // 
            // getRates
            // 
            this.getRates.Image = global::ShipWorks.Properties.Resources.currency_dollar_green16;
            this.getRates.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.getRates.Location = new System.Drawing.Point(9, 92);
            this.getRates.Name = "getRates";
            this.getRates.Size = new System.Drawing.Size(150, 23);
            this.getRates.TabIndex = 3;
            this.getRates.Text = "Get Rates";
            this.getRates.UseVisualStyleBackColor = true;
            this.getRates.Click += new System.EventHandler(this.OnGetRates);
            // 
            // panelEditButtons
            // 
            this.panelEditButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelEditButtons.Controls.Add(this.processAll);
            this.panelEditButtons.Controls.Add(this.processSelected);
            this.panelEditButtons.Controls.Add(this.panelSettingsButtons);
            this.panelEditButtons.Controls.Add(this.labelInternal);
            this.panelEditButtons.Controls.Add(this.unprocess);
            this.panelEditButtons.Controls.Add(this.applyProfile);
            this.panelEditButtons.Controls.Add(this.labelProcessing);
            this.panelEditButtons.Controls.Add(this.voidSelected);
            this.panelEditButtons.Controls.Add(this.labelPrep);
            this.panelEditButtons.Controls.Add(this.getRates);
            this.panelEditButtons.Controls.Add(this.print);
            this.panelEditButtons.Location = new System.Drawing.Point(665, 12);
            this.panelEditButtons.Name = "panelEditButtons";
            this.panelEditButtons.Size = new System.Drawing.Size(160, 332);
            this.panelEditButtons.TabIndex = 1;
            // 
            // processAll
            // 
            this.processAll.Image = global::ShipWorks.Properties.Resources.box_next;
            this.processAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.processAll.Location = new System.Drawing.Point(10, 16);
            this.processAll.Name = "processAll";
            this.processAll.Size = new System.Drawing.Size(150, 23);
            this.processAll.TabIndex = 13;
            this.processAll.Text = "Process All";
            this.processAll.UseVisualStyleBackColor = true;
            this.processAll.Click += new System.EventHandler(this.OnProcessAll);
            // 
            // processSelected
            // 
            this.processSelected.Image = global::ShipWorks.Properties.Resources.box_next;
            this.processSelected.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.processSelected.Location = new System.Drawing.Point(9, 64);
            this.processSelected.Name = "processSelected";
            this.processSelected.Size = new System.Drawing.Size(150, 23);
            this.processSelected.TabIndex = 12;
            this.processSelected.Text = "Process";
            this.processSelected.UseVisualStyleBackColor = true;
            this.processSelected.Click += new System.EventHandler(this.OnProcessSelected);
            // 
            // panelSettingsButtons
            // 
            this.panelSettingsButtons.Controls.Add(this.labelSettings);
            this.panelSettingsButtons.Controls.Add(this.shippingServices);
            this.panelSettingsButtons.Location = new System.Drawing.Point(0, 203);
            this.panelSettingsButtons.Name = "panelSettingsButtons";
            this.panelSettingsButtons.Size = new System.Drawing.Size(160, 78);
            this.panelSettingsButtons.TabIndex = 3;
            // 
            // labelInternal
            // 
            this.labelInternal.AutoSize = true;
            this.labelInternal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInternal.Location = new System.Drawing.Point(6, 281);
            this.labelInternal.Name = "labelInternal";
            this.labelInternal.Size = new System.Drawing.Size(53, 13);
            this.labelInternal.TabIndex = 10;
            this.labelInternal.Text = "Internal";
            // 
            // unprocess
            // 
            this.unprocess.Image = global::ShipWorks.Properties.Resources.skull;
            this.unprocess.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.unprocess.Location = new System.Drawing.Point(8, 298);
            this.unprocess.Name = "unprocess";
            this.unprocess.Size = new System.Drawing.Size(151, 23);
            this.unprocess.TabIndex = 11;
            this.unprocess.Text = "Unprocess";
            this.unprocess.UseVisualStyleBackColor = true;
            this.unprocess.Click += new System.EventHandler(this.OnUnprocess);
            // 
            // applyProfile
            // 
            this.applyProfile.AutoSize = true;
            this.applyProfile.ContextMenuStrip = this.contextMenuProfiles;
            this.applyProfile.Image = global::ShipWorks.Properties.Resources.document_out;
            this.applyProfile.Location = new System.Drawing.Point(9, 121);
            this.applyProfile.Name = "applyProfile";
            this.applyProfile.Size = new System.Drawing.Size(150, 23);
            this.applyProfile.SplitButton = false;
            this.applyProfile.SplitContextMenu = this.contextMenuProfiles;
            this.applyProfile.TabIndex = 4;
            this.applyProfile.Text = "Apply Profile";
            this.applyProfile.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(12, 12);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.shipmentControl);
            this.splitContainer.Panel1MinSize = 230;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControl);
            this.splitContainer.Size = new System.Drawing.Size(651, 489);
            this.splitContainer.SplitterDistance = 291;
            this.splitContainer.TabIndex = 0;
            this.splitContainer.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.OnSplitterMoved);
            // 
            // shipmentControl
            // 
            this.shipmentControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.shipmentControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shipmentControl.Location = new System.Drawing.Point(0, 0);
            this.shipmentControl.Name = "shipmentControl";
            this.shipmentControl.Size = new System.Drawing.Size(291, 489);
            this.shipmentControl.TabIndex = 0;
            this.shipmentControl.SelectionChanged += new ShipWorks.Shipping.Editing.ShipmentSelectionChangedEventHandler(this.OnChangeSelectedShipments);
            this.shipmentControl.ShipmentsAdded += new System.EventHandler(this.OnShipmentsAdded);
            this.shipmentControl.ShipmentsRemoved += new System.EventHandler(this.OnShipmentsRemoved);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageService);
            this.tabControl.Controls.Add(this.tabPageCustoms);
            this.tabControl.Controls.Add(this.tabPageTracking);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(356, 489);
            this.tabControl.TabIndex = 0;
            this.tabControl.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.OnTabDeselecting);
            // 
            // tabPageService
            // 
            this.tabPageService.Controls.Add(this.serviceControlArea);
            this.tabPageService.Controls.Add(this.comboShipmentType);
            this.tabPageService.Controls.Add(this.labelService);
            this.tabPageService.Location = new System.Drawing.Point(4, 22);
            this.tabPageService.Name = "tabPageService";
            this.tabPageService.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageService.Size = new System.Drawing.Size(348, 463);
            this.tabPageService.TabIndex = 0;
            this.tabPageService.Text = "Service";
            this.tabPageService.UseVisualStyleBackColor = true;
            // 
            // serviceControlArea
            // 
            this.serviceControlArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceControlArea.Location = new System.Drawing.Point(6, 36);
            this.serviceControlArea.Name = "serviceControlArea";
            this.serviceControlArea.Size = new System.Drawing.Size(336, 427);
            this.serviceControlArea.TabIndex = 2;
            // 
            // comboShipmentType
            // 
            this.comboShipmentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboShipmentType.FormattingEnabled = true;
            this.comboShipmentType.Items.AddRange(new object[] {
            "FedEx"});
            this.comboShipmentType.Location = new System.Drawing.Point(60, 9);
            this.comboShipmentType.Name = "comboShipmentType";
            this.comboShipmentType.PromptText = "(Multiple Values)";
            this.comboShipmentType.Size = new System.Drawing.Size(184, 21);
            this.comboShipmentType.TabIndex = 1;
            this.comboShipmentType.SelectedIndexChanged += new System.EventHandler(this.OnChangeShipmentType);
            // 
            // labelService
            // 
            this.labelService.AutoSize = true;
            this.labelService.Location = new System.Drawing.Point(7, 12);
            this.labelService.Name = "labelService";
            this.labelService.Size = new System.Drawing.Size(51, 13);
            this.labelService.TabIndex = 0;
            this.labelService.Text = "Provider:";
            // 
            // tabPageCustoms
            // 
            this.tabPageCustoms.Controls.Add(this.customsControlArea);
            this.tabPageCustoms.Location = new System.Drawing.Point(4, 22);
            this.tabPageCustoms.Name = "tabPageCustoms";
            this.tabPageCustoms.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCustoms.Size = new System.Drawing.Size(348, 463);
            this.tabPageCustoms.TabIndex = 2;
            this.tabPageCustoms.Text = "Customs";
            this.tabPageCustoms.UseVisualStyleBackColor = true;
            // 
            // customsControlArea
            // 
            this.customsControlArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customsControlArea.Location = new System.Drawing.Point(5, 6);
            this.customsControlArea.Name = "customsControlArea";
            this.customsControlArea.Size = new System.Drawing.Size(374, 454);
            this.customsControlArea.TabIndex = 0;
            // 
            // tabPageTracking
            // 
            this.tabPageTracking.Controls.Add(this.panelTrackingData);
            this.tabPageTracking.Controls.Add(this.panelTrackingMessage);
            this.tabPageTracking.Location = new System.Drawing.Point(4, 22);
            this.tabPageTracking.Name = "tabPageTracking";
            this.tabPageTracking.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTracking.Size = new System.Drawing.Size(348, 463);
            this.tabPageTracking.TabIndex = 3;
            this.tabPageTracking.Text = "Tracking";
            this.tabPageTracking.UseVisualStyleBackColor = true;
            // 
            // panelTrackingData
            // 
            this.panelTrackingData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTrackingData.Controls.Add(this.trackingControl);
            this.panelTrackingData.Controls.Add(this.track);
            this.panelTrackingData.Controls.Add(this.trackingNumbers);
            this.panelTrackingData.Controls.Add(this.labelTrackingNumbers);
            this.panelTrackingData.Controls.Add(this.trackingCost);
            this.panelTrackingData.Controls.Add(this.labelTrackingCost);
            this.panelTrackingData.Controls.Add(this.trackingProcessedDate);
            this.panelTrackingData.Controls.Add(this.labelTrackingProcessed);
            this.panelTrackingData.Location = new System.Drawing.Point(6, 6);
            this.panelTrackingData.Name = "panelTrackingData";
            this.panelTrackingData.Size = new System.Drawing.Size(373, 454);
            this.panelTrackingData.TabIndex = 1;
            this.panelTrackingData.Visible = false;
            // 
            // trackingControl
            // 
            this.trackingControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackingControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trackingControl.Location = new System.Drawing.Point(33, 77);
            this.trackingControl.Name = "trackingControl";
            this.trackingControl.Size = new System.Drawing.Size(325, 364);
            this.trackingControl.TabIndex = 8;
            // 
            // track
            // 
            this.track.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.track.Location = new System.Drawing.Point(283, 49);
            this.track.Name = "track";
            this.track.Size = new System.Drawing.Size(75, 23);
            this.track.TabIndex = 7;
            this.track.Text = "Track";
            this.track.UseVisualStyleBackColor = true;
            this.track.Click += new System.EventHandler(this.OnTrack);
            // 
            // trackingNumbers
            // 
            this.trackingNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackingNumbers.Location = new System.Drawing.Point(90, 51);
            this.trackingNumbers.Multiline = true;
            this.trackingNumbers.Name = "trackingNumbers";
            this.trackingNumbers.ReadOnly = true;
            this.trackingNumbers.Size = new System.Drawing.Size(191, 20);
            this.trackingNumbers.TabIndex = 5;
            // 
            // labelTrackingNumbers
            // 
            this.labelTrackingNumbers.AutoSize = true;
            this.labelTrackingNumbers.Location = new System.Drawing.Point(30, 54);
            this.labelTrackingNumbers.Name = "labelTrackingNumbers";
            this.labelTrackingNumbers.Size = new System.Drawing.Size(51, 13);
            this.labelTrackingNumbers.TabIndex = 4;
            this.labelTrackingNumbers.Text = "Tracking:";
            // 
            // trackingCost
            // 
            this.trackingCost.AutoSize = true;
            this.trackingCost.Location = new System.Drawing.Point(87, 29);
            this.trackingCost.Name = "trackingCost";
            this.trackingCost.Size = new System.Drawing.Size(35, 13);
            this.trackingCost.TabIndex = 3;
            this.trackingCost.Text = "$4.59";
            // 
            // labelTrackingCost
            // 
            this.labelTrackingCost.AutoSize = true;
            this.labelTrackingCost.Location = new System.Drawing.Point(48, 29);
            this.labelTrackingCost.Name = "labelTrackingCost";
            this.labelTrackingCost.Size = new System.Drawing.Size(33, 13);
            this.labelTrackingCost.TabIndex = 2;
            this.labelTrackingCost.Text = "Cost:";
            // 
            // trackingProcessedDate
            // 
            this.trackingProcessedDate.AutoSize = true;
            this.trackingProcessedDate.Location = new System.Drawing.Point(87, 8);
            this.trackingProcessedDate.Name = "trackingProcessedDate";
            this.trackingProcessedDate.Size = new System.Drawing.Size(99, 13);
            this.trackingProcessedDate.TabIndex = 1;
            this.trackingProcessedDate.Text = "06/08/09 12:34 PM";
            // 
            // labelTrackingProcessed
            // 
            this.labelTrackingProcessed.AutoSize = true;
            this.labelTrackingProcessed.Location = new System.Drawing.Point(6, 8);
            this.labelTrackingProcessed.Name = "labelTrackingProcessed";
            this.labelTrackingProcessed.Size = new System.Drawing.Size(75, 13);
            this.labelTrackingProcessed.TabIndex = 0;
            this.labelTrackingProcessed.Text = "Processed on:";
            // 
            // panelTrackingMessage
            // 
            this.panelTrackingMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTrackingMessage.Controls.Add(this.labelTrackingMessage);
            this.panelTrackingMessage.Location = new System.Drawing.Point(6, 6);
            this.panelTrackingMessage.Name = "panelTrackingMessage";
            this.panelTrackingMessage.Size = new System.Drawing.Size(373, 129);
            this.panelTrackingMessage.TabIndex = 0;
            this.panelTrackingMessage.Visible = false;
            // 
            // labelTrackingMessage
            // 
            this.labelTrackingMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTrackingMessage.Location = new System.Drawing.Point(5, 4);
            this.labelTrackingMessage.Name = "labelTrackingMessage";
            this.labelTrackingMessage.Size = new System.Drawing.Size(365, 55);
            this.labelTrackingMessage.TabIndex = 0;
            this.labelTrackingMessage.Text = "Multiple shipments are selected.  Select a single shipment to get tracking inform" +
    "ation.";
            // 
            // ShippingDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(834, 539);
            this.Controls.Add(this.panelEditButtons);
            this.Controls.Add(this.close);
            this.Controls.Add(this.splitContainer);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(780, 461);
            this.Name = "ShippingDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ship Orders";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.Shown += new System.EventHandler(this.OnShown);
            this.contextMenuProcess.ResumeLayout(false);
            this.contextMenuProfiles.ResumeLayout(false);
            this.panelEditButtons.ResumeLayout(false);
            this.panelEditButtons.PerformLayout();
            this.panelSettingsButtons.ResumeLayout(false);
            this.panelSettingsButtons.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageService.ResumeLayout(false);
            this.tabPageService.PerformLayout();
            this.tabPageCustoms.ResumeLayout(false);
            this.tabPageTracking.ResumeLayout(false);
            this.panelTrackingData.ResumeLayout(false);
            this.panelTrackingData.PerformLayout();
            this.panelTrackingMessage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelProcessing;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Button voidSelected;
        private System.Windows.Forms.Button shippingServices;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageService;
        private System.Windows.Forms.TabPage tabPageCustoms;
        private System.Windows.Forms.Label labelSettings;
        private ShipWorks.Shipping.Editing.ShipmentGridControl shipmentControl;
        private System.Windows.Forms.ContextMenuStrip contextMenuProcess;
        private System.Windows.Forms.ToolStripMenuItem menuProcessSelected;
        private System.Windows.Forms.ToolStripMenuItem menuProcessAll;
        private System.Windows.Forms.ContextMenuStrip contextMenuProfiles;
        private System.Windows.Forms.ToolStripMenuItem menuProfilePlaceholder;
        private ShipWorks.UI.Controls.MultiValueComboBox comboShipmentType;
        private System.Windows.Forms.Label labelService;
        private System.Windows.Forms.Label labelPrep;
        private System.Windows.Forms.Panel serviceControlArea;
        private System.Windows.Forms.Button print;
        private System.Windows.Forms.Button getRates;
        private System.Windows.Forms.Panel panelEditButtons;
        private System.Windows.Forms.Panel customsControlArea;
        private ShipWorks.UI.Controls.DropDownButton applyProfile;
        private System.Windows.Forms.TabPage tabPageTracking;
        private System.Windows.Forms.Panel panelTrackingMessage;
        private System.Windows.Forms.Label labelTrackingMessage;
        private System.Windows.Forms.Panel panelTrackingData;
        private System.Windows.Forms.Label trackingCost;
        private System.Windows.Forms.Label labelTrackingCost;
        private System.Windows.Forms.Label trackingProcessedDate;
        private System.Windows.Forms.Label labelTrackingProcessed;
        private System.Windows.Forms.Label labelTrackingNumbers;
        private System.Windows.Forms.TextBox trackingNumbers;
        private System.Windows.Forms.Button track;
        private ShipWorks.Shipping.Tracking.ShipmentTrackingControl trackingControl;
        private System.Windows.Forms.Label labelInternal;
        private System.Windows.Forms.Button unprocess;
        private System.Windows.Forms.Panel panelSettingsButtons;
        private System.Windows.Forms.Button processAll;
        private System.Windows.Forms.Button processSelected;
    }
}