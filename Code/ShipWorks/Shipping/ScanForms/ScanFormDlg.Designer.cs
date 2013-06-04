namespace ShipWorks.Shipping.ScanForms
{
    partial class ScanFormDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.labelInfo1 = new System.Windows.Forms.Label();
            this.labelInfo2 = new System.Windows.Forms.Label();
            this.createScan = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnOrder = new Divelements.SandGrid.GridColumn();
            this.gridColumnRecipient = new Divelements.SandGrid.GridColumn();
            this.gridColumnDate = new Divelements.SandGrid.Specialized.GridDateTimeColumn();
            this.gridColumnService = new Divelements.SandGrid.GridColumn();
            this.gridColumnAmount = new Divelements.SandGrid.Specialized.GridDecimalColumn();
            this.gridColumnTracking = new Divelements.SandGrid.GridColumn();
            this.labelSelected = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelInfo1
            // 
            this.labelInfo1.Location = new System.Drawing.Point(12, 9);
            this.labelInfo1.Name = "labelInfo1";
            this.labelInfo1.Size = new System.Drawing.Size(628, 24);
            this.labelInfo1.TabIndex = 0;
            this.labelInfo1.Text = "The following shipments will be included for barcode manifesting.  Uncheck or rem" +
                "ove any shipments you do not want to include.";
            // 
            // labelInfo2
            // 
            this.labelInfo2.Location = new System.Drawing.Point(12, 33);
            this.labelInfo2.Name = "labelInfo2";
            this.labelInfo2.Size = new System.Drawing.Size(689, 23);
            this.labelInfo2.TabIndex = 1;
            this.labelInfo2.Text = "Shipments included on a SCAN form are not eligible for a refund of postage.  Plea" +
                "se be sure to only include items that you are mailing today.";
            // 
            // createScan
            // 
            this.createScan.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.createScan.Location = new System.Drawing.Point(553, 442);
            this.createScan.Name = "createScan";
            this.createScan.Size = new System.Drawing.Size(112, 23);
            this.createScan.TabIndex = 2;
            this.createScan.Text = "Create SCAN Form";
            this.createScan.UseVisualStyleBackColor = true;
            this.createScan.Click += new System.EventHandler(this.OnCreateScanForm);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(671, 442);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 3;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // sandGrid
            // 
            this.sandGrid.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sandGrid.CheckBoxes = true;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnOrder,
            this.gridColumnRecipient,
            this.gridColumnDate,
            this.gridColumnService,
            this.gridColumnAmount,
            this.gridColumnTracking});
            this.sandGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.HorizontalOnly;
            this.sandGrid.Location = new System.Drawing.Point(15, 59);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
            this.sandGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("154978-MAN"),
                        new Divelements.SandGrid.GridCell("Nottingham, 62711"),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDateTimeCell(new System.DateTime(2009, 8, 14, 10, 15, 0, 0)))),
                        new Divelements.SandGrid.GridCell("USPS Priority"),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        12321,
                                        0,
                                        0,
                                        131072})))),
                        new Divelements.SandGrid.GridCell("9101148008600186377389 ")}),
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("987656"),
                        new Divelements.SandGrid.GridCell("Clayton, France"),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDateTimeCell(new System.DateTime(2009, 8, 14, 11, 23, 0, 0)))),
                        new Divelements.SandGrid.GridCell("Priority Mail International"),
                        ((Divelements.SandGrid.GridCell)(new Divelements.SandGrid.Specialized.GridDecimalCell(new decimal(new int[] {
                                        46123,
                                        0,
                                        0,
                                        131072})))),
                        new Divelements.SandGrid.GridCell("LG06540546")})});
            this.sandGrid.ShadeAlternateRows = true;
            this.sandGrid.Size = new System.Drawing.Size(731, 377);
            this.sandGrid.TabIndex = 4;
            this.sandGrid.AfterCheck += new Divelements.SandGrid.GridRowCheckEventHandler(this.OnCheckChanged);
            // 
            // gridColumnOrder
            // 
            this.gridColumnOrder.AllowReorder = false;
            this.gridColumnOrder.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnOrder.HeaderText = "Order #";
            this.gridColumnOrder.Width = 96;
            // 
            // gridColumnRecipient
            // 
            this.gridColumnRecipient.AllowReorder = false;
            this.gridColumnRecipient.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnRecipient.HeaderText = "Recipient";
            this.gridColumnRecipient.Width = 135;
            // 
            // gridColumnDate
            // 
            this.gridColumnDate.AllowReorder = false;
            this.gridColumnDate.DataFormatString = "{0:MM/dd/yy h:mm tt}";
            this.gridColumnDate.EditorType = typeof(Divelements.SandGrid.GridDateTimeEditor);
            this.gridColumnDate.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnDate.HeaderText = "Processed Date";
            this.gridColumnDate.Width = 124;
            // 
            // gridColumnService
            // 
            this.gridColumnService.AllowReorder = false;
            this.gridColumnService.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnService.HeaderText = "Service";
            this.gridColumnService.Width = 140;
            // 
            // gridColumnAmount
            // 
            this.gridColumnAmount.AllowReorder = false;
            this.gridColumnAmount.DataFormatString = "{0:c}";
            this.gridColumnAmount.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnAmount.HeaderText = "Amount";
            this.gridColumnAmount.Width = 64;
            // 
            // gridColumnTracking
            // 
            this.gridColumnTracking.AllowReorder = false;
            this.gridColumnTracking.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Spring;
            this.gridColumnTracking.ForeColorSource = Divelements.SandGrid.CellForeColorSource.RowCell;
            this.gridColumnTracking.HeaderText = "Tracking Number";
            this.gridColumnTracking.Width = 168;
            // 
            // labelSelected
            // 
            this.labelSelected.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSelected.AutoSize = true;
            this.labelSelected.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.labelSelected.Location = new System.Drawing.Point(12, 447);
            this.labelSelected.Name = "labelSelected";
            this.labelSelected.Size = new System.Drawing.Size(144, 13);
            this.labelSelected.TabIndex = 5;
            this.labelSelected.Text = "156 shipments selected.";
            // 
            // EndiciaScanFormDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(758, 477);
            this.Controls.Add(this.labelSelected);
            this.Controls.Add(this.sandGrid);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.createScan);
            this.Controls.Add(this.labelInfo2);
            this.Controls.Add(this.labelInfo1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(774, 1329);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(774, 329);
            this.Name = "EndiciaScanFormDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create SCAN Form";
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelInfo1;
        private System.Windows.Forms.Label labelInfo2;
        private System.Windows.Forms.Button createScan;
        private System.Windows.Forms.Button cancel;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnTracking;
        private Divelements.SandGrid.Specialized.GridDateTimeColumn gridColumnDate;
        private Divelements.SandGrid.GridColumn gridColumnOrder;
        private Divelements.SandGrid.GridColumn gridColumnRecipient;
        private Divelements.SandGrid.GridColumn gridColumnService;
        private Divelements.SandGrid.Specialized.GridDecimalColumn gridColumnAmount;
        private System.Windows.Forms.Label labelSelected;
    }
}