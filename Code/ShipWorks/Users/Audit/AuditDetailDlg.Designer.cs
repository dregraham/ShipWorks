namespace ShipWorks.Users.Audit
{
    partial class AuditDetailDlg
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
            ShipWorks.UI.Controls.SandGrid.WindowsXPShipWorksRenderer windowsXPShipWorksRenderer1 = new ShipWorks.UI.Controls.SandGrid.WindowsXPShipWorksRenderer();
            Divelements.SandGrid.Rendering.WindowsXPRenderer windowsXPRenderer1 = new Divelements.SandGrid.Rendering.WindowsXPRenderer();
            this.close = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.entityGrid = new ShipWorks.Data.Grid.Paging.PagedEntityGrid();
            this.sandGrid = new Divelements.SandGrid.SandGrid();
            this.gridColumnName = new Divelements.SandGrid.GridColumn();
            this.gridColumnBefore = new Divelements.SandGrid.GridColumn();
            this.gridColumnAfter = new Divelements.SandGrid.GridColumn();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // close
            // 
            this.close.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.close.Location = new System.Drawing.Point(478, 394);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(75, 23);
            this.close.TabIndex = 0;
            this.close.Text = "Close";
            this.close.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(12, 12);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.entityGrid);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.sandGrid);
            this.splitContainer.Size = new System.Drawing.Size(541, 376);
            this.splitContainer.SplitterDistance = 175;
            this.splitContainer.TabIndex = 3;
            // 
            // entityGrid
            // 
            this.entityGrid.AllowMultipleSelection = false;
            this.entityGrid.DetailViewSettings = null;
            this.entityGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.entityGrid.EnableSearching = false;
            this.entityGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.entityGrid.LiveResize = false;
            this.entityGrid.Location = new System.Drawing.Point(0, 0);
            this.entityGrid.Name = "entityGrid";
            this.entityGrid.NullRepresentation = "";
            this.entityGrid.Renderer = windowsXPShipWorksRenderer1;
            this.entityGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.Full;
            this.entityGrid.ShadeAlternateRows = true;
            this.entityGrid.Size = new System.Drawing.Size(175, 376);
            this.entityGrid.TabIndex = 1;
            this.entityGrid.WhitespaceClickBehavior = Divelements.SandGrid.WhitespaceClickBehavior.None;
            this.entityGrid.SelectionChanged += new Divelements.SandGrid.SelectionChangedEventHandler(this.OnChangeSelectedAuditChange);
            // 
            // sandGrid
            // 
            this.sandGrid.AllowMultipleSelection = false;
            this.sandGrid.ColumnClickBehavior = Divelements.SandGrid.ColumnClickBehavior.None;
            this.sandGrid.Columns.AddRange(new Divelements.SandGrid.GridColumn[] {
            this.gridColumnName,
            this.gridColumnBefore,
            this.gridColumnAfter});
            this.sandGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sandGrid.EnableSearching = false;
            this.sandGrid.GridLines = Divelements.SandGrid.GridLinesDisplayType.Both;
            this.sandGrid.Location = new System.Drawing.Point(0, 0);
            this.sandGrid.Name = "sandGrid";
            this.sandGrid.NullRepresentation = "";
            this.sandGrid.Renderer = windowsXPRenderer1;
            this.sandGrid.RowDragBehavior = Divelements.SandGrid.RowDragBehavior.InitiateDragDrop;
            this.sandGrid.RowHighlightType = Divelements.SandGrid.RowHighlightType.None;
            this.sandGrid.Rows.AddRange(new Divelements.SandGrid.GridRow[] {
            new Divelements.SandGrid.GridRow(new Divelements.SandGrid.GridCell[] {
                        new Divelements.SandGrid.GridCell("Order Number:"),
                        new Divelements.SandGrid.GridCell("10548-M"),
                        new Divelements.SandGrid.GridCell("10548-MP")})});
            this.sandGrid.SelectionGranularity = Divelements.SandGrid.SelectionGranularity.Cell;
            this.sandGrid.ShadeAlternateRows = true;
            this.sandGrid.Size = new System.Drawing.Size(362, 376);
            this.sandGrid.StretchPrimaryGrid = false;
            this.sandGrid.TabIndex = 0;
            // 
            // gridColumnName
            // 
            this.gridColumnName.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnName.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnName.CellHorizontalAlignment = System.Drawing.StringAlignment.Far;
            this.gridColumnName.Clickable = false;
            this.gridColumnName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnName.MinimumWidth = 100;
            this.gridColumnName.UseCellFont = false;
            // 
            // gridColumnBefore
            // 
            this.gridColumnBefore.AllowWrap = true;
            this.gridColumnBefore.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnBefore.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnBefore.Clickable = false;
            this.gridColumnBefore.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnBefore.HeaderText = "Before";
            this.gridColumnBefore.MinimumWidth = 100;
            // 
            // gridColumnAfter
            // 
            this.gridColumnAfter.AllowWrap = true;
            this.gridColumnAfter.AutoSize = Divelements.SandGrid.ColumnAutoSizeMode.Contents;
            this.gridColumnAfter.AutoSortType = Divelements.SandGrid.ColumnAutoSortType.None;
            this.gridColumnAfter.Clickable = false;
            this.gridColumnAfter.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.gridColumnAfter.HeaderText = "After";
            this.gridColumnAfter.MinimumWidth = 100;
            // 
            // AuditDetailDlg
            // 
            this.AcceptButton = this.close;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.close;
            this.ClientSize = new System.Drawing.Size(565, 429);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.close);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(546, 380);
            this.Name = "AuditDetailDlg";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Audit Detail";
            this.Load += new System.EventHandler(this.OnLoad);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button close;
        private ShipWorks.Data.Grid.Paging.PagedEntityGrid entityGrid;
        private System.Windows.Forms.SplitContainer splitContainer;
        private Divelements.SandGrid.SandGrid sandGrid;
        private Divelements.SandGrid.GridColumn gridColumnName;
        private Divelements.SandGrid.GridColumn gridColumnBefore;
        private Divelements.SandGrid.GridColumn gridColumnAfter;
    }
}