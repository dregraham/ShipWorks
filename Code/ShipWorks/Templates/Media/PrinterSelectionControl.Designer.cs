namespace ShipWorks.Templates.Media
{
    partial class PrinterSelectionControl
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
            this.components = new System.ComponentModel.Container();
            this.labelPrinterName = new System.Windows.Forms.Label();
            this.labelPaperSource = new System.Windows.Forms.Label();
            this.paperSource = new System.Windows.Forms.ComboBox();
            this.printer = new System.Windows.Forms.ComboBox();
            this.labelCalibrate = new System.Windows.Forms.Label();
            this.calibratePrinter = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelPrinter = new System.Windows.Forms.Panel();
            this.panelSource = new System.Windows.Forms.Panel();
            this.panelCalibrate = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.panelPrinter.SuspendLayout();
            this.panelSource.SuspendLayout();
            this.panelCalibrate.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPrinterName
            // 
            this.labelPrinterName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrinterName.Location = new System.Drawing.Point(3, 5);
            this.labelPrinterName.Name = "labelPrinterName";
            this.labelPrinterName.Size = new System.Drawing.Size(44, 16);
            this.labelPrinterName.TabIndex = 0;
            this.labelPrinterName.Text = "Printer:";
            // 
            // labelPaperSource
            // 
            this.labelPaperSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPaperSource.Location = new System.Drawing.Point(1, 5);
            this.labelPaperSource.Name = "labelPaperSource";
            this.labelPaperSource.Size = new System.Drawing.Size(44, 16);
            this.labelPaperSource.TabIndex = 2;
            this.labelPaperSource.Text = "Source:";
            // 
            // paperSource
            // 
            this.paperSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paperSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paperSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.paperSource.ItemHeight = 13;
            this.paperSource.Location = new System.Drawing.Point(53, 3);
            this.paperSource.Name = "paperSource";
            this.paperSource.Size = new System.Drawing.Size(289, 21);
            this.paperSource.TabIndex = 3;
            // 
            // printer
            // 
            this.printer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.printer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.printer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorProvider.SetIconPadding(this.printer, 2);
            this.printer.ItemHeight = 13;
            this.printer.Location = new System.Drawing.Point(53, 3);
            this.printer.Name = "printer";
            this.printer.Size = new System.Drawing.Size(289, 21);
            this.printer.TabIndex = 1;
            this.printer.SelectedIndexChanged += new System.EventHandler(this.OnChangePrinter);
            // 
            // labelCalibrate
            // 
            this.labelCalibrate.AutoSize = true;
            this.labelCalibrate.Location = new System.Drawing.Point(5, 6);
            this.labelCalibrate.Name = "labelCalibrate";
            this.labelCalibrate.Size = new System.Drawing.Size(41, 13);
            this.labelCalibrate.TabIndex = 4;
            this.labelCalibrate.Text = "Labels:";
            // 
            // calibratePrinter
            // 
            this.calibratePrinter.Location = new System.Drawing.Point(52, 1);
            this.calibratePrinter.Name = "calibratePrinter";
            this.calibratePrinter.Size = new System.Drawing.Size(115, 23);
            this.calibratePrinter.TabIndex = 5;
            this.calibratePrinter.Text = "Calibrate Printer...";
            this.calibratePrinter.UseVisualStyleBackColor = true;
            this.calibratePrinter.Click += new System.EventHandler(this.OnCalibratePrinter);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // panelPrinter
            // 
            this.panelPrinter.Controls.Add(this.printer);
            this.panelPrinter.Controls.Add(this.labelPrinterName);
            this.panelPrinter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPrinter.Location = new System.Drawing.Point(0, 0);
            this.panelPrinter.Name = "panelPrinter";
            this.panelPrinter.Size = new System.Drawing.Size(346, 28);
            this.panelPrinter.TabIndex = 6;
            // 
            // panelSource
            // 
            this.panelSource.Controls.Add(this.paperSource);
            this.panelSource.Controls.Add(this.labelPaperSource);
            this.panelSource.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSource.Location = new System.Drawing.Point(0, 28);
            this.panelSource.Name = "panelSource";
            this.panelSource.Size = new System.Drawing.Size(346, 29);
            this.panelSource.TabIndex = 7;
            // 
            // panelCalibrate
            // 
            this.panelCalibrate.Controls.Add(this.calibratePrinter);
            this.panelCalibrate.Controls.Add(this.labelCalibrate);
            this.panelCalibrate.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCalibrate.Location = new System.Drawing.Point(0, 57);
            this.panelCalibrate.Name = "panelCalibrate";
            this.panelCalibrate.Size = new System.Drawing.Size(346, 30);
            this.panelCalibrate.TabIndex = 8;
            // 
            // PrinterSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCalibrate);
            this.Controls.Add(this.panelSource);
            this.Controls.Add(this.panelPrinter);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "PrinterSelectionControl";
            this.Size = new System.Drawing.Size(346, 89);
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.panelPrinter.ResumeLayout(false);
            this.panelSource.ResumeLayout(false);
            this.panelCalibrate.ResumeLayout(false);
            this.panelCalibrate.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelPrinterName;
        private System.Windows.Forms.Label labelPaperSource;
        private System.Windows.Forms.ComboBox paperSource;
        private System.Windows.Forms.ComboBox printer;
        private System.Windows.Forms.Label labelCalibrate;
        private System.Windows.Forms.Button calibratePrinter;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Panel panelCalibrate;
        private System.Windows.Forms.Panel panelSource;
        private System.Windows.Forms.Panel panelPrinter;
    }
}
