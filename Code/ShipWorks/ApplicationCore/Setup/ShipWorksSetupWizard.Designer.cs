namespace ShipWorks.ApplicationCore.Setup
{
    partial class ShipWorksSetupWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShipWorksSetupWizard));
            this.wizardPageOnlineStore = new ShipWorks.UI.Wizard.WizardPage();
            this.labelSampleOrderHelp = new System.Windows.Forms.Label();
            this.radioStoreSamples = new System.Windows.Forms.RadioButton();
            this.radioStoreConnect = new System.Windows.Forms.RadioButton();
            this.labelStoreTypeHelp = new System.Windows.Forms.Label();
            this.comboStoreType = new ShipWorks.UI.Controls.ImageComboBox();
            this.pictureShoppingCart = new System.Windows.Forms.PictureBox();
            this.wizardPagePrinters = new ShipWorks.UI.Wizard.WizardPage();
            this.labelStandardPrinterHelp = new System.Windows.Forms.Label();
            this.pictureStandardPrinterHelp = new System.Windows.Forms.PictureBox();
            this.standardPrinter = new ShipWorks.Templates.Media.PrinterSelectionControl();
            this.labelStandardPrinter = new System.Windows.Forms.Label();
            this.pictureStandardPrinter = new System.Windows.Forms.PictureBox();
            this.labelPrinter = new ShipWorks.Templates.Media.PrinterSelectionControl();
            this.labelLabelPrinter = new System.Windows.Forms.Label();
            this.pictureLabelPrinter = new System.Windows.Forms.PictureBox();
            this.printerTypeControl = new ShipWorks.Common.IO.Hardware.Printers.PrinterTypeControl();
            this.wizardPageShipping = new ShipWorks.UI.Wizard.WizardPage();
            this.labelShippingHelp = new System.Windows.Forms.Label();
            this.comboShippingCarrier = new ShipWorks.UI.Controls.ImageComboBox();
            this.labelSelectCarriers = new System.Windows.Forms.Label();
            this.pictureTruck = new System.Windows.Forms.PictureBox();
            this.wizardPagePackingSlip = new ShipWorks.UI.Wizard.WizardPage();
            this.picturePackingSlip = new System.Windows.Forms.PictureBox();
            this.includePackingSlip = new System.Windows.Forms.CheckBox();
            this.labelPackingSlip = new System.Windows.Forms.Label();
            this.labelPicturePackingSlip = new System.Windows.Forms.PictureBox();
            this.wizardPage2 = new ShipWorks.UI.Wizard.WizardPage();
            this.linkControl2 = new ShipWorks.UI.Controls.LinkControl();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.linkControl1 = new ShipWorks.UI.Controls.LinkControl();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageOnlineStore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureShoppingCart)).BeginInit();
            this.wizardPagePrinters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinterHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLabelPrinter)).BeginInit();
            this.wizardPageShipping.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTruck)).BeginInit();
            this.wizardPagePackingSlip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePackingSlip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelPicturePackingSlip)).BeginInit();
            this.wizardPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(380, 343);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(461, 343);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(299, 343);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPageShipping);
            this.mainPanel.Size = new System.Drawing.Size(548, 271);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 333);
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
            // wizardPageOnlineStore
            // 
            this.wizardPageOnlineStore.Controls.Add(this.labelSampleOrderHelp);
            this.wizardPageOnlineStore.Controls.Add(this.radioStoreSamples);
            this.wizardPageOnlineStore.Controls.Add(this.radioStoreConnect);
            this.wizardPageOnlineStore.Controls.Add(this.labelStoreTypeHelp);
            this.wizardPageOnlineStore.Controls.Add(this.comboStoreType);
            this.wizardPageOnlineStore.Controls.Add(this.pictureShoppingCart);
            this.wizardPageOnlineStore.Description = "Configure ShipWorks for your online store.";
            this.wizardPageOnlineStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOnlineStore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOnlineStore.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOnlineStore.Name = "wizardPageOnlineStore";
            this.wizardPageOnlineStore.Size = new System.Drawing.Size(548, 271);
            this.wizardPageOnlineStore.TabIndex = 0;
            this.wizardPageOnlineStore.Title = "Online Store";
            // 
            // labelSampleOrderHelp
            // 
            this.labelSampleOrderHelp.AutoSize = true;
            this.labelSampleOrderHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelSampleOrderHelp.Location = new System.Drawing.Point(96, 104);
            this.labelSampleOrderHelp.Name = "labelSampleOrderHelp";
            this.labelSampleOrderHelp.Size = new System.Drawing.Size(217, 13);
            this.labelSampleOrderHelp.TabIndex = 55;
            this.labelSampleOrderHelp.Text = "(Play around now, and get connected later)";
            // 
            // radioStoreSamples
            // 
            this.radioStoreSamples.AutoSize = true;
            this.radioStoreSamples.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioStoreSamples.Location = new System.Drawing.Point(78, 82);
            this.radioStoreSamples.Name = "radioStoreSamples";
            this.radioStoreSamples.Size = new System.Drawing.Size(252, 17);
            this.radioStoreSamples.TabIndex = 53;
            this.radioStoreSamples.Text = "Just create some sample orders for now";
            this.radioStoreSamples.UseVisualStyleBackColor = true;
            this.radioStoreSamples.Click += new System.EventHandler(this.OnChangeStoreOption);
            // 
            // radioStoreConnect
            // 
            this.radioStoreConnect.AutoSize = true;
            this.radioStoreConnect.Checked = true;
            this.radioStoreConnect.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioStoreConnect.Location = new System.Drawing.Point(78, 9);
            this.radioStoreConnect.Name = "radioStoreConnect";
            this.radioStoreConnect.Size = new System.Drawing.Size(213, 17);
            this.radioStoreConnect.TabIndex = 52;
            this.radioStoreConnect.TabStop = true;
            this.radioStoreConnect.Text = "Get connected to my online store";
            this.radioStoreConnect.UseVisualStyleBackColor = true;
            this.radioStoreConnect.Click += new System.EventHandler(this.OnChangeStoreOption);
            // 
            // labelStoreTypeHelp
            // 
            this.labelStoreTypeHelp.AutoSize = true;
            this.labelStoreTypeHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelStoreTypeHelp.Location = new System.Drawing.Point(95, 56);
            this.labelStoreTypeHelp.Name = "labelStoreTypeHelp";
            this.labelStoreTypeHelp.Size = new System.Drawing.Size(344, 13);
            this.labelStoreTypeHelp.TabIndex = 49;
            this.labelStoreTypeHelp.Text = "(If you have multiple stores just pick one. It\'s easy to add more later.)";
            // 
            // comboStoreType
            // 
            this.comboStoreType.FormattingEnabled = true;
            this.comboStoreType.Items.AddRange(new object[] {
            "US Postal Service",
            "FedEx",
            "UPS",
            "Something else..."});
            this.comboStoreType.Location = new System.Drawing.Point(97, 30);
            this.comboStoreType.MaxDropDownItems = 20;
            this.comboStoreType.Name = "comboStoreType";
            this.comboStoreType.Size = new System.Drawing.Size(223, 21);
            this.comboStoreType.TabIndex = 48;
            // 
            // pictureShoppingCart
            // 
            this.pictureShoppingCart.Image = global::ShipWorks.Properties.Resources.shoppingcart;
            this.pictureShoppingCart.Location = new System.Drawing.Point(23, 9);
            this.pictureShoppingCart.Name = "pictureShoppingCart";
            this.pictureShoppingCart.Size = new System.Drawing.Size(48, 48);
            this.pictureShoppingCart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureShoppingCart.TabIndex = 46;
            this.pictureShoppingCart.TabStop = false;
            // 
            // wizardPagePrinters
            // 
            this.wizardPagePrinters.Controls.Add(this.labelStandardPrinterHelp);
            this.wizardPagePrinters.Controls.Add(this.pictureStandardPrinterHelp);
            this.wizardPagePrinters.Controls.Add(this.standardPrinter);
            this.wizardPagePrinters.Controls.Add(this.labelStandardPrinter);
            this.wizardPagePrinters.Controls.Add(this.pictureStandardPrinter);
            this.wizardPagePrinters.Controls.Add(this.labelPrinter);
            this.wizardPagePrinters.Controls.Add(this.labelLabelPrinter);
            this.wizardPagePrinters.Controls.Add(this.pictureLabelPrinter);
            this.wizardPagePrinters.Controls.Add(this.printerTypeControl);
            this.wizardPagePrinters.Description = "Select the printers that you will be using.";
            this.wizardPagePrinters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePrinters.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePrinters.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePrinters.Name = "wizardPagePrinters";
            this.wizardPagePrinters.Size = new System.Drawing.Size(548, 271);
            this.wizardPagePrinters.TabIndex = 0;
            this.wizardPagePrinters.Title = "Printer Selection";
            this.wizardPagePrinters.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextSelectPrinters);
            this.wizardPagePrinters.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoSelectPrinter);
            // 
            // labelStandardPrinterHelp
            // 
            this.labelStandardPrinterHelp.AutoSize = true;
            this.labelStandardPrinterHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelStandardPrinterHelp.Location = new System.Drawing.Point(102, 60);
            this.labelStandardPrinterHelp.Name = "labelStandardPrinterHelp";
            this.labelStandardPrinterHelp.Size = new System.Drawing.Size(301, 13);
            this.labelStandardPrinterHelp.TabIndex = 70;
            this.labelStandardPrinterHelp.Text = "Select an inkjet or laser printer that uses regular sized paper.";
            // 
            // pictureStandardPrinterHelp
            // 
            this.pictureStandardPrinterHelp.Image = ((System.Drawing.Image)(resources.GetObject("pictureStandardPrinterHelp.Image")));
            this.pictureStandardPrinterHelp.Location = new System.Drawing.Point(82, 58);
            this.pictureStandardPrinterHelp.Name = "pictureStandardPrinterHelp";
            this.pictureStandardPrinterHelp.Size = new System.Drawing.Size(16, 16);
            this.pictureStandardPrinterHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureStandardPrinterHelp.TabIndex = 69;
            this.pictureStandardPrinterHelp.TabStop = false;
            // 
            // standardPrinter
            // 
            this.standardPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.standardPrinter.Location = new System.Drawing.Point(80, 29);
            this.standardPrinter.Name = "standardPrinter";
            this.standardPrinter.ShowLabels = false;
            this.standardPrinter.ShowPaperSource = false;
            this.standardPrinter.Size = new System.Drawing.Size(280, 28);
            this.standardPrinter.TabIndex = 68;
            // 
            // labelStandardPrinter
            // 
            this.labelStandardPrinter.AutoSize = true;
            this.labelStandardPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStandardPrinter.Location = new System.Drawing.Point(78, 9);
            this.labelStandardPrinter.Name = "labelStandardPrinter";
            this.labelStandardPrinter.Size = new System.Drawing.Size(396, 13);
            this.labelStandardPrinter.TabIndex = 67;
            this.labelStandardPrinter.Text = "What printer should invoices, reports, and other documents print on?";
            // 
            // pictureStandardPrinter
            // 
            this.pictureStandardPrinter.Image = global::ShipWorks.Properties.Resources.document1;
            this.pictureStandardPrinter.Location = new System.Drawing.Point(24, 9);
            this.pictureStandardPrinter.Name = "pictureStandardPrinter";
            this.pictureStandardPrinter.Size = new System.Drawing.Size(48, 48);
            this.pictureStandardPrinter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureStandardPrinter.TabIndex = 66;
            this.pictureStandardPrinter.TabStop = false;
            // 
            // labelPrinter
            // 
            this.labelPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrinter.Location = new System.Drawing.Point(80, 113);
            this.labelPrinter.Name = "labelPrinter";
            this.labelPrinter.ShowLabels = false;
            this.labelPrinter.ShowPaperSource = false;
            this.labelPrinter.Size = new System.Drawing.Size(280, 28);
            this.labelPrinter.TabIndex = 62;
            this.labelPrinter.PrinterChanged += new System.EventHandler(this.OnLabelPrinterChanged);
            // 
            // labelLabelPrinter
            // 
            this.labelLabelPrinter.AutoSize = true;
            this.labelLabelPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabelPrinter.Location = new System.Drawing.Point(78, 93);
            this.labelLabelPrinter.Name = "labelLabelPrinter";
            this.labelLabelPrinter.Size = new System.Drawing.Size(258, 13);
            this.labelLabelPrinter.TabIndex = 61;
            this.labelLabelPrinter.Text = "What printer should shipping labels print on?";
            // 
            // pictureLabelPrinter
            // 
            this.pictureLabelPrinter.Image = global::ShipWorks.Properties.Resources.box_closed_with_label;
            this.pictureLabelPrinter.Location = new System.Drawing.Point(24, 93);
            this.pictureLabelPrinter.Name = "pictureLabelPrinter";
            this.pictureLabelPrinter.Size = new System.Drawing.Size(48, 48);
            this.pictureLabelPrinter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureLabelPrinter.TabIndex = 60;
            this.pictureLabelPrinter.TabStop = false;
            // 
            // printerTypeControl
            // 
            this.printerTypeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printerTypeControl.Location = new System.Drawing.Point(71, 140);
            this.printerTypeControl.Name = "printerTypeControl";
            this.printerTypeControl.Size = new System.Drawing.Size(427, 117);
            this.printerTypeControl.TabIndex = 0;
            this.printerTypeControl.Visible = false;
            // 
            // wizardPageShipping
            // 
            this.wizardPageShipping.Controls.Add(this.labelShippingHelp);
            this.wizardPageShipping.Controls.Add(this.comboShippingCarrier);
            this.wizardPageShipping.Controls.Add(this.labelSelectCarriers);
            this.wizardPageShipping.Controls.Add(this.pictureTruck);
            this.wizardPageShipping.Description = "Select the shipping carriers you\'ll use to ship.";
            this.wizardPageShipping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageShipping.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageShipping.Location = new System.Drawing.Point(0, 0);
            this.wizardPageShipping.Name = "wizardPageShipping";
            this.wizardPageShipping.Size = new System.Drawing.Size(548, 271);
            this.wizardPageShipping.TabIndex = 0;
            this.wizardPageShipping.Title = "Shipping Setup";
            // 
            // labelShippingHelp
            // 
            this.labelShippingHelp.AutoSize = true;
            this.labelShippingHelp.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelShippingHelp.Location = new System.Drawing.Point(94, 56);
            this.labelShippingHelp.Name = "labelShippingHelp";
            this.labelShippingHelp.Size = new System.Drawing.Size(338, 13);
            this.labelShippingHelp.TabIndex = 51;
            this.labelShippingHelp.Text = "(If you use more than one just pick one. It\'s easy to add more later.)";
            // 
            // comboShippingCarrier
            // 
            this.comboShippingCarrier.FormattingEnabled = true;
            this.comboShippingCarrier.Items.AddRange(new object[] {
            "US Postal Service",
            "FedEx",
            "UPS",
            "Something else..."});
            this.comboShippingCarrier.Location = new System.Drawing.Point(97, 29);
            this.comboShippingCarrier.MaxDropDownItems = 20;
            this.comboShippingCarrier.Name = "comboShippingCarrier";
            this.comboShippingCarrier.Size = new System.Drawing.Size(223, 21);
            this.comboShippingCarrier.TabIndex = 50;
            // 
            // labelSelectCarriers
            // 
            this.labelSelectCarriers.AutoSize = true;
            this.labelSelectCarriers.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectCarriers.Location = new System.Drawing.Point(79, 9);
            this.labelSelectCarriers.Name = "labelSelectCarriers";
            this.labelSelectCarriers.Size = new System.Drawing.Size(198, 13);
            this.labelSelectCarriers.TabIndex = 47;
            this.labelSelectCarriers.Text = "What shipping carrier do you use?";
            // 
            // pictureTruck
            // 
            this.pictureTruck.Image = global::ShipWorks.Properties.Resources.truck_blue1;
            this.pictureTruck.Location = new System.Drawing.Point(23, 9);
            this.pictureTruck.Name = "pictureTruck";
            this.pictureTruck.Size = new System.Drawing.Size(48, 48);
            this.pictureTruck.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureTruck.TabIndex = 46;
            this.pictureTruck.TabStop = false;
            // 
            // wizardPagePackingSlip
            // 
            this.wizardPagePackingSlip.Controls.Add(this.picturePackingSlip);
            this.wizardPagePackingSlip.Controls.Add(this.includePackingSlip);
            this.wizardPagePackingSlip.Controls.Add(this.labelPackingSlip);
            this.wizardPagePackingSlip.Controls.Add(this.labelPicturePackingSlip);
            this.wizardPagePackingSlip.Description = "Configure ShipWorks to print packing slips with shipping labels.";
            this.wizardPagePackingSlip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePackingSlip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPagePackingSlip.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePackingSlip.Name = "wizardPagePackingSlip";
            this.wizardPagePackingSlip.Size = new System.Drawing.Size(548, 271);
            this.wizardPagePackingSlip.TabIndex = 0;
            this.wizardPagePackingSlip.Title = "Packing Slip";
            this.wizardPagePackingSlip.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoPackingSlips);
            // 
            // picturePackingSlip
            // 
            this.picturePackingSlip.Image = global::ShipWorks.Properties.Resources.document_plain_shipping_labels;
            this.picturePackingSlip.Location = new System.Drawing.Point(92, 56);
            this.picturePackingSlip.Name = "picturePackingSlip";
            this.picturePackingSlip.Size = new System.Drawing.Size(175, 175);
            this.picturePackingSlip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picturePackingSlip.TabIndex = 67;
            this.picturePackingSlip.TabStop = false;
            // 
            // includePackingSlip
            // 
            this.includePackingSlip.AutoSize = true;
            this.includePackingSlip.Checked = true;
            this.includePackingSlip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.includePackingSlip.Location = new System.Drawing.Point(81, 31);
            this.includePackingSlip.Name = "includePackingSlip";
            this.includePackingSlip.Size = new System.Drawing.Size(239, 17);
            this.includePackingSlip.TabIndex = 66;
            this.includePackingSlip.Text = "Print a packing slip with every shipping label.";
            this.includePackingSlip.UseVisualStyleBackColor = true;
            this.includePackingSlip.CheckedChanged += new System.EventHandler(this.OnChangeIncludePackingSlip);
            // 
            // labelPackingSlip
            // 
            this.labelPackingSlip.AutoSize = true;
            this.labelPackingSlip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPackingSlip.Location = new System.Drawing.Point(78, 11);
            this.labelPackingSlip.Name = "labelPackingSlip";
            this.labelPackingSlip.Size = new System.Drawing.Size(80, 13);
            this.labelPackingSlip.TabIndex = 64;
            this.labelPackingSlip.Text = "Packing Slips";
            // 
            // labelPicturePackingSlip
            // 
            this.labelPicturePackingSlip.Image = global::ShipWorks.Properties.Resources.form_blue1;
            this.labelPicturePackingSlip.Location = new System.Drawing.Point(23, 9);
            this.labelPicturePackingSlip.Name = "labelPicturePackingSlip";
            this.labelPicturePackingSlip.Size = new System.Drawing.Size(48, 48);
            this.labelPicturePackingSlip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.labelPicturePackingSlip.TabIndex = 63;
            this.labelPicturePackingSlip.TabStop = false;
            // 
            // wizardPage2
            // 
            this.wizardPage2.Controls.Add(this.linkControl2);
            this.wizardPage2.Controls.Add(this.pictureBox4);
            this.wizardPage2.Controls.Add(this.label5);
            this.wizardPage2.Controls.Add(this.linkControl1);
            this.wizardPage2.Controls.Add(this.label4);
            this.wizardPage2.Controls.Add(this.label3);
            this.wizardPage2.Controls.Add(this.pictureBox3);
            this.wizardPage2.Controls.Add(this.label2);
            this.wizardPage2.Controls.Add(this.label1);
            this.wizardPage2.Controls.Add(this.pictureBox1);
            this.wizardPage2.Controls.Add(this.pictureBox2);
            this.wizardPage2.Description = "Get ShipWorks configured to download and ship.";
            this.wizardPage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage2.Location = new System.Drawing.Point(0, 0);
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.Size = new System.Drawing.Size(548, 271);
            this.wizardPage2.TabIndex = 0;
            this.wizardPage2.Title = "Final Configuration";
            // 
            // linkControl2
            // 
            this.linkControl2.AutoSize = true;
            this.linkControl2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkControl2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkControl2.ForeColor = System.Drawing.Color.Blue;
            this.linkControl2.Location = new System.Drawing.Point(41, 88);
            this.linkControl2.Name = "linkControl2";
            this.linkControl2.Size = new System.Drawing.Size(54, 13);
            this.linkControl2.TabIndex = 67;
            this.linkControl2.Text = "Configure";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::ShipWorks.Properties.Resources.check16;
            this.pictureBox4.Location = new System.Drawing.Point(39, 62);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(16, 16);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 66;
            this.pictureBox4.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.DarkGreen;
            this.label5.Location = new System.Drawing.Point(57, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 65;
            this.label5.Text = "Ready";
            // 
            // linkControl1
            // 
            this.linkControl1.AutoSize = true;
            this.linkControl1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.linkControl1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
            this.linkControl1.ForeColor = System.Drawing.Color.Blue;
            this.linkControl1.Location = new System.Drawing.Point(41, 38);
            this.linkControl1.Name = "linkControl1";
            this.linkControl1.Size = new System.Drawing.Size(54, 13);
            this.linkControl1.TabIndex = 64;
            this.linkControl1.Text = "Configure";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(22, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(226, 13);
            this.label4.TabIndex = 63;
            this.label4.Text = "Configure ShipWorks for your accounts";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(123, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 62;
            this.label3.Text = "Magento";
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ShipWorks.Properties.StoreIcons.magento;
            this.pictureBox3.Location = new System.Drawing.Point(102, 36);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(16, 16);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 61;
            this.pictureBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(123, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "UPS";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 59;
            this.label1.Text = "US Postal Service";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.ShippingIcons.ups;
            this.pictureBox1.Location = new System.Drawing.Point(101, 87);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 58;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ShipWorks.Properties.ShippingIcons.usps;
            this.pictureBox2.Location = new System.Drawing.Point(102, 62);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 57;
            this.pictureBox2.TabStop = false;
            // 
            // wizardPage1
            // 
            this.wizardPage1.Description = "The description of the page.";
            this.wizardPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage1.Location = new System.Drawing.Point(0, 0);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(548, 271);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Title = "Wizard page 6.";
            // 
            // ShipWorksSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 378);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(564, 416);
            this.MinimumSize = new System.Drawing.Size(564, 416);
            this.Name = "ShipWorksSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPagePrinters,
            this.wizardPagePackingSlip,
            this.wizardPageOnlineStore,
            this.wizardPageShipping,
            this.wizardPage2,
            this.wizardPage1});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Setup";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageOnlineStore.ResumeLayout(false);
            this.wizardPageOnlineStore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureShoppingCart)).EndInit();
            this.wizardPagePrinters.ResumeLayout(false);
            this.wizardPagePrinters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinterHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLabelPrinter)).EndInit();
            this.wizardPageShipping.ResumeLayout(false);
            this.wizardPageShipping.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureTruck)).EndInit();
            this.wizardPagePackingSlip.ResumeLayout(false);
            this.wizardPagePackingSlip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePackingSlip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelPicturePackingSlip)).EndInit();
            this.wizardPage2.ResumeLayout(false);
            this.wizardPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageOnlineStore;
        private UI.Wizard.WizardPage wizardPagePrinters;
        private UI.Wizard.WizardPage wizardPageShipping;
        private ShipWorks.Common.IO.Hardware.Printers.PrinterTypeControl printerTypeControl;
        private UI.Wizard.WizardPage wizardPagePackingSlip;
        private UI.Wizard.WizardPage wizardPage2;
        private Templates.Media.PrinterSelectionControl labelPrinter;
        private System.Windows.Forms.Label labelLabelPrinter;
        private System.Windows.Forms.PictureBox pictureLabelPrinter;
        private System.Windows.Forms.Label labelPackingSlip;
        private System.Windows.Forms.PictureBox labelPicturePackingSlip;
        private Templates.Media.PrinterSelectionControl standardPrinter;
        private System.Windows.Forms.Label labelStandardPrinter;
        private System.Windows.Forms.PictureBox pictureStandardPrinter;
        private System.Windows.Forms.Label labelStandardPrinterHelp;
        private System.Windows.Forms.PictureBox pictureStandardPrinterHelp;
        private System.Windows.Forms.CheckBox includePackingSlip;
        private System.Windows.Forms.PictureBox picturePackingSlip;
        private System.Windows.Forms.Label labelStoreTypeHelp;
        private ShipWorks.UI.Controls.ImageComboBox comboStoreType;
        private System.Windows.Forms.PictureBox pictureShoppingCart;
        private System.Windows.Forms.Label labelSelectCarriers;
        private System.Windows.Forms.PictureBox pictureTruck;
        private System.Windows.Forms.RadioButton radioStoreConnect;
        private System.Windows.Forms.Label labelSampleOrderHelp;
        private System.Windows.Forms.RadioButton radioStoreSamples;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label4;
        private UI.Controls.LinkControl linkControl2;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label5;
        private UI.Controls.LinkControl linkControl1;
        private UI.Wizard.WizardPage wizardPage1;
        private System.Windows.Forms.Label labelShippingHelp;
        private UI.Controls.ImageComboBox comboShippingCarrier;
    }
}