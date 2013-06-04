namespace ShipWorks.Templates.Media
{
    partial class LabelSheetEditorDlg
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
            this.components = new System.ComponentModel.Container();
            this.ok = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.groupSheetSize = new System.Windows.Forms.GroupBox();
            this.groupLabelDimensions = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.NumericUpDown();
            this.height = new System.Windows.Forms.NumericUpDown();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelHeight = new System.Windows.Forms.Label();
            this.groupLabelSpacing = new System.Windows.Forms.GroupBox();
            this.labelVertical = new System.Windows.Forms.Label();
            this.labelHorizontal = new System.Windows.Forms.Label();
            this.spacingVertical = new System.Windows.Forms.NumericUpDown();
            this.spacingHorizontal = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.groupLabelCount = new System.Windows.Forms.GroupBox();
            this.rows = new System.Windows.Forms.NumericUpDown();
            this.columns = new System.Windows.Forms.NumericUpDown();
            this.labelDown = new System.Windows.Forms.Label();
            this.labelAccross = new System.Windows.Forms.Label();
            this.groupPageMargins = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.marginLeft = new System.Windows.Forms.NumericUpDown();
            this.marginTop = new System.Windows.Forms.NumericUpDown();
            this.labelLeft = new System.Windows.Forms.Label();
            this.labelTop = new System.Windows.Forms.Label();
            this.labelLabelName = new System.Windows.Forms.Label();
            this.sheetName = new System.Windows.Forms.TextBox();
            this.paperDimensions = new ShipWorks.Templates.Media.PaperDimensionsControl();
            this.fieldLengthProvider = new ShipWorks.Data.Utility.EntityFieldLengthProvider(this.components);
            this.groupSheetSize.SuspendLayout();
            this.groupLabelDimensions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.width)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.height)).BeginInit();
            this.groupLabelSpacing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.spacingVertical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.spacingHorizontal)).BeginInit();
            this.groupLabelCount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.rows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.columns)).BeginInit();
            this.groupPageMargins.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) (this.marginLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.marginTop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // ok
            // 
            this.ok.Location = new System.Drawing.Point(185, 320);
            this.ok.Name = "ok";
            this.ok.Size = new System.Drawing.Size(75, 23);
            this.ok.TabIndex = 7;
            this.ok.Text = "OK";
            this.ok.UseVisualStyleBackColor = true;
            this.ok.Click += new System.EventHandler(this.OnOK);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(266, 320);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 8;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // groupSheetSize
            // 
            this.groupSheetSize.Controls.Add(this.paperDimensions);
            this.groupSheetSize.Location = new System.Drawing.Point(11, 39);
            this.groupSheetSize.Name = "groupSheetSize";
            this.groupSheetSize.Size = new System.Drawing.Size(330, 82);
            this.groupSheetSize.TabIndex = 2;
            this.groupSheetSize.TabStop = false;
            this.groupSheetSize.Text = "Sheet Size";
            // 
            // groupLabelDimensions
            // 
            this.groupLabelDimensions.Controls.Add(this.label19);
            this.groupLabelDimensions.Controls.Add(this.label20);
            this.groupLabelDimensions.Controls.Add(this.width);
            this.groupLabelDimensions.Controls.Add(this.height);
            this.groupLabelDimensions.Controls.Add(this.labelWidth);
            this.groupLabelDimensions.Controls.Add(this.labelHeight);
            this.groupLabelDimensions.Location = new System.Drawing.Point(181, 129);
            this.groupLabelDimensions.Name = "groupLabelDimensions";
            this.groupLabelDimensions.Size = new System.Drawing.Size(160, 82);
            this.groupLabelDimensions.TabIndex = 4;
            this.groupLabelDimensions.TabStop = false;
            this.groupLabelDimensions.Text = "Label Dimensions";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(138, 52);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(14, 18);
            this.label19.TabIndex = 5;
            this.label19.Text = "\"";
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(138, 26);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(14, 18);
            this.label20.TabIndex = 2;
            this.label20.Text = "\"";
            // 
            // width
            // 
            this.width.DecimalPlaces = 2;
            this.width.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.width.Location = new System.Drawing.Point(84, 48);
            this.width.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.width.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.width.Name = "width";
            this.width.Size = new System.Drawing.Size(54, 21);
            this.width.TabIndex = 4;
            this.width.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // height
            // 
            this.height.DecimalPlaces = 2;
            this.height.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.height.Location = new System.Drawing.Point(84, 22);
            this.height.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.height.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.height.Name = "height";
            this.height.Size = new System.Drawing.Size(54, 21);
            this.height.TabIndex = 1;
            this.height.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // labelWidth
            // 
            this.labelWidth.Location = new System.Drawing.Point(12, 50);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(70, 14);
            this.labelWidth.TabIndex = 3;
            this.labelWidth.Text = "Width:";
            // 
            // labelHeight
            // 
            this.labelHeight.Location = new System.Drawing.Point(12, 24);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(74, 18);
            this.labelHeight.TabIndex = 0;
            this.labelHeight.Text = "Height:";
            // 
            // groupLabelSpacing
            // 
            this.groupLabelSpacing.Controls.Add(this.labelVertical);
            this.groupLabelSpacing.Controls.Add(this.labelHorizontal);
            this.groupLabelSpacing.Controls.Add(this.spacingVertical);
            this.groupLabelSpacing.Controls.Add(this.spacingHorizontal);
            this.groupLabelSpacing.Controls.Add(this.label10);
            this.groupLabelSpacing.Controls.Add(this.label9);
            this.groupLabelSpacing.Location = new System.Drawing.Point(11, 223);
            this.groupLabelSpacing.Name = "groupLabelSpacing";
            this.groupLabelSpacing.Size = new System.Drawing.Size(160, 82);
            this.groupLabelSpacing.TabIndex = 5;
            this.groupLabelSpacing.TabStop = false;
            this.groupLabelSpacing.Text = "Label Spacing";
            // 
            // labelVertical
            // 
            this.labelVertical.Location = new System.Drawing.Point(12, 24);
            this.labelVertical.Name = "labelVertical";
            this.labelVertical.Size = new System.Drawing.Size(58, 18);
            this.labelVertical.TabIndex = 0;
            this.labelVertical.Text = "Vertical:";
            // 
            // labelHorizontal
            // 
            this.labelHorizontal.Location = new System.Drawing.Point(12, 50);
            this.labelHorizontal.Name = "labelHorizontal";
            this.labelHorizontal.Size = new System.Drawing.Size(62, 14);
            this.labelHorizontal.TabIndex = 3;
            this.labelHorizontal.Text = "Horizontal:";
            // 
            // spacingVertical
            // 
            this.spacingVertical.DecimalPlaces = 2;
            this.spacingVertical.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.spacingVertical.Location = new System.Drawing.Point(84, 24);
            this.spacingVertical.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spacingVertical.Name = "spacingVertical";
            this.spacingVertical.Size = new System.Drawing.Size(54, 21);
            this.spacingVertical.TabIndex = 1;
            // 
            // spacingHorizontal
            // 
            this.spacingHorizontal.DecimalPlaces = 2;
            this.spacingHorizontal.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.spacingHorizontal.Location = new System.Drawing.Point(84, 50);
            this.spacingHorizontal.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.spacingHorizontal.Name = "spacingHorizontal";
            this.spacingHorizontal.Size = new System.Drawing.Size(54, 21);
            this.spacingHorizontal.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(138, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(10, 18);
            this.label10.TabIndex = 2;
            this.label10.Text = "\"";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(138, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 18);
            this.label9.TabIndex = 5;
            this.label9.Text = "\"";
            // 
            // groupLabelCount
            // 
            this.groupLabelCount.Controls.Add(this.rows);
            this.groupLabelCount.Controls.Add(this.columns);
            this.groupLabelCount.Controls.Add(this.labelDown);
            this.groupLabelCount.Controls.Add(this.labelAccross);
            this.groupLabelCount.Location = new System.Drawing.Point(181, 223);
            this.groupLabelCount.Name = "groupLabelCount";
            this.groupLabelCount.Size = new System.Drawing.Size(160, 82);
            this.groupLabelCount.TabIndex = 6;
            this.groupLabelCount.TabStop = false;
            this.groupLabelCount.Text = "Label Count";
            // 
            // rows
            // 
            this.rows.Location = new System.Drawing.Point(84, 48);
            this.rows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.rows.Name = "rows";
            this.rows.Size = new System.Drawing.Size(54, 21);
            this.rows.TabIndex = 3;
            this.rows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // columns
            // 
            this.columns.Location = new System.Drawing.Point(84, 22);
            this.columns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.columns.Name = "columns";
            this.columns.Size = new System.Drawing.Size(54, 21);
            this.columns.TabIndex = 1;
            this.columns.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // labelDown
            // 
            this.labelDown.Location = new System.Drawing.Point(12, 50);
            this.labelDown.Name = "labelDown";
            this.labelDown.Size = new System.Drawing.Size(48, 14);
            this.labelDown.TabIndex = 2;
            this.labelDown.Text = "Down:";
            // 
            // labelAccross
            // 
            this.labelAccross.Location = new System.Drawing.Point(12, 24);
            this.labelAccross.Name = "labelAccross";
            this.labelAccross.Size = new System.Drawing.Size(74, 18);
            this.labelAccross.TabIndex = 0;
            this.labelAccross.Text = "Across:";
            // 
            // groupPageMargins
            // 
            this.groupPageMargins.Controls.Add(this.label23);
            this.groupPageMargins.Controls.Add(this.label24);
            this.groupPageMargins.Controls.Add(this.marginLeft);
            this.groupPageMargins.Controls.Add(this.marginTop);
            this.groupPageMargins.Controls.Add(this.labelLeft);
            this.groupPageMargins.Controls.Add(this.labelTop);
            this.groupPageMargins.Location = new System.Drawing.Point(11, 129);
            this.groupPageMargins.Name = "groupPageMargins";
            this.groupPageMargins.Size = new System.Drawing.Size(160, 82);
            this.groupPageMargins.TabIndex = 3;
            this.groupPageMargins.TabStop = false;
            this.groupPageMargins.Text = "Page Margins";
            // 
            // label23
            // 
            this.label23.Location = new System.Drawing.Point(138, 50);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(14, 18);
            this.label23.TabIndex = 11;
            this.label23.Text = "\"";
            // 
            // label24
            // 
            this.label24.Location = new System.Drawing.Point(138, 26);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(14, 18);
            this.label24.TabIndex = 10;
            this.label24.Text = "\"";
            // 
            // marginLeft
            // 
            this.marginLeft.DecimalPlaces = 2;
            this.marginLeft.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.marginLeft.Location = new System.Drawing.Point(84, 48);
            this.marginLeft.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.marginLeft.Name = "marginLeft";
            this.marginLeft.Size = new System.Drawing.Size(54, 21);
            this.marginLeft.TabIndex = 3;
            // 
            // marginTop
            // 
            this.marginTop.DecimalPlaces = 2;
            this.marginTop.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.marginTop.Location = new System.Drawing.Point(84, 22);
            this.marginTop.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.marginTop.Name = "marginTop";
            this.marginTop.Size = new System.Drawing.Size(54, 21);
            this.marginTop.TabIndex = 1;
            // 
            // labelLeft
            // 
            this.labelLeft.Location = new System.Drawing.Point(12, 50);
            this.labelLeft.Name = "labelLeft";
            this.labelLeft.Size = new System.Drawing.Size(36, 14);
            this.labelLeft.TabIndex = 2;
            this.labelLeft.Text = "Left:";
            // 
            // labelTop
            // 
            this.labelTop.Location = new System.Drawing.Point(12, 24);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(32, 18);
            this.labelTop.TabIndex = 0;
            this.labelTop.Text = "Top:";
            // 
            // labelLabelName
            // 
            this.labelLabelName.Location = new System.Drawing.Point(11, 12);
            this.labelLabelName.Name = "labelLabelName";
            this.labelLabelName.Size = new System.Drawing.Size(70, 14);
            this.labelLabelName.TabIndex = 0;
            this.labelLabelName.Text = "Sheet name:";
            // 
            // sheetName
            // 
            this.sheetName.Location = new System.Drawing.Point(80, 9);
            this.fieldLengthProvider.SetMaxLengthSource(this.sheetName, ShipWorks.Data.Utility.EntityFieldLengthSource.LabelSheetName);
            this.sheetName.Name = "sheetName";
            this.sheetName.Size = new System.Drawing.Size(184, 21);
            this.sheetName.TabIndex = 1;
            // 
            // paperDimensions
            // 
            this.paperDimensions.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.paperDimensions.Location = new System.Drawing.Point(10, 18);
            this.paperDimensions.Name = "paperDimensions";
            this.paperDimensions.PaperHeight = 11;
            this.paperDimensions.PaperWidth = 8.5;
            this.paperDimensions.Size = new System.Drawing.Size(312, 63);
            this.paperDimensions.TabIndex = 0;
            // 
            // LabelSheetEditorDlg
            // 
            this.AcceptButton = this.ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(353, 356);
            this.Controls.Add(this.sheetName);
            this.Controls.Add(this.labelLabelName);
            this.Controls.Add(this.groupSheetSize);
            this.Controls.Add(this.groupLabelDimensions);
            this.Controls.Add(this.groupLabelSpacing);
            this.Controls.Add(this.groupLabelCount);
            this.Controls.Add(this.groupPageMargins);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.ok);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LabelSheetEditorDlg";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom Label Sheet";
            this.Load += new System.EventHandler(this.OnLoad);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.groupSheetSize.ResumeLayout(false);
            this.groupLabelDimensions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.width)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.height)).EndInit();
            this.groupLabelSpacing.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.spacingVertical)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.spacingHorizontal)).EndInit();
            this.groupLabelCount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.rows)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.columns)).EndInit();
            this.groupPageMargins.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) (this.marginLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.marginTop)).EndInit();
            ((System.ComponentModel.ISupportInitialize) (this.fieldLengthProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ok;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.GroupBox groupSheetSize;
        private System.Windows.Forms.GroupBox groupLabelDimensions;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown width;
        private System.Windows.Forms.NumericUpDown height;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.GroupBox groupLabelSpacing;
        private System.Windows.Forms.Label labelVertical;
        private System.Windows.Forms.Label labelHorizontal;
        private System.Windows.Forms.NumericUpDown spacingVertical;
        private System.Windows.Forms.NumericUpDown spacingHorizontal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupLabelCount;
        private System.Windows.Forms.NumericUpDown rows;
        private System.Windows.Forms.NumericUpDown columns;
        private System.Windows.Forms.Label labelDown;
        private System.Windows.Forms.Label labelAccross;
        private System.Windows.Forms.GroupBox groupPageMargins;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.NumericUpDown marginLeft;
        private System.Windows.Forms.NumericUpDown marginTop;
        private System.Windows.Forms.Label labelLeft;
        private System.Windows.Forms.Label labelTop;
        private System.Windows.Forms.Label labelLabelName;
        private System.Windows.Forms.TextBox sheetName;
        private PaperDimensionsControl paperDimensions;
        private ShipWorks.Data.Utility.EntityFieldLengthProvider fieldLengthProvider;
    }
}