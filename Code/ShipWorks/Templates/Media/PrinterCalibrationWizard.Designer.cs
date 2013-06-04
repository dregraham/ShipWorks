namespace ShipWorks.Templates.Media
{
    partial class PrinterCalibrationWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterCalibrationWizard));
            this.wizardPagePrinter = new ShipWorks.UI.Wizard.WizardPage();
            this.label1 = new System.Windows.Forms.Label();
            this.printerControl = new ShipWorks.Templates.Media.PrinterSelectionControl();
            this.wizardPageCalibrate = new ShipWorks.UI.Wizard.WizardPage();
            this.paperSizeControl = new ShipWorks.Templates.Media.PaperDimensionsControl();
            this.yOffset = new System.Windows.Forms.TextBox();
            this.xOffset = new System.Windows.Forms.TextBox();
            this.labelHorizontalFold = new System.Windows.Forms.Label();
            this.labelVerticalFold = new System.Windows.Forms.Label();
            this.labelReadInstructions = new System.Windows.Forms.Label();
            this.labelReadHeading = new System.Windows.Forms.Label();
            this.print = new System.Windows.Forms.Button();
            this.labelPrintInstructions = new System.Windows.Forms.Label();
            this.labelPrintHeading = new System.Windows.Forms.Label();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPagePrinter.SuspendLayout();
            this.wizardPageCalibrate.SuspendLayout();
            this.SuspendLayout();
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(358, 363);
            // 
            // cancel
            // 
            this.cancel.Location = new System.Drawing.Point(439, 363);
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(277, 363);
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.wizardPagePrinter);
            this.mainPanel.Size = new System.Drawing.Size(526, 291);
            // 
            // etchBottom
            // 
            this.etchBottom.Location = new System.Drawing.Point(0, 353);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = global::ShipWorks.Properties.Resources.printer_preferences1;
            // 
            // wizardPagePrinter
            // 
            this.wizardPagePrinter.Controls.Add(this.label1);
            this.wizardPagePrinter.Controls.Add(this.printerControl);
            this.wizardPagePrinter.Description = "Select the printer and paper source tray to be calibrated.";
            this.wizardPagePrinter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPagePrinter.Location = new System.Drawing.Point(0, 0);
            this.wizardPagePrinter.Name = "wizardPagePrinter";
            this.wizardPagePrinter.Size = new System.Drawing.Size(526, 291);
            this.wizardPagePrinter.TabIndex = 0;
            this.wizardPagePrinter.Title = "Printer and Paper Source";
            this.wizardPagePrinter.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnPrinterSelectionStepNext);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(449, 55);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // printerControl
            // 
            this.printerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.printerControl.Location = new System.Drawing.Point(21, 76);
            this.printerControl.Name = "printerControl";
            this.printerControl.Size = new System.Drawing.Size(328, 59);
            this.printerControl.TabIndex = 1;
            // 
            // wizardPageCalibrate
            // 
            this.wizardPageCalibrate.Controls.Add(this.paperSizeControl);
            this.wizardPageCalibrate.Controls.Add(this.yOffset);
            this.wizardPageCalibrate.Controls.Add(this.xOffset);
            this.wizardPageCalibrate.Controls.Add(this.labelHorizontalFold);
            this.wizardPageCalibrate.Controls.Add(this.labelVerticalFold);
            this.wizardPageCalibrate.Controls.Add(this.labelReadInstructions);
            this.wizardPageCalibrate.Controls.Add(this.labelReadHeading);
            this.wizardPageCalibrate.Controls.Add(this.print);
            this.wizardPageCalibrate.Controls.Add(this.labelPrintInstructions);
            this.wizardPageCalibrate.Controls.Add(this.labelPrintHeading);
            this.wizardPageCalibrate.Description = "Calibrate the printer using a printed calibration page.";
            this.wizardPageCalibrate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageCalibrate.Location = new System.Drawing.Point(0, 0);
            this.wizardPageCalibrate.Name = "wizardPageCalibrate";
            this.wizardPageCalibrate.Size = new System.Drawing.Size(526, 291);
            this.wizardPageCalibrate.TabIndex = 0;
            this.wizardPageCalibrate.Title = "Calibration";
            this.wizardPageCalibrate.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnFinishStepNext);
            // 
            // paperSizeControl
            // 
            this.paperSizeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.paperSizeControl.Location = new System.Drawing.Point(48, 60);
            this.paperSizeControl.Name = "paperSizeControl";
            this.paperSizeControl.PaperHeight = 11;
            this.paperSizeControl.PaperWidth = 8.5;
            this.paperSizeControl.Size = new System.Drawing.Size(277, 63);
            this.paperSizeControl.TabIndex = 2;
            // 
            // yOffset
            // 
            this.yOffset.Location = new System.Drawing.Point(227, 254);
            this.yOffset.Name = "yOffset";
            this.yOffset.Size = new System.Drawing.Size(50, 21);
            this.yOffset.TabIndex = 9;
            this.yOffset.Text = "0";
            // 
            // xOffset
            // 
            this.xOffset.Location = new System.Drawing.Point(227, 230);
            this.xOffset.Name = "xOffset";
            this.xOffset.Size = new System.Drawing.Size(50, 21);
            this.xOffset.TabIndex = 7;
            this.xOffset.Text = "0";
            // 
            // labelHorizontalFold
            // 
            this.labelHorizontalFold.AutoSize = true;
            this.labelHorizontalFold.Location = new System.Drawing.Point(53, 257);
            this.labelHorizontalFold.Name = "labelHorizontalFold";
            this.labelHorizontalFold.Size = new System.Drawing.Size(171, 13);
            this.labelHorizontalFold.TabIndex = 8;
            this.labelHorizontalFold.Text = "Horizontal fold crosses \"B\" line at: ";
            // 
            // labelVerticalFold
            // 
            this.labelVerticalFold.AutoSize = true;
            this.labelVerticalFold.Location = new System.Drawing.Point(65, 233);
            this.labelVerticalFold.Name = "labelVerticalFold";
            this.labelVerticalFold.Size = new System.Drawing.Size(159, 13);
            this.labelVerticalFold.TabIndex = 6;
            this.labelVerticalFold.Text = "Vertical fold crosses \"A\" line at: ";
            // 
            // labelReadInstructions
            // 
            this.labelReadInstructions.Location = new System.Drawing.Point(42, 189);
            this.labelReadInstructions.Name = "labelReadInstructions";
            this.labelReadInstructions.Size = new System.Drawing.Size(457, 31);
            this.labelReadInstructions.TabIndex = 5;
            this.labelReadInstructions.Text = "Fold the page in half in both directions, creating four equal quarters.  Enter th" +
                "e number where each fold crosses the printed lines. ";
            // 
            // labelReadHeading
            // 
            this.labelReadHeading.AutoSize = true;
            this.labelReadHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelReadHeading.Location = new System.Drawing.Point(20, 166);
            this.labelReadHeading.Name = "labelReadHeading";
            this.labelReadHeading.Size = new System.Drawing.Size(113, 13);
            this.labelReadHeading.TabIndex = 4;
            this.labelReadHeading.Text = "2. Read the results";
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(48, 126);
            this.print.Name = "print";
            this.print.Size = new System.Drawing.Size(75, 23);
            this.print.TabIndex = 3;
            this.print.Text = "Print";
            this.print.UseVisualStyleBackColor = true;
            this.print.Click += new System.EventHandler(this.OnPrint);
            // 
            // labelPrintInstructions
            // 
            this.labelPrintInstructions.Location = new System.Drawing.Point(42, 28);
            this.labelPrintInstructions.Name = "labelPrintInstructions";
            this.labelPrintInstructions.Size = new System.Drawing.Size(436, 36);
            this.labelPrintInstructions.TabIndex = 1;
            this.labelPrintInstructions.Text = "Insert a plain piece of paper into the printer and source tray.  Select the size " +
                "of the paper you inserted into the printer and print the calibration page.";
            // 
            // labelPrintHeading
            // 
            this.labelPrintHeading.AutoSize = true;
            this.labelPrintHeading.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelPrintHeading.Location = new System.Drawing.Point(20, 9);
            this.labelPrintHeading.Name = "labelPrintHeading";
            this.labelPrintHeading.Size = new System.Drawing.Size(167, 13);
            this.labelPrintHeading.TabIndex = 0;
            this.labelPrintHeading.Text = "1. Print the calibration sheet";
            // 
            // PrinterCalibrationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 398);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.LastPageCancelable = true;
            this.Name = "PrinterCalibrationWizard";
            this.Pages.AddRange(new ShipWorks.UI.Wizard.WizardPage[] {
            this.wizardPagePrinter,
            this.wizardPageCalibrate});
            this.Text = "Printer Calibration Wizard";
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.pictureBox)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.wizardPagePrinter.ResumeLayout(false);
            this.wizardPageCalibrate.ResumeLayout(false);
            this.wizardPageCalibrate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ShipWorks.UI.Wizard.WizardPage wizardPagePrinter;
        private ShipWorks.UI.Wizard.WizardPage wizardPageCalibrate;
        private ShipWorks.Templates.Media.PrinterSelectionControl printerControl;
        private System.Windows.Forms.Label labelPrintHeading;
        private System.Windows.Forms.Button print;
        private System.Windows.Forms.Label labelPrintInstructions;
        private System.Windows.Forms.Label labelReadHeading;
        private System.Windows.Forms.Label labelReadInstructions;
        private System.Windows.Forms.Label labelHorizontalFold;
        private System.Windows.Forms.Label labelVerticalFold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox yOffset;
        private System.Windows.Forms.TextBox xOffset;
        private PaperDimensionsControl paperSizeControl;
    }
}