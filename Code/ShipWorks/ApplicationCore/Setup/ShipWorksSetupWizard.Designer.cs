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
            this.wizardPageOnlineStore = new ShipWorks.UI.Wizard.WizardPage();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.wizardPageLabelPrinter = new ShipWorks.UI.Wizard.WizardPage();
            this.labelPrinter = new ShipWorks.Templates.Media.PrinterSelectionControl();
            this.labelLabelPrinter = new System.Windows.Forms.Label();
            this.pictureLabelPrinter = new System.Windows.Forms.PictureBox();
            this.printerTypeControl = new ShipWorks.Common.IO.Hardware.Printers.PrinterTypeControl();
            this.wizardPage1 = new ShipWorks.UI.Wizard.WizardPage();
            this.wizardPageDocumentPrinter = new ShipWorks.UI.Wizard.WizardPage();
            this.wizardPage2 = new ShipWorks.UI.Wizard.WizardPage();
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.topPanel.SuspendLayout();
            this.wizardPageOnlineStore.SuspendLayout();
            this.wizardPageLabelPrinter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLabelPrinter)).BeginInit();
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
            this.mainPanel.Controls.Add(this.wizardPageLabelPrinter);
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
            // wizardPageLabelPrinter
            // 
            this.wizardPageLabelPrinter.Controls.Add(this.labelPrinter);
            this.wizardPageLabelPrinter.Controls.Add(this.labelLabelPrinter);
            this.wizardPageLabelPrinter.Controls.Add(this.pictureLabelPrinter);
            this.wizardPageLabelPrinter.Controls.Add(this.printerTypeControl);
            this.wizardPageLabelPrinter.Description = "Select the printer you will use for shipping labels.";
            this.wizardPageLabelPrinter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageLabelPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageLabelPrinter.Location = new System.Drawing.Point(0, 0);
            this.wizardPageLabelPrinter.Name = "wizardPageLabelPrinter";
            this.wizardPageLabelPrinter.Size = new System.Drawing.Size(548, 271);
            this.wizardPageLabelPrinter.TabIndex = 0;
            this.wizardPageLabelPrinter.Title = "Printer Selection";
            this.wizardPageLabelPrinter.StepNext += new System.EventHandler<ShipWorks.UI.Wizard.WizardStepEventArgs>(this.OnStepNextLabelPrinter);
            this.wizardPageLabelPrinter.SteppingInto += new System.EventHandler<ShipWorks.UI.Wizard.WizardSteppingIntoEventArgs>(this.OnSteppingIntoLabelPrinter);
            // 
            // labelPrinter
            // 
            this.labelPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrinter.Location = new System.Drawing.Point(80, 29);
            this.labelPrinter.Name = "labelPrinter";
            this.labelPrinter.ShowLabels = false;
            this.labelPrinter.ShowPaperSource = false;
            this.labelPrinter.Size = new System.Drawing.Size(280, 28);
            this.labelPrinter.TabIndex = 62;
            this.labelPrinter.PrinterChanged += new System.EventHandler(this.OnPrinterChanged);
            // 
            // labelLabelPrinter
            // 
            this.labelLabelPrinter.AutoSize = true;
            this.labelLabelPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabelPrinter.Location = new System.Drawing.Point(78, 9);
            this.labelLabelPrinter.Name = "labelLabelPrinter";
            this.labelLabelPrinter.Size = new System.Drawing.Size(258, 13);
            this.labelLabelPrinter.TabIndex = 61;
            this.labelLabelPrinter.Text = "What printer should shipping labels print on?";
            // 
            // pictureLabelPrinter
            // 
            this.pictureLabelPrinter.Image = global::ShipWorks.Properties.Resources.box_closed_with_label;
            this.pictureLabelPrinter.Location = new System.Drawing.Point(24, 9);
            this.pictureLabelPrinter.Name = "pictureLabelPrinter";
            this.pictureLabelPrinter.Size = new System.Drawing.Size(48, 48);
            this.pictureLabelPrinter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureLabelPrinter.TabIndex = 60;
            this.pictureLabelPrinter.TabStop = false;
            // 
            // printerFormatControl
            // 
            this.printerTypeControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.printerTypeControl.Location = new System.Drawing.Point(71, 59);
            this.printerTypeControl.Name = "printerFormatControl";
            this.printerTypeControl.Size = new System.Drawing.Size(427, 162);
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
            // wizardPageDocumentPrinter
            // 
            this.wizardPageDocumentPrinter.Description = "Select the printer you will use for documents";
            this.wizardPageDocumentPrinter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardPageDocumentPrinter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardPageDocumentPrinter.Location = new System.Drawing.Point(0, 0);
            this.wizardPageDocumentPrinter.Name = "wizardPageDocumentPrinter";
            this.wizardPageDocumentPrinter.Size = new System.Drawing.Size(548, 271);
            this.wizardPageDocumentPrinter.TabIndex = 0;
            this.wizardPageDocumentPrinter.Title = "Printer Selection";
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
            this.wizardPageLabelPrinter,
            this.wizardPageDocumentPrinter,
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
            this.wizardPageLabelPrinter.ResumeLayout(false);
            this.wizardPageLabelPrinter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureLabelPrinter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Wizard.WizardPage wizardPageOnlineStore;
        private UI.Wizard.WizardPage wizardPageLabelPrinter;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private UI.Wizard.WizardPage wizardPage1;
        private ShipWorks.Common.IO.Hardware.Printers.PrinterTypeControl printerTypeControl;
        private UI.Wizard.WizardPage wizardPageDocumentPrinter;
        private UI.Wizard.WizardPage wizardPage2;
        private Templates.Media.PrinterSelectionControl labelPrinter;
        private System.Windows.Forms.Label labelLabelPrinter;
        private System.Windows.Forms.PictureBox pictureLabelPrinter;
    }
}