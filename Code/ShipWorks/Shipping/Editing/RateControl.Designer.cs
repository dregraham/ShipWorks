using System.Drawing;
using System.Windows.Forms;
using ShipWorks.Properties;

namespace ShipWorks.Shipping.Editing
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
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnProvider = new Divelements.SandGrid.Specialized.GridImageColumn();
            this.gridColumnService = new Divelements.SandGrid.GridColumn();
            this.gridColumnDays = new Divelements.SandGrid.GridColumn();
            this.gridColumnRate = new Divelements.SandGrid.GridColumn();
            this.gridColumnSelect = new Divelements.SandGrid.Specialized.GridHyperlinkColumn();
            this.outOfDateBar = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.labelSecondary = new System.Windows.Forms.Label();
            this.labelPrimary = new System.Windows.Forms.Label();
            this.image = new System.Windows.Forms.PictureBox();
            this.panelOutOfDate = new System.Windows.Forms.Panel();
            this.panelFootnote = new System.Windows.Forms.Panel();
            this.kryptonBorderEdge = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.loadingImage = new System.Windows.Forms.PictureBox();
            this.loadingRatesPanel = new System.Windows.Forms.Panel();
            this.loadingRatesLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.outOfDateBar)).BeginInit();
            this.outOfDateBar.Panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.image)).BeginInit();
            this.panelOutOfDate.SuspendLayout();
            this.panelFootnote.SuspendLayout();
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
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
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
            this.gridColumnProvider.Visible = false;
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
            this.gridColumnSelect.HeaderText = "Select";
            this.gridColumnSelect.Width = 37;
            // 
            // outOfDateBar
            // 
            this.outOfDateBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.outOfDateBar.GroupBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridHeaderRowList;
            this.outOfDateBar.GroupBorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.ControlRibbon;
            this.outOfDateBar.Location = new System.Drawing.Point(4, 3);
            this.outOfDateBar.Name = "outOfDateBar";
            // 
            // outOfDateBar.Panel
            // 
            this.outOfDateBar.Panel.Controls.Add(this.labelSecondary);
            this.outOfDateBar.Panel.Controls.Add(this.labelPrimary);
            this.outOfDateBar.Panel.Controls.Add(this.image);
            this.outOfDateBar.Size = new System.Drawing.Size(485, 24);
            this.outOfDateBar.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(235)))));
            this.outOfDateBar.StateCommon.Border.Color1 = System.Drawing.Color.Silver;
            this.outOfDateBar.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.outOfDateBar.StateCommon.Border.Rounding = 4;
            this.outOfDateBar.TabIndex = 6;

            // 
            // labelSecondary
            // 
            this.labelSecondary.AutoSize = true;
            this.labelSecondary.BackColor = System.Drawing.Color.Transparent;
            this.labelSecondary.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.labelSecondary.Location = new System.Drawing.Point(85, 2);
            this.labelSecondary.Name = "labelSecondary";
            this.labelSecondary.Size = new System.Drawing.Size(231, 13);
            this.labelSecondary.TabIndex = 2;
            this.labelSecondary.Text = "The shipment has changed since getting rates.";
            // 
            // labelPrimary
            // 
            this.labelPrimary.AutoSize = true;
            this.labelPrimary.BackColor = System.Drawing.Color.Transparent;
            this.labelPrimary.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPrimary.Location = new System.Drawing.Point(16, 2);
            this.labelPrimary.Name = "labelPrimary";
            this.labelPrimary.Size = new System.Drawing.Size(71, 13);
            this.labelPrimary.TabIndex = 1;
            this.labelPrimary.Text = "Out of Date";
            // 
            // image
            // 
            this.image.BackColor = System.Drawing.Color.Transparent;
            this.image.Image = global::ShipWorks.Properties.Resources.warning16;
            this.image.Location = new System.Drawing.Point(1, 1);
            this.image.Name = "image";
            this.image.Size = new System.Drawing.Size(16, 16);
            this.image.TabIndex = 0;
            this.image.TabStop = false;
            // 
            // panelOutOfDate
            // 
            this.panelOutOfDate.Controls.Add(this.outOfDateBar);
            this.panelOutOfDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOutOfDate.Location = new System.Drawing.Point(0, 0);
            this.panelOutOfDate.Name = "panelOutOfDate";
            this.panelOutOfDate.Size = new System.Drawing.Size(493, 30);
            this.panelOutOfDate.TabIndex = 7;
            this.panelOutOfDate.Visible = false;
            // 
            // panelFootnote
            // 
            this.panelFootnote.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelFootnote.Controls.Add(this.kryptonBorderEdge);
            this.panelFootnote.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFootnote.Location = new System.Drawing.Point(0, 175);
            this.panelFootnote.Name = "panelFootnote";
            this.panelFootnote.Size = new System.Drawing.Size(493, 30);
            this.panelFootnote.TabIndex = 8;
            // 
            // kryptonBorderEdge
            // 
            this.kryptonBorderEdge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonBorderEdge.AutoSize = false;
            this.kryptonBorderEdge.BorderStyle = ComponentFactory.Krypton.Toolkit.PaletteBorderStyle.GridDataCellSheet;
            this.kryptonBorderEdge.Location = new System.Drawing.Point(0, 0);
            this.kryptonBorderEdge.Name = "kryptonBorderEdge";
            this.kryptonBorderEdge.Size = new System.Drawing.Size(493, 1);
            this.kryptonBorderEdge.Text = "kryptonBorderEdge1";
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
            this.loadingRatesPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
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
            this.Controls.Add(this.panelOutOfDate);
            this.Controls.Add(this.panelFootnote);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RateControl";
            this.Size = new System.Drawing.Size(493, 205);
            this.outOfDateBar.Panel.ResumeLayout(false);
            this.outOfDateBar.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.outOfDateBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.image)).EndInit();
            this.panelOutOfDate.ResumeLayout(false);
            this.panelFootnote.ResumeLayout(false);
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
        private ComponentFactory.Krypton.Toolkit.KryptonGroup outOfDateBar;
        private System.Windows.Forms.Label labelSecondary;
        private System.Windows.Forms.Label labelPrimary;
        private System.Windows.Forms.PictureBox image;
        private System.Windows.Forms.Panel panelOutOfDate;
        private System.Windows.Forms.Panel panelFootnote;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge;
        private Divelements.SandGrid.Specialized.GridImageColumn gridColumnProvider;
        private PictureBox loadingImage;
        private Panel loadingRatesPanel;
        private Label loadingRatesLabel;
    }
}
