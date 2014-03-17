using System.Windows.Forms;

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
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnProvider = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnService = new Divelements.SandGrid.GridColumn();
            this.gridColumnDays = new Divelements.SandGrid.GridColumn();
            this.gridColumnRate = new Divelements.SandGrid.GridColumn();
            this.gridColumnSelect = new Divelements.SandGrid.Specialized.GridHyperlinkColumn();
            this.panelFootnote = new System.Windows.Forms.Panel();
            this.loadingRatesLabel = new System.Windows.Forms.Label();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
            this.SuspendLayout();
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnProvider,
            this.gridColumnService,
            this.gridColumnDays,
            this.gridColumnRate,
            this.gridColumnSelect});
            this.sandGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sandGrid.EmptyTextForeColor = System.Drawing.Color.DimGray;
            this.sandGrid.ImageTextSeparation = 1;
            this.sandGrid.Location = new System.Drawing.Point(0, 0);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.PrimaryColumn = this.gridColumnService;
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.sandGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("USPS"),
                        new Divelements.SandGrid.GridCell("First Class"),
                        new Divelements.SandGrid.GridCell(),
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
                                        985,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))})});
            this.sandGrid.ShadeAlternateRows = true;
            this.sandGrid.Size = new System.Drawing.Size(493, 175);
            this.sandGrid.TabIndex = 1;
            this.sandGrid.ColumnResized += new Divelements.SandGrid.GridColumnEventHandler(this.OnColumnResized);
            this.sandGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnSelectedRateChanged);
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
            this.gridColumnService.Clickable = false;
            this.gridColumnService.HeaderText = "Service";
            this.gridColumnService.Width = 175;
            // 
            // gridColumnDays
            // 
            this.gridColumnDays.AllowEditing = false;
            this.gridColumnDays.AllowReorder = false;
            this.gridColumnDays.AutoSizeIncludeHeader = true;
            this.gridColumnDays.Clickable = false;
            this.gridColumnDays.HeaderText = "Days";
            this.gridColumnDays.Width = 75;
            // 
            // gridColumnRate
            // 
            this.gridColumnRate.AutoSizeIncludeHeader = true;
            this.gridColumnRate.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnRate.Clickable = false;
            this.gridColumnRate.HeaderText = "Rate";
            this.gridColumnRate.Width = 50;
            // 
            // gridColumnSelect
            // 
            this.gridColumnSelect.AllowEditing = false;
            this.gridColumnSelect.AllowReorder = false;
            this.gridColumnSelect.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnSelect.AutoSizeIncludeHeader = true;
            this.gridColumnSelect.Clickable = false;
            this.gridColumnSelect.Width = 0;
            // 
            // panelFootnote
            // 
            this.panelFootnote.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelFootnote.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFootnote.Location = new System.Drawing.Point(0, 175);
            this.panelFootnote.Name = "panelFootnote";
            this.panelFootnote.Size = new System.Drawing.Size(493, 30);
            this.panelFootnote.TabIndex = 8;
            // 
            // loadingRatesLabel
            // 
            this.loadingRatesLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadingRatesLabel.AutoSize = true;
            this.loadingRatesLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.loadingRatesLabel.Location = new System.Drawing.Point(202, 116);
            this.loadingRatesLabel.Name = "loadingRatesLabel";
            this.loadingRatesLabel.Size = new System.Drawing.Size(88, 13);
            this.loadingRatesLabel.TabIndex = 12;
            this.loadingRatesLabel.Text = "Fetching rates...";
            this.loadingRatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // loadingImage
            // 
            this.loadingImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadingImage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.loadingImage.Image = ((System.Drawing.Image)(resources.GetObject("loadingImage.Image")));
            this.loadingImage.Location = new System.Drawing.Point(227, 75);
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
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.panelFootnote);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RateControl";
            this.Size = new System.Drawing.Size(493, 205);
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnService;
        private Divelements.SandGrid.GridColumn gridColumnDays;
        private Divelements.SandGrid.Specialized.GridHyperlinkColumn gridColumnSelect;
        private Divelements.SandGrid.GridColumn gridColumnRate;
        private System.Windows.Forms.Panel panelFootnote;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnProvider;
        private Label loadingRatesLabel;
        private PictureBox loadingImage;
    }
}
