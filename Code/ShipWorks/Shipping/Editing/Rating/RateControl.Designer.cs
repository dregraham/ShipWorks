using System.Windows.Forms;
using Divelements.SandGrid;

namespace ShipWorks.Shipping.Editing.Rating
{
    partial class RateControl
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RateControl));
            this.rateGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnProvider = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnService = new Divelements.SandGrid.GridColumn();
            this.gridColumnDays = new Divelements.SandGrid.GridColumn();
            this.gridColumnShipping = new Divelements.SandGrid.GridColumn();
            this.gridColumnTax = new Divelements.SandGrid.GridColumn();
            this.gridColumnDuty = new Divelements.SandGrid.GridColumn();
            this.gridColumnRate = new Divelements.SandGrid.GridColumn();
            this.gridColumnSelect = new Divelements.SandGrid.Specialized.GridHyperlinkColumn();
            this.panelFootnote = new System.Windows.Forms.Panel();
            this.loadingRatesLabel = new System.Windows.Forms.Label();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
            this.SuspendLayout();
            // 
            // rateGrid
            // 
            this.rateGrid.AllowMultipleSelection = false;
            this.rateGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rateGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnProvider,
            this.gridColumnService,
            this.gridColumnDays,
            this.gridColumnShipping,
            this.gridColumnTax,
            this.gridColumnDuty,
            this.gridColumnRate,
            this.gridColumnSelect});
            this.rateGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rateGrid.EmptyTextForeColor = System.Drawing.Color.DimGray;
            this.rateGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.rateGrid.ImageTextSeparation = 1;
            this.rateGrid.Location = new System.Drawing.Point(0, 0);
            this.rateGrid.Name = "rateGrid";
            this.rateGrid.NullRepresentation = "";
            this.rateGrid.PrimaryColumn = this.gridColumnService;
            this.rateGrid.Renderer = windowsXPRenderer1;
            this.rateGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.rateGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.rateGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("USPS"),
                        new Divelements.SandGrid.GridCell("First Class"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        300,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        100,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        45,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        445,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("USPS"),
                        new Divelements.SandGrid.GridCell("     Delivery Confirmation (+$.045)"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        250,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        110,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        130,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        490,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("USPS"),
                        new Divelements.SandGrid.GridCell("     Signature Confirmation (+$0.85)"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        410,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        60,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        60,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        530,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("USPS"),
                        new Divelements.SandGrid.GridCell("Priority"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        635,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        200,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        150,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        985,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))})});
            this.rateGrid.ShadeAlternateRows = true;
            this.rateGrid.Size = new System.Drawing.Size(493, 94);
            this.rateGrid.TabIndex = 1;
            this.rateGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnSelectedRateChanged);
            // 
            // gridColumnProvider
            // 
            this.gridColumnProvider.AllowEditing = false;
            this.gridColumnProvider.AllowReorder = false;
            this.gridColumnProvider.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnProvider.AutoSizeIncludeHeader = true;
            this.gridColumnProvider.CellHorizontalAlignment = System.Drawing.StringAlignment.Center;
            this.gridColumnProvider.Clickable = false;
            this.gridColumnProvider.HeaderText = "Provider";
            this.gridColumnProvider.MinimumWidth = 50;
            this.gridColumnProvider.Width = 75;
            // 
            // gridColumnService
            // 
            this.gridColumnService.AllowEditing = false;
            this.gridColumnService.AllowReorder = false;
            this.gridColumnService.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnService.Clickable = false;
            this.gridColumnService.HeaderText = "Service";
            this.gridColumnService.Width = 175;
            // 
            // gridColumnDays
            // 
            this.gridColumnDays.AllowEditing = false;
            this.gridColumnDays.AllowReorder = false;
            this.gridColumnDays.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnDays.AutoSizeIncludeHeader = true;
            this.gridColumnDays.Clickable = false;
            this.gridColumnDays.HeaderText = "Days";
            this.gridColumnDays.Width = 75;
            // 
            // gridColumnShipping
            // 
            this.gridColumnShipping.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnShipping.AutoSizeIncludeHeader = true;
            this.gridColumnShipping.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnShipping.Clickable = false;
            this.gridColumnShipping.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumnShipping.HeaderText = "Shipping";
            this.gridColumnShipping.MinimumWidth = 20;
            this.gridColumnShipping.Width = 50;
            // 
            // gridColumnTax
            // 
            this.gridColumnTax.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnTax.AutoSizeIncludeHeader = true;
            this.gridColumnTax.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnTax.Clickable = false;
            this.gridColumnTax.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumnTax.HeaderText = "Tax";
            this.gridColumnTax.MinimumWidth = 20;
            this.gridColumnTax.Width = 40;
            // 
            // gridColumnDuty
            // 
            this.gridColumnDuty.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnDuty.AutoSizeIncludeHeader = true;
            this.gridColumnDuty.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnDuty.Clickable = false;
            this.gridColumnDuty.ForeColor = System.Drawing.SystemColors.GrayText;
            this.gridColumnDuty.HeaderText = "Duty";
            this.gridColumnDuty.MinimumWidth = 20;
            this.gridColumnDuty.Width = 40;
            // 
            // gridColumnRate
            // 
            this.gridColumnRate.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnRate.AutoSizeIncludeHeader = true;
            this.gridColumnRate.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnRate.Clickable = false;
            this.gridColumnRate.HeaderText = "Rate";
            this.gridColumnRate.MinimumWidth = 50;
            this.gridColumnRate.Width = 50;
            // 
            // gridColumnSelect
            // 
            this.gridColumnSelect.AllowEditing = false;
            this.gridColumnSelect.AllowReorder = false;
            this.gridColumnSelect.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnSelect.AutoSizeIncludeHeader = true;
            this.gridColumnSelect.Clickable = false;
            this.gridColumnSelect.MinimumWidth = 50;
            this.gridColumnSelect.Width = 75;
            // 
            // panelFootnote
            // 
            this.panelFootnote.BackColor = System.Drawing.Color.White;
            this.panelFootnote.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFootnote.Location = new System.Drawing.Point(0, 94);
            this.panelFootnote.Name = "panelFootnote";
            this.panelFootnote.Size = new System.Drawing.Size(493, 34);
            this.panelFootnote.TabIndex = 8;
            // 
            // loadingRatesLabel
            // 
            this.loadingRatesLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadingRatesLabel.AutoSize = true;
            this.loadingRatesLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.loadingRatesLabel.Location = new System.Drawing.Point(202, 78);
            this.loadingRatesLabel.Name = "loadingRatesLabel";
            this.loadingRatesLabel.Size = new System.Drawing.Size(90, 13);
            this.loadingRatesLabel.TabIndex = 12;
            this.loadingRatesLabel.Text = "Checking rates...";
            this.loadingRatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loadingImage
            // 
            this.loadingImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadingImage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.loadingImage.Image = ((System.Drawing.Image)(resources.GetObject("loadingImage.Image")));
            this.loadingImage.Location = new System.Drawing.Point(227, 37);
            this.loadingImage.Name = "loadingImage";
            this.loadingImage.Size = new System.Drawing.Size(39, 40);
            this.loadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.loadingImage.TabIndex = 11;
            this.loadingImage.TabStop = false;
            // 
            // RateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadingRatesLabel);
            this.Controls.Add(this.loadingImage);
            this.Controls.Add(this.rateGrid);
            this.Controls.Add(this.panelFootnote);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RateControl";
            this.Size = new System.Drawing.Size(493, 128);
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Divelements.SandGrid.SandGrid rateGrid;
        private Divelements.SandGrid.GridColumn gridColumnService;
        private Divelements.SandGrid.GridColumn gridColumnDays;
        private Divelements.SandGrid.Specialized.GridHyperlinkColumn gridColumnSelect;
        private Divelements.SandGrid.GridColumn gridColumnRate;
        private System.Windows.Forms.Panel panelFootnote;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnProvider;
        private Label loadingRatesLabel;
        private PictureBox loadingImage;
        private GridColumn gridColumnShipping;
        private GridColumn gridColumnTax;
        private GridColumn gridColumnDuty;
    }
}
