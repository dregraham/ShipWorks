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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer2 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnProvider = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnService = new Divelements.SandGrid.GridColumn();
            this.gridColumnDays = new Divelements.SandGrid.GridColumn();
            this.gridColumnRate = new Divelements.SandGrid.GridColumn();
            this.gridColumnSelect = new Divelements.SandGrid.Specialized.GridHyperlinkColumn();
            this.statusPanel = new System.Windows.Forms.Panel();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panelFootnote = new System.Windows.Forms.Panel();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            this.loadingRatesPanel = new System.Windows.Forms.Panel();
            this.loadingRatesLabel = new System.Windows.Forms.Label();
            this.statusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).BeginInit();
            this.loadingRatesPanel.SuspendLayout();
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
            this.sandGrid.Location = new System.Drawing.Point(0, 30);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.PrimaryColumn = this.gridColumnService;
            this.sandGrid.Renderer = windowsXPRenderer2;
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
            this.sandGrid.Size = new System.Drawing.Size(493, 145);
            this.sandGrid.TabIndex = 1;
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
            this.gridColumnService.Width = 393;
            // 
            // gridColumnDays
            // 
            this.gridColumnDays.AllowEditing = false;
            this.gridColumnDays.AllowReorder = false;
            this.gridColumnDays.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnDays.AutoSizeIncludeHeader = true;
            this.gridColumnDays.Clickable = false;
            this.gridColumnDays.HeaderText = "Days";
            this.gridColumnDays.Width = 32;
            // 
            // gridColumnRate
            // 
            this.gridColumnRate.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnRate.AutoSizeIncludeHeader = true;
            this.gridColumnRate.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnRate.Clickable = false;
            this.gridColumnRate.HeaderText = "Rate";
            this.gridColumnRate.Width = 31;
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
            // statusPanel
            // 
            this.statusPanel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusPanel.Controls.Add(this.labelStatus);
            this.statusPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.statusPanel.Location = new System.Drawing.Point(0, 0);
            this.statusPanel.Name = "statusPanel";
            this.statusPanel.Size = new System.Drawing.Size(493, 30);
            this.statusPanel.TabIndex = 7;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(4, 9);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(88, 13);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "Fetching rates...";
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
            // loadingImage
            // 
            this.loadingImage.BackColor = System.Drawing.Color.White;
            this.loadingImage.Image = global::ShipWorks.Properties.Resources.squares_circle_green;
            this.loadingImage.Location = new System.Drawing.Point(28, 3);
            this.loadingImage.Name = "loadingImage";
            this.loadingImage.Size = new System.Drawing.Size(39, 40);
            this.loadingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.loadingImage.TabIndex = 9;
            this.loadingImage.TabStop = false;
            // 
            // loadingRatesPanel
            // 
            this.loadingRatesPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loadingRatesPanel.AutoSize = true;
            this.loadingRatesPanel.BackColor = System.Drawing.Color.White;
            this.loadingRatesPanel.Controls.Add(this.loadingRatesLabel);
            this.loadingRatesPanel.Controls.Add(this.loadingImage);
            this.loadingRatesPanel.Location = new System.Drawing.Point(199, 60);
            this.loadingRatesPanel.Name = "loadingRatesPanel";
            this.loadingRatesPanel.Size = new System.Drawing.Size(95, 87);
            this.loadingRatesPanel.TabIndex = 10;
            // 
            // loadingRatesLabel
            // 
            this.loadingRatesLabel.AutoSize = true;
            this.loadingRatesLabel.Location = new System.Drawing.Point(3, 44);
            this.loadingRatesLabel.Name = "loadingRatesLabel";
            this.loadingRatesLabel.Size = new System.Drawing.Size(88, 13);
            this.loadingRatesLabel.TabIndex = 10;
            this.loadingRatesLabel.Text = "Fetching rates...";
            this.loadingRatesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RateControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loadingRatesPanel);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.statusPanel);
            this.Controls.Add(this.panelFootnote);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RateControl";
            this.Size = new System.Drawing.Size(493, 205);
            this.statusPanel.ResumeLayout(false);
            this.statusPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.loadingImage)).EndInit();
            this.loadingRatesPanel.ResumeLayout(false);
            this.loadingRatesPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnService;
        private Divelements.SandGrid.GridColumn gridColumnDays;
        private Divelements.SandGrid.Specialized.GridHyperlinkColumn gridColumnSelect;
        private Divelements.SandGrid.GridColumn gridColumnRate;
        private System.Windows.Forms.Panel statusPanel;
        private System.Windows.Forms.Panel panelFootnote;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnProvider;
        private PictureBox loadingImage;
        private Panel loadingRatesPanel;
        private Label loadingRatesLabel;
        private Label labelStatus;
    }
}
