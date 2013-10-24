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
            this.wizardPageFinish = new ShipWorks.UI.Wizard.WizardPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageOnlineStore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureShoppingCart)).BeginInit();
            this.wizardPagePrinters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinterHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLabelPrinter)).BeginInit();
            this.wizardPageFinish.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.next.Location = new System.Drawing.Point(380, 343);
            this.next.Text = "Finish";
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
            this.mainPanel.Controls.Add(this.wizardPageFinish);
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
            this.wizardPageOnlineStore.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextOnlineStore);
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
            // wizardPageFinish
            // 
            this.wizardPageFinish.Controls.Add(this.pictureBox3);
            this.wizardPageFinish.Controls.Add(this.pictureBox2);
            this.wizardPageFinish.Controls.Add(this.label5);
            this.wizardPageFinish.Controls.Add(this.label6);
            this.wizardPageFinish.Controls.Add(this.label4);
            this.wizardPageFinish.Controls.Add(this.label3);
            this.wizardPageFinish.Controls.Add(this.label2);
            this.wizardPageFinish.Controls.Add(this.label1);
            this.wizardPageFinish.Controls.Add(this.pictureBox1);
            this.wizardPageFinish.Description = "ShipWorks is ready to connect to your store!";
            this.wizardPageFinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageFinish.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageFinish.Location = new System.Drawing.Point(0, 0);
            this.wizardPageFinish.Name = "wizardPageFinish";
            this.wizardPageFinish.Size = new System.Drawing.Size(548, 271);
            this.wizardPageFinish.TabIndex = 0;
            this.wizardPageFinish.Title = "ShipWorks Setup";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ShipWorks.Properties.Resources.nav_down_green;
            this.pictureBox2.Location = new System.Drawing.Point(65, 57);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(103, 184);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(414, 35);
            this.label5.TabIndex = 6;
            this.label5.Text = "When you\'re ready to ship, select an order and click the \"Ship Orders\" button (sh" +
    "own at left).  ShipWorks will guide you through the rest.";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(103, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Printing shipping labels";
            // 
            // label4
            // 
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(103, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(430, 73);
            this.label4.TabIndex = 4;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(103, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Downloading orders";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "A couple things before you get started...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(62, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(258, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ShipWorks is ready to connect to your store!";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShipWorks.Properties.Resources.hand_thumb_up1;
            this.pictureBox1.Location = new System.Drawing.Point(24, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ShipWorks.Properties.Resources.box_closed32;
            this.pictureBox3.Location = new System.Drawing.Point(65, 165);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(32, 32);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // ShipWorksSetupWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 378);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximumSize = new System.Drawing.Size(564, 416);
            this.MinimumSize = new System.Drawing.Size(564, 416);
            this.Name = "ShipWorksSetupWizard";
            this.NextVisible = true;
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPagePrinters,
            this.wizardPageOnlineStore,
            this.wizardPageFinish});
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
            this.wizardPageFinish.ResumeLayout(false);
            this.wizardPageFinish.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageOnlineStore;
        private UI.Wizard.WizardPage wizardPagePrinters;
        private ShipWorks.Common.IO.Hardware.Printers.PrinterTypeControl printerTypeControl;
        private Templates.Media.PrinterSelectionControl labelPrinter;
        private System.Windows.Forms.Label labelLabelPrinter;
        private System.Windows.Forms.PictureBox pictureLabelPrinter;
        private Templates.Media.PrinterSelectionControl standardPrinter;
        private System.Windows.Forms.Label labelStandardPrinter;
        private System.Windows.Forms.PictureBox pictureStandardPrinter;
        private System.Windows.Forms.Label labelStandardPrinterHelp;
        private System.Windows.Forms.PictureBox pictureStandardPrinterHelp;
        private System.Windows.Forms.Label labelStoreTypeHelp;
        private ShipWorks.UI.Controls.ImageComboBox comboStoreType;
        private System.Windows.Forms.PictureBox pictureShoppingCart;
        private System.Windows.Forms.RadioButton radioStoreConnect;
        private System.Windows.Forms.Label labelSampleOrderHelp;
        private System.Windows.Forms.RadioButton radioStoreSamples;
        private UI.Wizard.WizardPage wizardPageFinish;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}