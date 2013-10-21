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
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
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
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
            this.wizardPagePackingSlip = new ShipWorks.UI.Wizard.WizardPage();
            this.picturePackingSlip = new System.Windows.Forms.PictureBox();
            this.includePackingSlip = new System.Windows.Forms.CheckBox();
            this.labelPackingSlip = new System.Windows.Forms.Label();
            this.labelPicturePackingSlip = new System.Windows.Forms.PictureBox();
            this.wizardPage2 = new ShipWorks.UI.Wizard.WizardPage();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageOnlineStore.SuspendLayout();
            this.wizardPagePrinters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinterHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLabelPrinter)).BeginInit();
            this.wizardPagePackingSlip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePackingSlip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelPicturePackingSlip)).BeginInit();
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
            this.mainPanel.Controls.Add(this.wizardPagePackingSlip);
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
            this.wizardPageOnlineStore.Controls.Add(this.radioButton2);
            this.wizardPageOnlineStore.Controls.Add(this.radioButton1);
            this.wizardPageOnlineStore.Description = "What platform do you sell on?";
            this.wizardPageOnlineStore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageOnlineStore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageOnlineStore.Location = new System.Drawing.Point(0, 0);
            this.wizardPageOnlineStore.Name = "wizardPageOnlineStore";
            this.wizardPageOnlineStore.Size = new System.Drawing.Size(548, 271);
            this.wizardPageOnlineStore.TabIndex = 0;
            this.wizardPageOnlineStore.Title = "Online Store";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(23, 82);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(299, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Just give me some sample orders and let me start playing";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(23, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(184, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Get connected to my online store";
            this.radioButton1.UseVisualStyleBackColor = true;
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
            this.labelStandardPrinterHelp.Size = new System.Drawing.Size(298, 13);
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
            // wizardPage1
            // 
            this.wizardPage1.Description = "The description of the page.";
            this.wizardPage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage1.Location = new System.Drawing.Point(0, 0);
            this.wizardPage1.Name = "wizardPage1";
            this.wizardPage1.Size = new System.Drawing.Size(548, 271);
            this.wizardPage1.TabIndex = 0;
            this.wizardPage1.Title = "Wizard page 3.";
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
            this.wizardPage2.Description = "The description of the page.";
            this.wizardPage2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPage2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPage2.Location = new System.Drawing.Point(0, 0);
            this.wizardPage2.Name = "wizardPage2";
            this.wizardPage2.Size = new System.Drawing.Size(548, 271);
            this.wizardPage2.TabIndex = 0;
            this.wizardPage2.Title = "Printer Selection";
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
            this.wizardPage1,
            this.wizardPage2});
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "ShipWorks Setup";
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPageOnlineStore.ResumeLayout(false);
            this.wizardPageOnlineStore.PerformLayout();
            this.wizardPagePrinters.ResumeLayout(false);
            this.wizardPagePrinters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinterHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureStandardPrinter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLabelPrinter)).EndInit();
            this.wizardPagePackingSlip.ResumeLayout(false);
            this.wizardPagePackingSlip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picturePackingSlip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.labelPicturePackingSlip)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageOnlineStore;
        private UI.Wizard.WizardPage wizardPagePrinters;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private UI.Wizard.WizardPage wizardPage1;
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
    }
}