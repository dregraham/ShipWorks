namespace ShipWorks.Stores.Management
{
    partial class StoreSettingsDlg
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StoreSettingsDlg));
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.titleStoreType = new System.Windows.Forms.Label();
            this.titleStoreName = new System.Windows.Forms.Label();
            this.imageStore = new System.Windows.Forms.PictureBox();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPageStoreDetails = new ShipWorks.UI.Controls.OptionPage();
            this.storeAddressControl = new ShipWorks.Stores.Management.StoreAddressControl();
            this.storeContactControl = new ShipWorks.Stores.Management.StoreContactControl();
            this.optionPageSettings = new ShipWorks.UI.Controls.OptionPage();
            this.panelDefaultFilters = new System.Windows.Forms.Panel();
            this.CreateDefaultFilters = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.sectionTitle2 = new ShipWorks.UI.Controls.SectionTitle();
            this.panelAddressValidation = new System.Windows.Forms.Panel();
            this.addressValidationSetting = new System.Windows.Forms.ComboBox();
            this.labelAddressValidationSetting = new System.Windows.Forms.Label();
            this.sectionAddressValidation = new ShipWorks.UI.Controls.SectionTitle();
            this.labelAllowDownload = new System.Windows.Forms.Label();
            this.comboAllowDownload = new ShipWorks.Stores.Management.ComputerDownloadAllowedComboBox();
            this.panelStoreStatus = new System.Windows.Forms.Panel();
            this.infotipStoreEnabled = new ShipWorks.UI.Controls.InfoTip();
            this.sectionStatus = new ShipWorks.UI.Controls.SectionTitle();
            this.label4 = new System.Windows.Forms.Label();
            this.storeDisabled = new System.Windows.Forms.CheckBox();
            this.configureDownloadComputers = new ShipWorks.UI.Controls.LinkControl();
            this.sectionAutoDownloads = new ShipWorks.UI.Controls.SectionTitle();
            this.sectionTitleManualOrders = new ShipWorks.UI.Controls.SectionTitle();
            this.automaticDownloadControl = new ShipWorks.Stores.Management.AutomaticDownloadControl();
            this.optionPageOnlineAccount = new ShipWorks.UI.Controls.OptionPage();
            this.optionPageStatusPreset = new ShipWorks.UI.Controls.OptionPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.orderStatusPresets = new ShipWorks.Stores.Management.StatusPresetEditor();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.itemStatusPresets = new ShipWorks.Stores.Management.StatusPresetEditor();
            this.optionPageLicense = new ShipWorks.UI.Controls.OptionPage();
            this.licenseStatus = new System.Windows.Forms.Label();
            this.labelLicense = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.licenseKey = new System.Windows.Forms.TextBox();
            this.changeLicense = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imageStore)).BeginInit();
            this.optionControl.SuspendLayout();
            this.optionPageStoreDetails.SuspendLayout();
            this.optionPageSettings.SuspendLayout();
            this.panelDefaultFilters.SuspendLayout();
            this.panelAddressValidation.SuspendLayout();
            this.panelStoreStatus.SuspendLayout();
            this.optionPageStatusPreset.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.optionPageLicense.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(611, 521);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(692, 521);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // titleStoreType
            // 
            this.titleStoreType.AutoSize = true;
            this.titleStoreType.ForeColor = System.Drawing.Color.DimGray;
            this.titleStoreType.Location = new System.Drawing.Point(48, 26);
            this.titleStoreType.Name = "titleStoreType";
            this.titleStoreType.Size = new System.Drawing.Size(31, 13);
            this.titleStoreType.TabIndex = 9;
            this.titleStoreType.Text = "eBay";
            // 
            // titleStoreName
            // 
            this.titleStoreName.AutoSize = true;
            this.titleStoreName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleStoreName.Location = new System.Drawing.Point(48, 10);
            this.titleStoreName.Name = "titleStoreName";
            this.titleStoreName.Size = new System.Drawing.Size(83, 13);
            this.titleStoreName.TabIndex = 8;
            this.titleStoreName.Text = "Sample Store";
            // 
            // imageStore
            // 
            this.imageStore.Image = global::ShipWorks.Properties.Resources.school32;
            this.imageStore.Location = new System.Drawing.Point(12, 9);
            this.imageStore.Name = "imageStore";
            this.imageStore.Size = new System.Drawing.Size(32, 32);
            this.imageStore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imageStore.TabIndex = 7;
            this.imageStore.TabStop = false;
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPageStoreDetails);
            this.optionControl.Controls.Add(this.optionPageSettings);
            this.optionControl.Controls.Add(this.optionPageOnlineAccount);
            this.optionControl.Controls.Add(this.optionPageStatusPreset);
            this.optionControl.Controls.Add(this.optionPageLicense);
            this.optionControl.Location = new System.Drawing.Point(14, 47);
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(753, 466);
            this.optionControl.TabIndex = 10;
            this.optionControl.Deselecting += new ShipWorks.UI.Controls.OptionControlCancelEventHandler(this.OnPageDeselecting);
            this.optionControl.Selected += new ShipWorks.UI.Controls.OptionControlEventHandler(this.OnPageSelected);
            // 
            // optionPageStoreDetails
            // 
            this.optionPageStoreDetails.BackColor = System.Drawing.Color.White;
            this.optionPageStoreDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageStoreDetails.Controls.Add(this.storeAddressControl);
            this.optionPageStoreDetails.Controls.Add(this.storeContactControl);
            this.optionPageStoreDetails.Location = new System.Drawing.Point(153, 0);
            this.optionPageStoreDetails.Name = "optionPageStoreDetails";
            this.optionPageStoreDetails.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageStoreDetails.Size = new System.Drawing.Size(600, 466);
            this.optionPageStoreDetails.TabIndex = 1;
            this.optionPageStoreDetails.Text = "Store Address";
            // 
            // storeAddressControl
            // 
            this.storeAddressControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeAddressControl.Location = new System.Drawing.Point(6, 8);
            this.storeAddressControl.Name = "storeAddressControl";
            this.storeAddressControl.Size = new System.Drawing.Size(360, 283);
            this.storeAddressControl.TabIndex = 1;
            // 
            // storeContactControl
            // 
            this.storeContactControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeContactControl.Location = new System.Drawing.Point(6, 294);
            this.storeContactControl.Name = "storeContactControl";
            this.storeContactControl.Size = new System.Drawing.Size(358, 115);
            this.storeContactControl.TabIndex = 0;
            // 
            // optionPageSettings
            // 
            this.optionPageSettings.AutoScroll = true;
            this.optionPageSettings.BackColor = System.Drawing.Color.White;
            this.optionPageSettings.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageSettings.Controls.Add(this.panelDefaultFilters);
            this.optionPageSettings.Controls.Add(this.panelAddressValidation);
            this.optionPageSettings.Controls.Add(this.labelAllowDownload);
            this.optionPageSettings.Controls.Add(this.comboAllowDownload);
            this.optionPageSettings.Controls.Add(this.panelStoreStatus);
            this.optionPageSettings.Controls.Add(this.configureDownloadComputers);
            this.optionPageSettings.Controls.Add(this.sectionAutoDownloads);
            this.optionPageSettings.Controls.Add(this.sectionTitleManualOrders);
            this.optionPageSettings.Controls.Add(this.automaticDownloadControl);
            this.optionPageSettings.Location = new System.Drawing.Point(153, 0);
            this.optionPageSettings.Name = "optionPageSettings";
            this.optionPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageSettings.Size = new System.Drawing.Size(600, 466);
            this.optionPageSettings.TabIndex = 7;
            this.optionPageSettings.Text = "Store Settings";
            // 
            // panelDefaultFilters
            // 
            this.panelDefaultFilters.Controls.Add(this.CreateDefaultFilters);
            this.panelDefaultFilters.Controls.Add(this.label3);
            this.panelDefaultFilters.Controls.Add(this.sectionTitle2);
            this.panelDefaultFilters.Location = new System.Drawing.Point(15, 394);
            this.panelDefaultFilters.Name = "panelDefaultFilters";
            this.panelDefaultFilters.Size = new System.Drawing.Size(564, 80);
            this.panelDefaultFilters.TabIndex = 35;
            // 
            // CreateDefaultFilters
            // 
            this.CreateDefaultFilters.Location = new System.Drawing.Point(261, 51);
            this.CreateDefaultFilters.Name = "CreateDefaultFilters";
            this.CreateDefaultFilters.Size = new System.Drawing.Size(127, 23);
            this.CreateDefaultFilters.TabIndex = 30;
            this.CreateDefaultFilters.Text = "Create Default Filters";
            this.CreateDefaultFilters.UseVisualStyleBackColor = true;
            this.CreateDefaultFilters.Click += new System.EventHandler(this.OnClickCreateDefaultFilters);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(435, 13);
            this.label3.TabIndex = 29;
            this.label3.Text = "Create the default filters for this store. This will not overwrite any of your ex" +
    "isting filters.";
            // 
            // sectionTitle2
            // 
            this.sectionTitle2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitle2.Location = new System.Drawing.Point(0, 0);
            this.sectionTitle2.Name = "sectionTitle2";
            this.sectionTitle2.Size = new System.Drawing.Size(564, 22);
            this.sectionTitle2.TabIndex = 28;
            this.sectionTitle2.Text = "Default Filters";
            // 
            // panelAddressValidation
            // 
            this.panelAddressValidation.Controls.Add(this.addressValidationSetting);
            this.panelAddressValidation.Controls.Add(this.labelAddressValidationSetting);
            this.panelAddressValidation.Controls.Add(this.sectionAddressValidation);
            this.panelAddressValidation.Location = new System.Drawing.Point(15, 324);
            this.panelAddressValidation.Name = "panelAddressValidation";
            this.panelAddressValidation.Size = new System.Drawing.Size(564, 64);
            this.panelAddressValidation.TabIndex = 33;
            // 
            // addressValidationSetting
            // 
            this.addressValidationSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.addressValidationSetting.FormattingEnabled = true;
            this.addressValidationSetting.Location = new System.Drawing.Point(261, 28);
            this.addressValidationSetting.Name = "addressValidationSetting";
            this.addressValidationSetting.Size = new System.Drawing.Size(158, 21);
            this.addressValidationSetting.TabIndex = 33;
            // 
            // labelAddressValidationSetting
            // 
            this.labelAddressValidationSetting.AutoSize = true;
            this.labelAddressValidationSetting.Location = new System.Drawing.Point(20, 31);
            this.labelAddressValidationSetting.Name = "labelAddressValidationSetting";
            this.labelAddressValidationSetting.Size = new System.Drawing.Size(235, 13);
            this.labelAddressValidationSetting.TabIndex = 32;
            this.labelAddressValidationSetting.Text = "Tell us how you want to use address validation:";
            // 
            // sectionAddressValidation
            // 
            this.sectionAddressValidation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionAddressValidation.Location = new System.Drawing.Point(0, 0);
            this.sectionAddressValidation.Name = "sectionAddressValidation";
            this.sectionAddressValidation.Size = new System.Drawing.Size(564, 22);
            this.sectionAddressValidation.TabIndex = 3;
            this.sectionAddressValidation.Text = "Address Validation";
            // 
            // labelAllowDownload
            // 
            this.labelAllowDownload.AutoSize = true;
            this.labelAllowDownload.Location = new System.Drawing.Point(32, 48);
            this.labelAllowDownload.Name = "labelAllowDownload";
            this.labelAllowDownload.Size = new System.Drawing.Size(231, 13);
            this.labelAllowDownload.TabIndex = 31;
            this.labelAllowDownload.Text = "Allow this computer to download for this store:";
            // 
            // comboAllowDownload
            // 
            this.comboAllowDownload.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAllowDownload.FormattingEnabled = true;
            this.comboAllowDownload.Location = new System.Drawing.Point(266, 45);
            this.comboAllowDownload.Name = "comboAllowDownload";
            this.comboAllowDownload.Size = new System.Drawing.Size(121, 21);
            this.comboAllowDownload.TabIndex = 30;
            // 
            // panelStoreStatus
            // 
            this.panelStoreStatus.Controls.Add(this.infotipStoreEnabled);
            this.panelStoreStatus.Controls.Add(this.sectionStatus);
            this.panelStoreStatus.Controls.Add(this.label4);
            this.panelStoreStatus.Controls.Add(this.storeDisabled);
            this.panelStoreStatus.Location = new System.Drawing.Point(15, 235);
            this.panelStoreStatus.Name = "panelStoreStatus";
            this.panelStoreStatus.Size = new System.Drawing.Size(564, 80);
            this.panelStoreStatus.TabIndex = 29;
            // 
            // infotipStoreEnabled
            // 
            this.infotipStoreEnabled.Caption = resources.GetString("infotipStoreEnabled.Caption");
            this.infotipStoreEnabled.Location = new System.Drawing.Point(281, 30);
            this.infotipStoreEnabled.Name = "infotipStoreEnabled";
            this.infotipStoreEnabled.Size = new System.Drawing.Size(12, 12);
            this.infotipStoreEnabled.TabIndex = 26;
            this.infotipStoreEnabled.Title = "Store Status";
            // 
            // sectionStatus
            // 
            this.sectionStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionStatus.Location = new System.Drawing.Point(0, 0);
            this.sectionStatus.Name = "sectionStatus";
            this.sectionStatus.Size = new System.Drawing.Size(564, 22);
            this.sectionStatus.TabIndex = 28;
            this.sectionStatus.Text = "Store Status";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label4.Location = new System.Drawing.Point(32, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(319, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "(This does not affect your Interapptive account or billing status.)";
            // 
            // storeDisabled
            // 
            this.storeDisabled.AutoSize = true;
            this.storeDisabled.Location = new System.Drawing.Point(20, 28);
            this.storeDisabled.Name = "storeDisabled";
            this.storeDisabled.Size = new System.Drawing.Size(263, 17);
            this.storeDisabled.TabIndex = 25;
            this.storeDisabled.Text = "I do not actively ship or download with this store.";
            this.storeDisabled.UseVisualStyleBackColor = true;
            this.storeDisabled.CheckedChanged += new System.EventHandler(this.OnChangeEnabledState);
            // 
            // configureDownloadComputers
            // 
            this.configureDownloadComputers.AutoSize = true;
            this.configureDownloadComputers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.configureDownloadComputers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.configureDownloadComputers.ForeColor = System.Drawing.Color.Blue;
            this.configureDownloadComputers.Location = new System.Drawing.Point(394, 48);
            this.configureDownloadComputers.Name = "configureDownloadComputers";
            this.configureDownloadComputers.Size = new System.Drawing.Size(148, 13);
            this.configureDownloadComputers.TabIndex = 5;
            this.configureDownloadComputers.Text = "Configure other computers...";
            this.configureDownloadComputers.Click += new System.EventHandler(this.OnConfigureDownloadPolicy);
            // 
            // sectionAutoDownloads
            // 
            this.sectionAutoDownloads.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionAutoDownloads.Location = new System.Drawing.Point(15, 15);
            this.sectionAutoDownloads.Name = "sectionAutoDownloads";
            this.sectionAutoDownloads.Size = new System.Drawing.Size(530, 22);
            this.sectionAutoDownloads.TabIndex = 3;
            this.sectionAutoDownloads.Text = "Downloading";
            // 
            // sectionTitleManualOrders
            // 
            this.sectionTitleManualOrders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionTitleManualOrders.Location = new System.Drawing.Point(15, 129);
            this.sectionTitleManualOrders.Name = "sectionTitleManualOrders";
            this.sectionTitleManualOrders.Size = new System.Drawing.Size(530, 22);
            this.sectionTitleManualOrders.TabIndex = 2;
            this.sectionTitleManualOrders.Text = "Manual Orders";
            // 
            // automaticDownloadControl
            // 
            this.automaticDownloadControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.automaticDownloadControl.Location = new System.Drawing.Point(32, 72);
            this.automaticDownloadControl.Name = "automaticDownloadControl";
            this.automaticDownloadControl.Size = new System.Drawing.Size(292, 50);
            this.automaticDownloadControl.TabIndex = 1;
            // 
            // optionPageOnlineAccount
            // 
            this.optionPageOnlineAccount.BackColor = System.Drawing.Color.White;
            this.optionPageOnlineAccount.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageOnlineAccount.Location = new System.Drawing.Point(153, 0);
            this.optionPageOnlineAccount.Name = "optionPageOnlineAccount";
            this.optionPageOnlineAccount.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageOnlineAccount.Size = new System.Drawing.Size(600, 466);
            this.optionPageOnlineAccount.TabIndex = 2;
            this.optionPageOnlineAccount.Text = "Store Connection";
            // 
            // optionPageStatusPreset
            // 
            this.optionPageStatusPreset.Controls.Add(this.tabControl1);
            this.optionPageStatusPreset.Location = new System.Drawing.Point(153, 0);
            this.optionPageStatusPreset.Name = "optionPageStatusPreset";
            this.optionPageStatusPreset.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageStatusPreset.Size = new System.Drawing.Size(600, 466);
            this.optionPageStatusPreset.TabIndex = 4;
            this.optionPageStatusPreset.Text = "Status Presets";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 460);
            this.tabControl1.TabIndex = 29;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.orderStatusPresets);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(586, 434);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Order Status";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox1.Location = new System.Drawing.Point(8, 404);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(61, 406);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(356, 13);
            this.label6.TabIndex = 35;
            this.label6.Text = "Editing or deleting a status preset will not affect existing orders or items.";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(25, 406);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 34;
            this.label5.Text = "Note:";
            // 
            // orderStatusPresets
            // 
            this.orderStatusPresets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.orderStatusPresets.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.orderStatusPresets.Location = new System.Drawing.Point(1, 1);
            this.orderStatusPresets.Name = "orderStatusPresets";
            this.orderStatusPresets.Size = new System.Drawing.Size(407, 404);
            this.orderStatusPresets.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.itemStatusPresets);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(586, 434);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Item Status";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox2.Image = global::ShipWorks.Properties.Resources.information16;
            this.pictureBox2.Location = new System.Drawing.Point(8, 404);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 33;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 406);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(356, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Editing or deleting a status preset will not affect existing orders or items.";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 406);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Note:";
            // 
            // itemStatusPresets
            // 
            this.itemStatusPresets.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.itemStatusPresets.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.itemStatusPresets.Location = new System.Drawing.Point(1, 1);
            this.itemStatusPresets.Name = "itemStatusPresets";
            this.itemStatusPresets.Size = new System.Drawing.Size(407, 404);
            this.itemStatusPresets.TabIndex = 1;
            // 
            // optionPageLicense
            // 
            this.optionPageLicense.BackColor = System.Drawing.Color.White;
            this.optionPageLicense.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.optionPageLicense.Controls.Add(this.licenseStatus);
            this.optionPageLicense.Controls.Add(this.labelLicense);
            this.optionPageLicense.Controls.Add(this.labelStatus);
            this.optionPageLicense.Controls.Add(this.licenseKey);
            this.optionPageLicense.Controls.Add(this.changeLicense);
            this.optionPageLicense.Location = new System.Drawing.Point(153, 0);
            this.optionPageLicense.Name = "optionPageLicense";
            this.optionPageLicense.Padding = new System.Windows.Forms.Padding(3);
            this.optionPageLicense.Size = new System.Drawing.Size(600, 466);
            this.optionPageLicense.TabIndex = 5;
            this.optionPageLicense.Text = "License";
            // 
            // licenseStatus
            // 
            this.licenseStatus.AutoSize = true;
            this.licenseStatus.Location = new System.Drawing.Point(62, 39);
            this.licenseStatus.Name = "licenseStatus";
            this.licenseStatus.Size = new System.Drawing.Size(38, 13);
            this.licenseStatus.TabIndex = 15;
            this.licenseStatus.Text = "Status";
            // 
            // labelLicense
            // 
            this.labelLicense.AutoSize = true;
            this.labelLicense.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLicense.Location = new System.Drawing.Point(6, 14);
            this.labelLicense.Name = "labelLicense";
            this.labelLicense.Size = new System.Drawing.Size(52, 13);
            this.labelLicense.TabIndex = 8;
            this.labelLicense.Text = "License:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(10, 38);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(47, 13);
            this.labelStatus.TabIndex = 14;
            this.labelStatus.Text = "Status:";
            // 
            // licenseKey
            // 
            this.licenseKey.BackColor = System.Drawing.Color.White;
            this.licenseKey.Location = new System.Drawing.Point(64, 12);
            this.licenseKey.Name = "licenseKey";
            this.licenseKey.ReadOnly = true;
            this.licenseKey.Size = new System.Drawing.Size(398, 21);
            this.licenseKey.TabIndex = 9;
            // 
            // changeLicense
            // 
            this.changeLicense.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.changeLicense.Location = new System.Drawing.Point(345, 66);
            this.changeLicense.Name = "changeLicense";
            this.changeLicense.Size = new System.Drawing.Size(117, 23);
            this.changeLicense.TabIndex = 13;
            this.changeLicense.Text = "Change Activation...";
            this.changeLicense.Click += new System.EventHandler(this.OnChangeLicense);
            // 
            // StoreSettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(779, 556);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.titleStoreType);
            this.Controls.Add(this.titleStoreName);
            this.Controls.Add(this.imageStore);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(795, 4594);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(795, 594);
            this.Name = "StoreSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Store Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.imageStore)).EndInit();
            this.optionControl.ResumeLayout(false);
            this.optionPageStoreDetails.ResumeLayout(false);
            this.optionPageSettings.ResumeLayout(false);
            this.optionPageSettings.PerformLayout();
            this.panelDefaultFilters.ResumeLayout(false);
            this.panelDefaultFilters.PerformLayout();
            this.panelAddressValidation.ResumeLayout(false);
            this.panelAddressValidation.PerformLayout();
            this.panelStoreStatus.ResumeLayout(false);
            this.panelStoreStatus.PerformLayout();
            this.optionPageStatusPreset.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.optionPageLicense.ResumeLayout(false);
            this.optionPageLicense.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button changeLicense;
        private System.Windows.Forms.TextBox licenseKey;
        private System.Windows.Forms.Label labelLicense;
        private System.Windows.Forms.Label titleStoreType;
        private System.Windows.Forms.Label titleStoreName;
        private System.Windows.Forms.PictureBox imageStore;
        private StoreContactControl storeContactControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private StatusPresetEditor orderStatusPresets;
        private StatusPresetEditor itemStatusPresets;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPageStoreDetails;
        private ShipWorks.UI.Controls.OptionPage optionPageOnlineAccount;
        private ShipWorks.UI.Controls.OptionPage optionPageStatusPreset;
        private ShipWorks.UI.Controls.OptionPage optionPageLicense;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label licenseStatus;
        private ShipWorks.UI.Controls.OptionPage optionPageSettings;
        private ShipWorks.Stores.Management.AutomaticDownloadControl automaticDownloadControl;
        private ShipWorks.UI.Controls.SectionTitle sectionTitleManualOrders;
        private UI.Controls.SectionTitle sectionAutoDownloads;
        private UI.Controls.SectionTitle sectionStatus;
        private UI.Controls.SectionTitle sectionAddressValidation;
        private System.Windows.Forms.Label label4;
        private UI.Controls.InfoTip infotipStoreEnabled;
        private System.Windows.Forms.CheckBox storeDisabled;
        private UI.Controls.LinkControl configureDownloadComputers;
        private System.Windows.Forms.Panel panelStoreStatus;
        private System.Windows.Forms.Label labelAllowDownload;
        private ComputerDownloadAllowedComboBox comboAllowDownload;
        private StoreAddressControl storeAddressControl;
        private System.Windows.Forms.Panel panelAddressValidation;
        private System.Windows.Forms.ComboBox addressValidationSetting;
        private System.Windows.Forms.Label labelAddressValidationSetting;
        private System.Windows.Forms.Panel panelDefaultFilters;
        private System.Windows.Forms.Button CreateDefaultFilters;
        private System.Windows.Forms.Label label3;
        private UI.Controls.SectionTitle sectionTitle2;
    }
}