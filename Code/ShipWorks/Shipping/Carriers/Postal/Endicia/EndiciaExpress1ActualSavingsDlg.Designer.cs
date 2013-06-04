namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    partial class EndiciaExpress1ActualSavingsDlg
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
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.close = new System.Windows.Forms.Button();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnService = new Divelements.SandGrid.GridColumn();
            this.gridColumnOriginal = new Divelements.SandGrid.GridColumn();
            this.gridColumnDiscounted = new Divelements.SandGrid.GridColumn();
            this.gridColumnSavings = new Divelements.SandGrid.GridColumn();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.close.Location = new System.Drawing.Point(369, 156);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnService,
            this.gridColumnOriginal,
            this.gridColumnDiscounted,
            this.gridColumnSavings});
            this.sandGrid.EmptyTextForeColor = System.Drawing.Color.DimGray;
            this.sandGrid.ImageTextSeparation = 1;
            this.sandGrid.Location = new System.Drawing.Point(12, 12);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.None;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
            this.sandGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("First Class"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        445,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("     Delivery Confirmation (+$.045)"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        490,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("     Signature Confirmation (+$0.85)"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        530,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Priority"),
                        new Divelements.SandGrid.GridCell(),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        985,
                                        0,
                                        0,
                                        131072})))),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridHyperlinkCell("Select")))})});
            this.sandGrid.ShadeAlternateRows = true;
            this.sandGrid.Size = new System.Drawing.Size(430, 135);
            this.sandGrid.StretchPrimaryGrid = false;
            this.sandGrid.TabIndex = 2;
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
            // gridColumnOriginal
            // 
            this.gridColumnOriginal.AllowEditing = false;
            this.gridColumnOriginal.AllowReorder = false;
            this.gridColumnOriginal.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnOriginal.AutoSizeIncludeHeader = true;
            this.gridColumnOriginal.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnOriginal.Clickable = false;
            this.gridColumnOriginal.HeaderText = "Original";
            this.gridColumnOriginal.Width = 32;
            // 
            // gridColumnDiscounted
            // 
            this.gridColumnDiscounted.AllowEditing = false;
            this.gridColumnDiscounted.AllowReorder = false;
            this.gridColumnDiscounted.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnDiscounted.AutoSizeIncludeHeader = true;
            this.gridColumnDiscounted.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnDiscounted.Clickable = false;
            this.gridColumnDiscounted.HeaderText = "Discounted";
            this.gridColumnDiscounted.Width = 31;
            // 
            // gridColumnSavings
            // 
            this.gridColumnSavings.AllowEditing = false;
            this.gridColumnSavings.AllowReorder = false;
            this.gridColumnSavings.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnSavings.AutoSizeIncludeHeader = true;
            this.gridColumnSavings.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnSavings.Clickable = false;
            this.gridColumnSavings.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnSavings.ForeColor = System.Drawing.Color.Green;
            this.gridColumnSavings.HeaderText = "Savings";
            this.gridColumnSavings.UseCellFont = false;
            // 
            // EndiciaExpress1ActualSavingsDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(456, 191);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EndiciaExpress1ActualSavingsDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Postage Savings";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnService;
        private Divelements.SandGrid.GridColumn gridColumnOriginal;
        private Divelements.SandGrid.GridColumn gridColumnDiscounted;
        private Divelements.SandGrid.GridColumn gridColumnSavings;
    }
}