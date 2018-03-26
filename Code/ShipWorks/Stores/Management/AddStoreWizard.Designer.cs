namespace ShipWorks.Stores.Management
{
    public partial class AddStoreWizard
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
                components?.Dispose();
                scope?.Dispose();
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
            this.wizardPageStoreType = new ShipWorks.UI.Wizard.WizardPage();
            this.skipPanel = new System.Windows.Forms.Panel();
            this.skipButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelStoreTypeHelp = new System.Windows.Forms.Label();
            this.pictureShoppingCart = new System.Windows.Forms.PictureBox();
            this.comboStoreType = new ShipWorks.UI.Controls.ImageComboBox();
            this.wizardPageContactInfo = new ShipWorks.UI.Wizard.WizardPage();
            this.storeContactControl = new ShipWorks.Stores.Management.StoreContactControl();
            this.wizardPageFinished = new ShipWorks.Stores.Management.AddStoreWizardFinishPage();
            this.wizardPageAlreadyActive = new ShipWorks.UI.Wizard.WizardPage();
            this.label3 = new System.Windows.Forms.Label();
            this.linkLicenseKeyHelp = new ShipWorks.UI.Controls.LinkControl();
            this.labelLicenseKeyHelp = new System.Windows.Forms.Label();
            this.pictureBoxLicense = new System.Windows.Forms.PictureBox();
            this.licenseKey = new System.Windows.Forms.TextBox();
            this.labelLicense = new System.Windows.Forms.Label();
            this.labelWelcomeBack = new System.Windows.Forms.Label();
            this.wizardPageSettings = new ShipWorks.UI.Wizard.WizardPage();
            this.panelSettingsContainer = new System.Windows.Forms.Panel();
            this.panelUploadSettings = new System.Windows.Forms.Panel();
            this.labelShipmentUpdate = new System.Windows.Forms.Label();
            this.panelOnlineUpdatePlaceholder = new System.Windows.Forms.Panel();
            this.pictureBoxShipmentUpdate = new System.Windows.Forms.PictureBox();
            this.panelDownloadSettings = new System.Windows.Forms.Panel();
            this.panelViewDownloadRange = new System.Windows.Forms.Panel();
            this.labelDownloadRange = new System.Windows.Forms.Label();
            this.downloadRange = new System.Windows.Forms.Label();
            this.linkEditDownloadRange = new System.Windows.Forms.LinkLabel();
            this.panelEditDownloadRange = new System.Windows.Forms.Panel();
            this.labelInitialDownloadRange = new System.Windows.Forms.Label();
            this.panelDateRange = new System.Windows.Forms.Panel();
            this.dateRangeRadioHider = new System.Windows.Forms.Panel();
            this.radioDateRangeAll = new System.Windows.Forms.RadioButton();
            this.labelDateRangeDays = new System.Windows.Forms.Label();
            this.dateRangeDays = new System.Windows.Forms.TextBox();
            this.radioDateRangeDays = new System.Windows.Forms.RadioButton();
            this.panelFirstOrder = new System.Windows.Forms.Panel();
            this.radioStartNumberAll = new System.Windows.Forms.RadioButton();
            this.initialDownloadFirstOrder = new System.Windows.Forms.TextBox();
            this.radioStartNumberLimit = new System.Windows.Forms.RadioButton();
            this.pictureBoxDownloadRange = new System.Windows.Forms.PictureBox();
            this.wizardPageAddress = new ShipWorks.UI.Wizard.WizardPage();
            this.storeAddressControl = new ShipWorks.Stores.Management.StoreAddressControl();
            this.wizardPageActivationError = new ShipWorks.Stores.Management.ActivationErrorWizardPage();
            this.ManualStoreHelpLinkLabel = new System.Windows.Forms.LinkLabel();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageStoreType.SuspendLayout();
            this.skipPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureShoppingCart)).BeginInit();
            this.wizardPageContactInfo.SuspendLayout();
            this.wizardPageAlreadyActive.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLicense)).BeginInit();
            this.wizardPageSettings.SuspendLayout();
            this.panelSettingsContainer.SuspendLayout();
            this.panelUploadSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShipmentUpdate)).BeginInit();
            this.panelDownloadSettings.SuspendLayout();
            this.panelViewDownloadRange.SuspendLayout();
            this.panelEditDownloadRange.SuspendLayout();
            this.panelDateRange.SuspendLayout();
            this.panelFirstOrder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownloadRange)).BeginInit();
            this.wizardPageAddress.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(380, 572);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(461, 572);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(299, 572);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageStoreType);
            this.mainPanel.Size = new System.Drawing.Size(548, 500);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 562);
            this.etchBottom.Size = new System.Drawing.Size(552, 2);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.sw_cubes_big;
            this.pictureBox.Location = new System.Drawing.Point(486, 3);
            this.pictureBox.Size = new System.Drawing.Size(54, 50);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            // 
            // topPanel
            // 
            this.topPanel.Size = new System.Drawing.Size(548, 56);
            // 
            // wizardPageStoreType
            // 
            this.wizardPageStoreType.Controls.Add(this.skipPanel);
            this.wizardPageStoreType.Controls.Add(this.label2);
            this.wizardPageStoreType.Controls.Add(this.labelStoreTypeHelp);
            this.wizardPageStoreType.Controls.Add(this.pictureShoppingCart);
            this.wizardPageStoreType.Controls.Add(this.comboStoreType);
            this.wizardPageStoreType.Description = "Configure ShipWorks for your online store.";
            this.wizardPageStoreType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageStoreType.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageStoreType.Location = new System.Drawing.Point(0, 0);
            this.wizardPageStoreType.Name = "wizardPageStoreType";
            this.wizardPageStoreType.Size = new System.Drawing.Size(548, 500);
            this.wizardPageStoreType.TabIndex = 0;
            this.wizardPageStoreType.Title = "Online Store";
            this.wizardPageStoreType.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextStoreType);
            this.wizardPageStoreType.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoStoreType);
            // 
            // skipPanel
            // 
            this.skipPanel.Controls.Add(this.ManualStoreHelpLinkLabel);
            this.skipPanel.Controls.Add(this.skipButton);
            this.skipPanel.Controls.Add(this.label1);
            this.skipPanel.Location = new System.Drawing.Point(84, 97);
            this.skipPanel.Name = "skipPanel";
            this.skipPanel.Size = new System.Drawing.Size(290, 107);
            this.skipPanel.TabIndex = 62;
            // 
            // skipButton
            // 
            this.skipButton.Font = new System.Drawing.Font("Tahoma", 9.25F);
            this.skipButton.Location = new System.Drawing.Point(12, 32);
            this.skipButton.Name = "skipButton";
            this.skipButton.Size = new System.Drawing.Size(138, 51);
            this.skipButton.TabIndex = 63;
            this.skipButton.Text = "Skip this step >>";
            this.skipButton.UseVisualStyleBackColor = true;
            this.skipButton.Click += new System.EventHandler(this.OnSkipButtonClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 16);
            this.label1.TabIndex = 62;
            this.label1.Text = "Not ready to connect directly?";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(93, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 13);
            this.label2.TabIndex = 61;
            this.label2.Text = "What platform do you sell on?";
            // 
            // labelStoreTypeHelp
            // 
            this.labelStoreTypeHelp.AutoSize = true;
            this.labelStoreTypeHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelStoreTypeHelp.Location = new System.Drawing.Point(93, 59);
            this.labelStoreTypeHelp.Name = "labelStoreTypeHelp";
            this.labelStoreTypeHelp.Size = new System.Drawing.Size(281, 13);
            this.labelStoreTypeHelp.TabIndex = 57;
            this.labelStoreTypeHelp.Text = "(If you need to add more than one, it\'s easy to do later.)";
            // 
            // pictureShoppingCart
            // 
            this.pictureShoppingCart.Image = global::ShipWorks.Properties.Resources.shoppingcart;
            this.pictureShoppingCart.Location = new System.Drawing.Point(23, 9);
            this.pictureShoppingCart.Name = "pictureShoppingCart";
            this.pictureShoppingCart.Size = new System.Drawing.Size(48, 48);
            this.pictureShoppingCart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureShoppingCart.TabIndex = 56;
            this.pictureShoppingCart.TabStop = false;
            // 
            // comboStoreType
            // 
            this.comboStoreType.Location = new System.Drawing.Point(96, 33);
            this.comboStoreType.MaxDropDownItems = 45;
            this.comboStoreType.Name = "comboStoreType";
            this.comboStoreType.Size = new System.Drawing.Size(193, 21);
            this.comboStoreType.TabIndex = 2;
            this.comboStoreType.SelectedIndexChanged += new System.EventHandler(this.OnChooseStoreType);
            // 
            // wizardPageContactInfo
            // 
            this.wizardPageContactInfo.Controls.Add(this.storeContactControl);
            this.wizardPageContactInfo.Description = "Enter the contact information for your store.";
            this.wizardPageContactInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageContactInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageContactInfo.Location = new System.Drawing.Point(0, 0);
            this.wizardPageContactInfo.Name = "wizardPageContactInfo";
            this.wizardPageContactInfo.Size = new System.Drawing.Size(548, 500);
            this.wizardPageContactInfo.TabIndex = 0;
            this.wizardPageContactInfo.Title = "Contact Information";
            this.wizardPageContactInfo.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextContactInfo);
            this.wizardPageContactInfo.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoContactInfo);
            // 
            // storeContactControl
            // 
            this.storeContactControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeContactControl.Location = new System.Drawing.Point(23, 3);
            this.storeContactControl.Name = "storeContactControl";
            this.storeContactControl.Size = new System.Drawing.Size(360, 300);
            this.storeContactControl.TabIndex = 0;
            // 
            // wizardPageFinished
            // 
            this.wizardPageFinished.Description = "ShipWorks is ready to connect to your store.";
            this.wizardPageFinished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinished.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinished.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinished.Name = "wizardPageFinished";
            this.wizardPageFinished.Size = new System.Drawing.Size(548, 500);
            this.wizardPageFinished.TabIndex = 0;
            this.wizardPageFinished.Title = "Setup Complete";
            this.wizardPageFinished.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoComplete);
            // 
            // wizardPageAlreadyActive
            // 
            this.wizardPageAlreadyActive.Controls.Add(this.label3);
            this.wizardPageAlreadyActive.Controls.Add(this.linkLicenseKeyHelp);
            this.wizardPageAlreadyActive.Controls.Add(this.labelLicenseKeyHelp);
            this.wizardPageAlreadyActive.Controls.Add(this.pictureBoxLicense);
            this.wizardPageAlreadyActive.Controls.Add(this.licenseKey);
            this.wizardPageAlreadyActive.Controls.Add(this.labelLicense);
            this.wizardPageAlreadyActive.Controls.Add(this.labelWelcomeBack);
            this.wizardPageAlreadyActive.Description = "A ShipWorks account already exists.";
            this.wizardPageAlreadyActive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAlreadyActive.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAlreadyActive.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAlreadyActive.Name = "wizardPageAlreadyActive";
            this.wizardPageAlreadyActive.Size = new System.Drawing.Size(548, 500);
            this.wizardPageAlreadyActive.TabIndex = 0;
            this.wizardPageAlreadyActive.Title = "ShipWorks Account";
            this.wizardPageAlreadyActive.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAlreadyActive);
            this.wizardPageAlreadyActive.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAlreadyActive);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label3.Location = new System.Drawing.Point(354, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 60;
            this.label3.Text = ")";
            // 
            // linkLicenseKeyHelp
            // 
            this.linkLicenseKeyHelp.AutoSize = true;
            this.linkLicenseKeyHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkLicenseKeyHelp.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkLicenseKeyHelp.ForeColor = System.Drawing.Color.Blue;
            this.linkLicenseKeyHelp.Location = new System.Drawing.Point(328, 75);
            this.linkLicenseKeyHelp.Name = "linkLicenseKeyHelp";
            this.linkLicenseKeyHelp.Size = new System.Drawing.Size(29, 13);
            this.linkLicenseKeyHelp.TabIndex = 59;
            this.linkLicenseKeyHelp.Text = "here";
            this.linkLicenseKeyHelp.Click += new System.EventHandler(this.OnHelpFindLicenseKey);
            // 
            // labelLicenseKeyHelp
            // 
            this.labelLicenseKeyHelp.AutoSize = true;
            this.labelLicenseKeyHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelLicenseKeyHelp.Location = new System.Drawing.Point(77, 75);
            this.labelLicenseKeyHelp.Name = "labelLicenseKeyHelp";
            this.labelLicenseKeyHelp.Size = new System.Drawing.Size(253, 13);
            this.labelLicenseKeyHelp.TabIndex = 58;
            this.labelLicenseKeyHelp.Text = "(You can find it in your ShipWorks account online at";
            // 
            // pictureBoxLicense
            // 
            this.pictureBoxLicense.Image = global::ShipWorks.Properties.Resources.key1;
            this.pictureBoxLicense.Location = new System.Drawing.Point(23, 9);
            this.pictureBoxLicense.Name = "pictureBoxLicense";
            this.pictureBoxLicense.Size = new System.Drawing.Size(48, 48);
            this.pictureBoxLicense.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxLicense.TabIndex = 57;
            this.pictureBoxLicense.TabStop = false;
            // 
            // licenseKey
            // 
            this.licenseKey.Location = new System.Drawing.Point(80, 50);
            this.licenseKey.Name = "licenseKey";
            this.licenseKey.Size = new System.Drawing.Size(408, 21);
            this.licenseKey.TabIndex = 6;
            // 
            // labelLicense
            // 
            this.labelLicense.AutoSize = true;
            this.labelLicense.Location = new System.Drawing.Point(77, 32);
            this.labelLicense.Name = "labelLicense";
            this.labelLicense.Size = new System.Drawing.Size(227, 13);
            this.labelLicense.TabIndex = 5;
            this.labelLicense.Text = "Enter your ShipWorks license key to continue:";
            // 
            // labelWelcomeBack
            // 
            this.labelWelcomeBack.AutoSize = true;
            this.labelWelcomeBack.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcomeBack.Location = new System.Drawing.Point(77, 9);
            this.labelWelcomeBack.Name = "labelWelcomeBack";
            this.labelWelcomeBack.Size = new System.Drawing.Size(92, 13);
            this.labelWelcomeBack.TabIndex = 4;
            this.labelWelcomeBack.Text = "Welcome back!";
            // 
            // wizardPageSettings
            // 
            this.wizardPageSettings.Controls.Add(this.panelSettingsContainer);
            this.wizardPageSettings.Description = "Configure how ShipWorks will work with your store.";
            this.wizardPageSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageSettings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageSettings.Location = new System.Drawing.Point(0, 0);
            this.wizardPageSettings.Name = "wizardPageSettings";
            this.wizardPageSettings.Size = new System.Drawing.Size(548, 500);
            this.wizardPageSettings.TabIndex = 0;
            this.wizardPageSettings.Title = "Store Setup";
            this.wizardPageSettings.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSettings);
            this.wizardPageSettings.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoSettings);
            // 
            // panelSettingsContainer
            // 
            this.panelSettingsContainer.Controls.Add(this.panelUploadSettings);
            this.panelSettingsContainer.Controls.Add(this.panelDownloadSettings);
            this.panelSettingsContainer.Location = new System.Drawing.Point(23, 9);
            this.panelSettingsContainer.Name = "panelSettingsContainer";
            this.panelSettingsContainer.Size = new System.Drawing.Size(503, 360);
            this.panelSettingsContainer.TabIndex = 28;
            // 
            // panelUploadSettings
            // 
            this.panelUploadSettings.Controls.Add(this.labelShipmentUpdate);
            this.panelUploadSettings.Controls.Add(this.panelOnlineUpdatePlaceholder);
            this.panelUploadSettings.Controls.Add(this.pictureBoxShipmentUpdate);
            this.panelUploadSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUploadSettings.Location = new System.Drawing.Point(0, 163);
            this.panelUploadSettings.Name = "panelUploadSettings";
            this.panelUploadSettings.Size = new System.Drawing.Size(503, 149);
            this.panelUploadSettings.TabIndex = 1;
            // 
            // labelShipmentUpdate
            // 
            this.labelShipmentUpdate.AutoSize = true;
            this.labelShipmentUpdate.Location = new System.Drawing.Point(31, 9);
            this.labelShipmentUpdate.Name = "labelShipmentUpdate";
            this.labelShipmentUpdate.Size = new System.Drawing.Size(156, 13);
            this.labelShipmentUpdate.TabIndex = 25;
            this.labelShipmentUpdate.Text = "When a shipment is processed:";
            // 
            // panelOnlineUpdatePlaceholder
            // 
            this.panelOnlineUpdatePlaceholder.Location = new System.Drawing.Point(35, 31);
            this.panelOnlineUpdatePlaceholder.Name = "panelOnlineUpdatePlaceholder";
            this.panelOnlineUpdatePlaceholder.Size = new System.Drawing.Size(430, 111);
            this.panelOnlineUpdatePlaceholder.TabIndex = 4;
            // 
            // pictureBoxShipmentUpdate
            // 
            this.pictureBoxShipmentUpdate.Image = global::ShipWorks.Properties.Resources.box_closed2;
            this.pictureBoxShipmentUpdate.Location = new System.Drawing.Point(1, 4);
            this.pictureBoxShipmentUpdate.Name = "pictureBoxShipmentUpdate";
            this.pictureBoxShipmentUpdate.Size = new System.Drawing.Size(24, 24);
            this.pictureBoxShipmentUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxShipmentUpdate.TabIndex = 24;
            this.pictureBoxShipmentUpdate.TabStop = false;
            // 
            // panelDownloadSettings
            // 
            this.panelDownloadSettings.Controls.Add(this.panelViewDownloadRange);
            this.panelDownloadSettings.Controls.Add(this.panelEditDownloadRange);
            this.panelDownloadSettings.Controls.Add(this.pictureBoxDownloadRange);
            this.panelDownloadSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDownloadSettings.Location = new System.Drawing.Point(0, 0);
            this.panelDownloadSettings.Name = "panelDownloadSettings";
            this.panelDownloadSettings.Size = new System.Drawing.Size(503, 163);
            this.panelDownloadSettings.TabIndex = 0;
            // 
            // panelViewDownloadRange
            // 
            this.panelViewDownloadRange.Controls.Add(this.labelDownloadRange);
            this.panelViewDownloadRange.Controls.Add(this.downloadRange);
            this.panelViewDownloadRange.Controls.Add(this.linkEditDownloadRange);
            this.panelViewDownloadRange.Location = new System.Drawing.Point(31, 5);
            this.panelViewDownloadRange.Name = "panelViewDownloadRange";
            this.panelViewDownloadRange.Size = new System.Drawing.Size(482, 22);
            this.panelViewDownloadRange.TabIndex = 27;
            // 
            // labelDownloadRange
            // 
            this.labelDownloadRange.AutoSize = true;
            this.labelDownloadRange.Location = new System.Drawing.Point(0, 0);
            this.labelDownloadRange.Name = "labelDownloadRange";
            this.labelDownloadRange.Size = new System.Drawing.Size(226, 13);
            this.labelDownloadRange.TabIndex = 20;
            this.labelDownloadRange.Text = "ShipWorks will download orders starting from:";
            // 
            // downloadRange
            // 
            this.downloadRange.AutoSize = true;
            this.downloadRange.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downloadRange.Location = new System.Drawing.Point(225, 0);
            this.downloadRange.Name = "downloadRange";
            this.downloadRange.Size = new System.Drawing.Size(75, 13);
            this.downloadRange.TabIndex = 21;
            this.downloadRange.Text = "30 days ago";
            // 
            // linkEditDownloadRange
            // 
            this.linkEditDownloadRange.AutoSize = true;
            this.linkEditDownloadRange.LinkColor = System.Drawing.Color.CornflowerBlue;
            this.linkEditDownloadRange.Location = new System.Drawing.Point(302, 0);
            this.linkEditDownloadRange.Name = "linkEditDownloadRange";
            this.linkEditDownloadRange.Size = new System.Drawing.Size(33, 13);
            this.linkEditDownloadRange.TabIndex = 22;
            this.linkEditDownloadRange.TabStop = true;
            this.linkEditDownloadRange.Text = "(Edit)";
            this.linkEditDownloadRange.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnEditInitialDownloadRange);
            // 
            // panelEditDownloadRange
            // 
            this.panelEditDownloadRange.Controls.Add(this.labelInitialDownloadRange);
            this.panelEditDownloadRange.Controls.Add(this.panelDateRange);
            this.panelEditDownloadRange.Controls.Add(this.panelFirstOrder);
            this.panelEditDownloadRange.Location = new System.Drawing.Point(31, 33);
            this.panelEditDownloadRange.Name = "panelEditDownloadRange";
            this.panelEditDownloadRange.Size = new System.Drawing.Size(431, 126);
            this.panelEditDownloadRange.TabIndex = 26;
            this.panelEditDownloadRange.Visible = false;
            // 
            // labelInitialDownloadRange
            // 
            this.labelInitialDownloadRange.AutoSize = true;
            this.labelInitialDownloadRange.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInitialDownloadRange.Location = new System.Drawing.Point(0, 0);
            this.labelInitialDownloadRange.Name = "labelInitialDownloadRange";
            this.labelInitialDownloadRange.Size = new System.Drawing.Size(226, 13);
            this.labelInitialDownloadRange.TabIndex = 15;
            this.labelInitialDownloadRange.Text = "ShipWorks will download orders starting from:";
            // 
            // panelDateRange
            // 
            this.panelDateRange.Controls.Add(this.dateRangeRadioHider);
            this.panelDateRange.Controls.Add(this.radioDateRangeAll);
            this.panelDateRange.Controls.Add(this.labelDateRangeDays);
            this.panelDateRange.Controls.Add(this.dateRangeDays);
            this.panelDateRange.Controls.Add(this.radioDateRangeDays);
            this.panelDateRange.Location = new System.Drawing.Point(3, 19);
            this.panelDateRange.Name = "panelDateRange";
            this.panelDateRange.Size = new System.Drawing.Size(415, 50);
            this.panelDateRange.TabIndex = 0;
            // 
            // dateRangeRadioHider
            // 
            this.dateRangeRadioHider.Location = new System.Drawing.Point(7, 3);
            this.dateRangeRadioHider.Name = "dateRangeRadioHider";
            this.dateRangeRadioHider.Size = new System.Drawing.Size(30, 18);
            this.dateRangeRadioHider.TabIndex = 9;
            this.dateRangeRadioHider.Visible = false;
            // 
            // radioDateRangeAll
            // 
            this.radioDateRangeAll.AutoSize = true;
            this.radioDateRangeAll.Location = new System.Drawing.Point(20, 29);
            this.radioDateRangeAll.Name = "radioDateRangeAll";
            this.radioDateRangeAll.Size = new System.Drawing.Size(153, 17);
            this.radioDateRangeAll.TabIndex = 2;
            this.radioDateRangeAll.TabStop = true;
            this.radioDateRangeAll.Text = "Download all of my orders.";
            this.radioDateRangeAll.UseVisualStyleBackColor = true;
            this.radioDateRangeAll.CheckedChanged += new System.EventHandler(this.OnChangeInitialDownloadOption);
            // 
            // labelDateRangeDays
            // 
            this.labelDateRangeDays.AutoSize = true;
            this.labelDateRangeDays.Location = new System.Drawing.Point(96, 5);
            this.labelDateRangeDays.Name = "labelDateRangeDays";
            this.labelDateRangeDays.Size = new System.Drawing.Size(55, 13);
            this.labelDateRangeDays.TabIndex = 6;
            this.labelDateRangeDays.Text = "days ago.";
            // 
            // dateRangeDays
            // 
            this.dateRangeDays.Location = new System.Drawing.Point(39, 2);
            this.dateRangeDays.Name = "dateRangeDays";
            this.dateRangeDays.Size = new System.Drawing.Size(55, 21);
            this.dateRangeDays.TabIndex = 1;
            // 
            // radioDateRangeDays
            // 
            this.radioDateRangeDays.AutoSize = true;
            this.radioDateRangeDays.Checked = true;
            this.radioDateRangeDays.Location = new System.Drawing.Point(20, 3);
            this.radioDateRangeDays.Name = "radioDateRangeDays";
            this.radioDateRangeDays.Size = new System.Drawing.Size(14, 13);
            this.radioDateRangeDays.TabIndex = 0;
            this.radioDateRangeDays.TabStop = true;
            this.radioDateRangeDays.UseVisualStyleBackColor = true;
            this.radioDateRangeDays.CheckedChanged += new System.EventHandler(this.OnChangeInitialDownloadOption);
            // 
            // panelFirstOrder
            // 
            this.panelFirstOrder.Controls.Add(this.radioStartNumberAll);
            this.panelFirstOrder.Controls.Add(this.initialDownloadFirstOrder);
            this.panelFirstOrder.Controls.Add(this.radioStartNumberLimit);
            this.panelFirstOrder.Location = new System.Drawing.Point(3, 71);
            this.panelFirstOrder.Name = "panelFirstOrder";
            this.panelFirstOrder.Size = new System.Drawing.Size(415, 50);
            this.panelFirstOrder.TabIndex = 1;
            this.panelFirstOrder.Visible = false;
            // 
            // radioStartNumberAll
            // 
            this.radioStartNumberAll.AutoSize = true;
            this.radioStartNumberAll.Location = new System.Drawing.Point(20, 30);
            this.radioStartNumberAll.Name = "radioStartNumberAll";
            this.radioStartNumberAll.Size = new System.Drawing.Size(153, 17);
            this.radioStartNumberAll.TabIndex = 12;
            this.radioStartNumberAll.TabStop = true;
            this.radioStartNumberAll.Text = "Download all of my orders.";
            this.radioStartNumberAll.UseVisualStyleBackColor = true;
            this.radioStartNumberAll.CheckedChanged += new System.EventHandler(this.OnChangeInitialDownloadOption);
            // 
            // initialDownloadFirstOrder
            // 
            this.initialDownloadFirstOrder.Location = new System.Drawing.Point(82, 3);
            this.initialDownloadFirstOrder.Name = "initialDownloadFirstOrder";
            this.initialDownloadFirstOrder.Size = new System.Drawing.Size(55, 21);
            this.initialDownloadFirstOrder.TabIndex = 1;
            // 
            // radioStartNumberLimit
            // 
            this.radioStartNumberLimit.AutoSize = true;
            this.radioStartNumberLimit.Checked = true;
            this.radioStartNumberLimit.Location = new System.Drawing.Point(20, 4);
            this.radioStartNumberLimit.Name = "radioStartNumberLimit";
            this.radioStartNumberLimit.Size = new System.Drawing.Size(64, 17);
            this.radioStartNumberLimit.TabIndex = 0;
            this.radioStartNumberLimit.TabStop = true;
            this.radioStartNumberLimit.Text = "Order #";
            this.radioStartNumberLimit.UseVisualStyleBackColor = true;
            this.radioStartNumberLimit.CheckedChanged += new System.EventHandler(this.OnChangeInitialDownloadOption);
            // 
            // pictureBoxDownloadRange
            // 
            this.pictureBoxDownloadRange.Image = global::ShipWorks.Properties.Resources.nav_down_green1;
            this.pictureBoxDownloadRange.Location = new System.Drawing.Point(2, 0);
            this.pictureBoxDownloadRange.Name = "pictureBoxDownloadRange";
            this.pictureBoxDownloadRange.Size = new System.Drawing.Size(24, 24);
            this.pictureBoxDownloadRange.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxDownloadRange.TabIndex = 23;
            this.pictureBoxDownloadRange.TabStop = false;
            // 
            // wizardPageAddress
            // 
            this.wizardPageAddress.Controls.Add(this.storeAddressControl);
            this.wizardPageAddress.Description = "Enter the name and physical address of your online store.";
            this.wizardPageAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageAddress.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageAddress.Location = new System.Drawing.Point(0, 0);
            this.wizardPageAddress.Name = "wizardPageAddress";
            this.wizardPageAddress.Size = new System.Drawing.Size(548, 500);
            this.wizardPageAddress.TabIndex = 0;
            this.wizardPageAddress.Title = "Store Information";
            this.wizardPageAddress.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextAddress);
            this.wizardPageAddress.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoAddress);
            // 
            // storeAddressControl
            // 
            this.storeAddressControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.storeAddressControl.Location = new System.Drawing.Point(23, 3);
            this.storeAddressControl.Name = "storeAddressControl";
            this.storeAddressControl.Size = new System.Drawing.Size(360, 300);
            this.storeAddressControl.TabIndex = 0;
            // 
            // wizardPageActivationError
            // 
            this.wizardPageActivationError.Description = "Enter the following information about your online store.";
            this.wizardPageActivationError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageActivationError.ErrorMessage = "An error has occurred";
            this.wizardPageActivationError.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageActivationError.Location = new System.Drawing.Point(0, 0);
            this.wizardPageActivationError.Name = "wizardPageActivationError";
            this.wizardPageActivationError.Size = new System.Drawing.Size(548, 500);
            this.wizardPageActivationError.TabIndex = 0;
            this.wizardPageActivationError.Title = "Store Setup";
            this.wizardPageActivationError.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoWizardPageActivationError);
            // 
            // ManualStoreHelpLinkLabel
            // 
            this.ManualStoreHelpLinkLabel.AutoSize = true;
            this.ManualStoreHelpLinkLabel.Location = new System.Drawing.Point(12, 91);
            this.ManualStoreHelpLinkLabel.Name = "ManualStoreHelpLinkLabel";
            this.ManualStoreHelpLinkLabel.Size = new System.Drawing.Size(118, 13);
            this.ManualStoreHelpLinkLabel.TabIndex = 64;
            this.ManualStoreHelpLinkLabel.TabStop = true;
            this.ManualStoreHelpLinkLabel.Text = "Click here for more info";
            this.ManualStoreHelpLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ManualStoreHelpLinkLabel_LinkClicked);
            // 
            // AddStoreWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 607);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(564, 1000);
            this.MinimumSize = new System.Drawing.Size(564, 452);
            this.Name = "AddStoreWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPageStoreType,
            this.wizardPageAlreadyActive,
            this.wizardPageAddress,
            this.wizardPageContactInfo,
            this.wizardPageSettings,
            this.wizardPageActivationError,
            this.wizardPageFinished});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Setup";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageStoreType.ResumeLayout(false);
            this.wizardPageStoreType.PerformLayout();
            this.skipPanel.ResumeLayout(false);
            this.skipPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureShoppingCart)).EndInit();
            this.wizardPageContactInfo.ResumeLayout(false);
            this.wizardPageAlreadyActive.ResumeLayout(false);
            this.wizardPageAlreadyActive.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLicense)).EndInit();
            this.wizardPageSettings.ResumeLayout(false);
            this.panelSettingsContainer.ResumeLayout(false);
            this.panelUploadSettings.ResumeLayout(false);
            this.panelUploadSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShipmentUpdate)).EndInit();
            this.panelDownloadSettings.ResumeLayout(false);
            this.panelDownloadSettings.PerformLayout();
            this.panelViewDownloadRange.ResumeLayout(false);
            this.panelViewDownloadRange.PerformLayout();
            this.panelEditDownloadRange.ResumeLayout(false);
            this.panelEditDownloadRange.PerformLayout();
            this.panelDateRange.ResumeLayout(false);
            this.panelDateRange.PerformLayout();
            this.panelFirstOrder.ResumeLayout(false);
            this.panelFirstOrder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDownloadRange)).EndInit();
            this.wizardPageAddress.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPageStoreType;
        private ShipWorks.UI.Controls.ImageComboBox comboStoreType;
        private ShipWorks.UI.Wizard.WizardPage wizardPageContactInfo;
        private AddStoreWizardFinishPage wizardPageFinished;
        private ShipWorks.UI.Wizard.WizardPage wizardPageAlreadyActive;
        private System.Windows.Forms.TextBox licenseKey;
        private System.Windows.Forms.Label labelLicense;
        private System.Windows.Forms.Label labelWelcomeBack;
        private ShipWorks.UI.Wizard.WizardPage wizardPageSettings;
        private System.Windows.Forms.Panel panelOnlineUpdatePlaceholder;
        private StoreContactControl storeContactControl;
        private UI.Wizard.WizardPage wizardPageAddress;
        private StoreAddressControl storeAddressControl;
        private System.Windows.Forms.Label labelStoreTypeHelp;
        private System.Windows.Forms.PictureBox pictureShoppingCart;
        private System.Windows.Forms.PictureBox pictureBoxLicense;
        private System.Windows.Forms.Label label3;
        private UI.Controls.LinkControl linkLicenseKeyHelp;
        private System.Windows.Forms.Label labelLicenseKeyHelp;
        private System.Windows.Forms.Label labelShipmentUpdate;
        private System.Windows.Forms.PictureBox pictureBoxShipmentUpdate;
        private System.Windows.Forms.PictureBox pictureBoxDownloadRange;
        private System.Windows.Forms.LinkLabel linkEditDownloadRange;
        private System.Windows.Forms.Label downloadRange;
        private System.Windows.Forms.Label labelDownloadRange;
        private System.Windows.Forms.Panel panelEditDownloadRange;
        private System.Windows.Forms.Label labelInitialDownloadRange;
        private System.Windows.Forms.Panel panelDateRange;
        private System.Windows.Forms.Panel dateRangeRadioHider;
        private System.Windows.Forms.RadioButton radioDateRangeAll;
        private System.Windows.Forms.Label labelDateRangeDays;
        private System.Windows.Forms.TextBox dateRangeDays;
        private System.Windows.Forms.RadioButton radioDateRangeDays;
        private System.Windows.Forms.Panel panelFirstOrder;
        private System.Windows.Forms.RadioButton radioStartNumberAll;
        private System.Windows.Forms.TextBox initialDownloadFirstOrder;
        private System.Windows.Forms.RadioButton radioStartNumberLimit;
        private System.Windows.Forms.Panel panelViewDownloadRange;
        private System.Windows.Forms.Panel panelSettingsContainer;
        private System.Windows.Forms.Panel panelUploadSettings;
        private System.Windows.Forms.Panel panelDownloadSettings;
        private System.Windows.Forms.Label label2;        
        private ActivationErrorWizardPage wizardPageActivationError;
        private System.Windows.Forms.Panel skipPanel;
        private System.Windows.Forms.Button skipButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel ManualStoreHelpLinkLabel;
    }
}
