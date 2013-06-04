namespace ShipWorks.Templates.Printing
{
    partial class PrintJobSettingsDlg
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
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.optionControl = new ShipWorks.UI.Controls.OptionControl();
            this.optionPagePrinting = new ShipWorks.UI.Controls.OptionPage();
            this.groupBoxFirstLabel = new System.Windows.Forms.GroupBox();
            this.firstLabelRow = new System.Windows.Forms.NumericUpDown();
            this.labelFirstLabelRow = new System.Windows.Forms.Label();
            this.firstLabelColumn = new System.Windows.Forms.NumericUpDown();
            this.labelFirstLabelColumn = new System.Windows.Forms.Label();
            this.labelPositionControl = new ShipWorks.Templates.Media.LabelPositionControl();
            this.groupBoxCopies = new System.Windows.Forms.GroupBox();
            this.copiesControl = new ShipWorks.Templates.Media.PageCopiesControl();
            this.groupBoxPrinter = new System.Windows.Forms.GroupBox();
            this.printerControl = new ShipWorks.Templates.Media.PrinterSelectionControl();
            this.optionPagePaper = new ShipWorks.UI.Controls.OptionPage();
            this.pageSetupControl = new ShipWorks.Templates.Media.PageSetupControl();
            this.optionPageLabels = new ShipWorks.UI.Controls.OptionPage();
            this.labelSheetControl = new ShipWorks.Templates.Media.LabelSheetControl();
            this.optionControl.SuspendLayout();
            this.optionPagePrinting.SuspendLayout();
            this.groupBoxFirstLabel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.firstLabelRow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.firstLabelColumn)).BeginInit();
            this.groupBoxCopies.SuspendLayout();
            this.groupBoxPrinter.SuspendLayout();
            this.optionPagePaper.SuspendLayout();
            this.optionPageLabels.SuspendLayout();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok.Location = new System.Drawing.Point(342, 476);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 0;
            this.ok.Text = "Print";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(423, 476);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 1;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // optionControl
            // 
            this.optionControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.optionControl.Controls.Add(this.optionPagePrinting);
            this.optionControl.Controls.Add(this.optionPagePaper);
            this.optionControl.Controls.Add(this.optionPageLabels);
            this.optionControl.Location = new System.Drawing.Point(12, 12);
            this.optionControl.Name = "optionControl";
            this.optionControl.SelectedIndex = 0;
            this.optionControl.Size = new System.Drawing.Size(484, 458);
            this.optionControl.TabIndex = 3;
            this.optionControl.Text = "optionControl";
            // 
            // optionPagePrinting
            // 
            this.optionPagePrinting.Controls.Add(this.groupBoxFirstLabel);
            this.optionPagePrinting.Controls.Add(this.groupBoxCopies);
            this.optionPagePrinting.Controls.Add(this.groupBoxPrinter);
            this.optionPagePrinting.Location = new System.Drawing.Point(153, 0);
            this.optionPagePrinting.Name = "optionPagePrinting";
            this.optionPagePrinting.Padding = new System.Windows.Forms.Padding(3);
            this.optionPagePrinting.Size = new System.Drawing.Size(331, 458);
            this.optionPagePrinting.TabIndex = 2;
            this.optionPagePrinting.Text = "General";
            // 
            // groupBoxFirstLabel
            // 
            this.groupBoxFirstLabel.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxFirstLabel.Controls.Add(this.firstLabelRow);
            this.groupBoxFirstLabel.Controls.Add(this.labelFirstLabelRow);
            this.groupBoxFirstLabel.Controls.Add(this.firstLabelColumn);
            this.groupBoxFirstLabel.Controls.Add(this.labelFirstLabelColumn);
            this.groupBoxFirstLabel.Controls.Add(this.labelPositionControl);
            this.groupBoxFirstLabel.Location = new System.Drawing.Point(6, 218);
            this.groupBoxFirstLabel.Name = "groupBoxFirstLabel";
            this.groupBoxFirstLabel.Size = new System.Drawing.Size(312, 240);
            this.groupBoxFirstLabel.TabIndex = 1;
            this.groupBoxFirstLabel.TabStop = false;
            this.groupBoxFirstLabel.Text = "First Label";
            // 
            // firstLabelRow
            // 
            this.firstLabelRow.Enabled = false;
            this.firstLabelRow.Location = new System.Drawing.Point(67, 23);
            this.firstLabelRow.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.firstLabelRow.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.firstLabelRow.Name = "firstLabelRow";
            this.firstLabelRow.Size = new System.Drawing.Size(56, 21);
            this.firstLabelRow.TabIndex = 5;
            this.firstLabelRow.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.firstLabelRow.ValueChanged += new System.EventHandler(this.OnLabelNumericPositionChanged);
            // 
            // labelFirstLabelRow
            // 
            this.labelFirstLabelRow.AutoSize = true;
            this.labelFirstLabelRow.Location = new System.Drawing.Point(29, 25);
            this.labelFirstLabelRow.Name = "labelFirstLabelRow";
            this.labelFirstLabelRow.Size = new System.Drawing.Size(32, 13);
            this.labelFirstLabelRow.TabIndex = 4;
            this.labelFirstLabelRow.Text = "Row:";
            // 
            // firstLabelColumn
            // 
            this.firstLabelColumn.Enabled = false;
            this.firstLabelColumn.Location = new System.Drawing.Point(67, 49);
            this.firstLabelColumn.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.firstLabelColumn.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.firstLabelColumn.Name = "firstLabelColumn";
            this.firstLabelColumn.Size = new System.Drawing.Size(56, 21);
            this.firstLabelColumn.TabIndex = 3;
            this.firstLabelColumn.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.firstLabelColumn.ValueChanged += new System.EventHandler(this.OnLabelNumericPositionChanged);
            // 
            // labelFirstLabelColumn
            // 
            this.labelFirstLabelColumn.AutoSize = true;
            this.labelFirstLabelColumn.Location = new System.Drawing.Point(18, 52);
            this.labelFirstLabelColumn.Name = "labelFirstLabelColumn";
            this.labelFirstLabelColumn.Size = new System.Drawing.Size(46, 13);
            this.labelFirstLabelColumn.TabIndex = 2;
            this.labelFirstLabelColumn.Text = "Column:";
            // 
            // labelPositionControl
            // 
            this.labelPositionControl.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPositionControl.Location = new System.Drawing.Point(129, 23);
            this.labelPositionControl.Name = "labelPositionControl";
            this.labelPositionControl.Size = new System.Drawing.Size(170, 200);
            this.labelPositionControl.TabIndex = 0;
            this.labelPositionControl.LabelPositionChanged += new System.EventHandler(this.OnLabelChooserPositionChanged);
            // 
            // groupBoxCopies
            // 
            this.groupBoxCopies.Controls.Add(this.copiesControl);
            this.groupBoxCopies.Location = new System.Drawing.Point(6, 112);
            this.groupBoxCopies.Name = "groupBoxCopies";
            this.groupBoxCopies.Size = new System.Drawing.Size(312, 100);
            this.groupBoxCopies.TabIndex = 0;
            this.groupBoxCopies.TabStop = false;
            this.groupBoxCopies.Text = "Copies";
            // 
            // copiesControl
            // 
            this.copiesControl.Collate = false;
            this.copiesControl.Copies = 1;
            this.copiesControl.Location = new System.Drawing.Point(5, 19);
            this.copiesControl.Name = "copiesControl";
            this.copiesControl.Size = new System.Drawing.Size(177, 78);
            this.copiesControl.TabIndex = 0;
            // 
            // groupBoxPrinter
            // 
            this.groupBoxPrinter.Controls.Add(this.printerControl);
            this.groupBoxPrinter.Location = new System.Drawing.Point(6, -3);
            this.groupBoxPrinter.Name = "groupBoxPrinter";
            this.groupBoxPrinter.Size = new System.Drawing.Size(312, 109);
            this.groupBoxPrinter.TabIndex = 2;
            this.groupBoxPrinter.TabStop = false;
            this.groupBoxPrinter.Text = "Printer";
            // 
            // printerControl
            // 
            this.printerControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.printerControl.Location = new System.Drawing.Point(6, 20);
            this.printerControl.Name = "printerControl";
            this.printerControl.ShowPrinterCalibration = true;
            this.printerControl.Size = new System.Drawing.Size(293, 88);
            this.printerControl.TabIndex = 0;
            // 
            // optionPagePaper
            // 
            this.optionPagePaper.Controls.Add(this.pageSetupControl);
            this.optionPagePaper.Location = new System.Drawing.Point(153, 0);
            this.optionPagePaper.Name = "optionPagePaper";
            this.optionPagePaper.Padding = new System.Windows.Forms.Padding(3);
            this.optionPagePaper.Size = new System.Drawing.Size(331, 458);
            this.optionPagePaper.TabIndex = 1;
            this.optionPagePaper.Text = "Page Setup";
            // 
            // pageSetupControl
            // 
            this.pageSetupControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.pageSetupControl.Location = new System.Drawing.Point(3, -6);
            this.pageSetupControl.MarginBottom = 0.75;
            this.pageSetupControl.MarginLeft = 0.65;
            this.pageSetupControl.MarginRight = 0.75;
            this.pageSetupControl.MarginTop = 0.65;
            this.pageSetupControl.Name = "pageSetupControl";
            this.pageSetupControl.PaperHeight = 11;
            this.pageSetupControl.PaperWidth = 8.5;
            this.pageSetupControl.Size = new System.Drawing.Size(333, 192);
            this.pageSetupControl.TabIndex = 0;
            // 
            // optionPageLabels
            // 
            this.optionPageLabels.Controls.Add(this.labelSheetControl);
            this.optionPageLabels.Location = new System.Drawing.Point(153, 0);
            this.optionPageLabels.Name = "optionPageLabels";
            this.optionPageLabels.Size = new System.Drawing.Size(331, 458);
            this.optionPageLabels.TabIndex = 3;
            this.optionPageLabels.Text = "Label Sheet";
            // 
            // labelSheetControl
            // 
            this.labelSheetControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSheetControl.Location = new System.Drawing.Point(3, -6);
            this.labelSheetControl.Name = "labelSheetControl";
            this.labelSheetControl.Size = new System.Drawing.Size(330, 227);
            this.labelSheetControl.TabIndex = 0;
            this.labelSheetControl.LabelSheetChanged += new System.EventHandler(this.OnLabelSheetChanged);
            // 
            // PrintJobSettingsDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(510, 509);
            this.Controls.Add(this.optionControl);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintJobSettingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Print Setup";
            this.Load += new System.EventHandler(this.OnLoad);
            this.optionControl.ResumeLayout(false);
            this.optionPagePrinting.ResumeLayout(false);
            this.groupBoxFirstLabel.ResumeLayout(false);
            this.groupBoxFirstLabel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) (this.firstLabelRow)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.firstLabelColumn)).EndInit();
            this.groupBoxCopies.ResumeLayout(false);
            this.groupBoxPrinter.ResumeLayout(false);
            this.optionPagePaper.ResumeLayout(false);
            this.optionPageLabels.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupBoxPrinter;
        private ShipWorks.Templates.Media.PrinterSelectionControl printerControl;
        private ShipWorks.UI.Controls.OptionControl optionControl;
        private ShipWorks.UI.Controls.OptionPage optionPagePaper;
        private ShipWorks.UI.Controls.OptionPage optionPagePrinting;
        private ShipWorks.UI.Controls.OptionPage optionPageLabels;
        private System.Windows.Forms.GroupBox groupBoxCopies;
        private ShipWorks.Templates.Media.PageSetupControl pageSetupControl;
        private ShipWorks.Templates.Media.LabelSheetControl labelSheetControl;
        private System.Windows.Forms.GroupBox groupBoxFirstLabel;
        private ShipWorks.Templates.Media.LabelPositionControl labelPositionControl;
        private System.Windows.Forms.NumericUpDown firstLabelRow;
        private System.Windows.Forms.Label labelFirstLabelRow;
        private System.Windows.Forms.NumericUpDown firstLabelColumn;
        private System.Windows.Forms.Label labelFirstLabelColumn;
        private ShipWorks.Templates.Media.PageCopiesControl copiesControl;
    }
}